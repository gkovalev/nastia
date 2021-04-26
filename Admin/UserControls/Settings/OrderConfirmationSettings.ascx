<%@ Control Language="C#" AutoEventWireup="true" CodeFile="OrderConfirmationSettings.ascx.cs"
    Inherits="Admin_UserControls_Settings_SettingsOrderConfirmation" %>
<table border="0" cellpadding="2" cellspacing="0">
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <asp:Localize ID="Localize_Admin_CommonSettings_HeadOrderConfirmation" runat="server"
                    Text="<%$ Resources:Resource, Admin_CommonSettings_HeadOrderConfirmation %>"></asp:Localize></span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr id="Tr1" class="rowsPost" runat="server" visible="false">
        <td style="width: 250px; text-align: left;">
            <asp:Localize ID="Localize_Admin_CommonSettings_UserGID" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_UserGID %>"></asp:Localize>
            <br />
        </td>
        <td>
            &nbsp;<asp:TextBox ID="txtQRUserID" runat="server" class="textBoxClass"></asp:TextBox>
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <asp:Localize ID="Localize_Admin_CommonSettings_UseAmountLimit" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_UseAmountLimit %>"></asp:Localize>
            <br />
        </td>
        <td>
            &nbsp;<asp:CheckBox ID="cbAmountLimitation" runat="server" />
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <asp:Localize ID="Localize5" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_CheckConfirmCode %>"></asp:Localize>
            <br />
        </td>
        <td>
            &nbsp;<asp:CheckBox ID="ckbEnableCheckConfirmCode" runat="server" />
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_MinimalPrice %>"></asp:Localize>
            <br />
        </td>
        <td>
            &nbsp;<asp:TextBox ID="txtMinimalPrice" runat="server" class="textBoxClass"></asp:TextBox>
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <asp:Localize ID="Localize6" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_GiftCertificates %>"></asp:Localize></span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <asp:Localize ID="Localize3" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_MinimalPriceCertificate %>"></asp:Localize>
            <br />
        </td>
        <td>
            &nbsp;<asp:TextBox ID="txtMinimalPriceCertificate" runat="server" class="textBoxClass"></asp:TextBox>
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_MaximalPriceCertificate %>"></asp:Localize>
            <br />
        </td>
        <td>
            &nbsp;<asp:TextBox ID="txtMaximalPricecertificate" runat="server" class="textBoxClass"></asp:TextBox>
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <asp:Localize ID="Localize4" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_EnableGiftCertificateService %>"></asp:Localize>
            <br />
        </td>
        <td>
            &nbsp;<asp:CheckBox ID="ckbEnableGiftCertificateService" runat="server" />
        </td>
    </tr>
        <tr class="rowsPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <asp:Localize ID="Localize12" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_BuyInOneclick_Header %>"></asp:Localize></span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr>
        <td style="width: 250px; text-align: left;">
            <asp:Localize ID="Localize11" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_BuyInOneclick_Active %>"></asp:Localize>
        </td>
        <td>
            <asp:CheckBox runat="server" ID="ckbBuyInOneClick" />
        </td>
    </tr>
    <tr>
        <td style="width: 250px; text-align: left;">
            <asp:Localize ID="Localize13" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_BuyInOneclick_FirstText %>"></asp:Localize>
        </td>
        <td>
            <asp:TextBox ID="txtFirstText" runat="server" class="textBoxClass"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td style="width: 250px; text-align: left;">
            <asp:Localize ID="Localize14" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_BuyInOneclick_SecondText %>"></asp:Localize>
        </td>
        <td>
            <asp:TextBox ID="txtSecondText" runat="server" class="textBoxClass"></asp:TextBox>
        </td>
    </tr>
</table>
