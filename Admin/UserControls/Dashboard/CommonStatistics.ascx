<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CommonStatistics.ascx.cs"
    Inherits="Admin_UserControls_CommonStatistics" %>
<%@ Register Src="~/Admin/UserControls/Dashboard/OrderStatistics.ascx" TagName="OrderStatistics"
    TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Dashboard/OrderStatusStatistics.ascx" TagName="OrderStatusStatistics"
    TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Dashboard/CustomersStatistics.ascx" TagName="CustomersStatistics"
    TagPrefix="adv" %>
<%@ Register Src="~/Admin/UserControls/Dashboard/ProductsStatistics.ascx" TagName="ProductsStatistics"
    TagPrefix="adv" %>
<table cellpadding="0" cellspacing="0" width="100%" style="padding-left: 0px">
    <tbody>
        <tr>
            <%--<td style="width: 50px;">
                <img src="images/Common.gif" alt="" />
            </td>--%>
            <td class="formheader_order">
                <asp:Label Font-Bold="true" runat="server" Text="<%$ Resources:Resource, Admin_CommonStatistics_CommonStatistics %>"></asp:Label>
            </td>
        </tr>
    </tbody>
</table>
<table border="0" style="background-color: #EEE; width: 100%;"
    cellpadding="0">
    <tr>
        <td valign="top" style="width: 50%">
            <adv:OrderStatusStatistics ID="OrderStatusStatistics" runat="server" />
            <adv:CustomersStatistics ID="CustomersStatistics1" runat="server" />
        </td>
        <td valign="top" style="height: 175px">
            <adv:OrderStatistics ID="OrderStatistics" runat="server" />
            <adv:ProductsStatistics ID="ProductsStatistics" runat="server" />
        </td>
    </tr>
    <tr>
        <td colspan="2">
        </td>
    </tr>
</table>
