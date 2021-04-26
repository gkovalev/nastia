<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ResizeNewsPhotos.ascx.cs"
    Inherits="Admin_UserControls_ResizePhoto_News" %>
<table border="0" cellpadding="2" cellspacing="0">
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <asp:Localize ID="Localize7" runat="server" Text='<%$ Resources: Resource, Admin_ResizePhotos_NewsPhotos %>'></asp:Localize></span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <div class="dvMailNotify">
                <asp:Localize ID="lclNotify" runat="server" Text="<%$ Resources:Resource, Admin_ResizePhotos_Notify %>" /><br/>
                <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources:Resource, Admin_ResizePhotos_NotifyNewImages %>" /></div>
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize21" runat="server" Text='<%$ Resources: Resource, Admin_ResizePhotos_Width %>'></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:TextBox class="niceTextBox shortTextBoxClass" ID="txtWidth" runat="server"></asp:TextBox> px.
            <asp:RangeValidator ID="RangeValidator1" runat="server" Type="Integer" MinimumValue="0"
                MaximumValue="1900" ControlToValidate="txtWidth"></asp:RangeValidator>
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize1" runat="server" Text='<%$ Resources: Resource, Admin_ResizePhotos_Height %>'></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:TextBox class="niceTextBox shortTextBoxClass" ID="txtHight" runat="server"></asp:TextBox> px.
            <asp:RangeValidator ID="RangeValidator0" runat="server" Type="Integer" MinimumValue="0"
                MaximumValue="1900" ControlToValidate="txtHight"></asp:RangeValidator>
        </td>
    </tr>
</table>
