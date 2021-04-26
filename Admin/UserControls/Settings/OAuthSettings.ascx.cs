using System;
using AdvantShop.Configuration;

public partial class Admin_UserControls_Settings_OAuthSettings : System.Web.UI.UserControl
{
    public string ErrMessage = Resources.Resource.Admin_CommonSettings_InvalidOAuth;

    protected void Page_Load(object sender, EventArgs e)
    {
        var providers = AdvantShop.Core.AdvantshopConfigService.GetActivityAuthProviders();
        tableFacebook.Visible = !providers.ContainsKey("facebook") || providers["facebook"];
        tableTwitter.Visible = !providers.ContainsKey("twitter") || providers["twitter"];
        tableVk.Visible = !providers.ContainsKey("vkontakte") || providers["vkontakte"];
        tableMailru.Visible = !providers.ContainsKey("mail.ru") || providers["mail.ru"];
        tableGoogle.Visible = !providers.ContainsKey("google") || providers["google"];
        tableYandex.Visible = !providers.ContainsKey("yandex") || providers["yandex"];

        if (!IsPostBack)
            LoadData();
    }

    private void LoadData()
    {
        ckbGoogleActive.Checked = tableGoogle.Visible && SettingsOAuth.GoogleActive;
        ckbMailActive.Checked = tableMailru.Visible && SettingsOAuth.MailActive;
        ckbYandexActive.Checked = tableYandex.Visible && SettingsOAuth.YandexActive;
        ckbVKActive.Checked = tableVk.Visible && SettingsOAuth.VkontakteActive;
        ckbFacebookActive.Checked = tableFacebook.Visible && SettingsOAuth.FacebookActive;
        ckbTwitterActive.Checked = tableTwitter.Visible && SettingsOAuth.TwitterActive;

        txtVKAppId.Text = SettingsOAuth.VkontakeClientId;
        txtVKSecret.Text = SettingsOAuth.VkontakeSecret;

        txtFacebookClientId.Text = SettingsOAuth.FacebookClientId;
        txtFacebookApplicationSecret.Text = SettingsOAuth.FacebookApplicationSecret;

        txtTwitterConsumerKey.Text = SettingsOAuth.TwitterConsumerKey;
        txtTwitterConsumerSecret.Text = SettingsOAuth.TwitterConsumerSecret;
        txtTwitterAccessToken.Text = SettingsOAuth.TwitterAccessToken;
        txtTwitterAccessTokenSecret.Text = SettingsOAuth.TwitterAccessTokenSecret;

    }
    public bool SaveData()
    {
        SettingsOAuth.GoogleActive = ckbGoogleActive.Checked;
        SettingsOAuth.MailActive = ckbMailActive.Checked;
        SettingsOAuth.YandexActive = ckbYandexActive.Checked;
        SettingsOAuth.VkontakteActive = ckbVKActive.Checked;
        SettingsOAuth.FacebookActive = ckbFacebookActive.Checked;
        SettingsOAuth.TwitterActive = ckbTwitterActive.Checked;
        //vk
        SettingsOAuth.VkontakeClientId = txtVKAppId.Text;
        SettingsOAuth.VkontakeSecret = txtVKSecret.Text;
        //facebook
        SettingsOAuth.FacebookClientId = txtFacebookClientId.Text;
        SettingsOAuth.FacebookApplicationSecret = txtFacebookApplicationSecret.Text;
        //twitter
        SettingsOAuth.TwitterConsumerKey = txtTwitterConsumerKey.Text;
        SettingsOAuth.TwitterConsumerSecret = txtTwitterConsumerSecret.Text;
        SettingsOAuth.TwitterAccessToken = txtTwitterAccessToken.Text;
        SettingsOAuth.TwitterAccessTokenSecret = txtTwitterAccessTokenSecret.Text;

        LoadData();

        return true;
    }
}