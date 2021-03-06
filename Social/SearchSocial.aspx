<%@ Page Title="" Language="C#" MasterPageFile="MasterPageSocial.master" AutoEventWireup="true"
    CodeFile="SearchSocial.aspx.cs" Inherits="SearchSocial" %>

<%@ Import Namespace="AdvantShop.Configuration" %>
<%@ Import Namespace="AdvantShop.Orders" %>
<%@ Import Namespace="Resources" %>
<%@ Register Src="~/Social/UserControls/ProductViewSocial.ascx" TagName="ProductViewSocial"
    TagPrefix="adv" %>
<%@ Register TagPrefix="adv" TagName="FilterPrice" Src="~/Social/UserControls/FilterPrice.ascx" %>
<%@ Register TagPrefix="adv" TagName="ProductViewChanger" Src="~/UserControls/Catalog/ProductViewChanger.ascx" %>
<%@ MasterType VirtualPath="MasterPageSocial.master" %>
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
    <div class="bloсk-search block-uc">
        <div class="title">
            <%=Resource.Client_Search_SearchParams %></div>
        <div class="content">
            <table class="t-search">
                <tr>
                    <td>
                        <div class="param-name">
                            <%=Resource.Client_Search_Find %>:</div>
                        <div class="param-value">
                            <adv:AdvTextBox runat="server" ID="txtName" DefaultButtonID="btnFind" />
                        </div>
                    </td>
                    <td>
                        <div class="param-name">
                            <%=Resource.Client_Search_InCategories %>:
                        </div>
                        <div class="param-value">
                            <asp:DropDownList ID="ddlCategory" runat="server">
                                <asp:ListItem Text="<%$ Resources:Resource, Client_Search_AllCategories %>" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </td>
                </tr>
                <tr>
                   <%-- <td>
                        <div class="param-name">
                            <%=Resource.Client_Search_SKU %>:</div>
                        <div class="param-value">
                            <adv:AdvTextBox ID="txtSKU" runat="server" DefaultButtonID="btnFind" />
                        </div>
                    </td>--%>
                    <td colspan="2">
                        <adv:FilterPrice runat="server" ID="filterPrice" NotExpander="True" />
                           <div class="search-apply">
                                                        <adv:Button runat="server" ID="btnFind" Type="Submit" Size="Big" OnClientClick="ApplySearch();"
                                Text="<%$ Resources:Resource, Client_Search_Find %>" />
                           </div>
                    </td>
                </tr>
            </table>
            <div class="declare">
            </div>
        </div>
    </div>
    <% if (!String.IsNullOrWhiteSpace(SearchTerm))
       {%>
    <div class="page-name">
        <%= Resource.Client_Search_Found%>
        <strong>
            <asp:Literal runat="server" ID="lItemsCount"></asp:Literal></strong>
        <%= Resource.Client_Search_OnQuery%>
        &quot;<%= SearchTerm%>&quot;</div>
    <div class="str-sort" runat="server" id="pnlSort">
        <div class="sort-variant sort-variant-l">
            <%= Resource.Client_Catalog_SortBy%>
            <asp:DropDownList ID="ddlSort" runat="server" onChange="ApplySearch();">
            </asp:DropDownList>
        </div>
        <adv:ProductViewChanger runat="server" ID="productViewChanger" CurrentPage="Search" />
        <br class="clear" />
    </div>
    <adv:ProductViewSocial ID="vProducts" runat="server" />
    <adv:AdvPaging runat="server" ID="paging" DisplayShowAll="True" />
    <% }
       else
       { %>
    <div class="page-name">
        <%= Resource.Client_Search_EnterSearchTerm %>
    </div>
    <% } %>
</asp:Content>
