<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RitmzSettings.ascx.cs"
    Inherits="Admin_UserControls_Settings_RitmzSettings" %>
<table border="0" cellpadding="2" cellspacing="0">
    <%--<tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <asp:Localize ID="Localize7" runat="server" Text="<%$ Resources: Resource, Admin_UserControl_RitmzSettings_Head %>"></asp:Localize></span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>--%>
    <tr>
        <td style="width: 100px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources: Resource, Admin_UserControl_RitmzSettings_Login %>"></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:TextBox ID="txtRitmzLogin" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td style="width: 100px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize21" runat="server" Text="<%$ Resources: Resource, Admin_UserControl_RitmzSettings_Password %>"></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:TextBox ID="txtRitmzPassword" runat="server"></asp:TextBox>
        </td>
    </tr>   
</table>
