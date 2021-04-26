<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="PriceRegulation.aspx.cs" Inherits="Admin_PriceRegulation" %>
<%@ Register Src="~/Admin/UserControls/Dashboard/ChangePrice.ascx" TagName="ChangePrice" TagPrefix="adv" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div id="mainDiv" runat="server">
        <center>
            <asp:Label ID="lblAdminHead" runat="server" CssClass="AdminHead" Text="<%$ Resources:Resource, Admin_PriceRegulation_Header %>"></asp:Label><br />
            <asp:Label ID="lblAdminSubHead" runat="server" CssClass="AdminSubHead" Text="<%$ Resources:Resource, Admin_PriceRegulation_SubHeader %>"></asp:Label>
            <br />
        </center>
        <br />
        <br />
        <div style="text-align: center">
            <adv:ChangePrice ID="ChangePrice" runat="server" />
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
