//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

public partial class UserControls_VoteForAdv : System.Web.UI.UserControl
{
    private bool _visible = true;
    public override bool Visible
    {
        get
        {
            return _visible;
        }
        set
        {
            _visible = value;
        }
    }
}

