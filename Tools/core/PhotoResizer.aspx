<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PhotoResizer.aspx.cs" Inherits="Tools_PhotoResizer"
    MasterPageFile="MasterPage.master" %>

<asp:Content runat="server" ID="cntHead" ContentPlaceHolderID="head">
    <script src="../../js/jq/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../../Admin/js/jquerytimer.pack.js" type="text/javascript"></script>
    <link href="../../Admin/css/AdminStyle.css" rel="stylesheet" type="text/css" />
    <title>Photo resizer</title>
</asp:Content>
<asp:Content runat="server" ID="cntMain" ContentPlaceHolderID="main">
    <div style="text-align: center;">
        <div style="width: 700px;">
            <asp:Button ID="btnDownload" runat="server" Text="Пережать фотографии" OnClick="btnDownload_Click" />
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
                                        url: "../../HttpHandlers/ProductPhotoData.ashx",
                                        dataType: "json",
                                        cache: false,
                                        success: function (data) {
                                            if (data.Total != 0) {
                                                processed = Math.round(data.Processed / data.Total * 100);
                                            } else {
                                                processed = 0;
                                            }

                                            $("#textBlock").html(processed + "% (" + data.Processed + "/" + data.Total + ")");
                                            $("#InDiv").css("width", processed + "%");

                                            if (data.Processed == data.Total) {
                                                stopTimer();
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
            <asp:ScriptManager ID="scr" runat="server">
            </asp:ScriptManager>
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
            <asp:Label ID="lError" runat="server" ForeColor="Blue" Font-Bold="true" Visible="false"
                EnableViewState="false"></asp:Label>
        </div>
    </div>
</asp:Content>
