//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Text;
using AdvantShop.Configuration;
using AdvantShop.ExportImport;
using Quartz;
using System.IO;
using AdvantShop.Helpers.CsvHelper;
using AdvantShop.Statistic;
using AdvantShop.Helpers;
using System.Collections.Generic;
using System;
using Resources;
using System.Globalization;
using AdvantShop.FullSearch;
using AdvantShop.Catalog;
using AdvantShop.Core.Caching;
using AdvantShop.FilePath;
using System.Data;
using AdvantShop.Mails;

namespace AdvantShop.Core.Scheduler
{
    public class GenerateNesProductAmountUpdateJob : IJob
    {
        private Dictionary<string, int> FieldMapping = new Dictionary<string, int>();

        private void ExecuteScript(string script)
        {
            string strResult = "True";
            try
            {

                AdvantShop.Core.SQLDataAccess.ExecuteNonQuery(script, CommandType.Text);
            }
            catch (Exception ex)
            {
            }

        }

        public void Execute(IJobExecutionContext context)
        {
            try
            {  
                
                ProcessCsv();//Used for update price and amount for all products
                FieldMapping.Clear();

                string _filePath = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp);
                string _fullPath = string.Format("{0}{1}", _filePath, "OstatkiWseCeny.csv");

                LogInvalidData("Start working on Update (Opt). File " + _fullPath + DateTime.Now.ToString());

                if (!File.Exists(_fullPath))
                    return;
                else
                {
                    GC.Collect();
                    ProcessOptPrices(_fullPath);
                    //Generate file for sending to Autospace
                    string autospace_filePath = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp);
                    string autospace_fullPath = string.Format("{0}{1}{2}", autospace_filePath, "OptPrice_InstrumentOpt", ".csv");
                   //Removed by Evgeni. Now price is generetaed by 1C 
                    //CsvExport.SaveProductsToCsvAutospace(autospace_fullPath, Encodings.EncodingsEnum.Utf8, Separators.SeparatorsEnum.SemicolonSeparated);
                    LogInvalidData("End working on Update (Opt). File " + _fullPath + DateTime.Now.ToString());
                    //Send file
                    SendMail.SendMailNow("zakaz.out@autospace.by;GKovalev@gmail.com", "Autospace Instrument-opt.by", "", false, autospace_fullPath);                   
                }
            }
            catch (Exception ex)
            {
                LogInvalidData("Exception occured at ublic void Execute(IJobExecutionContext context).Message: " + ex.Message + ". DAteTime: " + DateTime.Now.ToString());
            }
        }

        private void ProcessOptPrices(string optPricesFilePath)
        {
            using (var csv = new CsvHelper.CsvReader(new StreamReader(optPricesFilePath, Encoding.UTF8)))// Encodings.GetEncoding())))
            {
                LogInvalidData(string.Format("ProcessOptPrices Start 1: Encoding: {0}, File: {1}, Time: {2}",Encodings.GetEncoding(),optPricesFilePath, DateTime.Now) );

                csv.Configuration.Delimiter = Separators.GetCharSeparator();
                csv.Configuration.HasHeaderRecord = true;
                Product product;

                bool firstRead = true;

          //      LogInvalidData(string.Format("ProcessOptPrices Start 2: Encoding: {0}, File: {1}, Time: {2}", Encodings.GetEncoding(), optPricesFilePath, DateTime.Now));

                while (csv.Read())
                {
                    try
                    {
             //           LogInvalidData(string.Format("ProcessOptPrices Start 3: csv: {0}, File: {1}, Time: {2}", Encodings.GetEncoding(), optPricesFilePath, DateTime.Now));
                        if (firstRead)
                        {
                            for (int i = 0; i < csv.FieldHeaders.Length; i++)
                            {
                                FieldMapping.Add(csv.FieldHeaders[i], i);
                            }
                            firstRead = false;
                        }

             //           LogInvalidData(string.Format("ProcessOptPrices Start 4: csv: {0}, File: {1}, Time: {2}", Encodings.GetEncoding(), optPricesFilePath, DateTime.Now));
                 

                        var productInStrings = new Dictionary<ProductFields.Fields, string>();

                        string artNo = ProductFields.GetStringNameByEnum(ProductFields.Fields.Sku);
                        if (FieldMapping.ContainsKey(artNo))
                        {
                            productInStrings.Add(ProductFields.Fields.Sku, Convert.ToString(csv[FieldMapping[artNo]]));
                        }                     
                       
                        //sku;amount;priceOpt1;priceOpt2;priceAkc1;priceAkc2;priceSpec;priceEur

                        product = ProductService.GetProduct(Convert.ToString(csv[FieldMapping[artNo]]));
                        decimal priceOpt1;
                        decimal priceOpt2;

                        decimal priceAkc1;
                        decimal priceAkc2;

                        decimal priceSpec;
                        decimal priceEur;

                        decimal.TryParse(csv["priceOpt1"].ToString().Replace(" ",""), out  priceOpt1);
                        decimal.TryParse(csv["priceOpt2"].ToString().Replace(" ", ""), out  priceOpt2);
                        decimal.TryParse(csv["priceAkc1"].ToString().Replace(" ", ""), out  priceAkc1);
                        decimal.TryParse(csv["priceAkc2"].ToString().Replace(" ", ""), out  priceAkc2);
                        decimal.TryParse(csv["priceSpec"].ToString().Replace(" ", ""), out  priceSpec);
                        
                        decimal.TryParse(csv["priceEur"].ToString().Replace(" ", ""), out  priceEur);
                        if (priceEur == 0 && csv["priceEur"].ToString().Contains("."))
                        {
                            decimal.TryParse(csv["priceEur"].ToString().Replace(" ", "").Replace(".",","), out  priceEur); 
                        }
                        string briefDescription = csv["BriefDescription"].ToString();
                        product.BriefDescription = briefDescription;

                        product.Offers.Add(new Offer
                        {
                            Amount = product.Offers[0].Amount,
                            OfferListId = 9,
                            Price = priceOpt1,
                            ShippingPrice = 0,
                            SupplyPrice = 0,
                            Unit = product.Offers[0].Unit,
                            Multiplicity = product.Offers[0].Multiplicity
                        });

                        product.Offers.Add(new Offer
                        {
                            Amount = product.Offers[0].Amount,
                            OfferListId = 12,
                            Price = priceOpt2,
                            ShippingPrice = 0,
                            SupplyPrice = 0,
                            Unit = product.Offers[0].Unit,
                            Multiplicity = product.Offers[0].Multiplicity
                        });

                        product.Offers.Add(new Offer
                        {
                            Amount = product.Offers[0].Amount,
                            OfferListId = 13,
                            Price = priceAkc1,
                            ShippingPrice = 0,
                            SupplyPrice = 0,
                            Unit = product.Offers[0].Unit,
                            Multiplicity = product.Offers[0].Multiplicity
                        });

                        product.Offers.Add(new Offer
                        {
                            Amount = product.Offers[0].Amount,
                            OfferListId = 14,
                            Price = priceAkc2,
                            ShippingPrice = 0,
                            SupplyPrice = 0,
                            Unit = product.Offers[0].Unit,
                            Multiplicity = product.Offers[0].Multiplicity
                        });

                        product.Offers.Add(new Offer
                        {
                            Amount = product.Offers[0].Amount,
                            OfferListId = 15,
                            Price = priceSpec,
                            ShippingPrice = 0,
                            SupplyPrice = 0,
                            Unit = product.Offers[0].Unit,
                            Multiplicity = product.Offers[0].Multiplicity
                        });

                        product.Offers.Add(new Offer
                        {
                            Amount = product.Offers[0].Amount,
                            OfferListId = 16,
                            Price = priceEur,
                            ShippingPrice = 0,
                            SupplyPrice = 0,
                            Unit = product.Offers[0].Unit,
                            Multiplicity = product.Offers[0].Multiplicity
                        });

            //            LogInvalidData(string.Format("ProcessOptPrices Start 5: csv: {0}, File: {1}, Time: {2}", product.ArtNo, optPricesFilePath, DateTime.Now));
                 
                        ProductService.UpdateProduct(product, false);

           //             LogInvalidData(string.Format("ProcessOptPrices Start 6: csv: {0}, File: {1}, Time: {2}", product.ArtNo, optPricesFilePath, DateTime.Now));
              
                    }
                    catch (Exception ex)
                    {
                        LogInvalidData("Ex  " + ex.Message + DateTime.Now.ToString() );
                    }
                }
            }
            try
            {
                File.Copy(optPricesFilePath, optPricesFilePath.Replace("OstatkiWseCeny.csv", string.Format("OstatkiWseCeny_done{0}.csv", DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"))));
                FileHelpers.DeleteFile(optPricesFilePath);
            }
            catch (Exception ex)
            {
                LogInvalidData("Ex File.Copy(...  " + ex.Message + DateTime.Now.ToString());
            }
        }

        private static void LogInvalidData(string message)
        {
            ImportStatistic.WriteLog(message);
            ImportStatistic.TotalErrorRow++;
            ImportStatistic.RowPosition++;
        }

        private void ProcessCsv()
        {
            string _filePath = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp);
            string _fullPath = string.Format("{0}{1}", _filePath, "ostatki.csv");

            LogInvalidData("Start working on Update (1C). File " + _fullPath + DateTime.Now.ToString());
                         

            if (!File.Exists(_fullPath))
                return;

            long count = 0;
            using (var csv = new CsvHelper.CsvReader(new StreamReader(_fullPath, Encodings.GetEncoding())))
            {
                csv.Configuration.Delimiter = Separators.GetCharSeparator();
                csv.Configuration.HasHeaderRecord = true;
                while (csv.Read())
                    count++;
            }

            ImportStatistic.TotalRow = count;

            LogInvalidData("ImportStatistic.TotalRow = " + count.ToString() + DateTime.Now.ToString());

            using (var csv = new CsvHelper.CsvReader(new StreamReader(_fullPath, Encodings.GetEncoding())))
            {
                csv.Configuration.Delimiter = Separators.GetCharSeparator();
                csv.Configuration.HasHeaderRecord = true;

          
                bool firstRead = true;
                while (csv.Read())
                {
                   
                    try
                    {
                        if (firstRead)
                        {
                            for (int i = 0; i < csv.FieldHeaders.Length; i++)
                            {
                                FieldMapping.Add(csv.FieldHeaders[i], i);
                            }
                            firstRead = false;
                        }
                    

                        var productInStrings = new Dictionary<ProductFields.Fields, string>();

                        string nameField = ProductFields.GetStringNameByEnum(ProductFields.Fields.Sku);
                        if (FieldMapping.ContainsKey(nameField))
                        {
                            productInStrings.Add(ProductFields.Fields.Sku, Convert.ToString(csv[FieldMapping[nameField]]));
                        }

                        nameField = ProductFields.GetStringNameByEnum(ProductFields.Fields.Name).Trim('*');
                        if (FieldMapping.ContainsKey(nameField))
                        {
                            var name = Convert.ToString(csv[FieldMapping[nameField]]);
                            if (!string.IsNullOrEmpty(name))
                            {
                                productInStrings.Add(ProductFields.Fields.Name, name);
                            }
                            else
                            {
                                LogInvalidData(string.Format(Resource.Admin_ImportCsv_CanNotEmpty, ProductFields.GetDisplayNameByEnum(ProductFields.Fields.Name), ImportStatistic.RowPosition + 2));
                                continue;
                            }
                        }

                        nameField = ProductFields.GetStringNameByEnum(ProductFields.Fields.Enabled).Trim('*');
                        if (FieldMapping.ContainsKey(nameField))
                        {
                            string enabled = Convert.ToString(csv[FieldMapping[nameField]]);
                            productInStrings.Add(ProductFields.Fields.Enabled, enabled);
                            //product.Enabled = !string.IsNullOrEmpty(enabled) && enabled.Trim().Equals("+");
                        }

                        nameField = ProductFields.GetStringNameByEnum(ProductFields.Fields.Discount);
                        if (FieldMapping.ContainsKey(nameField))
                        {
                            var discount = Convert.ToString(csv[FieldMapping[nameField]]);
                            if (string.IsNullOrEmpty(discount))
                                discount = "0";
                            decimal tmp;
                            if (decimal.TryParse(discount, out  tmp))
                            {
                                productInStrings.Add(ProductFields.Fields.Discount, tmp.ToString());
                            }
                            else if (decimal.TryParse(discount, NumberStyles.Any, CultureInfo.InvariantCulture, out tmp))
                            {
                                productInStrings.Add(ProductFields.Fields.Discount, tmp.ToString());
                            }
                            else
                            {
                                LogInvalidData(string.Format(Resource.Admin_ImportCsv_MustBeNumber, ProductFields.GetDisplayNameByEnum(ProductFields.Fields.Discount), ImportStatistic.RowPosition + 2));
                                continue;
                            }
                        }

                        nameField = ProductFields.GetStringNameByEnum(ProductFields.Fields.Weight);
                        if (FieldMapping.ContainsKey(nameField))
                        {
                            var weight = Convert.ToString(csv[FieldMapping[nameField]]);
                            if (string.IsNullOrEmpty(weight))
                                weight = "0";
                            decimal tmp;
                            if (decimal.TryParse(weight, out  tmp))
                            {
                                productInStrings.Add(ProductFields.Fields.Weight, tmp.ToString());
                            }
                            else if (decimal.TryParse(weight, NumberStyles.Any, CultureInfo.InvariantCulture, out tmp))
                            {
                                productInStrings.Add(ProductFields.Fields.Weight, tmp.ToString());
                            }
                            else
                            {
                                LogInvalidData(string.Format(Resource.Admin_ImportCsv_MustBeNumber, ProductFields.GetDisplayNameByEnum(ProductFields.Fields.Weight), ImportStatistic.RowPosition + 2));
                                continue;
                            }
                        }

                        nameField = ProductFields.GetStringNameByEnum(ProductFields.Fields.Size);
                        if (FieldMapping.ContainsKey(nameField))
                        {
                            productInStrings.Add(ProductFields.Fields.Size, Convert.ToString(csv[FieldMapping[nameField]]));
                        }

                        nameField = ProductFields.GetStringNameByEnum(ProductFields.Fields.BriefDescription);
                        if (FieldMapping.ContainsKey(nameField))
                        {
                            productInStrings.Add(ProductFields.Fields.BriefDescription, Convert.ToString(csv[FieldMapping[nameField]]));
                        }

                        nameField = ProductFields.GetStringNameByEnum(ProductFields.Fields.Description);
                        if (FieldMapping.ContainsKey(nameField))
                        {
                            productInStrings.Add(ProductFields.Fields.Description, Convert.ToString(csv[FieldMapping[nameField]]));
                        }

                        nameField = ProductFields.GetStringNameByEnum(ProductFields.Fields.Price).Trim('*');
                        if (FieldMapping.ContainsKey(nameField))
                        {
                            var price = Convert.ToString(csv[FieldMapping[nameField]]);
                            if (string.IsNullOrEmpty(price))
                                price = "0";
                            decimal tmp;
                            if (decimal.TryParse(price, out  tmp))
                            {
                                productInStrings.Add(ProductFields.Fields.Price, tmp.ToString());
                            }
                            else if (decimal.TryParse(price, NumberStyles.Any, CultureInfo.InvariantCulture, out tmp))
                            {
                                productInStrings.Add(ProductFields.Fields.Price, tmp.ToString());
                            }
                            else
                            {
                                LogInvalidData(string.Format(Resource.Admin_ImportCsv_MustBeNumber, ProductFields.GetDisplayNameByEnum(ProductFields.Fields.Price), ImportStatistic.RowPosition + 2));
                                continue;
                            }
                        }

                        nameField = ProductFields.GetStringNameByEnum(ProductFields.Fields.PurchasePrice);
                        if (FieldMapping.ContainsKey(nameField))
                        {
                            var sypplyprice = Convert.ToString(csv[FieldMapping[nameField]]);
                            if (string.IsNullOrEmpty(sypplyprice))
                                sypplyprice = "0";
                            decimal tmp;
                            if (decimal.TryParse(sypplyprice, out  tmp))
                            {
                                productInStrings.Add(ProductFields.Fields.PurchasePrice, tmp.ToString());
                            }
                            else if (decimal.TryParse(sypplyprice, NumberStyles.Any, CultureInfo.InvariantCulture, out tmp))
                            {
                                productInStrings.Add(ProductFields.Fields.PurchasePrice, tmp.ToString());
                            }
                            else
                            {
                                LogInvalidData(string.Format(Resource.Admin_ImportCsv_MustBeNumber, ProductFields.GetDisplayNameByEnum(ProductFields.Fields.PurchasePrice), ImportStatistic.RowPosition + 2));
                                continue;
                            }
                        }

                        nameField = ProductFields.GetStringNameByEnum(ProductFields.Fields.ShippingPrice);
                        if (FieldMapping.ContainsKey(nameField))
                        {
                            var shippingPrice = Convert.ToString(csv[FieldMapping[nameField]]);
                            if (string.IsNullOrEmpty(shippingPrice))
                                shippingPrice = "0";
                            decimal tmp;
                            if (decimal.TryParse(shippingPrice, out  tmp))
                            {
                                productInStrings.Add(ProductFields.Fields.ShippingPrice, tmp.ToString());
                            }
                            else if (decimal.TryParse(shippingPrice, NumberStyles.Any, CultureInfo.InvariantCulture, out tmp))
                            {
                                productInStrings.Add(ProductFields.Fields.ShippingPrice, tmp.ToString());
                            }
                            else
                            {
                                LogInvalidData(string.Format(Resource.Admin_ImportCsv_MustBeNumber, ProductFields.GetDisplayNameByEnum(ProductFields.Fields.ShippingPrice), ImportStatistic.RowPosition + 2));
                                continue;
                            }
                        }

                        nameField = ProductFields.GetStringNameByEnum(ProductFields.Fields.Amount);
                        if (FieldMapping.ContainsKey(nameField))
                        {
                            var amount = Convert.ToString(csv[FieldMapping[nameField]]);
                            if (string.IsNullOrEmpty(amount))
                                amount = "0";
                            int tmp;
                            if (int.TryParse(amount, out  tmp))
                            {
                                productInStrings.Add(ProductFields.Fields.Amount, amount);
                            }
                            else
                            {
                                LogInvalidData(string.Format(Resource.Admin_ImportCsv_MustBeNumber, ProductFields.GetDisplayNameByEnum(ProductFields.Fields.Amount), ImportStatistic.RowPosition + 2));
                                continue;
                            }
                        }

                        nameField = ProductFields.GetStringNameByEnum(ProductFields.Fields.Unit);
                        if (FieldMapping.ContainsKey(nameField))
                        {
                            productInStrings.Add(ProductFields.Fields.Unit, Convert.ToString(csv[FieldMapping[nameField]]));
                        }

                        nameField = ProductFields.GetStringNameByEnum(ProductFields.Fields.ParamSynonym);
                        if (FieldMapping.ContainsKey(nameField))
                        {
                            string rewurl = Convert.ToString(csv[FieldMapping[nameField]]);
                            productInStrings.Add(ProductFields.Fields.ParamSynonym, rewurl);
                        }

                        nameField = ProductFields.GetStringNameByEnum(ProductFields.Fields.Title);
                        if (FieldMapping.ContainsKey(nameField))
                        {
                            productInStrings.Add(ProductFields.Fields.Title, Convert.ToString(csv[FieldMapping[nameField]]));
                        }

                        nameField = ProductFields.GetStringNameByEnum(ProductFields.Fields.MetaKeywords);
                        if (FieldMapping.ContainsKey(nameField))
                        {
                            productInStrings.Add(ProductFields.Fields.MetaKeywords, Convert.ToString(csv[FieldMapping[nameField]]));
                        }

                        nameField = ProductFields.GetStringNameByEnum(ProductFields.Fields.MetaDescription);
                        if (FieldMapping.ContainsKey(nameField))
                        {
                            productInStrings.Add(ProductFields.Fields.MetaDescription, Convert.ToString(csv[FieldMapping[nameField]]));
                        }

                        nameField = ProductFields.GetStringNameByEnum(ProductFields.Fields.Photos);
                        if (FieldMapping.ContainsKey(nameField))
                        {
                            productInStrings.Add(ProductFields.Fields.Photos, Convert.ToString(csv[FieldMapping[nameField]]));
                        }

                        nameField = ProductFields.GetStringNameByEnum(ProductFields.Fields.Markers);
                        if (FieldMapping.ContainsKey(nameField))
                        {
                            productInStrings.Add(ProductFields.Fields.Markers, Convert.ToString(csv[FieldMapping[nameField]]));
                        }

                        nameField = ProductFields.GetStringNameByEnum(ProductFields.Fields.Properties);
                        if (FieldMapping.ContainsKey(nameField))
                        {
                            productInStrings.Add(ProductFields.Fields.Properties, Convert.ToString(csv[FieldMapping[nameField]]));
                        }

                        nameField = ProductFields.GetStringNameByEnum(ProductFields.Fields.Producer);
                        if (FieldMapping.ContainsKey(nameField))
                        {
                            productInStrings.Add(ProductFields.Fields.Producer, Convert.ToString(csv[FieldMapping[nameField]]));
                        }

                        nameField = ProductFields.GetStringNameByEnum(ProductFields.Fields.OrderByRequest);
                        if (FieldMapping.ContainsKey(nameField))
                        {
                            string orderbyrequest = Convert.ToString(csv[FieldMapping[nameField]]);
                            productInStrings.Add(ProductFields.Fields.OrderByRequest, orderbyrequest);
                        }

                        nameField = ProductFields.GetStringNameByEnum(ProductFields.Fields.Category).Trim('*');
                        if (FieldMapping.ContainsKey(nameField))
                        {
                            var parentCategory = Convert.ToString(csv[FieldMapping[nameField]]);
                            if (!string.IsNullOrEmpty(parentCategory))
                            {
                                productInStrings.Add(ProductFields.Fields.Category, parentCategory);
                            }
                        }



                        ImportProduct.UpdateInsertProduct(productInStrings,false); 

                    }
                    catch (Exception ex)
                    {
                        LogInvalidData("Ex  " + ex.Message + DateTime.Now.ToString());
                    }
                }


                //All amounts to zero. except that was updated 0.02 = 30 minutes before and SubBrandID != -2 (tools.by)
                string script = "UPDATE Catalog.Offer SET Amount = 0 FROM Catalog.Product INNER JOIN Catalog.Offer ON Catalog.Product.ProductId = Catalog.Offer.ProductID WHERE (Catalog.Product.DateModified < GETDATE() - 0.02) AND Amount < 9000 ";
                ExecuteScript(script);

                LogInvalidData("Amounts of products that was not updated to zero" + DateTime.Now.ToString());

                LogInvalidData(" CategoryService.RecalculateProductsCountManual() before "  + DateTime.Now.ToString());
                CategoryService.RecalculateProductsCountManual();
                LogInvalidData(" CategoryService.RecalculateProductsCountManual() after " + DateTime.Now.ToString());
            }

            try
            {
                LogInvalidData("  LuceneSearch.CreateAllIndexInBackground(); " + DateTime.Now.ToString());
                LuceneSearch.CreateAllIndexInBackground();
                CacheManager.Clean();
                LogInvalidData("   File.Cop file:" + _fullPath + DateTime.Now.ToString());
              //  File.Copy(_fullPath, _fullPath.Replace("ostatki.csv", "ostatki_done_" + DateTime.Now.ToString("M-d-yyyy-HH-mm") + ".csv"));
			  
				FileHelpers.DeleteFile(_fullPath.Replace("ostatki.csv", "Instrument_opt_by__ostatki.csv"));
                File.Copy(_fullPath, _fullPath.Replace("ostatki.csv", "Instrument_opt_by__ostatki.csv"));
                FileHelpers.DeleteFile(_fullPath);
                //UPdate Sorting
                LogInvalidData("   //UPdate Sorting " + DateTime.Now.ToString());
                AdvantShop.Core.SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_UpdateSortingInCategories]", CommandType.StoredProcedure);
                LogInvalidData("   All Done:  " + DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                LogInvalidData("Ex 2 " + ex.Message + DateTime.Now.ToString());
            }
            
        }
    }
}