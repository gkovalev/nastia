using System;
using AdvantShop.Orders;
using AdvantShop.Shipping;

public partial class UserControls_OrderConfirmation_SecondStep : System.Web.UI.UserControl
{
    public OrderConfirmationData PageData { get; set; }

    public class SecondStepNextEventArgs
    {
        public int SelectShippingID { get; set; }
        public string SelectShippingButtonID { get; set; }
        public decimal SelectedShippingRate { get; set; }
        public string SelectedShippingText { get; set; }
        public ShippingOptionEx SelectedShippingExt { get; set; }
    }

    public event Action<object, SecondStepNextEventArgs> NextStep;
    public void OnNextStep(SecondStepNextEventArgs arg)
    {
        if (NextStep != null) NextStep(this, arg);
    }

    public event Action<object, SecondStepNextEventArgs> BackStep;
    public void OnBackStep(SecondStepNextEventArgs arg)
    {
        if (BackStep != null) BackStep(this, arg);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        if (PageData == null) return;
        btnNextFromShipPay.Enabled = true;
        LoadShipping();
    }

    protected void btnBackFromShipPay_Click(object sender, EventArgs e)
    {
        OnBackStep(new SecondStepNextEventArgs
        {
            SelectShippingButtonID = ShippingRates.SelectedID
        });
    }
    protected void btnNextFromShipPay_Click(object sender, EventArgs e)
    {
        OnNextStep(new SecondStepNextEventArgs
        {
            SelectShippingID = ShippingRates.SelectedMethodID,
            SelectShippingButtonID = ShippingRates.SelectedID,
            SelectedShippingRate = ShippingRates.SelectedRate,
            SelectedShippingText = ShippingRates.SelectedName,
            SelectedShippingExt = ShippingRates.SelectShippingOptionEx
        });
    }

    private void LoadShipping()
    {
        ShippingRates.CountryId = PageData.ShippingContact.CountryId;
        ShippingRates.Zip = PageData.ShippingContact.Zip;
        ShippingRates.City = PageData.ShippingContact.City;
        ShippingRates.Region = PageData.ShippingContact.RegionName;
        ShippingRates.SelectShippingOptionEx = PageData.ShippingOptionEx;
        ShippingRates.ShoppingCart = ShoppingCartService.CurrentShoppingCart;
        ShippingRates.LoadMethods(PageData.SelectShippingButtonId);
        btnNextFromShipPay.Enabled &= ShippingRates.ShippingRates.Count > 0;
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        btnNextFromShipPay.Visible = ShippingRates.SelectedID != string.Empty;
    }
}