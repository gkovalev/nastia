<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProfitSettings.ascx.cs"
    Inherits="Admin_UserControls_Settings_ProfitSettings" %>
<table border="0" cellpadding="2" cellspacing="0">
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <asp:Localize ID="Localize_Admin_CommonSettings_HeadProfitability" runat="server"
                    Text="<%$ Resources:Resource, Admin_CommonSettings_HeadProfitability %>"></asp:Localize></span>
            <hr style="color: #C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <asp:Localize ID="Localize8" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_SalesPlan %>"></asp:Localize>
            <br />
        </td>
        <td>
            &nbsp;<asp:TextBox ID="txtSalesPlan" runat="server" class="textBoxClass"></asp:TextBox>
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_ProfitPlan %>"></asp:Localize>
            <br />
        </td>
        <td>
            &nbsp;<asp:TextBox ID="txtProfitPlan" runat="server" class="textBoxClass"></asp:TextBox>
        </td>
    </tr>
</table>
