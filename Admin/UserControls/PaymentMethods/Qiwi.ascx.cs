using System.Collections.Generic;
using AdvantShop;
using AdvantShop.Controls;
using AdvantShop.Payment;

public partial class Admin_UserControls_PaymentMethods_Qiwi : ParametersControl
{

    public override Dictionary<string, string> Parameters
    {
        get
        {
            return _valid ||
                   ValidateFormData(new[] { txtQiwiId, txtLifetime }, null, new[] { txtLifetime })
                       ? new Dictionary<string, string>
                             {
                                 {QiwiTemplate.From, txtQiwiId.Text},
                                 {QiwiTemplate.Lifetime, txtLifetime.Text},
                                 
                             }
                       : null;
        }
        set
        {
            txtQiwiId.Text = value.ElementOrDefault(QiwiTemplate.From);
        }
    }

}