<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FinanceStatistic.ascx.cs"
    Inherits="Admin_UserControls_FinanceStatistic" %>
<%@ Import Namespace="Resources" %>
<table cellpadding="0" cellspacing="0" width="100%" style="padding-left: 0px">
    <tbody>
        <tr>
            <%--<td style="width: 50px;">
                <img src="images/Finnance.gif" alt="" />
            </td>--%>
            <td style="height: 31px; vertical-align: bottom; font-size: 14px;">
                <asp:Label Font-Bold="true" runat="server" Text="<%$ Resources:Resource, Admin_FinanceStatistics_FinanceStatistics %>"></asp:Label>
            </td>
        </tr>
    </tbody>
</table>
<asp:Repeater runat="server" DataSourceID="sdsProfit" OnItemDataBound="rpt_ItemDataBound">
    <HeaderTemplate>
        <table cellpadding="3" cellspacing="0" width="100%" style="border-left-color: #AAAAAA;
            border-left-width: 2px;">
            <tr class="header">
                <th align="left">
                    <b>
                        <%= Resource.Admin_FinanceStatistics_Orders %></b>
                </th>
                <%-- <th>
            <%= Resource.Admin_FinanceStatistics_Count %>
        </th>--%>
                <th>
                    <%= Resource.Admin_FinanceStatistics_Sum %>
                </th>
                <th>
                    <%= Resource.Admin_FinanceStatistics_SumWithousDiscount%>
                </th>
                <th>
                    <%= Resource.Admin_FinanceStatistics_Cost %>
                </th>
                <th>
                    <%= Resource.Admin_FinanceStatistics_TaxCost %>
                </th>
                <th>
                    <%= Resource.Admin_FinanceStatistics_ShippingCost %>
                </th>
                <th>
                    <%= Resource.Admin_FinanceStatistics_Profit %>
                </th>
                <th>
                    <%= Resource.Admin_FinanceStatistics_Profitability %>
                </th>
            </tr>
    </HeaderTemplate>
    <ItemTemplate>
        <tr runat="server" id="trItem" class="row1" align="center">
            <td align="left">
                <b>
                    <asp:HyperLink ID="hlLink" CssClass="Link" Text="" runat="server"></asp:HyperLink></b>
            </td>
            <%-- <td>
        <asp:Label runat="server" ID="lblCount"></asp:Label>
        </td>--%>
            <td>
                <asp:Label ID="lblSum" runat="server"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblSumDiscount" runat="server"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblCost" runat="server"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblTax" runat="server"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblShipping" runat="server"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblProfit" runat="server"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblProfitability" runat="server"></asp:Label>
            </td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </table> <span style="color: Red">*</span>
        <asp:Label runat="server" Text='<%$ Resources: Resource, Admin_FinnanceStatistic_Footer  %>'></asp:Label>
    </FooterTemplate>
</asp:Repeater>
<asp:SqlDataSource runat="server" ID="sdsProfit" OnInit="sds_Init" SelectCommand="[Order].[sp_GetProfitPeriods]"
    SelectCommandType="StoredProcedure"></asp:SqlDataSource>
