using System;
using System.Linq;
using AdvantShop;
using AdvantShop.Configuration;
using AdvantShop.Orders;
using AdvantShop.Payment;

public partial class UserControls_OrderConfirmation_ThirdStep : System.Web.UI.UserControl
{
    public OrderConfirmationData PageData { get; set; }

    public class ThirdStepNextEventArgs
    {
        public int SelectPaymentID { get; set; }
        public string SelectedPaymentText { get; set; }
        public ProcessType SelectedPaymentProcessType { get; set; }
        public PaymentType SelectedPaymentType { get; set; }
    }

    public event Action<object, ThirdStepNextEventArgs> NextStep;
    public void OnNextStep(ThirdStepNextEventArgs arg)
    {
        if (NextStep != null) NextStep(this, arg);
    }

    public event Action<object, ThirdStepNextEventArgs> BackStep;
    public void OnBackStep(ThirdStepNextEventArgs arg)
    {
        if (BackStep != null) BackStep(this, arg);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        if (PageData == null) return;
        btnNextFromShipPay.Enabled = true;
        LoadPayment();
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if(pm.SelectedID == 0)
        {
            btnNextFromShipPay.Visible = false;
        }
    }

    protected void btnBackFromShipPay_Click(object sender, EventArgs e)
    {
        OnBackStep(new ThirdStepNextEventArgs
        {
            SelectPaymentID = pm.SelectedID,
        });
    }
    protected void btnNextFromShipPay_Click(object sender, EventArgs e)
    {
        OnNextStep(new ThirdStepNextEventArgs
                       {
                           SelectPaymentID = pm.SelectedID,
                           SelectedPaymentText = pm.SelectedText,
                           SelectedPaymentProcessType = pm.SelectedProcessType,
                           SelectedPaymentType = pm.SelectedType,
                       });
    }

    private void LoadPayment()
    {
        var shpCart = ShoppingCartService.CurrentShoppingCart;
        bool showCertificate = SettingsOrderConfirmation.EnableGiftCertificateService && shpCart.Certificate != null &&
                               shpCart.TotalPrice - shpCart.TotalDiscount + PageData.SelectShippingRate <= 0;

        bool hideCash = false; // shpCart.All(item => item.ItemType != EnumItemType.Product); 

        pm.ShippingMethodId = PageData.SelectShippingId;
        pm.Ext = PageData.ShippingOptionEx;
        pm.SelectedID = PageData.SelectPaymentId;
        pm.LoadMethods(PageData.ShippingContact.Country, PageData.ShippingContact.City, showCertificate, hideCash);
        btnNextFromShipPay.Enabled &= pm.HasMethods;
    }
}