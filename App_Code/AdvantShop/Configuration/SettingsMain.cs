//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Web;

namespace AdvantShop.Configuration
{
    public class SettingsMain
    {
        public static bool EnableUserOnline
        {
            get { return Convert.ToBoolean(SettingProvider.Items["EnableUserOnline"]); }
            set { SettingProvider.Items["EnableUserOnline"] = value.ToString(); }
        }

        public static bool EnableCaptcha
        {
            get { return Convert.ToBoolean(SettingProvider.Items["EnableCheckOrderConfirmCode"]); }
            set { SettingProvider.Items["EnableCheckOrderConfirmCode"] = value.ToString(); }
        }

        public static bool EnableAutoUpdateCurrencies
        {
            get { return Convert.ToBoolean(SettingProvider.Items["EnableAutoUpdateCurrencies"]); }
            set { SettingProvider.Items["EnableAutoUpdateCurrencies"] = value.ToString(); }
        }

        public static string LogoImageName
        {
            get { return SettingProvider.Items["MainPageLogoFileName"]; }
            set { SettingProvider.Items["MainPageLogoFileName"] = value; }
        }

        public static string FaviconImageName
        {
            get { return SettingProvider.Items["MainFaviconFileName"]; }
            set { SettingProvider.Items["MainFaviconFileName"] = value; }
        }

        public static string SiteUrl
        {
            get { return SettingProvider.Items["ShopURL"]; }
            set { SettingProvider.Items["ShopURL"] = value; }
        }

        public static string ShopName
        {
            get { return SettingProvider.Items["ShopName"]; }
            set { SettingProvider.Items["ShopName"] = value; }
        }

        public static string LogoImageAlt
        {
            get { return HttpUtility.HtmlEncode(SettingProvider.Items["ImageAltText"]); }
            set { SettingProvider.Items["ImageAltText"] = value; }
        }

        public static string Language
        {
            get { return SettingProvider.Items["Language"]; }
            set { SettingProvider.Items["Language"] = value; }
        }

        public static string AdminDateFormat
        {
            get { return SettingProvider.Items["AdminDateFormat"]; }
            set { SettingProvider.Items["AdminDateFormat"] = value; }
        }

        public static string ShortDateFormat
        {
            get { return SettingProvider.Items["ShortDateFormat"]; }
            set { SettingProvider.Items["ShortDateFormat"] = value; }
        }

        public static int SalerCountryId
        {
            get { return Convert.ToInt32(SettingProvider.Items["SalerCountryID"]); }
            set { SettingProvider.Items["SalerCountryID"] = value.ToString(); }
        }

        public static int SalerRegionId
        {
            get { return Convert.ToInt32(SettingProvider.Items["SalerRegionID"]); }
            set { SettingProvider.Items["SalerRegionID"] = value.ToString(); }
        }

        public static string Phone
        {
            get { return SettingProvider.Items["Phone"]; }
            set { SettingProvider.Items["Phone"] = value; }
        }

        public static string City
        {
            get { return SettingProvider.Items["City"]; }
            set { SettingProvider.Items["City"] = value; }
        }
    }
}