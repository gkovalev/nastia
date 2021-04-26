<%@ Control Language="C#" AutoEventWireup="true" CodeFile="OrderStatusStatistics.ascx.cs"
    Inherits="Admin_UserControls_OrderStatusStatistics" %>
<%@ Import Namespace="AdvantShop.Orders" %>
<%@ Import Namespace="System.Drawing" %>
<%@ Import Namespace="AdvantShop.Repository.Currencies" %>
<table cellpadding="3">
    <tr>
        <td>
            <img src="images/icon_1.png" alt="" />
        </td>
        <td class="big">
            <b><u>
                <asp:Label ID="lblHeadOrders" runat="server" Text="<%$ Resources:Resource, Admin_Default_Orders %>"></asp:Label></u></b>
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;
        </td>
        <td>
        <asp:SqlDataSource runat="server" ID="sdsStatuses" OnInit="sds_Init" SelectCommand="SELECT [OrderStatus].[OrderStatusID], [StatusName], [CommandID], Sum([Sum]) as 'Sum', Count([Order].[OrderID]) as 'Count' FROM [Order].[OrderStatus] left join [Order].[Order] on [Order].[OrderStatusID] = [OrderStatus].[OrderStatusID] WHERE [OrderStatus].[OrderStatusID] <> 9 GROUP BY [OrderStatus].[OrderStatusID], [StatusName], [CommandID]"></asp:SqlDataSource>
            <asp:Repeater runat="server" ID="rptStatuses" DataSourceID="sdsStatuses">
                <HeaderTemplate>
                    <table  cellpadding="0" cellspacing="0">
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td align="left" valign="top">
                            <asp:Hyperlink runat="server" ID="lblName" Font-Bold='<%# OrderService.DefaultOrderStatus == (int)Eval("OrderStatusID") ? true : false %>' NavigateUrl='<%# "~/Admin/OrderSearch.aspx?status=" + Eval("StatusName") %>' Text='<%# Eval("StatusName") %>'
                                ForeColor='<%# OrderService.DefaultOrderStatus == (int)Eval("OrderStatusID") ? Color.Red : Color.Black %>'></asp:Hyperlink>: &nbsp;
                        </td>
                        <td valign="top">
                            <asp:Label runat="server" ID="lblCount" Text='<%# (Eval("Count") == DBNull.Value) ? "0" : Eval("Count") %>' Font-Bold="true"
                                ForeColor='<%# OrderService.DefaultOrderStatus == (int)Eval("OrderStatusID") ? Color.Red : Color.Black %>'></asp:Label>
                                &nbsp;
                        </td>
                        <td valign="top">
                            <asp:Label runat="server" ID="lblSum" Text='<%# (Eval("Sum") == DBNull.Value) ? string.Format("({0})", AdvantShop.Catalog.CatalogService.GetStringPrice(0, CurrencyService.CurrentCurrency.Value, CurrencyService.CurrentCurrency.Iso3)) : string.Format("({0})", AdvantShop.Catalog.CatalogService.GetStringPrice((decimal)Eval("Sum"), CurrencyService.CurrentCurrency.Value, CurrencyService.CurrentCurrency.Iso3)) %>'
                                ForeColor='<%# OrderService.DefaultOrderStatus == (int)Eval("OrderStatusID") ? Color.Red : Color.FromArgb(116,116,116) %>'></asp:Label>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </td>
    </tr>
</table>
