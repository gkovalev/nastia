<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RelatedProductsSocial.ascx.cs"
    Inherits="UserControls_RelatedProductsSocial" %>
<%@ Register TagPrefix="adv" TagName="Rating" Src="~/UserControls/Rating.ascx" %>
<%@ Import Namespace="AdvantShop.Core.UrlRewriter" %>
<%@ Import Namespace="AdvantShop.Helpers" %>
<div class="pv-tile carousel-default">
    <asp:ListView runat="server" ID="lvRelatedProducts" ItemPlaceholderID="liPlaceHolder">
        <LayoutTemplate>
            <ul class="jcarousel">
                <li runat="server" id="liPlaceHolder"></li>
            </ul>
        </LayoutTemplate>
        <ItemTemplate>
            <li>
                <table class="p-table">
                    <tr>
                        <td class="img-middle">
                            <div class="pv-photo" onclick="location.href='<%# "social/detailssocial.aspx?productId=" + Eval("ProductId") %>'">
                                <%# RenderPictureTag(SQLDataHelper.GetString(Eval("Photo")), SQLDataHelper.GetString(Eval("Name")))%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div>
                                <a href="<%# "social/detailssocial.aspx?productId=" + Eval("ProductId") %>"
                                    class="link-pv-name">
                                    <%# Eval("Name") %></a>
                            </div>
                            <%# SQLDataHelper.GetString(Eval("Name")).Length < 24 ? string.Format("<div class=\"sku\">{0}</div>", Eval("ArtNo")) : string.Empty %>
                            <adv:Rating ID="Rating1" runat="server" />
                            <div class="price-container">
                                <%# RenderPrice(Convert.ToDecimal(Eval("Price")), Convert.ToDecimal(Eval("Discount")))%>
                            </div>
                            <div class="pv-btns">
                                <adv:Button ID="btnAdd" CssSpan="btn-add-icon" runat="server" Type="Add" Size="Middle"
                                    Text='<%$ Resources:Resource, Client_Catalog_Add %>' Href='<%# UrlService.GetLink(ParamType.Product, SQLDataHelper.GetString(Eval("UrlPath")), Convert.ToInt32(Eval("ProductId"))) %>'
                                    Visible='<%# SQLDataHelper.GetDecimal(Eval("Price")) > 0 && SQLDataHelper.GetDecimal(Eval("Amount")) > 0 %>'
                                    Rel='<%# "productid:" + Eval("ProductID") %>' />
                                <adv:Button ID="btnAction" runat="server" Type="Action" Size="Small" Text='<%$ Resources:Resource, Client_Catalog_OrderByRequest %>'
                                    Href='<%# "sendrequestonproduct.aspx?productid=" + Eval("productId") %>' Visible='<%# (!(SQLDataHelper.GetDecimal(Eval("Price")) > 0 && SQLDataHelper.GetInt(Eval("Amount")) > 0) && SQLDataHelper.GetBoolean(Eval("OrderByRequest"))) %>' Target="_blank" />
                                <adv:Button ID="btnBuy" runat="server" Type="Buy" Size="Middle" Text='<%$ Resources:Resource, Client_More %>' Href='<%# "social/detailssocial.aspx?productId=" + Eval("ProductId") %>' />
                            </div>
                        </td>
                    </tr>
                </table>
            </li>
        </ItemTemplate>
    </asp:ListView>
</div>
