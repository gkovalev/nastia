//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Statistic;

namespace AdvantShop.ExportImport
{
    public class ExportFeedModuleGoogleBase : ExportFeedModule
    {
        private string _currency;
        private string _description;

        protected override string ModuleName
        {
            get { return "GoogleBase"; }
        }

        public override void GetExportFeedString(string filenameAndPath)
        {
            _description = ExportFeed.GetModuleSetting(ModuleName, "DescriptionSelection");
            _currency = ExportFeed.GetModuleSetting(ModuleName, "DescriptionSelection");

            var settings = new XmlWriterSettings { Encoding = Encoding.UTF8, Indent = true };

            using (var s = new FileStream(filenameAndPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
            {
                using (XmlWriter writer = XmlWriter.Create(s, settings))
                {

                    writer.WriteStartDocument();

                    writer.WriteStartElement("rss", "http://base.google.com/ns/1.0");
                    writer.WriteAttributeString("version", "2.0");

                    writer.WriteStartElement("channel");

                    writer.WriteStartElement("title");
                    writer.WriteString(ExportFeed.GetModuleSetting(ModuleName, "DatafeedTitle"));
                    writer.WriteEndElement();

                    writer.WriteStartElement("link");
                    writer.WriteString(GetShopUrl());
                    writer.WriteEndElement();

                    writer.WriteStartElement("description");
                    writer.WriteString(ExportFeed.GetModuleSetting(ModuleName, "DatafeedDescription"));
                    writer.WriteEndElement();

                    ExportFeedStatistic.TotalCategories = 0;
                    ExportFeedStatistic.TotalProducts = GetProdutsCount(ModuleName);

                    foreach (var productRow in GetProduts(ModuleName))
                    {
                        ProcessProductRow(productRow, writer);
                        ExportFeedStatistic.CurrentProduct++;
                    }

                    writer.WriteEndElement();

                    writer.WriteEndElement();

                    writer.WriteEndDocument();

                    writer.Flush();

                    SetShopUrlToNull();
                }
            }
        }

        private void ProcessProductRow(ExportFeedProduts row, XmlWriter writer)
        {
            writer.WriteStartElement("item");

            writer.WriteStartElement("title");
            writer.WriteString(row.Name);
            writer.WriteEndElement();

            writer.WriteStartElement("description");

            string desc = _description == "full" ? row.Description : row.BriefDescription;
            writer.WriteString(desc);

            writer.WriteEndElement();

            writer.WriteStartElement("link");
            writer.WriteString(SettingsGeneral.AbsoluteUrl.TrimEnd('/') + "/" + UrlService.GetLink(ParamType.Product, row.UrlPath, row.ProductID));
            writer.WriteEndElement();

            writer.WriteStartElement("g", "id");
            writer.WriteString(row.ProductID.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement("g", "price");
            var nfi = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();
            nfi.NumberDecimalSeparator = ".";
            writer.WriteString(CatalogService.CalculatePrice(row.Price, row.Discount).ToString(nfi));
            writer.WriteEndElement();

            writer.WriteStartElement("g", "image_link");

            var temp = row.Photo;
            if (!string.IsNullOrEmpty(temp))
            {
                writer.WriteString(GetImageProductPath(temp));
            }

            writer.WriteEndElement();

            writer.WriteStartElement("g", "currency");
            writer.WriteString(_currency);
            writer.WriteEndElement();

            writer.WriteStartElement("g", "condition");
            writer.WriteString("new");
            writer.WriteEndElement();

            writer.WriteEndElement();
        }
    }
}