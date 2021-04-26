<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Edost.ascx.cs" Inherits="Admin_UserControls_ShippingMethods_Edost" %>
<table border="0" cellpadding="2" cellspacing="0" style="width: 99%; margin-left: 5px;
    margin-top: 5px">
    <tr class="rowPost">
        <td colspan="3" style="height: 34px">
            <h4 style="display: inline; font-size: 12pt">
                <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethods_HeadSettings %>"></asp:Localize></h4>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize21" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_Edost_ShopId %>"></asp:Localize><span
                class="required">*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtShopId" Width="250"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Image ID="Image1" runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="<%$ Resources:Resource, Admin_ShippingMethod_Edost_ShopId_Description %>" /><asp:Label
                    runat="server" ID="msgShopId" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_Edost_Password %>"></asp:Localize><span
                class="required">*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtPassword" Width="250"></asp:TextBox>
            <asp:Literal runat="server" ID="lPassword" Text="******"/>
        </td>
        <td class="columnDescr">
            <asp:Image ID="Image2" runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="<%$ Resources:Resource, Admin_ShippingMethod_Edost_Password_Description %>" /><asp:Label
                    runat="server" ID="msgPassword" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize6" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_Edost_Rate %>"></asp:Localize><span
                class="required">*</span>
        </td>
        <td class="columnVal">
            <asp:TextBox runat="server" ID="txtRate" Width="250" Text="1"></asp:TextBox>
        </td>
        <td class="columnDescr">
            <asp:Image ID="Image6" runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="<%$ Resources:Resource, Admin_ShippingMethod_Edost_Rate_Description %>" /><asp:Label
                    runat="server" ID="msgRate" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize4" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_Edost_CreateCash %>"></asp:Localize>
        </td>
        <td class="columnVal">
            <asp:CheckBox ID="chbcreateCOD" runat="server" /><asp:HiddenField ID="hfCod" runat="server" />
        </td>
        <td class="columnDescr">
            <asp:Image ID="Image4" runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="<%$ Resources:Resource, Admin_ShippingMethod_Edost_CreateCash %>" /><asp:Label runat="server"
                    ID="Label2" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="columnName">
            <asp:Localize ID="Localize5" runat="server" Text="<%$ Resources:Resource, Admin_ShippingMethod_Edost_CreatePickPoint %>"></asp:Localize>
        </td>
        <td class="columnVal">
            <asp:CheckBox ID="chbcreatePickPoint" runat="server" /><asp:HiddenField ID="hfPickPoint"
                runat="server" />
        </td>
        <td class="columnDescr">
            <asp:Image ID="Image5" runat="server" Width="18" Height="18" ImageUrl="~/Admin/images/messagebox_info.png"
                ToolTip="<%$ Resources:Resource, Admin_ShippingMethod_Edost_CreatePickPoint %>" /><asp:Label runat="server"
                    ID="Label3" Visible="false" ForeColor="Red"></asp:Label>
        </td>
    </tr>    
</table>
