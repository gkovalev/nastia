<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ExportCsv.aspx.cs" Inherits="Admin_ExportCsv" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div id="mainDiv" runat="server">
        <div style="padding-left: 10px; padding-right: 10px">
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="width: 72px;">
                        <img src="images/orders_ico.gif" alt="" />
                    </td>
                    <td>
                        <asp:Label ID="lblAdminHead" runat="server" CssClass="AdminHead" Text="<%$ Resources:Resource, Admin_ExportExcel_Catalog %>"></asp:Label><br />
                        <asp:Label ID="lblAdminSubHead" runat="server" CssClass="AdminSubHead" Text="<%$ Resources:Resource, Admin_ExportExcel_CatalogDownload %>"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div style="padding-left: 10px;">
            <div id="divSomeMessage" runat="server">
                <span><% = Resources.Resource.Admin_ImportCsv_YouCanExport%></span><br />
                <span><% = Resources.Resource.Admin_ImportCsv_YouCanChoose%></span>
            </div>
            <br />
            <asp:Button ID="btnAsyncLoad" runat="server" Text="" Visible="false" OnClick="btnAsyncLoad_Click" />
            <input id="NotDoPost" type="hidden" value="<% =  NotDoPost%>" />
            <div id="divAction" runat="server">
                <table>
                    <tr style="background-color: #EFF0F2">
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
                    <tr style="background-color: #EFF0F2">
                        <td>
                            <span>
                                <% = Resources.Resource.Admin_ExportCsv_Action%>&nbsp;</span>
                        </td>
                        <td>
                            <input type="button" value="<%=  Resources.Resource.Admin_ExportCsv_Deselect %>"
                                onclick="ChangeBtState('deselect')" />&nbsp;
                            <input type="button" value="<%= Resources.Resource. Admin_ExportCsv_Select %>" onclick="ChangeBtState('select')" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <span style="font-weight: bold">
                                <% = Resources.Resource.Admin_ExportCsv_ChoseFields%></span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </div>
            <div id="choseDiv" runat="server">
            </div>
            <center>
                <div id="OutDiv" runat="server" visible="false" style="margin-bottom: 5px">
                    <table>
                        <tr>
                            <td>
                                <span> <% = Resources.Resource.Admin_ExportCsv_ExportCatalog%></span>
                            </td>
                            <td>
                                <div class="progressDiv">
                                    <div class="progressbarDiv" id="textBlock">
                                    </div>
                                    <div id="InDiv" class="progressInDiv">
                                        &nbsp;
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
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
            </center>
            <div id="divbtnAction" runat="server" style="margin-top: 5px; margin-left: 110px;">
                <table>
                    <tr>
                        <td>
                            <adv:OrangeRoundedButton runat="server" ID="btnDownload" OnClick="btnDownload_Click"
                                Text="<%$ Resources: Resource, Admin_ExportExcel_Export %>" />
                        </td>
                        <td>
                            &nbsp;<asp:Literal ID="ltLink" runat="server"></asp:Literal>
                        </td>
                    </tr>
                </table>
            </div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="linkCancel" EventName="Click" />
                </Triggers>
                <ContentTemplate>
                    <center>
                        <asp:LinkButton ID="linkCancel" runat="server" Text="<%$ Resources:Resource, Admin_ImportXLS_CancelImport%>"
                            OnClick="linkCancel_Click"></asp:LinkButton>
                        <div id="hrefAgaint" runat="server">
                            <a href="ExportCsv.aspx">
                                <%= Resources.Resource.Admin_ExportCsv_ExportAgain%></a><br />
                            <br />
                            <table border="0" cellpadding="0" cellspacing="0" style="margin-bottom:5px ">
                                <tr style="height: 27px">
                                    <td style="background-image: url(images/or_buttonbgleft.gif); border-bottom: medium none;
                                        border-left: medium none; width: 5px; border-top: medium none; cursor: pointer;
                                        border-right: medium none">
                                    </td>
                                    <td style="background-image: url(images/or_buttonbg.gif); border-bottom: medium none;
                                        border-left: medium none; border-top: medium none; border-right: medium none">
                                        <asp:Literal ID="hrefLink" runat="server"></asp:Literal>
                                    </td>
                                    <td style="background-image: url(images/or_buttonbgright.gif); border-bottom: medium none;
                                        border-left: medium none; width: 5px; border-top: medium none; cursor: pointer;
                                        border-right: medium none">
                                    </td>
                                </tr>
                            </table>
                            <asp:Literal ID="spanMessage" runat="server"></asp:Literal>
                        </div>
                    </center>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:Label ID="lError" runat="server" ForeColor="Red" Font-Bold="true" Visible="false"
                EnableViewState="false"></asp:Label>
        </div>
        <asp:DropDownList runat="server" ID="ddlProduct" Style="display: none" />
        <script type="text/javascript">
            function Change(obj) {
                var itemTd = $(obj).closest("td");
                var tr = itemTd.closest("tr");
                var tds = tr.children("td");
                var idx = tds.index(itemTd);
                tr.next("tr").find("td:eq(" + idx + ") span").text($("<% = '#' + ddlProduct.ClientID  %> [value='" + $(obj).val() + "']").text());
            }
            function ChangeState(obj) {
                window.location = 'ExportCsv.aspx?state=' + $(obj).val();
            }
            function ChangeBtState(obj) {
                window.location = 'ExportCsv.aspx?state=' + obj;
            }
        </script>
    </div>
    <div id="notInTariff" runat="server" visible="false" class="AdminSaasNotify">
        <center>
            <h2>
                <%=  Resources.Resource.Admin_DemoMode_NotAvailableFeature%>
            </h2>
        </center>
    </div>
</asp:Content>
