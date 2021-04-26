using System;
using System.Web.UI.WebControls;
using AdvantShop.Orders;



public partial class UserControls_MyAccountOrderHistory : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Visible = String.IsNullOrEmpty(Request["orderid"]);

        if (Visible == false)
            return;
    }
}