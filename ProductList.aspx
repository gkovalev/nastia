<%@ Page Language="C#" MasterPageFile="MasterPage.master" CodeFile="ProductList.aspx.cs"
    Inherits="ProductList_Page" %>

<%@ Import Namespace="AdvantShop.Catalog" %>
<%@ Import Namespace="AdvantShop.Orders" %>
<%@ Register TagPrefix="adv" TagName="ProductView" Src="UserControls/Catalog/ProductView.ascx" %>
<%@ Register TagPrefix="adv" TagName="FilterPrice" Src="~/UserControls/Catalog/FilterPrice.ascx" %>
<%@ Register TagPrefix="adv" TagName="FilterBrand" Src="~/UserControls/Catalog/FilterBrand.ascx" %>
<%@ Register TagPrefix="adv" TagName="RecentlyView" Src="~/UserControls/Catalog/RecentlyView.ascx" %>
<%@ Register TagPrefix="adv" TagName="BreadCrumbs" Src="~/UserControls/BreadCrumbs.ascx" %>
<%@ Register TagPrefix="adv" TagName="ProductViewChanger" Src="~/UserControls/Catalog/ProductViewChanger.ascx" %>
<%@ MasterType VirtualPath="MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <adv:SqlPagingDataSource ID="defaultDS" runat="server" CurrentPageIndex="1" TableName="[Catalog].[Product] LEFT JOIN [Catalog].[Offer] ON [Product].[ProductID] = [Offer].[ProductID] LEFT JOIN [Catalog].[Photo] ON [Product].[ProductID] = [Photo].[ObjId] And Type='Product' AND [Main] = 1 Left JOIN [Catalog].[ProductPropertyValue] ON [Product].[ProductID] = [ProductPropertyValue].[ProductID]">
        <Field Name="[Product].[ProductID] as ProductID" IsDistinct="true" />
        <Field Name="PhotoName AS Photo" />
        <Field Name="[Photo].[Description] AS PhotoDesc"></Field>
        <Field Name="BriefDescription" />
        <Field Name="ArtNo" />
        <Field Name="Name" />
        <Field Name="Price" />
        <Field Name="Price - Price*discount/100 as discountPrice" NotInQuery="true" />
        <Field Name="Recomended" />
        <Field Name="New" />
        <Field Name="Bestseller" />
        <Field Name="OnSale" />
        <Field Name="Discount" />
        <Field Name="UrlPath" />
        <Field Name="[Offer].Amount" />
        <Field Name="[Offer].MinAmount" />
        <Field Name="Enabled" />
        <Field Name="ItemId" />
        <Field Name="OrderByRequest" />
        <Field Name="Ratio" />
        <Field Name="RatioID" />
        <Field Name="DateModified" />
        <Field Name="[ShoppingCart].[CustomerID]" NotInQuery="true" />
        <Field Name="OfferListID" NotInQuery="true" />
        <Field Name="BrandID" NotInQuery="true" />
        <Field Name="HirecalEnabled" NotInQuery="True"></Field>
    </adv:SqlPagingDataSource>
    <div class="stroke">
        <div class="col-left">
            <div class="block-uc expander">
                <span class="title expand ex-control">
                    <%= Resources.Resource.Client_ProductList_Sections%><span class="control"></span></span>
                <div class="content list-link-marker ex-content">
                    <a href="productlist.aspx?type=bestseller">
                        <%= _typeFlag== ProductOnMain.TypeFlag.Bestseller ? "<span class='bold'>" +  Resources.Resource.Client_ProductList_AllBestSellers + "</span>" : Resources.Resource.Client_ProductList_AllBestSellers %></a>
                    <a href="productlist.aspx?type=new">
                        <%= _typeFlag== ProductOnMain.TypeFlag.New ? "<span class='bold'>" +  Resources.Resource.Client_ProductList_AllNew + "</span>" : Resources.Resource.Client_ProductList_AllNew %></a>
                    <a href="productlist.aspx?type=discount">
                        <%= _typeFlag == ProductOnMain.TypeFlag.Discount ? "<span class='bold'>" + Resources.Resource.Client_ProductList_AllDiscount + "</span>" : Resources.Resource.Client_ProductList_AllDiscount%></a>
                </div>
            </div>
            <%if ((filterBrand.Visible || filterPrice.Visible) && ProductsCount > 1)
              {%>
            <div class="expander block-uc" id="filter">
                <span class="title expand ex-control"><span class="control"></span>
                    <%= Resources.Resource.Client_Catalog_Filter %>
                </span>
                <div class="content expand ex-content">
                    <adv:FilterPrice runat="server" ID="filterPrice" />
                    <adv:FilterBrand runat="server" ID="filterBrand" />
                    <div class="aplly-price">
                        <adv:Button ID="Button1" runat="server" Size="Small" Type="Action" Text="<%$ Resources:Resource, Client_Catalog_ResetFilter%>"
                            OnClientClick="ClearFilter();"></adv:Button>
                        <adv:Button ID="Button2" runat="server" Size="Small" Type="Confirm" Text="<%$ Resources:Resource, Client_Catalog_FilterApply %>"
                            OnClientClick="ApplyFilter();"></adv:Button>
                    </div>
                </div>
            </div>
            <% } %>
            <adv:RecentlyView ID="RecentlyView1" runat="server" />
        </div>
        <div class="col-right">
            <h1>
                <%= PageName %></h1>
            <adv:BreadCrumbs ID="breadCrumbs" runat="server" />
            <% if (productView.HasProducts)
               {%>
            <div class="str-sort" runat="server" id="pnlSort">
                <div class="count-search">
                    <asp:Literal runat="server" ID="lTotalItems" /></div>
                <adv:ProductViewChanger runat="server" ID="productViewChanger" CurrentPage="Catalog" />
                <div class="cp">
                    <a href="compareproducts.aspx" target="_blank" id="compare">
                        <%=Resources.Resource.Client_Catalog_CompareCart%>
                        (<span id="compare-count"><%=ShoppingCartService.CurrentCompare.Count() %></span>)</a>
                </div>
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
    </div>
</asp:Content>
