//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Globalization;

namespace AdvantShop.Configuration
{
    public class SettingsLic
    {
        public static string LicKey
        {
            get { return SettingProvider.Items["LicKey"]; }
            set { SettingProvider.Items["LicKey"] = value; }
        }

        public static bool ActiveLic
        {
            get { return Convert.ToBoolean(SettingProvider.Items["ActiveLic"]); }
            set { SettingProvider.Items["ActiveLic"] = value.ToString(CultureInfo.InvariantCulture); }
        }
    }
}