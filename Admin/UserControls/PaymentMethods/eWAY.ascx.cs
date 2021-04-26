using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop;
using AdvantShop.Controls;
using AdvantShop.Payment;

public partial class Admin_UserControls_PaymentMethods_eWAY : ParametersControl
{
    public override Dictionary<string, string> Parameters
    {
        get
        {
            return _valid || ValidateFormData(new[] {txtCustomerID, txtCurrencyValue}, new[] {txtCurrencyValue})
                       ? new Dictionary<string, string>
                             {
                                 {eWAYTemplate.CustomerID, txtCustomerID.Text},
                                 {eWAYTemplate.Sandbox, chkSandbox.Checked.ToString()},
                                 {eWAYTemplate.CurrencyValue, txtCurrencyValue.Text}
                             }
                       : null;
        }
        set
        {
            txtCustomerID.Text = value.ElementOrDefault(eWAYTemplate.CustomerID);
            chkSandbox.Checked = value.ElementOrDefault(eWAYTemplate.Sandbox).TryParseBool();
            txtCurrencyValue.Text = value.ElementOrDefault(eWAYTemplate.CurrencyValue);
        }
    }
   
}