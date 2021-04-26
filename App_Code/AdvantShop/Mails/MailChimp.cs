//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace AdvantShop.Mails
{
    public class MailChimp
    {
        public enum TypeCompaign
        {
            regular,
            plaintext,
            absplit,
            rss,
            auto
        }

        public enum MemberStatus
        {
            subscribed,
            unsubscribed,
            cleaned,
            updated
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        public static MailChimpLists GetLists(string apiKey)
        {
            var lists = new MailChimpLists();

            if (apiKey.IsNullOrEmpty())
            {
                return lists;
            }

            var dc = GetApiDc(apiKey);
            if (dc.IsNullOrEmpty()) return null;

            try
            {
                var request = WebRequest.Create(string.Format("http://{0}.api.mailchimp.com/1.3/?method=lists&apikey={1}&filters={2}", dc, apiKey, string.Empty));
                request.Method = "GET";
                var response = request.GetResponse();

                var stream = response.GetResponseStream();
                if (stream != null)
                {
                    lists = JsonConvert.DeserializeObject<MailChimpLists>((new StreamReader(stream).ReadToEnd()));
                    if (lists != null && lists.Data != null)
                    {
                        lists.Data.Insert(0,
                                          new MailChimpList
                                          {
                                              Name = Resources.Resource.Admin_MailChimpSettings_NoListLinking,
                                              Id = "0"
                                          });
                    }
                    else
                    {
                        lists = new MailChimpLists { Total = 0, Data = new List<MailChimpList>() };
                    }
                }
            }
            catch (Exception)
            {

            }
            return lists;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="listid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static MailChimpListMembersLists GetListMembers(string apiKey, string listid, MemberStatus status = MemberStatus.subscribed)
        {
            var listMembersLists = new MailChimpListMembersLists();

            var dc = GetApiDc(apiKey);
            if (dc.IsNullOrEmpty()) return null;

            try
            {
                var request = WebRequest.Create(string.Format("http://{0}.api.mailchimp.com/1.3/?method=listMembers&apikey={1}&id={2}&status={3}", dc, apiKey, string.Empty, status));
                request.Method = "GET";
                var response = request.GetResponse();

                var stream = response.GetResponseStream();
                if (stream != null)
                {
                    listMembersLists = JsonConvert.DeserializeObject<MailChimpListMembersLists>((new StreamReader(stream).ReadToEnd()));
                    return listMembersLists;
                }
            }
            catch (Exception)
            { }

            return listMembersLists;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiKey"> </param>
        /// <param name="listId"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool SubscribeListMember(string apiKey, string listId, string email)
        {
            if (apiKey.IsNullOrEmpty() || listId.IsNullOrEmpty())
            {
                return false;
            }

            var dc = GetApiDc(apiKey);
            if (dc.IsNullOrEmpty()) return false;

            try
            {
                var request = WebRequest.Create(string.Format("http://{0}.api.mailchimp.com/1.3/?method=listSubscribe&apikey={1}&id={2}&email_address={3}&double_optin=false&send_welcome=false", dc, apiKey, listId, email));
                request.Method = "POST";
                var stream = request.GetResponse().GetResponseStream();
                if (stream != null)
                {

                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="listId"></param>
        /// <param name="emails"></param>
        /// <returns></returns>
        public static bool SubscribeListMember(string apiKey, string listId, List<string> emails)
        {
            if (apiKey.IsNullOrEmpty() || listId.IsNullOrEmpty() || emails.Count == 0)
            {
                return false;
            }

            var dc = GetApiDc(apiKey);
            if (dc.IsNullOrEmpty()) return false;

            try
            {
                foreach (var email in emails.Where(item => item != "admin"))
                {
                    var request = WebRequest.Create(string.Format("http://{0}.api.mailchimp.com/1.3/?method=listSubscribe&apikey={1}&id={2}&email_address={3}&double_optin=false&send_welcome=false", dc, apiKey, listId, email));
                    request.Method = "POST";
                    var stream = request.GetResponse().GetResponseStream();
                    if (stream != null)
                    {

                    }
                }
            }
            catch (Exception)
            { }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="listId"></param>
        /// <param name="email"></param>
        /// <param name="deleteMember"></param>
        /// <param name="sendGoodbye"></param>
        /// <param name="sendNotify"></param>
        /// <returns></returns>
        public static bool UnsubscribeListMember(string apiKey, string listId, string email, bool deleteMember = true, bool sendGoodbye = false, bool sendNotify = false)
        {
            if (apiKey.IsNullOrEmpty() || listId.IsNullOrEmpty())
            {
                return false;
            }

            var dc = GetApiDc(apiKey);
            if (dc.IsNullOrEmpty()) return false;

            var request = WebRequest.Create(string.Format("http://{0}.api.mailchimp.com/1.3/?method=listUnsubscribe&apikey={1}&id={2}&email_address={3}&delete_member={4}&send_goodbye={5}&send_notify={6}",
                dc,
                apiKey,
                listId,
                email,
                deleteMember,
                sendGoodbye,
                sendNotify));
            try
            {
                request.Method = "POST";
                var stream = request.GetResponse().GetResponseStream();
                if (stream != null)
                {

                }
            }
            catch (Exception)
            { }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="listId"></param>
        /// <returns></returns>
        public static bool UnsubscribeListMembers(string accountId, string listId)
        {
            if (accountId.IsNullOrEmpty() || listId.IsNullOrEmpty())
            {
                return false;
            }
            return UnsubscribeListMembers(accountId, listId, GetListMembers(accountId, listId));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="listId"></param>
        /// <param name="emails"></param>
        /// <param name="deleteMember"></param>
        /// <param name="sendGoodbye"></param>
        /// <param name="sendNotify"></param>
        /// <returns></returns>
        public static bool UnsubscribeListMembers(string apiKey, string listId, MailChimpListMembersLists lists, bool deleteMember = true, bool sendGoodbye = false, bool sendNotify = false)
        {
            if (apiKey.IsNullOrEmpty() || listId.IsNullOrEmpty() || lists.ListMembersList == null)
            {
                return false;
            }

            var dc = GetApiDc(apiKey);
            if (dc.IsNullOrEmpty()) return false;

            var request = WebRequest.Create(string.Format("http://{0}.api.mailchimp.com/1.3/?method=listUnsubscribe&apikey={1}&id={2}&emails={3}&delete_member={4}&send_goodbye={5}&send_notify={6}",
                dc,
                apiKey,
                listId,
                string.Format("[{0}]", lists.ListMembersList.Aggregate("", (current, list) => current + list.Email)),
                deleteMember,
                sendGoodbye,
                sendNotify));
            try
            {
                request.Method = "POST";
                var stream = request.GetResponse().GetResponseStream();
                if (stream != null)
                {

                }
            }
            catch (Exception)
            { }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="listId"></param>
        /// <param name="subject"></param>
        /// <param name="fromEmail"></param>
        /// <param name="fromName"></param>
        /// <param name="toName"></param>
        /// <param name="htmlContent"></param>
        /// <param name="textContent"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string CreateCampaign(string apiKey, string listId, string subject, string fromEmail, string fromName, string toName, string htmlContent, string textContent = "", TypeCompaign type = TypeCompaign.regular)
        {
            if (apiKey.IsNullOrEmpty() || listId.IsNullOrEmpty())
            {
                return string.Empty;
            }

            var dc = GetApiDc(apiKey);
            if (dc.IsNullOrEmpty()) return string.Empty;

            //Added by Evgeni to change to in Mailchimp Name
            fromName = @"ООО ''НесТулс'' и bosch-opt.by";
            //
            var compaignId = string.Empty;
            var request = (HttpWebRequest)WebRequest.Create(
                string.Format("http://{0}.api.mailchimp.com/1.3/?method=campaignCreate&apikey={1}&type={2}&options[list_id]={3}&options[subject]={4}&options[from_email]={5}&options[from_name]={6}&options[to_name]={7}&content[html]={8}&content[text]={9}",
                dc,
                apiKey,
                type.ToString().ToLower(),
                listId,
                subject,
                fromEmail,
                fromName,
                toName,
                HttpContext.Current.Server.UrlEncode(htmlContent),
                textContent));
            request.KeepAlive = false;
            request.Method = "POST";

            try
            {
                var stream = request.GetResponse().GetResponseStream();
                if (stream != null)
                {
                    compaignId = (new StreamReader(stream).ReadToEnd());
                }
            }
            catch (Exception)
            { }

            return compaignId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="listId"></param>
        /// <param name="subject"></param>
        /// <param name="fromEmail"></param>
        /// <param name="fromName"></param>
        /// <param name="toName"></param>
        /// <param name="htmlContent"></param>
        /// <param name="textContent"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool SendMail(string apiKey, string listId, string subject, string fromEmail, string fromName, string toName, string htmlContent, string textContent = "", TypeCompaign type = TypeCompaign.regular)
        {
            var compaignId = CreateCampaign(apiKey, listId, subject, fromEmail, fromName, toName, htmlContent, textContent, type);
            if (compaignId.Replace("\"", "").IsNotEmpty())
            {
                SendMail(apiKey, compaignId.Replace("\"", ""));
            }
            else
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="compaignId"></param>
        /// <returns></returns>
        public static bool SendMail(string apiKey, string compaignId)
        {
            if (apiKey.IsNullOrEmpty() || compaignId.IsNullOrEmpty())
            {
                return false;
            }

            var dc = GetApiDc(apiKey);
            if (dc.IsNullOrEmpty()) return false;

            var request = (HttpWebRequest)WebRequest.Create(string.Format("http://{0}.api.mailchimp.com/1.3/?method=campaignSendNow&apikey={1}&cid={2}", dc, apiKey, compaignId));
            request.KeepAlive = false;
            request.Method = "POST";
            try
            {
                var stream = request.GetResponse().GetResponseStream();
                if (stream != null)
                {
                    //var resp = (new StreamReader(stream).ReadToEnd());
                }
            }
            catch (Exception)
            { }

            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        public static MailChimpAccount GetAccountDetails(string apiKey)
        {
            if (apiKey.IsNullOrEmpty())
            {
                return null;
            }

            var dc = GetApiDc(apiKey);
            if (dc.IsNullOrEmpty()) return null;

            MailChimpAccount account = null;

            var request = (HttpWebRequest)WebRequest.Create(string.Format("http://{0}.api.mailchimp.com/1.3/?method=getAccountDetails&apikey={1}",
                dc,
                apiKey));
            request.KeepAlive = false;
            request.Method = "GET";
            try
            {
                var stream = request.GetResponse().GetResponseStream();
                if (stream != null)
                {
                    account = JsonConvert.DeserializeObject<MailChimpAccount>((new StreamReader(stream).ReadToEnd()));
                }
            }
            catch (Exception)
            { }

            return account;
        }

        private static string GetApiDc(string apiKey)
        {
            if (apiKey.IsNullOrEmpty() || !apiKey.Contains("-") || apiKey.LastIndexOf("-") + 1 >= apiKey.Length)
            {
                return string.Empty;
            }

            return apiKey.Substring(apiKey.LastIndexOf("-") + 1, apiKey.Length - apiKey.LastIndexOf("-") - 1);
        }

    }
}