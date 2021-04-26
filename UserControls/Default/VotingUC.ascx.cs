//-------------------------------------------------
// Project: AdvantShop.NET
// Made by: Maksim
// Web site: http:\\www.advantshop.net
//-------------------------------------------------- 

using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web.UI;
using AdvantShop.CMS;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;

public partial class UserControls_VotingUC : UserControl
{
    private readonly string _cookieCollectionNameVoting = AdvantShop.Configuration.SettingsMain.SiteUrl + "_Voting";

    protected void Page_PreRender(object sender, EventArgs e)
    {
        var theme = VoiceService.GetTopTheme();
        if (theme == null || theme.Answers == null || !theme.Answers.Any())
        {
            Visible = false;
            return;
        }
        try
        {
            if (Request.Browser.Cookies && Request.Cookies[_cookieCollectionNameVoting] == null)
            {
                CommonHelper.SetCookieCollection(_cookieCollectionNameVoting, new NameValueCollection(), new TimeSpan(365, 0, 0, 0));
            }

        }

        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
    }

    protected string GetAbsoluteLink(string link)
    {
        return UrlService.GetAbsoluteLink(link);
    }
}