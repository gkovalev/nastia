//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop.SEO;

namespace AdvantShop.Configuration
{
    public class SettingsSEO
    {
        public static string GoogleAnalyticsNumber
        {
            get { return SettingProvider.Items["GoogleAnalyticsNumber"]; }
            set { SettingProvider.Items["GoogleAnalyticsNumber"] = value; }
        }

        public static bool Enabled301Redirects
        {
            get { return Convert.ToBoolean(SettingProvider.Items["Enabled301Redirects"]); }
            set { SettingProvider.Items["Enabled301Redirects"] = value.ToString(); }
        }

        public static bool GoogleAnalyticsEnabled
        {
            get { return Convert.ToBoolean(SettingProvider.Items["GoogleAnalyticsEnabled"]); }
            set { SettingProvider.Items["GoogleAnalyticsEnabled"] = value.ToString(); }
        }

        public static string DefaultMetaTitle
        {
            get { return SettingProvider.Items[MetaType.Default + "Title"]; }
            set { SettingProvider.Items[MetaType.Default + "Title"] = value; }
        }

        public static string DefaultMetaKeywords
        {
            get { return SettingProvider.Items[MetaType.Default + "MetaKeywords"]; }
            set { SettingProvider.Items[MetaType.Default + "MetaKeywords"] = value; }
        }
        public static string DefaultMetaDescription
        {
            get { return SettingProvider.Items[MetaType.Default + "MetaDescription"]; }
            set { SettingProvider.Items[MetaType.Default + "MetaDescription"] = value; }
        }

        public static string ProductMetaTitle
        {
            get { return SettingProvider.Items[MetaType.Product + "Title"]; }
            set { SettingProvider.Items[MetaType.Product + "Title"] = value; }
        }
        public static string ProductMetaKeywords
        {
            get { return SettingProvider.Items[MetaType.Product + "MetaKeywords"]; }
            set { SettingProvider.Items[MetaType.Product + "MetaKeywords"] = value; }
        }
        public static string ProductMetaDescription
        {
            get { return SettingProvider.Items[MetaType.Product + "MetaDescription"]; }
            set { SettingProvider.Items[MetaType.Product + "MetaDescription"] = value; }
        }

        public static string CategoryMetaTitle
        {
            get { return SettingProvider.Items[MetaType.Category + "Title"]; }
            set { SettingProvider.Items[MetaType.Category + "Title"] = value; }
        }
        public static string CategoryMetaKeywords
        {
            get { return SettingProvider.Items[MetaType.Category + "MetaKeywords"]; }
            set { SettingProvider.Items[MetaType.Category + "MetaKeywords"] = value; }
        }
        public static string CategoryMetaDescription
        {
            get { return SettingProvider.Items[MetaType.Category + "MetaDescription"]; }
            set { SettingProvider.Items[MetaType.Category + "MetaDescription"] = value; }
        }

        public static string StaticPageMetaTitle
        {
            get { return SettingProvider.Items[MetaType.StaticPage + "Title"]; }
            set { SettingProvider.Items[MetaType.StaticPage + "Title"] = value; }
        }
        public static string StaticPageMetaKeywords
        {
            get { return SettingProvider.Items[MetaType.StaticPage + "MetaKeywords"]; }
            set { SettingProvider.Items[MetaType.StaticPage + "MetaKeywords"] = value; }
        }
        public static string StaticPageMetaDescription
        {
            get { return SettingProvider.Items[MetaType.StaticPage + "MetaDescription"]; }
            set { SettingProvider.Items[MetaType.StaticPage + "MetaDescription"] = value; }
        }


        public static string NewsMetaTitle
        {
            get { return SettingProvider.Items[MetaType.News + "Title"]; }
            set { SettingProvider.Items[MetaType.News + "Title"] = value; }
        }
        public static string NewsMetaKeywords
        {
            get { return SettingProvider.Items[MetaType.News + "MetaKeywords"]; }
            set { SettingProvider.Items[MetaType.News + "MetaKeywords"] = value; }
        }
        public static string NewsMetaDescription
        {
            get { return SettingProvider.Items[MetaType.News + "MetaDescription"]; }
            set { SettingProvider.Items[MetaType.News + "MetaDescription"] = value; }
        }


        public static string GetDefaultTitle(MetaType type)
        {
            return Convert.ToString(SettingProvider.Items[type.ToString() + "Title"]);
        }
        public static void SetDefaultTitle(MetaType type, string value)
        {
            SettingProvider.Items[type.ToString() + "Title"] = value;
        }

        public static string GetDefaultMetaDescription(MetaType metaType)
        {
            return Convert.ToString(SettingProvider.Items[metaType.ToString() + "MetaDescription"]);
        }
        public static void SetDefaultMetaDescription(MetaType metaType, string value)
        {
            SettingProvider.Items[metaType.ToString() + "MetaDescription"] = value;
        }

        public static string GetDefaultMetaKeywords(MetaType metaType)
        {
            return Convert.ToString(SettingProvider.Items[metaType.ToString() + "MetaKeywords"]);
        }

        public static void SetDefaultMetaKeywords(MetaType metaType, string value)
        {
            SettingProvider.Items[metaType.ToString() + "MetaKeywords"] = value;
        }

        public static string CustomMetaString
        {
            get { return SettingProvider.Items["CustomMetaString"]; }
            set { SettingProvider.Items["CustomMetaString"] = value; }
        }
    }
}
