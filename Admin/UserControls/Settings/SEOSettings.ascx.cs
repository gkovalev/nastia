using System;
using AdvantShop.Configuration;

public partial class Admin_UserControls_Settings_SEOSettings : System.Web.UI.UserControl
{
    public string ErrMessage = Resources.Resource.Admin_CommonSettings_InvalidSEO;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            LoadData();
    }

    private void LoadData()
    {
        txtProductsHeadTitle.Text = SettingsSEO.ProductMetaTitle;
        txtProductsMetaKeywords.Text = SettingsSEO.ProductMetaKeywords;
        txtProductsMetaDescription.Text = SettingsSEO.ProductMetaDescription;

        txtCategoriesHeadTitle.Text = SettingsSEO.CategoryMetaTitle;
        txtCategoriesMetaKeywords.Text = SettingsSEO.CategoryMetaKeywords;
        txtCategoriesMetaDescription.Text = SettingsSEO.CategoryMetaDescription;

        txtNewsHeadTitle.Text = SettingsSEO.NewsMetaTitle;
        txtNewsMetaKeywords.Text = SettingsSEO.NewsMetaKeywords;
        txtNewsMetaDescription.Text = SettingsSEO.NewsMetaDescription;

        txtStaticPageHeadTitle.Text = SettingsSEO.StaticPageMetaTitle;
        txtStaticPageMetaKeywords.Text = SettingsSEO.StaticPageMetaKeywords;
        txtStaticPageMetaDescription.Text = SettingsSEO.StaticPageMetaDescription;

        txtTitle.Text = SettingsSEO.DefaultMetaTitle;
        txtMetaKeys.Text = SettingsSEO.DefaultMetaKeywords;
        txtMetaDescription.Text = SettingsSEO.DefaultMetaDescription;

        txtGoogleAnalytics.Text = SettingsSEO.GoogleAnalyticsNumber;
        chbGoogleAnalytics.Checked = SettingsSEO.GoogleAnalyticsEnabled;
        txtCustomMetaString.Text = SettingsSEO.CustomMetaString;

    }
    public bool SaveData()
    {

        SettingsSEO.ProductMetaTitle = txtProductsHeadTitle.Text;
        SettingsSEO.ProductMetaKeywords = txtProductsMetaKeywords.Text;
        SettingsSEO.ProductMetaDescription = txtProductsMetaDescription.Text;

        SettingsSEO.CategoryMetaTitle = txtCategoriesHeadTitle.Text;
        SettingsSEO.CategoryMetaKeywords = txtCategoriesMetaKeywords.Text;
        SettingsSEO.CategoryMetaDescription = txtCategoriesMetaDescription.Text;

        SettingsSEO.NewsMetaTitle = txtNewsHeadTitle.Text;
        SettingsSEO.NewsMetaKeywords = txtNewsMetaKeywords.Text;
        SettingsSEO.NewsMetaDescription = txtNewsMetaDescription.Text;

        SettingsSEO.StaticPageMetaTitle = txtStaticPageHeadTitle.Text;
        SettingsSEO.StaticPageMetaKeywords = txtStaticPageMetaKeywords.Text;
        SettingsSEO.StaticPageMetaDescription = txtStaticPageMetaDescription.Text;

        SettingsSEO.GoogleAnalyticsNumber = txtGoogleAnalytics.Text;
        SettingsSEO.GoogleAnalyticsEnabled = chbGoogleAnalytics.Checked;

        SettingsSEO.DefaultMetaTitle = txtTitle.Text;
        SettingsSEO.DefaultMetaKeywords = txtMetaKeys.Text;
        SettingsSEO.DefaultMetaDescription = txtMetaDescription.Text;


        SettingsSEO.DefaultMetaTitle = txtTitle.Text;
        SettingsSEO.DefaultMetaKeywords = txtMetaKeys.Text;
        SettingsSEO.DefaultMetaDescription = txtMetaDescription.Text;
        SettingsSEO.GoogleAnalyticsNumber = txtGoogleAnalytics.Text;

        SettingsSEO.GoogleAnalyticsEnabled = chbGoogleAnalytics.Checked;

        SettingsSEO.CustomMetaString = txtCustomMetaString.Text;
        LoadData();

        return true;
    }
}