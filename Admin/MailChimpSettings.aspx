<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="MailChimpSettings.aspx.cs" Inherits="Admin_MailChimpSettings" %>

<%@ Register Src="~/Admin/UserControls/Modules/MailChimpSettings.ascx" TagName="MailChimpSettings"
    TagPrefix="adv" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <center>
        <asp:Label ID="lblAdminHead" runat="server" CssClass="AdminHead" Text="<%$ Resources:Resource, Admin_MailChimpSettings_Header %>"></asp:Label><br />
        <asp:Label ID="lblAdminSubHead" runat="server" CssClass="AdminSubHead" Text="<%$ Resources:Resource, Admin_MailChimpSettings_SubHeader %>"></asp:Label>
        <br />
        <br />
        <br />
        <adv:MailChimpSettings runat="server" ID="MailChimpSettings" />
        <br />
        <asp:Button runat="server" ID="btnSave" OnClick="btnSave_Click" Text="<%$ Resources: resource, Admin_MailChimpSettings_Update %>" />
    </center>
</asp:Content>
