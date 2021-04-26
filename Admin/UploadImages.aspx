<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="UploadImages.aspx.cs" Inherits="Admin_UploadImages" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="Server">
    <center>
        <asp:Label ID="lblAdminHead" runat="server" CssClass="AdminHead" Text="<%$ Resources:Resource,Admin_UploadImages_Header %>"></asp:Label><br />
        <asp:Label ID="lblAdminSubHead" runat="server" CssClass="AdminSubHead" Text="<%$ Resources:Resource,Admin_UploadImages_SubHeader %>"></asp:Label>
        <br />
        <br />
        <%= Resources.Resource.Admin_UploadImages_SourceFolder %>
        <br />
        <asp:TextBox ID="txtSource" runat="server" Text="~/upload_images" Width="213px"></asp:TextBox>
        <br />
        <br />
        <asp:Button ID="btnUpload" runat="server" Text="<%$ Resources:Resource,Admin_UploadImages_Upload %>"
            OnClick="btnUpload_Click" />
        <br />
        <br />
        <br />
        <div style="width: 65%; text-align: left">
            <asp:Label ID="lError" runat="server" ForeColor="Blue" Font-Bold="true" Visible="false"
                EnableViewState="false" Style="text-align: left"></asp:Label>
        </div>
    </center>
</asp:Content>
