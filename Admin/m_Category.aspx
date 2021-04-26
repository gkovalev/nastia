<%@ Page Language="C#" AutoEventWireup="true" CodeFile="m_Category.aspx.cs" Inherits="Admin_m_AddNewCategory"
    MasterPageFile="m_MasterPage.master" ValidateRequest="false" %>


<asp:Content runat="server" ContentPlaceHolderID="cphCenter">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="tree" EventName="TreeNodePopulate" />
        </Triggers>
        <ContentTemplate>
            <ajaxToolkit:ModalPopupExtender ID="mpeTree" runat="server" PopupControlID="pTree"
                TargetControlID="hhl" BackgroundCssClass="blackopacitybackground" CancelControlID="btnCancelParent"
                BehaviorID="ModalBehaviour">
            </ajaxToolkit:ModalPopupExtender>
            <asp:HyperLink ID="hhl" runat="server" Style="display: none;" />
            <asp:Panel runat="server" ID="pTree" CssClass="modal-admin">
                <table>
                    <tbody>
                        <tr>
                            <td>
                                <span style="font-size: 11pt;">
                                    <asp:Localize ID="Localize_Admin_CatalogLinks_ParentCategory" runat="server" Text="<%$ Resources:Resource, Admin_CatalogLinks_ParentCategory %>"></asp:Localize></span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div style="height: 360px; width: 450px; overflow: scroll; background-color: White;
                                    text-align: left">
                                    <asp:TreeView ID="tree" ForeColor="Black" PopulateNodesFromClient="true" runat="server"
                                        ShowLines="True" ExpandImageUrl="images/loading.gif" BackColor="White" OnTreeNodePopulate="PopulateNode"
                                        AutoPostBack="false" OnSelectedNodeChanged="Select_change" SelectedNodeStyle-BackColor="Yellow" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="bottom" style="height: 36px;">
                                <asp:Button ID="btnUpdateParent" runat="server" Text="<%$ Resources:Resource,Admin_CatalogLinks_UpdateCategory %>"
                                    OnClick="btnUpdateParent_Click" />
                                <asp:Button ID="btnCancelParent" runat="server" Text="<%$ Resources: Resource, Admin_Cancel %>"
                                    Width="67" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div style="padding-top: 5px;">
        <center>
            <asp:Label ID="lblBigHead" runat="server" CssClass="AdminHead" Text="<%$ Resources:Resource, Admin_m_Category_Header %>"></asp:Label>
            <br />
            <asp:Label ID="lblSubHead" runat="server" CssClass="AdminSubHead" Text="<%$ Resources:Resource, Admin_m_Category_EditCategory %>"></asp:Label><br />
            <asp:Label ID="lblRestrict" runat="server" Text="Label" Font-Bold="True" Visible="False"
                ForeColor="Red"></asp:Label>
        </center>
        <br />
        <table border="0" cellpadding="2" cellspacing="0" width="100%" id="TABLE2" class="catalog_link">
            <tr style="background-color: #eff0f1;">
                <td style="width: 49%; height: 33px; text-align: right">
                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Resource, Admin_m_Category_Name %>"></asp:Label><span
                        style="color: Red;">*</span>
                </td>
                <td style="vertical-align: middle; height: 33px;">
                    <asp:TextBox ID="txtName" runat="server" Width="230px" Text="" ValidationGroup="vGroup"> </asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName"
                        ValidationGroup="vGroup" ErrorMessage="<%$ Resources:Resource, Admin_FillChar %>"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td style="width: 49%; height: 26px; text-align: right; vertical-align: middle;">
                    <%=Resources.Resource.Admin_m_Product_UrlSynonym%><span style="color: red;">&nbsp;*</span>
                    <br />
                    <span class="PaymentMethod_description">
                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Resource, Admin_m_Category_SynonymInfo %>"></asp:Label>
                    </span>
                </td>
                <td style="vertical-align: middle; height: 26px;">
                    <asp:TextBox ID="txtSynonym" runat="server" Width="354"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="Dynamic"
                        ControlToValidate="txtSynonym" ValidationGroup="vGroup" ErrorMessage="<%$ Resources:Resource, Admin_FillChar %>"></asp:RequiredFieldValidator>
                    <br />
                    <!--Changed By Evgeni to Insert synonim in Russian-->
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtSynonym"
                        Display="Dynamic" ValidationGroup="vGroup" ErrorMessage='<%$ Resources: Resource, Admin_m_Category_SynonymInfo %>'
                        ValidationExpression="^[а-яА-Яa-zA-Z0-9_-]*$"></asp:RegularExpressionValidator>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="updPanel" runat="server" UpdateMode="Conditional">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnDeleteImage" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnDeleteMiniImage" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnUpdateParent" EventName="Click" />
            </Triggers>
            <ContentTemplate>
                <table border="0" cellpadding="2" cellspacing="0" width="100%" id="TABLE3" class="catalog_link">
                    <tr style="background-color: #eff0f1;">
                        <td style="width: 49%; height: 33px; text-align: right;">
                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Resource, Admin_m_Category_Parent %>"></asp:Label>
                        </td>
                        <td style="vertical-align: middle; height: 33px;">
                            <asp:Label ID="lParent" runat="server" Text=""></asp:Label>
                            <asp:LinkButton ID="lbParentChange" runat="server" Text="<%$ Resources:Resource, Admin_m_Category_ChangeParent %>"
                                OnClick="lbParentChange_Click"></asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 49%; text-align: right">
                            <asp:Label ID="lblPhoto" runat="server" Text="<%$ Resources:Resource, Admin_m_Category_Picture %>"></asp:Label>&nbsp;
                        </td>
                        <td style="vertical-align: top;">
                            <asp:Panel ID="pnlImage" runat="server" Width="100%">
                                &nbsp;<asp:Label ID="Label11" runat="server" Text="<%$ Resources:Resource, Admin_m_Category_CurrentImage %>"></asp:Label>
                                <br />
                                &nbsp;<asp:Image ID="Image1" runat="server" Width="200px" />
                                <br />
                                <asp:Button ID="btnDeleteImage" runat="server" Text="<%$  Resources:Resource, Admin_Delete%>"
                                    OnClick="btnDeleteImage_Click" />
                                <br />
                            </asp:Panel>
                            <asp:FileUpload ID="PictureFileUpload" runat="server" Height="20px" Width="308px" />
                            <asp:Label ID="Label10" runat="server" Text="Label" Visible="False"></asp:Label>
                            <br />
                            <asp:Label ID="lblImageInfo" runat="server" Font-Bold="False" Font-Size="Smaller"
                                ForeColor="Gray" Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr style="background-color: #eff0f1;">
                        <td style="width: 49%; text-align: right">
                            <asp:Label ID="lblMiniPhoto" runat="server" Text="<%$ Resources:Resource, Admin_m_Category_MiniPicture %>" />
                        </td>
                        <td style="vertical-align: top;">
                            <asp:Panel ID="pnlMiniImage" runat="server" Width="100%">
                                &nbsp;<asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, Admin_m_Category_CurrentImage %>"></asp:Label>
                                <br />
                                &nbsp;<asp:Image ID="imgMiniPicture" runat="server" Height="80px" />
                                <br />
                                <asp:Button ID="btnDeleteMiniImage" runat="server" Text="<%$  Resources:Resource, Admin_Delete%>"
                                    Style="height: 25px" OnClick="btnDeleteMiniImage_Click" />
                                <br />
                            </asp:Panel>
                            <asp:FileUpload ID="MiniPictureFileUpload" runat="server" Height="20px" Width="308px" />
                            <asp:Label ID="lblMiniPictureFileName" runat="server" Text="Label" Visible="False"></asp:Label>
                            <br />
                            <asp:Label ID="lblMiniPictureInfo" runat="server" Font-Bold="False" Font-Size="Smaller"
                                ForeColor="Gray"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 49%; text-align: right">
                            <asp:Label ID="lblSubCategoryDisplayStyle" runat="server" Text="<%$ Resources:Resource, Admin_m_Category_SubCategoryDisplayStyle %>" />
                        </td>
                        <td style="vertical-align: top;">
                            <asp:DropDownList ID="SubCategoryDisplayStyle" runat="server">
                                <asp:ListItem Text="<%$ Resources:Resource, Admin_m_Category_Tile %>" Value="True" />
                                <asp:ListItem Text="<%$ Resources:Resource, Admin_m_Category_None %>" Value="None" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr style="background-color: #eff0f1;">
                        <td style="width: 49%; height: 33px; text-align: right">
                            <asp:Label ID="lblSortOrder" runat="server" Text="<%$ Resources:Resource, Admin_m_Category_SortOrder %>"></asp:Label>
                        </td>
                        <td style="vertical-align: middle; height: 33px;">
                            <asp:TextBox ID="txtSortIndex" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 49%; height: 33px; text-align: right">
                            <asp:Label ID="lblActive" runat="server" Text="<%$ Resources:Resource, Admin_m_Category_Active %>"></asp:Label>
                        </td>
                        <td style="vertical-align: middle; height: 33px;">
                            <asp:CheckBox ID="ChkEnableCategory" runat="server" Checked="True" />
                        </td>
                    </tr>
                    <tr style="background-color: #eff0f1;">
                        <td style="width: 49%; height: 26px; text-align: right; vertical-align: top;">
                            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Resource, Admin_m_Category_DisplayChildProducts %>" />
                            <br />
                            <span class="PaymentMethod_description">
                                <asp:Localize runat="server" Text="<%$ Resources:Resource, Admin_m_Category_DisplayChildProductsWarning %>"></asp:Localize></span>
                        </td>
                        <td style="vertical-align: middle; height: 26px;">
                            <asp:CheckBox ID="ChkDisplayChildProducts" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 49%; height: 26px; text-align: right; vertical-align: top;">
                            <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Resource, Admin_m_Category_DisplayBrands %>" />
                        </td>
                        <td style="vertical-align: middle; height: 26px;">
                            <asp:CheckBox ID="ChkDisplayBrands" runat="server" Checked="False" />
                        </td>
                    </tr>
                    <tr style="background-color: #eff0f1;">
                        <td style="width: 49%; height: 26px; text-align: right; vertical-align: top;">
                            <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Resource, Admin_m_Category_DisplaySubCategories %>" />
                        </td>
                        <td style="vertical-align: middle; height: 26px;">
                            <asp:CheckBox ID="ChkDisplaySubCategories" runat="server" Checked="False" />
                        </td>
                    </tr>
                    
                    <tr>
                        <td style="width: 49%; height: 26px; text-align: right; vertical-align: top;">
                        </td>
                        <td style="vertical-align: middle; height: 26px;">
                        </td>
                    </tr>
                    <tr style="background-color: #eff0f1;">
                        <td style="width: 49%; height: 26px; text-align: right; vertical-align: top;">
                            <asp:Label ID="Label12" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_UseDefaultMeta %>" />
                        </td>
                        <td style="vertical-align: middle; height: 26px;">
                              <asp:CheckBox runat="server" ID="chbDefaultMeta" Checked="true"/>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 49%; height: 26px; text-align: right; vertical-align: middle;">
                            <%=Resources.Resource.Admin_m_Product_HeadTitle%>
                            <br />
                            <span class="PaymentMethod_description">
                                <asp:Label ID="lDescription" runat="server" Text="<%$ Resources:Resource, Admin_m_Category_UseGlobalVariables %>"></asp:Label>
                            </span>
                        </td>
                        <td style="vertical-align: middle; height: 26px;">
                            <asp:TextBox ID="txtTitle" runat="server" Width="354"></asp:TextBox>
                        </td>
                    </tr>
                    <tr style="background-color: #eff0f1;">
                        <td style="width: 49%; height: 29px; text-align: right">
                            <%=Resources.Resource.Admin_m_Product_MetaKeywords%>
                            <br />
                            <span class="PaymentMethod_description">
                                <asp:Label ID="Label35" runat="server" Text="<%$ Resources:Resource, Admin_m_Category_UseGlobalVariables %>"></asp:Label>
                            </span>
                        </td>
                        <td style="vertical-align: middle; height: 29px;">
                            <asp:TextBox ID="txtMetaKeywords" runat="server" TextMode="MultiLine" Width="354"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 49%; height: 26px; text-align: right; vertical-align: middle;">
                            <%=Resources.Resource.Admin_m_Product_MetaDescription%>
                            <br />
                            <span class="PaymentMethod_description">
                                <asp:Label ID="Label37" runat="server" Text="<%$ Resources:Resource, Admin_m_Category_UseGlobalVariables %>"></asp:Label>
                            </span>
                        </td>
                        <td style="vertical-align: middle; height: 26px;">
                            <asp:TextBox ID="txtMetaDescription" runat="server" TextMode="MultiLine" Width="354"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />
        <table border="0" id="table1" style="width: 100%; height: 206px;" cellspacing="0"
            cellpadding="0">
            <tr>
                <td align="center">
                    <asp:Panel ID="Panel1" runat="server" CssClass="fck">
                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Resource, Admin_m_Category_Description %>"></asp:Label>
                        <br />
                        <CKEditor:CKEditorControl ID="fckDescription" BasePath="~/ckeditor/" runat="server" Value="" Height="300px" />
                    </asp:Panel>
                    &nbsp;
                </td>
            </tr>
        </table>
        <br />
        <table border="0" id="table4" style="width: 100%; height: 206px;" cellspacing="0"
            cellpadding="0">
            <tr>
                <td align="center">
                    <asp:Panel ID="Panel2" runat="server" CssClass="fck">
                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Resource, Admin_m_Category_BriefDescription %>"></asp:Label>
                        <br />
                        <CKEditor:CKEditorControl ID="fckBriefDescription" BasePath="~/ckeditor/" SkinPath="skins/silver/"
                            ToolbarSet="Test" runat="server" Value="" Height="300px" Width="700px" />
                    </asp:Panel>
                    &nbsp;
                </td>
            </tr>
        </table>
        <div style="text-align: center;">
            <%--<asp:ValidationSummary runat="server" ID="vSumm" ValidationGroup="vGroup" ForeColor="Red" />--%>
            &nbsp;<asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False"></asp:Label>
            <asp:HiddenField ID="hfMetaId" runat="server" />
            <br />
            <asp:Button ID="btnAdd" runat="server" Text="Add" Width="103px" ValidationGroup="vGroup"
                OnClick="btnAdd_Click" />&nbsp;
        </div>
        <br />
        <br />
    </div>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="cphScript">
    <script type="text/javascript">
        function removeunloadhandler() {
            window.onbeforeunload = null;
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
            return base$TreeView_PopulateNodeDoCallBack(context, param)
        }

        function hide_wait_for_node(node) {
            if (node.wait_img) {
                node.removeChild(node.wait_img);
            }
        }

        function show_wait_for_node(node) {
            var wait_img = document.createElement("IMG");
            wait_img.src = "images/loading.gif";
            wait_img.border = 0;
            node.wait_img = wait_img;
            node.appendChild(wait_img);
        }

        var _TreePostBack = false;

        function endRequest() {
            if (_TreePostBack) {
                updatetree();
                //document.getElementById('mpeBehavior_backgroundElement').onclick = function () { $find('mpeBehavior').hide(); };
            }
            else {
                window.onbeforeunload = beforeunload;
                $(".photoinput").val("");
            }
        }

        function beforeunload(e) {
            if ($("img.floppy:visible, img.exclamation:visible").length > 0) {
                var evt = window.event || e;
                evt.returnValue = "<%= Resources.Resource.Admin_m_Category_ChangeWillBeLost %>";
            }
        }

        function addbeforeunloadhandler() {
            window.onbeforeunload = beforeunload;
        }

        $(document).ready(function () {
            // edithook();
            window.onbeforeunload = beforeunload;
            document.forms[0].onsubmit = removeunloadhandler;

            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_endRequest(endRequest);
        });

        function ATreeView_Select(sender, arg) {
            $("a.selectedtreenode").removeClass("selectedtreenode");
            $(sender).addClass("selectedtreenode");
            document.getElementById("TreeView_SelectedValue").value = arg;
            document.getElementById("TreeView_SelectedNodeText").value = sender.innerHTML;
            return false;
        }

        function fillUrl() {
            var text = $('#<%=txtName.ClientID %>').val();
            var url = $('#<%=txtSynonym.ClientID %>').val();
            if ((text != "") & (url == "")) {
                $('#<%=txtSynonym.ClientID %>').val(translite(text));
            }
        }

        $(function () {
            if ($('#<%= chbDefaultMeta.ClientID %>').is(":checked")) {
                $('#<%=txtTitle.ClientID %>').attr("disabled", "disabled");
                $('#<%=txtMetaDescription.ClientID %>').attr("disabled", "disabled");
                $('#<%=txtMetaKeywords.ClientID %>').attr("disabled", "disabled");
            }
        });

        $('#<%= chbDefaultMeta.ClientID %>').click(function () {
            if ($('#<%= chbDefaultMeta.ClientID %>').is(":checked")) {
                $('#<%=txtTitle.ClientID %>').val("");
                $('#<%=txtMetaDescription.ClientID %>').val("");
                $('#<%=txtMetaKeywords.ClientID %>').val("");

                $('#<%=txtTitle.ClientID %>').attr("disabled", "disabled");
                $('#<%=txtMetaDescription.ClientID %>').attr("disabled", "disabled");
                $('#<%=txtMetaKeywords.ClientID %>').attr("disabled", "disabled");

            } else {
                $('#<%=txtTitle.ClientID %>').removeAttr("disabled");
                $('#<%=txtMetaDescription.ClientID %>').removeAttr("disabled");
                $('#<%=txtMetaKeywords.ClientID %>').removeAttr("disabled");
            }
        });


        $('#<%=txtSynonym.ClientID %>').focus(fillUrl);
    </script>
</asp:Content>
