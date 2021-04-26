<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditCallBack.aspx.cs" Inherits="Modules_Callback_EditCallback" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        .reviewEditTable
        {
            width: 100%;
            border-collapse: collapse;
        }
        .reviewEditTable td
        {
            height: 30px;
            width: 50%;
        }
        .reviewEditTable tr.altRow
        {
            background-color: #eff0f1;
        }
        .reviewEditTable .tdRight
        {
            text-align: right;
            padding-right: 10px;
        }
        .reviewEditTable .tdLeft
        {
            text-align: left;
            padding-left: 10px;
        }
        .head
        {
            font-family: Verdana;
            font-size: 18pt;
            text-transform: uppercase;
        }
        .subHead
        {
            color: #666666;
            font-family: Verdana;
            font-size: 10pt;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="text-align: center;">
        <asp:Label CssClass="head" runat="server" Text="<%$ Resources: Callback_Header %>"></asp:Label>
        <br />
        <asp:Label CssClass="subHead" runat="server" Text="<%$ Resources: Callback_SubHeader %>"></asp:Label>
    </div>
    <br />
    <br />
    <table class="reviewEditTable">
        <tr>
            <td class="tdRight">
                <asp:Label ID="lblDate" runat="server" Text='<%$ Resources: Callback_DateAdded %>'></asp:Label>
            </td>
            <td class="tdLeft">
                <asp:Label ID="lblDateAdded" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="altRow">
            <td class="tdRight">
                <asp:Label ID="lblName" runat="server" Text='<%$ Resources: Callback_Name %>'></asp:Label>
            </td>
            <td class="tdLeft">
                <asp:TextBox ID="txtName" runat="server" Width="250px"></asp:TextBox>
            </td>
        </tr>
        <tr class="altRow">
            <td class="tdRight">
                <asp:Label ID="lblPhone" runat="server" Text='<%$ Resources: Callback_Phone %>'></asp:Label>
            </td>
            <td class="tdLeft">
                <asp:TextBox ID="txtPhone" runat="server" Width="250px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdRight">
                <asp:Label ID="lblComment" runat="server" Text='<%$ Resources: Callback_Comment %>'></asp:Label>
            </td>
            <td class="tdLeft">
                <asp:TextBox ID="txtComment" runat="server" TextMode="MultiLine" Width="250px" Height="100px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdRight">
                <asp:Label ID="lblAdminComment" runat="server" Text='<%$ Resources: Callback_AdminComment %>'></asp:Label>
            </td>
            <td class="tdLeft">
                <asp:TextBox ID="txtAdminComment" runat="server" TextMode="MultiLine" Width="250px" Height="100px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdRight">
                <asp:Label ID="lblProcessed" runat="server" Text='<%$ Resources: Callback_Processed %>'></asp:Label>
            </td>
            <td class="tdLeft">
                <asp:CheckBox ID="ckbProcessed" runat="server" />
            </td>
        </tr>
        
    </table>
    <div style="text-align: center; margin-top: 10px;">
        <asp:Button ID="btnSave" runat="server" OnClick="btnSaveClick" Text='<%$ Resources: Callback_Save %>' />
    </div>
    </form>
</body>
</html>
