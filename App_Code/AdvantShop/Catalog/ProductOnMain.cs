//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AdvantShop.Core;
using AdvantShop.Customers;

namespace AdvantShop.Catalog
{
    public static class ProductOnMain
    {
        public enum TypeFlag
        {
            None = 0,
            Bestseller = 1,
            New = 2,
            Discount = 3
        }

        public static List<int> GetProductIdByType(TypeFlag type)
        {
            string sqlCmd;
            switch (type)
            {
                case TypeFlag.Bestseller:
                    sqlCmd = "select ProductId from Catalog.Product where Bestseller=1";
                    break;
                case TypeFlag.New:
                    sqlCmd = "select ProductId from Catalog.Product where New=1";
                    break;
                case TypeFlag.Discount:
                    sqlCmd = "select ProductId from Catalog.Product where Discount > 0";
                    break;
                default:
                    throw new NotImplementedException();
            }
            return SQLDataAccess.ExecuteReadColumnIEnumerable<int>(sqlCmd, CommandType.Text, "ProductId", new SqlParameter { ParameterName = "@type", Value = (int)type }).ToList();
        }

        public static DataTable GetProductsByType(TypeFlag type, int count)
        {
            string sqlCmd;
            switch (type)
            {
                //Changed by Evgeni to order by random 
                //case TypeFlag.Bestseller:
                //    sqlCmd = "select Top(@count) Product.ProductId, ArtNo, Name, PhotoName as Photo, [Photo].[Description] AS PhotoDesc, Discount, Ratio, RatioID, OrderByRequest, Recomended, New, BestSeller, OnSale, UrlPath, ItemId, Price, Offer.Amount from Catalog.Product  LEFT JOIN [Catalog].[Offer] ON [Product].[ProductID] = [Offer].[ProductID] and offerListid=6 left Join Catalog.Photo on Product.ProductID=Photo.ObjId and Type=@Type and main=1 LEFT JOIN [Catalog].[ShoppingCart] ON [Catalog].[ShoppingCart].[EntityID] = [Catalog].[Product].[ProductID] AND [Catalog].[ShoppingCart].[ShoppingCartTypeId] = 3 AND [ShoppingCart].[CustomerID] = @CustomerId Left JOIN [Catalog].[Ratio] on Product.ProductId=Ratio.ProductID and Ratio.CustomerId=@CustomerId where Bestseller=1 and Enabled=1 and HirecalEnabled=1 and [Settings].[CountCategoriesByProduct](Product.ProductID) > 0 order by SortBestseller";
                //    break;
                //case TypeFlag.New:
                //    sqlCmd = "select Top(@count) Product.ProductId, ArtNo, Name, PhotoName as Photo, [Photo].[Description] AS PhotoDesc, Discount, Ratio, RatioID, OrderByRequest, Recomended, New, BestSeller, OnSale, UrlPath, ItemId, Price, Offer.Amount from Catalog.Product  LEFT JOIN [Catalog].[Offer] ON [Product].[ProductID] = [Offer].[ProductID] and offerListid=6 left Join Catalog.Photo on Product.ProductID=Photo.ObjId and Type=@Type and main=1 LEFT JOIN [Catalog].[ShoppingCart] ON [Catalog].[ShoppingCart].[EntityID] = [Catalog].[Product].[ProductID] AND [Catalog].[ShoppingCart].[ShoppingCartTypeId] = 3 AND [ShoppingCart].[CustomerID] = @CustomerId Left JOIN [Catalog].[Ratio] on Product.ProductId= Ratio.ProductID and Ratio.CustomerId=@CustomerId where New=1 and Enabled=1 and HirecalEnabled=1 and [Settings].[CountCategoriesByProduct](Product.ProductID) > 0 order by SortNew";
                //    break;
                //case TypeFlag.Discount:
                //    sqlCmd = "select Top(@count) Product.ProductId, ArtNo, Name, PhotoName as Photo, [Photo].[Description] AS PhotoDesc, Discount, Ratio, RatioID, OrderByRequest, Recomended, New, BestSeller, OnSale, UrlPath, ItemId, Price, Offer.Amount from Catalog.Product  LEFT JOIN [Catalog].[Offer] ON [Product].[ProductID] = [Offer].[ProductID] and offerListid=6 left Join Catalog.Photo on Product.ProductID=Photo.ObjId and Type=@Type and main=1 LEFT JOIN [Catalog].[ShoppingCart] ON [Catalog].[ShoppingCart].[EntityID] = [Catalog].[Product].[ProductID] AND [Catalog].[ShoppingCart].[ShoppingCartTypeId] = 3 AND [ShoppingCart].[CustomerID] = @CustomerId Left JOIN [Catalog].[Ratio] on Product.ProductId= Ratio.ProductID and Ratio.CustomerId=@CustomerId where Discount > 0 and Enabled=1 and HirecalEnabled=1 and [Settings].[CountCategoriesByProduct](Product.ProductID) > 0 order by SortDiscount";
                      //sqlCmd = "select Top(@count) Product.ProductId, ArtNo, Name, PhotoName as Photo, [Photo].[Description] AS PhotoDesc, Discount, Ratio, RatioID, OrderByRequest, Recomended, New, BestSeller, OnSale, UrlPath, ItemId, Price, Offer.Amount, Offer.MinAmount from Catalog.Product  LEFT JOIN [Catalog].[Offer] ON [Product].[ProductID] = [Offer].[ProductID] and offerListid=6 left Join Catalog.Photo on Product.ProductID=Photo.ObjId and Type=@Type and main=1 LEFT JOIN [Catalog].[ShoppingCart] ON [Catalog].[ShoppingCart].[EntityID] = [Catalog].[Product].[ProductID] AND [Catalog].[ShoppingCart].[ShoppingCartTypeId] = 3 AND [ShoppingCart].[CustomerID] = @CustomerId Left JOIN [Catalog].[Ratio] on Product.ProductId=Ratio.ProductID and Ratio.CustomerId=@CustomerId where Bestseller=1 and Enabled=1 and HirecalEnabled=1 and [Settings].[CountCategoriesByProduct](Product.ProductID) > 0 order by SortBestseller";
                //    break;
                case TypeFlag.Bestseller:
                    sqlCmd = "select Top(@count) Product.ProductId, ArtNo, Name, PhotoName as Photo,  [Photo].[Description] AS PhotoDesc ,Discount, Ratio, RatioID, OrderByRequest, Recomended, New, BestSeller, OnSale, UrlPath, ItemId, Price, Offer.Amount, Offer.MinAmount from Catalog.Product  LEFT JOIN [Catalog].[Offer] ON [Product].[ProductID] = [Offer].[ProductID] and offerListid=6 left Join Catalog.Photo on Product.ProductID=Photo.ObjId  and Type=@Type and main=1 LEFT JOIN [Catalog].[ShoppingCart] ON [Catalog].[ShoppingCart].[EntityID] = [Catalog].[Product].[ProductID] AND [Catalog].[ShoppingCart].[ShoppingCartTypeId] = 3 AND [ShoppingCart].[CustomerID] = @CustomerId Left JOIN [Catalog].[Ratio] on Product.ProductId=Ratio.ProductID and Ratio.CustomerId=@CustomerId where Bestseller=1 and Enabled=1 and HirecalEnabled=1 and [Settings].[CountCategoriesByProduct](Product.ProductID) > 0 ORDER BY SortBestseller";
                    break;
                case TypeFlag.New:
                    sqlCmd = "select Top(@count) Product.ProductId, ArtNo, Name, PhotoName as Photo, [Photo].[Description] AS PhotoDesc,Discount, Ratio, RatioID, OrderByRequest, Recomended, New, BestSeller, OnSale, UrlPath, ItemId, Price, Offer.Amount,  Offer.MinAmount from Catalog.Product  LEFT JOIN [Catalog].[Offer] ON [Product].[ProductID] = [Offer].[ProductID] and offerListid=6 left Join Catalog.Photo on Product.ProductID=Photo.ObjId  and Type=@Type and main=1 LEFT JOIN [Catalog].[ShoppingCart] ON [Catalog].[ShoppingCart].[EntityID] = [Catalog].[Product].[ProductID] AND [Catalog].[ShoppingCart].[ShoppingCartTypeId] = 3 AND [ShoppingCart].[CustomerID] = @CustomerId Left JOIN [Catalog].[Ratio] on Product.ProductId= Ratio.ProductID and Ratio.CustomerId=@CustomerId where New=1 and Enabled=1 and HirecalEnabled=1 and [Settings].[CountCategoriesByProduct](Product.ProductID) > 0 order by SortNew";
                    break;
                case TypeFlag.Discount:
                    sqlCmd = "select Top(@count) Product.ProductId, ArtNo, Name, PhotoName as Photo, [Photo].[Description] AS PhotoDesc,Discount, Ratio, RatioID, OrderByRequest, Recomended, New, BestSeller, OnSale, UrlPath, ItemId, Price, Offer.Amount, Offer.MinAmount from Catalog.Product  LEFT JOIN [Catalog].[Offer] ON [Product].[ProductID] = [Offer].[ProductID] and offerListid=6 left Join Catalog.Photo on Product.ProductID=Photo.ObjId  and Type=@Type and main=1 LEFT JOIN [Catalog].[ShoppingCart] ON [Catalog].[ShoppingCart].[EntityID] = [Catalog].[Product].[ProductID] AND [Catalog].[ShoppingCart].[ShoppingCartTypeId] = 3 AND [ShoppingCart].[CustomerID] = @CustomerId Left JOIN [Catalog].[Ratio] on Product.ProductId= Ratio.ProductID and Ratio.CustomerId=@CustomerId where Discount > 0 and Enabled=1 and HirecalEnabled=1 and [Settings].[CountCategoriesByProduct](Product.ProductID) > 0 order by SortDiscount";
                    break;
                default:
                    throw new NotImplementedException();
                //
            }
            return SQLDataAccess.ExecuteTable(sqlCmd, CommandType.Text, new SqlParameter { ParameterName = "@count", Value = count },
                                              new SqlParameter("@CustomerId", CustomerSession.CustomerId.ToString()),
                                              new SqlParameter("@Type", PhotoType.Product.ToString()));
        }

        public static DataTable GetAdminProductsByType(TypeFlag type, int count)
        {
            string sqlCmd;
            switch (type)
            {
                case TypeFlag.Bestseller:
                    sqlCmd = "select Top(@count) Product.ProductId, Name from Catalog.Product where Bestseller=1 order by SortBestseller";
                    break;
                case TypeFlag.New:
                    sqlCmd = "select Top(@count) Product.ProductId, Name from Catalog.Product where New=1 order by SortNew";
                    break;
                case TypeFlag.Discount:
                    sqlCmd = "select Top(@count) Product.ProductId, Name from Catalog.Product where Discount > 0 order by SortDiscount";
                    break;
                default:
                    throw new NotImplementedException();
            }
            return SQLDataAccess.ExecuteTable(sqlCmd, CommandType.Text, new SqlParameter { ParameterName = "@count", Value = count });
        }

        public static int GetProductCountByType(TypeFlag type)
        {
            string sqlCmd;
            switch (type)
            {
                case TypeFlag.Bestseller:
                    sqlCmd = "select  Count(ProductId) from Catalog.Product where Bestseller=1 and Enabled=1 and HirecalEnabled=1 and [Settings].[CountCategoriesByProduct](Product.ProductID) > 0";
                    break;
                case TypeFlag.New:
                    sqlCmd = "select  Count(ProductId) from Catalog.Product where New=1 and Enabled=1 and HirecalEnabled=1 and [Settings].[CountCategoriesByProduct](Product.ProductID) > 0 ";
                    break;
                case TypeFlag.Discount:
                    sqlCmd = "select  Count(ProductId) from Catalog.Product where Discount > 0 and Enabled=1 and HirecalEnabled=1 and [Settings].[CountCategoriesByProduct](Product.ProductID) > 0";
                    break;
                default:
                    throw new NotImplementedException();
            }
            return SQLDataAccess.ExecuteScalar<int>(sqlCmd, CommandType.Text);
        }

        public static void AddProductByType(int productId, int sortOrder, TypeFlag type)
        {
            string sqlCmd;
            switch (type)
            {
                case TypeFlag.Bestseller:
                    sqlCmd = "Update Catalog.Product set SortBestseller=@Sort, Bestseller=1 where ProductId=@productId";
                    break;
                case TypeFlag.New:
                    sqlCmd = "Update Catalog.Product set SortNew=@Sort, New=1 where ProductId=@productId";
                    break;
                case TypeFlag.Discount:
                    sqlCmd = "Update Catalog.Product set SortDiscount=@Sort where ProductId=@productId";
                    break;
                default:
                    return;
            }
            SQLDataAccess.ExecuteNonQuery(sqlCmd, CommandType.Text,
                                            new SqlParameter { ParameterName = "@productId", Value = productId },
                                            new SqlParameter { ParameterName = "@Sort", Value = sortOrder }
                                            );
        }

        public static void AddProductByType(int productId, TypeFlag type)
        {
            string sqlCmd;
            switch (type)
            {
                case TypeFlag.Bestseller:
                    sqlCmd = "Update Catalog.Product set SortBestseller=(Select min(SortBestseller)-10 from Catalog.Product), Bestseller=1 where ProductId=@productId";
                    break;
                case TypeFlag.New:
                    sqlCmd = "Update Catalog.Product set SortNew=(Select min(SortNew)-10 from Catalog.Product), New=1 where ProductId=@productId";
                    break;
                case TypeFlag.Discount:
                    sqlCmd = "Update Catalog.Product set SortDiscount=(Select min(SortDiscount)-10 from Catalog.Product) where ProductId=@productId";
                    break;
                default:
                    return;
            }
            SQLDataAccess.ExecuteNonQuery(sqlCmd, CommandType.Text, new SqlParameter { ParameterName = "@productId", Value = productId });
        }

        public static void DeleteProductByType(int prodcutId, TypeFlag type)
        {
            string sqlCmd;
            switch (type)
            {
                case TypeFlag.Bestseller:
                    sqlCmd = "Update Catalog.Product set SortBestseller=0, Bestseller=0 where ProductId=@productId";
                    break;
                case TypeFlag.New:
                    sqlCmd = "Update Catalog.Product set SortNew=0, New=0 where ProductId=@productId";
                    break;
                case TypeFlag.Discount:
                    sqlCmd = "Update Catalog.Product set SortDiscount=0, Discount =0 where ProductId=@productId";
                    break;
                default:
                    return;
            }

            SQLDataAccess.ExecuteNonQuery(sqlCmd, CommandType.Text,
                                            new SqlParameter { ParameterName = "@productId", Value = prodcutId }
                                            );
        }

        public static void UpdateProductByType(int productId, int sortOrder, TypeFlag type)
        {
            string sqlCmd;
            switch (type)
            {
                case TypeFlag.Bestseller:
                    sqlCmd = "Update Catalog.Product set SortBestseller=@sortOrder where ProductId=@productId and Bestseller=1";
                    break;
                case TypeFlag.New:
                    sqlCmd = "Update Catalog.Product set SortNew=@sortOrder where ProductId=@productId and New=1";
                    break;
                case TypeFlag.Discount:
                    sqlCmd = "Update Catalog.Product set SortDiscount=@sortOrder where ProductId=@productId";
                    break;
                default:
                    return;
            }

            SQLDataAccess.ExecuteNonQuery(sqlCmd, CommandType.Text,
                                            new SqlParameter { ParameterName = "@productId", Value = productId },
                                            new SqlParameter { ParameterName = "@sortOrder", Value = sortOrder }
                                            );
        }
    }
}