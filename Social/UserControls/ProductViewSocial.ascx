<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProductViewSocial.ascx.cs"
    Inherits="UserControls_ProductViewSocial" EnableViewState="false" %>
<%@ Register Src="~/UserControls/Rating.ascx" TagName="Rating" TagPrefix="adv" %>
<%@ Import Namespace="AdvantShop.Catalog" %>
<%@ Import Namespace="AdvantShop.Configuration" %>
<%@ Import Namespace="AdvantShop.Core.UrlRewriter" %>
<%@ Import Namespace="AdvantShop.Helpers" %>
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
                                        <%# RenderPictureTag(SQLDataHelper.GetString(Eval("Photo")), SQLDataHelper.GetString(Eval("Name")), "social/detailssocial.aspx?productId=" + Eval("ProductId"))%>
                                    </div>
                                    <%# CatalogService.RenderLabels(Convert.ToBoolean(Eval("Recomended")), Convert.ToBoolean(Eval("OnSale")), Convert.ToBoolean(Eval("Bestseller")), Convert.ToBoolean(Eval("New")), SQLDataHelper.GetDecimal(Eval("Discount")))%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="pv-div-link">
                                        <a href="<%# "social/detailssocial.aspx?productId=" + Eval("ProductId") %>" class="link-pv-name">
                                            <%# Eval("Name") %></a>
                                    </div>
                                    <%# SQLDataHelper.GetString(Eval("Name")).Length < 20 ? string.Format("<div class=\"sku\">{0}</div>", Eval("ArtNo")) : string.Empty %>
                                    <adv:Rating ID="rating1" runat="server" ProductId='<%# Convert.ToInt32(Eval("ProductID")) %>'
                                        ShowRating='<%# EnableRating %>' Rating='<%# Convert.ToDouble(Eval("Ratio")) %>'
                                        ReadOnly='<%# Eval("RatioID") != DBNull.Value %>' />
                                    <div class="price-container">
                                        <%# CatalogService.RenderPrice(Convert.ToDecimal(Eval("Price")), Convert.ToDecimal(Eval("Discount")), false, customerGroup)%>
                                    </div>
                                    <div class="pv-btns">
                                        <adv:Button ID="btnAdd" CssSpan="btn-add-icon" CssClass="no-pie" runat="server" Type="Add" Size="Middle"
                                            Text='<%$ Resources:Resource, Client_Catalog_Add %>' Href='<%# UrlService.GetLink(ParamType.Product, SQLDataHelper.GetString(Eval("UrlPath")), Convert.ToInt32(Eval("ProductId"))) %>'
                                            Visible='<%# SQLDataHelper.GetDecimal(Eval("Price")) > 0 && SQLDataHelper.GetDecimal(Eval("Amount")) > 0 %>'
                                            data-cart-add='<%# Eval("ProductID") %>' />
                                        <adv:Button ID="btnOrderByRequest" runat="server" Type="Action" Size="Middle" Text='<%$ Resources:Resource, Client_Catalog_OrderByRequest %>'
                                            Href='<%# "sendrequestonproduct.aspx?productid=" + Eval("productId") %>' Visible='<%# ((SQLDataHelper.GetDecimal(Eval("Price")) > 0 && SQLDataHelper.GetInt(Eval("Amount")) <= 0) && SQLDataHelper.GetBoolean(Eval("OrderByRequest"))) %>'
                                            Target="_blank" />
                                        <adv:Button ID="btnBuy" runat="server" Type="Buy" Size="Middle" Text='<%$ Resources:Resource, Client_More %>'
                                            Href='<%# "social/detailssocial.aspx?productId=" + Eval("ProductId") %>' />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </ItemTemplate>
                <EmptyItemTemplate>
                    <div class="no-items">
                        <%= Resources.Resource.Client_Catalog_NoItemsFound  %>
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
                                <%# RenderPictureTag(SQLDataHelper.GetString(Eval("Photo")), SQLDataHelper.GetString(Eval("Name")), "social/detailssocial.aspx?productId=" + Eval("ProductId"))%>
                            </div>
                            <%# CatalogService.RenderLabels(Convert.ToBoolean(Eval("Recomended")), Convert.ToBoolean(Eval("OnSale")), Convert.ToBoolean(Eval("Bestseller")), Convert.ToBoolean(Eval("New")), SQLDataHelper.GetDecimal(Eval("Discount")))%>
                        </div>
                        <div class="pv-info">
                            <a href="<%# "social/detailssocial.aspx?productId=" + Eval("ProductId")%>" class="link-pv-name">
                                <%# Eval("Name") %></a>
                            <div class="sku">
                                <%# Eval("ArtNo") %></div>
                            <adv:Rating ID="rating1" runat="server" ProductId='<%# Convert.ToInt32(Eval("ProductID")) %>'
                                ShowRating='<%# EnableRating %>' Rating='<%# Convert.ToDouble(Eval("Ratio")) %>'
                                ReadOnly='<%# Eval("RatioID") != DBNull.Value %>' />
                            <%# !string.IsNullOrWhiteSpace(Eval("BriefDescription").ToString()) ? "<div class=\"descr\">" + Eval("BriefDescription") + "</div>" : string.Empty %>
                            <div class="price-container">
                                <%# CatalogService.RenderPrice(Convert.ToDecimal(Eval("Price")), Convert.ToDecimal(Eval("Discount")), true, customerGroup)%>
                            </div>
                            <div class="pv-btns">
                                <adv:Button ID="btnAdd" CssClass="no-pie" CssSpan="btn-add-icon" runat="server" Type="Add" Size="Middle"
                                    Text='<%$ Resources:Resource, Client_Catalog_Add %>' Href='<%# UrlService.GetLink(ParamType.Product, SQLDataHelper.GetString(Eval("UrlPath")), Convert.ToInt32(Eval("ProductId"))) %>'
                                    Visible='<%# SQLDataHelper.GetDecimal(Eval("Price")) > 0 && SQLDataHelper.GetDecimal(Eval("Amount")) > 0 %>'
                                    data-cart-add='<%# Eval("ProductID") %>' />
                                <adv:Button ID="btnOrderByRequest" runat="server" Type="Action" Size="Middle" Text='<%$ Resources:Resource, Client_Catalog_OrderByRequest %>'
                                    Href='<%# "sendrequestonproduct.aspx?productid=" + Eval("productId") %>' Visible='<%# ((SQLDataHelper.GetDecimal(Eval("Price")) > 0 && SQLDataHelper.GetInt(Eval("Amount")) <= 0) && SQLDataHelper.GetBoolean(Eval("OrderByRequest"))) %>'
                                    Target="_blank" />
                                <adv:Button ID="btnBuy" runat="server" Type="Buy" Size="Middle" Text='<%$ Resources:Resource, Client_More %>'
                                    Href='<%# "social/detailssocial.aspx?productId=" + Eval("ProductId") %>' />
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
                <EmptyDataTemplate>
                    <div class="no-items">
                        <%= Resources.Resource.Client_Catalog_NoItemsFound  %>
                    </div>
                </EmptyDataTemplate>
            </asp:ListView>
        </asp:View>
        <asp:View runat="server" ID="viewTable">
            <asp:ListView runat="server" ID="lvTable" ItemPlaceholderID="tablePlaceHolder">
                <LayoutTemplate>
                    <table class="pv-table">
                        <tr class="head">
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
                        <td>
                            <a href="<%# "social/detailssocial.aspx?productId=" + Eval("ProductId")%> %>"
                                class="link-pv-name">
                                <%# Eval("Name") %></a>
                            <div class="sku">
                                <%# Eval("ArtNo") %></div>
                        </td>
                        <td class="rating">
                            <adv:Rating ID="rating1" runat="server" ProductId='<%# Convert.ToInt32(Eval("ProductID")) %>'
                                ShowRating='<%# EnableRating %>' Rating='<%# Convert.ToDouble(Eval("Ratio")) %>'
                                ReadOnly='<%# Eval("RatioID") != DBNull.Value %>' />
                        </td>
                        <td class="pv-price">
                            <%# CatalogService.RenderPrice(Convert.ToDecimal(Eval("Price")), Convert.ToDecimal(Eval("Discount")), false, customerGroup)%>
                            <%# CatalogService.RenderLabels(Convert.ToBoolean(Eval("Recomended")), Convert.ToBoolean(Eval("OnSale")), Convert.ToBoolean(Eval("Bestseller")), Convert.ToBoolean(Eval("New")), SQLDataHelper.GetDecimal(Eval("Discount")), 1)%>
                        </td>
                        <td class="btns">
                            <adv:Button ID="btnAdd" CssClass="no-pie" CssSpan="btn-add-icon" runat="server" Type="Add" Size="Middle"
                                Text='<%$ Resources:Resource, Client_Catalog_Add %>' Href='<%# UrlService.GetLink(ParamType.Product, SQLDataHelper.GetString(Eval("UrlPath")), Convert.ToInt32(Eval("ProductId"))) %>'
                                Visible='<%# SQLDataHelper.GetDecimal(Eval("Price")) > 0 && SQLDataHelper.GetDecimal(Eval("Amount")) > 0 %>'
                                data-cart-add='<%# Eval("ProductID") %>' />
                            <adv:Button ID="btnOrderByRequest" runat="server" Type="Action" Size="Middle" Text='<%$ Resources:Resource, Client_Catalog_OrderByRequest %>'
                                Href='<%# "sendrequestonproduct.aspx?productid=" + Eval("productId") %>' Visible='<%# ((SQLDataHelper.GetDecimal(Eval("Price")) > 0 && SQLDataHelper.GetInt(Eval("Amount")) <= 0) && SQLDataHelper.GetBoolean(Eval("OrderByRequest"))) %>'
                                Target="_blank" />
                            <adv:Button ID="btnBuy" runat="server" Type="Buy" Size="Middle" Text='<%$ Resources:Resource, Client_More %>'
                                Href='<%# "social/detailssocial.aspx?productId=" + Eval("ProductId") %>' />
                        </td>
                    </tr>
                </ItemTemplate>
                <EmptyItemTemplate>
                    <div class="no-items">
                        <%= Resources.Resource.Client_Catalog_NoItemsFound  %>
                    </div>
                </EmptyItemTemplate>
            </asp:ListView>
        </asp:View>
    </Views>
</asp:MultiView>
