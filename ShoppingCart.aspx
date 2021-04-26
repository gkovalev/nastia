<%@ Page Language="C#" MasterPageFile="MasterPage.master" CodeFile="ShoppingCart.aspx.cs"
    Inherits="ShoppingCart_Page" %>

<%@ MasterType VirtualPath="MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div class="stroke">
        <div class="content-owner">
            <h1>
                <%= Resources.Resource.Client_ShoppingCart_ShoppingCart %></h1>
            <div id="cartWrapper" class="cart-wrapper">
                <div id="dvOrderMerged" runat="server" visible="false" class="ShoppingCart_MergedOrder">
                    <asp:Localize ID="Localize_Client_ShoppingCart_ProductsInBasket" runat="server" Text="<%$ Resources:Resource, Client_ShoppingCart_ProductsInBasket %>"></asp:Localize>
                </div>
                <div data-plugin="cart"></div>
            </div>
            <div class="btn-cart-confirm">
                <adv:Button ID="aCheckOut" runat="server" Type="Confirm" Size="Big"
                    Text="<%$ Resources:Resource, Client_ShoppingCart_DrawUp %>" OnClick="btnConfirmOrder_Click" />
            </div>
            <asp:Label ID="lDemoWarning" runat="server" CssClass="warn" Text="<%$ Resources:Resource, Client_ShoppingCart_FakeShop %>" />
        </div>
    </div>
    <asp:Label ID="lblEmpty" runat="server" Text="" Visible="False" />
</asp:Content>
