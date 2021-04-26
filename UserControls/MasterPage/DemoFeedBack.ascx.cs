using System;
using System.Web;
using AdvantShop;
using AdvantShop.Configuration;
using AdvantShop.Controls;
using AdvantShop.Mails;

public partial class UserControls_MasterPage_DemoFeedback : System.Web.UI.UserControl
{

    protected void Page_Load(object sender, EventArgs e)
    {
        //Changed By Evgeni
        //this.Visible = Demo.IsDemoEnabled || Trial.IsTrialEnabled;
        feedBackCaptcha.Visible = this.Visible;

        txtMessage.Text = "Хочу получать письма с Вашими акциями!";
    }

    protected void btnSend_Click(object sender, EventArgs e)
    {
        //Changed By Evgeni
        if (txtEmail.Text == "" || txtMessage.Text == "" || !feedBackCaptcha.IsValid())
        {
            ((AdvantShopPage)Page).ShowMessage(Notify.NotifyType.Error, Resources.Resource.Client_Feedback_WrongData);
            feedBackCaptcha.TryNew();
            return;
        }


        string message = String.Format("Имя:{0} <br/>E-mail:{1}<br/>Текст сообщения:{2}",
                                       HttpUtility.HtmlEncode(txtSenderName.Text), HttpUtility.HtmlEncode(txtEmail.Text),
                                       HttpUtility.HtmlEncode(txtMessage.Text));

        SendMail.SendMailNow("info@bosch-opt.by, GKovalev@gmail.com", "Узнать о скидках: " + SettingsMain.SiteUrl, message, true);

        //Added by Evgeni to Subscribe Mailchimp
        //dc97a90a7e == Получатели писем с акциями (сбор с формы с сайта)
        AdvantShop.Mails.MailChimp.SubscribeListMember(SettingsMailChimp.MailChimpId, "dc97a90a7e", txtEmail.Text);


        txtSenderName.Text = "";
        txtEmail.Text = "";
        txtMessage.Text = "";

        ((AdvantShopPage)Page).ShowMessage(Notify.NotifyType.Notice,Resources.Resource.Client_Feedback_MessageSent);
        feedBackCaptcha.TryNew();
    }

    protected string GetCssClass()
    {
        //Changed by Evgeni
            return "link-feedback-demo-" + SettingsMain.Language;
    }
}