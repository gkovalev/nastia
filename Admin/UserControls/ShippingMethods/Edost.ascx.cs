using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using AdvantShop;
using AdvantShop.Controls;
using AdvantShop.Shipping;

public partial class Admin_UserControls_ShippingMethods_Edost : ParametersControl
{
    public override Dictionary<string, string> Parameters
    {
        get
        {
            return _valid || ValidateFormData(new[] { txtShopId, txtPassword }, new[]{txtRate})
                       ? new Dictionary<string, string>
                             {
                                 {EdostTemplate.ShopId, txtShopId.Text},
                                 {EdostTemplate.Password, txtPassword.Text},
                                 {EdostTemplate.EnabledCOD, chbcreateCOD.Checked.ToString()  },
                                 {EdostTemplate.EnabledPickPoint , chbcreatePickPoint.Checked.ToString()  },
                                 {EdostTemplate.ShipIdCOD, hfCod.Value  },
                                 {EdostTemplate.ShipIdPickPoint, hfPickPoint.Value },
                                 {EdostTemplate.Rate, txtRate.Text }
                             }
                       : null;
        }
        set
        {
            txtShopId.Text = value.ElementOrDefault(EdostTemplate.ShopId);
            txtRate.Text = value.ElementOrDefault(EdostTemplate.Rate);
            txtPassword.Text = value.ElementOrDefault(EdostTemplate.Password);
            txtPassword.Visible = ! (Demo.IsDemoEnabled || Trial.IsTrialEnabled);            
            lPassword.Visible = Demo.IsDemoEnabled || Trial.IsTrialEnabled;

            chbcreateCOD.Checked = Convert.ToBoolean(value.ElementOrDefault(EdostTemplate.EnabledCOD));
            chbcreatePickPoint.Checked = Convert.ToBoolean(value.ElementOrDefault(EdostTemplate.EnabledPickPoint));
            hfCod.Value = value.ElementOrDefault(EdostTemplate.ShipIdCOD);
            hfPickPoint.Value = value.ElementOrDefault(EdostTemplate.ShipIdPickPoint);            
        }
    }
}