//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using AdvantShop.Catalog;
using AdvantShop.Core;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using AdvantShop.Orders;
using AdvantShop.Statistic;
using Yogesh.ExcelXml;

namespace AdvantShop.ExportImport.Excel
{
    public class ExcelXmlWriter
    {
        #region Orders saving

        public bool SaveOrdersToXml(string filename, IEnumerable<Order> orders)
        {
            try
            {
                OrdersArrayToWorkbook(orders).Export(filename);
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                return false;
            }
        }
        private static string RenderOrderedItems(IEnumerable<OrderItem> items)
        {
            var res = new StringBuilder();
            foreach (OrderItem orderItem in items)
            {
                res.AppendFormat("[{0} - {1}{2}{3}], ", orderItem.Name, orderItem.Amount,
                    Resources.Resource.Admin_ExportOrdersExcel_Pieces, orderItem.SelectedOptions != null && orderItem.SelectedOptions.Count > 0 ? RenderSelectedOptions(orderItem.SelectedOptions) : string.Empty);
            }
            return res.ToString().TrimEnd(new[] { ',', ' ' });
        }

        //Added by Evgeni to return order Articles
        private static string RenderOrderedItemsArticles(IEnumerable<OrderItem> items)
        {
            var res = new StringBuilder();
            foreach (OrderItem orderItem in items)
            {
                res.AppendFormat("[{0}], ",orderItem.ArtNo);
            }
            return res.ToString().TrimEnd(new[] { ',', ' ' });
        }

        private static ExcelXmlWorkbook OrdersArrayToWorkbook(IEnumerable<Order> orders)
        {
            var book = new ExcelXmlWorkbook { Properties = { Author = "AdvantShop.Net", LastAuthor = "AdvantShop.Net" }, DefaultStyle = new XmlStyle { Font = { Name = "Calibri", Size = 11 } } };

            Worksheet sheet = book[0];

            sheet.Name = "Orders";

            sheet.PrintOptions.Orientation = PageOrientation.Landscape;
            sheet.PrintOptions.SetMargins(0.5, 0.4, 0.5, 0.4);

            //XLS columns definition

            sheet.Columns(0).Width = 50;
            sheet.Columns(1).Width = 80;
            sheet.Columns(2).Width = 100;
            sheet.Columns(3).Width = 130;
            sheet.Columns(4).Width = 70;
            sheet.Columns(5).Width = 150;
            sheet.Columns(6).Width = 80;
            sheet.Columns(7).Width = 80;
            sheet.Columns(8).Width = 80;
            sheet.Columns(9).Width = 80;
            sheet.Columns(10).Width = 300;
            sheet.Columns(11).Width = 300;
            sheet.Columns(12).Width = 300;
            sheet.Columns(13).Width = 100;
            sheet.Columns(14).Width = 300;
            sheet.Columns(15).Width = 300;
            sheet.Columns(16).Width = 300;
            

            sheet[0, 0].Value = "OrderID";
            sheet[1, 0].Value = "Status";
            sheet[2, 0].Value = "OrderDate";
            sheet[3, 0].Value = "FIO";
            sheet[4, 0].Value = "CustomerIP";
            sheet[5, 0].Value = "OrderedItemsArt";
            sheet[6, 0].Value = "Total";
            sheet[7, 0].Value = "Tax";
            sheet[8, 0].Value = "Cost";
            sheet[9, 0].Value = "Profit";
            sheet[10, 0].Value = "Payment";
            sheet[11, 0].Value = "Shipping";
            sheet[12, 0].Value = "Shipping Address";
            sheet[13, 0].Value = "Customer Phone";
            sheet[14, 0].Value = "Customer Comment";
            sheet[15, 0].Value = "Admin Comment";
            sheet[16, 0].Value = "OrderedItems";



            var i = 1;
            foreach (Order order in orders)
            {
                if (!ExportStatistic.IsRun) return book;

                ExportStatistic.RowPosition++;
                //Order to XLS row

                sheet[0, i].Value = order.OrderID;
                sheet[1, i].Value = order.OrderStatus.StatusName;
                sheet[2, i].Value = order.OrderDate;
                if (order.OrderCustomer != null)
                {
                    sheet[3, i].Value = order.OrderCustomer.LastName + " " + order.OrderCustomer.FirstName;
                    sheet[4, i].Value = order.OrderCustomer.CustomerIP ?? string.Empty;
                }
                else
                {
                    sheet[3, i].Value = "Неизвестный";
                    sheet[4, i].Value = string.Empty;
                }
                //Changed by Evgeni to return order Articles
                sheet[5, i].Value = RenderOrderedItemsArticles(order.OrderItems) ?? string.Empty;
                sheet[6, i].Value = CatalogService.GetStringPrice(order.Sum, order.OrderCurrency);
                sheet[7, i].Value = CatalogService.GetStringPrice(order.TaxCost, order.OrderCurrency);
                decimal totalCost = order.OrderItems.Sum(oi => oi.SupplyPrice * oi.Amount);
                sheet[8, i].Value = CatalogService.GetStringPrice(totalCost, order.OrderCurrency);
                sheet[9, i].Value = CatalogService.GetStringPrice(order.Sum - order.ShippingCost - order.TaxCost - totalCost, order.OrderCurrency);
                sheet[10, i].Value = order.PaymentMethodName;
                sheet[11, i].Value = order.ArchivedShippingName + " - " + CatalogService.GetStringPrice(order.ShippingCost, order.OrderCurrency);
                sheet[12, i].Value = order.ShippingContact != null
                                         ? order.ShippingContact.Country + ", " + order.ShippingContact.City + ", " +
                                         order.ShippingContact.Address : string.Empty;
                sheet[13, i].Value = order.OrderCustomer!= null ? order.OrderCustomer.MobilePhone : string.Empty;
                sheet[14, i].Value = order.CustomerComment ?? string.Empty;
                sheet[15, i].Value = order.AdminOrderComment ?? string.Empty;
                sheet[16, i].Value = RenderOrderedItems(order.OrderItems) ?? string.Empty;

                i++;
            }

            return book;
        }

        #endregion

        #region Product saving

        public static void SaveProductsToXml(string filename)
        {
            var book = new ExcelXmlWorkbook { Properties = { Author = "AdvantShop.Net", LastAuthor = "AdvantShop.Net" } };
            //-----------------------------------------------
            // Properties
            //-----------------------------------------------
            var style = new XmlStyle { Font = { Name = "Calibri", Size = 11 } };
            book.DefaultStyle = style;
            GenerateProductWorksheet(book[0], ProductService.GetProducts());
            book.Export(filename);
        }

        private static void GenerateProductWorksheet(Worksheet sheet, IEnumerable<Product> products)
        {
            sheet.Name = "Products";

            sheet.PrintOptions.Orientation = PageOrientation.Landscape;
            sheet.PrintOptions.SetMargins(0.5, 0.4, 0.5, 0.4);

            sheet.Columns(0).Width = 66;
            sheet.Columns(1).Width = 132;
            sheet.Columns(2).Width = 237;
            sheet.Columns(3).Width = 60;
            sheet.Columns(4).Width = 72;
            sheet.Columns(5).Width = 39;
            sheet.Columns(6).Width = 39;
            sheet.Columns(7).Width = 66;
            sheet.Columns(8).Width = 90;
            sheet.Columns(9).Width = 61;
            sheet.Columns(10).Width = 97;
            sheet.Columns(11).Width = 217;
            sheet.Columns(12).Width = 282;

            sheet[0, 0].Value = "SKU";
            sheet[1, 0].Value = "Name*";
            sheet[2, 0].Value = "ParamSynonym";
            sheet[3, 0].Value = "Category";
            sheet[4, 0].Value = "Enabled*";
            sheet[5, 0].Value = "Price*";
            sheet[6, 0].Value = "PurchasePrice*";
            sheet[7, 0].Value = "Amount*";
            sheet[8, 0].Value = "Unit";
            sheet[9, 0].Value = "Discount";
            sheet[10, 0].Value = "ShippingPrice";
            sheet[11, 0].Value = "Weight";
            sheet[12, 0].Value = "Size";
            sheet[13, 0].Value = "BriefDescription";
            sheet[14, 0].Value = "Description";

            int i = 1;
            foreach (Product product in products)
            {
                if (!ExportStatistic.IsRun) return;
                ExportStatistic.RowPosition++;
                sheet[0, i].Value = product.ArtNo;
                sheet[1, i].Value = product.Name;
                sheet[2, i].Value = product.UrlPath;
                sheet[3, i].Value = GetCategoryStringByProductId(product.ProductId);
                sheet[4, i].Value = product.Enabled ? "+" : "-";
                var offer = product.Offers.FirstOrDefault() ?? new Offer { Amount = 0, Price = 0, SupplyPrice = 0, Unit = "", ShippingPrice = 0 };
                sheet[5, i].Value = offer.Price.ToString("F2");
                sheet[6, i].Value = offer.SupplyPrice.ToString("F2");
                sheet[7, i].Value = offer.Amount.ToString(CultureInfo.InvariantCulture);
                sheet[8, i].Value = offer.Unit;
                sheet[10, i].Value = offer.ShippingPrice.ToString("F2");

                sheet[9, i].Value = product.Discount.ToString("F2");
                sheet[11, i].Value = product.Weight.ToString("F2");
                sheet[12, i].Value = product.Size.Replace("|", " x ");
                sheet[13, i].Value = product.BriefDescription;
                sheet[14, i].Value = product.Description;
                i++;
            }
        }

        private static string GetCategoryStringByProductId(int productId)
        {
            var strResSb = new StringBuilder();

            try
            {
                using (var db = new SQLDataAccess())
                {
                    db.cmd.CommandText = "Select CategoryID from Catalog.ProductCategories where ProductID=@id";
                    db.cmd.CommandType = CommandType.Text;
                    db.cmd.Parameters.Clear();
                    db.cmd.Parameters.AddWithValue("@id", productId);
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

        private static string GetParentCategoriesAsString(int childCategoryId, SQLDataAccess dbAccess)
        {
            var res = "";
            var categoies = CategoryService.GetParentCategories(childCategoryId, dbAccess);
            for (var i = categoies.Count - 1; i >= 0; i--)
            {
                if (i != categoies.Count - 1)
                {
                    res += " >> ";
                }
                res += categoies[i].Name;
            }
            return res;
        }
        #endregion

        protected static string RenderSelectedOptions(IList<EvaluatedCustomOptions> evlist)
        {
            var html = new StringBuilder();
            if (evlist != null && evlist.Count > 0)
            {
                html.Append(" (");

                foreach (EvaluatedCustomOptions ev in evlist)
                {
                    html.Append(string.Format("{0}: {1},", ev.CustomOptionTitle, ev.OptionTitle));
                }

                html.Append(")");
            }
            return html.ToString();
        }

    }
}