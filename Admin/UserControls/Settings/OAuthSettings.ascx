<%@ Control Language="C#" AutoEventWireup="true" CodeFile="OAuthSettings.ascx.cs"
    Inherits="Admin_UserControls_Settings_OAuthSettings" %>
<table runat="server" id="tableGoogle" style="width: 600px;">
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <h4 style="display: inline; font-size: 10pt;">
                <asp:Localize ID="Localize16" runat="server" Text="Google"></asp:Localize></h4>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr style="background-color: #eff0f1;">
        <td style="width: 30%; height: 26px; text-align: left; vertical-align: top; padding-top: 5px;">
            <asp:Localize ID="lclzGoogleActive" runat="server" Text="<%$ Resources: Resource, Admin_CommonSettings_HeadOAuthActive %>"></asp:Localize>
        </td>
        <td style="vertical-align: top; height: 26px;">
            <asp:CheckBox ID="ckbGoogleActive" runat="server" />
        </td>
    </tr>
</table>
<br />
<table runat="server" id="tableYandex" style="width: 600px;">
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <h4 style="display: inline; font-size: 10pt;">
                <asp:Localize ID="Localize1" runat="server" Text="Yandex"></asp:Localize></h4>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr>
        <td style="width: 30%; height: 29px; text-align: left; vertical-align: top; padding-top: 3px;">
            <asp:Localize ID="lclzYandexActive" runat="server" Text="<%$ Resources: Resource, Admin_CommonSettings_HeadOAuthActive %>"></asp:Localize>
        </td>
        <td style="vertical-align: top; height: 29px;">
            <asp:CheckBox ID="ckbYandexActive" runat="server" />
        </td>
    </tr>
</table>
<br />
<table runat="server" id="tableMailru" style="width: 600px;">
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <h4 style="display: inline; font-size: 10pt;">
                <asp:Localize ID="Localize2" runat="server" Text="Mail.ru"></asp:Localize></h4>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr style="background-color: #eff0f1;">
        <td style="width: 30%; height: 26px; text-align: left; vertical-align: top; padding-top: 3px;">
            <asp:Localize ID="lclzMailActive" runat="server" Text="<%$ Resources: Resource, Admin_CommonSettings_HeadOAuthActive %>"></asp:Localize>
        </td>
        <td style="vertical-align: top; height: 26px;">
            <asp:CheckBox ID="ckbMailActive" runat="server" />
        </td>
    </tr>
</table>
<br />
<table runat="server" id="tableVk" style="width: 600px;">
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <h4 style="display: inline; font-size: 10pt;">
                <asp:Localize ID="Localize17" runat="server" Text="Vk.com"></asp:Localize></h4>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowPost">
        <td style="width: 30%; height: 26px; text-align: left; vertical-align: top; padding-top: 3px;">
            <asp:Localize ID="Localize3" runat="server" Text="<%$ Resources: Resource, Admin_CommonSettings_HeadOAuthActive %>"></asp:Localize>
        </td>
        <td style="vertical-align: top; height: 26px;">
            <asp:CheckBox ID="ckbVKActive" runat="server" />
        </td>
    </tr>
    <tr class="rowPost">
        <td style="width: 30%; height: 26px; text-align: left; vertical-align: middle;">
            <asp:Label runat="server" Text="<%$ Resources: Resource, Admin_CommonSettings_HeadOAuthVKappid %>"></asp:Label>
        </td>
        <td style="vertical-align: middle; height: 26px;">
            <asp:TextBox class="niceTextBox shortTextBoxClass" runat="server" ID="txtVKAppId"
                Width="300"></asp:TextBox>
        </td>
    </tr>
    <tr class="rowPost">
        <td style="width: 30%; height: 26px; text-align: left; vertical-align: middle;">
            <asp:Label runat="server" Text="<%$ Resources: Resource, Admin_CommonSettings_HeadOAuthVKSecretKey %>"></asp:Label>
        </td>
        <td style="vertical-align: middle; height: 26px;">
            <asp:TextBox class="niceTextBox shortTextBoxClass" runat="server" ID="txtVKSecret"
                Width="300"></asp:TextBox>
        </td>
    </tr>
</table>
<br />
<table runat="server" id="tableFacebook" style="width: 600px;">
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <h4 style="display: inline; font-size: 10pt;">
                <asp:Localize ID="Localize4" runat="server" Text="Facebook"></asp:Localize></h4>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowPost">
        <td style="width: 30%; height: 26px; text-align: left; vertical-align: top; padding-top: 3px;">
            <asp:Localize ID="Localize5" runat="server" Text="<%$ Resources: Resource, Admin_CommonSettings_HeadOAuthActive %>"></asp:Localize>
        </td>
        <td style="vertical-align: top; height: 26px;">
            <asp:CheckBox ID="ckbFacebookActive" runat="server" />
        </td>
    </tr>
    <tr class="rowPost">
        <td style="width: 30%; height: 26px; text-align: left; vertical-align: middle;">
            <asp:Label ID="Label1" runat="server" Text="<%$ Resources: Resource, Admin_CommonSettings_OAuthFbApiKey %>"></asp:Label>
        </td>
        <td style="vertical-align: middle; height: 26px;">
            <asp:TextBox class="niceTextBox shortTextBoxClass" runat="server" ID="txtFacebookClientId"
                Width="300"></asp:TextBox>
        </td>
    </tr>
    <tr class="rowPost">
        <td style="width: 30%; height: 26px; text-align: left; vertical-align: middle;">
            <asp:Label ID="Label2" runat="server" Text="<%$ Resources: Resource, Admin_CommonSettings_OAuthFbApiSecret %>"></asp:Label>
        </td>
        <td style="vertical-align: middle; height: 26px;">
            <asp:TextBox class="niceTextBox shortTextBoxClass" runat="server" ID="txtFacebookApplicationSecret"
                Width="300"></asp:TextBox>
        </td>
    </tr>
</table>
<br />
<table runat="server" id="tableTwitter" style="width: 600px;">
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <h4 style="display: inline; font-size: 10pt;">
                <asp:Localize ID="Localize6" runat="server" Text="Twitter"></asp:Localize></h4>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowPost">
        <td style="width: 30%; height: 26px; text-align: left; vertical-align: top; padding-top: 3px;">
            <asp:Localize ID="Localize7" runat="server" Text="<%$ Resources: Resource, Admin_CommonSettings_HeadOAuthActive %>"></asp:Localize>
        </td>
        <td style="vertical-align: top; height: 26px;">
            <asp:CheckBox ID="ckbTwitterActive" runat="server" />
        </td>
    </tr>
    <tr class="rowPost">
        <td style="width: 30%; height: 26px; text-align: left; vertical-align: middle;">
            <asp:Label ID="Label3" runat="server" Text="<%$ Resources: Resource, Admin_CommonSettings_OAuthTwConsumerKey %>"></asp:Label>
        </td>
        <td style="vertical-align: middle; height: 26px;">
            <asp:TextBox class="niceTextBox shortTextBoxClass" runat="server" ID="txtTwitterConsumerKey"
                Width="300"></asp:TextBox>
        </td>
    </tr>
    <tr class="rowPost">
        <td style="width: 30%; height: 26px; text-align: left; vertical-align: middle;">
            <asp:Label ID="Label4" runat="server" Text="<%$ Resources: Resource, Admin_CommonSettings_OAuthTwConsumerSecret %>"></asp:Label>
        </td>
        <td style="vertical-align: middle; height: 26px;">
            <asp:TextBox class="niceTextBox shortTextBoxClass" runat="server" ID="txtTwitterConsumerSecret"
                Width="300"></asp:TextBox>
        </td>
    </tr>
    <tr class="rowPost">
        <td style="width: 30%; height: 26px; text-align: left; vertical-align: middle;">
            <asp:Label ID="Label5" runat="server" Text="<%$ Resources: Resource, Admin_CommonSettings_OAuthTwAccessToken %>"></asp:Label>
        </td>
        <td style="vertical-align: middle; height: 26px;">
            <asp:TextBox class="niceTextBox shortTextBoxClass" runat="server" ID="txtTwitterAccessToken"
                Width="300"></asp:TextBox>
        </td>
    </tr>
    <tr class="rowPost">
        <td style="width: 30%; height: 26px; text-align: left; vertical-align: middle;">
            <asp:Label ID="Label6" runat="server" Text="<%$ Resources: Resource, Admin_CommonSettings_OAuthTwAccessTokenSecret %>"></asp:Label>
        </td>
        <td style="vertical-align: middle; height: 26px;">
            <asp:TextBox class="niceTextBox shortTextBoxClass" runat="server" ID="txtTwitterAccessTokenSecret"
                Width="300"></asp:TextBox>
        </td>
    </tr>
</table>
