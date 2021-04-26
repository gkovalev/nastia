<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NewsSettings.ascx.cs"
    Inherits="Admin_UserControls_Settings_NewsSettings" %>
<table>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <h4 style="display: inline; font-size: 10pt;">
                <asp:Localize ID="Localize17" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_HeadNewsOther%>"></asp:Localize></h4>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr>
        <td style="width: 30%; height: 26px; vertical-align: middle;">
            <asp:Label runat="server" Text="<%$ Resources: Resource, Admin_CommonSettings_NewsPerPage %>"></asp:Label>
        </td>
        <td style="vertical-align: middle; height: 26px;">
            <asp:TextBox runat="server" ID="txtNewsPerPage"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td style="width: 30%; height: 26px; vertical-align: middle;">
            <asp:Label ID="Label1" runat="server" Text="<%$ Resources: Resource, Admin_CommonSettings_MainPageNews %>"></asp:Label>
        </td>
        <td style="vertical-align: middle; height: 26px;">
            <asp:TextBox runat="server" ID="txtNewsMainPageCount"></asp:TextBox>
        </td>
    </tr>
</table>
