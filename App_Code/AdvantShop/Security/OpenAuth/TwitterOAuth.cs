//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Web;
using AdvantShop.Configuration;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using Twitterizer;

namespace AdvOpenAuth
{
    public class TwitterOAuth
    {
        public static void TwitterOpenAuth()
        {
            try
            {
                //redirect url ?????? ????????? ? ??? ??? ???????? ? ??????????, ? ?????????? ? twitter ?????????? ????????? callbackurl!???????????!
                var authorizationTokens = OAuthUtility.GetRequestToken(SettingsOAuth.TwitterConsumerKey, SettingsOAuth.TwitterConsumerSecret, HttpContext.Current.Request.Url.ToString());
                
                var token = authorizationTokens.Token;
                HttpContext.Current.Response.Redirect(OAuthUtility.BuildAuthorizationUri(token).AbsoluteUri, true);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }

        public static bool TwitterGetUser(string oauthToken, string email)
        {
            return TwitterGetUser(oauthToken, string.Empty, email);
        }

        public static bool TwitterGetUser(string oauthToken, string verifier, string email)
        {
            try
            {
                OAuthTokenResponse tokens = OAuthUtility.GetAccessToken(SettingsOAuth.TwitterConsumerKey, SettingsOAuth.TwitterConsumerSecret, oauthToken, verifier);
                var userName = tokens.ScreenName;
                var userId = tokens.UserId;

                OAuthResponce.AuthOrRegCustomer(new Customer
                {
                    EMail = userId + "@temp.twitter",
                    FirstName = userName,
                    CustomerGroupId = 1,
                    Password = Guid.NewGuid().ToString()
                }, userId.ToString());
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                return false;
            }
            return true;
        }
    }
}