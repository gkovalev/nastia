<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="false"
    CodeFile="SiteMapGenerateXML.aspx.cs" Inherits="Admin_SiteMappingXML" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="Server">
    <center>
        <span class="AdminHead">
            <asp:Localize ID="Localize_Admin_SiteMapGenerateXML_Header" runat="server" Text="<%$ Resources:Resource, Admin_SiteMapGenerateXML_Header %>"></asp:Localize></span><br />
        <span class="AdminSubHead">
            <asp:Localize ID="Localize_Admin_SiteMapGenerateXML_SubHeader" runat="server" Text="<%$ Resources:Resource, Admin_SiteMapGenerateXML_SubHeader %>"></asp:Localize></span></center>
    <br />
    <br />
    <center>
        <table border="0" cellpadding="4" cellspacing="0" width="100%" class="catalog_link">
            <tr style="background-color: #eff0f1;">
                <td style="text-align: right; width:50%; font-weight:bold;height:26px;">
                    <asp:Localize ID="Localize_Admin_SiteMapGenerateXML_LastGenerationDate" runat="server"
                        Text="<%$ Resources:Resource, Admin_SiteMapGenerateXML_LastGenerationDate %>"></asp:Localize>
                </td>
                <td>
                    <asp:Label ID="lastMod" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;font-weight:bold">
                    <asp:Localize ID="Localize_Admin_SiteMapGenerateXML_LinkToFile" runat="server" Text="<%$ Resources:Resource, Admin_SiteMapGenerateXML_LinkSiteMap %>"></asp:Localize>
                </td>
                <td>
                    <%= ShowStrLinkToSiteMapFile() %>
                    (<a href="<%= ShowStrLinkToSiteMapFile() %>" target="_blank"><asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:Resource, Admin_SiteMapGenerateXML_LinkSiteMapGo %>"></asp:Localize></a>)
                </td>
            </tr>
            <tr style="background-color: #eff0f1; font-weight:bold">
                <td style="text-align: right; width:50%; font-weight:bold;height:26px;">
                    <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources:Resource, Admin_SiteMapGenerate_Schedule %>" ></asp:Localize>
                </td>
                <td>
                    <a href="CommonSettings.aspx#tabid=task"><asp:Localize ID="Localize3" runat="server" Text="<%$ Resources:Resource, Admin_SiteMapGenerate_Set %>"></asp:Localize></a> 
                </td>
            </tr>
        </table>
        <br />
        <br />
        <asp:Button ID="btnCreateMap" runat="server" Text="<%$ Resources:Resource,Admin_SiteMapGenerateXML_Generate %>"
            OnClick="btnCreateMap_Click" />
        <br />
        <br />
        <asp:Label ID="lblErr" runat="server" Text="" ForeColor="Blue"></asp:Label>
    </center>
</asp:Content>
