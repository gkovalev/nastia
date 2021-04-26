<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PaymentReturnUrl.aspx.cs" Inherits="PaymentReturnUrl" %>
<%@ Register TagPrefix="adv" TagName="StaticBlock" Src="~/UserControls/StaticBlock.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="Server">
    <div class="stroke">
        <div class="content-owner">
            <asp:Label runat="server" ID="lblResult"></asp:Label>
            <adv:StaticBlock ID="StaticBlock1" runat="server" SourceKey="PaymentReturn" />
            <asp:HyperLink ID="Hyperlink1" NavigateUrl="." runat="server" Text="<%$ Resources: Resource, Client_ShoppingCart_ToMain%>"></asp:HyperLink>
        </div>
    </div>
</asp:Content>