using System;
using System.Linq;
using System.Web.UI;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using AdvantShop;
using AdvantShop.Controls;
using AdvantShop.Payment;

public partial class Admin_UserControls_PaymentMethods_PayPal : ParametersControl
{
    public override Dictionary<string, string> Parameters
    {
        get
        {
            return _valid || ValidateFormData(new[] { txtCurrencyValue, txtPdtCode, txtCurrencyCode, txtEmailID }, new[] { txtCurrencyValue })
                       ? new Dictionary<string, string>
                             {
                                 {PayPalTemplate.EMail, txtEmailID.Text},
                                 {PayPalTemplate.PDTCode, txtPdtCode.Text},
                                 {PayPalTemplate.CurrencyCode, txtCurrencyCode.Text},
                                 {PayPalTemplate.CurrencyValue, txtCurrencyValue.Text},
                                 {PayPalTemplate.Sandbox, chkSandbox.Checked.ToString()},
                                 {PayPalTemplate.ShowTaxAndShipping, chkShowTax.Checked.ToString()}
                             }
                       : null;
        }
        set
        {
            txtEmailID.Text = value.ElementOrDefault(PayPalTemplate.EMail);
            txtPdtCode.Text = value.ElementOrDefault(PayPalTemplate.PDTCode);
            txtCurrencyCode.Text = value.ElementOrDefault(PayPalTemplate.CurrencyCode);
            txtCurrencyValue.Text = value.ElementOrDefault(PayPalTemplate.CurrencyValue);
            chkSandbox.Checked = value.ElementOrDefault(PayPalTemplate.Sandbox).TryParseBool();
            chkShowTax.Checked = value.ElementOrDefault(PayPalTemplate.ShowTaxAndShipping).TryParseBool();
        }
    }
}