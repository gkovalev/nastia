<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="OptovikFiles.aspx.cs" Inherits="Admin_RootFiles" %>

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

        $(document).ready(function() {
            document.onkeydown = keyboard_navigation;
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_beginRequest(Darken);
            prm.add_endRequest(Clear);
            initgrid();
            $("ineditcategory").tooltip();
            $(".showtooltip").tooltip({
                showURL: false
            });
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="Server">
    <div id="inprogress" style="display: none;">
        <div id="curtain" class="opacitybackground">
            &nbsp;</div>
        <div class="loader">
            <table width="100%" style="font-weight: bold; text-align: center;">
                <tbody>
                    <tr>
                        <td align="center">
                            <img src="images/ajax-loader.gif" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="color: #0D76B8;">
                            <asp:Localize ID="Localize_Admin_Properties_PleaseWait" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_PleaseWait %>"></asp:Localize>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    <div style="padding-left: 10px; padding-right: 10px">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tbody>
                <tr>
                    <td style="width: 72px;">
                        <img src="images/orders_ico.gif" alt="" />
                    </td>
                    <td>
                        <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text="Файлы в директории Optovik"></asp:Label><br />
                        <asp:Label ID="lblSubHead" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_RootFiles_Subheader %>"></asp:Label>
                    </td>
                </tr>
            </tbody>
        </table>
        <table cellpadding="0" width="100%" cellspacing="0" style="padding-left: 10px; padding-right: 10px">

            <tr>
                <td style="width: 130px;">
                    <asp:Label ID="lbCheckFile" runat="server" Text="<%$ Resources:Resource, Admin_RootFiles_SelectFile %>">:
                    </asp:Label>
                </td>
                <td style="color: Red;">
                    <asp:FileUpload ID="FileLoad" runat="server" />
                    <asp:Label runat="server" ID="lblErrorFile" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="bthAddFile" runat="server" Text="<%$ Resources:Resource, Admin_RootFiles_Add %>"
                        OnClick="bthAddFile_Click" />
                </td>
            </tr>
        </table>
        <div style="width: 100%">
            <div>
                <table style="width: 99%;" class="massaction">
                    <tr>
                        <td align="right" class="selecteditems">
                            <asp:UpdatePanel ID="upCounts" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <%=Resources.Resource.Admin_Catalog_Total %>
                                    <span class="bold">
                                        <asp:Label ID="lblFound" CssClass="foundrecords" runat="server" Text="" /></span>&nbsp;<%= Resources.Resource.Admin_Catalog_RecordsFound %>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td style="width: 8px;">
                        </td>
                    </tr>
                </table>
                <div style="border: 1px #c9c9c7 solid; width: 100%">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="pageNumberer" EventName="SelectedPageChanged" />
                            <asp:AsyncPostBackTrigger ControlID="btnGo" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="ddRowsPerPage" EventName="SelectedIndexChanged" />
                        </Triggers>
                        <ContentTemplate>
                            <adv:AdvGridView ID="grid" runat="server" AllowSorting="False" AutoGenerateColumns="False"
                                CellPadding="0" CellSpacing="0" Confirmation="<%$ Resources:Resource, Admin_RootFiles_Confirmation %>"
                                CssClass="tableview" Style="cursor: pointer" DataFieldForEditURLParam="" DataFieldForImageDescription=""
                                DataFieldForImagePath="" EditURL="" GridLines="None" OnRowCommand="grid_RowCommand"
                                OnRowDeleting="grid_RowDeleting" ShowFooter="false">
                                <Columns>
                                    <asp:TemplateField ItemStyle-Width="15px" HeaderStyle-Width="15px" >
                                        <HeaderTemplate>
                                        </HeaderTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="FileName" HeaderStyle-HorizontalAlign="Left">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lbOrderFileName" runat="server">
                                                <%=Resources.Resource.Admin_RootFiles_FileName%>
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lFileName" runat="server" Text='<%# Bind("FileName") %>'></asp:Label>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lFileName" runat="server" Text='<%# Bind("FileName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="FileName" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200px">
                                        <EditItemTemplate>
                                            <asp:HyperLink ID="hlFile" runat="server" NavigateUrl='<%#RenderFileLink(Eval("FileName").ToString()) %>'>
                                                <%= Resources.Resource.Admin_RootFiles_Download %>
                                            </asp:HyperLink>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hlFile" runat="server" NavigateUrl='<%#RenderFileLink(Eval("FileName").ToString()) %>'>
                                                <%= Resources.Resource.Admin_RootFiles_Download %>
                                            </asp:HyperLink>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="30px" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Left"
                                        FooterStyle-HorizontalAlign="Left">
                                        <EditItemTemplate>
                                            <asp:ImageButton ID="buttonDelete" runat="server" ImageUrl="images/deletebtn.png"
                                                CssClass="showtooltip" CommandName="DeleteFile" CommandArgument='<%# Eval("FileName")%>'
                                                ToolTip='<%$ Resources:Resource, Admin_MasterPageAdminCatalog_Delete %>' />
                                            <ajaxToolkit:ConfirmButtonExtender ID="ConfirmButtonExtender2" runat="server" TargetControlID="buttonDelete"
                                                ConfirmText="<%$ Resources:Resource, Admin_RootFiles_Confirmation %>">
                                            </ajaxToolkit:ConfirmButtonExtender>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="buttonDelete" runat="server" ImageUrl="images/deletebtn.png"
                                                CssClass="deletebtn showtooltip" CommandName="DeleteFile" CommandArgument='<%# Eval("FileName")%>'
                                                ToolTip='<%$ Resources:Resource, Admin_MasterPageAdminCatalog_Delete %>' />
                                            <ajaxToolkit:ConfirmButtonExtender ID="ConfirmButtonExtender2" runat="server" TargetControlID="buttonDelete"
                                                ConfirmText="<%$ Resources:Resource, Admin_RootFiles_Confirmation %>">
                                            </ajaxToolkit:ConfirmButtonExtender>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="header" />
                                <RowStyle CssClass="row1 propertiesRow_25 readonlyrow" />
                                <AlternatingRowStyle CssClass="row2 propertiesRow_25_alt readonlyrow" />
                                <EmptyDataTemplate>
                                    <center style="margin-top: 20px; margin-bottom: 20px;">
                                        <%=Resources.Resource.Admin_RootFiles_NoRecords%>
                                    </center>
                                </EmptyDataTemplate>
                            </adv:AdvGridView>
                            <div style="border-top: 1px #c9c9c7 solid;">
                                <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                            </div>
                            <table class="results2">
                                <tr>
                                    <td style="width: 157px; padding-left: 6px;">
                                        <%=Resources.Resource.Admin_Catalog_ResultPerPage%>:&nbsp;<asp:DropDownList ID="ddRowsPerPage"
                                            runat="server" OnSelectedIndexChanged="btnFilter_Click" CssClass="droplist" AutoPostBack="true">
                                            <asp:ListItem>10</asp:ListItem>
                                            <asp:ListItem>20</asp:ListItem>
                                            <asp:ListItem>50</asp:ListItem>
                                            <asp:ListItem>100</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td align="center">
                                        <adv:PageNumberer CssClass="PageNumberer" ID="pageNumberer" runat="server" DisplayedPages="7"
                                            UseHref="false" UseHistory="false" OnSelectedPageChanged="pn_SelectedPageChanged" />
                                    </td>
                                    <td style="width: 157px; text-align: right; padding-right: 12px">
                                        <asp:Panel ID="goToPage" runat="server" DefaultButton="btnGo">
                                            <span style="color: #494949">
                                                <%=Resources.Resource.Admin_Catalog_PageNum%>&nbsp;<asp:TextBox ID="txtPageNum" runat="server"
                                                    Width="30" /></span>
                                            <asp:Button ID="btnGo" runat="server" CssClass="btn" Text="<%$ Resources:Resource, Admin_Catalog_GO %>"
                                                OnClick="linkGO_Click" />
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <input type="hidden" id="SelectedIds" name="SelectedIds" />
            </div>
        </div>
    </div>
</asp:Content>
