using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Net;
using AdvantShop.Configuration;
using AdvantShop.Diagnostics;
using AdvantShop.Core.Caching;
using System.Xml.Serialization;
using AdvantShop.Helpers;
using Newtonsoft.Json;

namespace AdvantShop.Design
{
    public enum eDesign
    {
        Theme,
        Color,
        Background
    }

    public static class DesignService
    {
        private const string RequestBaseUrl = "http://design.advantshop.net/httphandlers/getdesign.ashx";

        private static readonly Dictionary<eDesign, string> TypeAndPath = new Dictionary<eDesign, string>()
                                                                            {
                                                                                { eDesign.Theme, "themes" },
                                                                                { eDesign.Color, "colors" },
                                                                                { eDesign.Background, "backgrounds" }
                                                                            };


        public static List<Theme> GetDesigns(eDesign design)
        {
            string strCacheName = CacheNames.GetDesignCacheObjectName(design.ToString());

            if (CacheManager.Contains(strCacheName))
            {
                return CacheManager.Get<List<Theme>>(strCacheName);
            }

            var list = GetDesignsFromConfig(design);
            CacheManager.Insert(strCacheName, list);
            return list;
        }

        private static List<Theme> GetDesignsFromConfig(eDesign design)
        {
            var themes = new List<Theme>();
            var temp = new List<Theme>();

            foreach (var configPath in Directory.GetDirectories(SettingsGeneral.AbsolutePath + "design\\" + TypeAndPath[design]))
            {
                var themeName = configPath.Split('\\').Last();
                var themeConfig = configPath + "\\" + design.ToString() + ".config";

                if (File.Exists(themeConfig))
                {
                    try
                    {
                        using (var myReader = new StreamReader(themeConfig))
                        {
                            var mySerializer = new XmlSerializer(typeof(Theme));
                            var theme = (Theme)mySerializer.Deserialize(myReader);

                            var themeTitle = theme.Names.Find(t => t.Lang == SettingsMain.Language);
                            theme.Title = themeTitle != null ? themeTitle.Value : themeName;
                            theme.Name = themeName;

                            if (themeName != "_none")
                                temp.Add(theme);
                            else
                                themes.Add(theme);

                            myReader.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError(ex);
                    }
                }
            }
            themes.AddRange(temp.OrderBy(t => t.Title));

            return themes;
        }

        public static string GetDesign(string type)
        {
            switch (type)
            {
                case "theme":
                    return GetTheme(type);
                case "colorscheme":
                    return GetColorScheme(type);
                default:
                    throw new Exception("Design type is underfined");
            }
        }

        private static string GetTheme(string type)
        {
            if (!Demo.IsDemoEnabled)
                return SettingsDesign.Theme != "_none" ? "themes/" + SettingsDesign.Theme.ToLower() : "backgrounds/" + SettingsDesign.BackGround.ToLower();

            var styleCss = CommonHelper.GetCookieString(type).ToLower();
            if (string.IsNullOrEmpty(styleCss))
            {
                if (SettingsDesign.Theme != "_none")
                {
                    CommonHelper.SetCookie("theme", SettingsDesign.Theme.ToLower());
                    return "themes/" + SettingsDesign.Theme.ToLower();
                }
                CommonHelper.SetCookie("background", SettingsDesign.BackGround.ToLower());
                return "backgrounds/" + SettingsDesign.BackGround.ToLower();
            }

            return styleCss != "_none" ? "themes/" + styleCss : "backgrounds/" + CommonHelper.GetCookieString("background").ToLower();
        }

        private static string GetColorScheme(string type)
        {
            if (!Demo.IsDemoEnabled)
                return SettingsDesign.ColorScheme.ToLower();

            var styleCss = CommonHelper.GetCookieString(type).ToLower();
            if (string.IsNullOrEmpty(styleCss))
            {
                CommonHelper.SetCookie("colorscheme", SettingsDesign.ColorScheme.ToLower());
                return SettingsDesign.ColorScheme.ToLower();
            }
            return styleCss;
        }

        private static string GetThemesOnLine(eDesign designType)
        {
            string responseFromServer = "";

            try
            {
                var request = WebRequest.Create(RequestBaseUrl + "?type=" + designType.ToString() + "&lang=" + CultureInfo.CurrentCulture.TwoLetterISOLanguageName);

                using (var response = request.GetResponse())
                using (var dataStream = response.GetResponseStream())
                {
                    if (dataStream != null)
                        using (var reader = new StreamReader(dataStream))
                        {
                            responseFromServer = reader.ReadToEnd();
                        }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }

            return responseFromServer;
        }


        public static List<Theme> GetAvaliableDesignsOnLine(eDesign designType)
        {
            string response = GetThemesOnLine(designType);
            if (!string.IsNullOrEmpty(response))
            {
                return JsonConvert.DeserializeObject<List<Theme>>(response);
            }
            return null;
        }

    }
}