<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BankSettings.ascx.cs"
    Inherits="Admin_UserControls_Settings_BankSettings" %>
<table border="0" cellpadding="2" cellspacing="0">
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <asp:Localize ID="Localize7" runat="server" Text="Банковские реквизиты"></asp:Localize></span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize1" runat="server" Text="Наименование организации"></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:TextBox  class="textBoxClass" ID="txtCompanyName" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize21" runat="server" Text="ИНН"></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:TextBox class="textBoxClass" ID="txtINN" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize2" runat="server" Text="КПП"></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:TextBox class="textBoxClass" ID="txtKPP" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize16" runat="server" Text="Расчетный счет"></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:TextBox class="textBoxClass" ID="txtRS" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize3" runat="server" Text="Наименование банка"></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:TextBox class="textBoxClass" ID="txtBankName" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize4" runat="server" Text="Корреспондентский  счет"></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:TextBox class="textBoxClass" ID="txtKS" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize5" runat="server" Text="БИК"></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:TextBox class="textBoxClass" ID="txtBIK" runat="server"></asp:TextBox>
        </td>
    </tr>
        <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize8" runat="server" Text="Адрес"></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:TextBox class="textBoxClass" ID="txtAddress" runat="server"></asp:TextBox>
        </td>
    </tr>

    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize15" runat="server" Text="Печать фирмы"></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top">
            <asp:Panel ID="pnlStamp" runat="server" Width="100%">
                <asp:Image ID="imgStamp" runat="server" Height="80" BorderColor="Gray" BorderWidth="1px" /><br />
                <asp:Button ID="btnDeleteStamp" runat="server" Text="<%$ Resources:Resource, Admin_Delete%>"
                    OnClick="DeleteStamp_Click" />
            </asp:Panel>
            <asp:FileUpload ID="fuStamp" runat="server" Height="20px" Width="308px" BackColor="White" />
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <asp:Localize ID="Localize6" runat="server" Text="Руководство"></asp:Localize></span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize18" runat="server" Text="Директор"></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:TextBox class="textBoxClass" ID="txtDirector" runat="server"></asp:TextBox>
        </td>
    </tr>
    
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize20" runat="server" Text="Главный бухгалтер"></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:TextBox class="textBoxClass" ID="txtHeadCounter" runat="server"></asp:TextBox>
        </td>
    </tr>

    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize19" runat="server" Text="Управляющий"></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:TextBox class="textBoxClass" ID="txtManager" runat="server"></asp:TextBox>
        </td>
    </tr>
   
</table>
