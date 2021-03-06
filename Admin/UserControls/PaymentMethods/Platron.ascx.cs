using System.Collections.Generic;
using AdvantShop;
using AdvantShop.Controls;
using AdvantShop.Payment;

public partial class Admin_UserControls_PaymentMethods_Platron : ParametersControl
{
    public override Dictionary<string, string> Parameters
    {
        get
        {

            return _valid ||
                   ValidateFormData(
                       new[] {txtMerchantId, txtCurrency, txtPaymentSystem, txtCurrencyValue, txtSecretKey},
                       new[] {txtCurrencyValue})
                       ? new Dictionary<string, string>
                             {
                                 {PlatronTemplate.MerchantId, txtMerchantId.Text},
                                 {PlatronTemplate.Currency, txtCurrency.Text},
                                 {PlatronTemplate.PaymentSystem, txtPaymentSystem.Text},
                                 {PlatronTemplate.CurrencyValue, txtCurrencyValue.Text},
                                 {PlatronTemplate.SecretKey, txtSecretKey.Text},
                             }
                       : null;
        }
        set
        {
            txtMerchantId.Text = value.ElementOrDefault(PlatronTemplate.MerchantId);
            txtCurrency.Text = value.ElementOrDefault(PlatronTemplate.Currency);
            txtPaymentSystem.Text = value.ElementOrDefault(PlatronTemplate.PaymentSystem);
            txtCurrencyValue.Text = value.ElementOrDefault(PlatronTemplate.CurrencyValue);
            txtSecretKey.Text = value.ElementOrDefault(PlatronTemplate.SecretKey);
        }
    }
 
}