using System;
using System.Web.UI;
using AdvOpenAuth;
using AdvantShop;
using AdvantShop.Configuration;

public partial class UserControls_LoginOpenID : UserControl
{
    public string PageToRedirect = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (SettingsOAuth.GoogleActive || SettingsOAuth.YandexActive || SettingsOAuth.TwitterActive ||
            SettingsOAuth.VkontakteActive || SettingsOAuth.FacebookActive || SettingsOAuth.MailActive)
        {
            var rootUrlPath = Request.Url.AbsoluteUri.Contains("localhost") ? "~/" : SettingsMain.SiteUrl;
            var strRedirectUrl = PageToRedirect.IsNotEmpty() ? rootUrlPath + "/" + PageToRedirect : rootUrlPath;
            if (!string.IsNullOrEmpty(Request["code"]) && Request["auth"] == "vk")
            {
                VkontakteOauth.VkontakteAuth(Request["code"], strRedirectUrl, string.Empty);
                Response.Redirect(strRedirectUrl, false);
            }
            else if (!string.IsNullOrEmpty(Request["oauth_token"]))
            {
                TwitterOAuth.TwitterGetUser(Request["oauth_token"], string.Empty);
                Response.Redirect(strRedirectUrl, false);
            }
            else if (!string.IsNullOrEmpty(Request["code"]))
            {
                FacebookOauth.SendFacebookRequest(Request["code"], SettingsMain.SiteUrl + "/" + PageToRedirect);
                Response.Redirect(strRedirectUrl, false);
            }
            else if (OAuthResponce.OAuthUser(Request))
            {
                Response.Redirect(strRedirectUrl, false);
            }

     
           }
        else
        {
            divSocial.Visible = false;
        }
    }

    protected void lnkbtnVkClick(object sender, EventArgs e)
    {
        VkontakteOauth.VkontakteAuthDialog();
    }

    protected void lnkbtnFacebookClick(object sender, EventArgs e)
    {
        //var rootUrlPath = Request.Url.AbsoluteUri.Contains("localhost") ? "~/" : SettingsMain.SiteUrl;
        //var strRedirectUrl = PageToRedirect.IsNotEmpty() ? rootUrlPath + "/" + PageToRedirect : rootUrlPath;
        FacebookOauth.ShowAuthDialog(SettingsMain.SiteUrl + "/" + PageToRedirect);
    }

    protected void lnkbtnMailClick(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtOauthUserId.Text))
            return;
        var userId = txtOauthUserId.Text;
        if (userId.Contains("@"))
        {
            userId = userId.Substring(0, userId.IndexOf("@"));
        }
        var oAuthRequest = new OAuthRequest { UserId = userId, Provider = OAuthRequest.Providers.Mail };
        oAuthRequest.CreateRequest(new ClaimParameters(), true);
    }

    protected void lnkbtnTwitterClick(object sender, EventArgs e)
    {
        TwitterOAuth.TwitterOpenAuth();
    }

    protected void lnkbtnGoogleClick(object sender, EventArgs e)
    {
        var oAuthRequest = new OAuthRequest { Provider = OAuthRequest.Providers.Google };
        var parameters = new FetchParameters();
        parameters.OpenidUserInformation.Add(RequestParameters.AxSchemaParams.Contact.email);
        parameters.OpenidUserInformation.Add(RequestParameters.AxSchemaParams.NamePerson.First);
        parameters.OpenidUserInformation.Add(RequestParameters.AxSchemaParams.NamePerson.Last);
        oAuthRequest.CreateRequest(parameters);
    }

    protected void lnkbtnYandexClick(object sender, EventArgs e)
    {
        var oAuthRequest = new OAuthRequest { Provider = OAuthRequest.Providers.Yandex };
        oAuthRequest.CreateRequest(new ClaimParameters(), false);
    }


    protected void Page_PreRender(object sender, EventArgs e)
    {
        lnkbtnGoogle.Visible = SettingsOAuth.GoogleActive;
        lnkbtnYandex.Visible = SettingsOAuth.YandexActive;
        lnkbtnTwitter.Visible = SettingsOAuth.TwitterActive;
        lnkbtnVk.Visible = SettingsOAuth.VkontakteActive;
        lnkbtnFacebook.Visible = SettingsOAuth.FacebookActive;
        lnkbtnMail.Visible = pnlMail.Visible = SettingsOAuth.MailActive;
    }
}