//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Web;
using AdvantShop.Configuration;
using AdvantShop.Customers;
using AdvantShop.Mails;
using AdvantShop.Controls;
using AdvantShop;
using AdvantShop.SEO;
using Resources;

public partial class Feedback : AdvantShopPage
{
    protected Customer curentCustomer = CustomerSession.CurrentCustomer;

    protected void btnSend_Click(object sender, EventArgs e)
    {
        bool boolIsValidPast = true;
        
        boolIsValidPast = txtSenderName.Text.IsNotEmpty() && txtMessage.Text.IsNotEmpty() &&
            txtEmail.Text.IsNotEmpty() && AdvantShop.Helpers.ValidationHelper.IsValidEmail(txtEmail.Text);

        if (!boolIsValidPast)
        {
            ShowMessage(Notify.NotifyType.Error, Resources.Resource.Client_Feedback_WrongData);
            validShield.TryNew();
            return;
        }

        if (!validShield.IsValid())
        {
            ShowMessage(Notify.NotifyType.Error, Resources.Resource.Client_Feedback_WrongCaptcha);
            validShield.TryNew();
            return;
        }

        try
        {
            var clsParam = new ClsMailParamOnFeedback
                                                  {
                                                      ShopUrl = SettingsMain.SiteUrl,
                                                      ShopName = SettingsMain.ShopName,
                                                      UserName = HttpUtility.HtmlEncode(txtSenderName.Text),
                                                      UserEmail = HttpUtility.HtmlEncode(txtEmail.Text),
                                                      UserPhone = HttpUtility.HtmlEncode(txtPhone.Text),
                                                      SubjectMessage = Resource.Client_Feedback_Header,
                                                      TextMessage = HttpUtility.HtmlEncode(txtMessage.Text)
                                                  };

            string message = SendMail.BuildMail(clsParam);
            SendMail.SendMailNow(SettingsMail.EmailForFeedback, Resource.Client_Feedback_Header + " // " + SettingsMain.SiteUrl, message, true);
            MultiView1.SetActiveView(ViewEmailSend);
        }
        catch (Exception ex)
        {
            AdvantShop.Diagnostics.Debug.LogError(ex);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SetMeta(new MetaInfo(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Client_Feedback_Header)), null);
        if (!Page.IsPostBack)
        {
            if (curentCustomer.RegistredUser)
            {
                txtSenderName.Text = curentCustomer.LastName + ' ' + curentCustomer.FirstName;
                txtEmail.Text = curentCustomer.EMail;
                txtPhone.Text = curentCustomer.Phone;
            }
        }
    }
}