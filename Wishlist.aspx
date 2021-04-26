<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeFile="Wishlist.aspx.cs" AutoEventWireup="true" 
    Inherits="Wishlist" EnableViewState="true" %>
<%@ Import Namespace="AdvantShop.Catalog" %>
<%@ Import Namespace="AdvantShop.Core.UrlRewriter" %>
<%@ Import Namespace="AdvantShop.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div class="stroke">
        <div class="content-owner">
        <h1>
            <asp:Localize ID="Localize_Client_Wishlist" runat="server" Text="<%$ Resources:Resource, Client_Wishlist %>" /></h1>
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
                            <%# RenderPictureTag(SQLDataHelper.GetString(Eval("Product.Photo")), SQLDataHelper.GetString(Eval("Product.Name")), UrlService.GetLink(ParamType.Product, Eval("Product.UrlPath").ToString(), Convert.ToInt32(Eval("Product.ProductId"))))%>
                        </div>
                        <%# CatalogService.RenderLabels(Convert.ToBoolean(Eval("Product.Recomended")), Convert.ToBoolean(Eval("Product.OnSale")), Convert.ToBoolean(Eval("Product.Bestseller")), Convert.ToBoolean(Eval("Product.New")), SQLDataHelper.GetDecimal(Eval("Product.Discount")))%>
                    </div>
                    <div class="pv-info">
                        <a href="<%# UrlService.GetLink(ParamType.Product, Eval("Product.UrlPath").ToString(), Convert.ToInt32(Eval("Product.ProductId"))) %>"
                            class="link-pv-name">
                            <%# Eval("Product.Name")%></a>
                        <div class="sku">
                            <%# Eval("Product.ArtNo")%></div>
                        <%# CatalogService.RenderSelectedOptions(Eval("AttributesXml").ToString()) %>
                       <%# !string.IsNullOrWhiteSpace(Eval("Product.BriefDescription").ToString()) ? "<div class=\"descr\">" + Eval("Product.BriefDescription") + "</div>" : string.Empty%>
                        <div class="price-container">
                            <%# CatalogService.RenderPrice(Convert.ToDecimal(Eval("Price")), Convert.ToDecimal(Eval("Product.Discount")), true, customerGroup)%>
                        </div>
                        <div class="pv-btns">
                            <adv:Button ID="btnAdd" runat="server" Type="Add" Size="Small" Text='<%$ Resources:Resource, Client_Catalog_Add %>'
                                OnClientClick='<%# "$(\"#hfWishListItemID\").val(" + SQLDataHelper.GetInt(Eval("ItemId")) + ");__doPostBack(\"" + lbAddTocart.UniqueID + "\",\"\")" %>'
                                Visible='<%# SQLDataHelper.GetDecimal(Eval("Product.Price")) > 0 && SQLDataHelper.GetDecimal(Eval("Product.Amount")) > 0 %>' />
                            <adv:Button ID="btnOrderByRequest" runat="server" Type="Action" Size="Small" Text='<%$ Resources:Resource, Client_Catalog_OrderByRequest %>'
                                Href='<%# "sendrequestonproduct.aspx?productid=" + Eval("Product.ProductId") %>' 
                                Visible='<%# ((SQLDataHelper.GetDecimal(Eval("Product.Price")) > 0  && SQLDataHelper.GetInt(Eval("Product.Amount")) == 0) && SQLDataHelper.GetBoolean(Eval("Product.OrderByRequest"))) %>' />
                            <adv:Button ID="btnBuy" runat="server" Type="Buy" Size="Small" Text='<%$ Resources:Resource, Client_More %>'
                                Href='<%# UrlService.GetLink(ParamType.Product, Eval("Product.UrlPath").ToString(), Convert.ToInt32(Eval("Product.ProductId"))) %>' />
                        </div>
                    </div>
                    <div class="wishlist-cross">
                        <a class="cross" onclick="<%# "$('#hfWishListItemID').val(" + SQLDataHelper.GetInt(Eval("ItemId")) + ");__doPostBack('"+  lbDelete.UniqueID + "', '');" %>" href="javascript:void(0);"></a>
                    </div>
                </div>
            </ItemTemplate>
            <EmptyDataTemplate>
                <div class="no-items">
                    <%= Resources.Resource.Client_WishList_NoRecords %>
                </div>
            </EmptyDataTemplate>
        </asp:ListView>
        <br class="clear" />
        </div>
    </div>
    <br />
    <asp:HiddenField runat="server" ID="hfWishListItemID"/>
    <asp:LinkButton runat="server" ID="lbDelete" OnClick="btnDeleteClick"></asp:LinkButton>
    <asp:LinkButton runat="server" ID="lbAddTocart" OnClick="btnAddToCartClick"></asp:LinkButton>
</asp:Content>
