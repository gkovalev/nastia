<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="Module.aspx.cs" Inherits="Admin_Module" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Head" runat="Server">
    <script type="text/javascript">
        function showElement(span) {
            var method_id = $("input:hidden", $(span)).val();
            location = "module.aspx?module=<%= Server.UrlEncode(Request["module"]) %>&currentcontrolindex=" + method_id;
        }   
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="Server">
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td colspan="3" style="vertical-align: middle;">
                <img src="images/orders_ico.gif" alt="" style="float: left; margin-right: 10px; " />
                <div style="float: left;  ">
                    <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_Module_Header %>"></asp:Label><br />
                    <asp:Label ID="lblSubHead" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_Module_SubHeader %>"></asp:Label>
                </div>
            </td>
        </tr>
        <tr>
            <td style="vertical-align: top; width: 100%" colspan="3">
                <table cellpadding="0px" cellspacing="0px" style="width: 100%;">
                    <tr>
                        <td style="vertical-align: top; width: 225px;">
                            <ul class="tabs" id="tabs-headers">
                                <asp:Repeater runat="server" ID="rptTabs">
                                    <ItemTemplate>
                                        <li id="Li1" runat="server" onclick="javascript:showElement(this)" class='<%# Container.ItemIndex == CurrentControlIndex ? "selected" : "" %>'
                                            style="">
                                            <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Container.ItemIndex %>' />
                                            <asp:Label ID="Literal4" runat="server" Text='<%# Eval("NameTab") %>' />
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                        </td>
                        <td class="tabContainer" id="tabs-contents">
                            <asp:Panel ID="pnlBody" runat="server" Style="width: 100%">
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
    <br />
    <asp:Label runat="server" ID="lblInfo"></asp:Label>
</asp:Content>
