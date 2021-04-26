//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Threading;

namespace AdvantShop.Localization
{

    public class Culture
    {
        public enum ListLanguage
        {
            Russian = 0,
            English = 1
        }

        public static ListLanguage Language
        {
            get
            {
                switch (Configuration.SettingsMain.Language)
                {
                    case "en":
                    case "en-US":
                        return ListLanguage.English;
                    case "ru":
                    case "ru-RU":
                        return ListLanguage.Russian;

                    default:
                        return ListLanguage.Russian;
                }
            }
            set
            {
                Configuration.SettingsMain.Language = GetStringLangByEnum(value);
                InitializeCulture();
            }
        }

        public static void InitializeCulture()
        {
            ListLanguage s = Language;
            string lang = GetStringLangByEnum(s);
            if (!string.IsNullOrEmpty(lang))
            {
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(lang);
                Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture(lang);
            }
        }

        public static void InitializeCulture(string langValue)
        {
            string lang = langValue;
            if (!string.IsNullOrEmpty(lang))
            {
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(lang);
                Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture(lang);
            }
        }

        private static string GetStringLangByEnum(ListLanguage lang)
        {
            switch (lang)
            {
                case ListLanguage.English:
                    return "en-US";
                case ListLanguage.Russian:
                    return "ru-RU";
                default:
                    return "ru-RU";
            }
        }

        public static string ConvertDate(DateTime d)
        {
            return d.ToString(Configuration.SettingsMain.AdminDateFormat);
        }

        public static string ConvertShortDate(DateTime d)
        {
            return d.ToString(Configuration.SettingsMain.ShortDateFormat);
        }

        public static string ConvertDateFromString(string s)
        {
            DateTime d = DateTime.Parse(s, System.Globalization.CultureInfo.GetCultureInfo(GetStringLangByEnum(Language)));
            return d.ToString(Configuration.SettingsMain.AdminDateFormat);
        }
    }
}
