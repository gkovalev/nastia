<%@ Page Title="ExportFeed" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master"
    AutoEventWireup="true" CodeFile="ExportFeedProgress.aspx.cs" Inherits="Admin_ExportFeedProgress" %>

<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div id="mainDiv" runat="server">
        <div style="margin-left: 20px; margin-right: 20px">
            <div class="pageHeader">
                <span class="AdminHead">
                    <asp:Literal runat="Server" Text='<%$ Resources:Resource, Admin_ExportFeed_PageHeader %>' /></span>
                <span id="PageSubheader" visible="false" runat="Server">
                    <br />
                    <span class="AdminSubHead">
                        <asp:Literal runat="Server" Text='<%$ Resources:Resource, Admin_ExportFeed_PageSubHeader %>' />
                        <asp:Literal ID="ModuleNameLiteral" runat="Server" /></span>
                    <br />
                    <br />
                </span>
            </div>
            <br />
            <div class="ui-tabs">
                <div id="tabs-1" class="ui-tabs-panel" style="text-align:center;">
                    <div id="OutDiv" runat="server" visible="false" style="margin-bottom: 5px">
                        <div class="progressDiv">
                            <div class="progressbarDiv" id="textBlock">
                            </div>
                            <div id="InDiv" class="progressInDiv">
                                &nbsp;
                            </div>
                        </div>
                        <div id="Div4">
                            <br />
                            <span>
                                <asp:Literal ID="Literal1" runat="Server" Text='<%$ Resources:Resource, Admin_ExportFeed_ExportedCategories %>' /></span><span
                                    id="lCategories"></span>
                            <br />
                            <span>
                                <asp:Literal ID="Literal2" runat="Server" Text='<%$ Resources:Resource, Admin_ExportFeed_ExportedProducts %>' /></span><span
                                    id="lProducts"></span>
                        </div>
                        <script type="text/javascript">
                            var _timerId = -1;
                            $(document).ready(function () {
                                $.fjTimer({
                                    interval: 500,
                                    repeat: true,
                                    tick: function (counter, timerId) {
                                        _timerId = timerId;
                                        jQuery.ajax({
                                            url: "ExportFeed_Data.ashx",
                                            dataType: "json",
                                            cache: false,
                                            success: function (data) {
                                                var total = data.TotalProducts + data.TotalCategories;
                                                var current = data.CurrentProduct + data.CurrentCategory;

                                                $(".progressDiv").show();
                                                $("#linkDiv").hide();

                                                var processed;
                                                if (total != 0) {
                                                    processed = Math.round(current / total * 100);
                                                } else {
                                                    processed = 0;
                                                }

                                                $("#textBlock").html(processed + "% (" + current + "/" + total + ")");
                                                $("#InDiv").css("width", processed + "%");

                                                $("#lCategories").html(data.CurrentCategory + " / " + data.TotalCategories);
                                                $("#lProducts").html(data.CurrentProduct + " / " + data.TotalProducts);


                                                if (!data.IsRun) {
                                                    stopTimer();
                                                    if (total > 0) {
                                                        $("#exportLink").html(data.FileName + " " + data.FileSize);
                                                        $("#exportLink").attr('href', data.FileName);
                                                        $("#linkDiv").show();
                                                    }
                                                    return;
                                                }
                                            }
                                        });
                                    }
                                });
                            });

                            function stopTimer() {
                                clearInterval(_timerId);
                            }
                        </script>
                    </div>
                    <br>
                    <div id="linkDiv" style="display: none">
                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, Admin_ExportFeed_SuccessfulExport %>" />.
                        <br />
                        <a id="exportLink" target="blank"></a>
                        <br />
                    </div>
                    <a href="ExportFeed.aspx?ModuleId=<% = ModuleName %>">Back</a>
                    <br />
                </div>
            </div>
        </div>
    </div>
    <div id="notInTariff" runat="server" visible="false" class="AdminSaasNotify">
        <center>
            <h2>
                <%=  Resource.Admin_DemoMode_NotAvailableFeature%>
            </h2>
        </center>
    </div>
</asp:Content>
