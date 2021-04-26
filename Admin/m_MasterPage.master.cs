using System;
using AdvantShop.Core.Caching;
using AdvantShop.Helpers;
using AdvantShop.Security;
using SquishIt.Framework.Css;
using SquishIt.Framework.JavaScript;
using SquishIt.Framework.Minifiers;
using SquishIt.Framework.Minifiers.CSS;
using SquishIt.Framework.Minifiers.JavaScript;
using SquishIt.Framework;

public partial class Admin_m_MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        Secure.VerifySessionForErrors();
        Secure.VerifyAccessLevel();
        CommonHelper.DisableBrowserCache();
    }

    public void Page_PreRender(object sender, EventArgs e)
    {
        if (!FileHelpers.IsCombineCssJsExsist(false))
        {
            CacheManager.RemoveByPattern("squishit_");
        }

        // combine css

        headStyle.Text = Bundle.Css()
            .Add("~/admin/css/jquery.tooltip.css")
            .Add("~/admin/css/AdminStyle.css")
            .Add("~/admin/css/catalogDataTreeStyles.css")
            .Add("~/admin/css/exportFeedStyles.css")
            .Add("~/admin/css/jqueryslidemenu.css")
            .Add("~/css/jq/jquery.autocomplete.css")
            .Add("~/css/advcss/modal.css")
            .Add("~/css/advcss/progress.css")  
            .WithMinifier(MinifierFactory.Get<CSSBundle, YuiCompressor>())
            .Render("~/admin/css/combined_#.css");

        // combine java
        headScript.Text = Bundle.JavaScript()
            .Add("~/js/jq/jquery-1.7.1.min.js")
            .Add("~/js/jq/jquery.autocomplete.js")
            .Add("~/js/jq/jquery.metadata.js")
            .Add("~/js/advjs/advModal.js")
            .Add("~/js/advjs/advTabs.js")
            .Add("~/js/advjs/advProgress.js")
            .Add("~/js/advjs/advUtils.js")
            .Add("~/admin/js/jquery.cookie.min.js")
            .Add("~/admin/js/jquery.qtip.min.js")
            .Add("~/admin/js/jquery.tooltip.min.js")
            .Add("~/admin/js/slimbox2.js")
            .Add("~/admin/js/jquery.history.js")
            .Add("~/admin/js/jquerytimer.js")
            .Add("~/admin/js/jqueryslidemenu.js")
            .Add("~/admin/js/admin.js")
            .Add("~/admin/js/grid.js")
            .WithMinifier(MinifierFactory.Get<JavaScriptBundle, YuiMinifier>())
            .Render("~/admin/js/combined_#.js");
    }
}
