//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using AdvantShop.Statistic;
using System.Text.RegularExpressions;

namespace AdvantShop.ExportImport
{
    public class ExportFeedModuleYandex : ExportFeedModule
    {
        private string _currency;
        private string _description;
        private string _salesNotes;
        protected override string ModuleName
        {
            get { return "YandexMarket"; }
        }

        public override void GetExportFeedString(string  filenameAndPath)
        {
            try
            {
                _currency = ExportFeed.GetModuleSetting(ModuleName, "Currency");
                _description = ExportFeed.GetModuleSetting(ModuleName, "DescriptionSelection");
                _salesNotes = ExportFeed.GetModuleSetting(ModuleName, "SalesNotes");

                var shopName = ExportFeed.GetModuleSetting(ModuleName, "ShopName").Replace("#STORE_NAME#", SettingsMain.ShopName);
                var companyName = ExportFeed.GetModuleSetting(ModuleName, "CompanyName").Replace("#STORE_NAME#", SettingsMain.ShopName);
                FileHelpers.DeleteFile(filenameAndPath);
                using (var outputFile = new FileStream(filenameAndPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
                {
                    var settings = new XmlWriterSettings { Encoding = Encoding.UTF8, Indent = true };
                    using (var writer = XmlWriter.Create(outputFile, settings))
                    {
                        writer.WriteStartDocument();
                        writer.WriteDocType("yml_catalog", null, "shops.dtd", null);
                        writer.WriteStartElement("yml_catalog");
                        writer.WriteAttributeString("date", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                        writer.WriteStartElement("shop");

                        writer.WriteStartElement("name");
                        writer.WriteString(shopName);
                        writer.WriteEndElement();

                        writer.WriteStartElement("company");
                        writer.WriteString(companyName);
                        writer.WriteEndElement();

                        writer.WriteStartElement("url");
                        writer.WriteString(_shopUrl);
                        writer.WriteEndElement();

                        writer.WriteStartElement("currencies");
                        foreach (DataRow curRow in GetCurrencies().Rows)
                        {
                            ProcessCurrencyRow(curRow, writer);
                        }
                        writer.WriteEndElement();

                        ExportFeedStatistic.TotalCategories = GetCategoriesCount(ModuleName);
                        ExportFeedStatistic.TotalProducts = GetProdutsCount(ModuleName);

                        writer.WriteStartElement("categories");
                        foreach (var categoryRow in GetCategories(ModuleName))
                        {
                            ProcessCategoryRow(categoryRow, writer);
                            ExportFeedStatistic.CurrentCategory++;
                        }
                        writer.WriteEndElement();

                        writer.WriteStartElement("offers");

                        foreach (var offerRow in GetProduts(ModuleName))
                        {
                            ProcessProductRow(offerRow, writer);
                            ExportFeedStatistic.CurrentProduct++;
                        }
                        writer.WriteEndElement();

                        writer.WriteEndElement();
                        writer.WriteEndElement();
                        writer.WriteEndDocument();

                        writer.Flush();
                        writer.Close();
                        SetShopUrlToNull();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }

        private static void ProcessCurrencyRow(DataRow row, XmlWriter writer)
        {
            writer.WriteStartElement("currency");
            var temp = SQLDataHelper.GetString(row["CurrencyIso3"]);
            writer.WriteAttributeString("id", temp == "RUB" ? "RUR" : temp);
            var nfi = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();
            nfi.NumberDecimalSeparator = ".";
            writer.WriteAttributeString("rate", SQLDataHelper.GetDecimal(row["CurrencyValue"]).ToString(nfi));
            writer.WriteEndElement();
        }

        private static void ProcessCategoryRow(ExportFeedCategories row, XmlWriter writer)
        {
            writer.WriteStartElement("category");
            writer.WriteAttributeString("id", row.Id.ToString());
            if (row.ParentCategory != 0)
            {
                writer.WriteAttributeString("parentId", row.ParentCategory.ToString());
            }
            writer.WriteString(row.Name);
            writer.WriteEndElement();
        }

        private void ProcessProductRow(ExportFeedProduts row, XmlWriter writer)
        {
            //var tempUrl = (_shopUrl.EndsWith("/") ? _shopUrl.TrimEnd('/') : _shopUrl);
            writer.WriteStartElement("offer");
            writer.WriteAttributeString("id", row.ProductID.ToString());
            writer.WriteAttributeString("available", (row.Amount > 0).ToString().ToLower());

            writer.WriteStartElement("url");
            writer.WriteString(SettingsGeneral.AbsoluteUrl.TrimEnd('/') + "/" + UrlService.GetLink(ParamType.Product, row.UrlPath, row.ProductID));
            writer.WriteEndElement();


            writer.WriteStartElement("price");
            var nfi = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();
            nfi.NumberDecimalSeparator = ".";
            writer.WriteString(CatalogService.CalculatePrice(row.Price, row.Discount).ToString(nfi));
            writer.WriteEndElement();

            writer.WriteStartElement("currencyId");
            writer.WriteString(_currency == "RUB" ? "RUR" : _currency);
            writer.WriteEndElement();

            writer.WriteStartElement("categoryId");
            writer.WriteString(row.ParentCategory.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement("picture");
            var photo = SQLDataHelper.GetString(row.Photo);
            if (!string.IsNullOrEmpty(photo))
            {
                writer.WriteString(GetImageProductPath(photo));
            }
            writer.WriteEndElement();

            writer.WriteStartElement("name");

            //Added by Evgeni to Change product Name like in Description
           //from details.aspx

            if (!row.Name.ToLower().Contains(row.BrandName.ToLower()))
            {
                if (row.BrandName.ToLower() == "bosch" && Regex.IsMatch(row.Name, @"[A-Z]"))
                {
                    row.Name = row.Name.Insert(Regex.Match(row.Name, @"[A-Z]").Index, row.BrandName + ' ');
                }
                else
                {
                    row.Name = row.BrandName + " " + row.Name;
                }
            }
            if (!row.Name.ToLower().Contains(row.ArtNo.Replace(".", "").Replace("-", "").ToLower()))
            {
                row.Name = row.Name + " [" + row.ArtNo.Replace(".", "").Replace("-", "") + "]";
            }

            ///

            writer.WriteString(row.Name);
            writer.WriteEndElement();

            writer.WriteStartElement("description");
            string desc = SQLDataHelper.GetString(_description == "full" ? row.Description : row.BriefDescription);

            writer.WriteString(desc);

            writer.WriteEndElement();

            if (_salesNotes.IsNotEmpty())
            {
                writer.WriteStartElement("sales_notes");
                writer.WriteString(_salesNotes);
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }
    }
}