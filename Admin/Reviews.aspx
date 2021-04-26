<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="false"
    CodeFile="Reviews.aspx.cs" Inherits="Admin_Reviews" %>

<%@ MasterType VirtualPath="~/Admin/MasterPageAdmin.master" %>
<asp:Content ID="ContentHead" ContentPlaceHolderID="ContentPlaceHolder_Head" runat="Server">
</asp:Content>
<asp:Content ID="RootContent" ContentPlaceHolderID="cphMain" runat="Server">
    <div id="inprogress" style="display: none;">
        <div id="curtain" class="opacitybackground">
            &nbsp;</div>
        <div class="loader">
            <table width="100%" style="font-weight: bold; text-align: center;">
                <tbody>
                    <tr>
                        <td align="center">
                            <img src="images/ajax-loader.gif" alt="" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="color: #0D76B8;">
                            <asp:Localize ID="Localize_Admin_Catalog_PleaseWait" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_PleaseWait %>"></asp:Localize>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    <div style="text-align: center;">
        <table cellpadding="0" cellspacing="0" width="100%" style="padding-left: 10px">
            <tbody>
                <tr>
                    <td style="width: 72px;">
                        <img src="images/orders_ico.gif" alt="" />
                    </td>
                    <td>
                        <asp:Label ID="lblReview" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_Reviews_Reviews %>" />
                        <br />
                        <asp:Label ID="lblReviewName" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_Reviews_ReviewsView %>" />
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div style="height: 10px;">
    </div>
    <div style="padding-left: 7px; padding-right: 7px">
        <div id="gridTable" runat="server">
            <div>
                <table style="width: 99%;" class="massaction">
                    <tr>
                        <td>
                            <span class="admin_catalog_commandBlock"><span style="display: inline-block; margin-right: 3px;">
                                <asp:Localize ID="Localize_Admin_Catalog_Command" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_Command %>"></asp:Localize>
                            </span><span style="display: inline-block">
                                <select id="commandSelect">
                                    <option value="selectAll">
                                        <asp:Localize ID="Localize_Admin_Catalog_SelectAll" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_SelectAll %>"></asp:Localize>
                                    </option>
                                    <option value="unselectAll">
                                        <asp:Localize ID="Localize_Admin_Catalog_UnselectAll" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_UnselectAll %>"></asp:Localize>
                                    </option>
                                    <option value="selectVisible">
                                        <asp:Localize ID="Localize_Admin_Catalog_SelectVisible" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_SelectVisible %>"></asp:Localize>
                                    </option>
                                    <option value="unselectVisible">
                                        <asp:Localize ID="Localize_Admin_Catalog_UnselectVisible" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_UnselectVisible %>"></asp:Localize>
                                    </option>
                                    <option value="deleteSelected">
                                        <asp:Localize ID="Localize_Admin_Catalog_DeleteSelected" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_DeleteSelected %>"></asp:Localize>
                                    </option>
                                    <option value="setChecked">
                                        <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_SetChecked %>"></asp:Localize>
                                    </option>
                                    <option value="setNotChecked">
                                        <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_SetNotChecked %>"></asp:Localize>
                                    </option>
                                </select>
                                <input id="commandButton" type="button" value="GO" style="font-size: 10px; width: 38px;
                                    height: 20px;" />
                                <asp:LinkButton ID="lbDeleteSelected" Style="display: none" runat="server" OnClick="lbDeleteSelected_Click" />
                                <asp:LinkButton ID="lbSetChecked" Style="display: none" runat="server" OnClick="lbSetChecked_Click" />
                                <asp:LinkButton ID="lbSetNotChecked" Style="display: none" runat="server" OnClick="lbSetNotChecked_Click" />
                                <asp:LinkButton ID="lbChangeStatus" Style="display: none" runat="server" />
                            </span><span class="selecteditems" style="vertical-align: baseline"><span style="padding: 0px 3px 0px 3px;">
                                |</span><span id="selectedIdsCount" class="bold">0</span>&nbsp;<span><%=Resources.Resource.Admin_Catalog_ItemsSelected %></span></span></span>
                        </td>
                        <td align="right" class="selecteditems">
                            <asp:UpdatePanel ID="upCounts" runat="server" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnFilter" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="Click" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:Localize ID="Localize_Admin_Catalog_Total" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_Total %>"></asp:Localize>
                                    <span class="bold">
                                        <asp:Label ID="lblFound" CssClass="foundrecords" runat="server" Text="" />
                                    </span>
                                    <asp:Localize ID="Localize_Admin_Catalog_RecordsFound" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_RecordsFound %>"></asp:Localize>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td style="width: 8px;">
                        </td>
                    </tr>
                </table>
                <div style="border: 1px #c9c9c7 solid; width: 100%">
                    <asp:Panel runat="server" ID="filterPanel" DefaultButton="btnFilter">
                        <table class="filter" cellpadding="2" cellspacing="0">
                            <tr>
                                <td colspan="7">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 74px; text-align: center;" rowspan="2">
                                    <div style="height: 0px; font-size: 0px; width: 74px">
                                    </div>
                                    <asp:DropDownList ID="ddSelect" TabIndex="10" CssClass="dropdownselect" runat="server"
                                        Width="65px">
                                        <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>" />
                                        <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" />
                                        <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" />
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 175px;" rowspan="2">
                                    <div style="height: 0px; font-size: 0px; width: 124px">
                                    </div>
                                </td>
                                <td style="width: 175px;" rowspan="2">
                                    <div style="height: 0px; font-size: 0px; width: 110px">
                                    </div>
                                    <asp:TextBox CssClass="filtertxtbox" ID="txtName" Width="100%" runat="server" TabIndex="11" />
                                </td>
                                <td style="width: 175px;" rowspan="2">
                                    <div style="height: 0px; font-size: 0px; width: 134px">
                                    </div>
                                    <asp:TextBox CssClass="filtertxtbox" ID="txtEmail" Width="100%" runat="server" TabIndex="11" />
                                </td>
                                <td rowspan="2">
                                    <div style="height: 0px; font-size: 0px; width: 200px">
                                    </div>
                                    <asp:TextBox CssClass="filtertxtbox" ID="txtText" Width="100%" runat="server" TabIndex="11" />
                                </td>
                                <td style="width: 112px; text-align: right; padding-right: 4px">
                                    <span class="textfromto">
                                        <%=Resources.Resource.Admin_Catalog_From %>:</span>
                                    <asp:TextBox CssClass="filtertxtbox" ID="txtDateFrom" runat="server" Font-Size="10px"
                                        Width="65" TabIndex="21" />
                                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txtDateFrom"
                                        MaskType="Date" Mask="99/99/9999" MessageValidatorTip="true" runat="server">
                                    </ajaxToolkit:MaskedEditExtender>
                                </td>
                                <td style="width: 20px;">
                                    <asp:Image ID="popupDateFrom" runat="server" ImageUrl="~/Admin/images/Calendar_scheduleHS.png" />
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDateFrom"
                                        PopupButtonID="popupDateFrom" CssClass="popupCalendar">
                                    </ajaxToolkit:CalendarExtender>
                                </td>
                                <td style="width: 110px; text-align: center;" rowspan="2">
                                    <div style="height: 0px; font-size: 0px; width: 94px">
                                    </div>
                                    <asp:DropDownList ID="ddlChecked" TabIndex="10" CssClass="dropdownselect" runat="server"
                                        Width="65px">
                                        <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>"
                                            Value="-1" />
                                        <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" Value="1" />
                                        <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" Value="0" />
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 54px; padding-right: 10px; text-align: center;" rowspan="2">
                                    <asp:Button ID="btnFilter" runat="server" CssClass="btn" OnClientClick="javascript:FilterClick();"
                                        TabIndex="23" Text="<%$ Resources:Resource, Admin_Catalog_Filter %>" OnClick="btnFilter_Click" />
                                    <asp:Button ID="btnReset" runat="server" CssClass="btn" OnClientClick="javascript:ResetFilter();"
                                        TabIndex="24" Text="<%$ Resources:Resource, Admin_Catalog_Reset %>" OnClick="btnReset_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 110px; text-align: right; padding-right: 4px">
                                    <span class="textfromto">
                                        <%=Resources.Resource.Admin_Catalog_To %>:</span>
                                    <asp:TextBox CssClass="filtertxtbox" ID="txtDateTo" runat="server" Font-Size="10px"
                                        Width="65" TabIndex="22" />
                                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txtDateTo"
                                        MaskType="Date" Mask="99/99/9999" MessageValidatorTip="true" OnInvalidCssClass="invalidDate"
                                        ErrorTooltipEnabled="true" runat="server">
                                    </ajaxToolkit:MaskedEditExtender>
                                </td>
                                <td style="width: 20px;">
                                    <asp:Image ID="popupDateTo" runat="server" ImageUrl="~/Admin/images/Calendar_scheduleHS.png" />
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtDateTo"
                                        PopupButtonID="popupDateTo" CssClass="popupCalendar">
                                    </ajaxToolkit:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="7">
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:UpdatePanel ID="UpdatePanelGrid" runat="server" UpdateMode="Conditional">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="pageNumberer" EventName="SelectedPageChanged" />
                            <asp:AsyncPostBackTrigger ControlID="lbDeleteSelected" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnFilter" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnGo" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="ddRowsPerPage" EventName="SelectedIndexChanged" />
                        </Triggers>
                        <ContentTemplate>
                            <adv:AdvGridView ID="grid" runat="server" AllowSorting="true" AutoGenerateColumns="False"
                                CellPadding="5" CellSpacing="0" Confirmation="<%$ Resources:Resource, Admin_Reviews_Confirmation %>"
                                CssClass="tableview" GridLines="None" TooltipTextCellIndex="5" DataKeyNames=""
                                OnRowDataBound="grid_RowDataBound" OnSorting="grid_Sorting" OnRowCommand="grid_RowCommand">
                                <Columns>
                                    <asp:TemplateField AccessibleHeaderText="Id" Visible="false" HeaderStyle-Width="100%">
                                        <ItemTemplate>
                                            <asp:Label ID="Label01" runat="server" Text='<%# Bind("ID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-CssClass="checkboxcolumnheader" ItemStyle-CssClass="checkboxcolumn"
                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="70">
                                        <HeaderTemplate>
                                            <div style="height: 0px; width: 70px; font-size: 0px;">
                                            </div>
                                            <asp:CheckBox ID="checkBoxCheckAll" CssClass="checkboxcheckall headerCb" runat="server"
                                                onclick="javascript:SelectVisible(this.checked);" Style="margin-left: 0px;" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%# (bool) Eval("IsSelected")? "<input type='checkbox' class='sel' checked='checked' />": "<input type='checkbox' class='sel' />"%>
                                            <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Eval("ID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Photo" HeaderStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="175">
                                        <HeaderTemplate>
                                            <div style="height: 0px; width: 175px; font-size: 0px;">
                                            </div>
                                            <%= Resources.Resource.Admin_Reviews_ProdPhoto%>
                                            <asp:Image ID="arrowProductPhoto" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif"
                                                Visible="false" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:HyperLink ID="lblName" runat="server" Text='<%# GetRequestName((int)Eval("ID")) %>'
                                                NavigateUrl='<%# GetRequestUrl((int)Eval("ID")) %>' /><br />
                                            <asp:HyperLink ID="ibPhoto" CssClass="imgtooltip" runat="server" ImageUrl='' AlternateText='' />
                                            <asp:HiddenField ID="hfID" runat="server" Value='<%# Eval("ID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="175">
                                        <HeaderTemplate>
                                            <div style="height: 0px; width: 175px; font-size: 0px;">
                                            </div>
                                            <asp:LinkButton ID="lbOrderName" runat="server" CommandName="Sort" CommandArgument="Name">
                                                <div style="width: 75px; float: left;">
                                                    <%= Resources.Resource.Admin_ReviewsBlock_UserName%></div>
                                                <asp:Image ID="arrowName" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtName" runat="server" Text='<%# Eval("Name") %>' Width="99%"></asp:TextBox>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Email" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="200">
                                        <HeaderTemplate>
                                            <div style="height: 0px; width: 200px; font-size: 0px;">
                                            </div>
                                            <asp:LinkButton ID="lbOrderEmail" runat="server" CommandName="Sort" CommandArgument="Email">
                                                <div style="width: 95px; float: left;">
                                                    <%= Resources.Resource.Admin_ReviewsBlock_Email%></div>
                                                <asp:Image ID="arrowEmail" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblEmail" runat="server" Text='<%# Eval("Email") %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEmail" runat="server" Text='<%# Eval("Email") %>' Width="99%"></asp:TextBox>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="[Text]" ItemStyle-HorizontalAlign="Left"
                                        HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100%">
                                        <HeaderTemplate>
                                            <div style="height: 0px; width: 200px; font-size: 0px;">
                                            </div>
                                            <%= Resources.Resource.Admin_Reviews_Text%>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblText" runat="server" Text='<%# Eval("[Text]") %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtText" runat="server" Text='<%# Eval("[Text]") %>' Width="99%"
                                                TextMode="MultiLine" Rows="4" Wrap="true" Style="overflow: auto"></asp:TextBox>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:Resource, Admin_Reviews_AddDate %>"
                                        ItemStyle-CssClass="coladdDate" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100%">
                                        <HeaderTemplate>
                                            <div style="height: 0px; width: 110px; font-size: 0px;">
                                            </div>
                                            <asp:LinkButton ID="lbAddDate" runat="server" CommandName="Sort" CommandArgument="AddDate">
                                                <div style="width: 80px; float: left;">
                                                    <%=Resources.Resource.Admin_Reviews_AddDate%></div>
                                                <asp:Image ID="arrowAddDate" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif"
                                                    Style="float: left;" />
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblAddDate" runat="server" Text='<%# AdvantShop.Localization.Culture.ConvertDate((DateTime)Eval("AddDate")) %>' />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                        <ItemStyle CssClass="colmodify"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Checked" HeaderStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="100%" ItemStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>
                                            <div style="height: 0px; width: 90px; font-size: 0px;">
                                            </div>
                                            <asp:LinkButton ID="lbOrderChecked" runat="server" CommandName="Sort" CommandArgument="Checked">
                                                <%= Resources.Resource.Admin_Reviews_Checked%>
                                                <asp:Image ID="arrowChecked" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cbChecked" runat="server" Checked='<%# Eval("Checked")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="59" AccessibleHeaderText="Buttons" ItemStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>
                                            <div style="height: 0px; width: 60px; font-size: 0px;">
                                            </div>
                                        </HeaderTemplate>
                                        <EditItemTemplate>
                                            <asp:LinkButton ID="buttonDelete" runat="server" CssClass="deletebtn showtooltip"
                                                ToolTip='<%$ Resources:Resource, Admin_MasterPageAdminCatalog_Delete %>' CommandName="DeleteReview"
                                                CommandArgument='<%# Eval("ID") %>'>
                                                <asp:Image ID="Image1" ImageUrl="images/deletebtn.png" runat="server" />
                                            </asp:LinkButton>
                                            <ajaxToolkit:ConfirmButtonExtender ID="ConfirmButtonExtenderbuttonDelete" runat="server"
                                                TargetControlID="buttonDelete" ConfirmText="<%$ Resources: Resource, Admin_Reviews_Confirmation %>">
                                            </ajaxToolkit:ConfirmButtonExtender>
                                            <input id="btnUpdate" name="btnUpdate" class="updatebtn showtooltip" type="image"
                                                src="images/updatebtn.png" onclick="<%#ClientScript.GetPostBackEventReference(grid, "Update$" + Container.DataItemIndex)%>;return false;"
                                                style="display: none" title='<%= Resources.Resource.Admin_MasterPageAdminCatalog_Update %>' />
                                            <input id="btnCancel" name="btnCancel" class="cancelbtn showtooltip" type="image"
                                                src="images/cancelbtn.png" onclick="row_canceledit($(this).parent().parent()[0]);return false;"
                                                style="display: none" title='<%=Resources.Resource.Admin_MasterPageAdminCatalog_Cancel %>' />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="header" />
                                <RowStyle CssClass="row1 readonlyrow" />
                                <AlternatingRowStyle CssClass="row2 readonlyrow" />
                                <EmptyDataTemplate>
                                    <div style="text-align: center; margin-top: 20px; margin-bottom: 20px;">
                                        <asp:Localize ID="Localize_Admin_Catalog_NoRecords" runat="server" Text="<%$ Resources:Resource, Admin_Reviews_EmptyDataPage %>"></asp:Localize>
                                    </div>
                                </EmptyDataTemplate>
                            </adv:AdvGridView>
                            <div style="border-top: 1px #c9c9c7 solid;">
                            </div>
                            <table class="results2">
                                <tr>
                                    <td style="width: 157px; padding-left: 6px;">
                                        <asp:Localize ID="Localize_Admin_Catalog_ResultPerPage" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_ResultPerPage %>"></asp:Localize>:&nbsp;<asp:DropDownList
                                            ID="ddRowsPerPage" runat="server" OnSelectedIndexChanged="btnFilter_Click" CssClass="droplist"
                                            AutoPostBack="true">
                                            <asp:ListItem>10</asp:ListItem>
                                            <asp:ListItem>20</asp:ListItem>
                                            <asp:ListItem>50</asp:ListItem>
                                            <asp:ListItem>100</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td align="center">
                                        <adv:PageNumberer CssClass="PageNumberer" ID="pageNumberer" runat="server" DisplayedPages="7"
                                            UseHref="false" OnSelectedPageChanged="pn_SelectedPageChanged" UseHistory="false" />
                                    </td>
                                    <td style="width: 157px; text-align: right; padding-right: 12px">
                                        <asp:Panel ID="goToPage" runat="server" DefaultButton="btnGo">
                                            <span style="color: #494949">
                                                <%=Resources.Resource.Admin_Catalog_PageNum%>&nbsp;<asp:TextBox ID="txtPageNum" runat="server"
                                                    Width="30" /></span>
                                            <asp:Button ID="btnGo" runat="server" CssClass="btn" Text="<%$ Resources:Resource, Admin_Catalog_GO %>" />
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <input type="hidden" id="SelectedIds" name="SelectedIds" />
                <asp:Label ID="Message" runat="server" Font-Bold="True" ForeColor="Red" Visible="False" /><br />
            </div>
        </div>
    </div>
    <%--        </ContentTemplate>
    </asp:UpdatePanel>--%>
    <script type="text/javascript">

        $(document).ready(function () {
            $("input.showtooltip").tooltip({
                showURL: false
            });
        });

        function hide_wait_for_node(node) {
            if (node.wait_img) {
                node.removeChild(node.wait_img);
            }
        }

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
            document.onkeydown = keyboard_navigation;
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_beginRequest(Darken);
            prm.add_endRequest(Clear);
            initgrid();
            $("ineditcategory").tooltip();
        });

        function removeunloadhandler() {
            window.onbeforeunload = null;
        }


        function beforeunload(e) {
            if ($("img.floppy:visible, img.exclamation:visible").length > 0) {
                var evt = window.event || e;
                evt.returnValue = '<%=Resources.Resource.Admin_OrderSearch_LostChanges%>';
            }
        }

        function addbeforeunloadhandler() {
            window.onbeforeunload = beforeunload;
        }

        function selectCange() {
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
                        var r = confirm("<%= Resources.Resource.Admin_Reviews_Confirm%>");
                        if (r) __doPostBack('<%= lbDeleteSelected.UniqueID %>', '');
                        break;
                    case "setChecked":
                        document.getElementById('<%=lbSetChecked.ClientID%>').click();
                        break;
                    case "setNotChecked":
                        document.getElementById('<%=lbSetNotChecked.ClientID%>').click();
                        break;
                }
            });
        }

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () { selectCange(); });

        
    </script>
</asp:Content>
