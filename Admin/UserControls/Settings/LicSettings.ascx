<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LicSettings.ascx.cs" Inherits="Admin_UserControls_Settings_LicSettings" %>
<table border="0" cellpadding="2" cellspacing="0">
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <h4 style="display: inline; font-size: 10pt;">
                <asp:Localize ID="Localize5" runat="server" Text="<%$ Resources:Resource, Admin_UserControls_Settings_LicSettings_header %>" /></h4>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr style="background-color: #eff0f1;">
        <td style="width: 30%; height: 26px; text-align: left; vertical-align: top; padding-top: 5px;">
            <asp:Localize ID="Localize10" runat="server" Text="<%$ Resources:Resource, Admin_UserControls_Settings_LicSettings_LicStatus %>" />
        </td>
        <td style="vertical-align: top; height: 26px;">
            <asp:Label runat="server" ID="lblState"></asp:Label>
        </td>
    </tr>
    <tr>
        <td style="width: 30%; height: 29px; text-align: left; vertical-align: top; padding-top: 3px;">
            <asp:Localize ID="Localize3" runat="server" Text="<%$ Resources:Resource, Admin_UserControls_Settings_LicSettings_LicKey %>" />
        </td>
        <td style="vertical-align: top; height: 29px;">
            <asp:TextBox ID="txtLicKey" runat="server" Width="384" CssClass="niceTextBox shortTextBoxClass" Text="" />
            <asp:Button runat="server" ID="btnCheakLic" OnClick="btnCheakLic_Click" Text="<%$ Resources:Resource, Admin_UserControls_Settings_LicSettings_Check %>" />
        </td>
    </tr>
</table>
