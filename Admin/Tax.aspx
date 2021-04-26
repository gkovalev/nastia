<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="Tax.aspx.cs" Inherits="Admin_Tax" %>

<%@ Register Src="UserControls/CatalogDataTreeViewForTaxes.ascx" TagName="CatalogDataTreeViewForTaxes"
    TagPrefix="adv" %>
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
    <div style="margin-left: 10px;">
        <table width="98%">
            <tr>
                <td style="width: 60px;">
                    <img src="images/orders_ico.gif" />
                </td>
                <td style="vertical-align: middle;">
                    <asp:Label ID="Label3" runat="server" CssClass="AdminHead" Text="<%# CurrentTax.Name %>"></asp:Label>
                </td>
                <td style="text-align:right">
                    <adv:OrangeRoundedButton ID="BtnSave" runat="server" Text="<%$ Resources: Resource, Admin_Tax_Save %>"
                        OnClick="saveClick" />
                </td>
            </tr>
        </table>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <asp:Label ID="Message" runat="server"></asp:Label></ContentTemplate>
        </asp:UpdatePanel>
        <table cellpadding="0px" cellspacing="0px" style="width: 98%; margin-left: 7px; margin-top: 10px;">
            <tr>
                <td style="vertical-align: top; width: 225px;">
                    <ul style="list-style: none; margin: 0px -5px 0px 0px; padding: 0px;">
                        <li id="li_1" onclick="javascript:showElement('1')" onmouseover="javascript:hoverElement('1')"
                            onmouseout="javascript:outElement('1')" class="selected" style="height: 18px;"><span>
                                <span style="float: left">
                                    <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources: Resource, Admin_Tax_Settings %>" />
                                </span><span id="iFloppy1" style="float: right; display: none; margin: 3px 5px 0 0">
                                    <img class="floppy" src="images/floppy.gif" />
                                </span></span></li>
                        <li id="li_2" onclick="javascript:showElement('2')" onmouseover="javascript:hoverElement('2')"
                            onmouseout="javascript:outElement('2')" class="free" style="height: 18px;"><span><span
                                style="float: left">
                                <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources: Resource, Admin_Tax_Regions %>" /></span>
                                <span id="iFloppy2" style="float: right; display: none; margin: 3px 5px 0 0">
                                    <img class="floppy" src="images/floppy.gif" />
                                </span></span></li>
                        <li id="li_3" onclick="javascript:showElement('3')" onmouseover="javascript:hoverElement('3')"
                            onmouseout="javascript:outElement('3')" class="free" style="height: 18px;"><span
                                style="float: left">
                                <asp:Literal ID="Literal7" runat="server" Text="<%$ Resources: Resource, Admin_Tax_ProductTree %>" /></span>
                                <span id="iFloppy3" style="float: right; display: none; margin: 3px 5px 0 0">
                                    <img class="floppy" src="images/floppy.gif" />
                                </span>
                                </li>
                    </ul>
                    <input type="hidden" runat="server" name="__liState" id="__liState" value="1" runat="server" />
                </td>
                <td style="background-color: #eff0f1;">
                    <div id="div_1" class="firstVTabPanel" style="height: 100%;">
                        <table style="width: 100%; padding-left: 5px; padding-right: 5px">
                            <tr>
                                <td class="formheader">
                                    <asp:Label runat="server" Font-Bold="true" Font-Size="14px" Text="<%$ Resources: Resource, Admin_Tax_Settings %>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="formPanel" UpdateMode="Conditional" runat="server">
                                        <ContentTemplate>
                                            <table border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
                                                        <%= Resources.Resource.Admin_Taxes_Name %>:
                                                    </td>
                                                    <td style="vertical-align: top;">
                                                        <asp:TextBox ID="txtName" runat="server" Text='<%# CurrentTax.Name %>'></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
                                                        <%= Resources.Resource.Admin_Taxes_Priority %>:
                                                    </td>
                                                    <td style="vertical-align: top;">
                                                        <asp:TextBox ID="txtPriority" runat="server" Text='<%# CurrentTax.Priority %>'></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
                                                        <%= Resources.Resource.Admin_Taxes_Enabled %>:
                                                    </td>
                                                    <td style="vertical-align: top;">
                                                        <asp:CheckBox ID="chkEnabled" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
                                                        <%= Resources.Resource.Admin_Taxes_DependsOn %>:
                                                    </td>
                                                    <td style="vertical-align: top;">
                                                        <asp:DropDownList ID="ddlDependsOnAddress" runat="server">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
                                                        <%= Resources.Resource.Admin_Taxes_ShowInPrice %>:
                                                    </td>
                                                    <td style="vertical-align: top;">
                                                        <asp:CheckBox ID="chkShowInPrice" runat="server" Checked='<%# CurrentTax.ShowInPrice %>' />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
                                                        <%= Resources.Resource.Admin_Taxes_RegNumber %>:
                                                    </td>
                                                    <td style="vertical-align: top;">
                                                        <asp:TextBox ID="txtRegNumber" runat="server" Text='<%# CurrentTax.RegNumber %>'></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
                                                        <%= Resources.Resource.Admin_Taxes_Country %>:
                                                    </td>
                                                    <td style="vertical-align: top;">
                                                        <asp:DropDownList ID="ddlCountry" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged"
                                                            Style="width: 200px;" AutoPostBack="true" runat="server" DataTextField="Name"
                                                            DataValueField="CountryID" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
                                                        <%= Resources.Resource.Admin_Taxes_RateType %>:
                                                    </td>
                                                    <td style="vertical-align: top;">
                                                        <asp:DropDownList ID="ddlRateType" runat="server">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
                                                        <%= Resources.Resource.Admin_Taxes_FederalRate %>:
                                                    </td>
                                                    <td style="vertical-align: top;">
                                                        <asp:TextBox ID="txtFederalRate" runat="server" Text='<%# CurrentTax.FederalRate.ToString("F2") %>' />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="div_2" class="vTabPanel" style="height: 100%;">
                        <table style="width: 100%; padding-left: 5px; padding-right: 5px">
                            <tr>
                                <td class="formheader">
                                    <asp:Label runat="server" Font-Bold="true" Font-Size="14px" Text="<%$ Resources: Resource, Admin_Tax_Regions %>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <div style="width: 100%">
                                            <asp:SqlDataSource ID="sdsRegion" runat="server" SelectCommand="SELECT RegionName, RegionID from [Customers].[Region] WHERE [CountryID] = @CountryID AND RegionID NOT IN (SELECT [TaxRegionRate].[RegionID] FROM [Catalog].[TaxRegionRate] WHERE [TaxId] = @TaxId)"
                                                OnInit="sds_Init">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="ddlCountry" PropertyName="SelectedValue" Name="CountryID"
                                                        Type="String" />
                                                    <asp:QueryStringParameter Name="TaxId" ConvertEmptyStringToNull="True" QueryStringField="TaxID" />
                                                </SelectParameters>
                                            </asp:SqlDataSource>
                                            <asp:UpdatePanel UpdateMode="Conditional" ID="regionsPanel" runat="server">
                                                <ContentTemplate>
                                                    <adv:AdvGridView ID="grid" runat="server" AllowSorting="true" AutoGenerateColumns="False"
                                                        CellPadding="2" CellSpacing="0" Confirmation="<%$ Resources:Resource, Admin_Taxes_Confirmation %>"
                                                        CssClass="tableview" DataFieldForEditURLParam="" DataFieldForImageDescription=""
                                                        DataFieldForImagePath="" EditURL="" GridLines="None" OnRowCommand="grid_Command"
                                                        OnSorting="grid_Sorting" OnRowDeleting="grid_RowDeleting" ShowFooter="true" ShowFooterWhenEmpty="true"
                                                        ShowHeader="true" ShowHeaderWhenEmpty="true">
                                                        <Columns>
                                                            <asp:TemplateField AccessibleHeaderText="RegionID" Visible="false">
                                                                <EditItemTemplate>
                                                                    <asp:Label ID="Label2" runat="server" Value='<%# Eval("RegionID") %>' />
                                                                </EditItemTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Label1" runat="server" Value='<%# Eval("RegionID") %>' />
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField AccessibleHeaderText="Name" HeaderStyle-HorizontalAlign="Left">
                                                                <HeaderTemplate>
                                                                    <div style="width: 100px; font-size: 0px; height: 0px;">
                                                                    </div>
                                                                    <asp:LinkButton ID="lbName" runat="server" CommandName="Sort" CommandArgument="RegionName">
                                                                        <%= Resources.Resource.Admin_Taxes_Region %>
                                                                        <asp:Image ID="arrowName" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" /></asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:HiddenField ID="hdRegionId" runat="server" Value='<%# Eval("RegionID") %>' />
                                                                    <asp:TextBox ID="txtName" runat="server" Text='<%# Eval("RegionName") %>' Width="99%"></asp:TextBox>
                                                                </EditItemTemplate>
                                                                <ItemTemplate>
                                                                    <asp:HiddenField ID="hdRegionId1" runat="server" Value='<%# Eval("RegionID") %>' />
                                                                    <asp:Label ID="lName" runat="server" Text='<%# Bind("RegionName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:DropDownList ID="ddlRegionNew" DataSourceID="sdsRegion" class="add" DataTextField="RegionName"
                                                                        DataValueField="RegionID" runat="server">
                                                                    </asp:DropDownList>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField AccessibleHeaderText="RegionRate" HeaderStyle-HorizontalAlign="Left"
                                                                ItemStyle-Width="130px">
                                                                <HeaderTemplate>
                                                                    <div style="width: 130px; font-size: 0px; height: 0px;">
                                                                    </div>
                                                                    <asp:LinkButton ID="lbCode" runat="server" CommandName="Sort" CommandArgument="RegionRate">
                                                                        <%= Resources.Resource.Admin_Taxes_Rate %>
                                                                        <asp:Image ID="arrowRate" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" /></asp:LinkButton>
                                                                </HeaderTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:TextBox ID="txtRegionRate" runat="server" Text='<%# ((decimal)Eval("RegionRate")).ToString("F2") %>'
                                                                        Width="99%"></asp:TextBox>
                                                                </EditItemTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lRegionRate" runat="server" Text='<%# ((decimal)Eval("RegionRate")).ToString("F2") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtRegionRateNew" CssClass="add" runat="server" Text='' Width="99%"></asp:TextBox>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="Center"
                                                                ItemStyle-Width="90px">
                                                                <EditItemTemplate>
                                                                    <input id="btnUpdate" name="btnUpdate" class="updatebtn showtooltip" type="image"
                                                                        src="images/updatebtn.png" onclick="<%#ClientScript.GetPostBackEventReference(grid, "Update$" + Container.DataItemIndex)%>;return false;"
                                                                        style="display: none" title='<%= Resources.Resource.Admin_MasterPageAdminCatalog_Update%>' />
                                                                    <asp:ImageButton ID="buttonDelete" runat="server" ImageUrl="images/deletebtn.png"
                                                                        CssClass="deletebtn showtooltip" CommandName="Delete" CommandArgument='<%# Eval("RegionID")%>'
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
                                                                    <asp:ImageButton ID="ibAddCurrency" runat="server" ImageUrl="images/addbtn.gif" CommandName="AddNewRegion"
                                                                        ToolTip="<%$ Resources:Resource, Admin_Tax_AddRegion  %>" />
                                                                    <asp:ImageButton ID="ibCancelAdd" runat="server" ImageUrl="images/cancelbtn.png"
                                                                        CommandName="CancelAdd" ToolTip="<%$ Resources:Resource, Admin_Currencies_CancelAdd  %>" />
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <FooterStyle BackColor="#ccffcc" />
                                                        <HeaderStyle CssClass="header" />
                                                        <RowStyle CssClass="row1 propertiesRow_25 readonlyrow" />
                                                        <AlternatingRowStyle CssClass="row2 propertiesRow_25_alt readonlyrow" />
                                                        <EmptyDataTemplate>
                                                            <center style="margin-top: 20px; margin-bottom: 20px;">
                                                                <%=Resources.Resource.Admin_Tax_NoRecords%>
                                                            </center>
                                                        </EmptyDataTemplate>
                                                    </adv:AdvGridView>
                                                    <asp:Label runat="server" ID="MessageRegions"></asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="div_3" class="vTabPanel" style="height: 100%;">
                        <table style="width: 100%; padding-left: 5px; padding-right: 5px">
                            <tr>
                                <td class="formheader">
                                    <asp:Label runat="server" Font-Bold="true" Font-Size="14px" Text="<%$ Resources: Resource, Admin_Tax_ProductTree %>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <div style="width: 100%">
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <adv:CatalogDataTreeViewForTaxes ID="CatalogDataTreeViewForTaxes" runat="server">
                                                    </adv:CatalogDataTreeViewForTaxes>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">

        function showElement(span_id) {
            var itemCount = 3;

            // Span
            var span = document.getElementById('li_' + span_id);
            span.className = "selected";
            for (i = 1; i <= itemCount; i++) {
                if (i != span_id) {
                    var span = document.getElementById('li_' + i);
                    span.className = "free";
                }
            }

            // Div
            var div = document.getElementById('div_' + span_id);
            div.className = "vTabPanel";
            div.style.display = "inline-block";
            for (i = 1; i <= itemCount; i++) {
                if (i != span_id) {
                    var div = document.getElementById('div_' + i);
                    div.className = "vTabPanel";
                    div.style.display = "none";
                }
            }

            // liState
            var liStateCtrl = document.getElementById('<%=__liState.ClientID%>');
            liStateCtrl.value = span_id;

        }


        function hoverElement(span_id) {
            var span = document.getElementById('li_' + span_id);
            if (span.className == "selected") {
                span.className = "selected_hovered";
            } else {
                span.className = "hovered";
            }
        }

        function outElement(span_id) {
            var span = document.getElementById('li_' + span_id);
            if (span.className == "selected_hovered" || span.className == "selected") {
                span.className = "selected";
            } else {
                span.className = "free";
            }
        }

        function liOnLoad() {

            var itemCount = 3;

            // Load span
            var liStateCtrl = document.getElementById('<%=__liState.ClientID%>');
            var span_id = liStateCtrl.value
            var span = document.getElementById('li_' + span_id);
            span.className = "selected";
            for (i = 1; i <= itemCount; i++) {
                if (i != span_id) {
                    var span = document.getElementById('li_' + i);
                    span.className = "free";
                }
            }

            // Load div
            var div = document.getElementById('div_' + span_id);
            div.className = "vTabPanel";
            div.style.display = "inline-block";
            for (i = 1; i <= itemCount; i++) {
                if (i != span_id) {
                    var div = document.getElementById('div_' + i);
                    div.className = "vTabPanel";
                    div.style.display = "none";
                }
            }

        }

        var dirty = false;
        var skipChecking = false;

        function showFloppy(num) {
            $("#iFloppy" + num).show("slow");
            dirty = true;
        }

        function checkForDirty(e) {
            if (!skipChecking) {
                var evt = e || window.event;
                if (dirty) {
                    evt.returnValue = '<%=Resources.Resource.Admin_Product_LosingChanges%>';
                }
            } else {
                skipChecking = false;
            }
        }

        $(document).ready(function () {
            $("#div_1 input, #div_1 select, #div_1 textarea").change(function () { showFloppy(1); });
            $("#div_2 input, #div_2 select, #div_2 textarea").change(function () { showFloppy(2); });
            $("#div_3 input, #div_3 select, #div_3 textarea").change(function () { showFloppy(3); });
            window.onbeforeunload = checkForDirty;

            $("#<%= BtnSave.ClientID %>").click(function () { skipChecking = true; });
        });
        
    </script>
    <script type="text/javascript">
        function setupTooltips() {
            $(".showtooltip").tooltip({
                showURL: false
            });
        }
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () { setupTooltips(); });
    </script>
</asp:Content>
