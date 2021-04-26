<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PrintCertificate.aspx.cs"
    Inherits="PrintCertificate" %>

<%@ Import Namespace="AdvantShop.Catalog" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="Admin/css/AdminStyle.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function position_this_window() {
            var x = (screen.availWidth - 770) / 2;
            window.resizeTo(762, 662);
            window.moveTo(Math.floor(x), 50);
        }
    </script>
    <title></title>
</head>
<body onload="position_this_window();window.print();">
    <form id="form1" runat="server">
    <center style="font-family: Arial">
        <table style="width: 98%; padding-left: 10px;">
            <tr>
                <td>
                    <center>
                        <asp:Label ID="lblCertificateID" CssClass="AdminHead" runat="server"></asp:Label><br />
                        <asp:Label ID="lblCertificateStatus" CssClass="AdminSubHead" runat="server"></asp:Label><br />
                        <br />
                    </center>
                    <b><%= Resources.Resource.Admin_PrintCertificate_Code %> </b>
                    <asp:Label ID="lblCertificateCode" runat="server"></asp:Label>
                    <br />
                    <b><%= Resources.Resource.Admin_PrintCertificate_OrderNumber %></b>
                    <asp:Label ID="lblOrderNumber" runat="server"></asp:Label>
                    <br />
                    <b><%= Resources.Resource.Admin_PrintCertificate_From %></b>
                    <asp:Label ID="lblFrom" runat="server"></asp:Label>
                    <br />
                    <b><%= Resources.Resource.Admin_PrintCertificate_To %></b>
                    <asp:Label ID="lblTo" runat="server"></asp:Label>
                    <br />
                    <b><%= Resources.Resource.Admin_PrintCertificate_Sum %></b>
                    <asp:Label ID="lblSum" runat="server"></asp:Label>
                    <br />
                    <b><%= Resources.Resource.Admin_PrintCertificate_DeliveryMethod %></b>
                    <asp:Label ID="lblType" runat="server"></asp:Label>
                    <br />
                    <asp:Panel ID="pnlEmail" runat="server">
                        <b>Email</b>
                        <asp:Label ID="lblEmail" runat="server"></asp:Label>
                        <br />
                    </asp:Panel>
                    <asp:Panel ID="pnlMail" runat="server">
                        <b><%= Resources.Resource.Admin_PrintCertificate_Address %></b>
                        <asp:Label ID="lblAddress" runat="server"></asp:Label>
                    </asp:Panel>                    
                    <br />
                    <br />
                    <asp:Label ID="lblUserMessage" runat="server" Text="<%$ Resources:Resource, Client_PrintOrder_YourComment %>"></asp:Label>
                </td>
            </tr>
        </table>
    </center>
    </form>
</body>
</html>
