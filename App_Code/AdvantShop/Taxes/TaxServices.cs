//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using AdvantShop.Orders;
using Resources;

namespace AdvantShop.Taxes
{

    public class TaxServices
    {
        public static void SwitchProductTax(int productId, int taxId, bool enable, bool recalcSelectedParent)
        {
            SQLDataAccess.ExecuteNonQuery(enable ? "[Catalog].[sp_SetProductTax]" : "[Catalog].[sp_UnSetProductTax]",
                                            CommandType.StoredProcedure,
                                            new SqlParameter("@ProductID", productId),
                                            new SqlParameter("@TaxId", taxId));
            foreach (var category in ProductService.GetCategoriesIDsByProductId(productId))
            {
                TaxLasyLoad.RecalcCategory(category, taxId);
            }
        }

        public static void ClearTaxMappingByProduct(int productId)
        {
            SQLDataAccess.ExecuteNonQuery("delete from Catalog.TaxMappingOnProduct where ProductID=@ProductID",
                                            CommandType.Text,
                                            new SqlParameter("@ProductID", productId));
        }

        public static void ClearTaxMappingByTax(int taxId)
        {
            SQLDataAccess.ExecuteNonQuery("delete from Catalog.TaxMappingOnProduct where TaxId=@TaxId",
                                            CommandType.Text,
                                            new SqlParameter("@TaxId", taxId));
        }

        public static void CreateTax(TaxElement t)
        {
            using (var da = new SQLDataAccess())
            {
                da.cmd.CommandText = "INSERT INTO [Catalog].[Tax]([Name], [Enabled], [Priority], [DependsOnAddress], [ShowInPrice], [RegNumber], [CountryID], [RateType], [FederalRate]) VALUES (@name, @enabled, @priority, @dependsOnAddress, @showInPrice, @regNumber, @countryId, @rateType, @federalRate); SELECT scope_identity()";
                da.cmd.CommandType = CommandType.Text;

                da.cmd.Parameters.Clear();
                da.cmd.Parameters.AddWithValue("@name", t.Name);
                da.cmd.Parameters.AddWithValue("@enabled", t.Enabled);
                da.cmd.Parameters.AddWithValue("@priority", t.Priority);
                da.cmd.Parameters.AddWithValue("@dependsOnAddress", (int)t.DependsOnAddress);
                da.cmd.Parameters.AddWithValue("@showInPrice", t.ShowInPrice);
                da.cmd.Parameters.AddWithValue("@regNumber", t.RegNumber ?? (object)DBNull.Value);
                da.cmd.Parameters.AddWithValue("@countryId", t.CountryID);
                da.cmd.Parameters.AddWithValue("@rateType", (int)t.Type);
                da.cmd.Parameters.AddWithValue("@federalRate", t.FederalRate);

                da.cnOpen();
                t.TaxId = SQLDataHelper.GetInt(da.cmd.ExecuteScalar());
                da.cnClose();

                if (t.RegionalRates != null)
                {
                    da.cnOpen();
                    da.cmd.CommandText = "INSERT INTO [Catalog].[TaxRegionRate] ([TaxId], [RegionID], [RegionRate]) VALUES (@TaxId, @regionId, @regionRate)";

                    foreach (var rr in t.RegionalRates)
                    {
                        da.cmd.Parameters.Clear();
                        da.cmd.Parameters.AddWithValue("@TaxId", t.TaxId);
                        da.cmd.Parameters.AddWithValue("@regionId", rr.RegionId);
                        da.cmd.Parameters.AddWithValue("@regionRate", rr.Rate);
                        da.cmd.ExecuteNonQuery();
                    }
                    da.cnClose();
                }
            }
        }

        public static TaxElement GetTax(int id)
        {
            return SQLDataAccess.ExecuteReadOne("SELECT * FROM [Catalog].[Tax] WHERE [TaxId] = @id", CommandType.Text, ReadTax, new SqlParameter("@id", id));
        }

        public static bool AddRegionRateToTax(int taxId, RegionalRate rate)
        {
            SQLDataAccess.ExecuteNonQuery("INSERT INTO [Catalog].[TaxRegionRate] ([TaxId], [RegionID], [RegionRate]) VALUES (@taxId, @regionId, @rate)",
                                            CommandType.Text,
                                            new SqlParameter("@taxId", taxId),
                                            new SqlParameter("@regionId", rate.RegionId),
                                            new SqlParameter("@rate", rate.Rate));
            return true;
        }

        public static void RemoveRegionalRateFromTax(int taxId, int regionId)
        {
            SQLDataAccess.ExecuteNonQuery("DELETE FROM [Catalog].[TaxRegionRate] WHERE [TaxId] = @taxId AND [RegionID] = @regionId",
                                            CommandType.Text,
                                            new SqlParameter("@taxId", taxId),
                                            new SqlParameter("@regionId", regionId));
        }

        public static void UpdateRegionalRate(int taxId, int regionId, decimal rate)
        {
            SQLDataAccess.ExecuteNonQuery("UPDATE [Catalog].[TaxRegionRate] SET [RegionRate] = @rate WHERE [TaxId] = @taxId AND [RegionID] = @regionId",
                                            CommandType.Text,
                                            new SqlParameter("@taxId", taxId),
                                            new SqlParameter("@regionId", regionId),
                                            new SqlParameter("@rate", rate));
        }

        public static void UpdateTax(TaxElement t)
        {
            using (var da = new SQLDataAccess())
            {
                da.cmd.CommandText = @"UPDATE [Catalog].[Tax] SET [Name] = @name, [Enabled] = @enabled, [Priority] = @priority, [DependsOnAddress] = @dependsOnAddress, 
                                                                  [ShowInPrice] = @showInPrice, [RegNumber] =  @regNumber, [CountryID] = @countryId, 
                                                                  [RateType] = @rateType, [FederalRate] = @federalRate 
                                       WHERE [TaxId] = @TaxId";
                da.cmd.CommandType = CommandType.Text;

                da.cmd.Parameters.Clear();
                da.cmd.Parameters.AddWithValue("@TaxId", t.TaxId);
                da.cmd.Parameters.AddWithValue("@name", t.Name);
                da.cmd.Parameters.AddWithValue("@enabled", t.Enabled);
                da.cmd.Parameters.AddWithValue("@priority", t.Priority);
                da.cmd.Parameters.AddWithValue("@dependsOnAddress", (int)t.DependsOnAddress);
                da.cmd.Parameters.AddWithValue("@showInPrice", t.ShowInPrice);
                da.cmd.Parameters.AddWithValue("@regNumber", t.RegNumber ?? (object)DBNull.Value);
                da.cmd.Parameters.AddWithValue("@countryId", t.CountryID);
                da.cmd.Parameters.AddWithValue("@rateType", (int)t.Type);
                da.cmd.Parameters.AddWithValue("@federalRate", t.FederalRate);

                da.cnOpen();

                da.cmd.ExecuteNonQuery();

                t.RegionalRates.Any(); //DO NOT DELETE!!! предзагрузка региональных ставок

                da.cmd.CommandText = "DELETE FROM [Catalog].[TaxRegionRate] WHERE [TaxId] = @TaxId";
                da.cmd.Parameters.Clear();
                da.cmd.Parameters.AddWithValue("@TaxId", t.TaxId);
                da.cmd.ExecuteNonQuery();

                da.cnClose();

                if (t.RegionalRates.Count > 0)
                {
                    da.cnOpen();
                    da.cmd.CommandText = "INSERT INTO [Catalog].[TaxRegionRate]([TaxId], [RegionID], [RegionRate]) VALUES (@TaxId, @regionId, @regionRate)";

                    foreach (var rr in t.RegionalRates)
                    {
                        da.cmd.Parameters.Clear();
                        da.cmd.Parameters.AddWithValue("@TaxId", t.TaxId);
                        da.cmd.Parameters.AddWithValue("@regionId", rr.RegionId);
                        da.cmd.Parameters.AddWithValue("@regionRate", rr.Rate);
                        da.cmd.ExecuteNonQuery();
                    }
                    da.cnClose();
                }
            }
        }

        public static bool DeleteTax(int taxId)
        {
            SQLDataAccess.ExecuteNonQuery("DELETE FROM [Catalog].[Tax] WHERE [TaxId] = @TaxId",
                                            CommandType.Text,
                                            new SqlParameter("@TaxId", taxId));
            return true;
        }

        public static List<int> GetAllTaxesIDs()
        {
            List<int> result = SQLDataAccess.ExecuteReadList<int>("select [TaxId] from [Catalog].[Tax]", CommandType.Text,
                                                             reader => SQLDataHelper.GetInt(reader, "TaxId"));
            return result;
        }

        public static ICollection<RegionalRate> GetRegionalRatesForTax(int t)
        {
            return SQLDataAccess.ExecuteReadList("SELECT * FROM [Catalog].[TaxRegionRate] WHERE [TaxId] = @id",
                                             CommandType.Text,
                                             reader => new RegionalRate
                                                         {
                                                             RegionId = SQLDataHelper.GetInt(reader, "RegionID"),
                                                             Rate = SQLDataHelper.GetDecimal(reader, "RegionRate")
                                                         },
                                             new SqlParameter("@id", t));
        }

        public static ICollection<TaxElement> GetSelectedTaxesForProduct(int productId)
        {
            return SQLDataAccess.ExecuteReadList<TaxElement>(@"Select * from Catalog.Tax where Catalog.Tax.enabled = 'true' and Tax.TaxId in (select TaxID from Catalog.TaxMappingOnProduct where TaxMappingOnProduct.ProductId=@productId )",
                                            CommandType.Text, ReadTax, new SqlParameter("@ProductID", productId));
        }

        public static ICollection<TaxElement> GetTaxesForProduct(int productId, CustomerContact billingContact, CustomerContact shippingContact)
        {
            var selC = SettingProvider.GetSellerContact();
            return GetTaxesForProduct(productId, selC, billingContact, shippingContact);
        }

        public static ICollection<TaxElement> GetTaxesForProduct(int productId, CustomerContact sellerContact, CustomerContact billingContact, CustomerContact shippingContact)
        {
            using (var da = new SQLDataAccess())
            {
                da.cmd.CommandText = @"
                                        SELECT [Catalog].[Tax].*, [Catalog].[TaxRegionRate].[RegionID], [Catalog].[TaxRegionRate].[RegionRate]
                                        FROM [Catalog].[Tax]
                                        JOIN [Catalog].[TaxMappingOnProduct] ON [Tax].[TaxId] = [TaxMappingOnProduct].[TaxId]
                                        LEFT JOIN [Catalog].[TaxRegionRate] ON [Tax].[TaxId] = [TaxRegionRate].[TaxId] AND (
		                                        ([DependsOnAddress] = @default  AND [CountryID] = @sellerCountry   AND [RegionID] = @sellerRegion) OR
		                                        ([DependsOnAddress] = @shipping AND [CountryID] = @shippingCountry AND [RegionID] = @shippingRegion) OR
		                                        ([DependsOnAddress] = @billing  AND [CountryID] = @billingCountry  AND [RegionID] = @billingRegion)
	                                        )
                                        WHERE [ProductID] = @ProductID AND [Enabled] = 'True'
                                    ";
                da.cmd.CommandType = CommandType.Text;
                da.cmd.Parameters.AddWithValue("@default", (int)TypeRateDepends.Default);
                da.cmd.Parameters.AddWithValue("@shipping", (int)TypeRateDepends.ByShippingAddress);
                da.cmd.Parameters.AddWithValue("@billing", (int)TypeRateDepends.ByBillingAddress);
                da.cmd.Parameters.AddWithValue("@sellerCountry", sellerContact.CountryId);
                da.cmd.Parameters.AddWithValue("@sellerRegion", sellerContact.RegionId.HasValue ? sellerContact.RegionId : (object)DBNull.Value);
                da.cmd.Parameters.AddWithValue("@shippingCountry", shippingContact.CountryId);
                da.cmd.Parameters.AddWithValue("@shippingRegion", shippingContact.RegionId.HasValue ? shippingContact.RegionId : (object)DBNull.Value);
                da.cmd.Parameters.AddWithValue("@billingCountry", billingContact.CountryId);
                da.cmd.Parameters.AddWithValue("@billingRegion", billingContact.RegionId.HasValue ? billingContact.RegionId : (object)DBNull.Value);
                da.cmd.Parameters.AddWithValue("@ProductID", productId);

                da.cnOpen();
                var result = new List<TaxElement>();
                using (SqlDataReader reader = da.cmd.ExecuteReader())
                    while (reader.Read())
                    {
                        var t = ReadTax(reader);

                        var regionalRateCollection = new List<RegionalRate>();
                        t.RegionalRates = regionalRateCollection;
                        if (!(reader["RegionID"] is DBNull))
                        {
                            var regionalRate = new RegionalRate
                            {
                                RegionId = SQLDataHelper.GetInt(reader, "RegionID"),
                                Rate = SQLDataHelper.GetDecimal(reader, "RegionRate")
                            };
                            regionalRateCollection.Add(regionalRate);

                        }
                        result.Add(t);
                    }

                return result;
            }
        }

        public static decimal CalculateTax(OrderItem orderItem, TaxElement tax)
        {
            switch (tax.Type)
            {
                case RateType.LumpSum:
                    return tax.RegionalRates.Count > 0 ? tax.RegionalRates.First().Rate : tax.FederalRate;

                case RateType.Proportional:
                    {
                        decimal returnTax = tax.RegionalRates.Count > 0 ? tax.RegionalRates.First().Rate : tax.FederalRate;

                        if (tax.ShowInPrice)
                        {
                            returnTax = returnTax * orderItem.Price * orderItem.Amount / (100.0M + returnTax);
                        }
                        else
                        {
                            returnTax = returnTax * orderItem.Price * orderItem.Amount / 100.0M;
                        }

                        return returnTax;
                    }

                default:
                    return 0;

                //case RateType.Progressive:
                //    //TODO: доделать работу с прогрессивной ставкой налога
                //    return 0;                
            }
        }

        private static TaxElement ReadTax(SqlDataReader reader)
        {
            var t = new TaxElement
            {
                TaxId = SQLDataHelper.GetInt(reader, "TaxId"),
                CountryID = SQLDataHelper.GetInt(reader, "CountryID"),
                Enabled = SQLDataHelper.GetBoolean(reader, "Enabled"),
                DependsOnAddress = (TypeRateDepends)SQLDataHelper.GetInt(reader, "DependsOnAddress"),
                Name = SQLDataHelper.GetString(reader, "Name"),
                Priority = SQLDataHelper.GetInt(reader, "Priority"),
                FederalRate = SQLDataHelper.GetDecimal(reader, "FederalRate"),
                Type = (RateType)SQLDataHelper.GetInt(reader, "RateType"),
                RegNumber = SQLDataHelper.GetString(reader, "RegNumber"),
                ShowInPrice = SQLDataHelper.GetBoolean(reader, "ShowInPrice")
            };
            return t;
        }


        public static List<TaxValue> GetOrderTaxes(int orderid)
        {
            List<TaxValue> returnList = SQLDataAccess.ExecuteReadList("select taxId, taxName, taxSum, taxShowInPrice from [Order].[OrderTax] where orderid=@orderid",
                                                            CommandType.Text, read => new TaxValue
                                                                                          {
                                                                                              TaxID = SQLDataHelper.GetInt(read, "taxId"),
                                                                                              TaxName = SQLDataHelper.GetString(read, "taxName"),
                                                                                              TaxSum = SQLDataHelper.GetDecimal(read, "taxSum"),
                                                                                              TaxShowInPrice = SQLDataHelper.GetBoolean(read, "taxShowInPrice")
                                                                                          },
                                                            new SqlParameter("@orderid", orderid));
            return returnList;
        }

        public static void SetOrderTaxes(int orderId, List<TaxValue> taxValues)
        {
            using (var db = new SQLDataAccess())
            {
                db.cmd.CommandText = "insert into [Order].[OrderTax] (TaxID, TaxName, TaxSum, TaxShowInPrice, OrderId) values (@TaxID, @TaxName, @TaxSum, @TaxShowInPrice, @OrderId)";
                db.cmd.CommandType = CommandType.Text;
                db.cnOpen();
                foreach (var taxValue in taxValues)
                {
                    db.cmd.Parameters.Clear();
                    db.cmd.Parameters.AddWithValue("@OrderId", orderId);
                    db.cmd.Parameters.AddWithValue("@TaxID", taxValue.TaxID);
                    db.cmd.Parameters.AddWithValue("@TaxName", taxValue.TaxName);
                    db.cmd.Parameters.AddWithValue("@TaxSum", taxValue.TaxSum);
                    db.cmd.Parameters.AddWithValue("@TaxShowInPrice", taxValue.TaxShowInPrice);
                    db.cmd.ExecuteNonQuery();
                }
                db.cnClose();
            }
        }
        public static void ClearOrderTaxes(int orderId)
        {
            SQLDataAccess.ExecuteNonQuery("delete from [Order].[OrderTax] where [OrderId] = @OrderId",
                                            CommandType.Text ,
                                            new SqlParameter("@OrderId", orderId));
        }


        public static decimal CalculateTaxesTotal(ShoppingCartItem basketItem, CustomerContact shippingContact, CustomerContact billingContact)
        {
            var orderItem = (OrderItem)basketItem;
            return CalculateTaxesTotal(orderItem, shippingContact, billingContact);
        }

        public static decimal CalculateTaxesTotal(OrderItem orderItem, CustomerContact shippingContact, CustomerContact billingContact)
        {
            ICollection<TaxElement> taxes = GetTaxesForProduct(orderItem.EntityId, billingContact, shippingContact);
            return taxes.Where(tax => !tax.ShowInPrice).Sum(tax => CalculateTax(orderItem, tax));
        }



        public static string BuildTaxTable(List<TaxValue> taxes, decimal currentCurrencyRate, string currentCurrencyIso3, string message)
        {
            var sb = new StringBuilder();
            if (!taxes.Any())
            {
                sb.Append("<tr><td style=\"background-color: #FFFFFF; text-align: right\">");
                sb.Append(message);
                sb.Append("&nbsp;</td><td style=\"background-color: #FFFFFF; width: 150px\">");
                sb.Append(CatalogService.GetStringPrice(0, currentCurrencyRate, currentCurrencyIso3));
                sb.Append("</td></tr>");
            }
            else
                foreach (TaxValue tax in taxes)
                {
                    sb.Append("<tr><td style=\"background-color: #FFFFFF; text-align: right\">");
                    sb.Append((tax.TaxShowInPrice ? Resource.Core_TaxServices_Include_Tax : "") + " " + tax.TaxName);
                    sb.Append(":&nbsp</td><td style=\"background-color: #FFFFFF; width: 150px\">" + (tax.TaxShowInPrice ? "" : "+"));
                    sb.Append(CatalogService.GetStringPrice(tax.TaxSum, currentCurrencyRate, currentCurrencyIso3));
                    sb.Append("</td></tr>");
                }
            return sb.ToString();
        }

        public static Dictionary<TaxElement, decimal> GetTaxItems(IList<OrderItem> shoppingCartitems, CustomerContact shippingContact, CustomerContact billingContact)
        {
            var taxesItems = new Dictionary<TaxElement, decimal>();

            foreach (var item in shoppingCartitems)
            {
                var t = (List<TaxElement>)GetTaxesForProduct(item.EntityId, billingContact, shippingContact);
                foreach (var tax in t)
                {
                    if (taxesItems.ContainsKey(tax))
                    {
                        taxesItems[tax] += CalculateTax(item, tax);
                    }
                    else
                    {
                        taxesItems.Add(tax, CalculateTax(item, tax));
                    }
                }
            }
            return taxesItems;
        }
    }
}