<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RbkMoney.ascx.cs" Inherits="Admin_UserControls_PaymentMethods_RbkMoney" %>
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
            <asp:Localize runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_RbkMoney_EshopId %>"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
         <td class="columnVal">
            <asp:TextBox runat="server" ID="txtEshopId" Width="250"></asp:TextBox>
        </td>
          <td class="columnDescr">
            <asp:Image runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png" ToolTip="<%$ Resources:Resource, Admin_PaymentMethod_RbkMoney_EshopId_Description %>" /><asp:Label runat="server" ID="msgEshopId" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
         <td class="columnName">
            <asp:Localize runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_RbkMoney_RecipientCurrency %>"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
         <td class="columnVal">
            <asp:TextBox runat="server" ID="txtRecipientCurrency" Width="250"></asp:TextBox>
        </td>
          <td class="columnDescr">
            <asp:Image runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png" ToolTip="<%$ Resources:Resource, Admin_PaymentMethod_RbkMoney_RecipientCurrency_Description %>" /><asp:Label runat="server" ID="msgRecipientCurrency" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
         <td class="columnName">
            <asp:Localize runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_RbkMoney_Preference %>"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
         <td class="columnVal">
            <asp:TextBox runat="server" ID="txtPreference" Width="250"></asp:TextBox>
        </td>
          <td class="columnDescr">
            <asp:Image runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png" ToolTip="<%$ Resources:Resource, Admin_PaymentMethod_RbkMoney_Preference_Description %>" /><asp:Label runat="server" ID="msgPreference" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
         <td class="columnName">
            <asp:Localize runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_CurrencyValue %>"></asp:Localize><span class="required">&nbsp;*</span>
        </td>
         <td class="columnVal">
            <asp:TextBox runat="server" ID="txtCurrencyValue" Width="250"></asp:TextBox>
        </td>
          <td class="columnDescr">
            <asp:Image runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png" ToolTip="<%$ Resources:Resource, Admin_PaymentMethod_CurrencyValue_Description %>" /><asp:Label runat="server" ID="msgCurrencyValue" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
</table>
