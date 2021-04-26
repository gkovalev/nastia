//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AdvantShop.Catalog;
using AdvantShop.Core;
using AdvantShop.Helpers;

namespace AdvantShop.Shipping
{
    public class ShippingMethodService
    {
        /// <summary>
        /// create ShippingMethod object from SqlDataReader
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="loadPic"></param>
        /// <returns></returns>
        public static ShippingMethod GetShippingMethodFromReader(SqlDataReader reader, bool loadPic = false)
        {
            return new ShippingMethod
                       {
                           ShippingMethodId = SQLDataHelper.GetInt(reader, "ShippingMethodID"),
                           Name = SQLDataHelper.GetString(reader, "Name"),
                           Type = (ShippingType)SQLDataHelper.GetInt(reader, "ShippingType"),
                           Description = SQLDataHelper.GetString(reader, "Description"),
                           Enabled = SQLDataHelper.GetBoolean(reader, "Enabled"),
                           SortOrder = SQLDataHelper.GetInt(reader, "SortOrder"),
                           IconFileName = loadPic ? new Photo(SQLDataHelper.GetInt(reader, "PhotoId"), SQLDataHelper.GetInt(reader, "ObjId"), PhotoType.Payment) { PhotoName = SQLDataHelper.GetString(reader, "PhotoName") } : null
                       };
        }

        /// <summary>
        /// return shipping service by his id
        /// </summary>
        /// <param name="shippingMethodId"></param>
        /// <returns>ShippingMethod</returns>
        public static DataTable GetShippingPayments(int shippingMethodId)
        {
            var temp = SQLDataAccess.ExecuteTable(@"SELECT [PaymentMethod].[PaymentMethodID], [PaymentMethod].[Name], (Select Count(PaymentMethodID) From [Order].[ShippingPayments] Where PaymentMethodID = [PaymentMethod].[PaymentMethodID] AND ShippingMethodID = @ShippingMethodID) as [Use] FROM [Order].[PaymentMethod]",
                CommandType.Text,
                new SqlParameter("@ShippingMethodID", shippingMethodId));

            return temp;
        }

        /// <summary>
        /// return shipping service by his id
        /// </summary>
        /// <param name="shippingMethodId"></param>
        /// <returns>ShippingMethod</returns>
        public static ShippingMethod GetShippingMethod(int shippingMethodId)
        {
            var temp = SQLDataAccess.ExecuteReadOne("SELECT ShippingMethodID,Name,ShippingType,Name,Description,Enabled,SortOrder FROM [Order].[ShippingMethod] WHERE ShippingMethodID = @ShippingMethodID",
                                                    CommandType.Text, reader => GetShippingMethodFromReader(reader), new SqlParameter("@ShippingMethodID", shippingMethodId));

            return temp;
        }


        public static bool IsPaymentNotUsed(int shippingMethodId, int paymentMethodId)
        {
            var temp = SQLDataAccess.ExecuteScalar<int>("SELECT Count(PaymentMethodID) FROM [Order].[ShippingPayments] WHERE ShippingMethodID = @ShippingMethodID AND PaymentMethodID = @PaymentMethodID",
                                                    CommandType.Text, new SqlParameter("@ShippingMethodID", shippingMethodId),
                                                    new SqlParameter("@PaymentMethodID", paymentMethodId));

            return temp > 0;
        }

        public static ShippingMethod GetShippingMethodByName(string name)
        {
            var temp = SQLDataAccess.ExecuteReadOne("SELECT * FROM [Order].[ShippingMethod] WHERE Name = @Name",
                                                   CommandType.Text, reader => GetShippingMethodFromReader(reader), new SqlParameter("@Name", name));
            return temp;
        }

        /// <summary>
        /// get all enabled shipping services
        /// </summary>
        /// <returns>List of ShippingMethod</returns>
        public static List<ShippingMethod> GetAllShippingMethods(bool enabled)
        {
            return SQLDataAccess.ExecuteReadList<ShippingMethod>("SELECT ShippingMethodID,Name,ShippingType,Name,ShippingMethod.Description,Enabled,SortOrder,ObjId,PhotoId,PhotoName FROM [Order].[ShippingMethod] " +
                                                                 " left join Catalog.Photo on Photo.objId=ShippingMethod.ShippingMethodID and Type=@Type" +
                                                                 " WHERE Enabled = @Enabled order by sortOrder",
                                                                  CommandType.Text, reader => GetShippingMethodFromReader(reader, true),
                                                                  new SqlParameter("@Enabled", enabled), new SqlParameter("@Type", PhotoType.Shipping.ToString()));
        }

        /// <summary>
        /// get all enabled shipping services
        /// </summary>
        /// <returns>List of ShippingMethod</returns>
        public static List<ShippingMethod> GetAllShippingMethods()
        {
            return SQLDataAccess.ExecuteReadList<ShippingMethod>("SELECT ShippingMethodID,Name,ShippingType,Name,ShippingMethod.Description,Enabled,SortOrder,ObjId,PhotoId,PhotoName FROM [Order].[ShippingMethod] left join Catalog.Photo on Photo.objId=ShippingMethod.ShippingMethodID and Type=@Type order by sortOrder",
                                                                       CommandType.Text, reader => GetShippingMethodFromReader(reader, true), new SqlParameter("@Type", PhotoType.Shipping.ToString()));
        }


        public static int InsertShippingMethod(ShippingMethod item)
        {
            return SQLDataAccess.ExecuteScalar<int>("INSERT INTO [Order].[ShippingMethod] ([ShippingType],[Name],[Description],[Enabled],[SortOrder]) VALUES (@ShippingType,@Name,@Description,@Enabled,@SortOrder); SELECT scope_identity();",
                                                        CommandType.Text,
                                                        new SqlParameter("@ShippingType", (int)item.Type),
                                                        new SqlParameter("@Name", item.Name),
                                                        new SqlParameter("@Description", item.Description),
                                                        new SqlParameter("@Enabled", item.Enabled),
                                                        new SqlParameter("@SortOrder", item.SortOrder));
        }

        public static bool UpdateShippingPayments(int shippingMethodId, List<int> payments)
        {
            var deleteCmd = string.Format("Delete From [Order].[ShippingPayments] Where [ShippingMethodID] = {0}", shippingMethodId);
            var insertCmd = payments.Aggregate(string.Empty,
                (current, paymentId) => current + string.Format("INSERT INTO [Order].[ShippingPayments] ([ShippingMethodID], [PaymentMethodID]) VALUES ({0}, {1});", shippingMethodId, paymentId));
            SQLDataAccess.ExecuteNonQuery(deleteCmd + insertCmd, CommandType.Text);
            return true;
        }

        /// <summary>
        /// update shipping method bu id
        /// </summary>
        /// <param name="item"></param>
        /// <returns>return 1 if sucsess and -1 when error</returns>
        public static bool UpdateShippingMethod(ShippingMethod item)
        {
            string cmd = item.Type == ShippingType.None
                        ? "UPDATE [Order].[ShippingMethod] SET [Name] = @Name,[Description] = @Description,[Enabled] = @Enabled,[SortOrder] = @SortOrder WHERE ShippingMethodID=@ShippingMethodID"
                        : "UPDATE [Order].[ShippingMethod] SET [ShippingType] = @ShippingType,[Name] = @Name,[Description] = @Description,[Enabled] = @Enabled,[SortOrder] = @SortOrder WHERE ShippingMethodID=@ShippingMethodID";
            SQLDataAccess.ExecuteNonQuery(cmd, CommandType.Text, new SqlParameter("@ShippingType", (int)item.Type),
                                              new SqlParameter("@Name", item.Name),
                                              new SqlParameter("@Description", item.Description),
                                              new SqlParameter("@Enabled", item.Enabled),
                                              new SqlParameter("@SortOrder", item.SortOrder),
                                              new SqlParameter("@ShippingMethodID", item.ShippingMethodId));
            return true;
        }

        public static void DeleteShippingMethod(int shippingId)
        {
            PhotoService.DeletePhotos(shippingId, PhotoType.Shipping);
            SQLDataAccess.ExecuteNonQuery("DELETE FROM [Order].[ShippingMethod] WHERE ShippingMethodID = @shippingId", CommandType.Text, new SqlParameter("@shippingId", shippingId));
        }

        /// <summary>
        /// gets list of shippingMethod by type and enabled
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<ShippingMethod> GetShippingMethodByType(ShippingType type)
        {
            List<ShippingMethod> items = SQLDataAccess.ExecuteReadList("SELECT * FROM [Order].[ShippingMethod] WHERE ShippingType = @ShippingType and enabled=1",
                                                       CommandType.Text, reader => GetShippingMethodFromReader(reader), new SqlParameter("@ShippingType", (int)type));
            return items;
        }


        //**** PARAMS
        public static Dictionary<string, string> GetShippingParams(int shippingMethodId)
        {
            return SQLDataAccess.ExecuteReadDictionary<string, string>("SELECT ParamName,ParamValue FROM [Order].[ShippingParam] WHERE ShippingMethodID = @ShippingMethodID",
               CommandType.Text, "ParamName", "ParamValue", new SqlParameter("@ShippingMethodID", shippingMethodId));
        }

        public static void InsertShippingParams(int shippingMethodId, Dictionary<string, string> parameters)
        {
            foreach (var parameter in parameters)
            {
                SQLDataAccess.ExecuteNonQuery("INSERT INTO [Order].[ShippingParam] ([ShippingMethodID],[ParamName],[ParamValue]) VALUES (@ShippingMethodID,@ParamName,@ParamValue)",
                                                CommandType.Text,
                                                new SqlParameter("@ShippingMethodID", shippingMethodId),
                                                new SqlParameter("@ParamName", parameter.Key),
                                                new SqlParameter("@ParamValue", parameter.Value));
            }
        }

        public static bool UpdateShippingParams(int shippingMethodId, Dictionary<string, string> parameters)
        {
            foreach (var parameter in parameters)
            {
                SQLDataAccess.ExecuteNonQuery(
                    @"if (SELECT COUNT(*) FROM [Order].[ShippingParam] WHERE [ShippingMethodID] = @ShippingMethodID AND [ParamName] = @ParamName) = 0
		                                            INSERT INTO [Order].[ShippingParam] ([ShippingMethodID], [ParamName], [ParamValue]) VALUES (@ShippingMethodID, @ParamName, @ParamValue)
	                                            else
		                                            UPDATE [Order].[ShippingParam] SET [ParamValue] = @ParamValue WHERE [ShippingMethodID] = @ShippingMethodID AND [ParamName] = @ParamName",
                    CommandType.Text,
                    new SqlParameter("@ShippingMethodID", shippingMethodId),
                    new SqlParameter("@ParamName", parameter.Key),
                    new SqlParameter("@ParamValue", parameter.Value));
            }
            return true;
        }
    }
}