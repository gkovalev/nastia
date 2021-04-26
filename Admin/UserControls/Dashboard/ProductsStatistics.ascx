<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProductsStatistics.ascx.cs"
    Inherits="Admin_UserControls_ProductsStatistics" %>
<table cellpadding="3">
    <tr>
        <td>
            <img src="images/icon_3.png" alt="" />
        </td>
        <td class="big">
            <b><u>
                <asp:Label ID="lblHeadProduct" runat="server" Text="<%$ Resources:Resource, Admin_Default_Products %>"></asp:Label></u></b>
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;
        </td>
        <td>
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td valign="top" align="right" style="width:50px;">
                        <asp:Label ID="lblCommonProductsCOUNT" runat="server"></asp:Label>
                        - 
                    </td>
                    <td>
                        <asp:Label ID="Label45" runat="server" Text="<%$ Resources:Resource, Admin_Default_ProductCount %>"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="right" style="width:50px;">
                        <asp:Label ID="lblCommonEnableProductCOUNT" runat="server"></asp:Label>
                        - 
                    </td>
                    <td>
                        <asp:Label ID="Label47" runat="server" Text="<%$ Resources:Resource, Admin_Default_ProductsForSale %>"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="right" style="width:50px;">
                        <asp:Label ID="lblProductCategoryCOUNT" runat="server"></asp:Label>
                        - 
                    </td>
                    <td>
                        <asp:Label ID="Label49" runat="server" Text="<%$ Resources:Resource, Admin_Default_CategoryCount %>"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="right" style="width:50px;" style="width:50px;">
                        <asp:Label ID="lblProductPostCOUNT" runat="server"></asp:Label>
                        - 
                    </td>
                    <td>
                        <asp:Label ID="Label51" runat="server" Text="<%$ Resources:Resource, Admin_Default_CommentCount %>"></asp:Label>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
