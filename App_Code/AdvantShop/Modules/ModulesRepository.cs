//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdvantShop.Core;
using AdvantShop.Helpers;

namespace AdvantShop.Modules
{
    public class ModulesRepository
    {

        /// <summary>
        /// Get Module From SQLDataReader
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static Module GetModuleFromReader(SqlDataReader reader)
        {
            return new Module
            {
                StringId = SQLDataHelper.GetString(reader, "ModuleStringID"),
                IsInstall = SQLDataHelper.GetBoolean(reader, "IsInstall"),
                DateAdded = SQLDataHelper.GetDateTime(reader, "DateAdded"),
                DateModified = SQLDataHelper.GetDateTime(reader, "DateModified"),
                Version = SQLDataHelper.GetString(reader, "Version")
            };
        }

        /// <summary>
        /// Add module to datebase and set Install
        /// </summary>
        /// <param name="module"></param>
        public static void InstallModuleToDb(Module module)
        {
            SQLDataAccess.ExecuteNonQuery(
                @"IF (SELECT COUNT([ModuleStringID]) FROM [dbo].[Modules] WHERE [ModuleStringID] = @ModuleStringID) = 0
                    BEGIN
                        INSERT INTO [dbo].[Modules] ([ModuleStringID],[IsInstall],[DateAdded],[DateModified],[Version]) VALUES (@ModuleStringID,1,@DateAdded,@DateModified,@Version)
                    END
                    ELSE
                    BEGIN
                        UPDATE [dbo].[Modules] SET [IsInstall] = 1, [DateModified] = @DateModified, [Version] = @Version WHERE [ModuleStringID] = @ModuleStringID
                    END",
                CommandType.Text,
                new SqlParameter("@ModuleStringID", module.StringId),
                new SqlParameter("@DateAdded", module.DateAdded),
                new SqlParameter("@DateModified", module.DateModified),
                new SqlParameter("@Version", module.Version.IsNullOrEmpty() ? DBNull.Value : (object)module.Version));
        }

        /// <summary>
        /// Get module from datebase
        /// </summary>
        /// <param name="moduleStringId"></param>
        public static Module GetModuleFromDb(string moduleStringId)
        {
            return SQLDataAccess.ExecuteReadOne<Module>(
                @"SELECT * FROM [dbo].[Modules] WHERE [ModuleStringID] = ModuleStringID",
                CommandType.Text,
                GetModuleFromReader,
                new SqlParameter("@ModuleStringID", moduleStringId));
        }

        /// <summary>
        /// Get all module from datebase
        /// </summary>
        public static List<Module> GetModulesFromDb()
        {
            return SQLDataAccess.ExecuteReadList<Module>(
                @"SELECT * FROM [dbo].[Modules]",
                CommandType.Text,
                GetModuleFromReader);
        }

        /// <summary>
        /// Update module in datebase and set Uninstall
        /// </summary>
        /// <param name="moduleStringId"></param>
        public static void UninstallModuleFromDb(string moduleStringId)
        {
            SQLDataAccess.ExecuteNonQuery(
                @"DELETE FROM [dbo].[Modules] WHERE [ModuleStringID] = @ModuleStringID
                  DELETE FROM [Settings].[ModuleSettings] WHERE [ModuleName] = @ModuleStringID",
                CommandType.Text,
                new SqlParameter("@ModuleStringID", moduleStringId));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleStringId"></param>
        /// <returns></returns>
        public static bool IsInstallModule(string moduleStringId)
        {
            return SQLDataAccess.ExecuteScalar<bool>(
                "SELECT [IsInstall] FROM [dbo].[Modules] WHERE [ModuleStringID] = @ModuleStringID",
                CommandType.Text,
                new SqlParameter("@ModuleStringID", moduleStringId));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="da"></param>
        /// <param name="schema"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static bool IsExistsModuleTable(SQLDataAccess da, string schema, string tableName)
        {
            da.cmd.CommandText = @"IF((SELECT COUNT(TABLE_NAME) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = @Schema AND TABLE_NAME = @TableName) > 0) Select 1 ELSE Select 0 ";
            da.cmd.CommandType = CommandType.Text;
            da.cmd.Parameters.Clear();
            da.cmd.Parameters.AddWithValue("@Schema", schema);
            da.cmd.Parameters.AddWithValue("@TableName", tableName);

            da.cnOpen();
            bool result = Convert.ToBoolean(da.cmd.ExecuteScalar());
            da.cnClose();
            
            return result;
        }
    }
}