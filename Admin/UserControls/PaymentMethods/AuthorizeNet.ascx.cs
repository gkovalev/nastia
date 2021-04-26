using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop;
using AdvantShop.Controls;
using AdvantShop.Payment;

public partial class Admin_UserControls_PaymentMethods_AuthorizeNet : ParametersControl
{
    public override Dictionary<string, string> Parameters
    {
        get
        {
            return _valid || ValidateFormData(new[] {txtLogin, txtTransactionKey, txtCurrencyValue},
                                              new[] {txtCurrencyValue},
                                              null)
                       ? new Dictionary<string, string>
                             {
                                 {AuthorizeNetTemplate.Login, txtLogin.Text},
                                 {AuthorizeNetTemplate.TransactionKey, txtTransactionKey.Text},
                                 {AuthorizeNetTemplate.CurrencyValue, txtCurrencyValue.Text},
                                 {AuthorizeNetTemplate.Sandbox, chkSandbox.Checked.ToString()}
                             }
                       : null;
        }
        set { txtLogin.Text = value.ElementOrDefault(AuthorizeNetTemplate.Login);
            txtTransactionKey.Text = value.ElementOrDefault(AuthorizeNetTemplate.TransactionKey);
            txtCurrencyValue.Text = value.ElementOrDefault(AuthorizeNetTemplate.CurrencyValue);
            chkSandbox.Checked = value.ElementOrDefault(AuthorizeNetTemplate.Sandbox).TryParseBool();
        }
    }
}