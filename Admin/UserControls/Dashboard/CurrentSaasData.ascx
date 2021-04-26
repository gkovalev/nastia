<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CurrentSaasData.ascx.cs"
    Inherits="Admin_UserControls_CurrentSaasData" %>
<style type="text/css">
    .style1
    {
        font: 14px tahoma;
        width: 400px;
    }
    .style2
    {
        font: 14px tahoma;
        width: 400px;
    }
</style>
<table cellpadding="3" cellspacing="3">
    <tr>
        <td class="style1">
            &nbsp;
        </td>
        <td class="style2">
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="style1">
            Дата последнего обновления тарифа:&nbsp;
        </td>
        <td class="style2">
            <asp:Label ID="lblLastUpdate" runat="server" Font-Bold="true" />&nbsp;
            <asp:LinkButton ID="btnUpdateSaasData" runat="server" Text="Обновить данные о тарифе"
                OnClick="btnUpdateSaasData_Click" />
        </td>
    </tr>
    <tr>
        <td class="style1">
            &nbsp;
        </td>
        <td class="style2">
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="style1">
            Название тарифного плана:&nbsp;
        </td>
        <td class="style2">
            <asp:Label ID="lblTariffName" runat="server" Font-Bold="true" />
        </td>
    </tr>
    <tr>
        <td class="style1">
            Тариф оплачен до:&nbsp;
        </td>
        <td class="style2">
            <asp:Label ID="lblPaidTo" runat="server" Font-Bold="true" />&nbsp;
        </td>
    </tr>
    <tr>
        <td class="style1">
            Дней до отключения:&nbsp;
        </td>
        <td class="style2">
            <asp:Label ID="lblDaysLeft" runat="server" Font-Bold="true" />
        </td>
    </tr>
    <tr>
        <td class="style1">
            &nbsp;
        </td>
        <td class="style2">
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="style1">
            Количество товаров в магазине:&nbsp;
        </td>
        <td class="style2">
            <asp:Label ID="lblProductsCount" runat="server" Font-Bold="true" />&nbsp;/&nbsp;
            <asp:Label ID="lblMaxProductsCount" runat="server" ForeColor="Red" Font-Bold="true" />
        </td>
    </tr>
    <tr>
        <td class="style1">
            Максимальное количество фотографий у товара:&nbsp;
        </td>
        <td class="style2">
            <asp:Label ID="lblMaxPhotos" runat="server" Font-Bold="true" />
        </td>
    </tr>
    <tr>
        <td class="style1">
            Интеграция с Excel/CSV:&nbsp;
        </td>
        <td class="style2">
            <asp:Label ID="lblHaveExcel" runat="server" Font-Bold="true" />
        </td>
    </tr>
    <tr>
        <td class="style1">
            Интеграция с 1C:&nbsp;
        </td>
        <td class="style2">
            <asp:Label ID="lblHave1C" runat="server" Font-Bold="true" />
        </td>
    </tr>
    <tr>
        <td class="style1">
            Экспорт каталога в Яндекс.Маркет:&nbsp;
        </td>
        <td class="style2">
            <asp:Label ID="lblHaveExportFeeds" runat="server" Font-Bold="true" />
        </td>
    </tr>
    <tr>
        <td class="style1">
            Регулирование цен:&nbsp;
        </td>
        <td class="style2">
            <asp:Label ID="lblHavePriceRegulating" runat="server" Font-Bold="true" />
        </td>
    </tr>
    <tr>
        <td class="style1">
            Обновление валют по текущему курсу ЦБ РФ:&nbsp;
        </td>
        <td class="style2">
            <asp:Label ID="lblHaveBankIntegration" runat="server" Font-Bold="true" />
        </td>
    </tr>
</table>
