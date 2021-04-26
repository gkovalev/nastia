//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using AdvantShop.Core;
using AdvantShop.Core.Caching;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.FullSearch;
using AdvantShop.Helpers;
using AdvantShop.SaasData;
using AdvantShop.SEO;
using Resources;

namespace AdvantShop.Catalog
{
    public class ProductService
    {
        #region Categories

        public static void SetProductHierarchicallyEnabled(int productId)
        {
            SQLDataAccess.ExecuteNonQuery("[Catalog].[SetProductHierarchicallyEnabled]", CommandType.StoredProcedure,
                                            new SqlParameter("@ProductId", productId));
        }

        /// <summary>
        /// get first categoryId by productId(сделал инклуд индекс)
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static int GetFirstCategoryIdByProductId(int productId)
        {
            int categoryId = SQLDataHelper.GetInt(SQLDataAccess.ExecuteScalar("[Catalog].[sp_GetCategoryIDByProductID]", CommandType.StoredProcedure, new SqlParameter("@ProductID", productId)), CategoryService.DefaultNonCategoryId);
            return categoryId;
        }

        /// <summary>
        /// get categoryIds by productId
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static IEnumerable<int> GetCategoriesIDsByProductId(int productId)
        {
            return SQLDataAccess.ExecuteReadColumnIEnumerable<int>("[Catalog].[sp_GetCategoriesIDsByProductId]",
                                                                   CommandType.StoredProcedure,
                                                                   "CategoryID",
                                                                   new SqlParameter("@ProductID", productId));
        }

        /// <summary>
        /// get categories by productId
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static List<Category> GetCategoriesByProductId(int productId)
        {
            var res = SQLDataAccess.ExecuteReadList("[Catalog].[sp_GetProductCategories]", CommandType.StoredProcedure, CategoryService.GetCategoryFromReader, new SqlParameter("@ProductID", productId));
            return res;
        }

        /// <summary>
        /// create html for tooltip
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static string CreateTooltipContent(int productId)
        {
            var res = new StringBuilder();
            var content = new StringBuilder();
            int categoryCounter = 0;
            try
            {
                using (var db = new SQLDataAccess())
                {
                    db.cmd.CommandText = "[Catalog].[sp_GetCategoriesPathesByProductID]";
                    db.cmd.CommandType = CommandType.StoredProcedure;
                    db.cmd.Parameters.Clear();
                    db.cmd.Parameters.AddWithValue("@ProductID", productId);

                    db.cnOpen();
                    using (SqlDataReader reader = db.cmd.ExecuteReader())

                        while (reader.Read())
                        {
                            content.Append("<br/>&nbsp;&nbsp;&nbsp;" + SQLDataHelper.GetString(reader, "CategoryPath"));
                            categoryCounter++;
                        }

                    db.cnClose();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
            if (categoryCounter > 0)
            {
                var strHead = new StringBuilder();
                strHead.Append("<div class=\'tooltipDiv\'><span class=\'tooltipBold\'>");
                strHead.Append(string.Format(Resource.Admin_CategoriesService_ProductInCategories, categoryCounter));
                strHead.Append("<br/><div style=\'height:5px;width:0px;\' />");
                strHead.Append(Resource.Admin_CategoriesService_Categories);

                res.Append(strHead);
                res.Append(content);
                res.Append("</span></div>");
            }
            else
            {
                res.Append("");
            }

            return res.ToString();
        }

        /// <summary>
        /// get count of categories by productId
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static int GetCountOfCategoriesByProductId(int productId)
        {
            var count = SQLDataAccess.ExecuteScalar<int>("[Catalog].[sp_GetCOUNTOfCategoriesByProductID]", CommandType.StoredProcedure, new SqlParameter("@ProductID", productId));
            return count;
        }

        #endregion

        #region Related Products

        /// <summary>
        /// Add related product
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="relatedProductId"></param>
        /// <param name="relatedType"></param>
        public static void AddRelatedProduct(int productId, int relatedProductId, RelatedType relatedType)
        {
            SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_AddRelatedProduct]", CommandType.StoredProcedure,
                                                new SqlParameter("@ProductID", productId),
                                                new SqlParameter("@RelatedProductID", relatedProductId),
                                                new SqlParameter("@RelatedType", (int)relatedType));
        }

        /// <summary>
        /// delete ralated product
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="relatedProductId"></param>
        /// <param name="relatedType"></param>
        public static void DeleteRelatedProduct(int productId, int relatedProductId, RelatedType relatedType)
        {
            SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_DeleteRelatedProduct]", CommandType.StoredProcedure,
                                            new SqlParameter("@ProductID", productId),
                                            new SqlParameter("@RelatedProductID", relatedProductId),
                                            new SqlParameter("@RelatedType", (int)relatedType));
        }

        private static void DeleteRelatedProducts(int productId)
        {
            SQLDataAccess.ExecuteNonQuery("Delete from catalog.RelatedProducts Where ProductId=@ProductID Or LinkedProductID=@ProductID", CommandType.Text, new SqlParameter("@ProductID", productId));
        }

        /// <summary>
        /// Get related products
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="relatedType"></param>
        /// <returns></returns>
        public static List<Product> GetRelatedProducts(int productId, RelatedType relatedType)
        {
            List<Product> res = SQLDataAccess.ExecuteReadList<Product>("[Catalog].[sp_GetRelatedProducts]", CommandType.StoredProcedure,
                                                              reader => new Product
                                                                            {
                                                                                ProductId = SQLDataHelper.GetInt(reader, "ProductId"),
                                                                                ArtNo = SQLDataHelper.GetString(reader, "ArtNo"),
                                                                                Photo = SQLDataHelper.GetString(reader, "Photo"),
                                                                                PhotoDesc = SQLDataHelper.GetString(reader, "PhotoDesc"),
                                                                                Name = SQLDataHelper.GetString(reader, "Name"),
                                                                                BriefDescription = SQLDataHelper.GetString(reader, "BriefDescription"),
                                                                                Discount = SQLDataHelper.GetDecimal(reader, "Discount"),
                                                                                UrlPath = SQLDataHelper.GetString(reader, "UrlPath"),
                                                                            },
                                                                            new SqlParameter("@ProductID", productId),
                                                                            new SqlParameter("@RelatedType", (int)relatedType),
                                                                            new SqlParameter("@Type", PhotoType.Product.ToString())
                                                                        );
            return res;
        }
        #endregion

        #region Get Add Update Delete

        /// <summary>
        /// delete product by productId
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="sentToLuceneIndex"></param>
        /// <returns></returns>
        public static bool DeleteProduct(int productId, bool sentToLuceneIndex)
        {
            PhotoService.DeletePhotos(productId, PhotoType.Product);
            DeleteRelatedProducts(productId);
            SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_DeleteProduct]", CommandType.StoredProcedure, new SqlParameter("@ProductID", productId));
            CategoryService.ClearCategoryCache();
            if (sentToLuceneIndex)
                LuceneSearch.ClearLuceneIndexRecord(productId);
            return true;
        }

        /// <summary>
        /// add product
        /// </summary>
        /// <param name="product"></param>
        /// <param name="sentToLuceneIndex"></param>
        /// <returns></returns>
        public static int AddProduct(Product product, bool sentToLuceneIndex)
        {
            if (SaasDataService.IsSaasEnabled && GetProductCountByOffer(6) >= SaasDataService.CurrentSaasData.ProductsCount)
            {
                return 0;
            }

            product.ProductId = Convert.ToInt32(SQLDataAccess.ExecuteScalar("[Catalog].[sp_AddProduct]",
                CommandType.StoredProcedure,
                new SqlParameter("@ArtNo", product.ArtNo),
                new SqlParameter("@Name", product.Name),
                new SqlParameter("@Ratio", product.Ratio),
                new SqlParameter("@Discount", product.Discount),
                new SqlParameter("@Weight", product.Weight),
                new SqlParameter("@Size", (product.Size ?? ((object)DBNull.Value))),
                new SqlParameter("@IsFreeShipping", product.IsFreeShipping),
                new SqlParameter("@ItemsSold", product.ItemsSold),
                new SqlParameter("@BriefDescription", (product.BriefDescription ?? ((object)DBNull.Value))),
                new SqlParameter("@Description", (product.Description ?? ((object)DBNull.Value))),
                new SqlParameter("@Enabled", product.Enabled),
                new SqlParameter("@Recomended", product.Recomended),
                new SqlParameter("@New", product.New),
                new SqlParameter("@BestSeller", product.BestSeller),
                new SqlParameter("@OnSale", product.OnSale),
                new SqlParameter("@OrderByRequest", product.OrderByRequest),
                new SqlParameter("@BrandID", (product.BrandId != 0 ? product.BrandId : (object)DBNull.Value)),
                new SqlParameter("@UrlPath", product.UrlPath)
                ));
            if (product.ProductId == 0)
                return 0;
            //by default in bd set ID if artNo is Null
            if (string.IsNullOrEmpty(product.ArtNo))
                product.ArtNo = product.ProductId.ToString(CultureInfo.InvariantCulture);

            SetProductHierarchicallyEnabled(product.ProductId);

            // ---- Offers
            if (product.Offers != null && product.Offers.Count != 0)
            {
                OfferService.AddOffersToProduct(product.ProductId, product.Offers.Where(o => o.OfferId == 0));
            }
            // ---- Meta
            if (product.Meta != null)
            {
                if (!product.Meta.Title.IsNullOrEmpty() || !product.Meta.MetaKeywords.IsNullOrEmpty() || !product.Meta.MetaDescription.IsNullOrEmpty())
                {
                    product.Meta.ObjId = product.ProductId;
                    MetaInfoService.SetMeta(product.Meta);
                }
            }

            if (sentToLuceneIndex)
                LuceneSearch.AddUpdateLuceneIndex(new SampleData(product.ProductId, product.ArtNo, product.Name));

            return product.ProductId;
        }

        /// <summary>
        /// update product by artno
        /// </summary>
        /// <param name="product"></param>
        /// <param name="sentToLuceneIndex"></param>
        /// <returns></returns>
        public static bool UpdateProductByArtNo(Product product, bool sentToLuceneIndex)
        {
            product.ProductId = SQLDataAccess.ExecuteScalar<int>("SELECT [ProductID] FROM [Catalog].[Product] WHERE [ArtNo] = @ArtNo",
                                                                   CommandType.Text, new SqlParameter("@ArtNo", product.ArtNo));
            if (product.ProductId > 0)
                return UpdateProduct(product, sentToLuceneIndex);
            return false;
        }

        /// <summary>
        /// update product by productId
        /// </summary>
        /// <param name="product"></param>
        /// <param name="sentToLuceneIndex"></param>
        /// <returns></returns>
        public static bool UpdateProduct(Product product, bool sentToLuceneIndex)
        {
            using (var db = new SQLDataAccess())
            {
                db.cnOpen();
                UpdateProduct(product, sentToLuceneIndex, db);
                db.cnClose();
            }
            return true;
        }

        /// <summary>
        /// update product
        /// </summary>
        /// <param name="product"></param>
        /// <param name="sentToLuceneIndex"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static bool UpdateProduct(Product product, bool sentToLuceneIndex, SQLDataAccess db)
        {
            db.cmd.CommandText = "[Catalog].[sp_UpdateProductById]";
            db.cmd.CommandType = CommandType.StoredProcedure;

            db.cmd.Parameters.Clear();
            db.cmd.Parameters.AddWithValue("@ArtNo", product.ArtNo);
            db.cmd.Parameters.AddWithValue("@Name", product.Name);
            db.cmd.Parameters.AddWithValue("@ProductID", product.ProductId);
            db.cmd.Parameters.AddWithValue("@Ratio", product.Ratio);
            db.cmd.Parameters.AddWithValue("@Discount", product.Discount);
            db.cmd.Parameters.AddWithValue("@Weight", product.Weight);
            db.cmd.Parameters.AddWithValue("@Size", (product.Size ?? ((object)DBNull.Value)));
            db.cmd.Parameters.AddWithValue("@IsFreeShipping", product.IsFreeShipping);
            db.cmd.Parameters.AddWithValue("@ItemsSold", product.ItemsSold);
            db.cmd.Parameters.AddWithValue("@BriefDescription",
                                                     (product.BriefDescription ?? ((object)DBNull.Value)));
            db.cmd.Parameters.AddWithValue("@Description",
                                                     (product.Description ?? ((object)DBNull.Value)));
            db.cmd.Parameters.AddWithValue("@Enabled", product.Enabled);
            db.cmd.Parameters.AddWithValue("@OrderByRequest", product.OrderByRequest);
            db.cmd.Parameters.AddWithValue("@Recomended", product.Recomended);
            db.cmd.Parameters.AddWithValue("@New", product.New);
            db.cmd.Parameters.AddWithValue("@BestSeller", product.BestSeller);
            db.cmd.Parameters.AddWithValue("@OnSale", product.OnSale);
            db.cmd.Parameters.AddWithValue("@BrandID", product.BrandId == 0 ? ((object)DBNull.Value) : product.BrandId);
            db.cmd.Parameters.AddWithValue("@UrlPath", product.UrlPath);
            db.cmd.ExecuteNonQuery();

            SetProductHierarchicallyEnabled(product.ProductId);

            if (product.Offers != null && product.Offers.Count > 0)
            {
                OfferService.UpdateOffersByProductId(product.ProductId, product.Offers);
            }

            if (product.Meta != null)
            {
                if (product.Meta.Title.IsNullOrEmpty() && product.Meta.MetaKeywords.IsNullOrEmpty() && product.Meta.MetaDescription.IsNullOrEmpty())
                {
                    if (MetaInfoService.IsMetaExist(product.ProductId, MetaType.Product))
                        MetaInfoService.DeleteMetaInfo(product.ProductId, MetaType.Product);
                }
                else
                    MetaInfoService.SetMeta(product.Meta);
            }

            if (sentToLuceneIndex)
                LuceneSearch.AddUpdateLuceneIndex(new SampleData(product.ProductId, product.ArtNo, product.Name));
            return true;
        }

        public static Product GetProductFromReader(SqlDataReader reader)
        {
            return new Product
                       {
                           ProductId = SQLDataHelper.GetInt(reader, "ProductId"),
                           ArtNo = SQLDataHelper.GetString(reader, "ArtNo"),
                           Name = SQLDataHelper.GetString(reader, "Name"),
                           BriefDescription = SQLDataHelper.GetString(reader, "BriefDescription", string.Empty),
                           Description = SQLDataHelper.GetString(reader, "Description", string.Empty),
                           Photo = SQLDataHelper.GetString(reader, "Photo"),
                           Discount = SQLDataHelper.GetDecimal(reader, "Discount"),
                           Size = SQLDataHelper.GetString(reader, "Size"),
                           Weight = SQLDataHelper.GetDecimal(reader, "Weight"),
                           Ratio = SQLDataHelper.GetDouble(reader, "Ratio"),
                           Enabled = SQLDataHelper.GetBoolean(reader, "Enabled", true),
                           OrderByRequest = SQLDataHelper.GetBoolean(reader, "OrderByRequest"),
                           Recomended = SQLDataHelper.GetBoolean(reader, "Recomended"),
                           New = SQLDataHelper.GetBoolean(reader, "New"),
                           BestSeller = SQLDataHelper.GetBoolean(reader, "Bestseller"),
                           OnSale = SQLDataHelper.GetBoolean(reader, "OnSale"),
                           BrandId = SQLDataHelper.GetInt(reader, "BrandID", 0),
                           UrlPath = SQLDataHelper.GetString(reader, "UrlPath"),
                           HirecalEnabled = SQLDataHelper.GetBoolean(reader, "HirecalEnabled"),
                           //Added by Evgeni to add EAN and SubbrandiD
                           EAN = SQLDataHelper.GetString(reader, "EAN"),
                           SubBrandId = SQLDataHelper.GetInt(reader, "SubBrandID", 0),
                           ManufactureArtNo = SQLDataHelper.GetString(reader, "ManufactureArtNo")
                           //
                       };
        }

        /// <summary>
        /// get product by productId
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static Product GetProduct(int productId)
        {
            var p = SQLDataAccess.ExecuteReadOne("[Catalog].[sp_GetProductById]", CommandType.StoredProcedure, GetProductFromReader,
                                                new SqlParameter("@ProductID", productId), new SqlParameter("@Type", PhotoType.Product.ToString()));
            return p;
        }

        public static int GetProductId(string artNo)
        {
            //Changed by Evgeni to get products with dotes

            if (artNo.Length == 10 )
            {

                string artNoWithDotes = artNo.Insert(7, ".").Insert(4, ".").Insert(1, ".");
                return SQLDataAccess.ExecuteScalar<int>("SELECT ProductID FROM [Catalog].[Product] WHERE ArtNo IN (@artNo,@artNoWithDotes)", CommandType.Text, new SqlParameter("@artNo", artNo), new SqlParameter("@artNoWithDotes", artNoWithDotes));

            }
            return SQLDataAccess.ExecuteScalar<int>("SELECT ProductID FROM [Catalog].[Product] WHERE ArtNo IN (@artNo)", CommandType.Text, new SqlParameter("@artNo", artNo));
        }

        //Added by Evgeni
        public static List<int> GetProductIdWithSameManufactureId(string ManufactureArtNo)
        {
            //Changed by Evgeni to get products with dotes

            var res = SQLDataAccess.ExecuteReadList<int>("SELECT ProductID FROM [Catalog].[Product] WHERE ManufactureArtNo IN (@ManufactureArtNo)",
                                                       CommandType.Text,
                                                       reader => SQLDataHelper.GetInt(reader, "ProductID"),
                                                       new SqlParameter("@ManufactureArtNo", ManufactureArtNo));
            return res;
        }

        /// <summary>
        /// get product by artNo
        /// </summary>
        /// <param name="artNo"></param>
        /// <returns></returns>
        public static Product GetProduct(string artNo)
        {
            var productId = GetProductId(artNo);
            return productId > 0 ? GetProduct(productId) : null;
        }


        /// <summary>
        /// Get all products id
        /// </summary>
        /// <returns></returns>
        public static List<int> GetProductsIDs()
        {
            return SQLDataAccess.ExecuteReadColumnIEnumerable<int>("SELECT [ProductID] FROM [Catalog].[Product]", CommandType.Text, "ProductID").ToList();
        }
        #endregion

        #region ProductLinks


        public static int DeleteAllProductLink(int productId)
        {
            var res = SQLDataAccess.ExecuteReadList<int>("Select [CategoryID] FROM [Catalog].[ProductCategories] WHERE [ProductID] =  @ProductId",
                                                        CommandType.Text,
                                                        reader => SQLDataHelper.GetInt(reader, "CategoryID"),
                                                        new SqlParameter("@ProductID", productId));
            foreach (var item in res)
            {
                CacheManager.Remove(CacheNames.GetCategoryCacheObjectName(item));
            }

            SQLDataAccess.ExecuteNonQuery("DELETE FROM [Catalog].[ProductCategories] WHERE [ProductID] =  @ProductId", CommandType.Text, new SqlParameter("@ProductID", productId));

            return 0;
        }
        /// <summary>
        /// delete relationship between product and category
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="catId"></param>
        /// <returns></returns>
        public static int DeleteProductLink(int productId, int catId)
        {
            SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_RemoveProductFromCategory]", CommandType.StoredProcedure, new SqlParameter("@ProductID", productId), new SqlParameter("@CategoryID", catId));
            CacheManager.Remove(CacheNames.GetCategoryCacheObjectName(catId));

            return 0;
        }

        /// <summary>
        /// enabled active trigger
        /// </summary>
        public static void EnableDynamicProductLinkRecalc()
        {
            SQLDataAccess.ExecuteNonQuery("ALTER TABLE [Catalog].[ProductCategories] ENABLE TRIGGER [InsertProductInCategory];" +
                                         " ALTER TABLE [Catalog].[ProductCategories] ENABLE TRIGGER [RemoveProductFromCategory];" +
                                         " ALTER TABLE [Catalog].[Product] ENABLE TRIGGER [EnabledChanged];", CommandType.Text);
        }

        /// <summary>
        /// disabled active trigger
        /// </summary>
        public static void DisableDynamicProductLinkRecalc()
        {
            SQLDataAccess.ExecuteNonQuery("ALTER TABLE [Catalog].[ProductCategories] DISABLE TRIGGER [InsertProductInCategory];" +
                                         " ALTER TABLE [Catalog].[ProductCategories] DISABLE TRIGGER [RemoveProductFromCategory];" +
                                         " ALTER TABLE [Catalog].[Product] DISABLE TRIGGER [EnabledChanged];",
                                            CommandType.Text);
        }

        /// <summary>
        /// add relationship between product and category
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="catId"></param>
        /// <returns></returns>
        public static int AddProductLink(int productId, int catId)
        {
            SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_AddProductToCategory]", CommandType.StoredProcedure,
                                            new SqlParameter("@ProductID", productId), new SqlParameter("@CategoryID", catId));
            CategoryService.ClearCategoryCache();
            return 1;
        }

        /// <summary>
        /// Update relationship
        /// </summary>
        /// <param name="productid"></param>
        /// <param name="sort"></param>
        /// <param name="cat"></param>
        /// <returns></returns>
        public static bool UpdateProductLinkSort(int productid, int sort, int cat)
        {
            SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_UpdateProductLinkSort]", CommandType.StoredProcedure,
                                            new SqlParameter { ParameterName = "@ProductID", Value = productid },
                                            new SqlParameter { ParameterName = "@CategoryID", Value = cat },
                                            new SqlParameter { ParameterName = "@SortOrder", Value = sort });

            return true;
        }

        #endregion

        #region Is Enabled
        /// <summary>
        /// Cheak if product enabled
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static bool IsProductEnabled(int productId)
        {
            var res = SQLDataAccess.ExecuteScalar<bool>("SELECT ([Enabled] & [HirecalEnabled]) as Enabled FROM [Catalog].[Product] WHERE [ProductID] = @id", CommandType.Text, new SqlParameter("@id", productId));

            return res;
        }

        /// <summary>
        /// disabled all products
        /// </summary>
        public static void DisableAllProducts()
        {
            SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_DisableAllProducts]", CommandType.StoredProcedure);
            CategoryService.ClearCategoryCache();
        }

        #endregion

        #region Filtered Select
        /// <summary>
        /// get all products
        /// </summary>
        /// <returns></returns>
        public static IList<Product> GetProducts()
        {
            List<Product> res;
            using (var db = new SQLDataAccess())
            {
                db.cnOpen();
                res = (List<Product>)GetProducts(db);
                db.cnClose();
            }
            return res;
        }


        /// <summary>
        /// Get products without category
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<int> GetProductIDsWithoutCategory()
        {
            return SQLDataAccess.ExecuteReadColumnIEnumerable<int>(
                    "SELECT [Product].[ProductID] FROM [Catalog].[Product] WHERE [Product].[ProductID] not in (select distinct [ProductID] from [Catalog].[ProductCategories])",
                    CommandType.Text,
                    "ProductID");
        }

        /// <summary>
        /// get products in categories
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<int> GetProductIDsInCategories()
        {
            return SQLDataAccess.ExecuteReadColumnIEnumerable<int>(
                    "select distinct [ProductID] from [Catalog].[ProductCategories]",
                    CommandType.Text,
                    "ProductID");
        }

        /// <summary>
        /// get all products
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static IList<Product> GetProducts(SQLDataAccess db)
        {
            return GetProductsWhere(db, null);
        }

        /// <summary>
        /// Gets products with condition
        /// </summary>
        /// <param name="db"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static IList<Product> GetProductsWhere(SQLDataAccess db, string condition)
        {
            var products = new List<Product>();

            //Changed by Evgeni to insert EAN, SubBrandId
//            const string queryFormat = @"select [Product].[ProductID],[ArtNo],[Name],[PhotoName] as [Photo],[Ratio],[Discount],[Weight],[UrlPath],[Size],[IsFreeShipping],[ItemsSold],[BriefDescription],[Product].[Description],[Enabled],[HirecalEnabled],[DateAdded],[DateModified],[Recomended],[New],[BestSeller],[OnSale],[BrandId],[OrderByRequest] FROM [Catalog].[Product]
//		                                LEFT JOIN [Catalog].[Photo] ON [Photo].[ObjId] = [Product].[ProductID] AND Type =@Type  AND [Main] = 1 {0}";

            const string queryFormat = @"select [Product].[ProductID],[ArtNo],[Name],[PhotoName] as [Photo],[Ratio],[Discount],[Weight],[UrlPath],[Size],[IsFreeShipping],[ItemsSold],[BriefDescription],[Product].[Description],[Enabled],[HirecalEnabled],[DateAdded],[DateModified],[Recomended],[New],[BestSeller],[OnSale],[BrandId],[OrderByRequest],ISNULL(EAN,0) as [EAN],ISNULL ([SubBrandID],0) as [SubBrandID],ISNULL ([ManufactureArtNo],0) as [ManufactureArtNo]  FROM [Catalog].[Product]
		                                LEFT JOIN [Catalog].[Photo] ON [Photo].[ObjId] = [Product].[ProductID] AND Type =@Type  AND [Main] = 1 {0}";

            db.cmd.CommandText = string.Format(queryFormat, condition == null ? "" : " WHERE " + condition);
            db.cmd.CommandType = CommandType.Text;
            db.cmd.Parameters.Add(new SqlParameter("@Type", PhotoType.Product.ToString()));
            using (var reader = db.cmd.ExecuteReader())
                while (reader.Read())
                {
                    products.Add(GetProductFromReader(reader));
                }
            return products;
        }

        #endregion

        #region Products Count and Existance
        /// <summary>
        /// get products count
        /// </summary>
        /// <returns></returns>
        public static int GetProductsCount()
        {
            var res = SQLDataAccess.ExecuteScalar<int>("SELECT Count([ProductID]) FROM [Catalog].[Product]", CommandType.Text);
            return res;
        }

        /// <summary>
        /// get product count by offer
        /// </summary>
        /// <returns></returns>
        public static int GetProductCountByOffer(int offerListId)
        {
            var res = SQLDataAccess.ExecuteScalar<int>("SELECT count(Catalog.Product.ProductID) FROM Catalog.Product INNER JOIN Catalog.Offer ON Catalog.Product.ProductID = Catalog.Offer.ProductID and OfferListId=@OfferListId",
                                                       CommandType.Text, new SqlParameter("@OfferListId", offerListId));
            return res;
        }

        /// <summary>
        /// cheak exist product by productid
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static bool IsExists(int productId)
        {
            bool boolres = SQLDataAccess.ExecuteScalar<int>("[Catalog].[sp_GetProductCOUNTbyID]", CommandType.StoredProcedure, new SqlParameter("@ProductID", productId)) > 0;
            return boolres;
        }

        #endregion

        #region Offers
        #endregion

        #region Photos

        /// <summary>
        /// Adding photo to product by ArtNo
        /// </summary>
        /// <param name="artNo">Product ArtNo</param>
        /// <param name="fullfileName"></param>
        /// <param name="description">Photo description</param>
        /// <param name="isMain"></param>
        /// <remarks></remarks>
        public static void AddProductPhotoByArtNo(string artNo, string fullfileName, string description, bool isMain)
        {
            AddProductPhotoByProductId(GetProductId(artNo), fullfileName, description, isMain);
        }

        public static void AddProductPhotoByProductId(int productId, string fullfilename, string description, bool isMain)
        {
            if (string.IsNullOrWhiteSpace(fullfilename) || (!IsExists(productId)))
            {
                return;
            }

            var tempName = PhotoService.AddPhoto(new Photo(0, productId, PhotoType.Product)
                                                   {
                                                       Description = description,
                                                       OriginName = Path.GetFileName(fullfilename),
                                                       PhotoSortOrder = 0
                                                   });
            if (string.IsNullOrWhiteSpace(tempName)) return;
            using (var image = Image.FromFile(fullfilename))
            {
                FileHelpers.SaveProductImageUseCompress(tempName, image);
            }
        }

        #endregion

        #region Product Price Change

        public static void IncrementAllProductsPrice(decimal value, bool percent, bool bySupply)
        {
            ChangeAllProductsPrice(value, percent, false, bySupply);
        }

        public static void DecrementAllProductsPrice(decimal value, bool percent, bool bySupply)
        {
            ChangeAllProductsPrice(value, percent, true, bySupply);
        }

        private static void ChangeAllProductsPrice(decimal value, bool percent, bool negative, bool bySupply)
        {
            SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_ChangeAllProductsPrice]", CommandType.StoredProcedure,
                                                new SqlParameter("@Value", value),
                                                new SqlParameter("@Percent", percent),
                                                new SqlParameter("@Negative", negative),
                                                new SqlParameter("@bySupply", bySupply)
                                                );

        }
        #endregion

        public static void SetMainLink(int productId, int categoryId)
        {
            SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_SetMainCategoryLink]", CommandType.StoredProcedure, new SqlParameter("@ProductID", productId), new SqlParameter("@CategoryID", categoryId));
        }

        public static bool IsMainLink(int productId, int categoryId)
        {
            return
                SQLDataAccess.ExecuteReadColumn<bool>(
                    "SELECT Main FROM [Catalog].[ProductCategories] WHERE [ProductID] = @ProductID AND [CategoryID] = @CategoryID",
                    CommandType.Text, "Main", new SqlParameter("@ProductID", productId),
                    new SqlParameter("@CategoryID", categoryId)).FirstOrDefault();
        }

        public static decimal CalculateProductPrice(decimal price, decimal productDiscount, CustomerGroup customerGroup, IList<EvaluatedCustomOptions> customOptions, bool withProductDiscount)
        {
            decimal customOptionPrice = 0;
            if (customOptions != null)
            {
                customOptionPrice = CustomOptionsService.GetCustomOptionPrice(price, customOptions);
            }

            //Changed by Evgeni to show discount for group and product discount
            decimal finalDiscount = 0;
            if (!withProductDiscount)
            {
                productDiscount = 0;
            }
            else
            {

                decimal groupDiscount = customerGroup.CustomerGroupId == 0 ? 0 : customerGroup.GroupDiscount;

                finalDiscount = Math.Max(productDiscount, groupDiscount);
            }
            //

            return (price + customOptionPrice) * (100 - finalDiscount) / 100;
        }


        public static void SetActive(int productId, bool active)
        {
            SQLDataAccess.ExecuteNonQuery(
                 "Update [Catalog].[Product] Set Enabled = @Enabled Where ProductID = @ProductID",
                 CommandType.Text,
                 new SqlParameter("@ProductID", productId),
                 new SqlParameter("@Enabled", active));
        }

        public static List<Product> GetProductsByCategoryForExport(int categoryId)
        {
            var productList = new List<Product>();
            using (var db = new SQLDataAccess())
            {
                db.cmd.CommandText =
                    @" SELECT [TProduct].[ProductId], [Name], [Enabled], [Price], [Amount],[Unit] FROM [Catalog].[Product] AS [TProduct]
                                            INNER JOIN [Catalog].[ProductCategories] on [ProductCategories].[ProductId] = [TProduct].[ProductId]
                                            INNER JOIN [Catalog].[Offer] on [Offer].[ProductID] = [TProduct].[ProductId]
                                        WHERE [ProductCategories].[CategoryId] = @CategoryId AND [TProduct].[Enabled] = 1 AND [TProduct].[HirecalEnabled] = 1 AND [Price] > 0 AND [Amount] > 0 AND [OfferListId] = 6 AND
                                            (SELECT TOP(1) [ProductCategories].[CategoryId] FROM [Catalog].[ProductCategories] INNER JOIN [Catalog].[Category] on [Category].[CategoryId] = [ProductCategories].[CategoryId] WHERE [ProductID] = [TProduct].[ProductID] AND [Enabled] = 1 AND [HirecalEnabled] = 1 AND [Main] = 1) = @CategoryId";
                db.cmd.CommandType = CommandType.Text;
                db.cmd.Parameters.Clear();
                db.cmd.Parameters.AddWithValue("@CategoryId", categoryId);

                db.cnOpen();
                using (var reader = db.cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var offer = new Offer
                        {
                            OfferListId = 6,
                            Amount = SQLDataHelper.GetInt(reader, "Amount"),
                            Price = SQLDataHelper.GetDecimal(reader, "Price"),
                            Unit = SQLDataHelper.GetString(reader, "Unit")
                        };

                        productList.Add(new Product
                        {
                            ProductId = SQLDataHelper.GetInt(reader, "ProductId"),
                            Name = SQLDataHelper.GetString(reader, "Name"),
                            Enabled = SQLDataHelper.GetBoolean(reader, "Enabled"),
                            Offers = new List<Offer> { offer }
                        });
                    }
                    reader.Close();
                }
                db.cnClose();

            }
            return productList;
        }

        public static void SetBrand(int productId, int brandId)
        {
            SQLDataAccess.ExecuteNonQuery("UPDATE [Catalog].[Product] SET BrandID = @BrandID WHERE ProductID = @ProductID", CommandType.Text, new SqlParameter("@ProductID", productId), new SqlParameter("@BrandID", brandId));
        }

        public static void DeleteBrand(int productId)
        {
            SQLDataAccess.ExecuteNonQuery("UPDATE [Catalog].[Product] SET BrandID = NULL Where ProductID = @ProductID", CommandType.Text, new SqlParameter("@ProductID", productId));
        }

        public static List<string> GetForAutoCompleteByIds(string productIds)
        {
            return SQLDataAccess.ExecuteReadList<string>("Select Product.Name, Product.ArtNo from Catalog.Product "
                                                         + " inner join (select item, sort from [Settings].[ParsingBySeperator](@productIds,'/') ) as dtt on Product.ProductId=convert(int, dtt.item) "
                                                         + " order by dtt.sort",
                                                   CommandType.Text,
                                                   reader => string.Format("{0}<span>({1})</span>", SQLDataHelper.GetString(reader, "Name"), SQLDataHelper.GetString(reader, "ArtNo")),
                                                   new SqlParameter("@productIds", productIds));
        }
    }
}