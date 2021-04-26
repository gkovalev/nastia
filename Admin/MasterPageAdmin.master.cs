//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------
using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using AdvantShop;
using AdvantShop.Configuration;
using AdvantShop.Core.Caching;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Customers;
using AdvantShop.Helpers;
using AdvantShop.SaasData;
using AdvantShop.Security;
using SquishIt.Framework;
using SquishIt.Framework.Css;
using SquishIt.Framework.JavaScript;
using SquishIt.Framework.Minifiers;
using SquishIt.Framework.Minifiers.CSS;
using SquishIt.Framework.Minifiers.JavaScript;


public partial class MasterPageAdmin : MasterPage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        Secure.VerifySessionForErrors();
        Secure.VerifyAccessLevel();
        CommonHelper.DisableBrowserCache();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if ((SaasDataService.IsSaasEnabled) && (!SaasDataService.CurrentSaasData.IsWorkingNow))
        {
            Response.Redirect(UrlService.GetAbsoluteLink("/app_offline.html"));
        }

        MenuAdmin.CurrentCustomer = CustomerSession.CurrentCustomer;

        if (CustomerSession.CurrentCustomer.IsVirtual)
        {
            lblIsDebug.Text = @"&nbsp;&nbsp;&nbsp; (Debug mode)";
            lblIsDebug.Visible = true;
            Image1.ImageUrl = "images/logo_red.gif";
        }
        else
        {
            lblIsDebug.Visible = false;
            Image1.ImageUrl = "images/logo.jpg";
        }
    }

    public void Page_PreRender(object sender, EventArgs e)
    {
        Customer _customer = CustomerSession.CurrentCustomer;
        if (_customer.CustomerRole == Role.Moderator)
        {
            var actions = RoleActionService.GetCustomerRoleActionsByCustomerId(_customer.Id);
            OnLineUsers.Visible = actions.Any(a => a.Key == RoleActionKey.DisplayOrders && a.Enabled);
        }

        OnLineUsers.Visible &= SettingsMain.EnableUserOnline;

        if (!FileHelpers.IsCombineCssJsExsist(true))
        {
            CacheManager.RemoveByPattern("squishit_");
        }

        headStyle.Text = Bundle.Css()
            .Add("~/admin/css/jquery.tooltip.css")
            .Add("~/admin/css/AdminStyle.css")
            .Add("~/admin/css/catalogDataTreeStyles.css")
            .Add("~/admin/css/exportFeedStyles.css")
            .Add("~/admin/css/jqueryslidemenu.css")
            .Add("~/css/jq/jquery.autocomplete.css")
            .Add("~/css/advcss/modal.css")
            .Add("~/js/plugins/progress/css/progress.css")
            .Add("~/js/plugins/jpicker/css/jpicker.css")
            .WithMinifier(MinifierFactory.Get<CSSBundle, YuiCompressor>())
            .Render("~/admin/css/combined_#.css");

        // combine java
        headScript.Text = Bundle.JavaScript()
            .Add("~/js/localization/" + SettingsMain.Language + "/lang.js")
            .Add("~/js/jq/jquery-1.7.1.min.js")
            .Add("~/js/jq/jquery.autocomplete.js")
            .Add("~/js/jq/jquery.metadata.js")
            .Add("~/js/fix/PIEInit.js")
            .Add("~/js/advjs/advModal.js")
            .Add("~/js/advjs/advTabs.js")
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
            .Add("~/js/advantshop.js")
            .Add("~/js/services/Utilities.js")
            .Add("~/js/services/scriptsManager.js")
            .Add("~/js/plugins/progress/progress.js")
            .Add("~/js/plugins/jpicker/jpicker.js")
            .WithMinifier(MinifierFactory.Get<JavaScriptBundle, YuiMinifier>())
            .Render("~/admin/js/combined_#.js");
    }

    protected void lnkExit_Click(object sender, EventArgs e)
    {
        CustomerSession.CreateAnonymousCustomerGuid();
        Session["isDebug"] = false;
        CommonHelper.DeleteCookie(HttpUtility.UrlEncode(SettingsMain.SiteUrl));
        Response.Redirect("~/");
    }
}