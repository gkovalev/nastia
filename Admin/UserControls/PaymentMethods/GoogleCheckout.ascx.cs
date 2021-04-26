using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop;
using AdvantShop.Controls;
using AdvantShop.Payment;

public partial class Admin_UserControls_PaymentMethods_GoogleCheckout : ParametersControl
{
    public override Dictionary<string, string> Parameters
    {
        get
        {
            return _valid ||
                   ValidateFormData(new[] {txtMerchantID, txtCurrencyValue, txtCurrencyCode}, new[] {txtCurrencyValue})
                       ? new Dictionary<string, string>
                             {
                                 {GoogleCheckoutTemplate.MerchantID, txtMerchantID.Text},
                                 {GoogleCheckoutTemplate.CurrencyValue, txtCurrencyValue.Text},
                                 {GoogleCheckoutTemplate.CurrencyCode, txtCurrencyCode.Text},
                                 {GoogleCheckoutTemplate.Sandbox, chkSandbox.Text}
                             }
                       : null;
        }
        set
        {
            txtMerchantID.Text = value.ElementOrDefault(GoogleCheckoutTemplate.MerchantID);
            txtCurrencyValue.Text = value.ElementOrDefault(GoogleCheckoutTemplate.CurrencyValue);
            txtCurrencyCode.Text = value.ElementOrDefault(GoogleCheckoutTemplate.CurrencyCode);
            chkSandbox.Checked = value.ElementOrDefault(GoogleCheckoutTemplate.Sandbox).TryParseBool();
        }
    }
}