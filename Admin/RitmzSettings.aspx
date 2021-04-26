<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="RitmzSettings.aspx.cs" Inherits="Admin_RitmzSettings" %>

<%@ Register Src="~/Admin/UserControls/Modules/RitmzSettings.ascx" TagName="RitmzSettings"
    TagPrefix="adv" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div style="text-align: center;">
        <asp:Label ID="lblAdminHead" runat="server" CssClass="AdminHead" Text="<%$ Resources:Resource, Admin_RitmzSettings_Header %>"></asp:Label><br />
        <asp:Label ID="lblAdminSubHead" runat="server" CssClass="AdminSubHead" Text="<%$ Resources:Resource, Admin_RitmzSettings_SubHeader %>"></asp:Label>
        <br />
        <br />
        <br />
        <adv:RitmzSettings runat="server" ID="RitmzSettings" />
        <br />
        <asp:Button runat="server" ID="btnSave" OnClick="btnSave_Click" Text="<%$ Resources:Resource, Admin_RitmzSettings_Save %>" />
    </div>
</asp:Content>
