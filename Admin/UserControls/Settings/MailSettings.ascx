<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MailSettings.ascx.cs" Inherits="Admin_UserControls_Settings_MailSettings" %>
<table border="0" cellpadding="2" cellspacing="0" style="width:540px;">
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <asp:Localize ID="Localize_Admin_CommonSettings_HeadMailServer" runat="server" 
                    Text="<%$ Resources:Resource, Admin_CommonSettings_HeadMailServer %>" /></span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <div class="dvMailNotify"><asp:Localize ID="Localize_Admin_CommonSettings_MailNotify" runat="server" 
                Text="<%$ Resources:Resource, Admin_CommonSettings_MailNotify %>" /></div>       
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
           <span class="spanSettCategory">
                <asp:Localize ID="Localize2" runat="server" 
                    Text="<%$ Resources:Resource, Admin_CommonSettings_TransportLevel %>" /></span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="width: 155px;">
            <asp:Localize ID="Localize_Admin_CommonSettings_SmtpServer" runat="server" 
                Text='<%$ Resources:Resource, Admin_CommonSettings_SmtpServer %>'></asp:Localize>
        </td>
        <td>
            &nbsp;<asp:TextBox ID="txtEmailSMTP" runat="server" class="niceTextBox shortTextBoxClass"></asp:TextBox>
        </td>
    </tr>
    <tr class="rowsPost">
        <td>
            <asp:Localize ID="Localize_Admin_CommonSettings_SupportLogin" runat="server" 
                Text='<%$ Resources:Resource, Admin_CommonSettings_SupportLogin %>'></asp:Localize>
        </td>
        <td>
            &nbsp;<asp:TextBox ID="txtEmailLogin" runat="server" class="niceTextBox shortTextBoxClass"></asp:TextBox>
        </td>
    </tr>
    <tr class="rowsPost">
        <td>
            <asp:Localize ID="Localize_Admin_CommonSettings_SupportPassword" runat="server" 
                Text='<%$ Resources:Resource, Admin_CommonSettings_SupportPassword %>'></asp:Localize>
        </td>
        <td>
            &nbsp;<asp:TextBox ID="txtEmailPassword" runat="server" class="niceTextBox shortTextBoxClass"></asp:TextBox>
        </td>
    </tr>
    <tr class="rowsPost">
        <td>
            <asp:Localize ID="Localize_Admin_CommonSettings_Port" runat="server" 
                Text='<%$ Resources:Resource, Admin_CommonSettings_Port %>'></asp:Localize>
        </td>
        <td>
            &nbsp;<asp:TextBox ID="txtEmailPort" runat="server" class="niceTextBox shortTextBoxClass2"></asp:TextBox>&nbsp;<span><asp:Localize ID="Local_defPort" 
                runat="server" Text='<%$ Resources:Resource, Admin_CommonSettings_MailNotify_defPort %>'></asp:Localize></span>
        </td>
    </tr>
    <tr class="rowsPost">
        <td>
            <asp:Localize ID="Localize_Admin_CommonSettings_EmailDistrib" runat="server" 
                Text='<%$ Resources:Resource, Admin_CommonSettings_EmailDistrib %>'></asp:Localize>
            <br />
        </td>
        <td>
            &nbsp;<asp:TextBox ID="txtEmail" runat="server" class="niceTextBox shortTextBoxClass"></asp:TextBox>
        </td>
    </tr>
    <tr class="rowsPost">
        <td>
            <asp:Localize ID="Localize_Admin_CommonSettings_EnableSSL" runat="server" 
                Text='<%$ Resources:Resource, Admin_CommonSettings_EnableSSL %>'></asp:Localize>
        </td>
        <td>
            &nbsp;<asp:CheckBox runat="server" ID="chkEnableSSL" />
        </td>
    </tr>
</table>
<br />
<table border="0" cellpadding="2" cellspacing="0" style="width:540px;">
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <asp:Localize ID="Localize_Admin_CommonSettings_SendTestMessage" runat="server" 
                    Text="<%$ Resources:Resource, Admin_CommonSettings_SendTestMessage %>" /></span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
</table>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>

        <table>
            <tbody>
              <tr class="rowsPost">
                    <td style="width: 155px;">
                        <span class="Label"><asp:Localize ID="Localize_Admin_CommonSettings_TestEmail_SendTo" runat="server" 
                            Text='<%$ Resources:Resource, Admin_CommonSettings_TestEmail_SendTo %>'></asp:Localize></span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtTo" runat="server" CssClass="niceTextBox shortTextBoxClass"></asp:TextBox>
                    </td>
                </tr>
                <tr class="rowsPost">
                    <td>
                        <span class="Label"><asp:Localize ID="Localize_Admin_CommonSettings_TestEmail_Subject" runat="server" 
                            Text='<%$ Resources:Resource, Admin_CommonSettings_TestEmail_Subject %>'></asp:Localize></span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtSubject" runat="server" CssClass="niceTextBox" Width="375px"></asp:TextBox>
                    </td>
                </tr>
                <tr class="rowsPost">
                    <td>
                        <span class="Label"><asp:Localize ID="Localize_Admin_CommonSettings_TestEmail_Message" runat="server" 
                            Text='<%$ Resources:Resource, Admin_CommonSettings_TestEmail_Message %>'></asp:Localize></span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtMessage" runat="server" CssClass="niceTextBox" style="padding-top:3px;" 
                            TextMode="MultiLine" Height="112px" Width="375px"></asp:TextBox>
                    </td>
                </tr>
            </tbody>
        </table>
        <%--<asp:Label ID="lblDegub" runat="server" Text="[]"></asp:Label>--%>
        <br />
        <asp:Button ID="btnSendMail" runat="server" style="height:26px; width:217px;" 
            Text="<%$ Resources:Resource, Admin_CommonSettings_TestEmail_SendButtom %>" onclick="btnSendMail_Click" />
        <br />
        <br />
        <asp:Literal ID="Message" runat="server"></asp:Literal>

    </ContentTemplate>
</asp:UpdatePanel>
