//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using AdvantShop.Diagnostics;
using Twitterizer;

namespace AdvOpenAuth
{
    public class OAuthRequest
    {
        private List<FetchParameters> _fetchParameters;
        private List<ClaimParameters> _claimParameters;

        private Dictionary<Providers, string> _providerEndPoint = new Dictionary<Providers, string>
                                                                    {
                                                                        {Providers.Google, "https://www.google.com/accounts/o8/ud"},
                                                                        {Providers.Yandex, "http://openid.yandex.ru/server/"},
                                                                        {Providers.Rambler, "http://rambler.ru"},
                                                                       // {Providers.Mail, "http://{0}.id.mail.ru"},
                                                                       {Providers.Mail, "http://openid.mail.ru/login"}
                                                                       
                                                                    };

        public enum Providers
        {
            Empty,
            Google,
            Yandex,
            Rambler,
            Mail
        }

        private string _userId;
        public string UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        private Providers _provider;
        public Providers Provider
        {
            get { return _provider; }
            set { _provider = value; }
        }

        private string _redirectUrl;
        public string RedirectUrl
        {
            set
            {
                _redirectUrl = value;
            }
            get
            {
                return string.IsNullOrEmpty(_redirectUrl)
                                    ? HttpContext.Current.Request.Url.AbsoluteUri
                                    : _redirectUrl;
            }
        }

        public List<ClaimParameters> ClaimParameters;

        public List<FetchParameters> FetchParameters;

        public OAuthRequest()
        {
            _provider = Providers.Empty;
        }

        public void CreateRequest(FetchParameters parameters)
        {
            if (_provider == Providers.Empty)
                return;

            var requestString = new StringBuilder();
            requestString.Append(_providerEndPoint[_provider] + "?");
            requestString.Append(parameters.OpenidMode.RequestParameter());
            requestString.Append(parameters.OpenidNs.RequestParameter());
            requestString.Append(parameters.OpenidReturnTo.RequestParameter());
            requestString.Append(parameters.OpenidRealm.RequestParameter());
            requestString.Append(parameters.OpenidClaimedId.RequestParameter());
            requestString.Append(parameters.OpenidIdentity.RequestParameter());
            requestString.Append(parameters.OpenidNsAx.RequestParameter());
            requestString.Append(parameters.OpenidAxMode.RequestParameter());

            for (int i = 0; i < parameters.OpenidUserInformation.Count; i++)
            {
                requestString.Append(parameters.OpenidUserInformation[i]);
            }

            requestString.Append(parameters.OpenidAxRequired.RequestParameter());

            if (_provider == Providers.Google)
            {
                requestString.Append( string.Format("hl={0}", CultureInfo.CurrentCulture.TwoLetterISOLanguageName));
            }


            try
            {
                var request = WebRequest.Create(requestString.ToString());
                var respons = (HttpWebResponse)request.GetResponse();
                if (respons != null)
                {
                    HttpContext.Current.Response.Redirect(respons.ResponseUri.AbsoluteUri, true);
                }
            }
            catch (Exception ex)
            {
                if (!(ex is System.Threading.ThreadAbortException))
                {
                    Debug.LogError(ex);
                }
            }
        }

        public void CreateRequest(ClaimParameters parameters, bool withUserId)
        {
            if (_provider == Providers.Empty)
                return;

            var requestString = new StringBuilder();
            if (withUserId)
            {
                if (_provider == Providers.Mail)
                {
                    parameters.OpenidClaimedId.Value = string.Format("http://{0}.id.mail.ru", UserId);
                    parameters.OpenidIdentity.Value = string.Format("http://{0}.id.mail.ru", UserId);
                }
            }

            

            requestString.Append(_providerEndPoint[_provider] + "?");
            requestString.Append(parameters.OpenidMode.RequestParameter());
            requestString.Append(parameters.OpenidNs.RequestParameter());
            requestString.Append(parameters.OpenidReturnTo.RequestParameter());
            requestString.Append(parameters.OpenidRealm.RequestParameter());
            requestString.Append(parameters.OpenidClaimedId.RequestParameter());
            requestString.Append(parameters.OpenidIdentity.RequestParameter());
            requestString.Append(parameters.OpenidNsSreg.RequestParameter());

            requestString.Append(parameters.OpenidSregRequired.RequestParameter());

            requestString.Append(parameters.OpenidSregOptional.RequestParameter());

            
            try
            {
                var request = WebRequest.Create(requestString.ToString());
                var respons = (HttpWebResponse)request.GetResponse();

                if (respons != null)
                {
                    HttpContext.Current.Response.Redirect(respons.ResponseUri.AbsoluteUri, true);
                }
            }
            catch (Exception ex)
            {
                if (!(ex is System.Threading.ThreadAbortException))
                {
                    Debug.LogError(ex);
                }
            }
        }
    }
}