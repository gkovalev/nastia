<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MainPageProduct.ascx.cs"
    Inherits="UserControls_MainPageProduct"  %>
<%@ Register TagPrefix="adv" TagName="Rating" Src="~/UserControls/Rating.ascx" %>
<%@ Import Namespace="AdvantShop.Catalog" %>
<%@ Import Namespace="AdvantShop.Core.UrlRewriter" %>
<%@ Import Namespace="AdvantShop.Helpers" %>
<!--container_special-->
<asp:MultiView runat="server" ID="mvMainPageProduct">
    <Views>
        <asp:View runat="server" ID="viewDefault">
            <table class="container-special">
                <tr>
                    <td class="block" runat="server" id="liBestsellers">
                        <div class="best-title">
                            <%= Resources.Resource.Client_Default_BestSellers %>
                            <a href="productlist.aspx?type=bestseller">
                                <%= Resources.Resource.Client_Default_AllBestSellers %></a></div>
                        <asp:ListView runat="server" ID="lvBestSellers" ItemPlaceholderID="liItemPlaceholder">
                            <LayoutTemplate>
                                <ul class="p-list">
                                    <li runat="server" id="liItemPlaceholder"></li>
                                </ul>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <li class="p-block">
                                    <table class="p-table">
                                        <tr>
                                            <td class="img-middle">
                                                <%# RenderPictureTag(SQLDataHelper.GetInt(Eval("productId")), SQLDataHelper.GetString(Eval("Photo")), SQLDataHelper.GetString(Eval("UrlPath")), SQLDataHelper.GetString(Eval("PhotoDesc")))%>
                                                <%# CatalogService.RenderLabels(Convert.ToBoolean(Eval("Recomended")), Convert.ToBoolean(Eval("OnSale")), Convert.ToBoolean(Eval("Bestseller")), Convert.ToBoolean(Eval("New")), SQLDataHelper.GetDecimal(Eval("Discount")))%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="pv-div-link">
                                                    <a href="<%# UrlService.GetLink(ParamType.Product, Eval("UrlPath").ToString(), Convert.ToInt32(Eval("ProductID"))) %>"
                                                        class="link-pv-name">
                                                        <%# Eval("Name") %></a>
                                                    <%# string.Format("<div class=\"sku\">{0}</div>", Eval("ArtNo")) %>
                                                </div>
                                                <adv:Rating runat="server" ProductId='<%# Convert.ToInt32(Eval("ProductID")) %>'
                                                    ShowRating='<%# EnableRating %>' Rating='<%# Convert.ToDouble(Eval("Ratio")) %>'
                                                    ReadOnly='<%# Eval("RatioID") != DBNull.Value %>' />
                                                <div class="price-container">
                                                    <div class="price">
                                                        <%# CatalogService.RenderPrice(Convert.ToDecimal(Eval("Price")), Convert.ToDecimal(Eval("Discount")), false, customerGroup)%>
                                                    </div>
                                                </div>
                                                <adv:Button data-cart-add='<%# Eval("ProductID") %>' data-cart-amount='<%# Eval("MinAmount") %>' runat="server" Size="Small" Type="Add"
                                                    Href='<%# UrlService.GetLink(ParamType.Product, Eval("UrlPath").ToString(),Convert.ToInt32(Eval("ProductID"))) %>'
                                                    Text='<%$ Resources:Resource, Client_Catalog_Add %>' Visible='<%# SQLDataHelper.GetDecimal(Eval("Price")) > 0 && SQLDataHelper.GetDecimal(Eval("Amount")) > 0 %>' />
                                                <adv:Button runat="server" Type="Action" Size="Small" Text='<%$ Resources:Resource, Client_Catalog_OrderByRequest %>'
                                                    Href='<%# "sendrequestonproduct.aspx?productid=" + Eval("productId") %>' Visible='<%# ((SQLDataHelper.GetDecimal(Eval("Price")) > 0 && SQLDataHelper.GetInt(Eval("Amount")) <= 0) && SQLDataHelper.GetBoolean(Eval("OrderByRequest"))) %>' />
                                                <adv:Button runat="server" Size="XSmall" Type="Buy" Text='<%$Resources:Resource, Client_More %>'
                                                    Href='<%# UrlService.GetLink(ParamType.Product, Eval("UrlPath").ToString(),Convert.ToInt32(Eval("ProductID"))) %>' />
                                            </td>
                                        </tr>
                                    </table>
                                </li>
                            </ItemTemplate>
                        </asp:ListView>
                    </td>
                    <td class="block" runat="server" id="liNew">
                        <div class="new-title">
                            <%= Resources.Resource.Client_Default_New %>
                            <a href="productlist.aspx?type=new">
                                <%= Resources.Resource.Client_Default_AllNew%></a></div>
                        <asp:ListView runat="server" ID="lvNew" ItemPlaceholderID="liItemPlaceholder">
                            <LayoutTemplate>
                                <ul class="p-list">
                                    <li runat="server" id="liItemPlaceholder"></li>
                                </ul>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <li class="p-block">
                                    <table class="p-table">
                                        <tr>
                                            <td class="img-middle">
                                                <%# RenderPictureTag(SQLDataHelper.GetInt(Eval("productId")), SQLDataHelper.GetString(Eval("Photo")), SQLDataHelper.GetString(Eval("UrlPath")), SQLDataHelper.GetString(Eval("PhotoDesc")))%>
                                                <%# CatalogService.RenderLabels(Convert.ToBoolean(Eval("Recomended")), Convert.ToBoolean(Eval("OnSale")), Convert.ToBoolean(Eval("Bestseller")), Convert.ToBoolean(Eval("New")), SQLDataHelper.GetDecimal(Eval("Discount")))%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="pv-div-link">
                                                    <a href="<%# UrlService.GetLink(ParamType.Product, Eval("UrlPath").ToString(),Convert.ToInt32(Eval("ProductID"))) %>"
                                                        class="link-pv-name">
                                                        <%# Eval("Name") %></a>
                                                    <%# string.Format("<div class=\"sku\">{0}</div>", Eval("ArtNo")) %>
                                                </div>
                                                <adv:Rating runat="server" ProductId='<%# Convert.ToInt32(Eval("ProductID")) %>'
                                                    ShowRating='<%# EnableRating %>' Rating='<%# Convert.ToDouble(Eval("Ratio")) %>'
                                                    ReadOnly='<%# Eval("RatioID") != DBNull.Value %>' />
                                                <div class="price-container">
                                                    <div class="price">
                                                        <%# CatalogService.RenderPrice(Convert.ToDecimal(Eval("Price")), Convert.ToDecimal(Eval("Discount")), false, customerGroup)%>
                                                    </div>
                                                </div>
                                                <adv:Button data-cart-add='<%# Eval("ProductID") %>' data-cart-amount='<%# Eval("MinAmount") %>' runat="server" Size="Small" Type="Add"
                                                    Href='<%# UrlService.GetLink(ParamType.Product, Eval("UrlPath").ToString(),Convert.ToInt32(Eval("ProductID"))) %>'
                                                    Text='<%$ Resources:Resource, Client_Catalog_Add %>' Visible='<%# SQLDataHelper.GetDecimal(Eval("Price")) > 0 && SQLDataHelper.GetDecimal(Eval("Amount")) > 0 %>' />
                                                <adv:Button runat="server" Type="Action" Size="Small" Text='<%$ Resources:Resource, Client_Catalog_OrderByRequest %>'
                                                    Href='<%# "sendrequestonproduct.aspx?productid=" + Eval("productId") %>' Visible='<%# ((SQLDataHelper.GetDecimal(Eval("Price")) > 0 && SQLDataHelper.GetInt(Eval("Amount")) <= 0) && SQLDataHelper.GetBoolean(Eval("OrderByRequest"))) %>' />
                                                <adv:Button runat="server" Size="XSmall" Type="Buy" Text='<%$Resources:Resource, Client_More %>'
                                                    Href='<%# UrlService.GetLink(ParamType.Product,Eval("UrlPath").ToString(), Convert.ToInt32(Eval("ProductID"))) %>' />
                                            </td>
                                        </tr>
                                    </table>
                                </li>
                            </ItemTemplate>
                        </asp:ListView>
                    </td>
                    <td class="block" runat="server" id="liDiscount">
                        <div class="discount-title">
                            <%= Resources.Resource.Client_Default_Discount %>
                            <a href="productlist.aspx?type=discount">
                                <%= Resources.Resource.Client_Default_AllDiscount %></a></div>
                        <asp:ListView runat="server" ID="lvDiscount" ItemPlaceholderID="liItemPlaceholder">
                            <LayoutTemplate>
                                <ul class="p-list">
                                    <li runat="server" id="liItemPlaceholder"></li>
                                </ul>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <li class="p-block">
                                    <table class="p-table">
                                        <tr>
                                            <td class="img-middle">
                                                <%# RenderPictureTag(SQLDataHelper.GetInt(Eval("productId")), SQLDataHelper.GetString(Eval("Photo")), SQLDataHelper.GetString(Eval("UrlPath")), SQLDataHelper.GetString(Eval("PhotoDesc")))%>
                                                <%# CatalogService.RenderLabels(Convert.ToBoolean(Eval("Recomended")), Convert.ToBoolean(Eval("OnSale")), Convert.ToBoolean(Eval("Bestseller")), Convert.ToBoolean(Eval("New")), SQLDataHelper.GetDecimal(Eval("Discount")))%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="pv-div-link">
                                                    <a href="<%# UrlService.GetLink(ParamType.Product,Eval("UrlPath").ToString(), Convert.ToInt32(Eval("ProductID"))) %>"
                                                        class="link-pv-name">
                                                        <%# Eval("Name") %></a>
                                                    <%# string.Format("<div class=\"sku\">{0}</div>", Eval("ArtNo")) %>
                                                </div>
                                                <adv:Rating runat="server" ProductId='<%# Convert.ToInt32(Eval("ProductID")) %>'
                                                    ShowRating='<%# EnableRating %>' Rating='<%# Convert.ToDouble(Eval("Ratio")) %>'
                                                    ReadOnly='<%# Eval("RatioID") != DBNull.Value %>' />
                                                <div class="price-container">
                                                    <div class="price">
                                                        <%# CatalogService.RenderPrice(Convert.ToDecimal(Eval("Price")), Convert.ToDecimal(Eval("Discount")), false, customerGroup)%>
                                                    </div>
                                                </div>
                                                <adv:Button data-cart-add='<%# Eval("ProductID") %>' data-cart-amount='<%# Eval("MinAmount") %>' runat="server" Size="Small" Type="Add"
                                                    Href='<%# UrlService.GetLink(ParamType.Product, Eval("UrlPath").ToString(),Convert.ToInt32(Eval("ProductID"))) %>'
                                                    Text='<%$ Resources:Resource, Client_Catalog_Add %>' Visible='<%# SQLDataHelper.GetDecimal(Eval("Price")) > 0 && SQLDataHelper.GetDecimal(Eval("Amount")) > 0 %>' />
                                                <adv:Button runat="server" Type="Action" Size="Small" Text='<%$ Resources:Resource, Client_Catalog_OrderByRequest %>'
                                                    Href='<%# "sendrequestonproduct.aspx?productid=" + Eval("productId") %>' Visible='<%# ((SQLDataHelper.GetDecimal(Eval("Price")) > 0 && SQLDataHelper.GetInt(Eval("Amount")) <= 0) && SQLDataHelper.GetBoolean(Eval("OrderByRequest"))) %>' />
                                                <adv:Button runat="server" Size="XSmall" Type="Buy" Text='<%$Resources:Resource, Client_More %>'
                                                    Href='<%# UrlService.GetLink(ParamType.Product, Eval("UrlPath").ToString(),Convert.ToInt32(Eval("ProductID"))) %>' />
                                            </td>
                                        </tr>
                                    </table>
                                </li>
                            </ItemTemplate>
                        </asp:ListView>
                    </td>
                </tr>
            </table>
            <script type="text/javascript">
                $("td:last-child", ".container-special").addClass("block-last");
            </script>
        </asp:View>
        <asp:View runat="server" ID="viewAlternative">
            <div class="block-best" runat="server" id="pnlBest">
                <div class="best-title">
                    <%= Resources.Resource.Client_Default_BestSellers %>
                    <a href="productlist.aspx?type=bestseller">
                        <%= Resources.Resource.Client_Default_AllBestSellers %></a></div>
                <div class="pv-tile">
                    <asp:ListView runat="server" ID="lvBestSellersAltervative"  ItemPlaceholderID="liItemPlaceholder">
                        <ItemTemplate>
                            <div class="pv-item">
                                <table class="p-table">
                                    <tr>
                                        <td class="img-middle">
                                            <div class="pv-photo">
                                                <%# RenderPictureTag(SQLDataHelper.GetInt(Eval("productId")), SQLDataHelper.GetString(Eval("Photo")), SQLDataHelper.GetString(Eval("UrlPath")), SQLDataHelper.GetString(Eval("PhotoDesc")))%>
                                            </div>
                                            <%# CatalogService.RenderLabels(Convert.ToBoolean(Eval("Recomended")), Convert.ToBoolean(Eval("OnSale")), Convert.ToBoolean(Eval("Bestseller")), Convert.ToBoolean(Eval("New")), SQLDataHelper.GetDecimal(Eval("Discount")))%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="pv-div-link">
                                                <a href="<%# UrlService.GetLink(ParamType.Product, Eval("UrlPath").ToString(),Convert.ToInt32(Eval("ProductID"))) %>"
                                                    class="link-pv-name">
                                                    <%# Eval("Name") %></a>
                                                <%# string.Format("<div class=\"sku\">{0}</div>", Eval("ArtNo")) %>
                                            </div>
                                            <adv:Rating runat="server" ProductId='<%# Convert.ToInt32(Eval("ProductID")) %>'
                                                ShowRating='<%# EnableRating %>' Rating='<%# Convert.ToDouble(Eval("Ratio")) %>'
                                                ReadOnly='<%# Eval("RatioID") != DBNull.Value %>' />
                                            <div class="price-container">
                                                <div class="price">
                                                    <%# CatalogService.RenderPrice(Convert.ToDecimal(Eval("Price")), Convert.ToDecimal(Eval("Discount")), false, customerGroup)%>
                                                </div>
                                            </div>
                                            <div class="pv-btns">
                                                <adv:Button data-cart-add='<%# Eval("ProductID") %>'  data-cart-amount='<%# Eval("MinAmount") %>' runat="server" Size="Small" Type="Add" 
                                                    Href='<%# UrlService.GetLink(ParamType.Product, Eval("UrlPath").ToString(),Convert.ToInt32(Eval("ProductID"))) %>'
                                                    Text='<%$ Resources:Resource, Client_Catalog_Add %>' Visible='<%# SQLDataHelper.GetDecimal(Eval("Price")) > 0 && SQLDataHelper.GetDecimal(Eval("Amount")) > 0 %>' />
                                                <adv:Button runat="server" Type="Action" Size="Small" Text='<%$ Resources:Resource, Client_Catalog_OrderByRequest %>'
                                                    Href='<%# "sendrequestonproduct.aspx?productid=" + Eval("productId") %>' Visible='<%# ((SQLDataHelper.GetDecimal(Eval("Price")) > 0 && SQLDataHelper.GetInt(Eval("Amount")) <= 0) && SQLDataHelper.GetBoolean(Eval("OrderByRequest"))) %>' />
                                                <adv:Button runat="server" Size="XSmall" Type="Buy" Text='<%$Resources:Resource, Client_More %>'
                                                    Href='<%# UrlService.GetLink(ParamType.Product,Eval("UrlPath").ToString(), Convert.ToInt32(Eval("ProductID"))) %>' />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ItemTemplate>
                    </asp:ListView>
                </div>
            </div>
            <div class="block-best" runat="server" id="pnlNew">
                <div class="best-title">
                    <%= Resources.Resource.Client_Default_New %>
                    <a href="productlist.aspx?type=new">
                        <%= Resources.Resource.Client_Default_AllNew%></a></div>
                <div class="pv-tile">
                    <asp:ListView runat="server" ID="lvNewAlternative" ItemPlaceholderID="liItemPlaceholder">
                        <ItemTemplate>
                            <div class="pv-item">
                                <table class="p-table">
                                    <tr>
                                        <td class="img-middle">
                                            <div class="pv-photo">
                                                <%# RenderPictureTag(SQLDataHelper.GetInt(Eval("productId")), SQLDataHelper.GetString(Eval("Photo")), SQLDataHelper.GetString(Eval("UrlPath")), SQLDataHelper.GetString(Eval("PhotoDesc")))%>
                                            </div>
                                            <%# CatalogService.RenderLabels(Convert.ToBoolean(Eval("Recomended")), Convert.ToBoolean(Eval("OnSale")), Convert.ToBoolean(Eval("Bestseller")), Convert.ToBoolean(Eval("New")), SQLDataHelper.GetDecimal(Eval("Discount")))%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="pv-div-link">
                                                <a href="<%# UrlService.GetLink(ParamType.Product,Eval("UrlPath").ToString(), Convert.ToInt32(Eval("ProductID"))) %>"
                                                    class="link-pv-name">
                                                    <%# Eval("Name") %></a>
                                                <%# string.Format("<div class=\"sku\">{0}</div>", Eval("ArtNo")) %>
                                            </div>
                                            <adv:Rating runat="server" ProductId='<%# Convert.ToInt32(Eval("ProductID")) %>'
                                                ShowRating='<%# EnableRating %>' Rating='<%# Convert.ToDouble(Eval("Ratio")) %>'
                                                ReadOnly='<%# Eval("RatioID") != DBNull.Value %>' />
                                            <div class="price-container">
                                                <div class="price">
                                                    <%# CatalogService.RenderPrice(Convert.ToDecimal(Eval("Price")), Convert.ToDecimal(Eval("Discount")), false, customerGroup)%>
                                                </div>
                                            </div>
                                            <div class="pv-btns">
                                                <adv:Button data-cart-add='<%# Eval("ProductID") %>'  data-cart-amount='<%# Eval("MinAmount") %>' runat="server" Size="Small" Type="Add" 
                                                    Href='<%# UrlService.GetLink(ParamType.Product, Eval("UrlPath").ToString(),Convert.ToInt32(Eval("ProductID"))) %>'
                                                    Text='<%$ Resources:Resource, Client_Catalog_Add %>' Visible='<%# SQLDataHelper.GetDecimal(Eval("Price")) > 0 && SQLDataHelper.GetDecimal(Eval("Amount")) > 0 %>' />
                                                <adv:Button runat="server" Type="Action" Size="Small" Text='<%$ Resources:Resource, Client_Catalog_OrderByRequest %>'
                                                    Href='<%# "sendrequestonproduct.aspx?productid=" + Eval("productId") %>' Visible='<%# ((SQLDataHelper.GetDecimal(Eval("Price")) > 0 && SQLDataHelper.GetInt(Eval("Amount")) <= 0) && SQLDataHelper.GetBoolean(Eval("OrderByRequest"))) %>' />
                                                <adv:Button runat="server" Size="XSmall" Type="Buy" Text='<%$Resources:Resource, Client_More %>'
                                                    Href='<%# UrlService.GetLink(ParamType.Product,Eval("UrlPath").ToString(), Convert.ToInt32(Eval("ProductID"))) %>' />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ItemTemplate>
                    </asp:ListView>
                </div>
            </div>
            <div class="block-best" runat="server" id="pnlDiscount">
                <div class="discount-title">
                    <%= Resources.Resource.Client_Default_Discount %>
                    <a href="productlist.aspx?type=discount">
                        <%= Resources.Resource.Client_Default_AllDiscount %></a></div>
                <div class="pv-tile">
                    <asp:ListView runat="server" ID="lvDiscountAlternative" ItemPlaceholderID="liItemPlaceholder">
                        <ItemTemplate>
                            <div class="pv-item">
                                <table class="p-table">
                                    <tr>
                                        <td class="img-middle">
                                            <div class="pv-photo">
                                                <%# RenderPictureTag(SQLDataHelper.GetInt(Eval("productId")), SQLDataHelper.GetString(Eval("Photo")), SQLDataHelper.GetString(Eval("UrlPath")), SQLDataHelper.GetString(Eval("PhotoDesc")))%>
                                            </div>
                                            <%# CatalogService.RenderLabels(Convert.ToBoolean(Eval("Recomended")), Convert.ToBoolean(Eval("OnSale")), Convert.ToBoolean(Eval("Bestseller")), Convert.ToBoolean(Eval("New")), SQLDataHelper.GetDecimal(Eval("Discount")))%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="pv-div-link">
                                                <a href="<%# UrlService.GetLink(ParamType.Product,Eval("UrlPath").ToString(), Convert.ToInt32(Eval("ProductID"))) %>"
                                                    class="link-pv-name">
                                                    <%# Eval("Name") %></a>
                                                <%# string.Format("<div class=\"sku\">{0}</div>", Eval("ArtNo")) %>
                                            </div>
                                            <adv:Rating runat="server" ProductId='<%# Convert.ToInt32(Eval("ProductID")) %>'
                                                ShowRating='<%# EnableRating %>' Rating='<%# Convert.ToDouble(Eval("Ratio")) %>'
                                                ReadOnly='<%# Eval("RatioID") != DBNull.Value %>' />
                                            <div class="price-container">
                                                <div class="price">
                                                    <%# CatalogService.RenderPrice(Convert.ToDecimal(Eval("Price")), Convert.ToDecimal(Eval("Discount")), false, customerGroup)%>
                                                </div>
                                            </div>
                                            <div class="pv-btns">
                                                <adv:Button data-cart-add='<%# Eval("ProductID") %>'  data-cart-amount='<%# Eval("MinAmount") %>' runat="server" Size="Small" Type="Add" 
                                                    Href='<%# UrlService.GetLink(ParamType.Product, Eval("UrlPath").ToString(),Convert.ToInt32(Eval("ProductID"))) %>'
                                                    Text='<%$ Resources:Resource, Client_Catalog_Add %>' Visible='<%# SQLDataHelper.GetDecimal(Eval("Price")) > 0 && SQLDataHelper.GetDecimal(Eval("Amount")) > 0 %>' />
                                                <adv:Button runat="server" Type="Action" Size="Small" Text='<%$ Resources:Resource, Client_Catalog_OrderByRequest %>'
                                                    Href='<%# "sendrequestonproduct.aspx?productid=" + Eval("productId") %>' Visible='<%# ((SQLDataHelper.GetDecimal(Eval("Price")) > 0 && SQLDataHelper.GetInt(Eval("Amount")) <= 0) && SQLDataHelper.GetBoolean(Eval("OrderByRequest"))) %>' />
                                                <adv:Button runat="server" Size="XSmall" Type="Buy" Text='<%$Resources:Resource, Client_More %>'
                                                    Href='<%# UrlService.GetLink(ParamType.Product,Eval("UrlPath").ToString(), Convert.ToInt32(Eval("ProductID"))) %>' />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ItemTemplate>
                    </asp:ListView>
                </div>
            </div>
        </asp:View>
    </Views>
</asp:MultiView>
<div class="clear">
</div>
<!--end_container_special-->
