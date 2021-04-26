using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop;
using AdvantShop.Controls;
using AdvantShop.Payment;

public partial class Admin_UserControls_PaymentMethods_Moneybookers : ParametersControl
{
    public override Dictionary<string, string> Parameters
    {
        get
        {
            return _valid ||
                   ValidateFormData(new[] {txtPayToEmai, txtCurrencyCode, txtCurrencyValue, txtSecretWord},
                                    new[] {txtCurrencyValue})
                       ? new Dictionary<string, string>
                             {
                                 {MoneybookersTemplate.PayToEmai, txtPayToEmai.Text},
                                 {MoneybookersTemplate.CurrencyIso, txtCurrencyCode.Text},
                                 {MoneybookersTemplate.CurrencyValue, txtCurrencyValue.Text},
                                 {MoneybookersTemplate.SecretWord, txtSecretWord.Text},
                                 {MoneybookersTemplate.Sandbox, chkSandbox.Checked.ToString()},
                             }
                       : null;
        }
        set
        {
            txtPayToEmai.Text = value.ElementOrDefault(MoneybookersTemplate.PayToEmai);
        txtCurrencyCode.Text = value.ElementOrDefault(MoneybookersTemplate.CurrencyIso);
        txtCurrencyValue.Text = value.ElementOrDefault(MoneybookersTemplate.CurrencyValue);
        txtSecretWord.Text = value.ElementOrDefault(MoneybookersTemplate.SecretWord);
        chkSandbox.Checked = value.ElementOrDefault(MoneybookersTemplate.Sandbox).TryParseBool();
        }
    }
   
}