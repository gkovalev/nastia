using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using AdvantShop;
using AdvantShop.Configuration;
using AdvantShop.Core;
using AdvantShop.Core.FieldFilters;
using AdvantShop.FilePath;
using AdvantShop.News;
using Resources;

public partial class Admin_NewsAdmin2 : System.Web.UI.Page
{
    private bool _inverseSelection;
    private SqlPaging _paging;
    private InSetFieldFilter _selectionFilter;

    protected override void InitializeCulture()
    {
        AdvantShop.Localization.Culture.InitializeCulture();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_NewsAdmin_Header);

        if (!IsPostBack)
        {
            _paging = new SqlPaging
            {
                TableName = "[Settings].[News]",
                ItemsPerPage = 20
            };

            _paging.AddFieldsRange(new List<Field>
                                 {
                                     new Field
                                         {
                                             Name = "NewsID as ID",
                                             IsDistinct = true
                                         },
                                     new Field{Name = "Title"},
                                     new Field {Name = "ShowOnMainPage"},
                                     new Field {Name = "NewsCategoryID"},
                                     new Field {Name = "AddingDate", Sorting = SortDirection.Descending}
                                 });

            grid.ChangeHeaderImageUrl("arrowAddingDate", "images/arrowdown.gif");

            pageNumberer.CurrentPageIndex = 1;
            _paging.CurrentPageIndex = 1;
            ViewState["Paging"] = _paging;
        }
        else
        {
            _paging = (SqlPaging)(ViewState["Paging"]);
            _paging.ItemsPerPage = Convert.ToInt32(ddRowsPerPage.SelectedValue);

            if (_paging == null)
            {
                throw (new Exception("Paging lost"));
            }

            string strIds = Request.Form["SelectedIds"];

            if (!string.IsNullOrEmpty(strIds))
            {
                strIds = strIds.Trim();
                string[] arrids = strIds.Split(' ');

                _selectionFilter = new InSetFieldFilter();
                if (arrids.Contains("-1"))
                {
                    _selectionFilter.IncludeValues = false;
                    _inverseSelection = true;
                }
                else
                {
                    _selectionFilter.IncludeValues = true;
                }
                _selectionFilter.Values = arrids.Where(id => id != "-1").ToArray();
            }
        }
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        //-----Selection filter
        if (ddSelect.SelectedIndex != 0)
        {
            if (ddSelect.SelectedIndex == 2)
            {
                if (_selectionFilter != null)
                {
                    _selectionFilter.IncludeValues = !_selectionFilter.IncludeValues;
                }
            }
            _paging.Fields["ID"].Filter = _selectionFilter;
        }
        else
        {
            _paging.Fields["ID"].Filter = null;
        }

        //----Name filter
        _paging.Fields["Title"].Filter = !string.IsNullOrEmpty(txtTitle.Text) ? new CompareFieldFilter { Expression = txtTitle.Text, ParamName = "@Title" } : null;

        _paging.Fields["NewsCategoryID"].Filter = ddlNewsCategoryID.SelectedValue != "0"
                                                        ? new EqualFieldFilter
                                                        {
                                                            ParamName = "@ShippingModuleID",
                                                            Value = ddlNewsCategoryID.SelectedValue
                                                        }
                                                        : null;
        _paging.Fields["ShowOnMainPage"].Filter = (ddlShowOnMainPage.SelectedValue != "any")
            ? new EqualFieldFilter { ParamName = "@Enabled", Value = ddlShowOnMainPage.SelectedValue } : null;
        pageNumberer.CurrentPageIndex = 1;
        _paging.CurrentPageIndex = 1;
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        btnFilter_Click(sender, e);
        grid.ChangeHeaderImageUrl(null, null);
    }

    protected void pn_SelectedPageChanged(object sender, EventArgs e)
    {
        _paging.CurrentPageIndex = pageNumberer.CurrentPageIndex;
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
        if (pagen >= 1 && pagen <= _paging.PageCount)
        {
            pageNumberer.CurrentPageIndex = pagen;
            _paging.CurrentPageIndex = pagen;
        }
    }

    protected void lbDeleteSelected_Click(object sender, EventArgs e)
    {
        if ((_selectionFilter != null) && (_selectionFilter.Values != null))
        {
            if (!_inverseSelection)
            {
                foreach (var id in _selectionFilter.Values)
                {
                    NewsService.DeleteNews(Convert.ToInt32(id));
                }
            }
            else
            {
                var itemsIds = _paging.ItemsIds<int>("NewsID as ID");
                foreach (var newsId in itemsIds.Where(nId => !_selectionFilter.Values.Contains(nId.ToString(CultureInfo.InvariantCulture))))
                {
                    NewsService.DeleteNews(newsId);
                }
            }
        }
    }

    protected void lbSetInMainPage_Click(object sender, EventArgs e)
    {
        if ((_selectionFilter != null) && (_selectionFilter.Values != null))
        {
            if (!_inverseSelection)
            {
                foreach (var id in _selectionFilter.Values)
                {
                    NewsService.SetNewsOnMainPage(Convert.ToInt32(id), true);
                }
            }
            else
            {
                var itemsIds = _paging.ItemsIds<int>("NewsID as ID");
                foreach (var newsId in itemsIds.Where(nId => !_selectionFilter.Values.Contains(nId.ToString(CultureInfo.InvariantCulture))))
                {
                    NewsService.SetNewsOnMainPage(newsId, true);
                }
            }
        }
    }

    protected void lbSetOutMainPage_Click(object sender, EventArgs e)
    {
        if ((_selectionFilter != null) && (_selectionFilter.Values != null))
        {
            if (!_inverseSelection)
            {
                foreach (var id in _selectionFilter.Values)
                {
                    NewsService.SetNewsOnMainPage(Convert.ToInt32(id), false);
                }
            }
            else
            {
                var itemsIds = _paging.ItemsIds<int>("NewsID as ID");
                foreach (var newsId in itemsIds.Where(nId => !_selectionFilter.Values.Contains(nId.ToString(CultureInfo.InvariantCulture))))
                {
                    NewsService.SetNewsOnMainPage(newsId, false);
                }
            }
        }
    }
    protected void lbChangeCategoryNews_Click(object sender, EventArgs e)
    {
        if ((_selectionFilter != null) && (_selectionFilter.Values != null))
        {
            if (!_inverseSelection)
            {
                foreach (var id in _selectionFilter.Values)
                {
                    NewsService.ChangeCategoryNews(Convert.ToInt32(id), Convert.ToInt32(ddlChangeCategoryNews.SelectedValue));
                }
            }
            else
            {
                var itemsIds = _paging.ItemsIds<int>("NewsID as ID");
                foreach (var id in itemsIds.Where(nId => !_selectionFilter.Values.Contains(nId.ToString(CultureInfo.InvariantCulture))))
                {
                    NewsService.ChangeCategoryNews(Convert.ToInt32(id), Convert.ToInt32(ddlChangeCategoryNews.SelectedValue));
                }
            }
        }
    }


    protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "DeleteNews")
        {
            NewsService.DeleteNews(Convert.ToInt32(e.CommandArgument));
        }
    }

    protected void grid_Sorting(object sender, GridViewSortEventArgs e)
    {
        var arrows = new Dictionary<string, string>
                         {
                             {"Title", "arrowTitle"},
                             {"NewsCategoryID", "arrowNewsCategoryID"},
                             {"ShowOnMainPage", "arrowShowOnMainPage"},
                             {"AddingDate", "arrowAddingDate"}
                         };

        const string urlArrowUp = "images/arrowup.gif";
        const string urlArrowDown = "images/arrowdown.gif";
        const string urlArrowGray = "images/arrowdownh.gif";


        Field csf = (from Field f in _paging.Fields.Values where f.Sorting.HasValue select f).First();
        Field nsf = _paging.Fields[e.SortExpression];

        if (nsf.Name.Equals(csf.Name))
        {
            csf.Sorting = csf.Sorting == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
            grid.ChangeHeaderImageUrl(arrows[csf.Name],
                                      (csf.Sorting == SortDirection.Ascending ? urlArrowUp : urlArrowDown));
        }
        else
        {
            csf.Sorting = null;
            grid.ChangeHeaderImageUrl(arrows[csf.Name], urlArrowGray);

            nsf.Sorting = SortDirection.Ascending;
            grid.ChangeHeaderImageUrl(arrows[nsf.Name], urlArrowUp);
        }


        pageNumberer.CurrentPageIndex = 1;
        _paging.CurrentPageIndex = 1;
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (grid.UpdatedRow != null)
        {
            NewsItem news = NewsService.GetNewsById(Convert.ToInt32(grid.UpdatedRow["ID"]));
            news.Title = grid.UpdatedRow["Title"];
            news.NewsCategoryID = Convert.ToInt32(grid.UpdatedRow["NewsCategoryID"]);
            news.ShowOnMainPage = Convert.ToBoolean(grid.UpdatedRow["ShowOnMainPage"]);
            NewsService.UpdateNews(news);
        }

        DataTable data = _paging.PageItems;
        while (data.Rows.Count < 1 && _paging.CurrentPageIndex > 1)
        {
            _paging.CurrentPageIndex--;
            data = _paging.PageItems;
        }

        var clmn = new DataColumn("IsSelected", typeof(bool)) { DefaultValue = _inverseSelection };
        data.Columns.Add(clmn);
        if ((_selectionFilter != null) && (_selectionFilter.Values != null))
        {
            for (int i = 0; i <= data.Rows.Count - 1; i++)
            {
                int intIndex = i;
                if (Array.Exists(_selectionFilter.Values, c => c == (data.Rows[intIndex]["ID"]).ToString()))
                {
                    data.Rows[i]["IsSelected"] = !_inverseSelection;
                }
            }
        }

        if (data.Rows.Count < 1)
        {
            goToPage.Visible = false;
        }

        grid.DataSource = data;
        grid.DataBind();

        pageNumberer.PageCount = _paging.PageCount;
        lblFound.Text = _paging.TotalRowsCount.ToString();
    }

    protected void sds_Init(object sender, EventArgs e)
    {
        ((SqlDataSource)sender).ConnectionString = Connection.GetConnectionString();
    }

    protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
            ((DropDownList)e.Row.FindControl("ddlNewsCategoryID")).SelectedValue =
                ((DataRowView)e.Row.DataItem)["NewsCategoryID"].ToString();
    }

    protected void ddlNewsCategoryID_DataBound(object sender, EventArgs e)
    {
        ddlNewsCategoryID.Items.Insert(0, new ListItem(Resource.Admin_Catalog_Any, "0"));
    }

    protected string GetImageItem(int id)
    {
        var abbr = "";
        var newsPic = NewsService.GetNewsById(id).Picture;
        
        if(newsPic != null && File.Exists(FoldersHelper.GetPathAbsolut(FolderType.News, newsPic.PhotoName)))
        {
            abbr = FoldersHelper.GetPath(FolderType.News, newsPic.PhotoName, true);
        }
        return abbr == "" ? "" : string.Format("<img abbr='{0}' class='imgtooltip' src='images/adv_photo_ico.gif'>", abbr);
    }
}