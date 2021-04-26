<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ResizeCategoryPhotos.ascx.cs"
    Inherits="Admin_UserControls_ResizePhoto_Category" %>
<table border="0" cellpadding="2" cellspacing="0">
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <asp:Localize ID="Localize7" runat="server" Text='<%$ Resources: Resource, Admin_ResizePhotos_CategoryPhotos %>'></asp:Localize></span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <div class="dvMailNotify">
                <asp:Localize ID="lclNotify" runat="server" Text="<%$ Resources:Resource, Admin_ResizePhotos_Notify %>" /><br/>
                <asp:Localize ID="Localize5" runat="server" Text="<%$ Resources:Resource, Admin_ResizePhotos_NotifyNewImages %>" /></div>
        </td>
    </tr>
    
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <asp:Localize ID="Localize6" runat="server" Text="<%$ Resources: Resource, Admin_ResizePhotos_BigCategoryImages %>" /></span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize2" runat="server" Text='<%$ Resources: Resource, Admin_ResizePhotos_Width %>'></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:TextBox class="niceTextBox shortTextBoxClass" ID="txtBigWidth" runat="server"></asp:TextBox> px.
            <asp:RangeValidator ID="RangeValidator2" runat="server" Type="Integer" MinimumValue="0"
                MaximumValue="1900" ControlToValidate="txtBigWidth"></asp:RangeValidator>
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize4" runat="server" Text='<%$ Resources: Resource, Admin_ResizePhotos_Height %>'></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:TextBox class="niceTextBox shortTextBoxClass" ID="txtBigHeight" runat="server"></asp:TextBox> px.
            <asp:RangeValidator ID="RangeValidator3" runat="server" Type="Integer" MinimumValue="0"
                MaximumValue="1900" ControlToValidate="txtBigWidth"></asp:RangeValidator>
        </td>
    </tr>


    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <asp:Localize ID="Localize3" runat="server" Text="<%$ Resources: Resource, Admin_ResizePhotos_SmallCategoryImages%>" /></span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize21" runat="server" Text='<%$ Resources: Resource, Admin_ResizePhotos_Width %>'></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:TextBox class="niceTextBox shortTextBoxClass" ID="txtSmallWidth" runat="server"></asp:TextBox> px.
            <asp:RangeValidator ID="RangeValidator1" runat="server" Type="Integer" MinimumValue="0"
                MaximumValue="1900" ControlToValidate="txtSmallWidth"></asp:RangeValidator>
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize1" runat="server" Text='<%$ Resources: Resource, Admin_ResizePhotos_Height %>'></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:TextBox class="niceTextBox shortTextBoxClass" ID="txtSmallHeight" runat="server"></asp:TextBox> px.
            <asp:RangeValidator ID="RangeValidator0" runat="server" Type="Integer" MinimumValue="0"
                MaximumValue="1900" ControlToValidate="txtSmallHeight"></asp:RangeValidator>
        </td>
    </tr>
    
    
</table>
