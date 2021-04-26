<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="Catalog.aspx.cs" Inherits="Admin_Catalog" EnableEventValidation="false" %>

<%@ Register Src="~/UserControls/Catalog/SiteNavigation.ascx" TagName="SiteNavigation"
    TagPrefix="uc1" %>
<%@ Register TagPrefix="adv" TagName="MainPageProduct" Src="~/Admin/UserControls/MainPageProduct.ascx" %>
<%@ MasterType VirtualPath="~/Admin/MasterPageAdmin.master" %>
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
            if ($.cookie("isVisiblePanel") != "false") {
                showPanel();
            }
            document.onkeydown = keyboard_navigation;
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_beginRequest(Darken);
            prm.add_endRequest(Clear);
            initgrid();
            $("ineditcategory").tooltip();
        });
        function showPanel() {
            document.getElementById("leftPanel").style.display = "block";
            document.getElementById("divHide").style.display = "block";
            document.getElementById("divShow").style.display = "none";
        }

        function togglePanel() {
            if ($.cookie("isVisiblePanel") == "true" || $.cookie("isVisiblePanel") == "") {
                $("div:.leftPanel").hide("fast");
                $("div:#divHide").hide("fast");
                $("div:#divShow").show("fast");
                $.cookie("isVisiblePanel", "false", { expires: 7 });
            } else {
                $("div:.leftPanel").show("fast");
                $("div:#divHide").show("fast");
                $("div:#divShow").hide("fast");
                $.cookie("isVisiblePanel", "true", { expires: 7 });
            }
        }

        var base$TreeView_ProcessNodeData;
        var base$TreeView_PopulateNodeDoCallBack;

        function updatetree() {
            var win = document.parentWindow || document.defaultView;
            if (win.TreeView_ProcessNodeData != ProcessNodeData) {
                base$TreeView_ProcessNodeData = win.TreeView_ProcessNodeData;
                win.TreeView_ProcessNodeData = ProcessNodeData;
            }
            if (win.TreeView_PopulateNodeDoCallBack != PopulateNodeDoCallBack) {
                base$TreeView_PopulateNodeDoCallBack = win.TreeView_PopulateNodeDoCallBack;
                win.TreeView_PopulateNodeDoCallBack = PopulateNodeDoCallBack;
            }
        }

        function ProcessNodeData(result, context) {
            hide_wait_for_node(context.node);
            return base$TreeView_ProcessNodeData(result, context);
        }

        function PopulateNodeDoCallBack(context, param) {
            show_wait_for_node(context.node);
            return base$TreeView_PopulateNodeDoCallBack(context, param);
        }

        function hide_wait_for_node(node) {
            if (node.wait_img) {
                node.removeChild(node.wait_img);
            }
        }

        function show_wait_for_node(node) {
            var waitImg = document.createElement("IMG");
            waitImg.src = "images/loading.gif";
            waitImg.border = 0;
            node.wait_img = waitImg;
            node.appendChild(waitImg);
        }

        var _TreePostBack = false;

        function endRequest() {
            if (_TreePostBack) {
                updatetree();
                document.getElementById('mpeBehavior_backgroundElement').onclick = function () { $find('mpeBehavior').hide(); };
            }
            else {
                $(".photoinput").val("");
            }
        }

        function ATreeView_Select(sender, arg) {
            $("a.selectedtreenode").removeClass("selectedtreenode");
            $(sender).addClass("selectedtreenode");
            document.getElementById("TreeView_SelectedValue").value = arg;
            document.getElementById("TreeView_SelectedNodeText").value = sender.innerHTML;
            return false;
        }
    </script>
    <style type="text/css">
        h2
        {
            margin: 1px 0;
            font-size: 11pt;
        }
    </style>
</asp:Content>
<asp:Content ID="RootContent" ContentPlaceHolderID="cphMain" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <ajaxToolkit:ModalPopupExtender ID="mpeTree2" runat="server" PopupControlID="pTree2"
                TargetControlID="hhl2" BackgroundCssClass="blackopacitybackground" CancelControlID="btnCancelParent2"
                BehaviorID="ModalBehaviour2">
            </ajaxToolkit:ModalPopupExtender>
            <asp:HyperLink ID="hhl2" runat="server" Style="display: none;" />
            <asp:Panel runat="server" ID="pTree2" CssClass="modal-admin">
                <div style="text-align: center;">
                    <table style="margin-top: 13px;">
                        <tbody>
                            <tr>
                                <td>
                                    <span style="font-size: 11pt;">
                                        <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources:Resource, Admin_CatalogLinks_ParentCategory %>"></asp:Localize></span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="tree2" />
                                            <asp:AsyncPostBackTrigger ControlID="lbChangeCategory" EventName="Click" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <div style="height: 360px; width: 450px; overflow: scroll; background-color: White;
                                                text-align: left">
                                                <asp:TreeView ID="tree2" ForeColor="Black" SelectedNodeStyle-BackColor="Blue" PopulateNodesFromClient="true"
                                                    OnSelectedNodeChanged="OnSelectedNodeChanged2" runat="server" ShowLines="True"
                                                    ExpandImageUrl="images/loading.gif" BackColor="White" OnTreeNodePopulate="PopulateNode2">
                                                    <SelectedNodeStyle BackColor="Yellow" />
                                                </asp:TreeView>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <asp:LinkButton ID="lbChangeCategory" Style="display: none" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_DeleteSelectedFromCategory %>"
                                        OnClick="lbChangeCategory_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 36px; text-align: right; vertical-align: bottom;">
                                    <asp:Button ID="btnChangeProductCategory" runat="server" Text="<%$ Resources: Resource, Admin_Catalog_SaveChangeCategory%>"
                                        OnClick="btnChangeProductCategory_Click" />
                                    <span></span>
                                    <asp:Button ID="btnCancelParent2" runat="server" Text="<%$ Resources: Resource, Admin_Cancel %>"
                                        Width="67" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <table border="0" width="100%" id="table2" cellspacing="0" cellpadding="0">
        <tr>
            <td style="vertical-align: top;">
                <div id="leftPanel" class="leftPanel dvLeftPanel">
                    <table border="0" cellspacing="0" cellpadding="0" class="catalog_part catelog_list">
                        <tr style="height: 28px;">
                            <td style="width: 30px;">
                                <img src="images/folder.gif" alt="" />
                            </td>
                            <td style="width: 137px;" class="catalog_label">
                                <asp:Localize ID="Localize3" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_Products %>"></asp:Localize>
                            </td>
                            <td style="width: 75px;text-align:right">
                                <asp:ImageButton ID="ibRecalculateProducts" CssClass="showtooltip" runat="server"
                                    ImageUrl="images/groundarrow.gif" ToolTip="<%$ Resources:Resource, Admin_MasterPageAdminCatalog_Recalculate%>"
                                    onmouseover="this.src='images/broundarrow.gif';" onmouseout="this.src='images/groundarrow.gif';"
                                    OnClick="ibRecalculate_Click" />
                            </td>
                        </tr>
                    </table>
                    <div class="catalog_part catelog_listContent">
                        <ul class="catelog_ullist">
                            <li><a class="Link" href="Catalog.aspx?CategoryID=AllProducts">
                                <%= RenderTotalProductLink()%></a></li>
                            <li><a class="Link" href="Catalog.aspx?CategoryID=InCategories">
                                <%= RenderTotalProductInCategoriesLink()%></a></li>
                            <li><a class="Link" href="Catalog.aspx?CategoryID=WithoutCategory">
                                <%= RenderTotalProductWithoutCategoriesLink()%></a></li>
                        </ul>
                    </div>
                    <table border="0" cellspacing="0" cellpadding="0" class="catalog_part catelog_list">
                        <tr style="height: 28px;">
                            <td style="width: 30px;">
                                <img src="images/folder.gif" alt="" />
                            </td>
                            <td style="width: 137px;" class="catalog_label">
                                <asp:Localize ID="Localize_Admin_MasterPageAdminCatalog_Catalog" runat="server" Text="<%$ Resources:Resource, Admin_MasterPageAdminCatalog_Catalog %>"></asp:Localize>
                            </td>
                            <td style="width: 75px;text-align:right">
                                <input id="ineditcategory" class="showtooltip" onclick="open_window('m_Category.aspx?CategoryID=0&mode=create', 750, 640);return false;"
                                    onmouseover="this.src='images/bplus.gif'" title="<%= Resources.Resource.Admin_MasterPageAdminCatalog_AddNewCategory %>"
                                    onmouseout="this.src='images/gplus.gif';" src="images/gplus.gif" type="image" />
                                <asp:ImageButton ID="ibRecalculate" CssClass="showtooltip" runat="server" ImageUrl="images/groundarrow.gif"
                                    ToolTip="<%$ Resources:Resource, Admin_MasterPageAdminCatalog_Recalculate%>"
                                    onmouseout="this.src='images/groundarrow.gif';" OnClick="ibRecalculate_Click" />
                                <input type="image" src="images/gudarrow.gif" class="showtooltip" onclick="open_window('m_CategorySortOrder.aspx', 750, 640);return false;"
                                    title="<%= Resources.Resource.Admin_MasterPageAdminCatalog_SortOrder %>" onmouseover="this.src='images/budarrow.gif';"
                                    onmouseout="this.src='images/gudarrow.gif';" />
                            </td>
                        </tr>
                    </table>
                    <div class="catalog_part catelog_listContent">
                        <adv:CommandTreeView ID="tree" runat="server" NodeWrap="True" ShowLines="true" CssClass="AdminTree_MainClass"
                            OnTreeNodeCommand="tree_TreeNodeCommand" OnTreeNodePopulate="tree_TreeNodePopulate">
                            <ParentNodeStyle Font-Bold="False" />
                            <SelectedNodeStyle ForeColor="#027dc2" Font-Bold="true" HorizontalPadding="0px" VerticalPadding="0px"
                                CssClass="selectedNode" />
                            <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" HorizontalPadding="5px"
                                NodeSpacing="0px" VerticalPadding="0px" />
                        </adv:CommandTreeView>
                    </div>
                    <adv:MainPageProduct ID="MainPageProductBestseller" runat="server" Flag="Bestseller " />
                    <adv:MainPageProduct ID="MainPageProductNew" runat="server" Flag="New " />
                    <adv:MainPageProduct ID="MainPageProductOnSale" runat="server" Flag="Discount " />
                </div>
            </td>
            <%=RenderSplitter()%>
            <td style="width: 100%; vertical-align: top; padding: 0px 10px;">
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
                                        <asp:Localize ID="Localize_Admin_Catalog_PleaseWait" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_PleaseWait %>"></asp:Localize>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
                <%--<asp:UpdatePanel ID="UPbtns" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAddProduct" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnAddCategory" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="hlDeleteCategory" EventName="Click" />
                    </Triggers>
                    <ContentTemplate>--%>
                <div>
                    <div class="catalog_Link" style="clear: both">
                        <asp:Label ID="lblCategoryName" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_MasterPageAdminCatalog_lblMain %>"></asp:Label>
                        <asp:HyperLink ID="hlEditCategory" runat="server" Text="<%$ Resources:Resource, Admin_MasterPageAdminCatalog_FPanel_EditCategory %>" />
                        <asp:Label ID="lblSeparator" runat="server" Text=" | "></asp:Label>
                        <asp:LinkButton ID="hlDeleteCategory" runat="server" Text="<%$ Resources:Resource, Admin_MasterPageAdminCatalog_FPanel_DeleteCategory %>"
                            OnClick="hlDeleteCategory_Click"></asp:LinkButton>
                        <ajaxToolkit:ConfirmButtonExtender ID="ConfirmButtonExtenderCategory" runat="server"
                            TargetControlID="hlDeleteCategory">
                        </ajaxToolkit:ConfirmButtonExtender>
                    </div>
                    <div>
                        <div style="float: left;">
                            <span class="AdminSubHead" style="display: inline-block;">
                                <%=Resources.Resource.Admin_MasterPageAdminCatalog_lblSubMain%></span>
                            <div id="siteNavigationBlock" style="margin-top: 10px; margin-bottom: 10px" runat="server">
                                <span style="font-weight: bold;">
                                    <asp:Localize ID="Localize_Admin_Catalog_CategoryLocation" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_CategoryLocation %>"></asp:Localize>
                                </span>
                                <br />
                                <uc1:SiteNavigation ID="sn" runat="server" />
                                <asp:Label ID="lMessage" Style="float: left;" runat="server" ForeColor="Red" Visible="false"
                                    EnableViewState="false" />
                            </div>
                        </div>
                        <div style="float: right">
                            <div style="height: 41px" id="advButtonFiller" runat="server">
                            </div>
                            <div>
                                <adv:AdvButton ID="btnAddCategory" runat="server" Width="200" Text="<%$ Resources:Resource, Admin_Catalog_AddSubcategory %>"
                                    CssMain="adv_OrangeButton_Main" CssInput="adv_OrangeButton_Input" CssInputMozz="adv_OrangeButton_Input-mozz"
                                    CssLeftDiv="adv_OrangeButton_LeftDiv" CssRightDiv="adv_OrangeButton_RightDiv"
                                    CssCenterDiv="adv_OrangeButton_CenterDiv2" ValidationGroup="0" />
                                <adv:AdvButton ID="btnAddProduct" runat="server" Width="170" Text="<%$ Resources:Resource, Admin_Catalog_AddProduct %>"
                                    CssMain="adv_OrangeButton_Main" CssInput="adv_OrangeButton_Input" CssInputMozz="adv_OrangeButton_Input-mozz"
                                    CssLeftDiv="adv_OrangeButton_LeftDiv" CssRightDiv="adv_OrangeButton_RightDiv"
                                    CssCenterDiv="adv_OrangeButton_CenterDiv" ValidationGroup="0" OnClick="btnAddProduct_Click" />
                            </div>
                        </div>
                    </div>
                </div>
                <%--                    </ContentTemplate>
                </asp:UpdatePanel>--%>
                <div style="width: 100%; clear: both;">
                    <table style="width: 100%;" class="massaction">
                        <tr>
                            <td>
                                <span class="admin_catalog_commandBlock"><span style="display: inline-block; margin-right: 3px;">
                                    <asp:Localize runat="server" Text="<%$ Resources:Resource, Admin_Catalog_Command %>"></asp:Localize>
                                </span><span style="display: inline-block">
                                    <select id="commandSelect">
                                        <option value="selectAll">
                                            <asp:Localize runat="server" Text="<%$ Resources:Resource, Admin_Catalog_SelectAll %>"></asp:Localize>
                                        </option>
                                        <option value="unselectAll">
                                            <asp:Localize runat="server" Text="<%$ Resources:Resource, Admin_Catalog_UnselectAll %>"></asp:Localize>
                                        </option>
                                        <option value="selectVisible">
                                            <asp:Localize runat="server" Text="<%$ Resources:Resource, Admin_Catalog_SelectVisible %>"></asp:Localize>
                                        </option>
                                        <option value="unselectVisible">
                                            <asp:Localize runat="server" Text="<%$ Resources:Resource, Admin_Catalog_UnselectVisible %>"></asp:Localize>
                                        </option>
                                        <option value="deleteSelected">
                                            <asp:Localize ID="Localize5" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_DeleteSelected %>"></asp:Localize>
                                        </option>
                                        <%if (ShowMethod == EShowMethod.Normal)
                                          {%>
                                        <option value="deleteSelectedFromCategory">
                                            <%= Resources.Resource.Admin_Catalog_DeleteSelectedFromCategory%>
                                        </option>
                                        <%}%>
                                        <option value="changeCategory">
                                            <%= Resources.Resource.Admin_Catalog_ChangeCategory%>
                                        </option>
                                    </select>
                                    <input id="commandButton" type="button" value="GO" style="font-size: 10px; width: 38px;
                                        height: 20px;" />
                                    <asp:LinkButton ID="lbDeleteSelected1" Style="display: none" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_DeleteSelected %>"
                                        OnClick="lbDeleteSelected1_Click" />
                                    <asp:LinkButton ID="lbDeleteSelectedFromCategory" Style="display: none" runat="server"
                                        Text="<%$ Resources:Resource, Admin_Catalog_DeleteSelectedFromCategory %>" OnClick="lbDeleteSelectedFromCategory_Click" />
                                </span><span class="selecteditems" style="vertical-align: baseline"><span style="padding: 0px 3px 0px 3px;">
                                    |</span><span id="selectedIdsCount" class="bold">0</span>&nbsp;<span><%=Resources.Resource.Admin_Catalog_ItemsSelected%></span></span></span>
                            </td>
                            <td align="right" class="selecteditems">
                                <asp:UpdatePanel ID="upCounts" runat="server">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnFilter" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="Click" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <asp:Panel ID="pnlNormalCount" runat="server">
                                            <div id="categoryCountBlock" style="display: inline-block" visible="false" runat="server">
                                                <asp:Localize ID="Localize_Admin_Catalog_Total" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_SubCategoriesFound %>"></asp:Localize>
                                                <span class="bold">
                                                    <asp:Label ID="lblsubCats" CssClass="foundrecords" runat="server" Text="" />
                                                </span>
                                            </div>
                                            <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_ProductsFound %>"></asp:Localize>
                                            <span class="bold">
                                                <asp:Label ID="lblProducts" CssClass="foundrecords" runat="server" Text="" />
                                            </span>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlFilterCount" runat="server" Visible="false">
                                            <asp:Localize ID="Localize4" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_Total %>"></asp:Localize>
                                            <span class="bold">
                                                <asp:Label ID="lblFound" CssClass="foundrecords" runat="server" Text="" /></span>&nbsp;<asp:Localize
                                                    ID="Localize8" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_RecordsFound %>"></asp:Localize>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td style="width: 8px;">
                            </td>
                        </tr>
                    </table>
                    <div style="border: 1px #c9c9c7 solid; width: 100%">
                        <asp:Panel runat="server" ID="filterPanel" DefaultButton="btnFilter">
                            <table class="filter" cellpadding="0" cellspacing="0">
                                <tr style="height: 5px;">
                                    <td colspan="9">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 35px; text-align: center; font-size: 13px" rowspan="2">
                                        <asp:DropDownList ID="ddSelect" TabIndex="10" CssClass="dropdownselect" runat="server"
                                            Width="35px">
                                            <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>" />
                                            <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" />
                                            <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" />
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 35px; text-align: center; font-size: 13px" rowspan="2">
                                        <asp:DropDownList ID="ddPhoto" TabIndex="10" CssClass="dropdownselect" runat="server"
                                            Width="35px">
                                            <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>" />
                                            <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_WithPhoto %>" />
                                            <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_WithoutPhoto %>" />
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 100px;" rowspan="2">
                                        <div style="width: 100px; height: 0px; font-size: 0px;">
                                        </div>
                                        <asp:TextBox CssClass="filtertxtbox" ID="txtArtNo" Width="99%" runat="server" TabIndex="11" />
                                    </td>
                                    <td rowspan="2">
                                        <div style="width: 134px; height: 0px; font-size: 0px;">
                                        </div>
                                        <asp:TextBox CssClass="filtertxtbox" ID="txtName" Width="99%" runat="server" TabIndex="12" />
                                    </td>
                                    <td style="width: 80px" rowspan="2">
                                    </td>
                                    <td style="width: 95px; padding-right: 0px; text-align: right; white-space: nowrap">
                                        <span><span class="textfromto">
                                            <asp:Localize ID="Localize_Admin_Catalog_From0" Text="<%$ Resources:Resource, Admin_Catalog_From %>"
                                                runat="server"></asp:Localize>:</span><asp:TextBox CssClass="filtertxtbox" ID="txtPriceFrom"
                                                    runat="server" TabIndex="13" />
                                        </span>
                                    </td>
                                    <td style="width: 100px; padding-right: 0px; text-align: right; white-space: nowrap">
                                        <span class="textfromto">
                                            <asp:Localize ID="Localize_Admin_Catalog_From1" Text="<%$ Resources:Resource, Admin_Catalog_From %>"
                                                runat="server"></asp:Localize>:</span><asp:TextBox CssClass="filtertxtbox" ID="txtQtyFrom"
                                                    runat="server" TabIndex="15" />
                                    </td>
                                    <td style="width: 100px; text-align: center;" rowspan="2">
                                        <asp:DropDownList ID="ddlEnabled" TabIndex="17" CssClass="dropdownselect" runat="server">
                                            <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>" />
                                            <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" />
                                            <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" />
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 120px; text-align: center; white-space: nowrap; display: none"
                                        rowspan="2">
                                        <asp:DropDownList ID="ddlIsFirstPageProducts" TabIndex="18" CssClass="dropdownselect"
                                            runat="server">
                                            <asp:ListItem Selected="True" Text="<%$ Resources:Resource, Admin_Catalog_Any %>" />
                                            <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_Yes %>" />
                                            <asp:ListItem Text="<%$ Resources:Resource, Admin_Catalog_No %>" />
                                        </asp:DropDownList>
                                    </td>
                                    <%if (ShowMethod == EShowMethod.Normal)
                                      {%>
                                    <td style="width: 95px; padding-right: 10px; text-align: right; white-space: nowrap;">
                                        <span class="textfromto">
                                            <%=Resources.Resource.Admin_Catalog_From%>:</span><asp:TextBox CssClass="filtertxtbox"
                                                ID="txtSortOrderFrom" runat="server" TabIndex="19" />
                                    </td>
                                    <%
                                      }%>
                                    <td style="width: 71px; text-align: center;" rowspan="2">
                                        <asp:Button ID="btnFilter" runat="server" CssClass="btn" OnClientClick="javascript:FilterClick();"
                                            TabIndex="23" Text="<%$ Resources:Resource, Admin_Catalog_Filter %>" OnClick="btnFilter_Click" />
                                        <asp:Button ID="btnReset" runat="server" CssClass="btn" OnClientClick="javascript:ResetFilter();"
                                            TabIndex="24" Text="<%$ Resources:Resource, Admin_Catalog_Reset %>" OnClick="btnReset_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 95px; padding-right: 0px; text-align: right; white-space: nowrap">
                                        <span class="textfromto">
                                            <%=Resources.Resource.Admin_Catalog_To%>:</span><asp:TextBox CssClass="filtertxtbox"
                                                ID="txtPriceTo" runat="server" Font-Size="10px" Width="50" TabIndex="14" />
                                    </td>
                                    <td style="width: 100px; padding-right: 0px; text-align: right; white-space: nowrap">
                                        <span class="textfromto">
                                            <%=Resources.Resource.Admin_Catalog_To%>:</span><asp:TextBox CssClass="filtertxtbox"
                                                ID="txtQtyTo" runat="server" Font-Size="10px" Width="50" TabIndex="16" />
                                    </td>
                                    <% if (ShowMethod == EShowMethod.Normal)
                                       {%>
                                    <td style="width: 95px; padding-right: 10px; text-align: right; white-space: nowrap">
                                        <span class="textfromto">
                                            <%=Resources.Resource.Admin_Catalog_To%>:</span><asp:TextBox CssClass="filtertxtbox"
                                                ID="txtSortOrderTo" runat="server" Font-Size="10px" Width="50" TabIndex="20" />
                                    </td>
                                    <%
                                       }%>
                                </tr>
                                <tr>
                                    <td style="height: 5px;" colspan="9">
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="pageNumberer" EventName="SelectedPageChanged" />
                                <asp:AsyncPostBackTrigger ControlID="btnFilter" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnGo" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="ddRowsPerPage" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="grid" EventName="DataBinding" />
                            </Triggers>
                            <ContentTemplate>
                                <adv:AdvGridView ID="grid" runat="server" AllowSorting="true" AutoGenerateColumns="False"
                                    CellPadding="0" CellSpacing="0" Confirmation="<%$ Resources:Resource, Admin_Catalog_Confirmation %>"
                                    CssClass="tableview" DataFieldForEditURLParam="ID" DataFieldForImageDescription="BriefDescription"
                                    DataFieldForImagePath="PhotoName" EditURL="Product.aspx?productid={0}" GridLines="None"
                                    TooltipImgCellIndex="2" TooltipTextCellIndex="5" OnDataBinding="grid_DataBinding"
                                    ReadOnlyGrid="True" OnSorting="grid_Sorting" OnRowCommand="grid_RowCommand" OnRowDataBound="grid_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField AccessibleHeaderText="Id" Visible="false" HeaderStyle-Width="50px">
                                            <EditItemTemplate>
                                                <asp:Label ID="Label0" runat="server" Text='<%# (int)Eval("typeItem") == 0 ? "Category_" + Eval("ID") : "Product_" + Eval("ID") %>'></asp:Label>--%>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label01" runat="server" Text='<%#(int)Eval("typeItem") == 0 ? "Category_" + Eval("ID") : "Product_" + Eval("ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-CssClass="checkboxcolumnheader" ItemStyle-CssClass="checkboxcolumn">
                                            <HeaderTemplate>
                                                <div style="height: 0px; width: 40px; font-size: 0px;">
                                                </div>
                                                <asp:CheckBox ID="checkBoxCheckAll" CssClass="checkboxcheckall" runat="server" onclick="javascript:SelectVisible(this.checked);" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%#(bool)Eval("IsSelected") ? "<input type='checkbox' class='sel' checked='checked' />" : "<input type='checkbox' class='sel' />"%>
                                                <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# (int)Eval("typeItem") == 0 ? "Category_" + Eval("ID"): "Product_" + Eval("ID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="Photo" HeaderStyle-Width="30px">
                                            <HeaderTemplate>
                                                <div style="height: 0px; width: 30px; font-size: 0px;">
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# GetImageItem((int)Eval("typeItem"), (int)Eval("ID"))%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="ArtNo" ItemStyle-CssClass="colid" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                <div style="height: 0px; width: 100px; font-size: 0px;">
                                                </div>
                                                <asp:LinkButton ID="lbOrderId" runat="server" CommandName="Sort" CommandArgument="ArtNo">
                                                    <%=Resources.Resource.Admin_Catalog_StockNumber%>
                                                    <asp:Image ID="arrowArtNo" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                                </asp:LinkButton>
                                            </HeaderTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtStockNumber" runat="server" Text='<%# (int)Eval("typeItem") == 0 ? Eval("Id") : Eval("ArtNo") %>'
                                                    Width="99%"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label3" runat="server" Text="0000"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="Name" HeaderStyle-HorizontalAlign="Left"
                                            HeaderStyle-Width="100%">
                                            <HeaderTemplate>
                                                <div style="height: 0px; width: 134px; font-size: 0px;">
                                                </div>
                                                <asp:LinkButton ID="lbOrderCategory" runat="server" CommandName="Sort" CommandArgument="Name">
                                                    <%=Resources.Resource.Admin_Catalog_Name%>
                                                    <asp:Image ID="arrowName" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                                </asp:LinkButton>
                                            </HeaderTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtName" runat="server" Text='<%# Eval("Name") %>' Width="99%"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="Categories" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <div style="height: 0px; width: 80px; font-size: 0px;">
                                                </div>
                                                <asp:LinkButton ID="lbCategories" runat="server">
                                        <%=Resources.Resource.Admin_Catalog_Categories%>
                                                </asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%#(((int)Eval("typeItem") == 1) && (GetCountOfCategoriesByProductID((int)Eval("ID")) > 0)) ? "<img src='images/adv_category_ico.gif' alt=''/>" : ""%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="Price" ItemStyle-Width="95" ItemStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <div style="height: 0px; width: 95px; font-size: 0px;">
                                                </div>
                                                <asp:LinkButton ID="lbPrice" runat="server" CommandName="Sort" CommandArgument="Price">
                                                    <%=Resources.Resource.Admin_Catalog_Price%>
                                                    <asp:Image ID="arrowPrice" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                                </asp:LinkButton>
                                            </HeaderTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtPrice" runat="server" Text='<%# String.Format("{0:##,##0.00}", Eval("Price")) %>'
                                                    Width="90%" Style="text-align: center;"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%#String.Format("{0:##,##0.00}", Eval("Price")) %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="Qty" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <div style="height: 0px; width: 100px; font-size: 0px;">
                                                </div>
                                                <asp:LinkButton ID="lbOrderInternalName" runat="server" CommandName="Sort" CommandArgument="Amount">
                                                    <%=Resources.Resource.Admin_Catalog_Qty%>
                                                    <asp:Image ID="arrowQty" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                                </asp:LinkButton>
                                            </HeaderTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtAmount" runat="server" Text='<%# Bind("Amount") %>' Width="99%"
                                                    Style="text-align: center;"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmount" runat="server" Text='<%# Bind("Amount") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="Enabled" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <div style="height: 0px; width: 100px; font-size: 0px;">
                                                </div>
                                                <asp:LinkButton ID="lbEnabled" runat="server" CommandName="Sort" CommandArgument="Enabled">
                                                    <%=Resources.Resource.Admin_CatalogLinks_Active%>
                                                    <asp:Image ID="arrowEnabled" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                                </asp:LinkButton>
                                            </HeaderTemplate>
                                            <EditItemTemplate>
                                                <asp:CheckBox ID="CheckBox2" runat="server" Checked='<%# Bind("Enabled") %>' />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("Enabled") %>' Enabled="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="SortOrder" ItemStyle-Width="105" ItemStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <div style="width: 105px; height: 0px; font-size: 0px;">
                                                </div>
                                                <asp:LinkButton ID="lbSortOrder" runat="server" CommandName="Sort" CommandArgument="SortOrder">
                                                    <%=Resources.Resource.Admin_Catalog_SortOrder%>
                                                    <asp:Image ID="arrowSortOrder" CssClass="arrow" runat="server" ImageUrl="images/arrowdownh.gif" />
                                                </asp:LinkButton>
                                            </HeaderTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtSortOrder" runat="server" Text='<%# Bind("SortOrder") %>' Width="90%"
                                                    Style="text-align: center;"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lSortOrder" runat="server" Text='<%# Bind("SortOrder") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="typeItem" Visible="false" HeaderStyle-Width="50px">
                                            <EditItemTemplate>
                                                <asp:Label ID="Label011" runat="server" Text='<%# Bind("typeItem") %>'></asp:Label>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label0111" runat="server" Text='<%# Bind("typeItem") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="69" AccessibleHeaderText="Buttons" ItemStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <div style="height: 0px; width: 71px; font-size: 0px;">
                                                </div>
                                            </HeaderTemplate>
                                            <EditItemTemplate>
                                                <a id="cmdlink" runat="server" href='' class="editbtn showtooltip" title='<%$ Resources:Resource,Admin_MasterPageAdminCatalog_Edit%>'>
                                                    <img src="images/editbtn.gif" style="border: none;" alt="" /></a>
                                                <%if (ShowMethod == EShowMethod.Normal)
                                                  {%>
                                                <asp:ImageButton ID="buttonDeleteLink" runat="server" ToolTip="<%$ Resources:Resource, Admin_MasterPageAdminCatalog_DeleteFromCategory%>"
                                                    ImageUrl="images/excludebtn.png" CssClass="deletebtn showtooltip" CommandName="Deletelink"
                                                    CommandArgument='<%# Eval("ID") %>' />
                                                <ajaxToolkit:ConfirmButtonExtender ID="ConfirmButtonExtender3" runat="server" TargetControlID="buttonDeleteLink"
                                                    ConfirmText="<%$ Resources:Resource, Admin_Product_ConfirmDeletingProductFormCategory %>">
                                                </ajaxToolkit:ConfirmButtonExtender>
                                                <%}%>
                                                <asp:LinkButton ID="buttonDelete" runat="server" CssClass="deletebtn showtooltip"
                                                    ToolTip='<%$ Resources:Resource, Admin_MasterPageAdminCatalog_Delete %>' CommandArgument='<%# Eval("ID") %>' >
                                                    <asp:Image ID="Image1" ImageUrl="images/deletebtn.png" runat="server" />
                                                </asp:LinkButton>
                                                <ajaxToolkit:ConfirmButtonExtender ID="ConfirmButtonExtenderbuttonDelete" runat="server"
                                                    TargetControlID="buttonDelete">
                                                </ajaxToolkit:ConfirmButtonExtender>
                                                <input id="btnUpdate" name="btnUpdate" class="updatebtn showtooltip" type="image"
                                                    src="images/updatebtn.png" onclick="<%#ClientScript.GetPostBackEventReference(grid, "Update$" + Container.DataItemIndex)%>;return false;"
                                                    style="display: none" title='<%=
                                                          Resources.Resource.Admin_MasterPageAdminCatalog_Update%>' />
                                                <input id="btnCancel" name="btnCancel" class="cancelbtn showtooltip" type="image"
                                                    src="images/cancelbtn.png" onclick="row_canceledit($(this).parent().parent()[0]);return false;"
                                                    style="display: none" title='<%=
                                                          Resources.Resource.Admin_MasterPageAdminCatalog_Cancel%>' />
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
                                            <asp:Localize ID="Localize_Admin_Catalog_NoRecords" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_NoRecords %>"></asp:Localize>
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
                                        <td style="text-align: center;">
                                            <adv:PageNumberer CssClass="PageNumberer" ID="pageNumberer" runat="server" DisplayedPages="7"
                                                UseHref="false" OnSelectedPageChanged="pn_SelectedPageChanged" UseHistory="false" />
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
            </td>
        </tr>
    </table>
    <script type="text/javascript">
        var base$TreeView_PopulateNodeDoCallBack = this.TreeView_PopulateNodeDoCallBack;
        var base$TreeView_ProcessNodeData = this.TreeView_ProcessNodeData;
        this.TreeView_ProcessNodeData = function (result, context) {
            //alert( "after load " );
            hide_wait_for_node(context.node);
            var r = base$TreeView_ProcessNodeData(result, context);
            setupHoverPanel();
            return r;
        };
        this.TreeView_PopulateNodeDoCallBack = function (context, param) {
            //alert( "before load " );
            show_wait_for_node(context.node);
            return base$TreeView_PopulateNodeDoCallBack(context, param);
        };

        function setupTooltips() {
            $(".showtooltip").tooltip({
                showURL: false
            });
        }

        function setupHoverPanel() {
            $(".newToolTip").each(function () {
                if ($(this).data('qtip')) {
                    return true;
                }
                var catId = $(this).attr("catId");
                var catName = $(this).attr("catName");
                if (catId != '0') {
                    var cnt = "<div class='hoverPanel'><div style='margin-bottom:5px;width:190px'><a onmouseover='this.style.color = \"#FF7F27\"' onmouseout='this.style.color = \"#000000\"' onclick=\"open_window('m_Category.aspx?CategoryID=" + catId + "&mode=create',750,640); return false;\"> <div class=\"hoverPanel_img_container\"><img onmouseout=\"this.src = 'images/glplus.gif';\" onmouseover=\"this.src = 'images/blplus.gif';\" src=\"images/glplus.gif\" alt=\"\"/></div>";
                    cnt += "<span><%= Resources.Resource.Admin_MasterPageAdminCatalog_FPanel_AddCategory %></span></a></div>";
                    cnt += "<div style='margin-bottom:5px;'><a onmouseover='this.style.color = \"#FF7F27\"' onmouseout='this.style.color = \"#000000\"' onclick=\"open_window('m_Category.aspx?CategoryID=" + catId + "&mode=edit',750,640); return false;\"> <div class=\"hoverPanel_img_container\"><img onmouseout=\"this.src = 'images/gpencil.gif';\" onmouseover=\"this.src = 'images/bpencil.gif';\" src=\"images/gpencil.gif\" alt=\"\"/></div>";
                    cnt += "<span><%= Resources.Resource.Admin_MasterPageAdminCatalog_FPanel_EditCategory %></span></a></div>";
                    cnt += "<div><a onmouseover='this.style.color = \"#FF7F27\"' onmouseout='this.style.color = \"#000000\"' onclick=\"if(confirm('" + catName + "')){__doPostBack('<%= tree.UniqueID %>','c$DeleteCategory#" + catId + "')} return false;\"> <div class=\"hoverPanel_img_container\"><img onmouseout=\"this.src = 'images/gcross.gif';\" onmouseover=\"this.src = 'images/bcross.gif';\" src=\"images/gcross.gif\" alt=\"\"/></div>";
                    cnt += "<span><%= Resources.Resource.Admin_MasterPageAdminCatalog_FPanel_DeleteCategory %></span></a></div></div>";
                }
                else {
                    var cnt = "<div class='hoverPanel'><div style='margin-bottom:5px;width:190px'><a onmouseover='this.style.color = \"#FF7F27\"' onmouseout='this.style.color = \"#000000\"' onclick=\"open_window('m_Category.aspx?CategoryID=" + catId + "&mode=create',750,640); return false;\"> <div class=\"hoverPanel_img_container\"><img onmouseout=\"this.src = 'images/glplus.gif';\" onmouseover=\"this.src = 'images/blplus.gif';\" src=\"images/glplus.gif\" alt=\"\"/></div>";
                    cnt += "<span><%= Resources.Resource.Admin_MasterPageAdminCatalog_FPanel_AddCategory %></span></a></div>";
                    cnt += "<div style='margin-bottom:5px;'><a onmouseover='this.style.color = \"#FF7F27\"' onmouseout='this.style.color = \"#000000\"' onclick=\"open_window('m_Category.aspx?CategoryID=" + catId + "&mode=edit',750,640); return false;\"> <div class=\"hoverPanel_img_container\"><img onmouseout=\"this.src = 'images/gpencil.gif';\" onmouseover=\"this.src = 'images/bpencil.gif';\" src=\"images/gpencil.gif\" alt=\"\"/></div>";
                    cnt += "<span><%= Resources.Resource.Admin_MasterPageAdminCatalog_FPanel_EditCategory %></span></a></div></div>";
                }

                $(this).qtip({
                    content: cnt,
                    position: { corner: { target: 'bottomLeft', tooltip: "topLeft" }, adjust: { screen: true} },
                    //раскомментировать в случае падения производительности скриптов на странице
                    hide: { fixed: true, delay: 100 /*,effect: function () { $(this).stop(true, true).hide(); }*/ },
                    show: { solo: true, delay: 600 /*,effect: function () { $(this).stop(true, true).show(); }*/ }
                });

                $(this).mouseover(function () {
                    $(this).addClass("AdminTree_HoverNodeStyle");
                });

                $(this).mouseout(function () {
                    $(this).removeClass("AdminTree_HoverNodeStyle");
                });
            });
        }

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () { setupTooltips(); setupHoverPanel(); setupAdvantGrid(); });

        function hide_wait_for_node(node) {
            if (node.wait_img) {
                node.removeChild(node.wait_img);
            }
        }

        function show_wait_for_node(node) {
            var wait_img = document.createElement("IMG");
            wait_img.src = "images/loader.gif";
            wait_img.border = 0;
            node.wait_img = wait_img;
            node.appendChild(wait_img);
        }

        function setupAdvantGrid() {
            $(".imgtooltip").tooltip({
                delay: 10,
                showURL: false,
                bodyHandler: function () {
                    var imagePath = $(this).attr("abbr");
                    if (imagePath.length == 0) {
                        return "<div><span><%= Resources.Resource.Admin_Catalog_NoMiniPicture %></span></div>";
                    }
                    else {
                        return $("<img/>").attr("src", imagePath);
                    }
                }
            });

            $("tr[rowType='category']").click(function (a) {
                window.location = "Catalog.aspx?CategoryID=" + $(this).attr("element_id");
            });

            $("tr[rowType='goToUpperLevel']").click(function (a) {
                window.location = "Catalog.aspx?CategoryID=" + $(this).attr("element_id");
            });

            $("tr[rowType='category'] input[type='image']").click(function (a) {
                a.cancelBubble = true;
                if (a.stopPropagation) a.stopPropagation();
            });

            $("tr[rowType='category'] a.editbtn").click(function (a) {
                a.cancelBubble = true;
                if (a.stopPropagation) a.stopPropagation();

                open_window('m_Category.aspx?CategoryID=' + $(this).parent().parent().attr("element_id") + '&mode=edit', 750, 640);
            });

            $("tr[rowType='category'] a.deletebtn").click(function (a) {
                a.cancelBubble = true;
                if (a.stopPropagation) a.stopPropagation();
            });

            $("tr[rowType='category'] td.checkboxcolumn").click(function (a) {
                a.cancelBubble = true;
                if (a.stopPropagation) a.stopPropagation();
            });

            $("tr[rowType='category'] input").css("cursor", "pointer");
        }

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
                        if (r) { window.__doPostBack('<%=lbDeleteSelected1.UniqueID%>', ''); $("selectedIdsCount").text("0"); }
                        break;
                    case "deleteSelectedFromCategory":
                        var r = confirm("<%=Resources.Resource.Admin_Catalog_ConfirmDeleteFromCategory%>");
                        if (r) window.__doPostBack('<%=lbDeleteSelectedFromCategory.UniqueID%>', '');
                        break;
                    case "changeCategory":
                        if ($("#SelectedIds").val() != "") {
                            window.__doPostBack('<%=lbChangeCategory.UniqueID%>', '');
                        } else {
                            alert("<%=Resources.Resource.Admin_Catalog_NoSelectedPositions%>");
                        }

                        break;
                }
            });
        });

        function ATreeView_Select(sender, arg) {
            $("a.selectedtreenode").removeClass("selectedtreenode");
            $(sender).addClass("selectedtreenode");
            document.getElementById("TreeView_SelectedValue").value = arg;
            document.getElementById("TreeView_SelectedNodeText").value = sender.innerHTML;
            return false;
        }

    </script>
</asp:Content>
