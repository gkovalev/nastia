#region Apache Notice
/*****************************************************************************
 * Date: November 11, 2007
 * 
 * Modelus Log4Net Extensions
 * Copyright (C) 2007 - Modelus LLC
 *  
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 ********************************************************************************/
#endregion

using System.Net;
using System.Net.Mail;
using log4net.Appender;

namespace AdvantShop.Diagnostics
{
    /// <summary>Sends an HTML email when logging event occurs</summary>
    public class HtmlSmtpAppender : SmtpAppender
    {
        /// <summary>Sends an email message</summary>
        protected override void SendEmail(string body)
        {
            using (var item = CreateSmtpClient())
            {
                using (var msg = CreateMailMessage(body))
                    item.Send(msg);
            }
        }

        /// <summary>Creats new SMTP client based on the properties set in the configuration file</summary>
        private SmtpClient CreateSmtpClient()
        {
            var smtpClient = new SmtpClient
                                 {
                                     Host = SmtpHost,
                                     Port = Port,
                                     DeliveryMethod = SmtpDeliveryMethod.Network,
                                     Credentials =
                                         string.IsNullOrEmpty(Username)
                                             ? CredentialCache.DefaultNetworkCredentials
                                             : new NetworkCredential(Username, Password)
                                 };


            return smtpClient;
        }

        /// <summary>Creats new mail message based on the properties set in the configuration file</summary>
        private MailMessage CreateMailMessage(string body)
        {
            var mailMessage = new MailMessage { From = new MailAddress(From) };
            mailMessage.To.Add(To);
            mailMessage.Subject = Subject;
            mailMessage.Body = body;
            mailMessage.Priority = Priority;
            mailMessage.IsBodyHtml = true;

            return mailMessage;
        }
    }
}
