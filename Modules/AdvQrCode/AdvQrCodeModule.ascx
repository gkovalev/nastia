<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AdvQrCodeModule.ascx.cs"
    Inherits="Advantshop.UserControls.Modules.Admin_AdvQrCodeModule" %>
<div style="text-align: center;">
        <table border="0" cellpadding="2" cellspacing="0">
            <tr class="rowsPost">
                <td colspan="2" style="height: 34px;">
                    <span class="spanSettCategory">
                        <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources: QrCode_Header%>" /></span>
                    <asp:Label ID="lblMessage" runat="server" Visible="False" Style="float: right;"></asp:Label>
                    <hr color="#C2C2C4" size="1px" />
                </td>
            </tr>
            <tr class="rowsPost">
                <td style="width: 250px; text-align: left;">
                    <asp:Localize ID="Localize10" runat="server" Text="<%$ Resources: QrCode_Active%>"></asp:Localize>
                </td>
                <td>
                    <asp:CheckBox runat="server" ID="ckbEnableQrCode" />
                </td>
            </tr>
              <tr>
                <td colspan="2">
                    <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="<%$ Resources: QrCode_Save%>" />
                </td>
            </tr>
        </table>
</div>
