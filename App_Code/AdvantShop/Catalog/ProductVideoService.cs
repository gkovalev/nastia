//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdvantShop.Core;
using AdvantShop.Helpers;

namespace AdvantShop.Catalog
{
    public class ProductVideoService
    {
        public static List<ProductVideo> GetProductVideos(int productId)
        {
            List<ProductVideo> list = SQLDataAccess.ExecuteReadList<ProductVideo>(
                "SELECT ProductVideoID, ProductID, Name, PlayerCode, Description, VideoSortOrder FROM [Catalog].[ProductVideo] WHERE [ProductID]=@ProductID ORDER BY [VideoSortOrder]",
                CommandType.Text, GetProductVideoFromReader, new SqlParameter { ParameterName = "@ProductID", Value = productId });

            return list;
        }

        public static ProductVideo GetProductVideoFromReader(SqlDataReader reader)
        {
            return new ProductVideo
                       {
                           ProductVideoId = SQLDataHelper.GetInt(reader, "ProductVideoID"),
                           ProductId = SQLDataHelper.GetInt(reader, "ProductID"),
                           Name = SQLDataHelper.GetString(reader, "Name"),
                           PlayerCode = SQLDataHelper.GetString(reader, "PlayerCode"),
                           Description = SQLDataHelper.GetString(reader, "Description"),
                           VideoSortOrder = SQLDataHelper.GetInt(reader, "VideoSortOrder")
                       };
        }

        public static void AddProductVideo(ProductVideo pv)
        {
            SQLDataAccess.ExecuteNonQuery("INSERT INTO [Catalog].[ProductVideo] ([ProductID], [Name], [PlayerCode], [Description], [VideoSortOrder]) VALUES (@ProductId, @Name, @PlayerCode, @Description, @VideoSortOrder)",
                            CommandType.Text, new[]
                                                        {
                                                         new SqlParameter("@ProductID", pv.ProductId),
                                                         new SqlParameter("@Name", pv.Name),
                                                         new SqlParameter("@PlayerCode", pv.PlayerCode),
                                                         new SqlParameter("@Description", pv.Description),
                                                         new SqlParameter("@VideoSortOrder", pv.VideoSortOrder)
                                                        }
                            );
        }

        public static void UpdateProductVideo(ProductVideo pv)
        {
            SQLDataAccess.ExecuteNonQuery("UPDATE Catalog.ProductVideo SET Name=@Name, PlayerCode=@PlayerCode, Description=@Description, VideoSortOrder=@VideoSortOrder WHERE ProductVideoID = @ProductVideoID",
                            CommandType.Text, new[]
                                                        {
                                                         new SqlParameter("@ProductVideoId", pv.ProductVideoId),
                                                         new SqlParameter("@Name", pv.Name),
                                                         new SqlParameter("@PlayerCode", pv.PlayerCode),
                                                         new SqlParameter("@Description", pv.Description),
                                                         new SqlParameter("@VideoSortOrder", pv.VideoSortOrder)
                                                        }
                            );
        }

        public static void UpdateProductVideo(int productVideoId, string name, int videoSortOrder)
        {
            SQLDataAccess.ExecuteNonQuery("UPDATE Catalog.ProductVideo SET Name=@Name, VideoSortOrder=@VideoSortOrder WHERE ProductVideoID=@ProductVideoID",
                                                CommandType.Text,
                                                new SqlParameter { ParameterName = "@Name", Value = name },
                                                new SqlParameter { ParameterName = "@VideoSortOrder", Value = videoSortOrder },
                                                new SqlParameter { ParameterName = "@ProductVideoId", Value = productVideoId }
                                                );
        }

        public static void DeleteProductVideo(int productVideoId)
        {
            SQLDataAccess.ExecuteNonQuery("DELETE FROM [Catalog].[ProductVideo] WHERE ProductVideoId=@ProductVideoId", CommandType.Text,
                                                new SqlParameter { ParameterName = "@ProductVideoId", Value = productVideoId });
        }

        public static ProductVideo GetProductVideo(int productVideoId)
        {
            return
                SQLDataAccess.ExecuteReadOne<ProductVideo>(
                    "SELECT * FROM [Catalog].[ProductVideo] WHERE [ProductVideoID] = @ProductVideoID", CommandType.Text,
                    GetProductVideoFromReader, new SqlParameter("@ProductVideoID", productVideoId));
        }
    }
}
