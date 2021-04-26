<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MenuTopSocial.ascx.cs" Inherits="UserControls_MenuTop"
    EnableViewState="false" %>
<%@ Register Src="SearchSocial.ascx" TagName="SearchSocial" TagPrefix="adv" %>
<div class="main-menu">
    <%= GetHtml() %>
    <adv:SearchSocial runat="server" ID="searchBlock" />
</div>
