<%@ Page Language="C#" MasterPageFile="MasterPage.master" EnableEventValidation="false"
    EnableViewState="false" AutoEventWireup="true" CodeFile="Catalog.aspx.cs" Inherits="Catalog_Page" %>

<%@ Import Namespace="AdvantShop.Configuration" %>
<%@ Import Namespace="AdvantShop.FilePath" %>
<%@ Import Namespace="AdvantShop.Orders" %>
<%@ Register Src="~/UserControls/Catalog/ProductView.ascx" TagName="ProductView"
    TagPrefix="adv" %>
<%@ Register Src="~/UserControls/Catalog/ProductViewChanger.ascx" TagName="ProductViewChanger"
    TagPrefix="adv" %>
<%@ Register Src="~/UserControls/Catalog/CategoryView.ascx" TagName="CategoryView"
    TagPrefix="adv" %>
<%@ Register Src="~/UserControls/Catalog/CatalogView.ascx" TagName="CatalogView"
    TagPrefix="adv" %>
<%@ Register Src="~/UserControls/Catalog/FilterProperty.ascx" TagName="FilterProperty"
    TagPrefix="adv" %>
<%@ Register Src="~/UserControls/Catalog/FilterBrand.ascx" TagName="FilterBrand"
    TagPrefix="adv" %>
<%@ Register Src="~/UserControls/Catalog/FilterPrice.ascx" TagName="FilterPrice"
    TagPrefix="adv" %>
<%@ Register Src="~/UserControls/Catalog/RecentlyView.ascx" TagName="RecentlyView"
    TagPrefix="adv" %>
<%@ Register Src="~/UserControls/BreadCrumbs.ascx" TagName="BreadCrumbs" TagPrefix="adv" %>
<%@ MasterType VirtualPath="MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <adv:SqlPagingDataSource ID="defaultDS" runat="server" CurrentPageIndex="1">
        <Field Name="[Product].[ProductID]" IsDistinct="true" />
        <Field Name="PhotoName AS Photo" />
        <Field Name="[Photo].[Description] AS PhotoDesc" />
        <Field Name="[ProductCategories].[CategoryID]" NotInQuery="true" />
        <Field Name="BriefDescription" />
        <Field Name="ArtNo" />
        <Field Name="Name" />
        <Field Name="Price" />
        <Field Name="Price - Price * discount/100 as discountPrice" NotInQuery="true" />
        <Field Name="(CASE Price WHEN 0 THEN 0 ELSE 1 END) as tempSort" Sorting="Descending" />
        <Field Name="Recomended" />
        <Field Name="New" />
        <Field Name="Bestseller" />
        <Field Name="OnSale" />
        <Field Name="Discount" />
        <Field Name="Offer.Amount" />
        <Field Name="Offer.MinAmount" />
        <Field Name="Offer.MaxAmount" />
        <Field Name="Enabled" />
        <Field Name="OrderByRequest" />
        <Field Name="Ratio" />
        <Field Name="RatioID" />
        <Field Name="DateModified" />
        <Field Name="ItemId" />
        <Field Name="UrlPath" />
        <Field Name="[ProductCategories].[SortOrder]" />
        <Field Name="[ShoppingCart].[CustomerID]" NotInQuery="true" />
        <Field Name="OfferListID" NotInQuery="true" />
        <Field Name="BrandID" NotInQuery="true" />
        <Field Name="HirecalEnabled" NotInQuery="True"></Field>
    </adv:SqlPagingDataSource>
    <div class="stroke">
        <div class="col-left col-left-padding expander-enable">
            <adv:CatalogView runat="server" ID="catalogView" />
            <%if (SettingsDesign.FilterVisibility && (filterBrand.Visible || filterPrice.Visible || filterProperty.Visible) && ProductsCount > 1)
              {%>
            <article class="block-uc" data-plugin="expander" id="filter">
                <h3 class="title" data-expander-control="#filter-content">
                    <%= Resources.Resource.Client_Catalog_Filter %></h3>
                <div class="content" id="filter-content">
                    <adv:FilterPrice runat="server" ID="filterPrice" />
                    <adv:FilterBrand runat="server" ID="filterBrand" />
                    <adv:FilterProperty runat="server" ID="filterProperty" />
                    <div class="aplly-price">
                        <adv:Button ID="Button1" runat="server" Size="Small" Type="Action" Text="<%$ Resources:Resource, Client_Catalog_ResetFilter%>"
                            OnClientClick="ClearFilter();"></adv:Button>
                       <adv:Button runat="server" Size="Small" Type="Confirm" Text="<%$ Resources:Resource, Client_Catalog_FilterApply %>"
                            OnClientClick="ApplyFilter(null, true, false);"></adv:Button>
                    </div>
                </div>
            </article>
            <% } %>
            <adv:RecentlyView ID="RecentlyView1" runat="server" />
        </div>
        <div class="col-right">
            <% if (category.Picture != null)
               {%>
            <div class="c-banner">
                <img src="<%= FoldersHelper.GetImageCategoryPath(CategoryImageType.Big , category.Picture.PhotoName , false) %>"
                    alt="<%= HttpUtility.HtmlEncode(category.Name) %>" />
            </div>
            <% } %>
            <h1>
                <asp:Literal ID="lblCategoryName" runat="server" /></h1>
            <adv:BreadCrumbs ID="breadCrumbs" runat="server" />
            <%if (!string.IsNullOrEmpty(category.BriefDescription))
              {%>
            <div class="c-description">
                <%= category.BriefDescription %>
            </div>
            <% } %>
            <adv:CategoryView ID="categoryView" runat="server" />
            <% if (productView.HasProducts)
               {%>
            <div class="str-sort" runat="server" id="pnlSort">
                <div class="count-search">
                    <asp:Literal runat="server" ID="lTotalItems" /></div>
                <adv:ProductViewChanger runat="server" ID="productViewChanger" CurrentPage="Catalog" />
                <% if (SettingsCatalog.EnableCompareProducts)
                   { %>
                <div class="cp">
                    <a <%= ShoppingCartService.CurrentCompare.Any() != true ? "class=\"compare-disabled\"" : ""  %> href="compareproducts.aspx" target="_blank" id="compareBasket">
                        <%= Resources.Resource.Client_Catalog_CompareCart %>
                        (<span id="compareCount"><%= ShoppingCartService.CurrentCompare.Count() %></span>)</a>
                </div>
                <% } %>
                <div class="sort-variant">
                    <%=Resources.Resource.Client_Catalog_SortBy%>
                    <asp:DropDownList ID="ddlSort" runat="server" onChange="ApplyFilter(null, true, false);" />
                </div>
                <div class="clear">
                </div>
            </div>
            <% } %>
            <adv:ProductView ID="productView" runat="server" />
            <adv:AdvPaging runat="server" ID="paging" DisplayShowAll="True" />
            <%if (!string.IsNullOrEmpty(category.Description))
              {%>
            <div class="c-briefdescription">
                <%= category.Description%>
            </div>
            <% } %>
            
        </div>
        <br class="clear" />
    </div>
</asp:Content>
