<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PrintProductInfo.aspx.cs"
    Inherits="PrintProductInfo" %>

<%@ Import Namespace="AdvantShop.FilePath" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>
        <%=Resources.Resource.Client_PrintProductInfo_Title%></title>
    <link href="css/style.css" rel="stylesheet" type="text/css" />
</head>
<script type="text/javascript">
    function position_this_window() {
        var x = (screen.availWidth - 730) / 2;
        window.resizeTo(730, screen.availHeight - 100);
        window.moveTo(Math.floor(x), 50);
    }
</script>
<body onload="position_this_window();window.print();" style="padding: 10px; background-image: none;">
    <form id="form1" runat="server">
    <div>
        <table border="0" width="100%" id="table1" cellspacing="1" cellpadding="0">
            <tr>
                <td valign="top" style="width: 215px;">
                    <asp:Image ID="Image1" runat="server" />
                </td>
                <td>
                    <font face="Arial" size="2"><b>
                        <asp:Label ID="lblShopName" runat="server" Text="Label"></asp:Label>
                    </b>
                        <br />
                    </font><font face="Arial" size="1">
                        <%=Resources.Resource.Client_PrintProductInfo_Address%>
                        <br />
                    </font>
                </td>
            </tr>
        </table>
        <br />
        <span class="PageHeader" style="color: Black;">
            <asp:Label ID="lblProductName" runat="server"></asp:Label>
            <asp:Label ID="lblPrice" runat="server"></asp:Label>
        </span>
        <hr class="HeaderHR" size="1px" />
        <br />
        <table border="0" width="100%" id="table2" cellspacing="0" cellpadding="0">
            <tr>
                <asp:Panel ID="pnlMidPic" runat="server" Visible="true">
                    <td style="width: 1px; vertical-align: top;">
                        <asp:ImageButton ID="imgMiddle" runat="server" />
                    </td>
                </asp:Panel>
                <td valign="top" style="padding-left: 15px; padding-right: 3px">
                    <asp:Panel ID="pnlDescription" runat="server" Width="100%" Visible="false">
                        <asp:Label ID="Label1" runat="server" CssClass="ContentTextBlack" Font-Bold="True"
                            Text="<%$ Resources:Resource, Client_PrintProductInfo_Description %>"></asp:Label>
                        <p>
                            <asp:Label ID="lblDescription" runat="server"></asp:Label>
                        </p>
                        <br />
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td colspan="2" valign="top">
                    <asp:DataList ID="DataListPhotos" runat="server" RepeatColumns="4" RepeatDirection="Horizontal">
                        <ItemTemplate>
                            <table border="0" width="77px" id="table3" cellspacing="0" cellpadding="0" style="margin-top: 10px;">
                                <tr>
                                    <td>
                                        <img alt='<%# Eval("Description") %>' src='<%# FoldersHelper.GetImageProductPath( ProductImageType.XSmall, (string )Eval("ID"), false) %>'
                                            style='<%# "border: 1px solid #000000; width:" + (string )Eval("Width") + "px; height:" + (string )Eval("Height") + "px;"%>'>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:DataList>
                </td>
            </tr>
        </table>
        <br />
        <asp:DataList ID="DataListProperties" runat="server" RepeatColumns="2" RepeatLayout="Table"
            RepeatDirection="Horizontal">
            <HeaderTemplate>
                <asp:Label ID="Label1" runat="server" CssClass="ContentTextBlack" Font-Bold="True"
                    Text="<%$ Resources:Resource, Client_PrintProductInfo_ProductProperty %>"></asp:Label>
                <ol>
            </HeaderTemplate>
            <ItemTemplate>
                <span class="ContentTextBlack">
                    <li style="padding-left: 10px; padding-right: 10px;">
                        <%# (string )Eval("Name") + ": " + (string )Eval("Value")%></li></span>
            </ItemTemplate>
            <FooterTemplate>
                </ol>
            </FooterTemplate>
        </asp:DataList>
        <center>
            <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="false"></asp:Label>
        </center>
    </div>
    </form>
</body>
</html>
