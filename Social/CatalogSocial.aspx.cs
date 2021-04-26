//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
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
using Resources;

public partial class CatalogSocial : AdvantShopPage
{
    private int _categoryId;
    protected Category category;
    protected int ProductsCount;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Request["categoryid"]) || !Int32.TryParse(Request["categoryid"], out _categoryId))
        {
            _categoryId = 0;
        }

        category = CategoryService.GetCategory(_categoryId);
        if (category == null || category.Enabled == false || category.HirecalEnabled == false)
        {
            Error404();
            return;
        }

        ProductsCount = category.GetProductCount();

        categoryView.CategoryID = _categoryId;
        categoryView.Visible = true;
        pnlSort.Visible = ProductsCount > 0;
        productView.Visible = ProductsCount > 0;

        lblCategoryName.Text = _categoryId != 0 ? category.Name : Resource.Client_MasterPage_Catalog;
        //lblCategoryDescription.Text = category.Description;

        //imgCategoryImage.ImageUrl = string.IsNullOrEmpty(category.Picture) ? "" : string.Format("{0}", ImageFolders.GetImageCategoryPath(false, category.Picture));

        breadCrumbs.Items =
            CategoryService.GetParentCategories(_categoryId).Select(parent => new BreadCrumbs
                                                                                  {
                                                                                      Name = parent.Name,
                                                                                      Url = "social/catalogsocial.aspx?categoryid=" + parent.CategoryId
                                                                                  }).Reverse().ToList();
        breadCrumbs.Items.Insert(0, new BreadCrumbs
                                        {
                                            Name = Resource.Client_MasterPage_MainPage,
                                            Url = UrlService.GetAbsoluteLink("social/catalogsocial.aspx")
                                        });

        SetMeta(category.Meta, category.Name);

        //перенесено из верстки из-за CustomerSession.CustomerId (Sckeef)

        defaultDS.TableName =
            "[Catalog].[Product] LEFT JOIN [Catalog].[Offer] ON [Product].[ProductID] = [Offer].[ProductID] LEFT JOIN [Catalog].[Photo] ON [Product].[ProductID] = [Photo].[ObjID] and Type='Product' AND [Main] = 1 inner join Catalog.ProductCategories on ProductCategories.ProductId = [Product].[ProductID] Left JOIN [Catalog].[ProductPropertyValue] ON [Product].[ProductID] = [ProductPropertyValue].[ProductID] LEFT JOIN [Catalog].[ShoppingCart] ON [Catalog].[ShoppingCart].[EntityID] = [Catalog].[Product].[ProductID] AND [Catalog].[ShoppingCart].[ShoppingCartTypeId] = 3 AND [ShoppingCart].[CustomerID] = @CustomerId Left JOIN [Catalog].[Ratio] on Product.ProductId= Ratio.ProductID and Ratio.CustomerId=@CustomerId";
        defaultDS.AddParamSql(new SqlParam { ParamName = "@CustomerId", ParamValue = CustomerSession.CustomerId.ToString() });
        defaultDS.AddCondition("[Settings].[CountCategoriesByProduct]( Product.ProductID) > 0");

        var olFilter = new EqualFieldFilter { ParamName = "@OfferListID", Value = CatalogService.CurrentOfferListId.ToString() };
        defaultDS.Fields["OfferListID"].Filter = olFilter;

        if (category.DisplayChildProducts)
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
                defaultDS.Fields["[ProductCategories].[SortOrder]"].Sorting = SortDirection.Ascending;
                break;
        }
    }


    protected void Page_PreRender(object sender, EventArgs e)
    {
        defaultDS.ItemsPerPage = paging.CurrentPage != 0 ? SettingsCatalog.ProductsPerPage : int.MaxValue;
        defaultDS.CurrentPageIndex = paging.CurrentPage != 0 ? paging.CurrentPage : 1;

        productView.ViewMode = (UserControls_ProductViewSocial.ProductViewMode)productViewChanger.CatalogViewMode;
        lTotalItems.Text = string.Format(Resource.Client_Catalog_ItemsFound, defaultDS.TotalCount);

        paging.TotalPages = defaultDS.PageCount;
        productView.DataSource = defaultDS.Items;
        productView.DataBind();
    }
}