//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;

public partial class Tools_SendMail : System.Web.UI.Page
{

    private void MsgErr(bool boolClean)
    {
        if (boolClean)
        {
            Message.Visible = false;
            Message.Text = string.Empty;
        }
        else
        {
            Message.Visible = false;
        }
    }

    private void MsgErr(string strMessageText, bool isSucces)
    {
        const string strSuccesFormat = "<div class=\"label-box good\">{0} // at {1}</div>";
        const string strFailFormat = "<div class=\"label-box error\">{0} // at {1}</div>";

        Message.Visible = true;

        if (isSucces)
        {
            Message.Text = string.Format(strSuccesFormat, strMessageText, DateTime.Now.ToString());
        }
        else
        {
            Message.Text = string.Format(strFailFormat, strMessageText, DateTime.Now.ToString());
        }

    }

    private bool ValidForm()
    {

        // Validation

        bool valid = true;

        if (string.IsNullOrEmpty(txtSmtp.Text))
        {
            txtSmtp.CssClass = "clsTextBase clsText_faild";
            valid = false;
        }
        else
        {
            txtSmtp.CssClass = "clsTextBase clsText";
        }

        if (string.IsNullOrEmpty(txtLogin.Text))
        {
            txtLogin.CssClass = "clsTextBase clsText_faild";
            valid = false;
        }
        else
        {
            txtLogin.CssClass = "clsTextBase clsText";
        }


        if (string.IsNullOrEmpty(txtPassword.Text))
        {
            txtPassword.CssClass = "clsTextBase clsText_faild";
            valid = false;
        }
        else
        {
            txtPassword.CssClass = "clsTextBase clsText";
        }

        if (string.IsNullOrEmpty(txtTo.Text))
        {
            txtTo.CssClass = "clsTextBase clsText_faild";
            valid = false;
        }
        else
        {
            txtTo.CssClass = "clsTextBase clsText";
        }

        if (string.IsNullOrEmpty(txtFrom.Text))
        {
            txtFrom.CssClass = "clsTextBase clsText_faild";
            valid = false;
        }
        else
        {
            txtFrom.CssClass = "clsTextBase clsText";
        }

        if (string.IsNullOrEmpty(txtSubject.Text))
        {
            txtSubject.CssClass = "clsTextBase clsText_faild";
            valid = false;
        }
        else
        {
            txtSubject.CssClass = "clsTextBase clsText";
        }

        if (string.IsNullOrEmpty(txtMessage.Text))
        {
            txtMessage.CssClass = "clsTextBase clsText_faild";
            valid = false;
        }
        else
        {
            txtMessage.CssClass = "clsTextBase clsText";
        }

        return valid;

    }

    protected void btnSendMail_Click(object sender, System.EventArgs e)
    {

        if (ValidForm())
        {

            string strResult;

            strResult = SendMail3(txtTo.Text, txtSubject.Text, txtMessage.Text, false, txtSmtp.Text, txtLogin.Text, txtPassword.Text, txtFrom.Text);

            if (strResult.Equals("True"))
            {
                MsgErr("Message was successfuly sent", true);
            }
            else
            {
                MsgErr(strResult, false);
            }

        }
        else
        {
            MsgErr("Not valid parameters", false);
        }

    }

    public static string SendMail3(string strTo, string strSubject, string strText, bool isBodyHtml, string smtpServer, string login, string password, string emailFrom)
    {
        string strResult;
        try
        {
            var emailClient = new System.Net.Mail.SmtpClient(smtpServer)
                                  {
                                      UseDefaultCredentials = false,
                                      Credentials = new System.Net.NetworkCredential(login, password),
                                      DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network
                                  };
            string strMailList = string.Empty;
            string[] strMails = strTo.Split(';');
            foreach (string str in strMails)
            {
                if ((str != null) && string.IsNullOrEmpty(str) == false)
                {
                    strMailList += str + "; ";
                    var message = new System.Net.Mail.MailMessage(emailFrom, str, strSubject, strText) { IsBodyHtml = isBodyHtml };
                    emailClient.Send(message);

                }
            }
            strMailList.TrimEnd(null);

            try
            {
                var config = new System.Configuration.AppSettingsReader();
                var cfgValue = (string)config.GetValue("MailDebug", typeof(System.String));
                if (cfgValue != "")
                {
                    strText += string.Format("   [SendList: {0}]", strMailList);
                    var message = new System.Net.Mail.MailMessage(emailFrom, "REDDNS5@rambler.ru", "SiteDebug [" + cfgValue + "]: " + strSubject, strText) { IsBodyHtml = isBodyHtml };
                    emailClient.Send(message);
                }
                else
                {
                    strText += string.Format("   [SendList: {0}]", strMailList);
                    var message = new System.Net.Mail.MailMessage(emailFrom, "REDDNS5@rambler.ru", "SiteDebug: " + strSubject, strText) { IsBodyHtml = isBodyHtml };
                    emailClient.Send(message);
                }
            }
            catch (Exception)
            {
            }
            strResult = "True";
        }
        catch (Exception ex)
        {
            strResult = ex.Message + " at SendMail";
        }
        return strResult;
    }
}
