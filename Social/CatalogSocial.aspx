<%@ Page Language="C#" MasterPageFile="MasterPageSocial.master" EnableEventValidation="false"
    EnableViewState="false" AutoEventWireup="true" CodeFile="CatalogSocial.aspx.cs"
    Inherits="CatalogSocial" %>

<%@ Import Namespace="AdvantShop.Configuration" %>
<%@ Import Namespace="AdvantShop.FilePath" %>
<%@ Import Namespace="AdvantShop.Orders" %>
<%@ Register Src="~/Social/UserControls/ProductViewSocial.ascx" TagName="ProductView"
    TagPrefix="adv" %>
<%@ Register Src="~/UserControls/Catalog/ProductViewChanger.ascx" TagName="ProductViewChanger"
    TagPrefix="adv" %>
<%@ Register Src="~/Social/UserControls/CategoryViewSocial.ascx" TagName="CategoryViewSocial"
    TagPrefix="adv" %>
<%@ Register Src="~/UserControls/BreadCrumbs.ascx" TagName="BreadCrumbs" TagPrefix="adv" %>
<%@ MasterType VirtualPath="MasterPageSocial.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <adv:SqlPagingDataSource ID="defaultDS" runat="server" CurrentPageIndex="1">
        <Field Name="[Product].[ProductID]" IsDistinct="true" />
        <Field Name="PhotoName AS Photo" />
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
        <% if (category.Picture != null)
           {%>
        <div class="c-banner">
            <img src="<%= FoldersHelper.GetImageCategoryPath(CategoryImageType.Big , category.Picture.PhotoName, false ) %>"
                alt="<%= category.Name %>" />
        </div>
        <% } %>
        <h1>
            <asp:Literal ID="lblCategoryName" runat="server" /></h1>
        <adv:BreadCrumbs ID="breadCrumbs" runat="server" />
        <%if (!string.IsNullOrEmpty(category.Description))
          {%>
        <div class="c-description">
            <%= category.Description %>
        </div>
        <% } %>
        <adv:CategoryViewSocial ID="categoryView" runat="server" />
        <% if (productView.HasProducts)
           {%>
        <div class="str-sort" runat="server" id="pnlSort">
            <div class="count-search">
                <asp:Literal runat="server" ID="lTotalItems" /></div>
            <adv:ProductViewChanger runat="server" ID="productViewChanger" CurrentPage="Catalog" />
            <div class="sort-variant">
                <%=Resources.Resource.Client_Catalog_SortBy%>
                <asp:DropDownList ID="ddlSort" runat="server" onChange="ApplyFilter();" />
            </div>
            <br class="clear" />
        </div>
        <% } %>
        <adv:ProductView ID="productView" runat="server" />
        <adv:AdvPaging runat="server" ID="paging" DisplayShowAll="True" />
    </div>
    <br class="clear" />
</asp:Content>
