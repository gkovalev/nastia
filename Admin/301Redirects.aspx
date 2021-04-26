<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="301Redirects.aspx.cs" Inherits="Admin_301Redirects" %>

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
            var prm = window.Sys.WebForms.PageRequestManager.getInstance();
            prm.add_beginRequest(Darken);
            prm.add_endRequest(Clear);
            initgrid();
        });

        $(document).ready(function () {
            $("#commandButton").click(function () {
                var command = $("#commandSelect").val();

                switch (command) {
                    case "selectAll":
                        SelectAll(true);
                        break;
                    case "unselectAll":
                        SelectAll(false);
                        break;
                    case "selectVisible":
                        SelectVisible(true);
                        break;
                    case "unselectVisible":
                        SelectVisible(false);
                        break;
                    case "deleteSelected":
                        var r = confirm("<%= Resources.Resource.Admin_Catalog_Confirm%>");
                        if (r) window.__doPostBack('<%=lbDeleteSelected.UniqueID%>', '');
                        break;
                }
            });
        });

    </script>
    <style type="text/css">
        .style1
        {
            height: 24px;
        }
        .style2
        {
            width: 8px;
            height: 24px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="Server">
    <div id="inprogress" style="display: none;">
        <div id="curtain" class="opacitybackground">
            &nbsp;</div>
        <div class="loader">
            <table width="100%" style="font-weight: bold; text-align: center;">
                <tbody>
                    <tr>
                        <td style="text-align: center;">
                            <img src="images/ajax-loader.gif" alt="" />
                        </td>
                    </tr>
                    <tr>
                        <td style="color: #0D76B8; text-align: center;">
                            <asp:Localize ID="Localize_Admin_Properties_PleaseWait" runat="server" Text="<%$ Resources:Resource, Admin_Properties_PleaseWait %>"></asp:Localize>
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
                        <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_301Redirects_Header %>"></asp:Label><br />
                        <asp:Label ID="lblSubHead" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_301Redirects_SubHeader %>"></asp:Label>
                        <br />
                        <asp:CheckBox ID="chbEnabled301Redirect" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_Use_301_Redirects %>"
                            OnCheckedChanged="chbEnabled301Redirect_CheckedChanged" AutoPostBack="True" />
                    </td>
                    <td style="vertical-align: bottom; padding-right: 10px">
                        <div style="float: right; padding-right: 10px">
                            <adv:AdvButton ID="btnAddRedirectSeo" runat="server" Width="170" Text="<%$ Resources:Resource, Admin_301Redirects_AddRecord %>"
                                CssMain="adv_OrangeButton_Main" CssInput="adv_OrangeButton_Input" CssInputMozz="adv_OrangeButton_Input-mozz"
                                CssLeftDiv="adv_OrangeButton_LeftDiv" CssRightDiv="adv_OrangeButton_RightDiv"
                                CssCenterDiv="adv_OrangeButton_CenterDiv" ValidationGroup="0" OnClick="btnAddRedirectSeo_Click" />
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
        <div style="width: 100%">
            <table style="width: 99%;" class="massaction">
                <tr>
                    <td class="style1">
                        <span class="admin_catalog_commandBlock"><span style="display: inline-block; margin-right: 3px;">
                            <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_Command %>"></asp:Localize>
                        </span><span style="display: inline-block;">
                            <select id="commandSelect">
                                <option value="selectAll">
                                    <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_SelectAll %>"></asp:Localize>
                                </option>
                                <option value="unselectAll">
                                    <asp:Localize ID="Localize3" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_UnselectAll %>"></asp:Localize>
                                </option>
                                <option value="selectVisible">
                                    <asp:Localize ID="Localize4" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_SelectVisible %>"></asp:Localize>
                                </option>
                                <option value="unselectVisible">
                                    <asp:Localize ID="Localize5" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_UnselectVisible %>"></asp:Localize>
                                </option>
                                <option value="deleteSelected">
                                    <asp:Localize ID="Localize6" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_DeleteSelected %>"></asp:Localize>
                                </option>
                            </select>
                            <input id="commandButton" type="button" value="GO" style="font-size: 10px; width: 38px;
                                height: 20px;" />
                            <asp:LinkButton ID="lbDeleteSelected" Style="display: none" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_DeleteSelected %>"
                                OnClick="lbDeleteSelected_Click" />
                        </span><span class="selecteditems" style="vertical-align: baseline"><span style="padding: 0px 3px 0px 3px;">
                            |</span><span id="selectedIdsCount" class="bold">0</span>&nbsp;<span><%=Resources.Resource.Admin_Catalog_ItemsSelected%></span></span>
                        </span>
                    </td>
                    <td class="style1" style="text-align: right;">
                        <asp:UpdatePanel ID="upCounts" runat="server">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnFilter" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="Click" />
                            </Triggers>
                            <ContentTemplate>
                                <%=Resources.Resource.Admin_Catalog_Total%>
                                <span class="bold">
                                    <asp:Label ID="lblFound" CssClass="foundrecords" runat="server" Text="" /></span>&nbsp;<%=Resources.Resource.Admin_Catalog_RecordsFound%>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td class="style2">
                    </td>
                </tr>
            </table>
            <div style="border: 1px #c9c9c7 solid; width: 100%">
                <asp:Panel runat="server" ID="filterPanel" DefaultButton="btnFilter">
                    <table class="filter" cellpadding="2" cellspacing="0">
                        <tr style="height: 5px;">
                            <td colspan="5">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 70px; text-align: center;">
                                <asp:DropDownList ID="ddSelect" TabIndex="10" CssClass="dropdownselect" runat="server"
                                    Width="65">
                                    <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>" />
                                    <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" />
                                    <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" />
                                </asp:DropDownList>
                            </td>
                            <td style="width: 100px;">
                                <div style="width: 100px; font-size: 0px; height: 0px;">
                                </div>
                                <asp:TextBox CssClass="filtertxtbox" ID="txtId" Width="99%" runat="server" TabIndex="12" />
                            </td>
                            <td>
                                <div style="width: 100px; font-size: 0px; height: 0px;">
                                </div>
                                <asp:TextBox CssClass="filtertxtbox" ID="txtRedirectFrom" Width="99%" runat="server"
                                    TabIndex="12" />
                            </td>
                            <td>
                                <div style="width: 100px; font-size: 0px; height: 0px;">
                                </div>
                                <asp:TextBox CssClass="filtertxtbox" ID="txtRedirectTo" Width="99%" runat="server"
                                    TabIndex="12" />
                            </td>
                            <td width="260px">
                                <div style="width: 260px; font-size: 0px; height: 0px;">
                                        </div>
                            </td>
                            <td style="width: 90px;">
                                <div style="width: 90px; font-size: 0px; height: 0px;">
                                </div>
                                <div style="text-align: center;">
                                    <asp:Button ID="btnFilter" runat="server" CssClass="btn" OnClientClick="javascript:FilterClick();"
                                        TabIndex="23" Text="<%$ Resources:Resource, Admin_Catalog_Filter %>" OnClick="btnFilter_Click" />
                                    <asp:Button ID="btnReset" runat="server" CssClass="btn" OnClientClick="javascript:ResetFilter();"
                                        TabIndex="24" Text="<%$ Resources:Resource, Admin_Catalog_Reset %>" OnClick="btnReset_Click" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 5px;" colspan="5">
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="pageNumberer" EventName="SelectedPageChanged" />
                        <asp:AsyncPostBackTrigger ControlID="lbDeleteSelected" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnFilter" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnGo" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="ddRowsPerPage" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="btnAddRedirectSeo" EventName="Click" />
                    </Triggers>
                    <ContentTemplate>
                        <adv:AdvGridView ID="grid" runat="server" AllowSorting="true" AutoGenerateColumns="False"
                            CellPadding="2" CellSpacing="0" Confirmation="<%$ Resources:Resource, Admin_Country_Confirmation %>"
                            CssClass="tableview" Style="cursor: pointer" DataFieldForEditURLParam="" DataFieldForImageDescription=""
                            DataFieldForImagePath="" EditURL="" GridLines="None" OnRowCommand="grid_RowCommand"
                            OnSorting="grid_Sorting" OnRowDataBound="grid_RowDataBound" ShowFooter="false">
                            <Columns>
                                <asp:TemplateField ItemStyle-CssClass="checkboxcolumn" HeaderStyle-Width="70px" ItemStyle-Width="70px"
                                    HeaderStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                        <div style="width: 70px; height: 0px; font-size: 0px;">
                                        </div>
                                        <asp:CheckBox ID="checkBoxCheckAll" CssClass="checkboxcheckall" runat="server" onclick="javascript:SelectVisible(this.checked);" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# ((bool)Eval("IsSelected"))? "<input type='checkbox' class='sel' checked='checked' />": "<input type='checkbox' class='sel' />"%>
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Eval("ID") %>' />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="ID" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="70px"
                                    ItemStyle-Width="70px">
                                    <HeaderTemplate>
                                        <div style="width: 70px; font-size: 0px; height: 0px;">
                                        </div>
                                        <asp:LinkButton ID="lbId" runat="server" CommandName="Sort" CommandArgument="ID">
                                            ID
                                            <asp:Image ID="arrowId" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" /></asp:LinkButton>
                                    </HeaderTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="txtId" runat="server" Text='<%# Eval("Id") %>' Width="99%"></asp:Label>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lId" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="RedirectFrom" HeaderStyle-HorizontalAlign="Left">
                                    <HeaderTemplate>
                                        <div style="width: 100px; font-size: 0px; height: 0px;">
                                        </div>
                                        <asp:LinkButton ID="lbRedirectFrom" runat="server" CommandName="Sort" CommandArgument="RedirectFrom">
                                            <%=Resources.Resource.Admin_301Redirects_From %>
                                            <asp:Image ID="arrowRedirectFrom" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" /></asp:LinkButton>
                                    </HeaderTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtRedirectFrom" runat="server" Text='<%# Eval("RedirectFrom") %>'
                                            Width="99%"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lRedirectFrom" runat="server" Text='<%# Bind("RedirectFrom") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtNexRedirectFrom" CssClass="add" runat="server" Text='' Width="99%"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="RedirectTo" HeaderStyle-HorizontalAlign="Left">
                                    <HeaderTemplate>
                                        <div style="width: 100px; font-size: 0px; height: 0px;">
                                        </div>
                                        <asp:LinkButton ID="lbRedirectTo" runat="server" CommandName="Sort" CommandArgument="RedirectTo">
                                            <%=Resources.Resource.Admin_301Redirects_To %>
                                            <asp:Image ID="arrowRedirectTo" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" /></asp:LinkButton>
                                    </HeaderTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtRedirectTo" runat="server" Text='<%# Eval("RedirectTo") %>' Width="99%"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lRedirectTo" runat="server" Text='<%# Bind("RedirectTo") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtNewRedirectTo" CssClass="add" runat="server" Text='' Width="99%"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="ProductArtNo" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="260">
                                    <HeaderTemplate>
                                        <div style="width: 260px; font-size: 0px; height: 0px;">
                                        </div>
                                            <%=Resources.Resource.Admin_301Redirects_SKU %>
                                    </HeaderTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtProductArtNo" runat="server" Text='<%# Eval("ProductArtNo") %>' Width="99%"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lProductArtNo" runat="server" Text='<%# Eval("ProductArtNo") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtNewProductArtNo" CssClass="add" runat="server" Text='' Width="99%"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="90px" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="center"
                                    FooterStyle-HorizontalAlign="Center">
                                    <EditItemTemplate>
                                        <input id="btnUpdate" name="btnUpdate" class="updatebtn showtooltip" type="image"
                                            src="images/updatebtn.png" onclick="<%#ClientScript.GetPostBackEventReference(grid, "Update$" + Container.DataItemIndex)%>;return false;"
                                            style="display: none" title='<%= Resources.Resource.Admin_MasterPageAdminCatalog_Update%>' />
                                        <asp:ImageButton ID="buttonDelete" runat="server" ImageUrl="images/deletebtn.png"
                                            CssClass="deletebtn showtooltip" CommandName="DeleteRedirectSeo" CommandArgument='<%# Eval("ID")%>'
                                            ToolTip='<%$ Resources:Resource, Admin_MasterPageAdminCatalog_Delete %>' />
                                        <ajaxToolkit:ConfirmButtonExtender ID="ConfirmButtonExtender2" runat="server" TargetControlID="buttonDelete"
                                            ConfirmText="<%$ Resources:Resource, Admin_301Redirect_Confirmation %>">
                                        </ajaxToolkit:ConfirmButtonExtender>
                                        <input id="btnCancel" name="btnCancel" class="cancelbtn showtooltip" type="image"
                                            src="images/cancelbtn.png" onclick="row_canceledit($(this).parent().parent()[0]);return false;"
                                            style="display: none" title="<%=Resources.Resource.Admin_MasterPageAdminCatalog_Cancel %>" />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:ImageButton ID="ibAddRedirectSeo" runat="server" ImageUrl="images/addbtn.gif"
                                            CommandName="AddRedirectSeo" ToolTip="<%$ Resources:Resource, Admin_301Redirects_AddRedirect   %>" />
                                        <asp:ImageButton ID="ibCancelAdd" runat="server" ImageUrl="images/cancelbtn.png"
                                            CommandName="CancelAdd" ToolTip="<%$ Resources:Resource, Admin_Country_CancelAdd  %>" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <FooterStyle BackColor="#ccffcc" />
                            <HeaderStyle CssClass="header" />
                            <RowStyle CssClass="row1 propertiesRow_25 readonlyrow" />
                            <AlternatingRowStyle CssClass="row2 propertiesRow_25_alt readonlyrow" />
                            <EmptyDataTemplate>
                                <div style="text-align: center; margin-top: 20px; margin-bottom: 20px;">
                                    <%=Resources.Resource.Admin_301Redirects_NoRecords%>
                                </div>
                            </EmptyDataTemplate>
                        </adv:AdvGridView>
                        <div style="border-top: 1px #c9c9c7 solid;">
                        </div>
                        <table class="results2">
                            <tr>
                                <td style="width: 157px; padding-left: 6px;">
                                    <%=Resources.Resource.Admin_Catalog_ResultPerPage%>:&nbsp;<asp:DropDownList ID="ddRowsPerPage"
                                        runat="server" OnSelectedIndexChanged="btnFilter_Click" CssClass="droplist" AutoPostBack="true">
                                        <asp:ListItem>10</asp:ListItem>
                                        <asp:ListItem Selected="True">20</asp:ListItem>
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
                        <asp:Label ID="lblMes" runat="server" ForeColor="Red"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <input type="hidden" id="SelectedIds" name="SelectedIds" />
        </div>
    </div>
</asp:Content>
