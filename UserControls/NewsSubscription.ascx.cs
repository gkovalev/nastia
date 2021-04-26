//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

public partial class UserControls_NewsSubscription : System.Web.UI.UserControl
{
    protected void btnSubmit_Click(object sender, System.EventArgs e)
    {
        //Changed By Evgeni
        //Page.Response.Redirect("subscribe.aspx?emailtosubscribe=" + txtEmail.Text);
        Page.Response.Redirect("~/subscribe.aspx?emailtosubscribe=" + txtEmail.Text);
    }
}
