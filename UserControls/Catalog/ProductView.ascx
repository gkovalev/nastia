<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProductView.ascx.cs" Inherits="UserControls_ProductView"
    EnableViewState="false" %>
<%@ Register Src="~/UserControls/Rating.ascx" TagName="Rating" TagPrefix="adv" %>
<%@ Register Src="~/UserControls/Catalog/CompareControl.ascx" TagName="CompareControl"
    TagPrefix="adv" %>
<%@ Import Namespace="AdvantShop.Catalog" %>
<%@ Import Namespace="AdvantShop.Core.UrlRewriter" %>
<%@ Import Namespace="AdvantShop.Helpers" %>
<%@ Import Namespace="Resources" %>
<asp:MultiView runat="server" ID="mvProducts">
    <Views>
        <asp:View runat="server" ID="viewTile">
            <asp:ListView runat="server" ID="lvTile" ItemPlaceholderID="tilePlaceHolder">
                <LayoutTemplate>
                    <div class="pv-tile">
                        <div runat="server" id="tilePlaceHolder">
                        </div>
                    </div>
                </LayoutTemplate>
                <ItemTemplate>
                    <div class="pv-item">
                        <table class="p-table">
                            <tr>
                                <td class="img-middle">
                                    <div class="pv-photo">
                                        <%# RenderPictureTag(SQLDataHelper.GetString(Eval("Photo")), UrlService.GetLink(ParamType.Product, SQLDataHelper.GetString(Eval("UrlPath")), Convert.ToInt32(Eval("ProductId"))), SQLDataHelper.GetString(Eval("PhotoDesc")))%>
                                    </div>
                                    <%# CatalogService.RenderLabels(SQLDataHelper.GetBoolean(Eval("Recomended")), SQLDataHelper.GetBoolean(Eval("OnSale")), SQLDataHelper.GetBoolean(Eval("Bestseller")), SQLDataHelper.GetBoolean(Eval("New")), SQLDataHelper.GetDecimal(Eval("Discount")))%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="pv-div-link compare-<%#Eval("productId") %>">
                                        <a href="<%# UrlService.GetLink(ParamType.Product, SQLDataHelper.GetString(Eval("UrlPath")), Convert.ToInt32(Eval("ProductId"))) %>"
                                            class="link-pv-name">
                                            <%# Eval("Name") %></a>
                                    </div>
                                    <%# string.Format("<div class=\"sku\">{0}</div>", Eval("ArtNo")) %>
                                    <adv:Rating runat="server" ProductId='<%# Convert.ToInt32(Eval("ProductID")) %>'
                                        ShowRating='<%# EnableRating %>' Rating='<%# Convert.ToDouble(Eval("Ratio")) %>'
                                        ReadOnly='<%# Eval("RatioID") != DBNull.Value %>' />
                                    <div class="price-container">
                                        <%# CatalogService.RenderPrice(SQLDataHelper.GetDecimal(Eval("Price")), SQLDataHelper.GetDecimal(Eval("Discount")), false, customerGroup)%>
                                    </div>
                                    <div class="pv-btns">
                                        <adv:Button data-cart-add='<%# Eval("ProductID") %>' data-cart-amount='<%# Eval("MinAmount") %>' runat="server" Type="Add" Size="Small" Text='<%$ Resources:Resource, Client_Catalog_Add %>'
                                            Href='<%# UrlService.GetLink(ParamType.Product, SQLDataHelper.GetString(Eval("UrlPath")), Convert.ToInt32(Eval("ProductId"))) %>'
                                            Visible='<%# SQLDataHelper.GetDecimal(Eval("Price")) > 0 && SQLDataHelper.GetDecimal(Eval("Amount")) > 0 %>' />
                                        <adv:Button runat="server" Type="Action" Size="Small" Text='<%$ Resources:Resource, Client_Catalog_OrderByRequest %>'
                                            Href='<%# "sendrequestonproduct.aspx?productid=" + Eval("productId") %>' Visible='<%# ((SQLDataHelper.GetDecimal(Eval("Price")) > 0 && SQLDataHelper.GetInt(Eval("Amount")) <= 0) && SQLDataHelper.GetBoolean(Eval("OrderByRequest"))) %>' />
                                        <adv:Button runat="server" Type="Buy" Size="Small" Text='<%$ Resources:Resource, Client_More %>'
                                            Href='<%# UrlService.GetLink(ParamType.Product, SQLDataHelper.GetString(Eval("UrlPath")), Convert.ToInt32(Eval("ProductId"))) %>' />
                                    </div>
                                    <adv:CompareControl ID="CompareControl" runat="server" Visible='<%# EnableCompare %>' ProductId='<%# Eval("productId") %>'
                                        IsSelected='<%# Eval("ItemId") != DBNull.Value%>' />
                                </td>
                            </tr>
                        </table>
                    </div>
                </ItemTemplate>
                <EmptyItemTemplate>
                    <div class="no-items">
                        <%= Resource.Client_Catalog_NoItemsFound  %>
                    </div>
                </EmptyItemTemplate>
            </asp:ListView>
        </asp:View>
        <asp:View runat="server" ID="viewList">
            <asp:ListView runat="server" ID="lvList" ItemPlaceholderID="listPlaceholder">
                <LayoutTemplate>
                    <div class="pv-list">
                        <div runat="server" id="listPlaceholder">
                        </div>
                    </div>
                </LayoutTemplate>
                <ItemTemplate>
                    <div class="pv-item">
                        <div class="pv-photo-c">
                            <div class="pv-photo">
                                <%# RenderPictureTag(SQLDataHelper.GetString(Eval("Photo")), UrlService.GetLink(ParamType.Product, SQLDataHelper.GetString(Eval("UrlPath")), Convert.ToInt32(Eval("ProductId"))), SQLDataHelper.GetString(Eval("PhotoDesc")))%>
                            </div>
                            <%# CatalogService.RenderLabels(SQLDataHelper.GetBoolean(Eval("Recomended")), SQLDataHelper.GetBoolean(Eval("OnSale")), SQLDataHelper.GetBoolean(Eval("Bestseller")), SQLDataHelper.GetBoolean(Eval("New")), SQLDataHelper.GetDecimal(Eval("Discount")))%>
                        </div>
                        <div class="pv-info">
                            <a href="<%# UrlService.GetLink(ParamType.Product, SQLDataHelper.GetString(Eval("UrlPath")), Convert.ToInt32(Eval("ProductId"))) %>"
                                class="link-pv-name compare-<%#Eval("productId") %>">
                                <%# Eval("Name") %></a>
                            <div class="sku">
                                <%# Eval("ArtNo") %></div>
                            <adv:Rating runat="server" ProductId='<%# Convert.ToInt32(Eval("ProductID")) %>'
                                ShowRating='<%# EnableRating %>' Rating='<%# Convert.ToDouble(Eval("Ratio")) %>'
                                ReadOnly='<%# Eval("RatioID") != DBNull.Value %>' />
                            <asp:Label ID="lblNoAmount" runat="server" CssClass="not-available" Text='<%$ Resources:Resource, Client_Catalog_NotAvailable%>'
                                Visible='<%#SQLDataHelper.GetDecimal(Eval("Amount")) <= 0 %>'></asp:Label>
                            <%# !string.IsNullOrWhiteSpace(Eval("BriefDescription").ToString()) ? "<div class=\"descr\">" + Eval("BriefDescription") + "</div>" : string.Empty %>
                            <div class="price-container">
                                <%# CatalogService.RenderPrice(SQLDataHelper.GetDecimal(Eval("Price")), SQLDataHelper.GetDecimal(Eval("Discount")), true, customerGroup)%>
                            </div>
                            <div class="pv-btns">
                                <adv:Button data-cart-add='<%# Eval("ProductID") %>' data-cart-amount='<%# Eval("MinAmount") %>' runat="server" Type="Add" Size="Small" Text='<%$ Resources:Resource, Client_Catalog_Add %>'
                                    Href='<%# UrlService.GetLink(ParamType.Product, SQLDataHelper.GetString(Eval("UrlPath")), Convert.ToInt32(Eval("ProductId"))) %>'
                                    Visible='<%# SQLDataHelper.GetDecimal(Eval("Price")) > 0 && SQLDataHelper.GetDecimal(Eval("Amount")) > 0 %>' />
                                <adv:Button runat="server" Type="Action" Size="Small" Text='<%$ Resources:Resource, Client_Catalog_OrderByRequest %>'
                                    Href='<%# "sendrequestonproduct.aspx?productid=" + Eval("productId") %>' Visible='<%# ((SQLDataHelper.GetDecimal(Eval("Price")) > 0 && SQLDataHelper.GetInt(Eval("Amount")) <= 0) && SQLDataHelper.GetBoolean(Eval("OrderByRequest"))) %>' />
                                <adv:Button runat="server" Type="Buy" Size="Small" Text='<%$ Resources:Resource, Client_More %>'
                                    Href='<%# UrlService.GetLink(ParamType.Product, SQLDataHelper.GetString(Eval("UrlPath")), Convert.ToInt32(Eval("ProductId"))) %>' />
                                <adv:CompareControl ID="CompareControl" runat="server" Visible='<%# EnableCompare %>'
                                    ProductId='<%# Eval("productId") %>' IsSelected='<%# Eval("ItemId") != DBNull.Value%>' />
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
                <EmptyDataTemplate>
                    <div class="no-items">
                        <%= Resource.Client_Catalog_NoItemsFound  %>
                    </div>
                </EmptyDataTemplate>
            </asp:ListView>
        </asp:View>
        <asp:View runat="server" ID="viewTable">
            <asp:ListView runat="server" ID="lvTable" ItemPlaceholderID="tablePlaceHolder">
                <LayoutTemplate>
                    <table class="pv-table">
                        <tr class="head">
                            <th class="icon">
                            </th>
                            <th class="p-name">
                                <asp:Literal runat="server" Text="<%$ Resources:Resource, Client_UserControls_ProductView_Name%>" />
                            </th>
                            <th class="rating">
                            </th>
                            <th class="pv-price">
                                <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Resource, Client_UserControls_ProductView_Price%>" />
                            </th>
                            <th class="btns">
                            </th>
                        </tr>
                        <tr runat="server" id="tablePlaceHolder" />
                    </table>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr class="pv-item">
                        <td class="icon" <%# RenderPictureTag(SQLDataHelper.GetString(Eval("Photo")), UrlService.GetLink(ParamType.Product, SQLDataHelper.GetString(Eval("UrlPath")), Convert.ToInt32(Eval("ProductId"))), SQLDataHelper.GetString(Eval("PhotoDesc")))%>>
                            <%# !string.IsNullOrEmpty(Eval("Photo").ToString()) ? "<div class=\"photo\"></div>" : string.Empty%>
                            <adv:CompareControl Type="Icon" ID="CompareControl" runat="server" Visible='<%# EnableCompare %>'
                                ProductId='<%# Eval("productId") %>' IsSelected='<%# Eval("ItemId") != DBNull.Value%>' />
                        </td>
                        <td>
                            <a href="<%# UrlService.GetLink(ParamType.Product, SQLDataHelper.GetString(Eval("UrlPath")), Convert.ToInt32(Eval("ProductId"))) %>"
                                class="link-pv-name compare-<%#Eval("productId") %>">
                                <%# Eval("Name") %></a>
                            <div class="sku">
                                <%# Eval("ArtNo") %></div>
                        </td>
                        <td class="rating">
                            <adv:Rating runat="server" ProductId='<%# Convert.ToInt32(Eval("ProductID")) %>'
                                ShowRating='<%# EnableRating %>' Rating='<%# Convert.ToDouble(Eval("Ratio")) %>'
                                ReadOnly='<%# Eval("RatioID") != DBNull.Value %>' />
                        </td>
                        <td class="pv-price">
                            <%# CatalogService.RenderPrice(SQLDataHelper.GetDecimal(Eval("Price")), SQLDataHelper.GetDecimal(Eval("Discount")), false, customerGroup)%>
                            <%# CatalogService.RenderLabels(SQLDataHelper.GetBoolean(Eval("Recomended")), SQLDataHelper.GetBoolean(Eval("OnSale")), SQLDataHelper.GetBoolean(Eval("Bestseller")), SQLDataHelper.GetBoolean(Eval("New")), SQLDataHelper.GetDecimal(Eval("Discount")), 1)%>
                        </td>
                        <td class="btns">
                            <adv:Button data-cart-add='<%# Eval("ProductID") %>' data-cart-amount='<%# Eval("MinAmount") %>' runat="server" Type="Add" Size="Small" Text='<%$ Resources:Resource, Client_Catalog_Add %>'
                                Href='<%# UrlService.GetLink(ParamType.Product, SQLDataHelper.GetString(Eval("UrlPath")), Convert.ToInt32(Eval("ProductId"))) %>'
                                Visible='<%# SQLDataHelper.GetDecimal(Eval("Price")) > 0 && SQLDataHelper.GetDecimal(Eval("Amount")) > 0 %>' />
                            <adv:Button runat="server" Type="Action" Size="Small" Text='<%$ Resources:Resource, Client_Catalog_OrderByRequest %>'
                                Href='<%# "sendrequestonproduct.aspx?productid=" + Eval("productId") %>' Visible='<%# ((SQLDataHelper.GetDecimal(Eval("Price")) > 0 && SQLDataHelper.GetInt(Eval("Amount")) <= 0) && SQLDataHelper.GetBoolean(Eval("OrderByRequest"))) %>' />
                            <adv:Button runat="server" Type="Buy" Size="Small" Text='<%$ Resources:Resource, Client_More %>'
                                Href='<%# UrlService.GetLink(ParamType.Product, SQLDataHelper.GetString(Eval("UrlPath")), Convert.ToInt32(Eval("ProductId"))) %>' />
                        </td>
                    </tr>
                </ItemTemplate>
                <EmptyItemTemplate>
                    <div class="no-items">
                        <%= Resource.Client_Catalog_NoItemsFound  %>
                    </div>
                </EmptyItemTemplate>
            </asp:ListView>
        </asp:View>
    </Views>
</asp:MultiView>
