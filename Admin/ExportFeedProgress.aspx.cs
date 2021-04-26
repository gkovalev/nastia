//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.IO;
using System.Threading;
using System.Web.UI;
using AdvantShop.Configuration;
using AdvantShop.ExportImport;
using AdvantShop.SaasData;
using AdvantShop.Statistic;
using Resources;

public partial class Admin_ExportFeedProgress : Page
{
    public static string PhysicalAppPath { get; set; }

    public string ModuleName
    {
        get { return Request["ModuleId"] ?? ""; }
    }

    public bool Start
    {
        get { return (Request["start"] ?? string.Empty) == "yes"; }
    }

    protected override void InitializeCulture()
    {
        AdvantShop.Localization.Culture.InitializeCulture();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!(ExportFeedStatistic.IsRun))
        {
            ExportFeedStatistic.Init();
        }
        if (!ExportFeedStatistic.IsRun && string.IsNullOrEmpty(ModuleName))
        {
            Response.Redirect("ExportFeed.aspx?ModuleId=YandexMarket");
            return;
        }

        PageSubheader.Visible = true;
        ModuleNameLiteral.Text = ModuleName;

        OutDiv.Visible = Start;
        PhysicalAppPath = Request.PhysicalApplicationPath;

        if ((SaasDataService.IsSaasEnabled) && (!SaasDataService.CurrentSaasData.HaveExportFeeds))
        {
            mainDiv.Visible = false;
            notInTariff.Visible = true;
        }

        Page.Title = string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_ExportFeed_Yandex_aspx);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (Start)
            ExportSelection(ModuleName);
    }

    public static void ExportSelection(string moduleName)
    {
        if (!(ExportFeedStatistic.IsRun))
        {
            ExportFeedStatistic.Init();
            ExportFeedStatistic.IsRun = true;
            var thread = new Thread(MakeExportFile);
            ExportFeedStatistic.ThreadImport = thread;
            thread.Start(new[] { moduleName, PhysicalAppPath });
        }
    }

    private static void MakeExportFile(object parameters)
    {
        var myParams = parameters as string[];

        try
        {
            var moduleName = myParams[0];
            var applicationPath = myParams[1];

            var fileName = ExportFeed.GetModuleSetting(moduleName, "FileName");
            var directory = applicationPath + "\\";
            var filePath = directory + fileName;

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            ExportFeedModule exportFeedModule = null;
            switch (moduleName)
            {
                case "YandexMarket":
                    exportFeedModule = new ExportFeedModuleYandex();
                    break;
                case "GoogleBase":
                    exportFeedModule = new ExportFeedModuleGoogleBase();
                    break;
                case "PriceGrabber":
                    exportFeedModule = new ExportFeedModulePriceGrabber();
                    break;
                case "ShoppingCom":
                    exportFeedModule = new ExportFeedModuleShoppingCom();
                    break;
                case "YahooShopping":
                    exportFeedModule = new ExportFeedModuleYahooShopping();
                    break;
                case "Amazon":
                    exportFeedModule = new ExportFeedModuleAmazon();
                    break;
                case "Shopzilla":
                    exportFeedModule = new ExportFeedModuleShopzilla();
                    break;
            }

            if (exportFeedModule != null)
            {
                exportFeedModule.GetExportFeedString(filePath);
            }

            var fileInfo = new FileInfo(filePath);

            ExportFeedStatistic.FileName = SettingsMain.SiteUrl + "/" + fileInfo.Name;
            ExportFeedStatistic.FileSize = " (" + Math.Ceiling(Convert.ToDecimal(fileInfo.Length) / 1024) + " Kb)";
        }
        catch (Exception ex)
        {
            AdvantShop.Diagnostics.Debug.LogError(ex, "on MakeExportFile in exportFeed");
        }
        finally
        {
            ExportFeedStatistic.IsRun = false;
        }
    }
}