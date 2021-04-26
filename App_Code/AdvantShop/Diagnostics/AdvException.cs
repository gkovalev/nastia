//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Web;
using System.Web.SessionState;
using Newtonsoft.Json;

namespace AdvantShop.Diagnostics
{
    public class AdvException
    {
        public class CommonExceptionData
        {
            public string InnerExceptionMessage { get; set; }
            public string InnerExceptionStackTrace { get; set; }

            public string ManualMessage { get; set; }

            public string ExceptionMessage { get; set; }
            public string ExceptionStackTrace { get; set; }

            public Dictionary<string, string> Parameters { get; set; }


            public CommonExceptionData()
            {
                Parameters = new Dictionary<string, string>();
            }

            public CommonExceptionData(Exception exception)
            {
                ExceptionMessage = exception.Message;
                ExceptionStackTrace = exception.StackTrace;
                if (exception.InnerException != null)
                {
                    InnerExceptionMessage = exception.InnerException.Message;
                    InnerExceptionStackTrace = exception.InnerException.StackTrace;
                }

                Parameters = new Dictionary<string, string>();
                foreach (var key in exception.Data.Keys)
                {
                    Parameters.Add(key.ToString(), exception.Data[key].ToString());
                }
            }
        }

        public class RequestExceptionData
        {
            public Dictionary<string, string> ColectionData;

            public RequestExceptionData()
            {
                ColectionData = new Dictionary<string, string>();
            }

            public RequestExceptionData(HttpRequest httpRequest)
            {
                ColectionData = new Dictionary<string, string>();
                ColectionData.Add("IsLocal", httpRequest.IsLocal.ToString(CultureInfo.InvariantCulture));
                ColectionData.Add("HttpMethod", httpRequest.HttpMethod);
                ColectionData.Add("RequestType", httpRequest.RequestType);
                ColectionData.Add("ContentLength", httpRequest.ContentLength.ToString(CultureInfo.InvariantCulture));
                ColectionData.Add("ContentEncoding", httpRequest.ContentEncoding.ToString());
                ColectionData.Add("FilePath", httpRequest.FilePath);
                ColectionData.Add("UserAgent", httpRequest.UserAgent);
                ColectionData.Add("UserHostName", httpRequest.UserHostName);
                ColectionData.Add("UserHostAddress", httpRequest.UserHostAddress);
                ColectionData.Add("RawUrl", httpRequest.RawUrl);
                ColectionData.Add("Url", httpRequest.Url.ToString());
                ColectionData.Add("UrlReferrer", httpRequest.UrlReferrer != null ? httpRequest.UrlReferrer.ToString() : "N/A");

                for (int i = 0; i < httpRequest.Headers.Count; i++)
                {
                    ColectionData.Add(httpRequest.Headers.GetKey(i), httpRequest.Headers[i]);
                }

                for (int i = 0; i < httpRequest.ServerVariables.Count; i++)
                {
                    ColectionData.Add(httpRequest.ServerVariables.GetKey(i), httpRequest.ServerVariables[i]);
                }

                //var properties = httpRequest.GetType().GetProperties();
                //foreach (var p in properties)
                //{

                //    if (p.Name == "Params" || p.Name == "Item" || p.Name == "HttpChannelBinding")
                //    {
                //        continue;
                //    }

                //    var o = p.GetValue(httpRequest, null);
                //    if (o is NameValueCollection)
                //    {
                //        foreach (string key in ((NameValueCollection)o))
                //        {
                //            if (key != "ALL_HTTP" && key != "ALL_RAW")
                //            {
                //                ColectionData.Add(key, httpRequest.Params[key]);
                //            }
                //        }
                //    }
                //    else
                //    {
                //        ColectionData.Add(p.Name, o != null ? o.ToString() : "n\a");
                //    }
                //}
            }
        }

        public class BrowserExceptionData
        {
            public Dictionary<string, string> ColectionData;

            public BrowserExceptionData()
            {
                ColectionData = new Dictionary<string, string>();
            }

            public BrowserExceptionData(HttpBrowserCapabilities browser)
            {
                ColectionData = new Dictionary<string, string>();
                ColectionData.Add("Type", browser.Type);
                ColectionData.Add("Browser", browser.Browser);
                ColectionData.Add("Version", browser.Version);
                ColectionData.Add("Platform", browser.Platform);
                ColectionData.Add("EcmaScriptVersion", browser.EcmaScriptVersion.ToString());
                ColectionData.Add("MSDomVersion", browser.MSDomVersion.ToString());
                ColectionData.Add("Beta", browser.Beta.ToString(CultureInfo.InvariantCulture));
                ColectionData.Add("Crawler", browser.Crawler.ToString(CultureInfo.InvariantCulture));
                ColectionData.Add("Win32", browser.Win32.ToString(CultureInfo.InvariantCulture));
                ColectionData.Add("Win16", browser.Win16.ToString(CultureInfo.InvariantCulture));
                ColectionData.Add("Frames", browser.Frames.ToString(CultureInfo.InvariantCulture));
                ColectionData.Add("Tables", browser.Tables.ToString(CultureInfo.InvariantCulture));
                ColectionData.Add("Cookies", browser.Cookies.ToString(CultureInfo.InvariantCulture));
                ColectionData.Add("VBScript", browser.VBScript.ToString(CultureInfo.InvariantCulture));
                ColectionData.Add("JavaScript", browser.JavaScript.ToString(CultureInfo.InvariantCulture));
                ColectionData.Add("JavaApplets", browser.JavaApplets.ToString(CultureInfo.InvariantCulture));
                ColectionData.Add("JScriptVersion", browser.JScriptVersion.ToString());
                ColectionData.Add("ActiveXControls", browser.ActiveXControls.ToString(CultureInfo.InvariantCulture));
                ColectionData.Add("BackgroundSounds", browser.BackgroundSounds.ToString(CultureInfo.InvariantCulture));
                ColectionData.Add("CDF", browser.CDF.ToString(CultureInfo.InvariantCulture));
                ColectionData.Add("IsMobileDevice", browser.IsMobileDevice.ToString(CultureInfo.InvariantCulture));
                ColectionData.Add("MobileDeviceManufacturer", browser.MobileDeviceManufacturer);
                ColectionData.Add("MobileDeviceModel", browser.MobileDeviceModel);
                ColectionData.Add("ScreenPixelsHeight", browser.ScreenPixelsHeight.ToString(CultureInfo.InvariantCulture));
                ColectionData.Add("ScreenPixelsWidth", browser.ScreenPixelsWidth.ToString(CultureInfo.InvariantCulture));
                ColectionData.Add("ScreenBitDepth", browser.ScreenBitDepth.ToString(CultureInfo.InvariantCulture));
                ColectionData.Add("IsColor", browser.IsColor.ToString(CultureInfo.InvariantCulture));
                ColectionData.Add("InputType", browser.InputType);

                //var properties = browser.GetType().GetProperties();
                //foreach (var p in properties)
                //{
                //    if (p.Name == "Item" || p.Name == "HtmlTextWriter" || p.Name == "PreferredRequestEncoding" || p.Name == "PreferredResponseEncoding" || p.Name == "RequiredMetaTagNameValue")
                //    {
                //        continue;
                //    }
                //    var o = p.GetValue(browser, null);
                //    ColectionData.Add(p.Name, o.ToString());
                //}
            }
        }

        public class SessionExceptionData
        {
            public Dictionary<string, string> ColectionData;

            public SessionExceptionData()
            {
                ColectionData = new Dictionary<string, string>();
            }

            public SessionExceptionData(HttpSessionState session)
            {
                ColectionData = new Dictionary<string, string>();
                ColectionData.Add("SessionID", session.SessionID);
                ColectionData.Add("Timeout", session.Timeout.ToString(CultureInfo.InvariantCulture));
                ColectionData.Add("IsNewSession", session.IsNewSession.ToString(CultureInfo.InvariantCulture));
                ColectionData.Add("IsCookieless", session.IsCookieless.ToString(CultureInfo.InvariantCulture));
                ColectionData.Add("Mode", session.Mode.ToString());
                ColectionData.Add("CookieMode", session.CookieMode.ToString());

                //var properties = session.GetType().GetProperties();
                //foreach (var p in properties)
                //{
                //    //skip error
                //    if (p.Name == "Item")
                //        continue;

                //    var o = p.GetValue(session, null);
                //    ColectionData.Add(p.Name, o.ToString());
                //}
                foreach (string key in session.Keys)
                {
                    ColectionData.Add(key, session[key].ToString());
                }
            }
        }

        public CommonExceptionData ExceptionData { get; set; }
        public RequestExceptionData RequestData { get; set; }
        public BrowserExceptionData BrowserData { get; set; }
        public SessionExceptionData SessionData { get; set; }

        public AdvException()
        {
            ExceptionData = new CommonExceptionData();
            RequestData = new RequestExceptionData();
            BrowserData = new BrowserExceptionData();
            SessionData = new SessionExceptionData();
        }

        public AdvException(Exception exception)
        {
            ExceptionData = new CommonExceptionData(exception);

            var context = HttpContext.Current;
            if (context != null)
            {
                RequestData = new RequestExceptionData(context.Request);
                BrowserData = new BrowserExceptionData(context.Request.Browser);
                if (context.Session != null)
                    SessionData = new SessionExceptionData(context.Session);
            }
        }

        public string ToJsonString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static AdvException GetFromJsonString(string str)
        {
            return JsonConvert.DeserializeObject<AdvException>(str);
        }
    }
}