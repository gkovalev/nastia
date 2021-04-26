//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.SaasData;
using AdvantShop.Statistic;
using Resources;

namespace AdvantShop.ExportImport
{
    public class ImportProduct
    {
        private static bool useMultiThreadImport = false;
        //Changed by Evgeni to avoid updates price for on sales products
        private static bool isNeedUpdatePriceForOnSalesProductsGlobal = true;

        //Changed by Evgeni to avoid updates price for on sales products
        public static void UpdateInsertProduct(Dictionary<ProductFields.Fields, string> productInStrings, bool isNeedUpdatePriceForOnSalesProducts = true)
        {
            //Changed by Evgeni to avoid updates price for on sales products
            isNeedUpdatePriceForOnSalesProductsGlobal = isNeedUpdatePriceForOnSalesProducts;
            if (useMultiThreadImport)
            {
                var added = false;
                while (!added)
                {
                    int workerThreads;
                    int asyncIoThreads;
                    ThreadPool.GetAvailableThreads(out workerThreads, out asyncIoThreads);
                    if (workerThreads != 0)
                    {
                        ThreadPool.QueueUserWorkItem(UpdateInsertProductWorker, productInStrings);
                        added = true;
                    }
                    else
                    {
                        Thread.Sleep(1000);
                    }
                }
            }
            else
            {
                UpdateInsertProductWorker(productInStrings);
            }
        }

        private static void UpdateInsertProductWorker(object o)
        {
            //return;

            var productInStrings = (Dictionary<ProductFields.Fields, string>)o;
            try
            {
                bool addingNew;
                Product product = null;
                var artNo = productInStrings.ContainsKey(ProductFields.Fields.Sku) ? productInStrings[ProductFields.Fields.Sku] : string.Empty;
                if (string.IsNullOrEmpty(artNo))
                {
                    addingNew = true;
                }
                else
                {
                    product = ProductService.GetProduct(artNo);
                    addingNew = product == null;
                }

                if (addingNew)
                {
                    product = new Product { ArtNo = string.IsNullOrEmpty(artNo) ? null : artNo };
                }

                if (productInStrings.ContainsKey(ProductFields.Fields.Name))
                    product.Name = productInStrings[ProductFields.Fields.Name];
                else
                    product.Name = product.Name ?? string.Empty;

                if (productInStrings.ContainsKey(ProductFields.Fields.Enabled))
                    product.Enabled = productInStrings[ProductFields.Fields.Enabled].Trim().Equals("+");

                if (productInStrings.ContainsKey(ProductFields.Fields.OrderByRequest))
                    product.OrderByRequest = productInStrings[ProductFields.Fields.OrderByRequest].Trim().Equals("+");

                if (productInStrings.ContainsKey(ProductFields.Fields.Discount))
                    product.Discount = Convert.ToDecimal(productInStrings[ProductFields.Fields.Discount]);

                if (productInStrings.ContainsKey(ProductFields.Fields.Weight))
                    product.Weight = Convert.ToDecimal(productInStrings[ProductFields.Fields.Weight]);

                if (productInStrings.ContainsKey(ProductFields.Fields.Size))
                    product.Size = GetSizeForBdFormat(productInStrings[ProductFields.Fields.Size]);

                if (productInStrings.ContainsKey(ProductFields.Fields.BriefDescription))
                    product.BriefDescription = productInStrings[ProductFields.Fields.BriefDescription];

                if (productInStrings.ContainsKey(ProductFields.Fields.Description))
                    product.Description = productInStrings[ProductFields.Fields.Description];

                if (product.Offers.Count == 0) product.Offers.Add(new Offer { OfferListId = CatalogService.DefaultOfferListId });

              //Changed by Evgeni to avoid updates price for on sales products
                if (productInStrings.ContainsKey(ProductFields.Fields.Price))
                {
                    if (product.Discount > 0 && isNeedUpdatePriceForOnSalesProductsGlobal == false)
                    {
                        //Update Product discount and price according to the algorithm
                       product.Offers[0].Price = Convert.ToDecimal(productInStrings[ProductFields.Fields.Price]) / (1.000m - (product.Discount/100));
                        //
                       // Log("Product Price for " + product.ArtNo + " was not updated");
                    }
                    else
                    {
                        product.Offers[0].Price = Convert.ToDecimal(productInStrings[ProductFields.Fields.Price]);
                    }
                }

                if (productInStrings.ContainsKey(ProductFields.Fields.PurchasePrice))
                    product.Offers[0].SupplyPrice = Convert.ToDecimal(productInStrings[ProductFields.Fields.PurchasePrice]);

                if (productInStrings.ContainsKey(ProductFields.Fields.ShippingPrice))
                    product.Offers[0].ShippingPrice = Convert.ToDecimal(productInStrings[ProductFields.Fields.ShippingPrice]);

                if (productInStrings.ContainsKey(ProductFields.Fields.Amount))
                    product.Offers[0].Amount = Convert.ToInt32(productInStrings[ProductFields.Fields.Amount]);

                if (productInStrings.ContainsKey(ProductFields.Fields.Unit))
                    product.Offers[0].Unit = productInStrings[ProductFields.Fields.Unit];

                if (productInStrings.ContainsKey(ProductFields.Fields.ParamSynonym))
                {
                    var prodUrl = productInStrings[ProductFields.Fields.ParamSynonym].IsNotEmpty() ? productInStrings[ProductFields.Fields.ParamSynonym] : product.ArtNo;
                    product.UrlPath = UrlService.GetEvalibleValidUrl(product.ID, ParamType.Product, prodUrl);
                }
                else product.UrlPath = product.UrlPath ?? UrlService.GetEvalibleValidUrl(product.ID, ParamType.Product, product.ArtNo);

                if (productInStrings.ContainsKey(ProductFields.Fields.Title))
                    product.Meta.Title = productInStrings[ProductFields.Fields.Title];
                else
                    product.Meta.Title = product.Meta.Title ?? SettingsSEO.ProductMetaTitle;

                if (productInStrings.ContainsKey(ProductFields.Fields.MetaKeywords))
                    product.Meta.MetaKeywords = productInStrings[ProductFields.Fields.MetaKeywords];
                else
                    product.Meta.MetaKeywords = product.Meta.MetaKeywords ?? SettingsSEO.ProductMetaKeywords;

                if (productInStrings.ContainsKey(ProductFields.Fields.MetaDescription))
                    product.Meta.MetaDescription = productInStrings[ProductFields.Fields.MetaDescription];
                else
                    product.Meta.MetaDescription = product.Meta.MetaDescription ?? SettingsSEO.ProductMetaDescription;

                if (productInStrings.ContainsKey(ProductFields.Fields.Markers))
                    ParseMarkers(product, productInStrings[ProductFields.Fields.Markers]);

                if (productInStrings.ContainsKey(ProductFields.Fields.Producer))
                {
                    if (string.IsNullOrWhiteSpace(productInStrings[ProductFields.Fields.Producer]))
                    {
                        product.BrandId = 0;
                    }
                    else if (!BrandService.IsExist(productInStrings[ProductFields.Fields.Producer]))
                    {
                        var tempBrand = new Brand
                                            {
                                                Enabled = true,
                                                Name = productInStrings[ProductFields.Fields.Producer],
                                                Description = productInStrings[ProductFields.Fields.Producer],
                                                UrlPath = UrlService.GetEvalibleValidUrl(0, ParamType.Brand, productInStrings[ProductFields.Fields.Producer]),
                                                Meta = null
                                            };

                        product.BrandId = BrandService.AddBrand(tempBrand);
                    }
                    else
                    {
                        product.BrandId = BrandService.GetBrandIdByName(productInStrings[ProductFields.Fields.Producer]);
                    }
                }

                //if (SaasDataService.IsSaasEnabled && ProductService.GetProductCountByOffer(6) >= SaasDataService.CurrentSaasData.ProductsCount)
                //{
                //    ImportStatistic.IsRun = false;
                //}
                //else
                //{
                if (!addingNew)
                {
                    ProductService.UpdateProduct(product, false);
                    if (product.ProductId > 0)
                        OtherFields(productInStrings, product.ProductId);
                //    Log(string.Format(Resource.Admin_Import1C_Updated, product.Name, product.ArtNo));
                    ImportStatistic.TotalUpdateRow++;
                }
                else
                {
                    if (!(SaasDataService.IsSaasEnabled && ProductService.GetProductCountByOffer(6) >= SaasDataService.CurrentSaasData.ProductsCount))
                    {
                        ProductService.AddProduct(product, false);
                        if (product.ProductId > 0)
                            OtherFields(productInStrings, product.ProductId);
                 //       Log(string.Format(Resource.Admin_Import1C_Added, product.Name, product.ArtNo));
                        ImportStatistic.TotalAddRow++;
                    }
                }
                //}
            }
            catch (Exception e)
            {
                ImportStatistic.TotalErrorRow++;
                Log(e.Message);
                Debug.LogError(e);
            }

            productInStrings.Clear();
            ImportStatistic.RowPosition++;
        }

        private static void ParseMarkers(Product product, string source)
        {
            // b,n,r,s
            if (!string.IsNullOrWhiteSpace(source))
            {
                var items = source.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                product.BestSeller = !string.IsNullOrEmpty(items.FirstOrDefault(item => item == "b"));
                product.New = !string.IsNullOrEmpty(items.FirstOrDefault(item => item == "n"));
                product.Recomended = !string.IsNullOrEmpty(items.FirstOrDefault(item => item == "r"));
                product.OnSale = !string.IsNullOrEmpty(items.FirstOrDefault(item => item == "s"));
            }
        }

        private static void OtherFields(Dictionary<ProductFields.Fields, string> fields, int productId)
        {
            //Category
            if (fields.ContainsKey(ProductFields.Fields.Category))
            {
                var parentCategory = fields[ProductFields.Fields.Category];
                CategoryService.SubParseAndCreateCategory(parentCategory, productId);
            }

            //photo
            if (fields.ContainsKey(ProductFields.Fields.Photos))
            {
                string photos = fields[ProductFields.Fields.Photos];
                if (!string.IsNullOrEmpty(photos))
                    ParseProductPhoto(productId, photos);
            }

            //Properties
            if (fields.ContainsKey(ProductFields.Fields.Properties))
            {
                string properties = fields[ProductFields.Fields.Properties];
                if (!string.IsNullOrEmpty(properties))
                    ParseProductProperty(productId, properties);
            }
        }

        private static void Log(string message)
        {
            ImportStatistic.WriteLog(message);
        }

        private static string GetSizeForBdFormat(string str)
        {
            if (string.IsNullOrEmpty(str)) return "0|0|0";

            var listSymb = new List<char> { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ',', '.' };
            string res = string.Empty;
            var list = new List<string>();
            foreach (char t in str)
            {
                if (listSymb.Contains(t))
                {
                    res += t;
                }
                else
                {
                    if (!string.IsNullOrEmpty(res))
                    {
                        list.Add(res.Trim());
                        res = string.Empty;
                    }
                }
            }
            if (!string.IsNullOrEmpty(res))
                list.Add(res.Trim());

            res = list.AggregateString('|');

            return res;
        }

        private static void ParseProductPhoto(int productId, string photos)
        {
            var arrPhotos = photos.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < arrPhotos.Length; i++)
            {
                if (SaasDataService.IsSaasEnabled && PhotoService.GetCountPhotos(productId, PhotoType.Product) >= SaasDataService.CurrentSaasData.PhotosCount)
                {
                    return;
                }

                var photo = arrPhotos[i].Trim();
                // if remote picture we must download it
                if (photo.Contains("http://"))
                {
                    //get name photo
                    var photoname = photo.Split('/').LastOrDefault();
                    if (!string.IsNullOrWhiteSpace(photoname))
                    {
                        //if error in download proccess
                        if (!FileHelpers.DownloadRemoteImageFile(photo, FoldersHelper.GetPathAbsolut(FolderType.ImageTemp, photoname)))
                            continue;
                    }
                    photo = photoname;
                }

                // where temp picture folder
                var fullfilename = FoldersHelper.GetPathAbsolut(FolderType.ImageTemp, photo);
                if (File.Exists(fullfilename))
                {
                    if (!PhotoService.IsProductHaveThisPhotoByName(productId, photo))
                    {
                        ProductService.AddProductPhotoByProductId(productId, fullfilename, string.Empty, i == 0);
                    }
                }
                //File.Delete(TempFolders.GetImageTempAbsoluteFolderPath() + photo);
            }
        }

        private static void ParseProductProperty(int productId, string properties)
        {
            try
            {
                // [type:value][type:value]...
                var items = properties.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < items.Length; i++)
                {
                    var temp = items[i].Trim().Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                    if (temp.Length != 2)
                        continue;
                    var tempType = temp[0].Trim();
                    var tempValue = temp[1].Trim();
                    if (!string.IsNullOrWhiteSpace(tempType) && !string.IsNullOrWhiteSpace(tempValue))
                    {
                        // inside stored procedure not thread save/ do save mode by logic 
                        SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_ParseProductProperty]", CommandType.StoredProcedure,
                                                      new SqlParameter("@nameProperty", tempType),
                                                      new SqlParameter("@propertyValue", tempValue),
                                                      new SqlParameter("@productId", productId));
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }
    }
}