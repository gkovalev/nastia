<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ExportExcel.aspx.cs" Inherits="Admin_ExportExcel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div id="mainDiv" runat="server">
        <center>
            <asp:Label ID="lblAdminHead" runat="server" CssClass="AdminHead" Text="<%$ Resources:Resource, Admin_ExportExcel_Catalog %>"></asp:Label><br />
            <asp:Label ID="lblAdminSubHead" runat="server" CssClass="AdminSubHead" Text="<%$ Resources:Resource, Admin_ExportExcel_CatalogDownload %>"></asp:Label>
            <br />
        </center>
        <br />
        <br />
        <div style="text-align: center">
            <asp:Button ID="btnAsyncLoad" runat="server" Text="" Visible="false" OnClick="btnAsyncLoad_Click" />
            <input id="NotDoPost" type="hidden" value="<% =  NotDoPost%>" />
            <asp:Button ID="btnDownload" runat="server" Text="<%$ Resources:Resource, Admin_ExportExcel_Export %>"
                OnClick="btnDownload_Click" />
            <br />
            <div id="OutDiv" runat="server" visible="false" style="margin-bottom: 5px">
                <div class="progressDiv">
                    <div class="progressbarDiv" id="textBlock">
                    </div>
                    <div id="InDiv" class="progressInDiv">
                        &nbsp;
                    </div>
                </div>
                <br />
                <div id="divScript" runat="server">
                    <script type="text/javascript">
                        var _timerId = -1;
                        var _stopLinkId = "#<%= linkCancel.ClientID %>";

                        $(document).ready(function () {
                            $.fjTimer({
                                interval: 100,
                                repeat: true,
                                tick: function (counter, timerId) {
                                    _timerId = timerId;
                                    jQuery.ajax({
                                        url: "../HttpHandlers/ExportData.ashx",
                                        dataType: "json",
                                        cache: false,
                                        success: function (data) {
                                            var processed;
                                            if (data.Total != 0) {
                                                processed = Math.round(data.Processed / data.Total * 100);
                                            } else {
                                                processed = 0;
                                            }

                                            $("#textBlock").html(processed + "% (" + data.Processed + "/" + data.Total + ")");
                                            $("#InDiv").css("width", processed + "%");

                                            if ((data.Processed == data.Total) || (!data.IsRun)) {
                                                stopTimer();
                                                if ($("#NotDoPost").val() != "true") {
                                                    window.__doPostBack('<%=btnAsyncLoad.UniqueID%>', '');
                                                }
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
            </div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="linkCancel" EventName="Click" />
                </Triggers>
                <ContentTemplate>
                    <asp:LinkButton ID="linkCancel" runat="server" Text="<%$ Resources:Resource, Admin_ImportXLS_CancelImport%>"
                        OnClick="linkCancel_Click"></asp:LinkButton><br />
                </ContentTemplate>
            </asp:UpdatePanel>
            <br />
            <% =Link%>
            <br />
            <asp:Label ID="lError" runat="server" ForeColor="Blue" Font-Bold="true" Visible="false"
                EnableViewState="false"></asp:Label>
        </div>
    </div>
    <div id="notInTariff" runat="server" visible="false" class="AdminSaasNotify">
        <center>
            <h2>
                <%=  Resources.Resource.Admin_DemoMode_NotAvailableFeature%>
            </h2>
        </center>
    </div>
</asp:Content>
