<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="about.aspx.cs" Inherits="Admin_about" Title="AdVantShop.NET - About us" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <table style="width: 100%; height: 300px">
        <tr>
            <td>
                <p style="text-align:center;"><span style="font-size: 24px;">Ad<b>Vant</b>Shop.NET Ultimate</span></p>
                <p style="text-align:center;"><em>Best software in your hands</em></p>
                <p style="text-align:center;"><%=AdvantShop.Configuration.SettingsGeneral.SiteVersion%></p>
                <p style="text-align:center; display:none;">Статус модификаций: 0 - Базовая поставка</p>
            </td>
        </tr>
    </table>
    <br />
</asp:Content>
