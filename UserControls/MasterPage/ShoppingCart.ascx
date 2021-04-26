<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ShoppingCart.ascx.cs"
    Inherits="UserControls_ShoppingCart" %>
<div class="top-panel-cart minicart" data-plugin="cart" data-cart-options="{type:'mini'}">
    <%= Resources.Resource.Client_ShoppingCart_ShoppingCart%>: <span data-cart-count="true" class="minicart-count"><%= Count %></span>
</div>
       

	