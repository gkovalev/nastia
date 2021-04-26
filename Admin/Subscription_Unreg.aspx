<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="Subscription_Unreg.aspx.cs" Inherits="Admin_SubscriptionUnreg" %>

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
        })

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
                        if (r) __doPostBack('<%=lbDeleteSelected.UniqueID%>', '');
                        break;
                }
            });
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="Server">
    <div style="padding-left: 10px; padding-right: 10px">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tbody>
                <tr>
                    <td style="width: 72px;">
                        <img src="images/orders_ico.gif" alt="" />
                    </td>
                    <td>
                        <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_Subscription_Unreg_Header %>"></asp:Label><br />
                        <asp:Label ID="lblSubHead" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_Subscription_Unreg_SubHeader %>"></asp:Label>
                    </td>
                    <td>
                        <div style="float: right; padding-right: 20px">
                            <div style="height: 41px" id="advButtonFiller" runat="server">
                            </div>
                            <adv:AdvButton ID="btnAdmin_SubscriptionReg" runat="server" Width="300" Text="<%$ Resources:Resource, Admin_Subscription_SubHeader%>"
                                CssMain="adv_OrangeButton_Main" CssInput="adv_OrangeButton_Input" CssInputMozz="adv_OrangeButton_Input-mozz"
                                CssLeftDiv="adv_OrangeButton_LeftDiv" CssRightDiv="adv_OrangeButton_RightDiv"
                                CssCenterDiv="adv_OrangeButton_CenterDiv3" ValidationGroup="0" OnClick="btnAdmin_SubscriptionReg_Click" />
                            <adv:AdvButton ID="btnAdmin_SubscriptionDeactivatereason" runat="server" Width="300"
                                Text="<%$ Resources:Resource, Admin_Subscription_DeactivateReason_SubHeader%>"
                                CssMain="adv_OrangeButton_Main" CssInput="adv_OrangeButton_Input" CssInputMozz="adv_OrangeButton_Input-mozz"
                                CssLeftDiv="adv_OrangeButton_LeftDiv" CssRightDiv="adv_OrangeButton_RightDiv"
                                CssCenterDiv="adv_OrangeButton_CenterDiv3" ValidationGroup="0" OnClick="btnAdmin_Subscription_DeactivateReason_Click" />
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
        <asp:UpdatePanel ID="updPanel" runat="server">
            <ContentTemplate>
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
                                        <asp:Localize ID="Localize_Admin_Properties_PleaseWait" runat="server" Text="<%$ Resources:Resource, Admin_Properties_PleaseWait %>"></asp:Localize>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
                <div style="width: 100%">
                    <table style="width: 99%;" class="massaction">
                        <tr>
                            <td>
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
                                    |</span><span id="selectedIdsCount" class="bold">0</span>&nbsp;<span><%=Resources.Resource.Admin_Catalog_ItemsSelected%></span></span></span>
                            </td>
                            <td align="right" class="selecteditems">
                                <asp:UpdatePanel ID="upCounts" runat="server" UpdateMode="Conditional">
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
                            <td style="width: 8px;">
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
                                    <td style="width: 70px; text-align: center;" rowspan="2">
                                        <asp:DropDownList ID="ddSelect" TabIndex="10" CssClass="dropdownselect" runat="server"
                                            Width="65">
                                            <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>" />
                                            <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" />
                                            <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" />
                                        </asp:DropDownList>
                                    </td>
                                    <td rowspan="2">
                                        <div style="width: 100px; font-size: 0px; height: 0px;">
                                        </div>
                                        <asp:TextBox CssClass="filtertxtbox" ID="txtEmail" Width="99%" runat="server" TabIndex="12" />
                                    </td>
                                    <td style="width: 140px;" rowspan="2">
                                        <div style="width: 140px; font-size: 0px; height: 0px;">
                                        </div>
                                    </td>
                                    <td style="width: 140px;" rowspan="2">
                                        <div style="width: 140px; font-size: 0px; height: 0px;">
                                        </div>
                                    </td>
                                    <td style="width: 100px; text-align: center;" rowspan="2">
                                        <asp:DropDownList ID="ddEnable" TabIndex="10" CssClass="dropdownselect" runat="server"
                                            Width="100">
                                            <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>" />
                                            <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" />
                                            <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" />
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 120px; text-align: right; padding-right: 4px">
                                        <span class="textfromto">
                                            <%=Resources.Resource.Admin_Catalog_From%>:</span><asp:TextBox CssClass="filtertxtbox"
                                                ID="txtAddDateFrom" runat="server" Font-Size="10px" Width="64" TabIndex="21" />
                                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txtAddDateFrom"
                                            MaskType="Date" Mask="99/99/9999" MessageValidatorTip="true" runat="server">
                                        </ajaxToolkit:MaskedEditExtender>
                                    </td>
                                    <td style="width: 16px; padding-right: 30px;">
                                        <asp:Image ID="popupAddDateFrom" runat="server" ImageUrl="~/Admin/images/Calendar_scheduleHS.png" />
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtAddDateFrom"
                                            PopupButtonID="popupAddDateFrom" CssClass="popupCalendar">
                                        </ajaxToolkit:CalendarExtender>
                                    </td>
                                    <td style="width: 120px; text-align: right; padding-right: 4px">
                                        <span class="textfromto">
                                            <%=Resources.Resource.Admin_Catalog_From%>:</span><asp:TextBox CssClass="filtertxtbox"
                                                ID="txtActivatedDateFrom" runat="server" Font-Size="10px" Width="64" TabIndex="21" />
                                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender3" TargetControlID="txtActivatedDateFrom"
                                            MaskType="Date" Mask="99/99/9999" MessageValidatorTip="true" runat="server">
                                        </ajaxToolkit:MaskedEditExtender>
                                    </td>
                                    <td style="width: 16px; padding-right: 30px;">
                                        <asp:Image ID="popupActivateDateFrom" runat="server" ImageUrl="~/Admin/images/Calendar_scheduleHS.png" />
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtActivatedDateFrom"
                                            PopupButtonID="popupActivateDateFrom" CssClass="popupCalendar">
                                        </ajaxToolkit:CalendarExtender>
                                    </td>
                                    <td style="width: 90px;" rowspan="2">
                                        <div style="width: 90px; font-size: 0px; height: 0px;">
                                        </div>
                                        <center>
                                            <asp:Button ID="btnFilter" runat="server" CssClass="btn" OnClientClick="javascript:FilterClick();"
                                                TabIndex="23" Text="<%$ Resources:Resource, Admin_Catalog_Filter %>" OnClick="btnFilter_Click" />
                                            <asp:Button ID="btnReset" runat="server" CssClass="btn" OnClientClick="javascript:ResetFilter();"
                                                TabIndex="24" Text="<%$ Resources:Resource, Admin_Catalog_Reset %>" OnClick="btnReset_Click" />
                                        </center>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 120px; text-align: right; padding-right: 4px">
                                        <span class="textfromto">
                                            <%=Resources.Resource.Admin_Catalog_To%>:</span><asp:TextBox CssClass="filtertxtbox"
                                                ID="txtAddDateTo" runat="server" Font-Size="10px" Width="64" TabIndex="22" />
                                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txtAddDateTo"
                                            MaskType="Date" Mask="99/99/9999" MessageValidatorTip="true" OnInvalidCssClass="invalidDate"
                                            ErrorTooltipEnabled="true" runat="server">
                                        </ajaxToolkit:MaskedEditExtender>
                                    </td>
                                    <td style="width: 16px; padding-right: 30px;">
                                        <asp:Image ID="popupAddDateTo" runat="server" ImageUrl="~/Admin/images/Calendar_scheduleHS.png" />
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtAddDateTo"
                                            PopupButtonID="popupAddDateTo" CssClass="popupCalendar">
                                        </ajaxToolkit:CalendarExtender>
                                    </td>
                                    <td style="width: 120px; text-align: right; padding-right: 4px">
                                        <span class="textfromto">
                                            <%=Resources.Resource.Admin_Catalog_To%>:</span><asp:TextBox CssClass="filtertxtbox"
                                                ID="txtActivatedDateTo" runat="server" Font-Size="10px" Width="64" TabIndex="22" />
                                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender4" TargetControlID="txtActivatedDateTo"
                                            MaskType="Date" Mask="99/99/9999" MessageValidatorTip="true" OnInvalidCssClass="invalidDate"
                                            ErrorTooltipEnabled="true" runat="server">
                                        </ajaxToolkit:MaskedEditExtender>
                                    </td>
                                    <td style="width: 16px; padding-right: 30px;">
                                        <asp:Image ID="popupActivateDateTo" runat="server" ImageUrl="~/Admin/images/Calendar_scheduleHS.png" />
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtActivatedDateTo"
                                            PopupButtonID="popupActivateDateTo" CssClass="popupCalendar">
                                        </ajaxToolkit:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 5px;" colspan="5">
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
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
                                    CellPadding="2" CellSpacing="0" Confirmation="<%$ Resources:Resource, Admin_Subscription_Unreg_QDelete %>"
                                    CssClass="tableview" Style="cursor: pointer" DataFieldForEditURLParam="" DataFieldForImageDescription=""
                                    DataFieldForImagePath="" EditURL="" GridLines="None" OnRowCommand="grid_RowCommand"
                                    OnSorting="grid_Sorting" ShowFooter="false">
                                    <Columns>
                                        <asp:TemplateField AccessibleHeaderText="ID" Visible="false">
                                            <EditItemTemplate>
                                                <asp:Label ID="Label0" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label01" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-CssClass="checkboxcolumn" ItemStyle-Width="70px" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <div style="width: 70px; height: 0px; font-size: 0px;">
                                                </div>
                                                <asp:CheckBox ID="checkBoxCheckAll" CssClass="checkboxcheckall" runat="server" onclick="javascript:SelectVisible(this.checked);" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# ((bool)Eval("IsSelected"))? "<input type='checkbox' class='sel' checked='checked' />": "<input type='checkbox' class='sel' />"%>
                                                <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Eval("ID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="Email" HeaderStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                <div style="width: 100px; font-size: 0px; height: 0px;">
                                                </div>
                                                <asp:LinkButton ID="lbEmail" runat="server" CommandName="Sort" CommandArgument="Email">
                                                    <%=Resources.Resource.Admin_Subscription_Unreg_Email%>
                                                    <asp:Image ID="arrowEmail" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" /></asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lEmail" runat="server" Text='<%# Bind("Email") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="ActivateCode" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-Width="140px">
                                            <HeaderTemplate>
                                                <div style="width: 140px; font-size: 0px; height: 0px;">
                                                </div>
                                                <%=Resources.Resource.Admin_Subscription_Unreg_ActivateCode%>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lActivateCode" runat="server" Text='<%# Bind("ActivateCode") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="DectivateCode" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-Width="140px">
                                            <HeaderTemplate>
                                                <div style="width: 140px; font-size: 0px; height: 0px;">
                                                </div>
                                                <%=Resources.Resource.Admin_Subscription_Unreg_DeactivateCode%>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lDectivateCode" runat="server" Text='<%# Bind("DectivateCode") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="Enable" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <div style="width: 100px; height: 0px; font-size: 0px;">
                                                </div>
                                                <asp:LinkButton ID="lbEnable" runat="server" CommandName="Sort" CommandArgument="Enable">
                                                    <%=Resources.Resource.Admin_Subscription_Unreg_Status%>
                                                    <asp:Image ID="arrowEnable" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" /></asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="cbEnable" runat="server" Checked='<%# Eval("Enable")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="AddDate" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-Width="170px">
                                            <HeaderTemplate>
                                                <div style="width: 170px; font-size: 0px; height: 0px;">
                                                </div>
                                                <asp:LinkButton ID="lbAddDate" runat="server" CommandName="Sort" CommandArgument="AddDate">
                                                    <%=Resources.Resource.Admin_Subscription_Unreg_Added%>
                                                    <asp:Image ID="arrowAddDate" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" /></asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lAddDate" runat="server" Text='<%# AdvantShop.Localization.Culture.ConvertShortDate((DateTime) Eval("AddDate"))  %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="ActivateDate" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-Width="170px">
                                            <HeaderTemplate>
                                                <div style="width: 170px; font-size: 0px; height: 0px;">
                                                </div>
                                                <asp:LinkButton ID="lbActivateDate" runat="server" CommandName="Sort" CommandArgument="ActivateDate">
                                                    <%=Resources.Resource.Admin_Subscription_Unreg_Activated%>
                                                    <asp:Image ID="arrowActivateDate" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" /></asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lActivateDate" runat="server" Text='<%# Eval("ActivateDate")!= DBNull.Value  ? AdvantShop.Localization.Culture.ConvertShortDate((DateTime)Eval("ActivateDate")): string.Empty  %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="Center"
                                            ItemStyle-Width="90px">
                                            <EditItemTemplate>
                                                <input id="btnUpdate" name="btnUpdate" class="updatebtn showtooltip" type="image"
                                                    src="images/updatebtn.png" onclick="<%#ClientScript.GetPostBackEventReference(grid, "Update$" + Container.DataItemIndex)%>;return false;"
                                                    style="display: none" title='<%= Resources.Resource.Admin_MasterPageAdminCatalog_Update%>' />
                                                <input id="btnCancel" name="btnCancel" class="cancelbtn showtooltip" type="image"
                                                    src="images/cancelbtn.png" onclick="row_canceledit($(this).parent().parent()[0]);return false;"
                                                    style="display: none" title="<%=Resources.Resource.Admin_MasterPageAdminCatalog_Cancel %>" />
                                                <asp:ImageButton ID="buttonDelete" runat="server" ImageUrl="images/deletebtn.png"
                                                    CssClass="deletebtn showtooltip" CommandName="DeleteUser" CommandArgument='<%# Eval("ID")%>'
                                                    ToolTip='<%$ Resources:Resource, Admin_MasterPageAdminCatalog_Delete %>' />
                                                <ajaxToolkit:ConfirmButtonExtender ID="ConfirmButtonExtender2" runat="server" TargetControlID="buttonDelete"
                                                    ConfirmText="<%$ Resources:Resource, Admin_Subscription_Unreg_QDelete %>">
                                                </ajaxToolkit:ConfirmButtonExtender>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="#ccffcc" />
                                    <HeaderStyle CssClass="header" />
                                    <RowStyle CssClass="row1 propertiesRow_25 readonlyrow" />
                                    <AlternatingRowStyle CssClass="row2 propertiesRow_25_alt readonlyrow" />
                                    <EmptyDataTemplate>
                                        <center style="margin-top: 20px; margin-bottom: 20px;">
                                            <%=Resources.Resource.Admin_Catalog_NoRecords%>
                                        </center>
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
                </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <input type="hidden" id="SelectedIds" name="SelectedIds" />
    </div>
</asp:Content>
