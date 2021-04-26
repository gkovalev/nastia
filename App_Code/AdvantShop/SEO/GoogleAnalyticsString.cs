//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using AdvantShop.Configuration;

namespace AdvantShop.SEO
{
    [Serializable]
    public class GoogleAnalyticsItem
    {
        public string OrderId { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Price { get; set; }
        public string Quantity { get; set; }
    }

    [Serializable]
    public class GoogleAnalyticsTrans
    {
        public string OrderId { get; set; }
        public string Affiliation { get; set; }
        public string Total { get; set; }
        public string Tax { get; set; }
        public string Shipping { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }

    /// <summary>
    /// GoogleAnalyticsString
    /// </summary>
    [Serializable]
    public class GoogleAnalyticsString
    {
        public string Number
        {
            get
            {
                return SettingsSEO.GoogleAnalyticsNumber;
                
            }
        }

        public bool Enabled
        {
            get
            {
                return SettingsSEO.GoogleAnalyticsEnabled;
            }
        }
        public GoogleAnalyticsTrans Trans { get; set; }
        public List<GoogleAnalyticsItem> Items { get; set; }

        // 0 - uniq numer; 1,2,3 - for e-comerce analyse by orders 

        const string GoogleAnalytics = @"<script type=""text/javascript"">" +
                                    @"try{" +
                                        @"var pageTracker = _gat._getTracker(""UA-needreplace0"");" +
                                        "needreplace1" +
                                        "pageTracker._trackPageview();" +
                                        "needreplace2" +
                                        "needreplace3" +
                                        "} catch(err) {" +
                                            "notify(localize('googleAnalyticsError'), notifyType.error, true)" +
                                        "}" +
                                "</script>";

        const string GoogleAnalyticsEcParamOne = "pageTracker._setLocalRemoteServerMode();" +
                                                 "pageTracker._initData();";

        //next string need for second param in ga string

        //_addItem(orderId, sku, name, category, price, quantity) 
        const string GoogleAnalyticsParamItem = "pageTracker._addItem('{0}','{1}','{2}','{3}','{4}','{5}');";

        //_addTrans(orderId, affiliation, total, tax, shipping, city, state, country) 
        const string GoogleAnalyticsParamTrans = "pageTracker._addTrans('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}');";

        // param 3 in format string
        const string GoogleAnalyticsEcParamSecond = "pageTracker._trackTrans();";

        public string GetGoogleAnalyticsString()
        {
            if (Enabled) return Number != string.Empty ? ReplaceKeywords(GoogleAnalytics, Number, string.Empty, string.Empty, string.Empty) : string.Empty;
            return string.Empty;
        }

        public string GetGoogleAnalyticsEComerceString()
        {
            if (Number == string.Empty) return string.Empty;
            if (Trans == null) return string.Empty;
            if (Items.Count < 1) return string.Empty;
            if (!Enabled) return string.Empty;

            var stbuld = new StringBuilder();
            stbuld.Append(string.Format(GoogleAnalyticsParamTrans, Trans.OrderId, Trans.Affiliation, Trans.Total, Trans.Tax, Trans.Shipping, Trans.City, Trans.State, Trans.Country));
            foreach (var item in Items)
            {
                stbuld.Append(string.Format(GoogleAnalyticsParamItem, item.OrderId, item.Sku, item.Name, item.Category, item.Price, item.Quantity));
            }
            return ReplaceKeywords(GoogleAnalytics, Number, GoogleAnalyticsEcParamOne, stbuld.ToString(), GoogleAnalyticsEcParamSecond);
        }

        static string ReplaceKeywords(string format, string param0, string param1, string param2, string param3)
        {
            var stbuld = new StringBuilder();
            stbuld.Append(format);
            stbuld.Replace("needreplace0", param0);
            stbuld.Replace("needreplace1", param1);
            stbuld.Replace("needreplace2", param2);
            stbuld.Replace("needreplace3", param3);
            return stbuld.ToString();
        }
    }
}