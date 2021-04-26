using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdvantShop.Core;
using AdvantShop.Core.Caching;

public class ModuleSettingsProvider
{
    public static T GetSettingValue<T>(string strKey, string moduleName)
    {
        object obj = GetSqlSettingValue(strKey, moduleName);
        if (obj == null)
        {
            return default(T);
        }

        return (T)Convert.ChangeType(obj, typeof(T));
    }

    public static void SetSettingValue<T>(string strKey, T strValue, string moduleName)
    {
        SetSqlSettingValue(strKey, strValue, moduleName);
    }

    /// <summary>
    /// Save settings into DB
    /// </summary>
    /// <param name="strName"></param>
    /// <param name="strValue"></param>
    /// <param name="moduleName"> </param>
    /// <remarks></remarks>
    private static void SetSqlSettingValue<T>(string strName, T strValue, string moduleName)
    {
        SQLDataAccess.ExecuteNonQuery("[Settings].[sp_UpdateModuleSettings]", CommandType.StoredProcedure,
            new SqlParameter("@Name", strName),
            new SqlParameter("@Value", strValue.ToString()),
            new SqlParameter("@ModuleName", moduleName));
    }

    /// <summary>
    /// Read settings from DB.
    /// On Err: Function will return Nothing
    /// </summary>
    /// <param name="strName"></param>
    /// <param name="moduleName"> </param>
    /// <returns></returns>
    /// <remarks></remarks>
    private static object GetSqlSettingValue(string strName, string moduleName)
    {
        return GetSqlSettingValueFromDB(strName, moduleName);
    }

    private static object GetSqlSettingValueFromDB(string strName, string moduleName)
    {
        return SQLDataAccess.ExecuteScalar("[Settings].[sp_GetModuleSettingValue]", CommandType.StoredProcedure,
                                                            new SqlParameter("@SettingName", strName),
                                                            new SqlParameter("@ModuleName", moduleName));
    }

    public static bool IsSqlSettingExist(string strKey, string moduleName)
    {
        return SQLDataAccess.ExecuteScalar<int>("SELECT COUNT(Name) AS COUNTID FROM [Settings].[ModuleSettings] WHERE [Name] = @Name AND [ModuleName] = @ModuleName",
                                               CommandType.Text,
                                               new SqlParameter("@Name", strKey),
                                               new SqlParameter("@ModuleName", moduleName)) > 0;
    }

    public static bool RemoveSqlSetting(string strKey, string moduleName)
    {
        SQLDataAccess.ExecuteNonQuery("DELETE FROM [Settings].[ModuleSettings] WHERE [Name] = @Name AND [ModuleName] = @ModuleName",
            CommandType.Text,
            new SqlParameter("@Name", strKey),
            new SqlParameter("@ModuleName", moduleName));

        // Clear cache object
        string strCacheName = CacheNames.GetModuleSettingsCacheObjectName(strKey + moduleName);
        CacheManager.Remove(strCacheName);
        return true;
    }



    public static string GetAbsolutePath()
    {
        return AdvantShop.Configuration.SettingsGeneral.AbsolutePath;
    }

}