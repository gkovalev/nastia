//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdvantShop.Catalog;
using AdvantShop.Core;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Helpers.CsvHelper;
using AdvantShop.SEO;
using AdvantShop.Statistic;

namespace AdvantShop.ExportImport
{
    public class CsvExport
    {
        public static void SaveProductsToCsv(string path, Encodings.EncodingsEnum encodeType, Separators.SeparatorsEnum delimetr, List<ProductFields.Fields> fieldMapping)
        {
            using (var writer = new CsvHelper.CsvWriter(new StreamWriter(path, false, Encodings.GetEncoding(encodeType))))
            {
                writer.Configuration.Delimiter = Separators.GetCharSeparator(delimetr);
                //var fields = new List<string> { "SKU", "Name*", "ParamSynonym", "Category", "Enabled*", "Price*", "PurchasePrice*", "Amount*", "Unit", "Discount", "ShippingPrice", "Weight", "Size", "BriefDescription", "Description" };
                //foreach (var item in fields)
                //    writer.WriteField(item);
                foreach (var item in fieldMapping)
                    writer.WriteField(ProductFields.GetStringNameByEnum(item));
                writer.NextRecord();
                var items = ProductService.GetProducts();
                if (items == null) return;
                var regex = new Regex("^[0-9]+$");
                for (int j = 0; j < items.Count; j++)
                {
                    var product = items[j];
                    if (!ExportStatistic.IsRun) return;
                    var offer = product.Offers.FirstOrDefault() ?? new Offer { Amount = 0, Price = 0, SupplyPrice = 0, Unit = "", ShippingPrice = 0 };
                    var meta = MetaInfoService.GetMetaInfo(product.ID, MetaType.Product) ??
                        new MetaInfo(0, 0, MetaType.Product, string.Empty, string.Empty, string.Empty);
                    for (int i = 0; i < fieldMapping.Count; i++)
                    {
                        var item = fieldMapping[i];
                        //Changed by Evgeni to insert dotes
                        if (item == ProductFields.Fields.Sku)
                        {
                           
                            string art = product.ArtNo;
                            //if (art.Contains("-") == false && art.Length > 7)
                           // {
                           //     art = art.Insert(7, ".").Insert(4, ".").Insert(1, ".");
                           // }
                            //
                            writer.WriteField(art);
                            // writer.WriteField(product.ArtNo);
                        }
                        //
                        if (item == ProductFields.Fields.Name)
                            writer.WriteField(product.Name);
                        if (item == ProductFields.Fields.ParamSynonym)
                        {
                            if (regex.IsMatch(product.UrlPath))
                            {
                                writer.WriteField("IPD" + product.UrlPath);
                            }
                            else
                            {
                                writer.WriteField(product.UrlPath);
                            }
                        }
                        if (item == ProductFields.Fields.Category)
                            writer.WriteField((GetCategoryStringByProductID(product.ProductId)));
                        if (item == ProductFields.Fields.Enabled)
                            writer.WriteField(product.Enabled ? "+" : "-");
                        if (item == ProductFields.Fields.Price)
                            writer.WriteField(offer.Price.ToString("F2"));
                        if (item == ProductFields.Fields.PurchasePrice)
                            writer.WriteField(offer.SupplyPrice.ToString("F2"));
                        if (item == ProductFields.Fields.Amount)
                            writer.WriteField(offer.Amount.ToString());
                        if (item == ProductFields.Fields.Unit)
                            writer.WriteField(offer.Unit);
                        if (item == ProductFields.Fields.ShippingPrice)
                            writer.WriteField(offer.ShippingPrice.ToString("F2"));
                        if (item == ProductFields.Fields.Discount)
                            writer.WriteField(product.Discount.ToString("F2"));
                        if (item == ProductFields.Fields.Weight)
                            writer.WriteField(product.Weight.ToString("F2"));
                        if (item == ProductFields.Fields.Size)
                            writer.WriteField(product.Size.Replace("|", " x "));
                        if (item == ProductFields.Fields.BriefDescription)
                            writer.WriteField(product.BriefDescription);
                        if (item == ProductFields.Fields.Description)
                            writer.WriteField(product.Description);

                        if (item == ProductFields.Fields.Title)
                            writer.WriteField(meta.Title);
                        if (item == ProductFields.Fields.MetaKeywords)
                            writer.WriteField(meta.MetaKeywords);
                        if (item == ProductFields.Fields.MetaDescription)
                            writer.WriteField(meta.MetaDescription);
                        if (item == ProductFields.Fields.Markers)
                            writer.WriteField(MarkersToString(product));
                        if (item == ProductFields.Fields.Photos)
                            writer.WriteField(PhotoToString(product.ProductPhotos));
                        if (item == ProductFields.Fields.Properties)
                            writer.WriteField(PropertiesToString(product.ProductPropertyValues));
                        //Changed by Evgeni
                        if (item == ProductFields.Fields.EAN)
                            writer.WriteField(product.EAN);
                        if (item == ProductFields.Fields.SubBrandId)
                            writer.WriteField(product.SubBrandId);

                        //
                        if (item == ProductFields.Fields.Producer)
                        {
                            var brand = BrandService.GetBrandById(product.BrandId);
                            writer.WriteField(brand != null ? brand.Name : string.Empty);
                        }
                        if (item == ProductFields.Fields.OrderByRequest)
                            writer.WriteField(product.OrderByRequest ? "+" : "-");
                    }
                    writer.NextRecord();
                    ExportStatistic.RowPosition++;
                }
            }
        }

        public static string PropertiesToString(List<PropertyValue> productPropertyValues)
        {
            var res = new StringBuilder();
            for (int i = 0; i < productPropertyValues.Count; i++)
            {
                res.Append(productPropertyValues[i].Property.Name + ":" + productPropertyValues[i].Value + ";");
            }
            return res.ToString();
        }

        public static string PhotoToString(List<Photo> productPhotos)
        {
            var res = new StringBuilder();
            for (int i = 0; i < productPhotos.Count; i++)
            {
                if (productPhotos[i].PhotoName.Contains("."))
                    res.Append(productPhotos[i].PhotoName + ",");
                else
                    res.Append(Path.GetFileNameWithoutExtension(productPhotos[i].PhotoName) 
                             + FoldersHelper.ProductPhotoPostfix[ProductImageType.Big]
                             + Path.GetExtension(productPhotos[i].PhotoName) + ",");
            }
            return res.ToString().Trim(',');
        }

        public static string MarkersToString(Product product)
        {
            // b,n,r,s
            string res = string.Empty;
            res += product.BestSeller ? "b," : string.Empty;
            res += product.New ? "n," : string.Empty;
            res += product.Recomended ? "r," : string.Empty;
            res += product.OnSale ? "s," : string.Empty;
            if (res.Length > 0)
                res = res.Remove(res.Length - 1, 1);
            return res;
        }

        public static string GetCategoryStringByProductID(int productID)
        {
            var strResSb = new StringBuilder();

            try
            {
                using (var db = new SQLDataAccess())
                {
                    db.cmd.CommandText = "Select CategoryID from Catalog.ProductCategories where ProductID=@id";
                    // !!! Ïåðåâåñòè íà sp
                    db.cmd.CommandType = CommandType.Text;

                    db.cmd.Parameters.Clear();
                    db.cmd.Parameters.AddWithValue("@id", productID);

                    db.cnOpen();

                    var read = db.cmd.ExecuteReader();

                    using (var dbCat = new SQLDataAccess())
                    {
                        dbCat.cnOpen();
                        while (read.Read())
                        {
                            strResSb.AppendFormat(strResSb.Length == 0 ? "[{0}]" : ";[{0}]", GetParentCategoriesAsString(SQLDataHelper.GetInt(read, "CategoryID"), dbCat));
                        }
                        read.Close();
                        dbCat.cnClose();
                    }

                    db.cnClose();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }

            return strResSb.ToString();
        }

        public static string GetParentCategoriesAsString(int childCategoryId, SQLDataAccess dbAccess)
        {
            var res = new StringBuilder();
            var categoies = CategoryService.GetParentCategories(childCategoryId, dbAccess);
            for (var i = categoies.Count - 1; i >= 0; i--)
            {
                if (i != categoies.Count - 1)
                {
                    res.Append(" >> ");
                }
                res.Append(categoies[i].Name);
            }
            return res.ToString();
        }


        //Added by Evgeni
        public static void SaveProductsToCsvAutospace(string path, Encodings.EncodingsEnum encodeType, Separators.SeparatorsEnum delimetr)
        {
            ImportStatistic.WriteLog("Start working on SaveProductsToCsvAutospace. DateTime: " + DateTime.Now);

            using (var writer = new CsvHelper.CsvWriter(new StreamWriter(path, false, Encodings.GetEncoding(encodeType))))
            {
                ImportStatistic.WriteLog("Start working on SaveProductsToCsvAutospace 2.Encoding: " +Encodings.GetEncoding(encodeType) + " DateTime: " + DateTime.Now);

                writer.Configuration.Delimiter = Separators.GetCharSeparator(delimetr);

                //var items = new List<Product>();

                //using (var db = new SQLDataAccess())
                //{
                //    db.cnOpen();
                //    items = (List<Product>)ProductService.GetProductsWhere(db, "ArtNo like '2.607.001.733'");
                //    db.cnClose();
                //}
                var items = ProductService.GetProducts();// .Where(t => t.ArtNo.Contains("2.607.001.733")).ToList();
             
                List<string> loadedProducts = new List<string>();

                ImportStatistic.WriteLog("Start working on SaveProductsToCsvAutospace 3. DateTime: " + DateTime.Now + "Number of products to proceed: " + items.Count);
                //select only relevants offers
              //  items = items.Where(t => t.Brand.Name.ToLower() == "bosch" || t.Brand.Name.ToLower() == "skil").Where(t => t.Offers.FirstOrDefault().Amount > 0 && t.Offers.FirstOrDefault().Price > 0).ToList();

                if (items == null) return;
                var regex = new Regex("^[0-9]+$");
                string pcs = "шт.";
                for (int j = 0; j < items.Count; j++)
                {
                    try
                    {
                        if (j % 1000 == 0)
                        {
                            ImportStatistic.WriteLog("Start working on SaveProductsToCsvAutospace 4. DateTime: " + DateTime.Now
                                + " File No " + j); 
                        }

                        //check that product was not inserted yet
                        if (loadedProducts.Where(t => t.Contains(items[j].ArtNo)).ToList().Count > 0)
                        {
                            continue;
                        }
                        else
                        {
                            loadedProducts.Add(items[j].ArtNo);
                        }

                        var product = items[j];
                        var offer = product.Offers.Where(t => t.OfferListId == 15).FirstOrDefault() ?? new Offer { Amount = 0, Price = 0, SupplyPrice = 0, Unit = "", ShippingPrice = 0 };//get only spec price
                        if (offer.OfferListId != 15 || offer.Amount <= 0 || offer.Price <= 0)
                            continue;

                        if (product.Brand == null)
                        {
                            if (product.ArtNo.ToLower().StartsWith("f"))
                            {
                                writer.WriteField("Skil");
                            }
                            else
                            {
                                writer.WriteField("Bosch");
                            }
                        }
                        else
                        {
                            writer.WriteField(product.Brand.Name);
                        }

                        writer.WriteField(product.ArtNo);
                        writer.WriteField(product.BriefDescription.Replace(";", "").Replace("&nbsp;", " ").Replace(",", "").Replace("<p>", "").Replace("<p/>", ""));

                        if (product.ProductCategories.Count == 0)
                        {
                            writer.WriteField("Прочее");
                        }
                        else
                        {
                            writer.WriteField(product.ProductCategories.FirstOrDefault().Name.Replace(";", "").Replace("&nbsp;", "").Replace(",", ""));
                        }

                        writer.WriteField(offer.Amount);
                        writer.WriteField(offer.Price.ToString("F2"));
                        writer.WriteField(pcs);
                        writer.WriteField("1");

                        writer.NextRecord();
                    }
                    catch (Exception ex)
                    {
                        ImportStatistic.WriteLog("Error while adding product:" + ex.Message);
                    }
                }

                ImportStatistic.WriteLog("End working on SaveProductsToCsvAutospace 5. DateTime: " + DateTime.Now);
         
            }
            ImportStatistic.WriteLog("End working on SaveProductsToCsvAutospace 6. DateTime: " + DateTime.Now);
        }

    }
}