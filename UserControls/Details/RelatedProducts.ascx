<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RelatedProducts.ascx.cs"
    Inherits="UserControls_RelatedProducts" %>
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
                            <div class="pv-photo" onclick="location.href='<%# UrlService.GetLink(ParamType.Product, Eval("UrlPath").ToString(), Convert.ToInt32(Eval("ProductId"))) %>'">
                                <%# RenderPictureTag(SQLDataHelper.GetString(Eval("Photo")), SQLDataHelper.GetString(Eval("Name")), SQLDataHelper.GetString(Eval("PhotoDesc")))%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div>
                                <a href="<%# UrlService.GetLink(ParamType.Product, Eval("UrlPath").ToString(), Convert.ToInt32(Eval("ProductId"))) %>"
                                    class="link-pv-name">
                                    <%# Eval("Name") %></a>
                            </div>
                            <div class="sku"><%# Eval("ArtNo") %></div>
                            <adv:Rating ID="Rating1" runat="server" />
                            <div class="price-container">
                                    <%# RenderPrice(Convert.ToDecimal(Eval("Price")), Convert.ToDecimal(Eval("Discount")))%>
                            </div>
                            <div class="pv-btns">
                                <adv:Button ID="btnAdd"  data-cart-add='<%# Eval("ProductID") %>' runat="server" Type="Add" Size="Small" Text='<%$ Resources:Resource, Client_Catalog_Add %>'
                                    Visible='<%# SQLDataHelper.GetDecimal(Eval("Price")) > 0 && SQLDataHelper.GetDecimal(Eval("Amount")) > 0 %>' />
                                <adv:Button ID="btnAction" runat="server" Type="Action" Size="Small" Text='<%$ Resources:Resource, Client_Catalog_OrderByRequest %>'
                                    Href='<%# "sendrequestonproduct.aspx?productid=" + Eval("productId") %>' Visible='<%# (!(SQLDataHelper.GetDecimal(Eval("Price")) > 0 && SQLDataHelper.GetInt(Eval("Amount")) > 0) && SQLDataHelper.GetBoolean(Eval("OrderByRequest"))) %>' />
                                <adv:Button ID="btnBuy" runat="server" Type="Buy" Size="Small" Text='<%$ Resources:Resource, Client_More %>'
                                    Href='<%# UrlService.GetLink(ParamType.Product, Eval("UrlPath").ToString(), Convert.ToInt32(Eval("ProductId"))) %>' />
                            </div>
                        </td>
                    </tr>
                </table>
            </li>
        </ItemTemplate>
    </asp:ListView>
</div>
