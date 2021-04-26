<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Search.aspx.cs" Inherits="Search" %>

<%@ Import Namespace="AdvantShop.Configuration" %>
<%@ Import Namespace="AdvantShop.Orders" %>
<%@ Import Namespace="Resources" %>
<%@ Register Src="UserControls/Catalog/ProductView.ascx" TagName="ProductView" TagPrefix="adv" %>
<%@ Register TagPrefix="adv" TagName="FilterPrice" Src="~/UserControls/Catalog/FilterPrice.ascx" %>
<%@ Register TagPrefix="adv" TagName="RecentlyView" Src="~/UserControls/Catalog/RecentlyView.ascx" %>
<%@ Register TagPrefix="adv" TagName="ProductViewChanger" Src="~/UserControls/Catalog/ProductViewChanger.ascx" %>
<%@ MasterType VirtualPath="MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <adv:SqlPagingDataSource ID="defaultDS" runat="server" CurrentPageIndex="1">
        <Field Name="[Product].[ProductID]" IsDistinct="true" />
        <Field Name="PhotoName AS Photo" />
        <Field Name="[Photo].[Description] AS PhotoDesc"></Field>
        <Field Name="[ProductCategories].[CategoryID]" NotInQuery="true" />
        <Field Name="BriefDescription" />
        <Field Name="ArtNo" />
        <Field Name="Name" />
        <Field Name="Price" />
        <Field Name="Price - Price * discount/100 as discountPrice" NotInQuery="true" />
        <Field Name="Recomended" />
        <Field Name="New" />
        <Field Name="Bestseller" />
        <Field Name="OnSale" />
        <Field Name="Discount" />
        <Field Name="Offer.Amount" />
        <Field Name="Offer.MinAmount" />
        <Field Name="Offer.MaxAmount" />
        <Field Name="UrlPath" />
        <Field Name="Enabled" />
        <Field Name="HirecalEnabled" />
        <Field Name="OrderByRequest" />
        <Field Name="Ratio" />
        <Field Name="RatioID" />
        <Field Name="ItemId" />
        <Field Name="DateModified" />
        <Field Name="OfferListID" NotInQuery="true" />
    </adv:SqlPagingDataSource>
    <div class="stroke">
        <div class="col-left">
            <article class="block-uc bloсk-search" id="filter">
                <h3 class="title">
                    <%=Resource.Client_Search_SearchParams %></h3>
                <div class="content">
                    <div class="param-name">
                        <%=Resource.Client_Search_Find %>:</div>
                    <div class="param-value">
                        <adv:AdvTextBox runat="server" ID="txtName" DefaultButtonID="btnFind" CssClass="autocompleteSearch" />
                    </div>
                    <div class="param-name">
                        <%=Resource.Client_Search_InCategories %>:
                    </div>
                    <div class="param-value">
                        <asp:DropDownList ID="ddlCategory" runat="server">
                            <asp:ListItem Text="<%$ Resources:Resource, Client_Search_AllCategories %>" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <adv:FilterPrice runat="server" ID="filterPrice" />
                    <div class="uc-search">
                        <adv:Button runat="server" ID="btnFind" Type="Submit" Size="Middle" OnClientClick="ApplySearch();"
                            Text="<%$ Resources:Resource, Client_Search_Find %>" />
                    </div>
                    <div class="declare">
                    </div>
                </div>
            </article>
            <adv:RecentlyView runat="server" />
        </div>
        <div class="col-right">
            <%--<% if (!String.IsNullOrWhiteSpace(SearchTerm))
               {%>--%>
            <div class="page-name">
                <%= Resource.Client_Search_Found%>
                <strong>
                    <asp:Literal runat="server" ID="lItemsCount"></asp:Literal></strong>
                <% if (!String.IsNullOrWhiteSpace(SearchTerm))
                   {%>
                <%= Resource.Client_Search_OnQuery%>
                &quot;<%= SearchTerm%>&quot;
                <% } %>
            </div>
            <div class="str-sort" runat="server" id="pnlSort">
                <div class="sort-variant sort-variant-l">
                    <%= Resource.Client_Catalog_SortBy%>
                    <asp:DropDownList ID="ddlSort" runat="server" onChange="ApplySearch();">
                    </asp:DropDownList>
                </div>
                <adv:ProductViewChanger runat="server" ID="productViewChanger" CurrentPage="Search" />
                <% if (SettingsCatalog.EnableCompareProducts)
                   { %>
                <div class="cp">
                    <a href="compareproducts.aspx" target="_blank" id="compare">
                        <%= Resource.Client_Catalog_CompareCart %>
                        (<span id="compare-count"><%= ShoppingCartService.CurrentCompare.Count() %></span>)</a>
                </div>
                <% } %>
                <br class="clear" />
            </div>
            <adv:ProductView ID="vProducts" runat="server" />
            <adv:AdvPaging runat="server" ID="paging" DisplayShowAll="True" />
            <%--<% }
               else
               { %>
            <div class="page-name">
                <%= Resource.Client_Search_EnterSearchTerm %>
            </div>
            <% } %>--%>
        </div>
        <br class="clear" />
    </div>
</asp:Content>
