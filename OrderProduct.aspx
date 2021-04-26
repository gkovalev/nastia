<%@ Page Language="C#" MasterPageFile="MasterPage.master" AutoEventWireup="true"
    CodeFile="OrderProduct.aspx.cs" Inherits="OrderProduct" Title="Untitled Page"
    EnableViewState="false" %>

<%@ MasterType VirtualPath="MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <h1>
        Покупка товара под заказ</h1>
    <br />
    <span class="ContentText">
        <asp:Label ID="lblMessage" runat="server"></asp:Label>
    </span>
    <a href="<%= GetMainPageLink() %>" >На главную</a>
</asp:Content>
