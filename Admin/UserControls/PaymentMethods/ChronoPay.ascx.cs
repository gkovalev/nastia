using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop;
using AdvantShop.Controls;
using AdvantShop.Payment;

public partial class Admin_UserControls_PaymentMethods_ChronoPay : ParametersControl
{
    public override Dictionary<string, string> Parameters
    {
        get
        {
            return _valid || ValidateFormData(new[] {  txtProductId,
                         txtProductName,
                         txtSharedSecret },null,null)
                       ? new Dictionary<string, string>
                             {
                                 {ChronoPayTemplate.ProductId, txtProductId.Text},
                                 {ChronoPayTemplate.ProductName, txtProductName.Text},
                                 {ChronoPayTemplate.SharedSecret, txtSharedSecret.Text}
                             }
                       : null;
        }
        set
        {
            txtProductId.Text = value.ElementOrDefault(ChronoPayTemplate.ProductId);
            txtProductName.Text = value.ElementOrDefault(ChronoPayTemplate.ProductName);
            txtSharedSecret.Text = value.ElementOrDefault(ChronoPayTemplate.SharedSecret);
        }
    }
   
}