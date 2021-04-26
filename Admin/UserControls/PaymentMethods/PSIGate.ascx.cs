using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop;
using AdvantShop.Controls;
using AdvantShop.Payment;

public partial class Admin_UserControls_PaymentMethods_PSIGate : ParametersControl
{
    public override Dictionary<string, string> Parameters
    {
        get
        {
            return _valid || ValidateFormData(new[] { txtStoreKey, txtCurrencyValue },new []{txtCurrencyValue})
                       ? new Dictionary<string, string>
                             {
                                 {PSIGateTemplate.StoreKey, txtStoreKey.Text},
                                 {PSIGateTemplate.Sandbox, chkSandbox.Checked.ToString()},
                                 {PSIGateTemplate.CurrencyValue, txtCurrencyValue.Text}
                             }
                       : null;

        }
        set
        {
            txtStoreKey.Text = value.ElementOrDefault(PSIGateTemplate.StoreKey);
            chkSandbox.Checked = value.ElementOrDefault(PSIGateTemplate.Sandbox).TryParseBool();
            txtCurrencyValue.Text = value.ElementOrDefault(PSIGateTemplate.CurrencyValue);
        }
    }
    
}