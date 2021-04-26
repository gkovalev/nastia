using System;
using System.Linq;
using System.Web.UI.WebControls;
using AdvantShop.Customers;

public partial class Admin_UserControls_CommonStatistics : System.Web.UI.UserControl
{
    private void MsgErr(string messageText)
    {
        Parent.FindControl("Message").Visible = true;
        ((TextBox)Parent.FindControl("Message")).Text += @"<br/>" + messageText + @"<br/>";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Customer _customer = CustomerSession.CurrentCustomer;

        if (_customer.CustomerRole == Role.Moderator)
        {
            var actions = RoleActionService.GetCustomerRoleActionsByCustomerId(_customer.Id);

            OrderStatusStatistics.Visible = actions.Any(a => a.Key == RoleActionKey.DisplayOrders && a.Enabled);
            OrderStatistics.Visible = actions.Any(a => a.Key == RoleActionKey.DisplayOrders && a.Enabled);
            CustomersStatistics1.Visible = actions.Any(a => a.Key == RoleActionKey.DisplayCustomers && a.Enabled);
            ProductsStatistics.Visible =  actions.Any(a => a.Key == RoleActionKey.DisplayCatalog && a.Enabled);
        }
    }
}