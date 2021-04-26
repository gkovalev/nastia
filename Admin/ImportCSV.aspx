<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ImportCSV.aspx.cs" Inherits="Admin_ImportCSV" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Head" runat="Server">
    <link href="../uploadify/uploadify.css" rel="stylesheet" type="text/css" />
    <%--<script type="text/javascript" src="js/ajaxfileupload.js"></script>--%>
    <script src="../uploadify/jquery.uploadify.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div id="mainDiv" runat="server">
        <div style="padding-left: 10px; padding-right: 10px">
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="width: 72px;">
                        <img src="images/orders_ico.gif" alt="" />
                    </td>
                    <td>
                        <asp:Label runat="server" CssClass="AdminHead" Text="<%$ Resources:Resource, Admin_ImportXLS_Catalog %>"></asp:Label><br />
                        <asp:Label runat="server" CssClass="AdminSubHead" Text="<%$ Resources:Resource, Admin_ImportXLS_CatalogUpload %>"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <asp:Panel ID="pUpload" runat="server">
                <div id="divStart" style="padding-left: 10px;" runat="server">
                    <table>
                        <tr>
                            <td>
                                <span>
                                    <% = Resources.Resource.Admin_ImportCsv_ChoseSeparator%>&nbsp;</span>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlSeparetors" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span>
                                    <% = Resources.Resource.Admin_ImportCsv_ChoseEncoding%>&nbsp;</span>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlEncoding" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:CheckBox runat="server" ID="chbHasHeadrs" Text="<%$ Resources: Resource, Admin_ImportCsv_SkipFirstLine %>"
                                    Checked="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span>
                                    <%= Resources.Resource.Admin_ImportCsv_CsvPath%>&nbsp;</span>
                            </td>
                            <td>
                                <asp:FileUpload ID="FileUpload" runat="server" Width="220" />
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: top;">
                                <span>
                                    <% = Resources.Resource. Admin_ImportCsv_ZipPhotoPath%>&nbsp;</span>
                            </td>
                            <td>
                                <input id="file_upload" name="file_upload" type="file">
                            </td>
                        </tr>
                    </table>
                    <asp:Button ID="btnSaveSettings" runat="server" Text="<%$ Resources: Resource, Admin_ImportCsv_Next %>"
                        OnClick="btnSaveSettings_Click" />
                    <span id="fuPhotoError" style="color: Red; font-weight: bold; display: none;">
                        <%= Resources.Resource.Admin_ImportCsv_SelectFile %></span>
                </div>
                <div id="divAction" runat="server">
                    <div style="padding-left: 10px; margin-top: 10px;">
                        <asp:CheckBox ID="chboxDisableProducts" runat="server" Text="<%$ Resources:Resource, Admin_ImportXLS_DeactiveProducts %>" />
                    </div>
                    <div style="padding-left: 10px; margin-top: 10px; margin-bottom: 10px">
                        <asp:Button runat="server" OnClientClick="javascript:window.location='ImportCSV.aspx'; return false;"
                            Text="<%$ Resources: Resource, Admin_ImportCsv_Back %>" />&nbsp;
                        <asp:Button ID="btnAction" runat="server" Text="<%$ Resources:Resource, Admin_ImportCsv_btnAction %>"
                            OnClick="btnAction_Click" />
                    </div>
                </div>
            </asp:Panel>
            <div style="padding-left: 10px;">
                <div style="margin-bottom: 10px">
                    <asp:Label ID="lblError" runat="server" Font-Bold="True" ForeColor="Red" Visible="False"></asp:Label>
                </div>
                <div id="choseDiv" runat="server" style="float: left;">
                </div>
                <div style="text-align: center;">
                    <span id="lProgress" style="display: none">/</span><br />
                    <div id="OutDiv" runat="server" visible="false" style="margin-bottom: 5px">
                        <div class="progressDiv">
                            <div class="progressbarDiv" id="textBlock">
                            </div>
                            <div id="InDiv" class="progressInDiv">
                                &nbsp;
                            </div>
                        </div>
                        <br />
                        <div>
                            <% = Resources.Resource.Admin_ImportXLS_AddProducts %>:<span id="addBlock" class="">
                            </span>
                            <br />
                            <% =Resources.Resource.Admin_ImportXLS_UpdateProducts%>:<span id="updateBlock" class="">
                            </span>
                            <br />
                            <% = Resources.Resource. Admin_ImportXLS_ProductsWithError %>:<span id="errorBlock"
                                class=""></span>
                        </div>
                        <script type="text/javascript">
                            var _timerId = -1;
                            var _stopLinkId = "#<%= linkCancel.ClientID %>";

                            $(document).ready(function () {
                                $("#lProgress").css("display", "inline");
                                $.fjTimer({
                                    interval: 500,
                                    repeat: true,
                                    tick: function (counter, timerId) {
                                        _timerId = timerId;
                                        switch ($("#lProgress").html()) {
                                            case "\\":
                                                $("#lProgress").html("|");
                                                break;
                                            case "|":
                                                $("#lProgress").html("/");
                                                break;
                                            case "/":
                                                $("#lProgress").html("--");
                                                break;
                                            case "-":
                                                $("#lProgress").html("\\");
                                                break;
                                        }

                                        jQuery.ajax({
                                            url: "../HttpHandlers/ImportData.ashx",
                                            dataType: "json",
                                            cache: false,
                                            success: function (data) {
                                                if (data.Processed != 0) {
                                                    $("#lProgress").css("display", "none");
                                                }
                                                var processed;
                                                if (data.Total != 0) {
                                                    processed = Math.round(data.Processed / data.Total * 100);
                                                } else {
                                                    processed = 0;
                                                }


                                                $("#textBlock").html(processed + "% (" + data.Processed + "/" + data.Total + ")");
                                                $("#InDiv").css("width", processed + "%");

                                                $("#addBlock").html(data.Add);
                                                $("#updateBlock").html(data.Update);
                                                $("#errorBlock").html(data.Error);

                                                if ((!data.IsRun)) {
                                                    stopTimer();
                                                    if (data.Error != 0)
                                                        $("#<%= hlDownloadImportLog.ClientID %>").css("display", "inline");
                                                    $("#<%= hlStart.ClientID %>").css("display", "inline");
                                                    $("#<%= lblRes.ClientID %>").css("display", "inline");
                                                    if (data.Error == 0) {
                                                        $("#<%= lblRes.ClientID %>").html("<% =  Resources.Resource.Admin_ImportXLS_UpdoadingSuccessfullyCompleted %>");
                                                    }
                                                    else {
                                                        $("#<%= lblRes.ClientID %>").html("<% =  Resources.Resource.Admin_ImportXLS_UpdoadingCompletedWithErrors %>");
                                                        $("#<%= lblRes.ClientID %>").css("color", "red");
                                                    }
                                                    $("#<%= linkCancel.ClientID %>").css("display", "none");
                                                }
                                            }
                                        });
                                    }
                                });

                                $(_stopLinkId).click(function () {
                                    if (_timerId != -1) {
                                        stopTimer();
                                    }
                                });
                            });

                            function stopTimer() {
                                clearInterval(_timerId);
                            }
                        </script>
                    </div>
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="linkCancel" EventName="Click" />
                        </Triggers>
                        <ContentTemplate>
                            <asp:LinkButton ID="linkCancel" runat="server" Text="<%$ Resources:Resource, Admin_ImportXLS_CancelImport%>"
                                OnClick="linkCancel_Click"></asp:LinkButton><br />
                            <asp:Label ID="lblRes" runat="server" Font-Bold="True" ForeColor="Blue" Style="display: none" /><br />
                            <asp:HyperLink CssClass="Link" ID="hlDownloadImportLog" runat="server" Style="display: none"
                                Text="<%$ Resources:Resource, Admin_ImportXLS_DownloadImportLog%>" NavigateUrl="~/HttpHandlers/DownloadLog.ashx" />
                            <asp:HyperLink CssClass="Link" ID="hlStart" runat="server" Style="display: none"
                                Text="<%$ Resources:Resource, Admin_ImportCsv_StartLoad%>" NavigateUrl="ImportCSV.aspx" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <div id="notInTariff" runat="server" visible="false" class="AdminSaasNotify">
        <center>
            <h2>
                <%= Resources.Resource.Admin_DemoMode_NotAvailableFeature%>
            </h2>
        </center>
    </div>
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
                                    <asp:Localize ID="Localize_Admin_Product_PleaseWait" runat="server" Text="<%$ Resources:Resource, Admin_Product_PleaseWait %>"></asp:Localize>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <script type="text/javascript">
        $(function () {
            $('#file_upload').uploadify({
                'swf': '../uploadify/uploadify.swf',
                'uploader': '../HttpHandlers/UploadZipPhoto.ashx',
                'buttonText': '<%= Resources.Resource.Admin_ImportCsv_SelectFile %>',
                'onUploadSuccess': function (file, data, response) {
                    if(data != "OK") {
                        alert(data);
                    }
                }
            });
        });
	</script>
</asp:Content>
