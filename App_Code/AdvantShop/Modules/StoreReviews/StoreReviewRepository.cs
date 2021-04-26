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
    public class StoreReviewRepository
    {
        public static bool InstallStoreReviewsModule()
        {
            bool isInstall;
            using (var da = new SQLDataAccess())
            {
                if (ModulesRepository.IsExistsModuleTable(da, "Module", "StoreReview"))
                {
                    return true;
                }

                da.cmd.CommandText = @"CREATE TABLE Module.StoreReview
	                                        (
	                                        ID int NOT NULL IDENTITY (1, 1),
	                                        ParentID int NULL,
	                                        ReviewerEmail nvarchar(50) NOT NULL,
                                            ReviewerName nvarchar(100) NOT NULL,
	                                        Review nvarchar(MAX) NOT NULL,
	                                        DateAdded datetime NOT NULL,
                                            Moderated bit NOT NULL,
	                                        Rate int NULL
	                                        )  ON [PRIMARY]
	                                         TEXTIMAGE_ON [PRIMARY]                                        
                                        ALTER TABLE Module.StoreReview ADD CONSTRAINT
	                                        PK_StoreReview PRIMARY KEY CLUSTERED 
	                                        (
	                                        ID
	                                        ) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]                                        
                                        ALTER TABLE Module.StoreReview SET (LOCK_ESCALATION = TABLE)
                                        SET IDENTITY_INSERT Module.StoreReview ON
                                        ";
                da.cmd.CommandType = CommandType.Text;

                da.cnOpen();
                da.cmd.ExecuteNonQuery();
                da.cnClose();

                ModuleSettingsProvider.SetSettingValue("PageSize", "20", "StoreReviews");
                isInstall = ModulesRepository.IsExistsModuleTable(da, "Module", "StoreReview");


            }
            return isInstall;
        }

        public static bool UninstallStoreReviewsModule()
        {
            bool isInstall;
            using (var da = new SQLDataAccess())
            {
                if (!ModulesRepository.IsExistsModuleTable(da, "Module", "StoreReview"))
                {
                    return true;
                }

                da.cmd.CommandText = "DROP TABLE Module.StoreReview";
                da.cmd.CommandType = CommandType.Text;

                da.cnOpen();
                da.cmd.ExecuteNonQuery();
                da.cnClose();

                isInstall = ModulesRepository.IsExistsModuleTable(da, "Module", "StoreReview");
            }
            return isInstall;
        }

        public static bool IsAliveStoreReviewsModule()
        {
            using (var da = new SQLDataAccess())
            {
                return ModulesRepository.IsExistsModuleTable(da, "Module", "StoreReview");
            }
        }

        private static StoreReview GetStoreReviewFromReader(SqlDataReader reader)
        {
            return new StoreReview
                {
                    Id = SQLDataHelper.GetInt(reader, "ID"),
                    ParentId = SQLDataHelper.GetInt(reader, "ParentID"),
                    Rate = SQLDataHelper.GetInt(reader, "Rate"),
                    Review = SQLDataHelper.GetString(reader, "Review"),
                    ReviewerEmail = SQLDataHelper.GetString(reader, "ReviewerEmail"),
                    ReviewerName = SQLDataHelper.GetString(reader, "ReviewerName"),
                    DateAdded = SQLDataHelper.GetDateTime(reader, "DateAdded"),
                    Moderated = SQLDataHelper.GetBoolean(reader, "Moderated"),
                    HasChild = SQLDataHelper.GetInt(reader, "ChildsCount") > 0
                };
        }

        public static List<StoreReview> GetStoreReviewsByParentId(int parentId)
        {
            return GetStoreReviewsByParentId(parentId, false);
        }


        public static DataTable GetStoreReviews()
        {
            return SQLDataAccess.ExecuteTable(
                "SELECT * FROM [Module].[StoreReview] ORDER BY [DateAdded] DESC",
                CommandType.Text
            );
        }

        public static List<StoreReview> GetStoreReviewsByParentId(int parentId, bool isModerated)
        {
            return SQLDataAccess.ExecuteReadList<StoreReview>(
                "SELECT *, (SELECT Count(ID) FROM [Module].[StoreReview] WHERE [ParentID] = ParentReview.ID) as ChildsCount FROM [Module].[StoreReview] as ParentReview WHERE "
                + (parentId == 0 ? "[ParentID] is NULL" : "[ParentID] = " + parentId)
                + (isModerated ? " AND [Moderated] = 1" : string.Empty) + " ORDER BY [DateAdded] DESC",
                CommandType.Text,
                (reader) =>
                {
                    var review = GetStoreReviewFromReader(reader);
                    review.ChildrenReviews = SQLDataHelper.GetInt(reader, "ChildsCount") > 0
                                                 ? GetStoreReviewsByParentId(
                                                     SQLDataHelper.GetInt(reader, "ID"), isModerated)
                                                 : new List<StoreReview>();
                    return review;
                }
            );
        }

        public static StoreReview GetStoreReview(int id)
        {
            return SQLDataAccess.ExecuteReadOne<StoreReview>(
                "SELECT *, (SELECT Count(ID) FROM [Module].[StoreReview] WHERE [ParentID] = ParentReview.ID) as ChildsCount FROM [Module].[StoreReview] as ParentReview WHERE [ID] = @ID",
                CommandType.Text,
                (reader) =>
                {
                    var review = GetStoreReviewFromReader(reader);
                    review.ChildrenReviews = SQLDataHelper.GetInt(reader, "ChildsCount") > 0
                                                 ? GetStoreReviewsByParentId(
                                                     SQLDataHelper.GetInt(reader, "ParentID"))
                                                 : new List<StoreReview>();
                    return review;
                },
                new SqlParameter("@ID", id));
        }

        public static void DeleteStoreReviewsById(int id)
        {
            SQLDataAccess.ExecuteNonQuery(
                "DELETE FROM [Module].[StoreReview] WHERE [ID] = @ID",
                CommandType.Text,
                new SqlParameter("@ID", id));
        }

        public static void AddStoreReview(StoreReview storeReview)
        {
            SQLDataAccess.ExecuteNonQuery(
                "INSERT INTO [Module].[StoreReview] ([ParentID],[Rate],[Review],[ReviewerEmail],[ReviewerName],[DateAdded],[Moderated]) VALUES (@ParentID,@Rate,@Review,@ReviewerEmail,@ReviewerName,GETDATE(),@Moderated)",
                CommandType.Text,
                new SqlParameter("@ParentID", storeReview.ParentId == 0 ? DBNull.Value : (object)storeReview.ParentId),
                new SqlParameter("@Rate", storeReview.Rate),
                new SqlParameter("@Review", storeReview.Review),
                new SqlParameter("@ReviewerEmail", storeReview.ReviewerEmail),
                new SqlParameter("@ReviewerName", storeReview.ReviewerName),
                new SqlParameter("@Moderated", storeReview.Moderated));
        }

        public static void UpdateStoreReview(StoreReview storeReview)
        {
            SQLDataAccess.ExecuteNonQuery(
                "UPDATE [Module].[StoreReview] SET [ParentID]=@ParentID,[Rate]=@Rate,[Review]=@Review,[ReviewerEmail]=@ReviewerEmail,[ReviewerName]=@ReviewerName, [Moderated]=@Moderated, [DateAdded]=@DateAdded WHERE [ID]=@ID",
                CommandType.Text,
                new SqlParameter("@ID", storeReview.Id),
                new SqlParameter("@ParentID", storeReview.ParentId == 0 ? DBNull.Value : (object)storeReview.ParentId),
                new SqlParameter("@Rate", storeReview.Rate),
                new SqlParameter("@Review", storeReview.Review),
                new SqlParameter("@ReviewerEmail", storeReview.ReviewerEmail),
                new SqlParameter("@ReviewerName", storeReview.ReviewerName),
                new SqlParameter("@Moderated", storeReview.Moderated),
                new SqlParameter("@DateAdded", storeReview.DateAdded));
        }
    }
}
