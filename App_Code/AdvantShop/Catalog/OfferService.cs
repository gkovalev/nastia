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

namespace AdvantShop.Catalog
{
    public class OfferService
    {
        public static IList<Offer> GetOffersByProductId(int productId)
        {
            List<Offer> p = SQLDataAccess.ExecuteReadList<Offer>("[Catalog].[sp_GetProductOffersByProductID]",
                                                                   CommandType.StoredProcedure,
                                                                   reader => new Offer
                                                                        {
                                                                            Price = SQLDataHelper.GetDecimal(reader, "Price"),
                                                                            OfferListId = SQLDataHelper.GetInt(reader, "OfferListId"),
                                                                            Unit = SQLDataHelper.GetString(reader, "Unit"),
                                                                            Amount = SQLDataHelper.GetInt(reader, "Amount"),
                                                                            SupplyPrice = SQLDataHelper.GetDecimal(reader, "SupplyPrice"),
                                                                            ShippingPrice = SQLDataHelper.GetDecimal(reader, "ShippingPrice"),
                                                                            Multiplicity = SQLDataHelper.GetInt(reader, "Multiplicity"),
                                                                            MinAmount = SQLDataHelper.GetNullableInt(reader, "MinAmount"),
                                                                            MaxAmount = SQLDataHelper.GetNullableInt(reader, "MaxAmount")
                                                                        }, new SqlParameter("@ProductID", productId));
            return p;
        }

        public static void AddOfferToProduct(int poductid, Offer offr, SQLDataAccess da)
        {
            da.cmd.Parameters.Clear();
            da.cmd.Parameters.AddWithValue("@OfferListID", offr.OfferListId);
            da.cmd.Parameters.AddWithValue("@ProductID", poductid);
            da.cmd.Parameters.AddWithValue("@Amount", offr.Amount);
            da.cmd.Parameters.AddWithValue("@Price", offr.Price);
            da.cmd.Parameters.AddWithValue("@Unit", (offr.Unit ?? ((object)DBNull.Value)));
            da.cmd.Parameters.AddWithValue("@SupplyPrice", offr.SupplyPrice);
            da.cmd.Parameters.AddWithValue("@ShippingPrice", offr.ShippingPrice);
            da.cmd.Parameters.AddWithValue("@Multiplicity", offr.Multiplicity);
            da.cmd.Parameters.AddWithValue("@MaxAmount", offr.MaxAmount ?? (object)DBNull.Value);
            da.cmd.Parameters.AddWithValue("@MinAmount", offr.MinAmount ?? (object)DBNull.Value);
            da.cmd.ExecuteNonQuery();
        }

        public static void AddOfferToProduct(int productId, Offer offer)
        {
            SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_AddOffer]", CommandType.StoredProcedure,
                                            new SqlParameter("@OfferListID", offer.OfferListId),
                                            new SqlParameter("@ProductID", productId),
                                            new SqlParameter("@Amount", offer.Amount),
                                            new SqlParameter("@Price", offer.Price),
                                            new SqlParameter("@Unit", (offer.Unit ?? ((object)DBNull.Value))),
                                            new SqlParameter("@SupplyPrice", offer.SupplyPrice),
                                            new SqlParameter("@ShippingPrice", offer.ShippingPrice),
                                            new SqlParameter("@Multiplicity", offer.Multiplicity),
                                            new SqlParameter("@MaxAmount", offer.MaxAmount ?? (object)DBNull.Value),
                                            new SqlParameter("@MinAmount", offer.MinAmount ?? (object)DBNull.Value));
        }

        public static void AddOffersToProduct(int productid, IEnumerable<Offer> oflist)
        {
            using (var da = new SQLDataAccess())
            {
                da.cmd.CommandText = "[Catalog].[sp_AddOffer]";
                da.cmd.CommandType = CommandType.StoredProcedure;
                da.cnOpen();
                foreach (Offer offr in oflist)
                {
                    AddOfferToProduct(productid, offr, da);
                }
                da.cnClose();
            }
        }

        public static void UpdateOfferByProductId(int productId, Offer offer)
        {
            SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_UpdateInsertOffer]", CommandType.StoredProcedure,
                                            new SqlParameter("@OfferListID", offer.OfferListId),
                                            new SqlParameter("@ProductID", productId),
                                            new SqlParameter("@Amount", offer.Amount),
                                            new SqlParameter("@Price", offer.Price),
                                            new SqlParameter("@Unit", (offer.Unit ?? ((object)DBNull.Value))),
                                            new SqlParameter("@SupplyPrice", offer.SupplyPrice),
                                            new SqlParameter("@ShippingPrice", offer.ShippingPrice),
                                            new SqlParameter("@Multiplicity", offer.Multiplicity),
                                            new SqlParameter("@MaxAmount", offer.MaxAmount ?? (object)DBNull.Value),
                                            new SqlParameter("@MinAmount", offer.MinAmount ?? (object)DBNull.Value));
        }

        public static void UpdateOffersByProductId(int productId, List<Offer> offers)
        {
            foreach (var item in offers)
            {
                UpdateOfferByProductId(productId, item);
            }
        }
        
        /// <summary>
        /// get Offer by productid and offerid
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="offerListId"></param>
        /// <returns></returns>
        public static Offer GetOffer(int productId, int offerListId)
        {
            return SQLDataAccess.ExecuteReadOne<Offer>("[Catalog].[sp_GetProductOfferByID]", CommandType.StoredProcedure,
                                                        GetOfferFromReader,
                                                        new SqlParameter("@ProductID", productId),
                                                        new SqlParameter("@OfferListID", offerListId));
        }

        public static decimal GetOfferPrice(int productId, int offerListId)
        {
            return SQLDataAccess.ExecuteScalar<decimal>(
                "SELECT [Price] FROM [Catalog].[Offer] WHERE [ProductID] = @ProductID AND [OfferListID] = @OfferListID",
                CommandType.Text,
                new SqlParameter("@ProductID", productId),
                new SqlParameter("@OfferListID", offerListId));
        }

        public static List<Offer> GetProductOffers(int productID)
        {
            return SQLDataAccess.ExecuteReadList<Offer>(
                     "SELECT * FROM [Catalog].[Offer] WHERE [ProductID] = @ProductID",
                     CommandType.Text,
                     GetOfferFromReader,
                     new SqlParameter("@ProductID", productID));
        }

        public static Offer GetOfferFromReader(SqlDataReader reader)
        {
            return new Offer
            {
                Price = SQLDataHelper.GetDecimal(reader, "Price"),
                Amount = SQLDataHelper.GetInt(reader, "Amount"),
                Unit = SQLDataHelper.GetString(reader, "Unit"),
                SupplyPrice = SQLDataHelper.GetDecimal(reader, "SupplyPrice"),
                OfferListId = SQLDataHelper.GetInt(reader, "OfferListId"),
                ProductId = SQLDataHelper.GetInt(reader, "ProductID"),
                OfferId = SQLDataHelper.GetInt(reader, "OfferID"),
                ShippingPrice = SQLDataHelper.GetDecimal(reader, "ShippingPrice"),
                MinAmount = SQLDataHelper.GetNullableInt(reader, "MinAmount"),
                MaxAmount = SQLDataHelper.GetNullableInt(reader, "MaxAmount")
            };
        }

        public static int AddOfferList(string name)
        {
            return SQLDataAccess.ExecuteScalar<int>("INSERT INTO [Catalog].[OffersList] (Name) VALUES (@Name); select SCOPE_IDENTITY();",
                                                    CommandType.Text, new SqlParameter("@Name", name));
        }

        public static void DeleteOfferList(int id)
        {
            SQLDataAccess.ExecuteNonQuery("DELETE FROM [Catalog].[OffersList] WHERE [OfferListID] = @ID",
                                          CommandType.Text, new SqlParameter("@ID", id));
        }

        public static bool UpdateOfferList(int id, string name)
        {
            SQLDataAccess.ExecuteNonQuery("UPDATE [Catalog].[OffersList] SET [Name] = @Name WHERE [OfferListID] = @ID",
                                        CommandType.Text, new SqlParameter("@Name", name), new SqlParameter("@ID", id));
            return true;
        }
        public static IEnumerable<int> GetOfferListIds()
        {
            return SQLDataAccess.ExecuteReadColumnIEnumerable<int>("SELECT [OfferListID] FROM [Catalog].[OffersList]", CommandType.Text, "OfferListID");
        }

        public static OfferList GetOfferList(int offerListId)
        {
            return SQLDataAccess.ExecuteReadOne<OfferList>("SELECT * FROM [Catalog].[OffersList] WHERE [OfferListId] = @OfferListID", CommandType.Text,
                                                            reader => new OfferList
                                                                            {
                                                                                OfferListID = SQLDataHelper.GetInt(reader, "OfferListID"),
                                                                                StartDate = SQLDataHelper.GetDateTime(reader, "StartDate"),
                                                                                EndDate = SQLDataHelper.GetDateTime(reader, "EndDate"),
                                                                                Name = SQLDataHelper.GetString(reader, "Name")
                                                                            }, new SqlParameter("@OfferListID", offerListId));
        }
    }
}