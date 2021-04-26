//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using AdvantShop.Configuration;
using AdvantShop.SEO;

namespace AdvantShop.Core
{

    public class GlobalStringVariableService
    {
        public static IDictionary<string, string> GetGlobalVariables()
        {
            var dict = new Dictionary<string, string> { { "#STORE_NAME#", SettingsMain.ShopName } };
            //
            // Extend this fo more variables
            //
            return dict;
        }

        public static string TranslateExpression(string str)
        {
            var dict = (Dictionary<string, string>)GetGlobalVariables();
            return TranslateExpression(str, dict);
        }

        public static string TranslateExpression(string str, MetaType type, string name)
        {
            var dict = (Dictionary<string, string>)GetGlobalVariables().AddRange(GetSpecificVariables(type, name));
            
            return TranslateExpression(str, dict);
        }

        public static Dictionary<string, string> GetSpecificVariables(MetaType type, string name)
        {
            if (name != null)
            {
                switch (type)
                {
                    case MetaType.Category:
                        return new Dictionary<string, string> { { "#CATEGORY_NAME#", name } };
                    case MetaType.Product:
                        return new Dictionary<string, string> { { "#PRODUCT_NAME#", name } };
                    case MetaType.News:                        
                        return new Dictionary<string, string> { { "#NEWS_NAME#", name } };
                    case MetaType.Brand:
                        return new Dictionary<string, string> { { "#BRAND_NAME#", name } };
                    case MetaType.StaticPage:
                        return new Dictionary<string, string> { { "#PAGE_NAME#", name } };
                    default:
                        return new Dictionary<string, string>();
                }
            }
            return new Dictionary<string, string>();
        }
        public static string TranslateExpression(string strExpr, Dictionary<string, string> dict)
        {
            string strRes = strExpr;

            foreach (KeyValuePair<string, string> rec in dict)
            {
                if (strRes.Contains(rec.Key))
                {
                    strRes = strRes.Replace(rec.Key, rec.Value);
                }
            }
            return strRes;
        }
    }
}
