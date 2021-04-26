using System;
using AdvantShop;
using AdvantShop.Configuration;
using Resources;

public partial class Admin_UserControls_Settings_CatalogSettings : System.Web.UI.UserControl
{
    public string ErrMessage = Resources.Resource.Admin_CommonSettings_InvalidCatalog;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            LoadData();
    }

    private void LoadData()
    {
        ddlDefaultCurrency.DataSource = SqlDataSource2;
        ddlDefaultCurrency.DataTextField = "Name";
        ddlDefaultCurrency.DataValueField = "CurrencyIso3";
        ddlDefaultCurrency.DataBind();

        ddlDefaultCurrency.SelectedValue = SettingsCatalog.DefaultCurrencyIso3;
        txtProdPerPage.Text = SettingsCatalog.ProductsPerPage.ToString();
        cbEnableProductRating.Checked = SettingsCatalog.EnableProductRating;
        cbEnableCompareProducts.Checked = SettingsCatalog.EnableCompareProducts;
        cbEnableCatalogViewChange.Checked = SettingsCatalog.EnabledCatalogViewChange;
        cbEnableSearchViewChange.Checked = SettingsCatalog.EnabledSearchViewChange;
        ckbModerateReviews.Checked = SettingsCatalog.ModerateReviews;
        chkAllowReviews.Checked = SettingsCatalog.AllowReviews;

        ddlCatalogView.SelectedValue = SettingsCatalog.DefaultCatalogView.ToString();
        ddlSearchView.SelectedValue = SettingsCatalog.DefaultSearchView.ToString();
        chkCompressBigImage.Checked = SettingsCatalog.CompressBigImage;

        txtBlockOne.Text = SettingsCatalog.RelatedProductName;
        txtBlockTwo.Text = SettingsCatalog.AlternativeProductName;

        cbShowProductsCount.Checked = SettingsCatalog.ShowProductsCount;
    }


    public bool SaveData()
    {
        if (!ValidateData())
            return false;

        SettingsCatalog.DefaultCurrencyIso3 = ddlDefaultCurrency.SelectedValue;
        SettingsCatalog.ProductsPerPage = Convert.ToInt32(txtProdPerPage.Text);
        SettingsCatalog.EnableProductRating = cbEnableProductRating.Checked;
        SettingsCatalog.EnableCompareProducts = cbEnableCompareProducts.Checked;
        SettingsCatalog.EnabledCatalogViewChange = cbEnableCatalogViewChange.Checked;
        SettingsCatalog.EnabledSearchViewChange = cbEnableSearchViewChange.Checked;
        SettingsCatalog.DefaultCatalogView = Convert.ToInt32(ddlCatalogView.SelectedValue);
        SettingsCatalog.DefaultSearchView = Convert.ToInt32(ddlSearchView.SelectedValue);
        SettingsCatalog.CompressBigImage = chkCompressBigImage.Checked;
        SettingsCatalog.ModerateReviews = ckbModerateReviews.Checked;
        SettingsCatalog.AllowReviews = chkAllowReviews.Checked;

        SettingsCatalog.RelatedProductName = txtBlockOne.Text;
        SettingsCatalog.AlternativeProductName = txtBlockTwo.Text;

        SettingsCatalog.ShowProductsCount = cbShowProductsCount.Checked;

        LoadData();
        return true;
    }

    private bool ValidateData()
    {
        if (string.IsNullOrEmpty(ddlDefaultCurrency.SelectedValue))
        {
            ErrMessage = "";
            return false;
        }

        if (string.IsNullOrEmpty(txtProdPerPage.Text))
        {
            ErrMessage = "";
            return false;
        }

        int ti;
        if (!int.TryParse(txtProdPerPage.Text, out ti))
        {
            ErrMessage = Resource.Admin_CommonSettings_NoNumberPerPage;
            return false;
        }
        return true;
    }

    protected void SqlDataSource2_Init(object sender, EventArgs e)
    {
        SqlDataSource2.ConnectionString = Connection.GetConnectionString();
    }

    protected void btnDoindex_Click(object sender, EventArgs e)
    {
        AdvantShop.FullSearch.LuceneSearch.CreateAllIndexInBackground();
        lbDone.Visible = true;
    }

}