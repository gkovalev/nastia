<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Search.ascx.cs" Inherits="UserControls_MasterPage_Search" %>
<div class="search">
    <adv:AdvTextBox runat="server" ID="txtSearch" CssClass="search-text autocompleteSearch"
        Placeholder="<%$ Resources:Resource, Client_MasterPage_Search %>" DefaultButtonID="btnGoSearch"  />
    <adv:Button runat="server" ID="btnGoSearch" Text="<%$ Resources:Resource, Client_MasterPage_Find %>" DisableValidation="True" CssClass="btn-search"
        OnClientClick="searchNow(); " />
</div>