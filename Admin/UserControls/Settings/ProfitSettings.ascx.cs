using System;
using System.Globalization;
using AdvantShop.Catalog;
using AdvantShop.Orders;
using Resources;

public partial class Admin_UserControls_Settings_ProfitSettings : System.Web.UI.UserControl
{
    public string ErrMessage = Resource.Admin_CommonSettings_InvalidProfit;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            LoadData();
    }

    private void LoadData()
    {
        txtSalesPlan.Text = CatalogService.FormatPriceInvariant(OrderStatisticsService.SalesPlan);
        txtProfitPlan.Text =  CatalogService.FormatPriceInvariant(OrderStatisticsService.ProfitPlan);

    }

    public bool SaveData()
    {
        bool isValid = true;

        decimal sales = 0;
        decimal profit = 0;

        if (decimal.TryParse(txtSalesPlan.Text.Replace(" ", ""), out sales) && sales > 0 && decimal.TryParse(txtProfitPlan.Text.Replace(" ", ""), out profit) && profit > 0)
        {
           OrderStatisticsService.SetProfitPlan(sales, profit);
        }
        else
        {
            ErrMessage += Resource.Admin_CommonSettings_ProfitError;
            isValid = false;
        }
        
        LoadData();
        return isValid;
    }
}