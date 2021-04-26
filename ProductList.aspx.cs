//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using AdvantShop;
using AdvantShop.Catalog;
using AdvantShop.CMS;
using AdvantShop.Configuration;
using AdvantShop.Controls;
using AdvantShop.Core;
using AdvantShop.Core.FieldFilters;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Customers;
using AdvantShop.Repository.Currencies;
using Newtonsoft.Json;
using Resources;

public partial class ProductList_Page : AdvantShopPage
{
    protected ProductOnMain.TypeFlag _typeFlag = ProductOnMain.TypeFlag.None;
    protected string PageName;
    protected int ProductsCount;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Request["type"]) || !Enum.TryParse(Request["type"], true, out _typeFlag) || _typeFlag == ProductOnMain.TypeFlag.None)
        {
            Error404();
        }

        ProductsCount = ProductOnMain.GetProductCountByType(_typeFlag);
        pnlSort.Visible = ProductsCount > 0;
        productView.Visible = ProductsCount > 0;

        filterBrand.CategoryId = 0;
        filterBrand.InDepth = true;
        filterBrand.WorkType = _typeFlag;

        filterPrice.CategoryId = 0;
        filterPrice.InDepth = true;
        //changed by Evgeni for multiple price
        defaultDS.TableName = "[Catalog].[Product] LEFT JOIN [Catalog].[Offer] ON [Product].[ProductID] = [Offer].[ProductID]  AND [OfferListID] = 6 LEFT JOIN [Catalog].[Photo] ON [Product].[ProductID] = [Photo].[ObjId]  And Type='Product' AND [Main] = 1  LEFT JOIN [Catalog].[ShoppingCart] ON [Catalog].[ShoppingCart].[EntityID] = [Catalog].[Product].[ProductID] AND [Catalog].[ShoppingCart].[ShoppingCartTypeId] = 3 AND [ShoppingCart].[CustomerID] = @CustomerId  Left JOIN [Catalog].[Ratio] on Product.ProductId= Ratio.ProductID and Ratio.CustomerId=@CustomerId";
        defaultDS.AddParamSql(new SqlParam { ParamName = "@CustomerId", ParamValue = CustomerSession.CustomerId.ToString() });
        defaultDS.AddCondition("[Settings].[CountCategoriesByProduct](Product.ProductID) > 0");

        defaultDS.Fields["HirecalEnabled"].Filter = new EqualFieldFilter { Value = "1", ParamName = "@HirecalEnabled" };
        defaultDS.Fields["Enabled"].Filter = new EqualFieldFilter { Value = "1", ParamName = "@Enabled" };

        if (_typeFlag == ProductOnMain.TypeFlag.Bestseller)
        {
            var filterB = new EqualFieldFilter { ParamName = "@Bestseller", Value = "1" };
            defaultDS.Fields["Bestseller"].Filter = filterB;
            defaultDS.Fields.Add(new KeyValuePair<string, Field>("SortBestseller", new Field { Name = "SortBestseller as Sort" }));
            PageName = Resource.Client_ProductList_AllBestSellers;

            SetMeta(new AdvantShop.SEO.MetaInfo(SettingsMain.ShopName + " - " + Resource.Client_Bestsellers_Header), string.Empty);
        }
        if (_typeFlag == ProductOnMain.TypeFlag.New)
        {
            var filterN = new EqualFieldFilter { ParamName = "@New", Value = "1" };
            defaultDS.Fields["New"].Filter = filterN;
            defaultDS.Fields.Add(new KeyValuePair<string, Field>("SortNew", new Field { Name = "SortNew as Sort" }));
            PageName = Resource.Client_ProductList_AllNew;

            SetMeta(new AdvantShop.SEO.MetaInfo(SettingsMain.ShopName + " - " + Resource.Client_New_Header), string.Empty);
        }
        if (_typeFlag == ProductOnMain.TypeFlag.Discount)
        {
            var filterN = new NotEqualFieldFilter() { ParamName = "@Discount", Value = "0" };
            defaultDS.Fields["Discount"].Filter = filterN;
            defaultDS.Fields.Add(new KeyValuePair<string, Field>("SortDiscount", new Field { Name = "SortDiscount as Sort" }));
            PageName = Resource.Client_ProductList_AllDiscount;

            SetMeta(new AdvantShop.SEO.MetaInfo(SettingsMain.ShopName + " - " + Resource.Client_Discount_Header), string.Empty);
        }

        breadCrumbs.Items.AddRange(new BreadCrumbs[]{ new BreadCrumbs(){Name = Resource.Client_MasterPage_MainPage, Url = UrlService.GetAbsoluteLink("/")},
                                                        new BreadCrumbs() {Name = PageName, Url = null }});

        BuildSorting();
        BuildFilter();
    }

    private void BuildSorting()
    {
        var sort = SortOrder.NoSorting;
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
                defaultDS.Fields["Ratio"].Sorting = SortDirection.Ascending;
                break;

            case SortOrder.DescByRatio:
                defaultDS.Fields["Ratio"].Sorting = SortDirection.Descending;
                break;

            default:
                switch (_typeFlag)
                {
                    case ProductOnMain.TypeFlag.Bestseller:
                        defaultDS.Fields["SortBestseller"].Sorting = SortDirection.Ascending;
                        break;
                    case ProductOnMain.TypeFlag.New:
                        defaultDS.Fields["SortNew"].Sorting = SortDirection.Ascending;
                        break;
                    case ProductOnMain.TypeFlag.Discount:
                        defaultDS.Fields["SortDiscount"].Sorting = SortDirection.Ascending;
                        break;
                }
                break;
        }
    }

    private void BuildFilter()
    {
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

            defaultDS.Fields["Price"].Filter = new NotEqualFieldFilter { ParamName = "@price", Value = "0" };
        }
        else
        {
            filterPrice.CurValMin = 0;
            filterPrice.CurValMax = int.MaxValue;
        }

        if (!string.IsNullOrEmpty(Request["brand"]))
        {
            int id = 0;
            var brandIds = (from b in Request["brand"].Split(',') where int.TryParse(b, out id) select id).ToList();
            filterBrand.SelectedBrandIDs = brandIds;
            var isf = new InSetFieldFilter
            {
                IncludeValues = true,
                ParamName = "@brands",
                Values = brandIds.Select(brandid => brandid.ToString()).ToArray()
            };
            defaultDS.Fields["BrandID"].Filter = isf;
            filterBrand.SelectedBrandIDs = brandIds;
        }
        else
        {
            filterBrand.SelectedBrandIDs = new List<int>();
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        defaultDS.CurrentPageIndex = paging.CurrentPage != 0 ? paging.CurrentPage : 1;
        defaultDS.ItemsPerPage = paging.CurrentPage != 0 ? SettingsCatalog.ProductsPerPage : int.MaxValue;
        var pageCount = defaultDS.PageCount;
        var totalCount = defaultDS.TotalCount;
        paging.TotalPages = (int)(Math.Ceiling((double)totalCount / defaultDS.ItemsPerPage));

        if (totalCount != 0 && paging.TotalPages < paging.CurrentPage || paging.CurrentPage < 0)
        {
            Error404();
            return;
        }

        var prices = defaultDS.AvaliblePrices;

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

        productView.DataSource = defaultDS.Items;
        productView.ViewMode = (UserControls_ProductView.ProductViewMode)productViewChanger.CatalogViewMode;
        productView.DataBind();
        paging.TotalPages = pageCount;


        filterBrand.AvalibleBrandIDs = defaultDS.BrandList;
    }
}