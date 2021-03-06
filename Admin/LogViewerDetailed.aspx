<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="LogViewerDetailed.aspx.cs" Inherits="Admin_LogViewerDetailed" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="Server">
    <div>
        <div style="text-align: center;">
            <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_MasterPageAdmin_BugTracker_HeaderDetails %>"></asp:Label>
            <br />
            <br />
            /&nbsp;<asp:HyperLink ID="hfBack" runat="server" CssClass="Link"><%= Resources.Resource.Admin_MasterPageAdmin_BugTracker_GoBack%></asp:HyperLink>&nbsp;/
            <br />
            <br />
            <asp:Label runat="server" ID="lblErr" ForeColor="Red"></asp:Label>
            <br />
            <table class="subform" id="tabs">
                <tr>
                    <td style="width: 225px;">
                        <ul id="tabs-headers">
                            <li id="commoninfo">
                                <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Resource, Admin_MasterPageAdmin_BugTracker_CommonInfo %>" />
                            </li>
                            <li id="request">
                                <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Resource, Admin_MasterPageAdmin_BugTracker_Request %>" />
                            </li>
                            <li id="browser">
                                <asp:Literal ID="Literal6" runat="server" Text="<%$ Resources:Resource, Admin_MasterPageAdmin_BugTracker_Browser %>" /></li>
                            <li id="session">
                                <asp:Literal ID="Literal7" runat="server" Text="<%$ Resources:Resource, Admin_MasterPageAdmin_BugTracker_Session %>" />
                            </li>
                        </ul>
                    </td>
                    <td id="tabs-contents">
                        <%=OutHtml.ToString() %>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
