<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CustomOptions.ascx.cs"  Inherits="UserControls_CustomOptions" %>
<%@ Import Namespace="AdvantShop.Catalog" %>
<asp:Panel ID="panel" runat="server">
</asp:Panel>
<input type="hidden" id="<%= "customOptionsHidden_"+ ProductId %>" value="<%= HttpUtility.UrlEncode(CustomOptionsService.SerializeToXml(CustomOptions, SelectedOptions)) %>" />