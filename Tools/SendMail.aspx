<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SendMail.aspx.cs" Inherits="Tools_SendMail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AdvantShop.NET Tools - Send mail test</title>
    <style type="text/css" >
        .Header1 {font-family: Tahoma; font-weight: bold;}
        .ContentDiv {font:0.75em 'Lucida Grande', sans-serif;}
        .Label {font-family: Tahoma; font-size: 16px; color: #666666;}
        .clsTextBase {width: 150px;}
        .clsText {border:1px solid #DDDDDD; padding:3px; font-size:14px;}
        .clsText_faild {border:1px solid #E5A3A3; padding:3px; font-size:14px; background-color: #FFCFCF;}
        .label-box {border-color:#DBDBDB; border-style:solid; border-width:1px 1px 1px 1px; color:#666666; display:none; font-size:14px; line-height:1.45em; padding:0.85em 10px 0.85em 10px; text-transform:lowercase; width: 735px; display: block;}
        .label-box.good {background-color:#D3F9BF; border-color:#E1EFDB;}
        .label-box.error {background-color:#FFCFCF; background-image:none; border-color:#E5A3A3; color:#801B1B; padding-left:10px;}
        .btn {background:url("img/bg-btn.gif") repeat-x scroll 0 0 #DDDDDD; border-color:#DDDDDD #DDDDDD #CCCCCC; border-style:solid; border-width:1px; color:#333333; cursor:pointer; font:11px/14px "Lucida Grande",sans-serif; margin:0; overflow:visible; padding:4px 8px 5px; width:auto;}
        .btn-m {background-position:0 -200px; font-size:15px; line-height:20px !important; padding:5px 15px 6px;}
        .btn-m:hover, .btn-m:focus {background-position:0 -206px;}
        .fieldsetData {margin-bottom:22px; padding: 12px 15px 15px 15px; font-family:Tahoma; }
        .tableActualData {font-size: 14px;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <span style="font-family: Tahoma; font-weight: bold;">AdvantShop.NET tools for 3.x</span>
    -
    <asp:HyperLink ID="HyperLink6" runat="server" ForeColor="Green" NavigateUrl="Default.aspx"
        Text="Back to main page"></asp:HyperLink>
    <br />
    <br />

    <%--<fieldset style="margin-bottom: 22px; padding: 15px 15px 15px 15px;">--%>

        <div class="ContentDiv">
            <table>
                <tbody>
                    <tr>
                        <td>
                           <span class="Label">Smtp server</span>&nbsp;&nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtSmtp" runat="server" CssClass="clsTextBase clsText" ></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="Label">Login</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLogin" runat="server" CssClass="clsTextBase clsText" ></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="Label">Password</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPassword" runat="server" CssClass="clsTextBase clsText" ></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="Label">From</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFrom" runat="server" CssClass="clsTextBase clsText" ></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="Label">To</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTo" runat="server" CssClass="clsTextBase clsText"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="Label">Subject</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSubject" runat="server" CssClass="clsTextBase clsText" Width="375px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="Label">Message</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMessage" runat="server" CssClass="clsTextBase clsText" 
                                TextMode="MultiLine" Height="112px" Width="499px"></asp:TextBox>
                        </td>
                    </tr>
                </tbody>
            </table>
            <br />
            <asp:Button ID="btnSendMail" runat="server" Text="SendMail" CssClass="btn btn-m" OnClick="btnSendMail_Click" />
            <br />
            <br />
            <asp:Literal ID="Message" runat="server"></asp:Literal>
        </div>
    </form>
</body>
</html>
