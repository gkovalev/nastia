//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Data;
using System.Data.SqlClient;
using AdvantShop.Core;

namespace AdvantShop.ExportImport
{

    public class ExportFeed
    {
        private static string _shopURL;

        public static bool IsExistsModuleSetting(string moduleName, string settingName)
        {
            bool result = SQLDataAccess.ExecuteScalar<int>("SELECT COUNT([Value]) FROM [Settings].[Settings] WHERE [Name] = @settingName;",
                                                           CommandType.Text, new SqlParameter("@settingName", moduleName + settingName)) > 0;
            return result;
        }

        public static string GetModuleSetting(string moduleName, string settingName)
        {
            var result = SQLDataAccess.ExecuteScalar<string>("SELECT [Value] FROM [Settings].[Settings] WHERE [Name] = @settingName;",
                                                           CommandType.Text, new SqlParameter("@settingName", moduleName + settingName));
            return result;
        }

        public static void AddModuleSetting(string moduleName, string settingName, string settingValue)
        {
            SQLDataAccess.ExecuteNonQuery("INSERT INTO [Settings].[Settings](Name, Value) VALUES(@settingName, @settingValue);",
                                            CommandType.Text,
                                            new SqlParameter("@settingName", moduleName + settingName),
                                            new SqlParameter("@settingValue", settingValue));
        }

        public static void UpdateModuleSetting(string moduleName, string settingName, string settingValue)
        {
            SQLDataAccess.ExecuteNonQuery("UPDATE [Settings].[Settings] SET Value = @settingValue WHERE Name = @settingName;",
                                            CommandType.Text,
                                            new SqlParameter("@settingName", moduleName + settingName),
                                            new SqlParameter("@settingValue", settingValue));
        }

        public static void RefreshModuleSetting(string moduleName, string settingName, string settingValue)
        {
            if (IsExistsModuleSetting(moduleName, settingName))
            {
                UpdateModuleSetting(moduleName, settingName, settingValue);
            }
            else
            {
                AddModuleSetting(moduleName, settingName, settingValue);
            }
        }

        public static string GetShopUrl()
        {
            if (_shopURL == null)
            {
                _shopURL = SQLDataAccess.ExecuteScalar<string>("SELECT Value FROM [Settings].[Settings] WHERE Name = \'ShopURL\';", CommandType.Text);
            }
            return string.IsNullOrEmpty(_shopURL) ? _shopURL : _shopURL.TrimEnd("/ ".ToCharArray());
        }

        public static void SetShopUrlToNull()
        {
            _shopURL = null;
        }
    }
}