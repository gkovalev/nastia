//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using AdvantShop;
using AdvantShop.CMS;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Controls;
using AdvantShop.Core;
using AdvantShop.Core.FieldFilters;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Customers;
using AdvantShop.Repository.Currencies;
using Newtonsoft.Json;
using Resources;

public partial class Catalog_Page : AdvantShopPage
{
    private const bool ExluderingFilters = true;

    private int _categoryId;
    protected Category category;
    protected int ProductsCount;
    protected bool indepth;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Request["categoryid"]) || !Int32.TryParse(Request["categoryid"], out _categoryId))
        {
            Error404();
        }

        //Changed by Evgeni for Igor(bosch-opt.by) ONLY!! Zapchast redirect to Novost
        if (_categoryId == 10020)
        {
            Response.Redirect(@"https://instrument-opt.by/news/service_and_spare_parts_bosch");
        }

        category = CategoryService.GetCategory(_categoryId);
        if (category == null || category.Enabled == false || category.HirecalEnabled == false)
        {
            Error404();
            return;
        }
        indepth = Request["indepth"] == "1";

        ProductsCount = indepth ? category.TotalProductsCount : category.GetProductCount();

        categoryView.CategoryID = _categoryId;
        categoryView.Visible = category.DisplayStyle == "True" || ProductsCount == 0;
        pnlSort.Visible = ProductsCount > 0;
        productView.Visible = ProductsCount > 0;
        catalogView.CategoryID = _categoryId;

        filterProperty.CategoryId = _categoryId;

        filterBrand.CategoryId = _categoryId;
        filterBrand.InDepth = category.DisplayChildProducts || indepth;

        filterPrice.CategoryId = _categoryId;
        filterPrice.InDepth = category.DisplayChildProducts || indepth;

        lblCategoryName.Text = _categoryId != 0 ? category.Name : Resource.Client_MasterPage_Catalog;

        breadCrumbs.Items =
            CategoryService.GetParentCategories(_categoryId).Select(parent => new BreadCrumbs
            {
                Name = parent.Name,
                Url = UrlService.GetLink(ParamType.Category, parent.UrlPath, parent.CategoryId)
            }).Reverse().ToList();
        breadCrumbs.Items.Insert(0, new BreadCrumbs
        {
            Name = Resource.Client_MasterPage_MainPage,
            Url = UrlService.GetAbsoluteLink("/")
        });

        SetMeta(category.Meta, category.Name);

        //перенесено из верстки из-за CustomerSession.CustomerId (Sckeef)

        defaultDS.TableName =
            "[Catalog].[Product] LEFT JOIN [Catalog].[Offer] ON [Product].[ProductID] = [Offer].[ProductID] LEFT JOIN [Catalog].[Photo] ON [Product].[ProductID] = [Photo].[ObjId] and Type=@Type AND [Main] = 1 inner join Catalog.ProductCategories on ProductCategories.ProductId = [Product].[ProductID] Left JOIN [Catalog].[ProductPropertyValue] ON [Product].[ProductID] = [ProductPropertyValue].[ProductID] LEFT JOIN [Catalog].[ShoppingCart] ON [Catalog].[ShoppingCart].[EntityID] = [Catalog].[Product].[ProductID] AND [Catalog].[ShoppingCart].[ShoppingCartTypeId] = 3 AND [ShoppingCart].[CustomerID] = @CustomerId Left JOIN [Catalog].[Ratio] on Product.ProductId= Ratio.ProductID and Ratio.CustomerId=@CustomerId";
        defaultDS.AddParamSql(new SqlParam { ParamName = "@CustomerId", ParamValue = CustomerSession.CustomerId.ToString() });
        defaultDS.AddParamSql(new SqlParam { ParamName = "@Type", ParamValue = PhotoType.Product.ToString() });
        defaultDS.AddCondition("[Settings].[CountCategoriesByProduct]( Product.ProductID) > 0");

        var olFilter = new EqualFieldFilter { ParamName = "@OfferListID", Value = CatalogService.CurrentOfferListId.ToString() };
        defaultDS.Fields["OfferListID"].Filter = olFilter;

        if (category.DisplayChildProducts || indepth)
        {
            var cfilter = new InChildCategoriesFieldFilter
            {
                CategoryId = _categoryId.ToString(),
                ParamName = "@CategoryID"
            };
            defaultDS.Fields["[ProductCategories].[CategoryID]"].Filter = cfilter;
        }
        else
        {
            var cfilter = new EqualFieldFilter { Value = _categoryId.ToString(), ParamName = "@catalog" };
            defaultDS.Fields["[ProductCategories].[CategoryID]"].Filter = cfilter;
        }

        var efilter = new EqualFieldFilter { Value = "1", ParamName = "@enabled" };
        defaultDS.Fields["Enabled"].Filter = efilter;
        defaultDS.Fields["HirecalEnabled"].Filter = new EqualFieldFilter { Value = "1", ParamName = "@HirecalEnabled" };

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
            if (!SettingsCatalog.EnableProductRating && (enumItem == SortOrder.DescByRatio || enumItem == SortOrder.AscByRatio))
            {
                continue;
            }

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
                defaultDS.Fields["[ProductCategories].[SortOrder]"].Sorting = SortDirection.Ascending;
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

        if (!string.IsNullOrEmpty(Request["prop"]))
        {
            var valIDs = new Dictionary<int, List<int>>();
            var selectedPropertyIDs = new List<int>();
            var filterCollection = Request["prop"].Split('-');
            int key = 1;
            foreach (var val in filterCollection)
            {
                var tempListIds = new List<int>();
                var tempIds = val.Split(',');
                foreach (var item in tempIds)
                {
                    int id = 0;
                    if (int.TryParse(item, out id))
                    {
                        tempListIds.Add(id);
                        selectedPropertyIDs.Add(id);
                    }
                }
                valIDs.Add(key, tempListIds);
                key++;
            }
            //int id = 0;
            //var valIDs = (from val in Request["prop"].Split(',') where int.TryParse(val, out id) select id).ToList();
            if (valIDs.Count != 0)
            {
                var category = CategoryService.GetCategory(_categoryId);
                var lfilter = new PropertyFieldFilter { ListFilter = valIDs, ParamName = "@prop", CategoryId = category.CategoryId, GetSubCategoryes = category.DisplayChildProducts };
                defaultDS.Fields["[Product].[ProductID]"].Filter = lfilter;
            }
            filterProperty.SelectedPropertyIDs = selectedPropertyIDs;
        }
        else
        {
            filterProperty.SelectedPropertyIDs = new List<int>();
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
        defaultDS.ItemsPerPage = paging.CurrentPage != 0 ? SettingsCatalog.ProductsPerPage : int.MaxValue;
        defaultDS.CurrentPageIndex = paging.CurrentPage != 0 ? paging.CurrentPage : 1;

        var totalCount = defaultDS.TotalCount;

        productView.ViewMode = (UserControls_ProductView.ProductViewMode)productViewChanger.CatalogViewMode;
        lTotalItems.Text = string.Format(Resource.Client_Catalog_ItemsFound, totalCount);

        paging.TotalPages = (int)(Math.Ceiling((double)totalCount / defaultDS.ItemsPerPage));

        if ((paging.TotalPages < paging.CurrentPage && paging.CurrentPage > 1) || paging.CurrentPage < 0)
        {
            Error404();
            return;
        }

        // if we get request from ajax filter
        if (Request["ajax"] == "1")
        {
            Response.Clear();
            Response.ContentType = "application/json";

            var prices = defaultDS.AvaliblePrices;
            var res = JsonConvert.SerializeObject(new
            {
                ProductsCount = totalCount,
                AvalibleProperties = filterProperty.AvaliblePropertyIDs,
                AvalibleBrands = filterBrand.AvalibleBrandIDs,
                AvaliblePriceFrom = Math.Floor(prices.Key / CurrencyService.CurrentCurrency.Value),
                AvaliblePriceTo = Math.Ceiling(prices.Value / CurrencyService.CurrentCurrency.Value),
            });
            Response.Write(res);
            Response.End();
            return;
        }



        productView.DataSource = defaultDS.Items;
        productView.DataBind();

        filterProperty.AvaliblePropertyIDs = ExluderingFilters ? defaultDS.PropertyValuesList : null;
        filterBrand.AvalibleBrandIDs = ExluderingFilters ? defaultDS.BrandList : null;
    }
}