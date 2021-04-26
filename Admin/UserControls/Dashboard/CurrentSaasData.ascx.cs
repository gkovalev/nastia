using System;
using System.Drawing;
using AdvantShop.SaasData;
using AdvantShop.Catalog;

public partial class Admin_UserControls_CurrentSaasData : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (SaasDataService.IsSaasEnabled)
        {
            UpdateData();
        }
    }

    protected void btnUpdateSaasData_Click(object sender, EventArgs e)
    {
        SaasDataService.UpdateSaasDataFromService();
        UpdateData();
    }

    private void UpdateData()
    {
        var saasData = SaasDataService.CurrentSaasData;

        lblTariffName.Text = saasData.Name;
        lblLastUpdate.Text = saasData.LastUpdate.ToString();
        lblDaysLeft.Text = (saasData.PaidTo - DateTime.Now).Days.ToString();
        lblPaidTo.Text = saasData.PaidTo.ToString();
        if ((saasData.PaidTo - DateTime.Now).Days < 5)
        {
            lblPaidTo.ForeColor = Color.FromArgb(230, 0, 0);
        }

        lblProductsCount.Text = ProductService.GetProductsCount().ToString();
        lblProductsCount.ForeColor = GetColor(ProductService.GetProductsCount(), saasData.ProductsCount);
        lblMaxProductsCount.Text = saasData.ProductsCount.ToString();
        lblMaxPhotos.Text = saasData.PhotosCount.ToString();

        lblHaveExcel.Text = saasData.HaveExcel ? "Да" : "Нет";
        lblHave1C.Text = saasData.Have1C ? "Да" : "Нет";
        lblHaveExportFeeds.Text = saasData.HaveExportFeeds ? "Да" : "Нет";
        lblHavePriceRegulating.Text = saasData.HavePriceRegulating ? "Да" : "Нет";
        lblHaveBankIntegration.Text = saasData.HaveBankIntegration ? "Да" : "Нет";
    }

    // Переход от зеленого к красному
    private static Color GetColor(int value, int maxValue)
    {
        // Делаем так, чтобы ratio был не больше 1.
        value = Math.Min(value, maxValue);
        double ratio = (double)value / (double)maxValue;
		
		if (double.IsNaN(ratio))
        {
            ratio = 1;
        }

        int green = 200 - (int)Math.Round(200 * ratio, 0);
        int red = 200 - green;

        return Color.FromArgb(red, green, 0);
    }
}