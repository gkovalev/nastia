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
using AdvantShop.SEO;
using Newtonsoft.Json;
using Resources;

public partial class Search : AdvantShopPage
{
    protected string SearchTerm = string.Empty;
    SortOrder _sort = SortOrder.NoSorting;

    protected void Page_Load(object sender, EventArgs e)
    {
        defaultDS.TableName = "[Catalog].[Product] LEFT JOIN [Catalog].[Offer] ON [Product].[ProductID] = [Offer].[ProductID] LEFT JOIN [Catalog].[Photo] ON [Product].[ProductID] = [Photo].[ObjId] and Type='Product' AND [Main] = 1 inner join Catalog.ProductCategories on ProductCategories.ProductId = [Product].[ProductID] Left JOIN [Catalog].[ProductPropertyValue] ON [Product].[ProductID] = [ProductPropertyValue].[ProductID] LEFT JOIN [Catalog].[ShoppingCart] ON [Catalog].[ShoppingCart].[EntityID] = [Catalog].[Product].[ProductID] AND [Catalog].[ShoppingCart].[ShoppingCartTypeId] = 3 AND [ShoppingCart].[CustomerID] = @CustomerId Left JOIN [Catalog].[Ratio] on Product.ProductId= Ratio.ProductID and Ratio.CustomerId=@CustomerId";
        defaultDS.AddParamSql(new SqlParam { ParamName = "@CustomerId", ParamValue = CustomerSession.CustomerId.ToString() });

        defaultDS.Fields["OfferListID"].Filter = new EqualFieldFilter
                                                     {
                                                         ParamName = "@Filter",
                                                         Value = CatalogService.CurrentOfferListId.ToString(CultureInfo.InvariantCulture)
                                                     };

        defaultDS.Fields["Enabled"].Filter = new EqualFieldFilter { ParamName = "@enabled", Value = "1" };
        defaultDS.Fields["HirecalEnabled"].Filter = new EqualFieldFilter { ParamName = "@HirecalEnabled", Value = "1" };

        BuildSorting();
        BuildFilter();

        SetMeta(new MetaInfo(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Client_Search_AdvancedSearch)), string.Empty);
        txtName.Focus();
    }

    private void BuildSorting()
    {
        if (!string.IsNullOrEmpty(Request["sort"]))
        {
            Enum.TryParse(Request["sort"], true, out _sort);
        }


        foreach (SortOrder enumItem in Enum.GetValues(typeof(SortOrder)))
        {
            ddlSort.Items.Add(new ListItem
                                  {
                                      Text = enumItem.GetLocalizedName(),
                                      Value = enumItem.ToString(),
                                      Selected = _sort == enumItem
                                  });
        }

        switch (_sort)
        {
          //Added by Evgeni to improve sorting capabilities
            case SortOrder.NoSorting:
                //defaultDS.ExtendedSorting = "Sort";
                //defaultDS.ExtendedSortingDirection = SortDirection.Descending;
                var amoubtField = new Field();
                amoubtField.FilterExpression = "AmountSort";
                amoubtField.Name = "AmountSort";
              //  amoubtField.SelectExpression = "'AmountSort' = CASE WHEN Offer.Amount = 0 THEN 0 ELSE 10 END";
                amoubtField.SelectExpression = "'AmountSort' = CASE WHEN sort < 3 THEN sort WHEN Offer.Amount > 0 AND Offer.Price > 0 THEN sort WHEN Offer.Amount = 0 AND Offer.Price = 0 THEN sort + 100 WHEN Offer.Amount = 0 AND Offer.Price > 0 THEN sort + 50 ELSE sort END";
                defaultDS.Fields.Add("AmountSort", amoubtField);
                defaultDS.Fields["AmountSort"].Sorting = SortDirection.Ascending;

                //defaultDS.Fields["Offer.Amount"].Sorting = SortDirection.Descending;
                break;
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
                defaultDS.Fields["Ratio"].Sorting = SortDirection.Ascending;
                break;

            case SortOrder.DescByRatio:
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
            if (_sort == SortOrder.NoSorting)
            {
                defaultDS.Fields.Add("sort", new Field("sort"));
                defaultDS.Fields["sort"].Sorting = SortDirection.Ascending;
            }


            SearchTerm = HttpUtility.HtmlEncode(name);
        }

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
        defaultDS.ItemsPerPage = paging.CurrentPage != 0 ? SettingsCatalog.ProductsPerPage :int.MaxValue;
        var pageCount = defaultDS.PageCount;
        var totalCount = defaultDS.TotalCount;
        paging.TotalPages = (int)(Math.Ceiling((double)totalCount / defaultDS.ItemsPerPage));

        if ( totalCount !=0 && paging.TotalPages < paging.CurrentPage || paging.CurrentPage < 0)
        {
            Error404();
            return;
        }

        var prices = defaultDS.AvaliblePrices;
        // if we get request from ajax filter
        if (Request["ajax"] == "1")
        {
            Response.Clear();
            Response.ContentType = "application/json";
            
            var res = JsonConvert.SerializeObject(new
            {
                ProductsCount = totalCount,
                AvaliblePriceFrom = Math.Floor(prices.Key / CurrencyService.CurrentCurrency.Value),
                AvaliblePriceTo = Math.Ceiling(prices.Value / CurrencyService.CurrentCurrency.Value),
            });
            Response.Write(res);
            Response.End();
            return;
        }

        filterPrice.Min = prices.Key / CurrencyService.CurrentCurrency.Value;
        filterPrice.Max = prices.Value / CurrencyService.CurrentCurrency.Value;

        vProducts.DataSource = defaultDS.Items;
        vProducts.ViewMode = (UserControls_ProductView.ProductViewMode)productViewChanger.SearchViewMode;
        vProducts.DataBind();
        paging.TotalPages = pageCount;

        int itemsCount = totalCount;
        lItemsCount.Text = string.Format("{0} {1}", itemsCount.ToString(), Strings.Numerals(itemsCount, Resource.Client_Searsh_NoResults,
                                                                                            Resource.Client_Searsh_1Result,
                                                                                            Resource.Client_Searsh_2Results,
                                                                                            Resource.Client_Searsh_5Results));
        pnlSort.Visible = itemsCount > 0;

        
    }
}