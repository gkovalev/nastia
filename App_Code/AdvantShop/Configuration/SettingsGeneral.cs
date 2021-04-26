//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;

namespace AdvantShop.Configuration
{
    public class SettingsGeneral
    {
        private static readonly Object ThisLock = new Object();
        private static string _absoluteUrl;
        public static string AbsoluteUrl { get { return _absoluteUrl; } }

        public static void SetAbsoluteUrl(string st)
        {
            lock (ThisLock)
            {
                _absoluteUrl = st;
            }
        }

        private static string _absolutePath;
        public static string AbsolutePath { get { return _absolutePath; } }
        public static void SetAbsolutePath(string st)
        {
            lock (ThisLock)
            {
                _absolutePath = st;
            }
        }

        public static string InstallFilePath { get { return AbsolutePath + "App_Data/install.txt"; } }

        public static string SiteVersion
        {
            get { return SettingProvider.GetConfigSettingValue("PublicVersion"); }
        }

        public static string SiteVersionDev
        {
            get { return SettingProvider.GetConfigSettingValue("Version"); }
        }

        public static string QrUserId
        {
            get { return SettingProvider.Items["QRUserID"]; }
        }

        public static string CurrentSaasId
        {
            get { return SettingProvider.Items["CurrentSaasId"]; }
            set { SettingProvider.Items["CurrentSaasId"] = value; }
        }

        public static string CsvSeparator
        {
            get { return SettingProvider.Items["CsvSeparator"]; }
            set { SettingProvider.Items["CsvSeparator"] = value; }
        }

        public static string CsvEnconing
        {
            get { return SettingProvider.Items["CsvEnconing"]; }
            set { SettingProvider.Items["CsvEnconing"] = value; }
        }
    }
}

