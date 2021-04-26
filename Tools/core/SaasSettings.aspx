<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SaasSettings.aspx.cs" Inherits="Tools_core_SaasSettings"
    MasterPageFile="MasterPage.master" %>

<%@ Register Src="../../Admin/UserControls/Dashboard/CurrentSaasData.ascx" TagName="CurrentSaasData"
    TagPrefix="uc1" %>
<asp:Content runat="server" ID="cntHead" ContentPlaceHolderID="head">
    <title>AdvantShop.NET Core Tools - Saas Settings</title>
</asp:Content>
<asp:Content runat="server" ID="cntMain" ContentPlaceHolderID="main">
    <div>
        <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click">GetCurrentSaasId</asp:LinkButton>
        <br />
        <br />
        Current SaasId:
        <asp:TextBox ID="txtCurrentSaasId" runat="server" Text="" Width="400px"></asp:TextBox>&nbsp;
        <asp:Button ID="btnSetSaasId" runat="server" Text="Set Saas Id" OnClick="btnSetSaasId_Click" />
        <uc1:CurrentSaasData ID="CurrentSaasData1" runat="server" />
    </div>
</asp:Content>
