using System.Collections.Generic;
using AdvantShop;
using AdvantShop.Controls;
using AdvantShop.Payment;

public partial class Admin_UserControls_PaymentMethods_YandexMoney : ParametersControl
{
    public override Dictionary<string, string> Parameters
    {
        get
        {
            return _valid ||
                   ValidateFormData(
                       new[] {txtShopID, txtScID, txtBankID, txtCustomerNumber, txtMethodID, txtCurrencyValue},
                       new[] {txtCurrencyValue}, new[] {txtShopID})
                       ? new Dictionary<string, string>
                             {
                                 {YandexMoneyTemplate.ShopID, txtShopID.Text},
                                 {YandexMoneyTemplate.ScID, txtScID.Text},
                                 {YandexMoneyTemplate.BankID, txtBankID.Text},
                                 {YandexMoneyTemplate.CustomerNumber, txtCustomerNumber.Text},
                                 {YandexMoneyTemplate.MethodID, txtMethodID.Text},
                                 {YandexMoneyTemplate.CurrencyValue, txtCurrencyValue.Text},
                             }
                       : null;
        }
        set
        {
            txtShopID.Text = value.ElementOrDefault(YandexMoneyTemplate.ShopID);
            txtScID.Text = value.ElementOrDefault(YandexMoneyTemplate.ScID);
            txtBankID.Text = value.ElementOrDefault(YandexMoneyTemplate.BankID);
            txtCustomerNumber.Text = value.ElementOrDefault(YandexMoneyTemplate.CustomerNumber);
            txtMethodID.Text = value.ElementOrDefault(YandexMoneyTemplate.MethodID);
            txtCurrencyValue.Text = value.ElementOrDefault(YandexMoneyTemplate.CurrencyValue);
        }
    }
    
}