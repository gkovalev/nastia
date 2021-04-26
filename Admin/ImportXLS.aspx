<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ImportXLS.aspx.cs" Inherits="Admin_ImportXLS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div id="mainDiv" runat="server">
        <center>
            <asp:Label ID="lblAdminHead" runat="server" CssClass="AdminHead" Text="<%$ Resources:Resource, Admin_ImportXLS_Catalog %>"></asp:Label><br />
            <asp:Label ID="lblAdminSubHead" runat="server" CssClass="AdminSubHead" Text="<%$ Resources:Resource, Admin_ImportXLS_CatalogUpload %>"></asp:Label>
            <br />
        </center>
        <br />
        <br />
        <asp:Panel ID="pUploadExcel" runat="server">
            <div style="text-align: center;">
                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Resource, Admin_ImportXLS_XLSPath %>" />
                <asp:FileUpload ID="FileUpload1" runat="server" />
                <asp:Button ID="btnLoad" runat="server" Height="22px" Text="<%$ Resources:Resource, Admin_ImportXLS_Upload %>"
                    OnClick="btnLoad_Click" />
            </div>
            <div style="text-align: center; margin-top: 20px;">
                <asp:CheckBox ID="chboxDisableProducts" runat="server" Text="<%$ Resources:Resource, Admin_ImportXLS_DeactiveProducts %>" />
            </div>
        </asp:Panel>
        <center>
            <div style="text-align: left; width: 600px;">
                <center>
                    <asp:Label ID="lblError" runat="server" Font-Bold="True" ForeColor="Red" Visible="False"></asp:Label>
                </center>
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

                                            if ((data.Processed == data.Total && data.Total != 0) || (!data.IsRun)) {
                                                stopTimer();
                                                $("#<%= hlDownloadImportLog.ClientID %>").css("display", "inline");
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
                <center>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="linkCancel" EventName="Click" />
                        </Triggers>
                        <ContentTemplate>
                            <asp:LinkButton ID="linkCancel" runat="server" Text="<%$ Resources:Resource, Admin_ImportXLS_CancelImport%>"
                                OnClick="linkCancel_Click"></asp:LinkButton><br />
                            <asp:Label ID="lblRes" runat="server" Font-Bold="True" ForeColor="Blue" Style="display: none" /><br />
                            <asp:HyperLink CssClass="Link" ID="hlDownloadImportLog" runat="server" Style="display: none"
                                Text="<%$ Resources:Resource, Admin_ImportXLS_DownloadImportLog%>" NavigateUrl="~/HttpHandlers/ImportLog.ashx" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </center>
            </div>
        </center>
    </div>
    <div id="notInTariff" runat="server" visible="false" class="AdminSaasNotify">
        <center>
            <h2>
                <%=  Resources.Resource.Admin_DemoMode_NotAvailableFeature%>
            </h2>
        </center>
    </div>
</asp:Content>
