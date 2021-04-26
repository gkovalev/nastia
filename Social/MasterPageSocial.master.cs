//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop;
using AdvantShop.Configuration;
using AdvantShop.Core.Caching;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.SaasData;
using SquishIt.Framework;
using SquishIt.Framework.Css;
using SquishIt.Framework.JavaScript;
using SquishIt.Framework.Minifiers;
using SquishIt.Framework.Minifiers.CSS;
using SquishIt.Framework.Minifiers.JavaScript;

public partial class MasterPageSocial : System.Web.UI.MasterPage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        form.Action = Request.RawUrl;
        string type = Request["type"];

        if (type.IsNullOrEmpty())
        {
            type = CommonHelper.GetCookieString("socialtype");
        }

        if (type.IsNotEmpty())
        {
            CommonHelper.SetCookie("socialtype", type);
            lsocialCss.Text = string.Format("<link type=\"text/css\" rel=\"stylesheet\" href=\"social/css/{0}.css\" />", type);
        }

        vk.Visible = type == "vk" && !SettingsGeneral.AbsoluteUrl.Contains("localhost");

        if (SaasDataService.IsSaasEnabled && !SaasDataService.CurrentSaasData.IsWorkingNow)
        {
            Response.Redirect(UrlService.GetAbsoluteLink("/app_offline.html"));
        }

        Logo.ImgSource = FoldersHelper.GetPath(FolderType.Pictures, SettingsMain.LogoImageName, false);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!FileHelpers.IsCombineCssJsExsist(false))
        {
            CacheManager.RemoveByPattern("squishit_");
        }

        headStyle.Text = Bundle.Css()
            .Add("css/normalize.css")
            .Add("css/advcss/modal.css")
            .Add("css/advcss/notify.css")

            .Add("css/jq/jquery.cloud-zoom.css")
            .Add("css/jq/jquery-ui-1.8.17.custom.css")
            .Add("css/jq/jquery.autocomplete.css")
            .Add("css/jq/jquery.fancybox-1.3.4.css")
            .Add("css/jq/jquery.flexslider.css")
            .Add("css/theme.css")
            .Add("css/constructor.css")
            .Add("css/carousel.css")
            .Add("css/forms.css")
            .Add("css/styles.css")
            .Add("css/styles-extra.css")
            .Add("css/validator.css")
            .Add("js/plugins/jpicker/css/jpicker.css")
            .Add("js/plugins/upper/css/upper.css")
            .Add("js/plugins/expander/css/expander.css")
            .Add("js/plugins/vote/css/vote.css")
            .Add("js/plugins/progress/css/progress.css")
            .Add("js/plugins/compare/css/compare.css")
            .Add("js/plugins/spinbox/css/spinbox.css")
            .Add("js/plugins/cart/css/cart.css")
            .Add("js/plugins/scrollbar/css/scrollbar.css")
            .Add("js/plugins/tabs/css/tabs.css")
        .WithMinifier(MinifierFactory.Get<CSSBundle, YuiCompressor>())
        .ForceDebug()
        .Render("css/combined_#.css");

        headScript.Text = Bundle.JavaScript()
    .Add("js/jq/jquery-1.7.1.min.js")
    .Add("js/modernizr.custom.70373.js")
    .Add("js/ejs_fulljslint.js")
    .Add("js/ejs.js")
    .WithMinifier(MinifierFactory.Get<JavaScriptBundle, YuiMinifier>())
    .Render("js/combined_#.js");

        bottomScript.Text = Bundle.JavaScript()
            .Add("~/js/fix/PIEInit.js")
            .Add("~/js/localization/" + SettingsMain.Language + "/lang.js")
            .Add("~/js/string.format-1.0.js")
            .Add("~/js/advantshop.js")
            .Add("~/js/services/Utilities.js")
            .Add("~/js/services/scriptsManager.js")
            .Add("~/js/jq/jquery-ui-1.8.17.custom.min.js")
            .Add("~/js/jq/jquery.cloud-zoom.1.0.2.js")
            .Add("~/js/jq/jquery.cookie.js")
            .Add("~/js/jq/jquery.metadata.js")
            .Add("~/js/jq/jquery.fancybox-1.3.4.js")
            .Add("~/js/jq/jquery.flexslider.js")
            .Add("~/js/jq/jquery.jcarousel.min.js")
            .Add("~/js/jq/jquery.placeholder.js")
            .Add("~/js/jq/jquery.validate.js")
            .Add("~/js/jq/jquery.autocomplete.js")
            .Add("~/js/jq/jquery.raty.js")
            .Add("~/js/jq/jquery.mousewheel.js")
            .Add("~/js/advjs/advDetectTouch.js")
            .Add("~/js/advjs/advFeedback.js")
            .Add("~/js/advjs/advNotify.js")
            .Add("~/js/advjs/advModal.js")
            .Add("~/js/advjs/advMoveCaret.js")
            .Add("~/js/advjs/advGiftCertificate.js")
            .Add("~/js/advjs/advMyAccount.js")
            .Add("~/js/advjs/advOrderConfirmation.js")
            .Add("~/js/advjs/advReviews.js")
            .Add("~/js/advjs/advUtils.js")

            .Add("~/js/plugins/cart/cart.js")
            .Add("~/js/plugins/compare/compare.js")
            .Add("~/js/plugins/expander/expander.js")
            .Add("~/js/plugins/jpicker/jpicker.js")
            .Add("~/js/plugins/progress/progress.js")
            .Add("~/js/plugins/reviews/reviews.js")
            .Add("~/js/plugins/scrollbar/scrollbar.js")
            .Add("~/js/plugins/spinbox/spinbox.js")
            .Add("~/js/plugins/tabs/tabs.js")
            .Add("~/js/plugins/upper/upper.js")
            .Add("~/js/plugins/vote/vote.js")

            .Add("~/js/common.js")
            .Add("~/js/constructor.js")
            .Add("~/js/dopostback.js")
            .Add("~/js/validateInit.js")
            .WithMinifier(MinifierFactory.Get<JavaScriptBundle, YuiMinifier>())
            .ForceDebug()
            .Render("js/combined_#.js");
    }
}