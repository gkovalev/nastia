<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Notepad.ascx.cs" Inherits="Admin_UserControls_Notepad" %>
<table cellpadding="0" cellspacing="0" width="100%" style="padding-left: 0px">
    <tbody>
        <tr>
            <td style="height: 31px; vertical-align: bottom; font-size: 14px;">
                <asp:Label ID="Label1" Font-Bold="true" runat="server" Text="<%$ Resources:Resource, Admin_Notepad_Header %>"></asp:Label>
            </td>
        </tr>
    </tbody>
</table>
<asp:Label runat="server" ID="Message" Visible="false" ForeColor="Blue"></asp:Label>
<CKEditor:CKEditorControl ID="CKEditorControl1" BasePath="~/ckeditor/" runat="server" Height="550px" Width="100%" />
<div style="float: right; margin-top: 5px;">
    <adv:OrangeRoundedButton runat="server" ID="btnSave" Text="<%$ Resources: Resource, Admin_Save %>"
        OnClick="btnSave_Click" />
</div>
