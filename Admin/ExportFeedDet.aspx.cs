//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop.Configuration;
using AdvantShop.ExportImport;
using AdvantShop.Repository.Currencies;
using AdvantShop.SaasData;
using AdvantShop.Statistic;
using Resources;

public partial class Admin_ExportFeedDet : Page
{
    public static string PhysicalAppPath { get; set; }

    public string ModuleName
    {
        get { return Request["moduleid"] ?? ""; }
    }


    protected override void InitializeCulture()
    {
        AdvantShop.Localization.Culture.InitializeCulture();
    }

    protected override void OnPreRender(EventArgs e)
    {
    }

    protected override void OnLoad(EventArgs e)
    {
        Page.Title = string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_ExportFeed_Yandex_aspx);
        if ((SaasDataService.IsSaasEnabled) && (!SaasDataService.CurrentSaasData.HaveExportFeeds))
        {
            mainDiv.Visible = false;
            notInTariff.Visible = true;
        }

        if (!(ExportFeedStatistic.IsRun))
        {
            ExportFeedStatistic.Init();
        }

        if (string.IsNullOrEmpty(ModuleName))
        {
            Response.Redirect("ExportFeedDet.aspx?ModuleId=YandexMarket");
            return;
        }

        LoadSettings();

        PhysicalAppPath = Request.PhysicalApplicationPath;
    }



    private void LoadModuleSettings()
    {
        var moduleName = ModuleName;
        DatafeedTitleTextBox.Text = ExportFeed.GetModuleSetting(moduleName, "DatafeedTitle");
        DatafeedDescriptionTextBox.Text = ExportFeed.GetModuleSetting(moduleName, "DatafeedDescription");
        var fileName = ExportFeed.GetModuleSetting(moduleName, "FileName");
        if (fileName != null)
        {
            int dotIndex = fileName.LastIndexOf(".");
            FileNameTextBox.Text = dotIndex != -1 ? fileName.Substring(0, fileName.LastIndexOf(".")) : fileName;
        }

        string selectCurrency = ExportFeed.GetModuleSetting(moduleName, "Currency");
        if (selectCurrency != null)
        {
            CurrencyListBox.SelectedValue = ExportFeed.GetModuleSetting(moduleName, "Currency");
        }

        SalesNotesTextBox.Text = ExportFeed.GetModuleSetting(moduleName, "SalesNotes");
        companyName1.Text = SettingsMain.ShopName;
        companyName2.Text = SettingsMain.ShopName;

        switch (moduleName)
        {
            case "YandexMarket":
                recomendationLiteral.InnerHtml = Resource.Admin_ExportFeed_YandexRecomendation;
                SalesNotes.Visible = true;
                TrYandexCompany.Visible = true;
                TrYandexShop.Visible = true;

                var shopName = ExportFeed.GetModuleSetting(moduleName, "ShopName");
                var companyName = ExportFeed.GetModuleSetting(moduleName, "CompanyName");

                txtShopName.Text = string.IsNullOrEmpty(shopName) ? "#STORE_NAME#" : shopName;
                txtCompanyName.Text = string.IsNullOrEmpty(companyName) ? "#STORE_NAME#" : companyName;

                break;

            case "GoogleBase":
                datafeedDescRow.Visible = true;
                datafeedNameRow.Visible = true;
                break;

            case "PriceGrabber":
            case "ShoppingCom":
            case "YahooShopping":
            case "Amazon":
            case "Shopzilla":
                currencyWarning.Visible = true;
                break;
        }

        recomendationLiteral.InnerHtml = Resource.Admin_ExportFeed_GoogleRecomendation;
        MainCurrencyLiteral.Text = Resource.Admin_ExportFeed_UsdCurrencyString;


        FileNameExtLiteral.Text = GetFileExtention(moduleName);

        lShopUrl.Text = string.Format("{0}/", SettingsMain.SiteUrl);
    }

    private void LoadSettings()
    {
        PageSubheader.Visible = true;
        ModuleNameLiteral.Text = ModuleName;
        if (IsPostBack)
            return;
        CurrencyListBox.Items.Clear();
        foreach (var item in CurrencyService.GetAllCurrencies())
        {
            CurrencyListBox.Items.Add(new ListItem { Text = item.Name, Value = item.Iso3 });
        }
        LoadModuleSettings();
    }

    private static string GetFileExtention(string moduleName)
    {
        switch (moduleName)
        {
            case "YandexMarket":
                return @"xml";

            case "GoogleBase":
                return @"xml";

            case "PriceGrabber":
                return @"csv";

            case "ShoppingCom":
                return @"csv";

            case "YahooShopping":
                return @"txt";

            case "Amazon":
                return @"txt";

            case "Shopzilla":
                return @"txt";
        }
        return "";
    }
    protected void Unnamed12_Click(object sender, EventArgs e)
    {
        ExportFeed.RefreshModuleSetting(ModuleName, "FileName", FileNameTextBox.Text + "." + GetFileExtention(ModuleName));
        ExportFeed.RefreshModuleSetting(ModuleName, "SalesNotes", SalesNotesTextBox.Text);
        ExportFeed.RefreshModuleSetting(ModuleName, "Currency", CurrencyListBox.SelectedValue);
        ExportFeed.RefreshModuleSetting(ModuleName, "DescriptionSelection", DescriptionSelectListBox.SelectedValue);

        if (ModuleName == "YandexMarket")
        {
            ExportFeed.RefreshModuleSetting(ModuleName, "ShopName", txtShopName.Text);
            ExportFeed.RefreshModuleSetting(ModuleName, "CompanyName", txtCompanyName.Text);
        }

        if (ModuleName == "GoogleBase")
        {
            ExportFeed.RefreshModuleSetting(ModuleName, "DatafeedTitle", DatafeedTitleTextBox.Text);
            ExportFeed.RefreshModuleSetting(ModuleName, "DatafeedDescription", DatafeedDescriptionTextBox.Text);
        }
        saveSuccess.Visible = true;
    }
}