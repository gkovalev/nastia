//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

namespace AdvantShop.Configuration
{
    public class SettingsMailChimp
    {
        public static string MailChimpId
        {
            get { return SettingProvider.Items["MailChimpId"]; }
            set { SettingProvider.Items["MailChimpId"] = value; }
        }

        public static string MailChimpRegUsersList
        {
            get { return SettingProvider.Items["MailChimpRegUsersList"]; }
            set { SettingProvider.Items["MailChimpRegUsersList"] = value; }
        }

        public static string MailChimpUnRegUsersList
        {
            get { return SettingProvider.Items["MailChimpNoRegUsersList"]; }
            set { SettingProvider.Items["MailChimpNoRegUsersList"] = value; }
        }
        
        public static string MailChimpAllUsersList
        {
            get { return SettingProvider.Items["MailChimpAllUsersList"]; }
            set { SettingProvider.Items["MailChimpAllUsersList"] = value; }
        }

        public static bool MailChimpActive
        {
            get { return System.Convert.ToBoolean(SettingProvider.Items["MailChimpActive"]); }
            set { SettingProvider.Items["MailChimpActive"] = value.ToString(); }
        }
    }
}