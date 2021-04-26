using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop;
using AdvantShop.Controls;
using AdvantShop.Payment;

public partial class Admin_UserControls_PaymentMethods_RbkMoney : ParametersControl
{
    public override Dictionary<string, string> Parameters
    {
        get
        {
            return _valid || ValidateFormData(new[] { txtEshopId, txtRecipientCurrency, txtPreference, txtCurrencyValue },new[]{txtCurrencyValue})
                       ? new Dictionary<string, string>
                             {
                                 {RbkmoneyTemplate.EshopId, txtEshopId.Text},
                                 {RbkmoneyTemplate.RecipientCurrency, txtRecipientCurrency.Text},
                                 {RbkmoneyTemplate.Preference, txtPreference.Text},
                                 {RbkmoneyTemplate.CurrencyValue, txtCurrencyValue.Text},
                             }
                       : null;
        }
        set
        {
            txtEshopId.Text = value.ElementOrDefault(RbkmoneyTemplate.EshopId);
        txtRecipientCurrency.Text = value.ElementOrDefault(RbkmoneyTemplate.RecipientCurrency);
        txtPreference.Text = value.ElementOrDefault(RbkmoneyTemplate.Preference);
        txtCurrencyValue.Text = value.ElementOrDefault(RbkmoneyTemplate.CurrencyValue);
        }
    }
   
}