public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, System.EventArgs e)
    {
        if (Session["YouCanUserCore"] == null || !(bool)Session["YouCanUserCore"])
        {
            Page.Response.Redirect("~/tools/");
        }
    }
}