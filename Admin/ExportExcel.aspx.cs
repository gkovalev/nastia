//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.IO;
using System.Threading;
using System.Web.UI;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Diagnostics;
using AdvantShop.ExportImport.Excel;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Statistic;
using Resources;
using AdvantShop.SaasData;

public partial class Admin_ExportExcel : Page
{
    private readonly string _strFilePath;
    private readonly string _strFullPath;
    public string NotDoPost = string.Empty;
    public string Link = string.Empty;
    private const string StrFileName = "products.xls";

    public Admin_ExportExcel()
    {
        _strFilePath = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp);
        _strFullPath = string.Format("{0}{1}", _strFilePath, StrFileName);
    }

    protected override void InitializeCulture()
    {
        AdvantShop.Localization.Culture.InitializeCulture();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if ((SaasDataService.IsSaasEnabled) && (!SaasDataService.CurrentSaasData.HaveExcel))
        {
            mainDiv.Visible = false;
            notInTariff.Visible = true;
        }

        Page.Title = string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_ExportExcel_Title);
        if (IsPostBack) return;
        OutDiv.Visible = ExportStatistic.IsRun;
        linkCancel.Visible = ExportStatistic.IsRun;
    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        if (ExportStatistic.IsRun == false)
        {
            ExportStatistic.Init();
            ExportStatistic.IsRun = true;
            linkCancel.Visible = true;
            OutDiv.Visible = true;
            btnDownload.Visible = false;
            try
            {
                // Directory
                FileHelpers.CreateDirectory(_strFilePath);
                ExportStatistic.TotalRow = ProductService.GetProductCountByOffer(6);

                var tr = new Thread(Save);
                ExportStatistic.ThreadImport = tr;
                tr.Start();
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                lError.Text = ex.Message;
                lError.Visible = true;
            }
        }
    }

    protected void Save()
    {
        //var wrt = new ExcelXmlWriter();
        ExcelXmlWriter.SaveProductsToXml(_strFullPath);
        ExportStatistic.IsRun = false;
        //ExportStatistic.ThreadImport.Abort();
    }

    protected void linkCancel_Click(object sender, EventArgs e)
    {
        if (!ExportStatistic.ThreadImport.IsAlive) return;
        ExportStatistic.IsRun = false;
        //ExportStatistic.ThreadImport.Abort();
        ExportStatistic.Init();
    }

    protected void btnAsyncLoad_Click(object sender, EventArgs e)
    {
        linkCancel.Visible = false;
        NotDoPost = "true";
        try
        {
            using (FileStream f = File.Open(_strFullPath, FileMode.Open))
            {
                f.Close();
            }
        }
        catch (Exception ex)
        {
            //Debug.LogError(ex, sender, e);
            Debug.LogError(ex);
            lError.Text = ex.Message;
            lError.Visible = true;
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (File.Exists(_strFullPath))
        {
            var f = new FileInfo(_strFullPath);
            const double size = 0;
            double sizeM = (double)f.Length / 1048576; //1024 * 1024

            string sizeMesage;
            if ((int)sizeM > 0)
            {
                sizeMesage = ((int)sizeM) + " MB";
            }
            else
            {
                double sizeK = (double)f.Length / 1024;
                if ((int)sizeK > 0)
                {
                    sizeMesage = ((int)sizeK) + " KB";
                }
                else
                {
                    sizeMesage = ((int)size) + " B";
                }
            }

            Link = "<a href=\'" + UrlService.GetAbsoluteLink("price_temp/" + StrFileName) + "\'   >" + Resource.Admin_ExportExcel_DownloadFile +
                   "</a><span> " + Resource.Admin_ExportExcel_FileSize + ": " + sizeMesage + "</span>";
            Link += "<span>" + ", " + AdvantShop.Localization.Culture.ConvertDate(File.GetLastWriteTime(_strFullPath)) + "</span>";
        }
        else
        {
            Link = "<span>" + Resource.Admin_ExportExcel_NotExistDownloadFile + "</span>";
        }
    }
}