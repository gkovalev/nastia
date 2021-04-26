//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Globalization;
using System.IO;
using System.Text;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Helpers;
using AdvantShop.Statistic;
using Resources;

namespace AdvantShop.ExportImport
{
    public class ExportFeedModuleShopzilla : ExportFeedModule
    {
        private string _description;

        protected override string ModuleName
        {
            get { return "Shopzilla"; }
        }

        public override void GetExportFeedString(string file)
        {
            _description = ExportFeed.GetModuleSetting(ModuleName, "DescriptionSelection");


            using (var s = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
            {
                using (var memoryBuffer = new StreamWriter(s, Encoding.UTF8))
                {
                    memoryBuffer.Write("Category");
                    memoryBuffer.Write("\t");

                    memoryBuffer.Write("Manufacturer");
                    memoryBuffer.Write("\t");

                    memoryBuffer.Write("Title");
                    memoryBuffer.Write("\t");

                    memoryBuffer.Write("Description");
                    memoryBuffer.Write("\t");

                    memoryBuffer.Write("Link");
                    memoryBuffer.Write("\t");

                    memoryBuffer.Write("Image");
                    memoryBuffer.Write("\t");

                    memoryBuffer.Write("SKU");
                    memoryBuffer.Write("\t");

                    memoryBuffer.Write("Quantity on Hand");
                    memoryBuffer.Write("\t");

                    memoryBuffer.Write("Condition");
                    memoryBuffer.Write("\t");

                    memoryBuffer.Write("Shipping Weight");
                    memoryBuffer.Write("\t");

                    memoryBuffer.Write("Shipping Cost");
                    memoryBuffer.Write("\t");

                    memoryBuffer.Write("Bid");
                    memoryBuffer.Write("\t");

                    memoryBuffer.Write("Promo Text");
                    memoryBuffer.Write("\t");

                    memoryBuffer.Write("UPC");
                    memoryBuffer.Write("\t");

                    memoryBuffer.Write("Price");

                    memoryBuffer.Write("\n");

                    ExportFeedStatistic.TotalCategories = 0;
                    ExportFeedStatistic.TotalProducts = GetProdutsCount(ModuleName); ;

                    foreach (var productRow in GetProduts(ModuleName))
                    {
                        ProcessProductRow(productRow, memoryBuffer);
                        ExportFeedStatistic.CurrentProduct++;
                    }

                    memoryBuffer.Flush();
                    SetShopUrlToNull();
                }
            }
        }

        private void ProcessProductRow(ExportFeedProduts row, StreamWriter memoryBuffer)
        {
            var nfi = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();
            nfi.NumberDecimalSeparator = ".";

            //Category
            var categorizationBuffer = new StringBuilder();
            Category category = CategoryService.GetCategory(row.ParentCategory);
            categorizationBuffer.Insert(0, category.Name);
            while (category.ParentCategoryId != 0)
            {
                category = CategoryService.GetCategory(category.ParentCategoryId);
                categorizationBuffer.Insert(0, category.Name + " >> ");
            }
            memoryBuffer.Write(categorizationBuffer.ToString());
            memoryBuffer.Write("\t");

            //Manufacturer
            memoryBuffer.Write("\t");

            //Title
            memoryBuffer.Write(row.Name);
            memoryBuffer.Write("\t");

            //Description
            string desc = _description == "full" ? row.Description : row.BriefDescription;

            memoryBuffer.Write(!string.IsNullOrEmpty(desc) ? desc : Resource.ExportFeed_NoDescription);
            memoryBuffer.Write("\t");

            //Link
            memoryBuffer.Write(SettingsGeneral.AbsoluteUrl.TrimEnd('/') + "/" + UrlService.GetLink(ParamType.Product, row.UrlPath, row.ProductID));
            memoryBuffer.Write("\t");

            //Image
            var temp = SQLDataHelper.GetString(row.Photo);
            if (!string.IsNullOrEmpty(temp))
            {
                //memoryBuffer.Write(GetShopUrl().EndsWith("/") ? GetShopUrl() : GetShopUrl() + "/" + ImageFolders.GetImageProductPath(ProductImageType.Middle, temp, false));
                memoryBuffer.Write(GetImageProductPath(row.Photo));
            }
            memoryBuffer.Write("\t");

            //SKU
            memoryBuffer.Write(row.ProductID.ToString());
            memoryBuffer.Write("\t");

            //Quantity on Hand
            memoryBuffer.Write("\t");

            //Condition
            memoryBuffer.Write("new");
            memoryBuffer.Write("\t");

            ////Shipping Weight
            //memoryBuffer.Write(SQLDataHelper.GetDecimal(row["Weight"]));
            //memoryBuffer.Write("\t");

            ////Shipping Cost
            //memoryBuffer.Write(SQLDataHelper.GetDecimal(row["ShippingPrice"]).ToString(nfi));
            //memoryBuffer.Write("\t");

            //Bid
            memoryBuffer.Write("\t");

            //Promo Text
            memoryBuffer.Write("\t");

            //UPC
            memoryBuffer.Write("\t");

            //Price
            memoryBuffer.Write(CatalogService.CalculatePrice(row.Price, row.Discount).ToString(nfi));
            memoryBuffer.Write("\n");
        }
    }
}