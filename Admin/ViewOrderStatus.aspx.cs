
//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

public partial class Admin_ViewOrderStatus : System.Web.UI.Page
{
    protected override void InitializeCulture()
    {
        AdvantShop.Localization.Culture.InitializeCulture();
    }

    protected void SqlDataSource1_Init(object sender, System.EventArgs e)
    {
        SqlDataSource1.ConnectionString = AdvantShop.Connection.GetConnectionString();
    }
}
