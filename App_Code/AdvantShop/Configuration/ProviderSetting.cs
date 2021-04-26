//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Configuration;
using AdvantShop.Core;
using AdvantShop.Core.Caching;
using AdvantShop.Customers;
using AdvantShop.Helpers;

namespace AdvantShop.Configuration
{
    /// <summary>
    /// Setting provider class
    /// </summary>
    /// <remarks></remarks>
    public class SettingProvider
    {
        public sealed class SettingIndexer
        {
            public string this[string name]
            {
                get { return GetSqlSettingValue(name); }
                set { SetSqlSettingValue(name, value); }
            }
        }

        private static SettingIndexer _staticIndexer;
        public static SettingIndexer Items
        {
            get { return _staticIndexer ?? (_staticIndexer = new SettingIndexer()); }
        }

        #region  SQL storage

        /// <summary>
        /// Save settings into DB
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="strValue"></param>
        /// <remarks></remarks>
        public static bool SetSqlSettingValue(string strName, string strValue)
        {
            bool boolReult = SetSqlSettingValueFromDB(strName, strValue);

            // Add into cahce
            string strCacheName = CacheNames.GetCommonSettingsCacheObjectName(strName);
            CacheManager.Insert(strCacheName, strValue);

            return boolReult;
        }

        public static void SetSqlSettingValue(string strName, string strValue, SQLDataAccess db)
        {
            SetSqlSettingValueFromDB(strName, strValue, db);

            // Add into cahce
            string strCacheName = CacheNames.GetCommonSettingsCacheObjectName(strName);
            CacheManager.Insert(strCacheName, strValue);
        }

        private static bool SetSqlSettingValueFromDB(string strName, string strValue)
        {
            using (var db = new SQLDataAccess())
            {
                // NOTE:
                // If the record are not exist, then it will be
                // created into DB via storedProcedure.
                db.cnOpen();
                SetSqlSettingValueFromDB(strName, strValue, db);
                db.cnClose();
            }
            return true;
        }

        private static void SetSqlSettingValueFromDB(string strName, string strValue, SQLDataAccess db)
        {
            db.cmd.CommandText = "[Settings].[sp_UpdateSettings]";
            db.cmd.CommandType = CommandType.StoredProcedure;
            db.cmd.Parameters.Clear();
            db.cmd.Parameters.AddWithValue("@Name", strName);
            db.cmd.Parameters.AddWithValue("@Value", strValue);
            db.cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Read settings from DB.
        /// On Err: Function will return Nothing
        /// </summary>
        /// <param name="strName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetSqlSettingValue(string strName)
        {
            string strRes;

            string strCacheName = CacheNames.GetCommonSettingsCacheObjectName(strName);
            if (CacheManager.Contains(strCacheName))
            {
                strRes = CacheManager.Get<string>(strCacheName);
            }
            else
            {
                strRes = GetSqlSettingValueFromDB(strName);
             
                if (strRes != null) CacheManager.Insert(strCacheName, strRes);
            }

            return strRes;
        }

        private static string GetSqlSettingValueFromDB(string strName)
        {
            var strRes = SQLDataAccess.ExecuteScalar<string>("[Settings].[sp_GetSettingValue]", CommandType.StoredProcedure,
                                                                new SqlParameter { ParameterName = "@SettingName", Value = strName });
            return strRes;
        }


        public static bool IsSqlSettingExist(string strKey)
        {
            return SQLDataAccess.ExecuteScalar<int>("SELECT COUNT(Name) AS COUNTID FROM [Settings].[Settings] WHERE [Name]=@Name",
                                                   CommandType.Text, new SqlParameter { ParameterName = "@Name", Value = strKey }) > 0;
        }

        public static bool RemoveSqlSetting(string strKey)
        {
            SQLDataAccess.ExecuteNonQuery("DELETE FROM [Settings].[Settings] WHERE [Name]=@Name", CommandType.Text, new SqlParameter("@Name", strKey));

            // Clear cache object
            string strCacheName = CacheNames.GetCommonSettingsCacheObjectName(strKey);
            CacheManager.Remove(strCacheName);
            return true;
        }

        #endregion

        #region  Web.config storage

        /// <summary>
        /// Read settings from appSettings node web.config file.
        /// On Err: Function will return an empty string
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetConfigSettingValue(string strKey)
        {
            var config = new AppSettingsReader();
            return config.GetValue(strKey, typeof(String)).ToString();
        }
        
        public static T GetConfigSettingValue<T>(string strKey)
        {
            var config = new AppSettingsReader();
            return (T)config.GetValue(strKey, typeof(T));
        }

        /// <summary>
        /// Save settings from appSettings node web.config
        /// </summary>
        /// <param name="strKey"></param>
        /// <param name="strValue"></param>
        /// <remarks></remarks>
        public static bool SetConfigSettingValue(string strKey, string strValue)
        {
            System.Configuration.Configuration config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
            var myAppSettings = (AppSettingsSection)config.GetSection("appSettings");
            myAppSettings.Settings[strKey].Value = strValue;
            config.Save();

            return true;
        }

        #endregion
        
        public static CustomerContact GetSellerContact()
        {
            var result = new CustomerContact();

            using (var db = new SQLDataAccess())
            {
                db.cmd.CommandText =
                    "SELECT [Name], [Value] FROM [Settings].[Settings] WHERE [Name] = 'SalerCountryID' or [Name] = 'SalerRegionID'";
                db.cmd.CommandType = CommandType.Text;
                db.cnOpen();
                using (SqlDataReader reader = db.cmd.ExecuteReader())
                    while (reader.Read())
                    {
                        if (SQLDataHelper.GetString(reader, "Name") == "SalerCountryID")
                        {
                            result.CountryId = int.Parse(SQLDataHelper.GetString(reader, "Value"));
                        }
                        else
                        {
                            result.RegionId = int.Parse(SQLDataHelper.GetString(reader, "Value"));
                        }
                    }
                db.cnClose();
            }
            return result;
        }
    }
}