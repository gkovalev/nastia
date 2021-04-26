<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="false"
    CodeFile="SiteMapGenerate.aspx.cs" Inherits="Admin_SiteMapping" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="Server">
    <center>
        <span class="AdminHead">
            <asp:Localize ID="Localize_Admin_SiteMapGenerate_Header" runat="server" Text="<%$ Resources:Resource, Admin_SiteMapGenerate_Header %>"></asp:Localize></span><br />
        <span class="AdminSubHead">
            <asp:Localize ID="Localize_Admin_SiteMapGenerate_SubHead" runat="server" Text="<%$ Resources:Resource, Admin_SiteMapGenerate_SubHead %>"></asp:Localize></span></center>
    <br />
    <br />
    <center>
        <table border="0" cellpadding="4" cellspacing="0" width="100%" class="catalog_link">
            <tr style="background-color: #eff0f1;" >
                <td style="text-align: right; width:50%; font-weight:bold;height:26px;">
                    <asp:Localize ID="Localize_Admin_SiteMapGenerate_ModDateSiteMap" runat="server" Text="<%$ Resources:Resource, Admin_SiteMapGenerate_ModDateSiteMap %>"></asp:Localize>
                </td>
                <td>
                    <asp:Label ID="lastMod" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width:50%; font-weight:bold;height:26px;">
                    <asp:Localize ID="Localize_Admin_SiteMapGenerate_LinkSiteMap" runat="server" Text="<%$ Resources:Resource, Admin_SiteMapGenerate_LinkSiteMap %>"></asp:Localize>
                </td>
                <td>
                    <%= ShowStrLinkToSiteMapFile() %>
                    (<a href="<%= ShowStrLinkToSiteMapFile() %>" target="_blank"><asp:Localize ID="Localize_Admin_SiteMapGenerate_LinkSiteMapGo" runat="server" Text="<%$ Resources:Resource, Admin_SiteMapGenerate_LinkSiteMapGo %>"></asp:Localize></a>)
                </td>
            </tr>
            <tr style="background-color: #eff0f1; font-weight:bold">
                <td style="text-align: right; width:50%; font-weight:bold;height:26px;">
                    <asp:Localize runat="server" Text="<%$ Resources:Resource, Admin_SiteMapGenerate_Schedule %>" ></asp:Localize>
                </td>
                <td>
                    <a href="CommonSettings.aspx#tabid=task"><asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:Resource, Admin_SiteMapGenerate_Set %>"></asp:Localize></a> 
                </td>
            </tr>
        </table>
        <br />
        <br />
        <asp:Button ID="btnCreateMap" runat="server" OnClick="createMap_Click" Text="<%$ Resources:Resource,Admin_SiteMapGenerate_ButtonGenerate%>" />
        
        <br />
        <br />
        <asp:Label ID="lblError" runat="server" ForeColor="Blue" Visible="False"></asp:Label>
    </center>
</asp:Content>
