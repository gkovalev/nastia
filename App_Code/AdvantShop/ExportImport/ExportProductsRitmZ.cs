//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Globalization;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using AdvantShop.Catalog;
using AdvantShop.Core;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Configuration;
using Resources;

namespace AdvantShop.ExportImport
{
    public class ExportProductsRitmZ
    {
        public const string ExportFile = "exportRitmZProducts.xml";
        public const string ExporDir = "~/Export";
        private static string _currency = "RUR";
        private static string _salesNotes = string.Empty;
        private static string _description = "full";

        public static void Export()
        {
            if (!Directory.Exists(HttpContext.Current.Server.MapPath(ExporDir)))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(ExporDir));
            }
            using (var fs = new FileStream(HttpContext.Current.Server.MapPath(ExporDir + "/" + ExportFile), FileMode.Create, FileAccess.ReadWrite))
            {
                GetExportFeedString(fs);
                fs.Close();
            }
        }

        public static void WriteToResponce(HttpResponse httpResponse, string exportWay)
        {
            using (var fs = new FileStream(exportWay, FileMode.Open))
            using (var reader = new StreamReader(fs, Encoding.UTF8))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    httpResponse.Write(line);
                }
            }
        }

        public static long GetExportFeedString(Stream s)
        {
            long filesize = 0;
            var settings = new XmlWriterSettings { Encoding = Encoding.UTF8, Indent = true };
            using (var writer = XmlWriter.Create(s, settings))
            {
                writer.WriteStartDocument();
                writer.WriteDocType("yml_catalog", null, "shops.dtd", null);
                writer.WriteStartElement("yml_catalog");
                writer.WriteAttributeString("date", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                writer.WriteStartElement("shop");

                writer.WriteStartElement("name");
                writer.WriteString(SettingsMain.ShopName);
                writer.WriteEndElement();

                writer.WriteStartElement("company");
                writer.WriteString(SettingsMain.ShopName);
                writer.WriteEndElement();

                writer.WriteStartElement("url");
                writer.WriteString(SettingsMain.SiteUrl);
                writer.WriteEndElement();

                writer.WriteStartElement("currencies");
                DataTable currenciesDataTable = GetCurrencies();
                foreach (DataRow curRow in currenciesDataTable.Rows)
                {
                    ProcessCurrencyRow(curRow, writer);
                }
                writer.WriteEndElement();

                writer.WriteStartElement("categories");
                DataTable categoriesDataTable = GetCategories();
                foreach (DataRow catRow in categoriesDataTable.Rows)
                {
                    ProcessCategoryRow(catRow, writer);
                }
                writer.WriteEndElement();

                writer.WriteStartElement("offers");
                DataTable offersDataTable = GetProducts();
                foreach (DataRow offerRow in offersDataTable.Rows)
                {
                    ProcessProductRow(offerRow, writer);
                }

                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndDocument();

                writer.Flush();
                filesize = s.Length;
                writer.Close();
            }
            return filesize;
        }

        private static void ProcessCurrencyRow(DataRow row, XmlWriter writer)
        {
            writer.WriteStartElement("currency");
            writer.WriteAttributeString("id", SQLDataHelper.GetString(row["CurrencyIso3"]));
            var nfi = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();
            nfi.NumberDecimalSeparator = ".";
            writer.WriteAttributeString("rate", SQLDataHelper.GetDecimal(row["CurrencyValue"]).ToString(nfi));
            writer.WriteEndElement();
        }

        private static void ProcessCategoryRow(DataRow row, XmlWriter writer)
        {
            writer.WriteStartElement("category");
            writer.WriteAttributeString("id", row["CategoryID"].ToString().Replace("ID", ""));
            if (row["ParentCategory"].ToString().Trim() != "0")
            {
                writer.WriteAttributeString("parentId", row["ParentCategory"].ToString().Replace("ID", ""));
            }
            writer.WriteString(SQLDataHelper.GetString(row["Name"]));
            writer.WriteEndElement();
        }

        private static void ProcessProductRow(DataRow row, XmlWriter writer)
        {
            writer.WriteStartElement("offer");
            writer.WriteAttributeString("available", SQLDataHelper.GetBoolean(row["Enabled"]).ToString().ToLower());
            writer.WriteAttributeString("id", row["ArtNo"].ToString());

            writer.WriteStartElement("url");
            if (string.IsNullOrEmpty(HttpContext.Current.Request.Url.Query))
            {
                writer.WriteString(HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath, "") + "/" + UrlService.GetLink(ParamType.Product, (string)row["UrlPath"], (int)row["ProductID"]));
            }
            else
            {
                writer.WriteString(HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath, "").Replace(HttpContext.Current.Request.Url.Query, "") + "/" + UrlService.GetLink(ParamType.Product, (string)row["UrlPath"], (int)row["ProductID"]));
            }
            writer.WriteEndElement();


            writer.WriteStartElement("price");
            var nfi = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();
            nfi.NumberDecimalSeparator = ".";
            writer.WriteString(CatalogService.CalculatePrice(SQLDataHelper.GetDecimal(row["Price"]), SQLDataHelper.GetDecimal(row["discount"])).ToString(nfi));
            writer.WriteEndElement();

            writer.WriteStartElement("currencyId");
            writer.WriteString(_currency);
            writer.WriteEndElement();

            writer.WriteStartElement("categoryId");
            writer.WriteString(row["ParentCategory"].ToString().Replace("ID", ""));
            writer.WriteEndElement();

            writer.WriteStartElement("picture");
            var temp = SQLDataHelper.GetString(row["photo"]);
            if (!string.IsNullOrEmpty(temp))
            {
                if (string.IsNullOrEmpty(HttpContext.Current.Request.Url.Query))
                    writer.WriteString(HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath, "") + "/" + FoldersHelper.GetImageProductPath(ProductImageType.Middle, temp, false));
                else
                    writer.WriteString(HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath, "").Replace(HttpContext.Current.Request.Url.Query, "") + "/" + FoldersHelper.GetImageProductPath(ProductImageType.Middle, temp, false));
            }
            writer.WriteEndElement();

            writer.WriteStartElement("name");
            writer.WriteString(SQLDataHelper.GetString(row["Name"]));
            writer.WriteEndElement();

            writer.WriteStartElement("description");
            string desc = SQLDataHelper.GetString(_description == "full" ? row["Description"] : row["BriefDescription"]);

            writer.WriteString(!string.IsNullOrEmpty(desc) ? desc : Resource.ExportFeed_NoDescription);

            writer.WriteEndElement();

            writer.WriteStartElement("sales_notes");
            writer.WriteString(_salesNotes);
            writer.WriteEndElement();

            // расширение от Ritmz
            writer.WriteStartElement("rz_Active");
            writer.WriteString(SQLDataHelper.GetString(row["Enabled"]));
            writer.WriteEndElement();

            writer.WriteStartElement("rz_Quantity");
            writer.WriteString(SQLDataHelper.GetString(row["Amount"]));
            writer.WriteEndElement();

            writer.WriteStartElement("rz_Weight");
            writer.WriteString(SQLDataHelper.GetString(row["Weight"]));
            writer.WriteEndElement();

            writer.WriteStartElement("rz_Length");
            writer.WriteString(string.Empty);
            writer.WriteEndElement();
            writer.WriteStartElement("rz_Width");
            writer.WriteString(string.Empty);
            writer.WriteEndElement();
            writer.WriteStartElement("rz_Height");
            writer.WriteString(string.Empty);
            writer.WriteEndElement();

            writer.WriteStartElement("rz_SupplierName");
            writer.WriteString(string.Empty);
            writer.WriteEndElement();
            writer.WriteStartElement("rz_SupplierCode");
            writer.WriteString(string.Empty);
            writer.WriteEndElement();
            writer.WriteStartElement("rz_SupplierPrice");
            writer.WriteString(SQLDataHelper.GetString(row["SupplyPrice"]));
            writer.WriteEndElement();

            //add fields
            //rz_Active - принимает значение «true», если товар активен и
            //участвует в продажах, «false» - если товар неактивен,
            //даже если количество товара положительное.
            //Значение по умолчанию - «true»;
            //rz_Quantity - cодержит количество товара на складе ИМ в штуках (не обязательное);
            //rz_Weight - cодержит вес товара с упаковкой в килограммах (не обязательное);
            //rz_Length - cодержит длину упаковки в метрах (не обязательное);
            //rz_Width - cодержит ширину упаковки в метрах (не обязательное);
            //rz_Height - содержит высоту упаковки в метрах (не обязательное);
            //rz_SupplierName - наименование поставщика;
            //rz_SupplierCode - код поставщика;
            //rz_SupplierPrice - цена поставщика. Указывается в случае, если
            //требуется получать отчетность по маржинальной
            //прибыли или установить приоритетность выбора
            //поставщиков одного и того же товара по приоритету цены (не обязательное).

            writer.WriteEndElement();
        }

        protected static DataTable GetCategories()
        {
            var result = new DataTable { TableName = "Categories" };
            try
            {
                using (var db = new SQLDataAccess())
                {
                    db.cmd.CommandText =
                        @"SELECT [Category].[CategoryID], [Category].[ParentCategory], [Name] 
                            FROM [Catalog].[Category]      
                            WHERE [Category].CategoryID <> '0'";
                    db.cmd.Parameters.Clear();
                    db.cmd.CommandType = CommandType.Text;
                    var dataAdapter = new SqlDataAdapter(db.cmd);
                    dataAdapter.Fill(result);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
            return result;
        }

        protected static DataTable GetProducts()
        {
            var result = new DataTable { TableName = "Categories" };
            try
            {
                using (var db = new SQLDataAccess())
                {
                    db.cmd.CommandText =
                        @"DECLARE @defaultCurrencyRatio float;
                            SELECT @defaultCurrencyRatio = CurrencyValue FROM [Catalog].[Currency] WHERE CurrencyIso3 = @selectedCurrency;
                            SELECT [Catalog].[Product].[Enabled], [Catalog].[Product].[ArtNo], [Catalog].[Product].[ProductID], [Catalog].[Product].[Discount],[UrlPath], 
                                    ([Catalog].[Offer].[Price] / @defaultCurrencyRatio) AS Price, [Catalog].[Product].[Name], 
                                    [Catalog].[Product].[Description], [Catalog].[Product].[BriefDescription], [Catalog].[Product].[Weight], 
                                    [Catalog].[Offer].[ShippingPrice], [Catalog].[Offer].[SupplyPrice], [Catalog].[Offer].[Amount], PhotoName AS Photo, 
                                    (Select TOP(1) [CategoryID] from [Catalog].[ProductCategories] where [ProductCategories].productId = [Product].[ProductID]) as [ParentCategory]
                                FROM [Catalog].[Product]
                                JOIN  [Catalog].[Offer] ON [Catalog].[Offer].[ProductID] = [Catalog].[Product].[ProductID] 	
                                LEFT JOIN [Catalog].[Photo] ON [Product].[ProductID] = [Photo].[ProductID] and Type=@type AND [Main] = 1	";
                    db.cmd.CommandType = CommandType.Text;
                    db.cmd.Parameters.Clear();
                    db.cmd.Parameters.AddWithValue("@selectedCurrency", "RUB");
                    db.cmd.Parameters.AddWithValue("@type", PhotoType.Product.ToString());
                    var dataAdapter = new SqlDataAdapter(db.cmd);
                    dataAdapter.Fill(result);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }

            foreach (DataRow productRow in result.Rows)
            {
                if (productRow["Description"] is DBNull)
                {
                    productRow["Description"] = " ";
                }
                if (productRow["BriefDescription"] is DBNull)
                {
                    productRow["BriefDescription"] = " ";
                }
                productRow["Description"] = ProcessProductDescription((string)productRow["Description"]);
                productRow["BriefDescription"] = ProcessProductDescription((string)productRow["BriefDescription"]);
            }

            result.AcceptChanges();
            return result;
        }

        protected static string ProcessProductDescription(string desc)
        {
            var sb = new StringBuilder(desc);
            sb.Replace("\n", "");
            sb.Replace('\t', ' ');

            var regEx = new Regex("<[a-zA-Z/]+>");
            var res = regEx.Replace(sb.ToString(), "");

            return res;
        }

        protected static DataTable GetCurrencies()
        {
            var dataTable = new DataTable();
            try
            {
                using (var db = new SQLDataAccess())
                {
                    db.cmd.CommandText = "SELECT CurrencyValue, CurrencyIso3 FROM [Catalog].[Currency];";
                    db.cmd.CommandType = CommandType.Text;
                    var adapter = new SqlDataAdapter(db.cmd);
                    adapter.Fill(dataTable);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
            return dataTable;
        }

    }
}