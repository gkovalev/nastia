<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PickPoint.ascx.cs" Inherits="Admin_UserControls_PaymentMethods_PickPoint" %>
<table border="0" cellpadding="2" cellspacing="0" style="width: 99%; margin-left: 5px;
    margin-top: 5px;">
    <tr class="rowPost">
        <td colspan="3" style="height: 34px;">
            <h4 style="display: inline; font-size: 12pt;">
                <asp:Localize ID="Localize13" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethods_HeadSettings %>"></asp:Localize></h4>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethods_LinkedShippingMethod %>"></asp:Localize><span
                class="required">&nbsp;*</span>
        </td>
        <td class="columnVal">
            <asp:DropDownList runat="server" ID="ddlShipings" />
        </td>
        <td class="columnDescr">
            <asp:Image ID="Image1" runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="<%$ Resources:Resource, Admin_PaymentMethod_PayPal_EmailID_Description %>" /><asp:Label
                    runat="server" ID="msgEmailID" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
</table>