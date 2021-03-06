<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PaymentMethods.ascx.cs"
    Inherits="UserControls_PaymentMethods" %>
<%@ Import Namespace="AdvantShop.Payment" %>
<asp:RadioButtonList CssClass="ContentText" ID="rblPaymentMethod" runat="server"
    Width="100%">
</asp:RadioButtonList>
<asp:ListView ID="lvPaymentMethod" runat="server" ItemPlaceholderID="itemPlaceHolder">
    <LayoutTemplate>
        <div class="payment">
            <table class="avangard">
                <tr class="header">
                    <th colspan="3">
                    </th>
                    <th class="note">
                        <asp:Literal runat="server" Text="<%$Resources:Resource, Client_OrderConfirmation_Note %>" />
                    </th>
                </tr>
                <tr runat="server" id="itemPlaceHolder">
                </tr>
            </table>
        </div>
    </LayoutTemplate>
    <ItemTemplate>
        <tr onclick="setValuePayment(this);" id='<%# Eval("PaymentMethodID ") %>'>
            <td class="checkbox">
                <input type="radio" name="gr" />
            </td>
            <td class="shipping-img">
                <img src='<%# PaymentIcons.GetPaymentIcon((PaymentType)Eval("Type"), Eval("IconFileName.PhotoName") as string , Eval("Name").ToString()) %>'
                    <%# string.Format("alt=\"{0}\" title=\"{0}\"", HttpUtility.HtmlEncode(Eval("Name"))) %> />
            </td>
            <td>
                <%#Eval("Name") %>
            </td>
            <td class="note">
                <%#Eval("Description") %>
            </td>
        </tr>
    </ItemTemplate>
    <SelectedItemTemplate>
        <tr class="selected" onclick="setValuePayment(this);" id='<%# Eval("PaymentMethodID ") %>'>
            <td class="checkbox">
                <input type="radio" name="gr" checked="checked" />
            </td>
            <td class="shipping-img">
                <img src='<%# PaymentIcons.GetPaymentIcon((PaymentType)Eval("Type"), Eval("IconFileName.PhotoName") as string, Eval("Name").ToString()) %>'
                    <%# string.Format("alt=\"{0}\" title=\"{0}\"", HttpUtility.HtmlEncode(Eval("Name"))) %> />
            </td>
            <td>
                <%#Eval("Name") %>
            </td>
            <td class="note">
                <%#Eval("Description") %>
            </td>
        </tr>
    </SelectedItemTemplate>
    <EmptyDataTemplate>
        <div class="payment">
            <table class="avangard">
                <tr class="header">
                    <th style="text-align: center;">
                        <br/>
                        
                        <asp:Label runat="server" ID="lblError" ForeColor="Red" Text="<%$ Resources: Resource, Client_OrderConfirmation_NoPaymentMethod %>"></asp:Label>
                    </th>
                </tr>
            </table>
        </div>
    </EmptyDataTemplate>
</asp:ListView>
<asp:HiddenField ID="hfPaymentMethodId" runat="server" />
