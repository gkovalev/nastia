using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop;
using AdvantShop.Controls;
using AdvantShop.Payment;

public partial class Admin_UserControls_PaymentMethods_PayPoint : ParametersControl
{
    public override Dictionary<string, string> Parameters
    {
        get
        {
            return _valid ||
                   ValidateFormData(new[] {txtMerchant, txtCurrencyCode, txtCurrencyValue, txtPassword},
                                    new[] {txtCurrencyValue})
                       ? new Dictionary<string, string>
                             {
                                 {PayPointTemplate.Merchant, txtMerchant.Text},
                                 {PayPointTemplate.CurrencyCode, txtCurrencyCode.Text},
                                 {PayPointTemplate.CurrencyValue, txtCurrencyValue.Text},
                                 {PayPointTemplate.Password, txtPassword.Text}
                             }
                       : null;
        }
        set
        {
            txtMerchant.Text = value.ElementOrDefault(PayPointTemplate.Merchant);
            txtCurrencyCode.Text = value.ElementOrDefault(PayPointTemplate.CurrencyCode);
            txtCurrencyValue.Text = value.ElementOrDefault(PayPointTemplate.CurrencyValue);
            txtPassword.Text = value.ElementOrDefault(PayPointTemplate.Password);
        }
    }
  
}