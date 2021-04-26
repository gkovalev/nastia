//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AdvantShop.Core;
using AdvantShop.Helpers;
using AdvantShop.Catalog;
using AdvantShop.Diagnostics;

namespace AdvantShop.Taxes
{
    public class TaxCategorySelectionState
    {
        public Category Category;
        public bool Full;
    }

    public class TaxLasyLoad
    {

        public static void SetProductSelectionState(int productID, int TaxID)
        {
            TaxServices.SwitchProductTax(productID, TaxID, true, false);
        }

        public static void RecalcCategory(int categoryID, int taxID)
        {
            int selProdCount = GetSelectedProductsCount(categoryID, taxID);
            int selCatCount = GetSelectedSubCategoriesCount(categoryID, taxID);
            SetCategorySelectionState(categoryID.ToString(), taxID, !(selProdCount == 0 && selCatCount == 0), false);
            Category cat = CategoryService.GetCategory(categoryID);
            if (cat.CategoryId != cat.ParentCategoryId)
                RecalcCategory(cat.ParentCategoryId, taxID);
        }

        public static void SetCategorySelectionState(string categoryID, int taxID, bool state, bool full)
        {
            try
            {
                using (var db = new SQLDataAccess())
                {
                    db.cmd.CommandText = state ? full ? "[Catalog].[sp_TaxSelectCategoryFull]" : "[Catalog].[sp_TaxSelectCategory]" : "[Catalog].[sp_TaxDeSelectCategory]";
                    db.cmd.CommandType = CommandType.StoredProcedure;
                    db.cmd.Parameters.AddWithValue("@CategoryID", categoryID);
                    db.cmd.Parameters.AddWithValue("@TaxID", taxID);
                    db.cnOpen();
                    db.cmd.ExecuteNonQuery();
                    db.cnClose();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }

        public static TaxCategorySelectionState[] GetSelectedCategoriesByParentCategoryId(int catId, string taxID)
        {
            var res = new List<TaxCategorySelectionState>();

            using (var db = new SQLDataAccess())
            {
                db.cmd.CommandText =
                    @"SELECT [Catalog].[Category].[CategoryId] 
                        FROM  [Catalog].[Category] 
                            inner join [Catalog].[TaxSelectedCategories] 
                                on [Category].[ID] = [TaxSelectedCategories].[CategoryID] 
                        WHERE [Catalog].[Category].[ParentCategory] = @catID AND [Catalog].[Category].[CategoryId] <> @catId and [TaxID] = @TaxID";
                db.cmd.CommandType = CommandType.Text;
                db.cmd.Parameters.Clear();
                db.cmd.Parameters.AddWithValue("@catID", catId);
                db.cmd.Parameters.AddWithValue("@TaxID", taxID);

                db.cnOpen();

                using (var reader = db.cmd.ExecuteReader())
                    while (reader.Read())
                    {
                        var c = new Category { CategoryId = SQLDataHelper.GetInt(reader, "CategoryId"), ParentCategoryId = catId };

                        var cs = new TaxCategorySelectionState { Category = c, Full = false };

                        TaxCategorySelectionState[] subCats = GetSelectedCategoriesByParentCategoryId(c.CategoryId, taxID);

                        res.Add(cs);
                        res.AddRange(subCats);
                    }

                db.cnClose();
            }


            return res.ToArray();
        }
        //отметить категорию или нет
        public static bool GetCategorySelectionState(int categoryId, int taxID)
        {
            bool result = false;
            try
            {
                using (var db = new SQLDataAccess())
                {
                    db.cmd.CommandText =
                        "SELECT 'True' FROM [Catalog].[TaxSelectedCategories] WHERE [TaxID] = @TaxID AND [CategoryID] = @CategoryID;";
                    db.cmd.CommandType = CommandType.Text;
                    db.cmd.Parameters.Clear();
                    db.cmd.Parameters.AddWithValue("@CategoryID", categoryId);
                    db.cmd.Parameters.AddWithValue("@TaxID", taxID);
                    db.cnOpen();
                    using (var reader = db.cmd.ExecuteReader())
                        result = reader.Read();
                    db.cnClose();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }

            return result;
        }

        public static bool GetProductSelectionState(int productId, int taxID)
        {
            bool result = false;
            try
            {
                using (var da = new SQLDataAccess())
                {
                    da.cmd.CommandType = CommandType.Text;
                    da.cmd.CommandText =
                        "select count(Product.ProductID) from catalog.Product inner join catalog.TaxMappingOnProduct on TaxMappingOnProduct.ProductID = Product.ProductID where Product.ProductID=@ProductID and [TaxID] = @TaxID";
                    da.cmd.Parameters.Clear();
                    da.cmd.Parameters.AddWithValue("@ProductID", productId);
                    da.cmd.Parameters.AddWithValue("@TaxID", taxID);

                    da.cnOpen();
                    var useTax = SQLDataHelper.GetInt(da.cmd.ExecuteScalar());
                    result = useTax != 0;
                    da.cnClose();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
            return result;
        }

        public static void ProcessCategoryRow(Category category, int depth, int parentCategoryId, Table table, bool recurse, int taxID)
        {

            string parentCategoryPath = GetCategoryPath(category.CategoryId);

            var catTableRow = new TableRow();

            catTableRow.Attributes.Add("class", "categoryRow");
            catTableRow.Attributes.Add("onmouseover", "onDataRowMouseOver(this);");
            catTableRow.Attributes.Add("onmouseout", "onDataRowMouseOut(this);");
            table.Controls.Add(catTableRow);

            var catNameBlock = new TableCell();
            var margin = 5;
            margin += 20 * depth;

            var cellContent = new HtmlGenericControl("div");
            cellContent.Attributes.Add("class", "categoryIndentFiller");
            cellContent.Style.Add("margin-left", margin.ToString());
            catNameBlock.Controls.Add(cellContent);
            // todo eVo: remove to resurce
            var expandImage = new HtmlImage { Alt = Resources.Resource.TreeView_OpenCategory, Src = "~/Admin/images/expand_blue.jpg" };


            expandImage.Attributes.Add("class", "categoryImage");
            if (category.ProductsCount == 0)
            {
                expandImage.Src = "~/Admin/images/empty.gif";
            }

            cellContent.Controls.Add(expandImage);

            var checkBox = new CheckBox
                               {
                                   ID = parentCategoryPath + "Category",
                                   EnableViewState = true,
                                   Checked = GetCategorySelectionState(category.CategoryId, taxID)
                               };

            var catSelectionState = new HiddenField { ID = parentCategoryPath + "Category_State" };
            int ssc = GetSelectedSubCategoriesCount(category.CategoryId, taxID);
            int ssp = GetSelectedProductsCount(category.CategoryId, taxID);
            if (ssc == category.ChildCategories.Count && ssp == category.Products.Count)
            {
                catSelectionState.Value = "full";
            }
            else if (ssc != 0 || ssp != 0)
            {
                catSelectionState.Value = "partial";
            }

            cellContent.Controls.Add(catSelectionState);

            cellContent.Controls.Add(checkBox);


            var signLink = new HtmlAnchor();
            cellContent.Controls.Add(signLink);
            signLink.Attributes.Add("class", "categoryLink");

            var catNameLiteral = new HtmlGenericControl("span") { InnerText = category.Name };
            catNameLiteral.Attributes.Add("class", "categoryLiteral");
            catNameLiteral.InnerHtml += "&nbsp;<span>(" + category.ProductsCount + ")</span>";
            signLink.Controls.Add(catNameLiteral);
            catNameBlock.ColumnSpan = 3;
            catTableRow.Cells.Add(catNameBlock);

            catTableRow.ID = parentCategoryPath + "TableRow";


            if (recurse)
            {
                foreach (var cat in category.ChildCategories)
                {
                    ProcessCategoryRow(cat, depth + 1, category.CategoryId, table, false, taxID);
                }

                foreach (var pr in category.Products)
                {
                    var tr = ProcessProductRow(pr, depth + 1, category.CategoryId, taxID);
                    table.Controls.Add(tr);
                }
            }
        }

        public static TableRow ProcessProductRow(Product product, int depth, int parentCategoryId, int taxID)
        {

            string parentCategoryPath = GetCategoryPath(parentCategoryId);

            var productTableRow = new TableRow();
            productTableRow.Attributes.Add("class", "productRow");
            productTableRow.Attributes.Add("onmouseover", "onDataRowMouseOver(this);");
            productTableRow.Attributes.Add("onmouseout", "onDataRowMouseOut(this);");
            productTableRow.Attributes.Add("onclick", "ProductRow_ClickHandler(this);");


            var productNameBlock = new TableCell();
            var margin = 15;
            for (var i = 1; i <= depth; i++)
            {
                margin += 20;
            }

            var span = new HtmlGenericControl("span");
            span.Style.Add("margin-left", margin.ToString());
            span.Style.Add("display", "block");
            span.Style.Add("position", "relative");

            var chkSpan = new HtmlGenericControl("span");
            chkSpan.Style.Add("position", "absolute");
            chkSpan.Style.Add("top", "50%");

            span.Controls.Add(chkSpan);

            var checkBox = new CheckBox
                               {
                                   ID = parentCategoryPath + product.ProductId + "_Product",
                                   Checked = GetProductSelectionState(product.ProductId, taxID)
                               };
            checkBox.Style.Add("position", "absolute");

            if (HttpContext.Current.Request.Browser.Browser == "IE")
            {
                checkBox.Style.Add("top", "-15px");
            }
            else
            {
                checkBox.Style.Add("top", "-9px");
            }

            chkSpan.Controls.Add(checkBox);

            var productNameLiteral = new HtmlGenericControl("span") { InnerText = product.Name };
            productNameLiteral.Attributes.Add("class", "productLiteral");
            productNameBlock.Controls.Add(productNameLiteral);
            span.Controls.Add(productNameLiteral);
            productNameBlock.Controls.Add(span);
            productTableRow.Cells.Add(productNameBlock);

            var priceBlock = new TableCell();
            productTableRow.Cells.Add(priceBlock);

            var quantBlock = new TableCell();
            productTableRow.Cells.Add(quantBlock);

            if (product.Offers.Count > 0 && (product.Offers[0] != null))
            {
                priceBlock.Text = CatalogService.GetStringPrice(product.Offers[0].Price);
                quantBlock.Text = product.Offers[0].Unit;
            }

            productTableRow.ID = parentCategoryPath + product.ProductId + "_TableRow";

            return productTableRow;
        }

        private static string GetCategoryPath(int catId)
        {
            if (catId == 0)
            {
                return "_0_";
            }
            var sb = new StringBuilder();
            Category cat = CategoryService.GetCategory(catId);
            while (cat != null)
            {
                sb.Insert(0, cat.CategoryId + "_", 1);
                if (cat.CategoryId != cat.ParentCategoryId)
                {
                    cat = CategoryService.GetCategory(cat.ParentCategoryId);
                }
                else
                {
                    sb.Insert(0, "_", 1);
                    return sb.ToString();
                }
            }

            return sb.ToString();
        }

        public static int GetSelectedSubCategoriesCount(int parentCategoryId, int taxID)
        {
            using (var da = new SQLDataAccess())
            {
                da.cmd.CommandType = CommandType.Text;
                da.cmd.CommandText =
                    @"SELECT count(*) 
                        FROM [Catalog].[Category] 
                            inner join [Catalog].[TaxSelectedCategories] on [Category].[CategoryID] = [TaxSelectedCategories].[CategoryID] 
                        where [Category].[ParentCategory] = @ParentCategory and [Category].[CategoryID] <> @ParentCategory and [TaxID] = @TaxID";
                //"Select Count([WhereHasTax]) from (SELECT (select count(Product.ID) from catalog.Product inner join catalog.TaxMappingOnProduct on TaxMappingOnProduct.ProductID = Product.ID inner join Catalog.ProductCategories on ProductCategories.ProductID = Product.ID where ProductCategories.CategoryID= p.ID) AS [WhereHasTax] FROM [Catalog].[Category] AS p WHERE [ParentCategory] = @id AND ID <> '0' ) as Row where [WhereHasTax] > 0 ";
                da.cmd.Parameters.Clear();
                da.cmd.Parameters.AddWithValue("@ParentCategory", parentCategoryId);
                da.cmd.Parameters.AddWithValue("@TaxID", taxID);

                da.cnOpen();
                var result = SQLDataHelper.GetInt(da.cmd.ExecuteScalar());
                da.cnClose();

                return result;
            }
        }

        public static int GetSelectedProductsCount(int parentCategoryId, int taxID)
        {
            using (var da = new SQLDataAccess())
            {
                da.cmd.CommandType = CommandType.Text;
                da.cmd.CommandText =
                    "select count(Product.ProductID) from catalog.Product inner join catalog.TaxMappingOnProduct on TaxMappingOnProduct.ProductID = Product.ProductID inner join Catalog.ProductCategories on ProductCategories.ProductID = Product.ProductID where ProductCategories.CategoryID=@catID and [TaxID] = @taxID";
                da.cmd.Parameters.Clear();
                da.cmd.Parameters.AddWithValue("@catID", parentCategoryId);
                da.cmd.Parameters.AddWithValue("@taxID", taxID);

                da.cnOpen();
                var result = SQLDataHelper.GetInt(da.cmd.ExecuteScalar());
                da.cnClose();

                return result;
            }
        }
    }
}