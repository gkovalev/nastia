<%@ Control Language="C#" AutoEventWireup="true" CodeFile="OrderStatistics.ascx.cs"
    Inherits="Admin_UserControls_OrderStatistics" %>
<table cellpadding="3">
    <tr>
        <td>
            <img src="images/icon_2.png" alt="" />
        </td>
        <td class="big">
            <b><u>
                <asp:Label ID="lblHeadOrders" runat="server" Text="<%$ Resources:Resource, Admin_OrderStatistic_OrderStatistic %>"></asp:Label></u></b>
        </td>
    </tr>
    <tr>
        <td style="height: 144px">
            &nbsp;
        </td>
        <td style="height: 144px">
            <table  cellpadding="0" cellspacing="0">
                <tr>
                    <td valign="top" align="right">
                        <asp:Label ID="lblAvCheck"  runat="server"></asp:Label>
                        </td><td valign="top">&nbsp;-&nbsp;
                    </td>
                    <td valign="top">
                        <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Resource, Admin_OrderStatistic_AvgCheck %>"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="right">
                        <asp:Label ID="LblProfitability"  runat="server" Text="0%"></asp:Label>
                        </td><td valign="top">&nbsp;-&nbsp;
                    </td>
                    <td valign="top">
                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Resource, Admin_OrderStatistic_Profitability %>" />
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="right">
                        <asp:Label  ID="lblAvOrders" runat="server" Text="0"></asp:Label>
                        </td><td valign="top">&nbsp;-&nbsp; 
                    </td>
                    <td valign="top">
                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, Admin_OrderStatistic_AvgOrdersDay %>" />
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="right">
                        <asp:Label ID="lblAvItems"  runat="server" Text="0"></asp:Label>
                        </td><td valign="top">&nbsp;-&nbsp; 
                    </td>
                    <td valign="top">
                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Resource, Admin_OrderStatistic_AvgOrderItems %>" />
                    </td>
                </tr>
                <tr>
                <td valign="top">&nbsp;</td>
                </tr>
                <tr>
                    <td valign="top" align="right">
                        <asp:Label ID="LblPopPayment"  runat="server" Text="<%$ Resources:Resource, Admin_OrderStatistic_None %>"></asp:Label>
                        </td><td valign="top">&nbsp;-&nbsp; 
                    </td>
                    <td valign="top">
                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Resource, Admin_OrderStatistic_PopPayment %>" />
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="right">
                        <asp:Label ID="LblPopShipping"  runat="server" Text="<%$ Resources:Resource, Admin_OrderStatistic_None %>"></asp:Label>
                        </td><td valign="top">&nbsp;-&nbsp;&nbsp; 
                    </td>
                    <td valign="top">
                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Resource, Admin_OrderStatistic_PopShipping %>" />
                    </td>
                </tr>
                
                <tr>
                    <td valign="top" align="right">
                        <asp:Label ID="LblCountry"  Text="<%$ Resources:Resource, Admin_OrderStatistic_None %>"
                            runat="server"></asp:Label>
                        </td><td valign="top">&nbsp;-&nbsp; 
                    </td>
                    <td valign="top">
                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Resource, Admin_OrderStatistic_Country %>" />
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="right">
                        <asp:Label ID="LblRegion"  Text="<%$ Resources:Resource, Admin_OrderStatistic_None %>"
                            runat="server"></asp:Label>
                        </td><td valign="top">&nbsp;-&nbsp; 
                    </td>
                    <td valign="top">
                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Resource, Admin_OrderStatistic_Zone %>" />
                    </td>
                </tr>
                <tr>
                <td valign="top">&nbsp;</td>
                </tr>
                <tr>
                    <td valign="top" align="right">
                        <asp:Label ID="LblReg"  runat="server" Text="0%"></asp:Label>
                        </td><td valign="top">&nbsp;-&nbsp;
                    </td>
                    <td valign="top">
                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Resource, Admin_OrderStatistic_Reg %>" />
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="right">
                        <asp:Label ID="LblUnreg"  runat="server" Text="0%"></asp:Label>
                        </td><td valign="top">&nbsp;-&nbsp; 
                    </td>
                    <td valign="top">
                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Resource, Admin_OrderStatistic_Unreg %>" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
