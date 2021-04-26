using System;
using System.Globalization;
using AdvantShop.Configuration;
using AdvantShop.Mails;
using Resources;

//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

public partial class Admin_SendMessage : System.Web.UI.Page
{
    protected override void InitializeCulture()
    {
        AdvantShop.Localization.Culture.InitializeCulture();
    }

    private void MsgErr(bool clean)
    {
        if (clean)
        {
            lblError.Visible = false;
            lblError.Text = string.Empty;
        }
        else
        {
            lblError.Visible = false;
        }
    }

    private void MsgErr(string messageText)
    {
        lblError.Visible = true;
        lblError.Text = @"<br/>" + messageText + @"<br/>";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        fckMailContent.Language = CultureInfo.CurrentCulture.ToString();
        MsgErr(true);
        lblInfo.Text = string.Empty;
        Page.Title = string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_SendMessage_Title);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!SettingsMailChimp.MailChimpActive)
        {
            mvSendingMessages.SetActiveView(vErrorForm);
        }
        else if (!IsPostBack)
        {
            mvSendingMessages.SetActiveView(vSendForm);
        }

    }

    protected void btnSend_Click(object sender, EventArgs e)
    {
        if (!IsValidData())
            return;
        if (SettingsMailChimp.MailChimpActive)
        {
            if (rbToAll.Checked)
            {
                MailChimp.SendMail(SettingsMailChimp.MailChimpId, SettingsMailChimp.MailChimpRegUsersList, txtTitle.Text,
                                   SettingsMail.From, SettingsMain.ShopName, string.Empty, fckMailContent.Text);
                MailChimp.SendMail(SettingsMailChimp.MailChimpId, SettingsMailChimp.MailChimpUnRegUsersList,
                               txtTitle.Text, SettingsMail.From, SettingsMain.ShopName, string.Empty,
                               fckMailContent.Text);
            }
            if (rbToReg.Checked)
            {
                MailChimp.SendMail(SettingsMailChimp.MailChimpId, SettingsMailChimp.MailChimpRegUsersList, txtTitle.Text,
                                   SettingsMail.From, SettingsMain.ShopName, string.Empty, fckMailContent.Text);
            }
            if (rbToUnReg.Checked)
            {
                MailChimp.SendMail(SettingsMailChimp.MailChimpId, SettingsMailChimp.MailChimpUnRegUsersList,
                                   txtTitle.Text, SettingsMail.From, SettingsMain.ShopName, string.Empty,
                                   fckMailContent.Text);
            }
        }
        mvSendingMessages.SetActiveView(vFinishForm);
    }

    private bool IsValidData()
    {
        if ((txtTitle.Text.IndexOf(">") != -1) || (txtTitle.Text.IndexOf("<") != -1))
        {
            MsgErr(Resource.Admin_SendMessage_HtmlNotSupported);
            return false;
        }

        if (string.IsNullOrEmpty(txtTitle.Text))
        {
            MsgErr(Resource.Admin_SendMessage_NoTitle);
            return false;
        }

        if (string.IsNullOrEmpty(fckMailContent.Text))
        {
            MsgErr(Resource.Admin_SendMessage_NoEmailText);
            return false;
        }

        return true;
    }
}