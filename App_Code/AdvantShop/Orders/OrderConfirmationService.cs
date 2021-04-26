//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Data;
using System.Data.SqlClient;
using AdvantShop.Core;
using AdvantShop.Helpers;
using Newtonsoft.Json;

namespace AdvantShop.Orders
{
    public class OrderConfirmationService
    {
        public enum BuyInOneclickPage
        {
            details,
            shoppingcart
        }

        #region OrderConfirmation

        public static bool IsExist(Guid customerId)
        {
            return SQLDataAccess.ExecuteScalar<int>("Select Count(CustomerId) from [Order].OrderConfirmation where CustomerId=@CustomerId", CommandType.Text,
                                                    new SqlParameter { ParameterName = "@CustomerId", Value = customerId }) > 0;
        }

        public static OrderConfirmation Get(Guid customerId)
        {
            return SQLDataAccess.ExecuteReadOne<OrderConfirmation>("select * from [Order].OrderConfirmation where CustomerId=@CustomerId", CommandType.Text, GetFromReader,
                                                                    new SqlParameter { ParameterName = "@CustomerId", Value = customerId });
        }

        public static void Add(OrderConfirmation item)
        {
            SQLDataAccess.ExecuteNonQuery("insert into [Order].OrderConfirmation ([CustomerId],[OrderConfirmationData],LastUpdate) values (@CustomerId,@OrderConfirmationData,GetDate())",
                                              CommandType.Text,
                                              new SqlParameter { ParameterName = "@CustomerId", Value = item.CustomerId },
                                              new SqlParameter { ParameterName = "@OrderConfirmationData", Value = JsonConvert.SerializeObject(item.OrderConfirmationData) }
                                              );

        }

        public static void Update(OrderConfirmation item)
        {
            SQLDataAccess.ExecuteNonQuery("update [Order].OrderConfirmation set OrderConfirmationData=@OrderConfirmationData,LastUpdate=GetDate() where CustomerId=@CustomerId",
                                            CommandType.Text,
                                            new SqlParameter { ParameterName = "@CustomerId", Value = item.CustomerId },
                                            new SqlParameter { ParameterName = "@OrderConfirmationData", Value = JsonConvert.SerializeObject(item.OrderConfirmationData) }
                                            );
        }

        public static void Delete(Guid customerId)
        {
            SQLDataAccess.ExecuteNonQuery("delete from [Order].OrderConfirmation where CustomerId=@CustomerId", CommandType.Text, new SqlParameter { ParameterName = "@CustomerId", Value = customerId });
        }

        public static void DeleteExpired()
        {
            SQLDataAccess.ExecuteNonQuery("delete from [Order].OrderConfirmation where DATEADD(month, 1, LastUpdate) > GetDate()", CommandType.Text);
        }

        private static OrderConfirmation GetFromReader(SqlDataReader reader)
        {
            return new OrderConfirmation
            {
                CustomerId = SQLDataHelper.GetGuid(reader, "CustomerId"),
                OrderConfirmationData = JsonConvert.DeserializeObject<OrderConfirmationData>(SQLDataHelper.GetString(reader, "OrderConfirmationData")),
                LastUpdate = SQLDataHelper.GetDateTime(reader, "LastUpdate", DateTime.MinValue)

            };
        }

        #endregion region
    }
}