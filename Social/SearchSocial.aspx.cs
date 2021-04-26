//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using AdvantShop;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Controls;
using AdvantShop.Core;
using AdvantShop.Core.FieldFilters;
using AdvantShop.Customers;
using AdvantShop.FullSearch;
using AdvantShop.Repository.Currencies;

public partial class SearchSocial : AdvantShopPage
{
    protected string SearchTerm = string.Empty;
    SortOrder sort = SortOrder.NoSorting;

    protected void Page_Load(object sender, EventArgs e)
    {
        
        defaultDS.TableName = "[Catalog].[Product] LEFT JOIN [Catalog].[Offer] ON [Product].[ProductID] = [Offer].[ProductID] LEFT JOIN [Catalog].[Photo] ON [Product].[ProductID] = [Photo].[ObjId] and Type='Product' AND [Main] = 1 inner join Catalog.ProductCategories on ProductCategories.ProductId = [Product].[ProductID] Left JOIN [Catalog].[ProductPropertyValue] ON [Product].[ProductID] = [ProductPropertyValue].[ProductID] LEFT JOIN [Catalog].[ShoppingCart] ON [Catalog].[ShoppingCart].[EntityID] = [Catalog].[Product].[ProductID] AND [Catalog].[ShoppingCart].[ShoppingCartTypeId] = 3 AND [ShoppingCart].[CustomerID] = @CustomerId Left JOIN [Catalog].[Ratio] on Product.ProductId= Ratio.ProductID and Ratio.CustomerId=@CustomerId";
        defaultDS.AddParamSql(new SqlParam { ParamName = "@CustomerId", ParamValue = CustomerSession.CustomerId.ToString() });


        defaultDS.Fields["[Product].[ProductID]"].Filter = new CountProductInCategory();
        defaultDS.Fields["OfferListID"].Filter = new EqualFieldFilter
                                                     {
                                                         ParamName = "@Filter",
                                                         Value = CatalogService.CurrentOfferListId.ToString(CultureInfo.InvariantCulture)
                                                     };

        defaultDS.Fields["Enabled"].Filter = new EqualFieldFilter { ParamName = "@enabled", Value = "1" };
        defaultDS.Fields["HirecalEnabled"].Filter = new EqualFieldFilter { ParamName = "@HirecalEnabled", Value = "1" };

        BuildSorting();
        BuildFilter();

        SetMeta(null, string.Empty);
        txtName.Focus();
    }

    private void BuildSorting()
    {
        if (!string.IsNullOrEmpty(Request["sort"]))
        {
            Enum.TryParse(Request["sort"], true, out sort);
        }


        foreach (SortOrder enumItem in Enum.GetValues(typeof(SortOrder)))
        {
            ddlSort.Items.Add(new ListItem
                                  {
                                      Text = enumItem.GetLocalizedName(),
                                      Value = enumItem.ToString(),
                                      Selected = sort == enumItem
                                  });
        }

        switch (sort)
        {
            case SortOrder.AscByName:
                defaultDS.Fields["Name"].Sorting = SortDirection.Ascending;
                break;

            case SortOrder.DescByName:
                defaultDS.Fields["Name"].Sorting = SortDirection.Descending;
                break;

            case SortOrder.AscByPrice:
                defaultDS.ExtendedSorting = "Price - Price * Discount / 100";
                defaultDS.ExtendedSortingDirection = SortDirection.Ascending;
                break;

            case SortOrder.DescByPrice:
                defaultDS.ExtendedSorting = "Price - Price * Discount / 100";
                defaultDS.ExtendedSortingDirection = SortDirection.Descending;
                break;

            case SortOrder.AscByRatio:
                defaultDS.Fields["ShowRatio"].Sorting = SortDirection.Descending;
                defaultDS.Fields["Ratio"].Sorting = SortDirection.Ascending;
                break;

            case SortOrder.DescByRatio:
                defaultDS.Fields["ShowRatio"].Sorting = SortDirection.Descending;
                defaultDS.Fields["Ratio"].Sorting = SortDirection.Descending;
                break;
        }
    }

    private void BuildFilter()
    {

        foreach (Category c in (CategoryService.GetChildCategoriesByCategoryId(0, true).Where(p => p.Enabled)))
        {
            ddlCategory.Items.Add(new ListItem { Text = c.Name, Value = c.CategoryId.ToString(), Selected = Request["category"] == c.CategoryId.ToString() });
        }

        defaultDS.Fields["[ProductCategories].[CategoryID]"].Filter = new InChildCategoriesFieldFilter
        {
            CategoryId = ddlCategory.SelectedValue,
            ParamName = "@CategoryID"
        };


        if (!string.IsNullOrEmpty(Page.Request["name"]))
        {
            var name = HttpUtility.UrlDecode(Page.Request["name"]).Trim();
            txtName.Text = name;
            var productIds = LuceneSearch.Search(txtName.Text).AggregateString('/');
            defaultDS.TableName += " inner join (select item, sort from [Settings].[ParsingBySeperator](@source,'/') ) as dtt on Product.ProductId=convert(int, dtt.item) ";
            defaultDS.AddParamSql(new SqlParam { ParamName = "@source", ParamValue = productIds });
            if (sort == SortOrder.NoSorting)
            {
                defaultDS.Fields.Add("sort", new Field("sort"));
                defaultDS.Fields["sort"].Sorting = SortDirection.Ascending;
            }


            SearchTerm = HttpUtility.HtmlEncode(name);
        }

        //if (!string.IsNullOrEmpty(Page.Request["sku"]))
        //{
        //    var skuHtml = HttpUtility.UrlDecode(Page.Request["sku"]);
        //    txtSKU.Text = HttpUtility.HtmlDecode(skuHtml).Trim();
        //    defaultDS.Fields["ArtNo"].Filter = new EqualFieldFilter() { Value = txtSKU.Text, ParamName = "@artno" };
        //    if (SearchTerm.IsNotEmpty()) SearchTerm += " - ";
        //    SearchTerm += HttpUtility.HtmlEncode(txtSKU.Text);
        //}

        filterPrice.CategoryId = 0;
        filterPrice.InDepth = true;

        if (!string.IsNullOrEmpty(Request["pricefrom"]) || !string.IsNullOrEmpty(Request["priceto"]))
        {
            int pricefrom = Request["pricefrom"].TryParseInt(0);
            int priceto = Request["priceto"].TryParseInt(int.MaxValue);

            filterPrice.CurValMin = pricefrom;
            filterPrice.CurValMax = priceto;

            defaultDS.Fields["discountPrice"].Filter = new RangeFieldFilter
            {
                ParamName = "@priceRange",
                From = pricefrom * CurrencyService.CurrentCurrency.Value,
                To = priceto * CurrencyService.CurrentCurrency.Value
            };

            // why?
            //defaultDS.Fields["Price"].Filter = new NotEqualFieldFilter { ParamName = "@price", Value = "0" };
        }
        else
        {
            filterPrice.CurValMin = 0;
            filterPrice.CurValMax = int.MaxValue;
        }
    }

    protected void Page_Prerender(object sender, EventArgs e)
    {
        defaultDS.CurrentPageIndex = paging.CurrentPage != 0 ? paging.CurrentPage : 1;
        defaultDS.ItemsPerPage = SettingsCatalog.ProductsPerPage;

        vProducts.DataSource = defaultDS.Items;
        vProducts.ViewMode = (UserControls_ProductViewSocial.ProductViewMode)productViewChanger.SearchViewMode;
        vProducts.DataBind();
        paging.TotalPages = defaultDS.PageCount;
        int itemsCount = defaultDS.TotalCount;
        lItemsCount.Text = string.Format("{0} {1}", itemsCount.ToString(), Strings.Numerals(itemsCount, Resources.Resource.Client_Searsh_NoResults,
                                                                                            Resources.Resource.Client_Searsh_1Result,
                                                                                            Resources.Resource.Client_Searsh_2Results,
                                                                                            Resources.Resource.Client_Searsh_5Results));
        pnlSort.Visible = itemsCount > 0;
    }
}