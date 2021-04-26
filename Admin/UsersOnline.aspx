<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="UsersOnline.aspx.cs" Inherits="Admin_UsersOnline" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Head" runat="Server">
    <script type="text/javascript">

        function CreateHistory(hist) {
            $.historyLoad(hist);
        }

        var timeOut;
        function Darken() {
            timeOut = setTimeout('document.getElementById("inprogress").style.display = "block";', 1000);
        }

        function Clear() {
            clearTimeout(timeOut);
            document.getElementById("inprogress").style.display = "none";

            $("input.sel").each(function (i) {
                if (this.checked) $(this).parent().parent().addClass("selectedrow");
            });

            initgrid();
        }

        $(document).ready(function () {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_beginRequest(Darken);
            prm.add_endRequest(Clear);
            initgrid();
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="Server">
    <div style="width: 100%">
        <center>
            <table style="width: 98%">
                <tr>
                    <td style="width: 52px;">
                        <img alt="" src="images/customers_ico.gif" />
                    </td>
                    <td style="width: 100%">
                        <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_OnLineUsers_Header %>"></asp:Label><br />
                        <asp:Label ID="lblSubHead" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_OnLineUsers_SubHeader %>"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div style="height: 3px; font-size: 3px">
                            &nbsp;
                        </div>
                        <adv:AdvGridView ID="grid" AutoGenerateColumns="false" runat="server" CssClass="tableview"
                            ReadOnlyGrid="true" CellPadding="2" CellSpacing="0" Width="100%" GridLines="None">
                            <Columns>
                                <asp:TemplateField HeaderText="Start time" AccessibleHeaderText="Start time" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="Left" ItemStyle-Width="160px">
                                    <HeaderTemplate>
                                        <span>&nbsp;<% = Resources.Resource.Admin_OnLineUsers_StartTime%></span>
                                    </HeaderTemplate>
                                    <EditItemTemplate>
                                        &nbsp;<%# Eval("Started") %>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        &nbsp;<%# Eval("Started") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Address" AccessibleHeaderText="Address" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="Left" ItemStyle-Width="100px">
                                    <HeaderTemplate>
                                        <span>
                                            <% = Resources.Resource.Admin_OnLineUsers_IPAddress%></span>
                                    </HeaderTemplate>
                                    <EditItemTemplate>
                                        <%# Eval("Address") %>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <%# Eval("Address") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="User Country" AccessibleHeaderText="User Country"
                                    HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="150px">
                                    <HeaderTemplate>
                                        <span>
                                            <% = Resources.Resource.Admin_OnLineUsers_UserAgentCountry%></span>
                                    </HeaderTemplate>
                                    <EditItemTemplate>
                                        <%# Eval("CountryByGeoIp")%>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <%# Eval("CountryByGeoIp")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Last page" AccessibleHeaderText="Last page" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="Left">
                                    <HeaderTemplate>
                                        <span>
                                            <% = Resources.Resource.Admin_OnLineUsers_LastPage%></span>
                                    </HeaderTemplate>
                                    <EditItemTemplate>
                                        <%# Eval("LastAccessedPath") %>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <%# Eval("LastAccessedPath") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="User agent" AccessibleHeaderText="User agent" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="Left" ItemStyle-Width="225px">
                                    <HeaderTemplate>
                                        <span>
                                            <% = Resources.Resource.Admin_OnLineUsers_UserAgentBrowser%></span>
                                    </HeaderTemplate>
                                    <EditItemTemplate>
                                        <%# RenderBrowserIcon((string)Eval("UserAgentBrowser"))%>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <%# RenderBrowserIcon((string)Eval("UserAgentBrowser"))%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="User OS" AccessibleHeaderText="User OS" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="Left" ItemStyle-Width="200px">
                                    <HeaderTemplate>
                                        <span>
                                            <% = Resources.Resource.Admin_OnLineUsers_UserAgentOS%></span>
                                    </HeaderTemplate>
                                    <EditItemTemplate>
                                        <%# RenderOsIcon((string)Eval("UserAgentOS")) %>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <%# RenderOsIcon((string)Eval("UserAgentOS")) %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="header" />
                            <RowStyle CssClass="row1 propertiesRow_25 readonlyrow" />
                            <AlternatingRowStyle CssClass="row2 propertiesRow_25_alt readonlyrow" />
                            <EmptyDataTemplate>
                                <center style="margin-top: 20px; margin-bottom: 20px;">
                                    <%=Resources.Resource.Admin_Catalog_NoRecords%>
                                </center>
                            </EmptyDataTemplate>
                        </adv:AdvGridView>
                    </td>
                </tr>
            </table>
        </center>
    </div>
</asp:Content>
