<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="Taxes.aspx.cs" Inherits="Admin_Taxs" %>

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
    <div style="padding-left: 10px; padding-right: 10px">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tbody>
                <tr>
                    <td style="width: 72px;">
                        <img src="images/orders_ico.gif" alt="" />
                    </td>
                    <td>
                        <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text="<%$ Resources: Resource, Admin_Taxes_Header %>"></asp:Label><br />
                        <asp:Label ID="lblSubHead" CssClass="AdminSubHead" runat="server" Text="<%$ Resources: Resource, Admin_Taxes_SubHeader %>"></asp:Label>
                    </td>
                    <td style="vertical-align:bottom;">
                        <div style="float: right; padding-right: 20px;">
                            <adv:AdvButton ID="btnAddTax" runat="server" Width="170" Text="<%$ Resources: Resource, Admin_Taxes_Add %>"
                                CssMain="adv_OrangeButton_Main" CssInput="adv_OrangeButton_Input" CssInputMozz="adv_OrangeButton_Input-mozz"
                                CssLeftDiv="adv_OrangeButton_LeftDiv" CssRightDiv="adv_OrangeButton_RightDiv"
                                CssCenterDiv="adv_OrangeButton_CenterDiv" ValidationGroup="0" OnClick="btnAddTax_Click" />
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
        <div style="width: 100%">
            <div style="height: 10px">
            </div>
            <table style="width: 99%;" class="massaction">
                <tr>
                    <td>
                        <span class="admin_catalog_commandBlock"><span style="display: inline-block; margin-right: 3px;">
                            <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_Command %>"></asp:Localize>
                        </span><span style="display: inline-block">
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
                            |</span><span id="selectedIdsCount" class="bold">0</span>&nbsp;<span><%=Resources.Resource.Admin_Catalog_ItemsSelected%></span>
                        </span></span>
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
                            <td colspan="6">
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
                            <td>
                                <div style="width: 150px; height: 0px; font-size: 0px;">
                                </div>
                                <asp:TextBox CssClass="filtertxtbox" ID="txtName" Width="98%" runat="server" TabIndex="12" />
                            </td>
                            <td>
                                <div style="width: 200px; height: 0px; font-size: 0px;">
                                </div>
                                <asp:TextBox CssClass="filtertxtbox" ID="txtRegNumber" Width="98%" runat="server"
                                    TabIndex="12" />
                            </td>
                            <td style="text-align: center;">
                                <div style="width: 100px; font-size: 0px; height: 0px;">
                                </div>
                                <asp:DropDownList ID="ddlEnabled" TabIndex="18" CssClass="dropdownselect" runat="server">
                                    <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>" />
                                    <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" />
                                    <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" />
                                </asp:DropDownList>
                            </td>
                            <td style="width: 300px;">
                                <div style="width: 300px; font-size: 0px; height: 0px;">
                                </div>
                                <asp:DropDownList Style="width: 98%" CssClass="dropdownselect" ID="ddlCountryFilter"
                                    runat="server" DataTextField="Name" DataValueField="CountryID">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 80px;">
                                <center>
                                    <asp:Button ID="btnFilter" runat="server" CssClass="btn" OnClientClick="javascript:FilterClick();"
                                        TabIndex="23" Text="<%$ Resources:Resource, Admin_Catalog_Filter %>" OnClick="btnFilter_Click" />
                                    <asp:Button ID="btnReset" runat="server" CssClass="btn" OnClientClick="javascript:ResetFilter();"
                                        TabIndex="24" Text="<%$ Resources:Resource, Admin_Catalog_Reset %>" OnClick="btnReset_Click" />
                                </center>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 5px;" colspan="6">
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
                            CellPadding="2" CellSpacing="0" Confirmation="<%$ Resources:Resource, Admin_Taxes_Confirmation %>"
                            CssClass="tableview" Style="cursor: pointer" DataFieldForEditURLParam="" DataFieldForImageDescription=""
                            DataFieldForImagePath="" EditURL="" GridLines="None" OnRowCommand="grid_RowCommand"
                            OnSorting="grid_Sorting" OnRowDataBound="grid_RowDataBound" OnRowDeleting="grid_RowDeleting"
                            ShowFooter="false" ShowFooterWhenEmpty="true" ShowHeader="true" ShowHeaderWhenEmpty="true">
                            <Columns>
                                <asp:TemplateField AccessibleHeaderText="ID" Visible="false">
                                    <EditItemTemplate>
                                        <asp:Label ID="Label0" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label01" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="Label01" runat="server" Text='0'></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-CssClass="checkboxcolumn" HeaderStyle-Width="70px" FooterStyle-Width="70px"
                                    ItemStyle-Width="70px" HeaderStyle-HorizontalAlign="Center">
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
                                        &nbsp;
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="Name" HeaderStyle-HorizontalAlign="Left">
                                    <HeaderTemplate>
                                        <div style="width: 150px; font-size: 0px; height: 0px;">
                                        </div>
                                        <asp:LinkButton ID="lbName" runat="server" CommandName="Sort" CommandArgument="Name">
                                            <%=Resources.Resource.Admin_Taxes_Name%>
                                            <asp:Image ID="arrowName" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" /></asp:LinkButton>
                                    </HeaderTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtName" Width="98%" runat="server" Text='<%# Eval("Name") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lName" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtNewName" Width="98%" CssClass="add" runat="server" Text=''></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="RegNumber" HeaderStyle-HorizontalAlign="Left">
                                    <HeaderTemplate>
                                        <div style="width: 200px; font-size: 0px; height: 0px;">
                                        </div>
                                        <asp:LinkButton ID="lbRegNumber" runat="server" CommandName="Sort" CommandArgument="RegNumber">
                                            <%=Resources.Resource.Admin_Taxes_RegNumber%>
                                            <asp:Image ID="arrowRegNumber" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" /></asp:LinkButton>
                                    </HeaderTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtRegNumber" runat="server" Text='<%# Eval("RegNumber") %>' Width="98%"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lRegNumber" runat="server" Text='<%# Bind("RegNumber") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtNewRegNumber" CssClass="add" runat="server" Text='' Width="98%"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="Enable" ItemStyle-HorizontalAlign="Center"
                                    FooterStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                        <div style="width: 100px; font-size: 0px; height: 0px;">
                                        </div>
                                        <asp:LinkButton ID="lbEnabled" runat="server" CommandName="Sort" CommandArgument="Enabled">
                                            <%= Resources.Resource.Admin_Taxes_Enabled %>
                                            <asp:Image ID="arrowEnabled" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" /></asp:LinkButton>
                                    </HeaderTemplate>
                                    <EditItemTemplate>
                                        <asp:CheckBox ID="CheckBox2" runat="server" Checked='<%# Bind("Enabled") %>' />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("Enabled") %>' Enabled="false" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddlNewEnabled" TabIndex="18" CssClass="add" runat="server">
                                            <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" Value="true" />
                                            <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" Value="false" />
                                        </asp:DropDownList>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField AccessibleHeaderText="CountryID" HeaderStyle-Width="300px" FooterStyle-Width="300px"
                                    ItemStyle-Width="300px" HeaderText="<%$ Resources: Resource, Admin_Taxes_Country %>">
                                    <HeaderTemplate>
                                        <div style="width: 300px; font-size: 0px; height: 0px;">
                                        </div>
                                        <asp:LinkButton ID="lbCountryID" runat="server" CommandName="Sort" CommandArgument="[Tax].CountryID"
                                            Style="text-align: center;">
                                            <%= Resources.Resource.Admin_Taxes_Country %>
                                            <asp:Image ID="arrowCountryID" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                        </asp:LinkButton>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Eval("CountryName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList Style="width: 98%" ID="ddlCountry" runat="server" DataTextField="Name"
                                            DataValueField="CountryID">
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList Style="width: 98%" ID="ddlCountry" CssClass="add" runat="server"
                                            DataTextField="Name" DataValueField="CountryID">
                                        </asp:DropDownList>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="80px" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="center"
                                    FooterStyle-HorizontalAlign="center">
                                    <EditItemTemplate>
                                        <a id="cmdlink" href='Tax.aspx?TaxID=<%# Eval("ID") %>' class="editbtn showtooltip"
                                            title='<%=Resources.Resource.Admin_MasterPageAdminCatalog_Edit%>'>
                                            <img src="images/editbtn.gif" style="border: none;" /></a>
                                        <input id="btnUpdate" name="btnUpdate" class="updatebtn showtooltip" type="image"
                                            src="images/updatebtn.png" onclick="<%#ClientScript.GetPostBackEventReference(grid, "Update$" + Container.DataItemIndex)%>;return false;"
                                            style="display: none" title='<%=
                                                          Resources.Resource.Admin_MasterPageAdminCatalog_Update%>' />
                                        <asp:ImageButton ID="buttonDelete" runat="server" ImageUrl="images/deletebtn.png"
                                            CssClass="deletebtn showtooltip" CommandName="Delete" CommandArgument='<%# Eval("ID")%>'
                                            ToolTip='<%$ Resources:Resource, Admin_MasterPageAdminCatalog_Delete %>' />
                                        <ajaxToolkit:ConfirmButtonExtender ID="ConfirmButtonExtender2" runat="server" TargetControlID="buttonDelete"
                                            ConfirmText="<%$ Resources:Resource, Admin_Taxes_Confirmation %>">
                                        </ajaxToolkit:ConfirmButtonExtender>
                                        <input id="btnCancel" name="btnCancel" class="cancelbtn showtooltip" type="image"
                                            src="images/cancelbtn.png" onclick="row_canceledit($(this).parent().parent()[0]);return false;"
                                            style="display: none" title="<%=Resources.Resource.Admin_MasterPageAdminCatalog_Cancel %>" />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:ImageButton ID="ibAddTax" runat="server" ImageUrl="images/addbtn.gif" CommandName="AddTax"
                                            ToolTip="<%$ Resources:Resource, Admin_Taxes_Add  %>" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <FooterStyle />
                            <HeaderStyle CssClass="header" />
                            <RowStyle CssClass="row1 propertiesRow_25 readonlyrow" />
                            <AlternatingRowStyle CssClass="row2 propertiesRow_25_alt readonlyrow" />
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
            <input type="hidden" id="SelectedIds" name="SelectedIds" />
        </div>
    </div>
    <script type="text/javascript">
        function setupTooltips() {
            $(".showtooltip").tooltip({
                showURL: false
            });
        }
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () { setupTooltips(); });
    </script>
</asp:Content>
