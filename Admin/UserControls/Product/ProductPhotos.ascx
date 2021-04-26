<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProductPhotos.ascx.cs"
    Inherits="Admin_UserControls_ProductPhotos" %>
<%@ Import Namespace="AdvantShop.FilePath" %>
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
<table class="table-p">
    <tr>
        <td class="formheader" colspan="2">
            <h2>
                <%=Resources.Resource.Admin_m_Product_Photos%>
            </h2>
            <span id="fuPhotoError" style="color: Red; font-weight: bold; display: none;">
                <%=Resources.Resource.Admin_m_Product_SelectPhoto%></span>
        </td>
    </tr>
    <tr class="formheaderfooter">
        <td>
        </td>
    </tr>
    <tr>
        <td style="height: 22px" valign="bottom">
            <asp:Label ID="Label33" runat="server" Text="<%$ Resources:Resource, Admin_m_Product_Path %>"
                EnableViewState="false"></asp:Label>
        </td>
        <td style="height: 17px;">
            <input type="file" id="fuPhoto" name="fuPhoto" style="width: 455px" /><input type="button"
                style="height: 21px; width: 83px" name="btnUploadPhoto" value="<%=Resources.Resource.Admin_m_Product_Upload%>"
                onclick="ajaxFileUpload()" />
        </td>
    </tr>
    <tr style="height: 10px;">
        <td>
        </td>
    </tr>
    <tr style="background-color: #eff0f1;">
        <td style="vertical-align: bottom; height: 24px;" valign="bottom" colspan="2">
            <asp:Label ID="Label47" runat="server" Text="<%$ Resources:Resource, Admin_m_Product_PhotoDescription %>"
                Font-Bold="False"></asp:Label>
        </td>
    </tr>
    <tr>
        <td style="height: 68px;" valign="bottom" colspan="2">
            <asp:TextBox ID="txtPhotoDescription" runat="server" Height="63px" Width="523px"
                TextMode="MultiLine" CssClass="photoinput toencode"></asp:TextBox>
        </td>
    </tr>
    <tr style="height: 40px;">
        <td colspan="2">
            <asp:Label ID="Label20" runat="server" Text="<%$ Resources:Resource, Admin_m_Product_CurrentImages %>"
                EnableViewState="false"></asp:Label>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Label ID="lPhotoMessage" runat="server" ForeColor="Red" Visible="false" EnableViewState="false"></asp:Label>
            <asp:LinkButton ID="lnkUpdatePhoto" runat="server" OnClick="lnkUpdatePhoto_Click"
                EnableViewState="false" />
            <div style="width: 100%">
                <table style="width: 100%;" class="massaction">
                    <tr>
                        <td>
                            <span class="admin_catalog_commandBlock"><span style="display: inline-block; margin-right: 3px;">
                                <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_Command %>"></asp:Localize>
                            </span><span style="display: inline-block;">
                                <select id="commandSelect">
                                    <option value="selectAll">
                                        <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_SelectAll %>"
                                            EnableViewState="false"></asp:Localize>
                                    </option>
                                    <option value="unselectAll">
                                        <asp:Localize ID="Localize3" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_UnselectAll %>"
                                            EnableViewState="false"></asp:Localize>
                                    </option>
                                    <option value="selectVisible">
                                        <asp:Localize ID="Localize4" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_SelectVisible %>"
                                            EnableViewState="false"></asp:Localize>
                                    </option>
                                    <option value="unselectVisible">
                                        <asp:Localize ID="Localize5" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_UnselectVisible %>"
                                            EnableViewState="false"></asp:Localize>
                                    </option>
                                    <option value="deleteSelected">
                                        <asp:Localize ID="Localize6" runat="server" Text="<%$ Resources:Resource, Admin_Catalog_DeleteSelected %>"
                                            EnableViewState="false"></asp:Localize>
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
                            <asp:UpdatePanel ID="upCounts" runat="server">
                                <Triggers>
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
                <div style="border: 1px #c9c9c7 solid; width: 100%; background-color: White;">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="pageNumberer" EventName="SelectedPageChanged" />
                            <asp:AsyncPostBackTrigger ControlID="lbDeleteSelected" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnGo" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="ddRowsPerPage" EventName="SelectedIndexChanged" />
                        </Triggers>
                        <ContentTemplate>
                            <adv:AdvGridView ID="grid" runat="server" AllowSorting="true" AutoGenerateColumns="False"
                                CellPadding="2" CellSpacing="0" Confirmation="<%$ Resources:Resource, Admin_Currencies_QDelete %>"
                                CssClass="tableview" Style="cursor: pointer" DataFieldForEditURLParam="" DataFieldForImageDescription="Description"
                                EditURL="" GridLines="None" OnRowCommand="grid_RowCommand" DataFieldForImagePath="PhotoName"
                                OnSorting="grid_Sorting" OnRowDeleting="grid_RowDeleting" OnRowDataBound="grid_RowDataBound"
                                ShowFooter="false" TooltipImgCellIndex="2">
                                <Columns>
                                    <asp:TemplateField AccessibleHeaderText="ID" Visible="false">
                                        <EditItemTemplate>
                                            <asp:Label ID="Label0" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label01" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
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
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="PhotoName" HeaderStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="120px" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>
                                            <div style="width: 120px; font-size: 0px; height: 0px;">
                                            </div>
                                            <%= Resources.Resource.Admin_m_Product_Image %>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Image ID="imgPath" runat="server" ImageUrl='<%# FoldersHelper.GetImageProductPath(ProductImageType.XSmall, Eval("PhotoName").ToString(), true) %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Description" HeaderStyle-HorizontalAlign="Left">
                                        <HeaderTemplate>
                                            <div style="width: 200px; font-size: 0px; height: 0px;">
                                            </div>
                                            <asp:LinkButton ID="lbDescription" runat="server" CommandName="Sort" CommandArgument="Description">
                                                <%= Resources.Resource.Admin_Product_Description %>
                                                <asp:Image ID="arrowDescription" CssClass="arrow" runat="server" ImageUrl="~/admin/images/arrowdownh.gif" /></asp:LinkButton>
                                        </HeaderTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtDescription" runat="server" Text='<%# Eval("Description") %>'
                                                Width="99%"></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lCode" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="PhotoSortOrder" HeaderStyle-HorizontalAlign="Left"
                                        ItemStyle-Width="100px">
                                        <HeaderTemplate>
                                            <div style="width: 100px; font-size: 0px; height: 0px;">
                                            </div>
                                            <asp:LinkButton ID="lbPhotoSortOrder" runat="server" CommandName="Sort" CommandArgument="PhotoSortOrder">
                                                <%= Resources.Resource.Admin_m_Product_Order %>
                                                <asp:Image ID="arrowPhotoSortOrder" CssClass="arrow" runat="server" ImageUrl="~/admin/images/arrowdownh.gif" /></asp:LinkButton>
                                        </HeaderTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtPhotoSortOrder" runat="server" Text='<%# Eval("PhotoSortOrder") %>'
                                                Width="99%"></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lPhotoSortOrder" runat="server" Text='<%# Bind("PhotoSortOrder") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField AccessibleHeaderText="Main" HeaderStyle-HorizontalAlign="Left"
                                        ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>
                                            <div style="width: 100px; height: 0px; font-size: 0px;">
                                            </div>
                                            <asp:LinkButton ID="lbMain" runat="server" CommandName="Sort" CommandArgument="Main">
                                                <%= Resources.Resource.Admin_m_Product_Default %>
                                                <asp:Image ID="arrowMain" CssClass="arrow" runat="server" ImageUrl="~/admin/images/arrowdownh.gif" /></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cbMain" runat="server" Checked='<%# Eval("Main")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="Center"
                                        ItemStyle-Width="90px">
                                        <EditItemTemplate>
                                            <input id="btnUpdate" name="btnUpdate" class="updatebtn showtooltip" type="image"
                                                src="~/admin/images/updatebtn.png" onclick="<%# this.Page.ClientScript.GetPostBackEventReference(grid, "Update$" + Container.DataItemIndex)%>;return false;"
                                                style="display: none" title='<%= Resources.Resource.Admin_MasterPageAdminCatalog_Update%>' />
                                            <asp:ImageButton ID="buttonDelete" runat="server" ImageUrl="~/admin/images/deletebtn.png"
                                                CssClass="deletebtn showtooltip" CommandName="Delete" CommandArgument='<%# Eval("ID")%>'
                                                ToolTip='<%$ Resources:Resource, Admin_MasterPageAdminCatalog_Delete %>' />
                                            <ajaxToolkit:ConfirmButtonExtender ID="ConfirmButtonExtender2" runat="server" TargetControlID="buttonDelete"
                                                ConfirmText="<%$ Resources:Resource, Admin_Product_ConfirmDeletingPhoto %>">
                                            </ajaxToolkit:ConfirmButtonExtender>
                                            <input id="btnCancel" name="btnCancel" class="cancelbtn showtooltip" type="image"
                                                src="images/cancelbtn.png" onclick="row_canceledit($(this).parent().parent()[0]);return false;"
                                                style="display: none" title="<%=Resources.Resource.Admin_MasterPageAdminCatalog_Cancel %>" />
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
                                        <%=Resources.Resource.Admin_m_Product_NoFoto %>
                                    </center>
                                </EmptyDataTemplate>
                            </adv:AdvGridView>
                            <input type="hidden" id="SelectedIds" name="SelectedIds" />
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
                                        <asp:Panel ID="goToPage" runat="server" DefaultButton="btnGo" EnableViewState="false">
                                            <span style="color: #494949">
                                                <%=Resources.Resource.Admin_Catalog_PageNum%>&nbsp;<asp:TextBox ID="txtPageNum" runat="server"
                                                    Width="30" /></span>
                                            <asp:Button ID="btnGo" runat="server" CssClass="btn" Text="<%$ Resources:Resource, Admin_Catalog_GO %>"
                                                OnClick="linkGO_Click" EnableViewState="false" />
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </td>
    </tr>
</table>
<asp:UpdateProgress runat="server" ID="uprogress">
    <ProgressTemplate>
        <div id="inprogress">
            <div id="curtain" class="opacitybackground">
                &nbsp;</div>
            <div class="loader">
                <table width="100%" style="font-weight: bold; text-align: center;">
                    <tbody>
                        <tr>
                            <td align="center">
                                <img src="~/admin/images/ajax-loader.gif" alt="" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center" style="color: #0D76B8;">
                                <asp:Localize ID="Localize_Admin_Product_PleaseWait" runat="server" Text="<%$ Resources:Resource, Admin_Product_PleaseWait %>"
                                    EnableViewState="false"></asp:Localize>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<script type="text/javascript">
    function ajaxFileUpload() {
         $("#fuPhotoError").hide();
        if($("#fuPhoto").val() == ""){
            $("#fuPhotoError").fadeIn();
            return false;
        }

            //starting setting some animation when the ajax starts and completes
         $("#<%= uprogress.ClientID %>")
		.ajaxStart(function () {
		    $(this).show();
		})
		.ajaxComplete(function () {
		    $(this).hide();
		});

            /*
            prepareing ajax file upload
            url: the url of script file handling the uploaded files
            fileElementId: the file type of input element id and it will be the index of  $_FILES Array()
            dataType: it support json, xml
            secureuri:use secure protocol
            success: call back function when the ajax complete
            error: callback function when the ajax failed
            */
         $.ajaxFileUpload(
            {
                url: '../HttpHandlers/UploadPhoto.ashx?ProductID=<%= Request["productid"] %>&description=' + encodeURIComponent($("#<%=txtPhotoDescription.ClientID %>").val()),
                secureuri: false,
                fileElementId: 'fuPhoto',
                dataType: 'json',
                success: function(data, status) {
                    if (typeof(data.error) != 'undefined') {
                        if (data.error != '') {
                            alert(data.error);
                        } else {
                            <%= Page.ClientScript.GetPostBackEventReference(lnkUpdatePhoto, "") %>;
                        }
                    }
                },
                error: function(data, status, e) {
                    alert(e);
                }
            }
        );
            return false;
    }
    <%--
        $.ajaxFileUpload(
            {
                url: '../HttpHandlers/UploadPhoto.ashx?ProductID=<%= Request["productid"] %>&description=' + encodeURIComponent($("#<%=txtPhotoDescription.ClientID %>").val()) + '&posX=' + encodeURIComponent($("#<%=hfWatermarkPositionX.ClientID %>").val())+ '&posY=' + encodeURIComponent($("#<%=hfWatermarkPositionY.ClientID %>").val()) + '&active=' + encodeURIComponent($("#<%= ckbEnableWatermark.ClientID %>").is(":checked")),
                secureuri: false,
                fileElementId: 'fuPhoto',
                dataType: 'json',
                success: function(data, status) {
                    if (typeof(data.error) != 'undefined') {
                        if (data.error != '') {
                            alert(data.error);
                        } else {
                            <%= Page.ClientScript.GetPostBackEventReference(lnkUpdatePhoto, "") %>;
                        }
                    }
                },
                error: function(data, status, e) {
                    alert(e);
                }
            }
        );
            return false;
    }
   
      $(document).ready(function () {
        $(".div_watermark table tr td").click(function () {
            $(".div_watermark table tr td").css("background-image", "");
            $(this).css("background-image", "url('../Admin/images/td_watermark_adv.png')");
            $("#<%=hfWatermarkPositionX.ClientID %>").val($(this).attr("data-x"));
            $("#<%=hfWatermarkPositionY.ClientID %>").val($(this).attr("data-y"));
        });

        $(".div_watermark table tr td[data-x='" + $("#<%=hfWatermarkPositionX.ClientID %>").val() + "'][data-y='" + $("#<%=hfWatermarkPositionY.ClientID %>").val() + "']").css("background-image", "url('../Admin/images/td_watermark_adv.png')");
    });
    --%>

</script>
