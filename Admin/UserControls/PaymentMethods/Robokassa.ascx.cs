using System.Collections.Generic;
using AdvantShop;
using AdvantShop.Controls;
using AdvantShop.Payment;

public partial class Admin_UserControls_PaymentMethods_Robokassa : ParametersControl
{
    public override Dictionary<string, string> Parameters
    {
        get
        {
            return _valid ||
                   ValidateFormData(new[] { txtMerchantLogin, txtPassword, txtCurrencyLabel, txtCurrencyValue },
                                    new[] { txtCurrencyValue })
                       ? new Dictionary<string, string>
                             {
                                 {RobokassaTemplate.MerchantLogin, txtMerchantLogin.Text},
                                 {RobokassaTemplate.Password, txtPassword.Text},
                                 {RobokassaTemplate.CurrencyLabel, txtCurrencyLabel.Text},
                                 {RobokassaTemplate.CurrencyValue, txtCurrencyValue.Text}
                             }
                       : null;
        }
        set
        {
            txtMerchantLogin.Text = value.ElementOrDefault(RobokassaTemplate.MerchantLogin);
            txtPassword.Text = value.ElementOrDefault(RobokassaTemplate.Password);
            txtCurrencyLabel.Text = value.ElementOrDefault(RobokassaTemplate.CurrencyLabel);
            txtCurrencyValue.Text = value.ElementOrDefault(RobokassaTemplate.CurrencyValue);
        }
    }
}