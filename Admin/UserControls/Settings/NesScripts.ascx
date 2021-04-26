<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NesScripts.ascx.cs" Inherits="Admin_UserControls_Settings_MailSettings" %>
<table border="0" cellpadding="2" cellspacing="0" style="width:540px;">
    <tr class="rowPost">
        <td style="height: 34px;">
            <span class="spanSettCategory">
                <asp:Localize ID="Localize_Admin_CommonSettings_HeadMailServer" runat="server" 
                    Text="Запуск скриптов" /></span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    </table>
<br />
<table border="0" cellpadding="2" cellspacing="0" style="width:540px;">
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <asp:Localize ID="Localize_Admin_CommonSettings_SendTestMessage" runat="server" 
                    Text="Скрипт" /></span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
</table>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>

        <table>
            <tbody>
                <tr class="rowsPost">
                    <td>
                        <span class="Label"><asp:Localize ID="Localize_Admin_CommonSettings_TestEmail_Message" runat="server" 
                            Text=''></asp:Localize></span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtMessage" runat="server" CssClass="niceTextBox" style="padding-top:3px;" 
                            TextMode="MultiLine" Height="112px" Width="375px" ReadOnly="True"></asp:TextBox>
                    </td>
                    <td>
                        <br />
                        <br />
                        <br />
                        <br />
                        <br />
                        <br />
                    </td>
                </tr>
            </tbody>
        </table>
        <%--<asp:Label ID="lblDegub" runat="server" Text="[]"></asp:Label>--%>
        <br />
        <asp:Button ID="btnAllToZero" runat="server" style="height:26px; width:217px;" 
            Text="Обнулить все" onclick="btnAllToZero_Click" />
        <asp:Button ID="btnToZeroAllExceptDremel" runat="server" 
            onclick="btnToZeroAllExceptDremel_Click" style="height:26px; width:217px;" 
            Text="Обнулить все кроме Дремел" />
        <asp:Button ID="btnToZeroAllExceptDremelAndSpareParts" runat="server" 
            onclick="btnToZeroAllExceptDremelAndSpareParts_Click" Text="Обнулить все кроме Дремел и Запчастей" 
            Width="283px" />
        <asp:Button ID="btnToZeroDremelOnly" runat="server" 
            onclick="btnToZeroDremelOnly_Click" style="height:26px; width:217px;" 
            Text="Обнулить только Дремел" />
        <br />
        <br />
        <asp:Button ID="btnUpdateYandexMarket0" runat="server" 
            onclick="btnUpdateYandexMarket_Click" style="height:26px; width:217px;" 
            Text="Обновить Яндекс.Маркет" />
        <asp:Button ID="btnUpdateCategSorting" runat="server" 
            onclick="btnUpdateCategSorting_Click" style="height:26px; width:217px;" 
            Text="Обновить Сортировку" />
        <br />
        <asp:Literal ID="Message" runat="server"></asp:Literal>

    </ContentTemplate>
</asp:UpdatePanel>
