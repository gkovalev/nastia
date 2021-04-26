using System;
using System.Web.UI.WebControls;
using AdvantShop;

public partial class Admin_UserControls_OrderStatusStatistics : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void sds_Init(object sender, EventArgs e)
    {
        ((SqlDataSource) sender).ConnectionString = Connection.GetConnectionString();
    }
}