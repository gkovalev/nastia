using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop;
using AdvantShop.Configuration;
using AdvantShop.Helpers;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Diagnostics;
using Resources;

public partial class Admin_RootFiles :Page
{
    private int _currentPageIndex;
    private int _itemsPerPage;
    private int _totalRowCount;

    protected override void InitializeCulture()
    {
        AdvantShop.Localization.Culture.InitializeCulture();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        lblError.Text = string.Empty;
        lblError.Visible = false;

        Page.Title = string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_RootFiles_Header);

        if (!IsPostBack)
        {
            pageNumberer.CurrentPageIndex = 1;
            _currentPageIndex = 1;
            _itemsPerPage = 10;
        }
        else
        {
            _itemsPerPage = Convert.ToInt32(ddRowsPerPage.SelectedValue);
            _currentPageIndex = pageNumberer.CurrentPageIndex;
        }
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        _currentPageIndex = 1;
        pageNumberer.CurrentPageIndex = 1;
    }

    protected void pn_SelectedPageChanged(object sender, EventArgs e)
    {
        _currentPageIndex = pageNumberer.CurrentPageIndex;
    }

    protected void linkGO_Click(object sender, EventArgs e)
    {
        int pagen;
        try
        {
            pagen = int.Parse(txtPageNum.Text);
        }
        catch (Exception)
        {
            pagen = -1;
        }
        if (pagen >= 1 && pagen <= pageNumberer.PageCount)
        {
            _currentPageIndex = pagen;
            pageNumberer.CurrentPageIndex = pagen;
        }
    }

    protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "DeleteFile")
        {
            FileHelpers.DeleteFile(SettingsGeneral.AbsolutePath + e.CommandArgument);
            upCounts.Update();
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        _totalRowCount = TotalFilesCount();
        var data = FilesTable();
        if (data.Rows.Count < 1 && _currentPageIndex > 1)
        {
            _currentPageIndex--;
            data = FilesTable();
        }

        if (data.Rows.Count < 1)
        {
            goToPage.Visible = false;
        }

        grid.DataSource = data;
        grid.DataBind();

        pageNumberer.CurrentPageIndex = _currentPageIndex;
        pageNumberer.PageCount = (int)(Math.Ceiling((double)_totalRowCount / _itemsPerPage));
        lblFound.Text = _totalRowCount.ToString();
    }

    protected void grid_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
    }

    private static int TotalFilesCount()
    {
        return GetFiles().Count;
    }

    private DataTable FilesTable()
    {
        var tbl = new DataTable();
        var col = new DataColumn("FileName", typeof(string)) { Caption = "FileName" };
        tbl.Columns.Add(col);

        var fileNames = GetFiles();

        // for paging
        var start = _totalRowCount > _itemsPerPage 
            ? _itemsPerPage * _currentPageIndex - _itemsPerPage : 0;
        var end = (_itemsPerPage * _currentPageIndex) > _totalRowCount 
            ? _totalRowCount : _itemsPerPage * _currentPageIndex;
        for (int i = start; i < end; i++)
        {
            var row = tbl.NewRow();
            row["FileName"] = @"Optovik\" + fileNames[i];
            tbl.Rows.Add(row);
        }

        return tbl;
    }

    private static List<string> GetFiles()
    {
        var filesWithPath = Directory.GetFiles(SettingsGeneral.AbsolutePath + @"Optovik\").Where(t => !t.Contains(".cs") && !t.Contains(".aspx") && !t.Contains(".ascx"));
        return filesWithPath.Where(file => !string.IsNullOrEmpty(file) && file.Contains("\\")).Select(
            file => file.Substring(file.LastIndexOf("\\", StringComparison.Ordinal)).Replace("\\", "")).ToList();
        
       
    }

    protected void bthAddFile_Click(object sender, EventArgs e)
    {
        lblErrorFile.Text = string.Empty;

        if (!FileLoad.HasFile)
        {
            return;
        }

        try
        {
            using (var file = FileLoad.PostedFile.InputStream)
            {
                FileHelpers.SaveFile(SettingsGeneral.AbsolutePath + @"Optovik\" + FileLoad.FileName, file);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex, "at loading file in root directory");
        }

        Response.Redirect(UrlService.GetAdminAbsoluteLink("OptovikFiles.aspx"));
    }

    protected string RenderFileLink(string fileName)
    {
        return string.Format("~/HttpHandlers/DownloadFile.ashx?file={0}", fileName);
    }
}