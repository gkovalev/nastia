using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using AdvantShop.Payment;
using AdvantShop.Configuration;

public partial class Admin_PaymentMethod : Page
{

    private int _paymentMethodId;
    protected int PaymentMethodId
    {
        get
        {
            if (_paymentMethodId != 0)
                return _paymentMethodId;
            var intval = 0;
            int.TryParse(Request["paymentmethodid"], out intval);
            return intval;
        }

        set { _paymentMethodId = value; }
    }

    protected void Msg(string message)
    {
        lblMessage.Text = message;
        lblMessage.Visible = true;
    }

    protected void ClearMsg()
    {
        lblMessage.Visible = false;
    }

    protected static readonly Dictionary<PaymentType, string> UcIds = new Dictionary<PaymentType, string>
                                                                          {
                                                                              {PaymentType.SberBank, "ucSberBank"},
                                                                              {PaymentType.Bill, "ucBill"},
                                                                              {PaymentType.Cash, "ucCash"},
                                                                              {PaymentType.MailRu, "ucMailRu"},
                                                                              {PaymentType.WebMoney, "ucWebMoney"},
                                                                              {PaymentType.Robokassa, "ucRobokassa"},
                                                                              {PaymentType.YandexMoney, "ucYandexMoney"},
                                                                              {PaymentType.AuthorizeNet, "ucAuthorizeNet"},
                                                                              {PaymentType.GoogleCheckout, "ucGoogleCheckout"},
                                                                              {PaymentType.eWAY, "uceWAY"},
                                                                              {PaymentType.Check, "ucCheck"},
                                                                              {PaymentType.PayPal, "ucPayPal"},
                                                                              {PaymentType.TwoCheckout, "ucTwoCheckout"},
                                                                              {PaymentType.Assist, "ucAssist"},
                                                                              {PaymentType.ZPayment, "ucZPayment"},
                                                                              {PaymentType.Platron, "ucPlatron"},
                                                                              {PaymentType.Rbkmoney, "ucRbkmoney"},
                                                                              {PaymentType.CyberPlat, "ucCyberPlat"},
                                                                              {PaymentType.Moneybookers,"ucMoneybookers"},
                                                                              {PaymentType.AmazonSimplePay,"ucAmazonSimplePay"},
                                                                              {PaymentType.ChronoPay, "ucChronoPay"},
                                                                              {PaymentType.PayOnline, "ucPayOnline"},
                                                                              {PaymentType.PSIGate, "ucPSIGate"},
                                                                              {PaymentType.PayPoint, "ucPayPoint"},
                                                                              {PaymentType.SagePay, "ucSagePay"},
                                                                              {PaymentType.WorldPay, "ucWorldPay"},
                                                                              {PaymentType.OnPay, "ucOnPay"},
                                                                              {PaymentType.PickPoint, "ucPickPoint"},
                                                                              {PaymentType.CashOnDelivery, "ucCashOnDelivery"},
                                                                              {PaymentType.GiftCertificate, "ucGiftCertificate"},
                                                                              {PaymentType.MasterBank, "ucMasterBank"},
                                                                              {PaymentType.WalletOneCheckout, "ucWalletOneCheckout"},
                                                                              {PaymentType.QIWI, "ucQiwi"}
                                                                          };


    protected override void InitializeCulture()
    {
        AdvantShop.Localization.Culture.InitializeCulture();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = string.Format("{0} - {1}", SettingsMain.ShopName, Resources.Resource.Admin_PaymentMethod_Header);

        ClearMsg();
        if (!IsPostBack)
            LoadMethods();
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        ddlType.DataSource = AdvantShop.Core.AdvantshopConfigService.GetDropdownPayments();
        ddlType.DataBind();
    }

    protected void LoadMethods()
    {
        var methods = PaymentService.GetAllPaymentMethods(false).ToList();
        if (methods.Count > 0)
        {
            if (PaymentMethodId == 0)
                PaymentMethodId = methods.First().PaymentMethodID;
            rptTabs.DataSource = methods;
            rptTabs.DataBind();

        }
        ShowMethod(PaymentMethodId);
    }
    
    protected void ShowMethod(int methodId)
    {
        var method = PaymentService.GetPaymentMethod(methodId);
        foreach (var ucId in UcIds)
        {
            var uc = (Admin_UserControls_PaymentMethods_MasterControl)pnMethods.FindControl(ucId.Value);
            if (method == null)
            {
                uc.Visible = false;
                continue;
            }
            if (ucId.Key == method.Type)
                uc.Method = method;
            uc.Visible = ucId.Key == method.Type;
        }
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        var type = (PaymentType)int.Parse(ddlType.SelectedValue);
        var method = PaymentMethod.Create(type);
        method.Name = txtName.Text;
        method.Description = txtDescription.Text;
        if (!string.IsNullOrEmpty(txtSortOrder.Text))
            method.SortOrder = int.Parse(txtSortOrder.Text);
        method.Enabled = type == PaymentType.Cash;
        //Some dirty magic
        if (method.Parameters.ContainsKey(AssistTemplate.CurrencyValue))
        {
            var parameters = method.Parameters;
            parameters[AssistTemplate.CurrencyValue] = "1";
            method.Parameters = parameters;
        }
        //End of dirty magic
        var id = PaymentService.AddPaymentMethod(method);
        if (id != 0)
            Response.Redirect("~/Admin/PaymentMethod.aspx?PaymentMethodID=" + id);
    }

    protected void PaymentMethod_Saved(object sender, Admin_UserControls_PaymentMethods_MasterControl.SavedEventArgs args)
    {
        LoadMethods();
        Msg(string.Format(Resources.Resource.Admin_PaymentMethod_Saved, args.Name));
    }

    protected void PaymentMethod_Error(object arg1, Admin_UserControls_PaymentMethods_MasterControl.ErrorEventArgs arg2)
    {
        Msg(arg2.Message);
    }
}