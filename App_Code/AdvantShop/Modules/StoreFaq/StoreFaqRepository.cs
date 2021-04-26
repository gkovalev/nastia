//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

//todo: without AdvantShop 
using AdvantShop.Core;
using AdvantShop.Helpers;

namespace AdvantShop.Modules
{
    public class StoreFaqRepository
    {
        public static bool InstallStoreFaqsModule()
        {
            bool isInstall;
            using (var da = new SQLDataAccess())
            {
                if (ModulesRepository.IsExistsModuleTable(da, "Module", "StoreFaq"))
                {
                    return true;
                }

                da.cmd.CommandText = @"CREATE TABLE Module.StoreFaq
	                                        (
	                                        ID int NOT NULL IDENTITY (1, 1),
	                                        ParentID int NULL,
	                                        FaqerEmail nvarchar(50) NOT NULL,
                                            FaqerName nvarchar(100) NOT NULL,
	                                        Faq nvarchar(MAX) NOT NULL,
	                                        DateAdded datetime NOT NULL,
                                            Moderated bit NOT NULL,
	                                        Rate int NULL
	                                        )  ON [PRIMARY]
	                                         TEXTIMAGE_ON [PRIMARY]                                        
                                        ALTER TABLE Module.StoreFaq ADD CONSTRAINT
	                                        PK_StoreFaq PRIMARY KEY CLUSTERED 
	                                        (
	                                        ID
	                                        ) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]                                        
                                        ALTER TABLE Module.StoreFaq SET (LOCK_ESCALATION = TABLE)
                                        SET IDENTITY_INSERT Module.StoreFaq ON
                                        ";
                da.cmd.CommandType = CommandType.Text;

                da.cnOpen();
                da.cmd.ExecuteNonQuery();
                da.cnClose();

                ModuleSettingsProvider.SetSettingValue("PageSize", "20", "StoreFaqs");
                isInstall = ModulesRepository.IsExistsModuleTable(da, "Module", "StoreFaq");


            }
            return isInstall;
        }

        public static bool UninstallStoreFaqsModule()
        {
            bool isInstall;
            using (var da = new SQLDataAccess())
            {
                if (!ModulesRepository.IsExistsModuleTable(da, "Module", "StoreFaq"))
                {
                    return true;
                }

                da.cmd.CommandText = "DROP TABLE Module.StoreFaq";
                da.cmd.CommandType = CommandType.Text;

                da.cnOpen();
                da.cmd.ExecuteNonQuery();
                da.cnClose();

                isInstall = ModulesRepository.IsExistsModuleTable(da, "Module", "StoreFaq");
            }
            return isInstall;
        }

        public static bool IsAliveStoreFaqsModule()
        {
            using (var da = new SQLDataAccess())
            {
                return ModulesRepository.IsExistsModuleTable(da, "Module", "StoreFaq");
            }
        }

        private static StoreFaq GetStoreFaqFromReader(SqlDataReader reader)
        {
            return new StoreFaq
                {
                    Id = SQLDataHelper.GetInt(reader, "ID"),
                    ParentId = SQLDataHelper.GetInt(reader, "ParentID"),
                    Rate = SQLDataHelper.GetInt(reader, "Rate"),
                    Faq = SQLDataHelper.GetString(reader, "Faq"),
                    FaqerEmail = SQLDataHelper.GetString(reader, "FaqerEmail"),
                    FaqerName = SQLDataHelper.GetString(reader, "FaqerName"),
                    DateAdded = SQLDataHelper.GetDateTime(reader, "DateAdded"),
                    Moderated = SQLDataHelper.GetBoolean(reader, "Moderated"),
                    HasChild = SQLDataHelper.GetInt(reader, "ChildsCount") > 0
                };
        }

        public static List<StoreFaq> GetStoreFaqsByParentId(int parentId)
        {
            return GetStoreFaqsByParentId(parentId, false);
        }


        public static DataTable GetStoreFaqs()
        {
            return SQLDataAccess.ExecuteTable(
                "SELECT * FROM [Module].[StoreFaq] ORDER BY [DateAdded] DESC",
                CommandType.Text
            );
        }

        public static List<StoreFaq> GetStoreFaqsByParentId(int parentId, bool isModerated)
        {
            return SQLDataAccess.ExecuteReadList<StoreFaq>(
                "SELECT *, (SELECT Count(ID) FROM [Module].[StoreFaq] WHERE [ParentID] = ParentFaq.ID) as ChildsCount FROM [Module].[StoreFaq] as ParentFaq WHERE "
                + (parentId == 0 ? "[ParentID] is NULL" : "[ParentID] = " + parentId)
                + (isModerated ? " AND [Moderated] = 1" : string.Empty) + " ORDER BY [DateAdded] DESC",
                CommandType.Text,
                (reader) =>
                {
                    var Faq = GetStoreFaqFromReader(reader);
                    Faq.ChildrenFaqs = SQLDataHelper.GetInt(reader, "ChildsCount") > 0
                                                 ? GetStoreFaqsByParentId(
                                                     SQLDataHelper.GetInt(reader, "ID"), isModerated)
                                                 : new List<StoreFaq>();
                    return Faq;
                }
            );
        }

        public static StoreFaq GetStoreFaq(int id)
        {
            return SQLDataAccess.ExecuteReadOne<StoreFaq>(
                "SELECT *, (SELECT Count(ID) FROM [Module].[StoreFaq] WHERE [ParentID] = ParentFaq.ID) as ChildsCount FROM [Module].[StoreFaq] as ParentFaq WHERE [ID] = @ID",
                CommandType.Text,
                (reader) =>
                {
                    var Faq = GetStoreFaqFromReader(reader);
                    Faq.ChildrenFaqs = SQLDataHelper.GetInt(reader, "ChildsCount") > 0
                                                 ? GetStoreFaqsByParentId(
                                                     SQLDataHelper.GetInt(reader, "ParentID"))
                                                 : new List<StoreFaq>();
                    return Faq;
                },
                new SqlParameter("@ID", id));
        }

        public static void DeleteStoreFaqsById(int id)
        {
            SQLDataAccess.ExecuteNonQuery(
                "DELETE FROM [Module].[StoreFaq] WHERE [ID] = @ID",
                CommandType.Text,
                new SqlParameter("@ID", id));
        }

        public static void AddStoreFaq(StoreFaq storeFaq)
        {
            SQLDataAccess.ExecuteNonQuery(
                "INSERT INTO [Module].[StoreFaq] ([ParentID],[Rate],[Faq],[FaqerEmail],[FaqerName],[DateAdded],[Moderated]) VALUES (@ParentID,@Rate,@Faq,@FaqerEmail,@FaqerName,GETDATE(),@Moderated)",
                CommandType.Text,
                new SqlParameter("@ParentID", storeFaq.ParentId == 0 ? DBNull.Value : (object)storeFaq.ParentId),
                new SqlParameter("@Rate", storeFaq.Rate),
                new SqlParameter("@Faq", storeFaq.Faq),
                new SqlParameter("@FaqerEmail", storeFaq.FaqerEmail),
                new SqlParameter("@FaqerName", storeFaq.FaqerName),
                new SqlParameter("@Moderated", storeFaq.Moderated));
        }

        public static void UpdateStoreFaq(StoreFaq storeFaq)
        {
            SQLDataAccess.ExecuteNonQuery(
                "UPDATE [Module].[StoreFaq] SET [ParentID]=@ParentID,[Rate]=@Rate,[Faq]=@Faq,[FaqerEmail]=@FaqerEmail,[FaqerName]=@FaqerName, [Moderated]=@Moderated, [DateAdded]=@DateAdded WHERE [ID]=@ID",
                CommandType.Text,
                new SqlParameter("@ID", storeFaq.Id),
                new SqlParameter("@ParentID", storeFaq.ParentId == 0 ? DBNull.Value : (object)storeFaq.ParentId),
                new SqlParameter("@Rate", storeFaq.Rate),
                new SqlParameter("@Faq", storeFaq.Faq),
                new SqlParameter("@FaqerEmail", storeFaq.FaqerEmail),
                new SqlParameter("@FaqerName", storeFaq.FaqerName),
                new SqlParameter("@Moderated", storeFaq.Moderated),
                new SqlParameter("@DateAdded", storeFaq.DateAdded));
        }
    }
}
