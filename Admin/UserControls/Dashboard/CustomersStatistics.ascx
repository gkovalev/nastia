<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CustomersStatistics.ascx.cs"
    Inherits="Admin_UserControls_CustomersStatistics" %>
<table cellpadding="3">
    <tr>
        <td>
            <img src="images/icon_4.png" alt="" />
        </td>
        <td class="big">
            <b><u>
                <asp:Label ID="lblHeadCustomers" runat="server" Text="<%$ Resources:Resource, Admin_Default_Buyers %>"></asp:Label></u></b>
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;
        </td>
        <td>
            <table cellpadding="0" cellspacing="0">
            <tr>
                    <td valign="top" valign="top" align="right" style="width:50px;">
                        <asp:Label ID="lblTodayRegUserCOUNT" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="HyperLink1" runat="server"
                            Text="<%$ Resources:Resource, Admin_Default_TodayRegUsers %>"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="right" style="width:50px;">
                        <asp:Label ID="lblRegUserCOUNT" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:HyperLink ID="HyperLink7" runat="server" CssClass="Link" NavigateUrl="~/Admin/CustomerSearch.aspx"
                            Text="<%$ Resources:Resource, Admin_Default_RegUsers %>"></asp:HyperLink>
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="right" style="width:50px;">
                        <asp:Label ID="lblUsersWithOrder" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="HyperLink2" runat="server"
                            Text="<%$ Resources:Resource, Admin_Default_UsersWithOrder %>"></asp:Label>
                    </td>
                </tr>
                <tr>
                <td>&nbsp;</td></tr>
                <tr>
                    <td valign="top" align="right" style="width:50px;">
                        <asp:Label ID="lblSubscribeUserCOUNT" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:HyperLink ID="HyperLink5" runat="server" CssClass="Link" NavigateUrl="~/Admin/Subscription.aspx"
                            Text="<%$ Resources:Resource, Admin_Default_NewsSubscribers %>"></asp:HyperLink>
                    </td>
                </tr>
                
            </table>
        </td>
    </tr>
</table>
