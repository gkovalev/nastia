<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NotifyEmailsSettings.ascx.cs"
    Inherits="Admin_UserControls_Settings_NotifyEmailsSettings" %>
<table border="0" cellpadding="2" cellspacing="0">
    <tr class="rowsPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <asp:Localize ID="Localize_Admin_CommonSettings_NotifyEmails" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_NotifyEmails %>"></asp:Localize></span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <asp:Localize ID="Localize_Admin_CommonSettings_EmailForReports" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_EmailForReports %>"></asp:Localize>
            <br />
        </td>
        <td>
            &nbsp;<asp:TextBox ID="txtEmailRegReport" runat="server" class="textBoxClass"></asp:TextBox>
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <asp:Localize ID="Localize_Admin_CommonSettings_EmailForOrders" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_EmailForOrders %>"></asp:Localize>
            <br />
        </td>
        <td>
            &nbsp;<asp:TextBox ID="txtOrderEmail" runat="server" class="textBoxClass"></asp:TextBox>
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <asp:Localize ID="Localize_Admin_CommonSettings_EmailForComment" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_EmailForComment %>"></asp:Localize>
            <br />
        </td>
        <td>
            &nbsp;<asp:TextBox ID="txtEmailProductDiscuss" runat="server" class="textBoxClass"></asp:TextBox>
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <asp:Localize ID="Localize_Admin_CommonSettings_EmailForFeedBack" runat="server"
                Text="<%$ Resources:Resource, Admin_CommonSettings_EmailForFeedBack %>"></asp:Localize>
            <br />
        </td>
        <td>
            &nbsp;<asp:TextBox ID="txtFeedbackEmail" runat="server" class="textBoxClass"></asp:TextBox>
        </td>
    </tr>
</table>
