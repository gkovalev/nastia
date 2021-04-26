//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdvantShop.Core;
using AdvantShop.Helpers;
using System;

namespace AdvantShop.SEO
{
    public static class RedirectSeoService
    {
        public static RedirectSeo GetRedirectSeoById(int id)
        {
            return SQLDataAccess.ExecuteReadOne<RedirectSeo>("SELECT * FROM [Settings].[Redirect] WHERE ID = @ID",
                                                                                CommandType.Text, GetRedirectSeoFromReader,
                                                                                new SqlParameter { ParameterName = "@ID", Value = id });
        }

        public static IEnumerable<RedirectSeo> GetRedirectsSeo()
        {
            return SQLDataAccess.ExecuteReadIEnumerable<RedirectSeo>("SELECT * FROM [Settings].[Redirect]", CommandType.Text, GetRedirectSeoFromReader);
        }

        public static RedirectSeo GetByInputUrl(string relativeUrl, string absoluteUrl)
        {
            return SQLDataAccess.ExecuteReadOne<RedirectSeo>("SELECT ID, Replace(RedirectFrom, '*', '') as RedirectFrom, RedirectTo, ProductArtNo  FROM [Settings].[Redirect] where @relativeUrl like Replace(RedirectFrom, '*', '%') or @absoluteUrl like Replace(RedirectFrom, '*', '%')", CommandType.Text,
                GetRedirectSeoFromReader, new SqlParameter("@relativeUrl", relativeUrl.Trim()), new SqlParameter("@absoluteUrl", absoluteUrl.Trim()));
        }

        private static RedirectSeo GetRedirectSeoFromReader(SqlDataReader reader)
        {
            return new RedirectSeo
            {
                ID = SQLDataHelper.GetInt(reader, "ID"),
                RedirectFrom = SQLDataHelper.GetString(reader, "RedirectFrom"),
                RedirectTo = SQLDataHelper.GetString(reader, "RedirectTo"),
                ProductArtNo = SQLDataHelper.GetString(reader, "ProductArtNo")
            };
        }

        public static void AddRedirectSeo(RedirectSeo redirectSeo)
        {
            redirectSeo.ID =
                SQLDataAccess.ExecuteScalar<int>(
                    "INSERT INTO [Settings].[Redirect] ([RedirectFrom], [RedirectTo], [ProductArtNo]) VALUES (@RedirectFrom, @RedirectTo, @ProductArtNo); SELECT SCOPE_IDENTITY();",
                    CommandType.Text,
                    new SqlParameter("@RedirectFrom", redirectSeo.RedirectFrom),
                    new SqlParameter("@RedirectTo", redirectSeo.RedirectTo),
                    new SqlParameter("@ProductArtNo", redirectSeo.ProductArtNo)
                    );
        }

        public static void UpdateRedirectSeo(RedirectSeo redirectSeo)
        {
            SQLDataAccess.ExecuteNonQuery("UPDATE [Settings].[Redirect] SET RedirectFrom = @RedirectFrom, RedirectTo = @RedirectTo, ProductArtNo = @ProductArtNo WHERE ID = @ID", CommandType.Text,
                                                new SqlParameter("@ID", redirectSeo.ID),
                                                new SqlParameter("@RedirectFrom", redirectSeo.RedirectFrom),
                                                new SqlParameter("@RedirectTo", redirectSeo.RedirectTo),
                                                new SqlParameter("@ProductArtNo", redirectSeo.ProductArtNo)
                                         );
        }

        public static void DeleteRedirectSeo(int id)
        {
            SQLDataAccess.ExecuteNonQuery("DELETE FROM [Settings].[Redirect] WHERE ID = @ID", CommandType.Text,
                                          new SqlParameter { ParameterName = "@ID", Value = id });
        }
    }
}