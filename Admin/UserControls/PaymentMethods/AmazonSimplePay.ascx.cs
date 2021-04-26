using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop;
using AdvantShop.Controls;
using AdvantShop.Payment;

public partial class Admin_UserControls_PaymentMethods_AmazonSimplePay : ParametersControl
{
    public override Dictionary<string, string> Parameters
    {
        get
        {
            return _valid || ValidateFormData(new[] { txtAccessKey, txtSecretKey }, null,null)
                       ? new Dictionary<string, string>
                             {
                                 {AmazonSimplePayTemplate.AccessKey, txtAccessKey.Text},
                                 {AmazonSimplePayTemplate.SecretKey, txtSecretKey.Text},
                                 {AmazonSimplePayTemplate.Sandbox, chkSandbox.Checked.ToString()}
                             }
                       : null;
        }
        set { txtAccessKey.Text = value.ElementOrDefault(AmazonSimplePayTemplate.AccessKey);
            txtSecretKey.Text = value.ElementOrDefault(AmazonSimplePayTemplate.SecretKey);
            bool boolval;
            chkSandbox.Checked = !bool.TryParse(value.ElementOrDefault(AmazonSimplePayTemplate.Sandbox), out boolval) ||
                                 boolval;
        }
    }

}