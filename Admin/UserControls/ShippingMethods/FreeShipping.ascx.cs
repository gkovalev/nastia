using System.Collections.Generic;
using System.Web.UI.WebControls;
using AdvantShop;
using AdvantShop.Controls;
using AdvantShop.Shipping;

public partial class Admin_UserControls_ShippingMethods_FreeShipping : ParametersControl
{

    public override Dictionary<string, string> Parameters
    {
        get
        {
            return _valid || ValidateFormData(new TextBox[0])
                       ? new Dictionary<string, string>
                             {
                                 {FreeShippingTemplate.DeliveryTime, txtDeliveryTime.Text},
                             }
                       : null;
        }
        set
        {
            txtDeliveryTime.Text = value.ElementOrDefault(FreeShippingTemplate.DeliveryTime);
        }
    }
}