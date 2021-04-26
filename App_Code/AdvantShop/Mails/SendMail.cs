//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Threading;
using AdvantShop.Configuration;
using AdvantShop.Core;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.Helpers;

namespace AdvantShop.Mails
{
    public class SendMail
    {
        //Changed by EVgeni to add attachments to email
        public static bool SendMailThread(string strTo, string strSubject, string strText, bool isBodyHtml, string smtpServer, string login, string password, int port, string emailFrom, bool ssl, string attachement = "")
        {
            return (SendMailThreadStringResult(strTo, strSubject, strText, isBodyHtml, smtpServer, login, password, port, emailFrom, ssl,attachement) == "True");
        }
        //Changed by EVgeni to add attachments to email
        public static string SendMailThreadStringResult(string strTo, string strSubject, string strText, bool isBodyHtml, string smtpServer, string login, string password, int port, string emailFrom, bool ssl, string attachement = "")
        {

            string strResult = "True";

            try
            {
                using (var emailClient = new SmtpClient(smtpServer)
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(login, password),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Port = port,
                    EnableSsl = ssl
                })
                {
                    string[] strMails = strTo.Split(';');
                    foreach (string strEmail in strMails)
                    {
                        string strE = strEmail.Trim();
                        if (string.IsNullOrEmpty(strE)) continue;

                        if (!ValidationHelper.IsValidEmail(strE)) continue;
                        using (var message = new MailMessage(emailFrom, strE, strSubject, strText))
                        {
                            message.IsBodyHtml = isBodyHtml;
                            if (attachement != "")
                            {
                                message.Attachments.Add(new Attachment(attachement));
                            }
                            emailClient.Send(message);
                            
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                strResult = ex.Message;
                Debug.LogError(ex, false);
            }

            return strResult;
        }

        //Changed by EVgeni to add attachments to email
        public static bool SendMailNow(string strTo, string strSubject, string strText, bool isBodyHtml, string setSmtpServer, int setPort, string setLogin, string setPassword, string setEmailFrom, bool setSsl, string attachement = "")
        {

            int workerThreads;
            int asyncIoThreads;
            ThreadPool.GetAvailableThreads(out workerThreads, out asyncIoThreads);
            if (workerThreads != 0)
                ThreadPool.QueueUserWorkItem(a => SendMailThread(strTo, strSubject, strText, isBodyHtml, setSmtpServer, setLogin, setPassword, setPort, setEmailFrom, setSsl,attachement));
            else
                new Thread(a => SendMailThread(strTo, strSubject, strText, isBodyHtml, setSmtpServer, setLogin, setPassword, setPort, setEmailFrom, setSsl,attachement)).Start();

            return true;
        }

        //Changed by EVgeni to add attachments to email
        public static bool SendMailNow(string strTo, string strSubject, string strText, bool isBodyHtml, string attachement = "")
        {
            string smtp = SettingsMail.SMTP;
            string login = SettingsMail.Login;
            string password = SettingsMail.Password;
            int port = SettingsMail.Port;
            string email = SettingsMail.From;
            bool ssl = SettingsMail.SSL;
            return SendMailNow(strTo, strSubject, strText, isBodyHtml, smtp, port, login, password, email, ssl, attachement);
        }

        #region  BuildMail

        private static string GetMailFormatByType(MailType type)
        {
            return SQLDataAccess.ExecuteScalar<string>("[Settings].[sp_GetMailFormatByID]", CommandType.StoredProcedure, new SqlParameter("@FormatType", (int)type));
        }

        public static string BuildMail<T>(T clsParam) where T : ClsMailParam
        {
            var strResult = string.Empty;
            var logo = String.Format("<p><img src=\"{0}\" alt=\"{1}\" title=\"{2}\" /></p>", SettingsMain.SiteUrl.Trim('/') + '/'
                + FoldersHelper.GetPath(FolderType.Pictures, SettingsMain.LogoImageName, false), SettingsMain.ShopName, SettingsMain.ShopName);
            strResult = GetMailFormatByType(clsParam.Type);
            strResult = logo + clsParam.FormatString(strResult);
                        return strResult;
        }
        #endregion
    }
}