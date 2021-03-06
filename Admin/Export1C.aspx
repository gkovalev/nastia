<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="Export1C.aspx.cs" Inherits="Admin_Export1C" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" Runat="Server">
    <div id="mainDiv" runat="server">
        <center>
            <asp:Label ID="lblAdminHead" runat="server" CssClass="AdminHead" Text="<%$ Resources:Resource, Admin_Export1C_Orders %>"></asp:Label><br />
            <asp:Label ID="lblAdminSubHead" runat="server" CssClass="AdminSubHead" Text="<%$ Resources:Resource, Admin_Export1C_OrdersExport %>"></asp:Label>
            <br />
        </center>

        <br />
        <br />
    
        <div style="text-align: center">
            <asp:Button ID="btnDownload" runat="server" Text="<%$ Resources:Resource, Admin_Export1C_Download %>"  OnClick="btnDownload_Click"/>
            <br />
            <asp:Label ID="lError" runat="server" ForeColor="Blue" Font-Bold="true" Visible="false" EnableViewState="false"></asp:Label>
        </div>
    </div>
    <div id="notInTariff" runat="server" visible="false" class="AdminSaasNotify">
        <center>
        <h2>
            <%= Resources.Resource.Admin_DemoMode_NotAvailableFeature%>
        </h2>
        </center>
    </div>
</asp:Content>
