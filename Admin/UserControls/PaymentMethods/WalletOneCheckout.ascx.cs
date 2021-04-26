using System.Collections.Generic;
using AdvantShop;
using AdvantShop.Controls;
using AdvantShop.Payment;

public partial class Admin_UserControls_PaymentMethods_WalletOneCheckout : ParametersControl
{
    public override Dictionary<string, string> Parameters
    {
        get
        {
            return _valid ||
                   ValidateFormData(new[] { txtMerchantId, txtSecretKey },
                                    null,null)
                       ? new Dictionary<string, string>
                             {
                                 {WalletOneCheckoutTemplate.MerchantId, txtMerchantId.Text},
                                 {WalletOneCheckoutTemplate.SecretKey, txtSecretKey.Text}
                             }
                       : null;
        }
        set
        {
            txtMerchantId.Text = value.ElementOrDefault(WalletOneCheckoutTemplate.MerchantId);
            txtSecretKey.Text = value.ElementOrDefault(WalletOneCheckoutTemplate.SecretKey);
        }
    }
}