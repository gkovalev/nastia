//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using AdvantShop.CMS;

public partial class UserControls_LiveinternetCounter_WebUserControl : System.Web.UI.UserControl
{
    public string RenderLiveCounter()
    {
        if (this.Visible)
        {
            var block = StaticBlockService.GetPagePartByKey("LiveCounter");
            if (block != null && block.Enabled)
                return block.Content;
        }
        return string.Empty;
    }

}

