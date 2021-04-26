<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ResizeProductPhotos.ascx.cs"
    Inherits="Admin_UserControls_ResizePhoto_Product" %>
<table border="0" cellpadding="2" cellspacing="0">
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <asp:Localize ID="Localize7" runat="server" Text='<%$ Resources: Resource, Admin_ResizePhotos_Products %>'></asp:Localize></span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <div class="dvMailNotify">
                <asp:Localize ID="lclNotify" runat="server" Text="<%$ Resources:Resource, Admin_ResizePhotos_Notify %>" /></div>
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <asp:Localize ID="Localize6" runat="server" Text="<%$ Resources: Resource, Admin_ResizePhotos_BigProductImages %>" /></span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize21" runat="server" Text='<%$ Resources: Resource, Admin_ResizePhotos_Width %>'></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:TextBox class="niceTextBox shortTextBoxClass" ID="txtBigWidth" runat="server"></asp:TextBox> px.
            <asp:RangeValidator ID="RangeValidator1" runat="server" Type="Integer" MinimumValue="0"
                MaximumValue="1900" ControlToValidate="txtBigWidth"></asp:RangeValidator>
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize1" runat="server" Text='<%$ Resources: Resource, Admin_ResizePhotos_Height %>'></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:TextBox class="niceTextBox shortTextBoxClass" ID="txtBigHight" runat="server"></asp:TextBox> px.
            <asp:RangeValidator ID="RangeValidator0" runat="server" Type="Integer" MinimumValue="0"
                MaximumValue="1900" ControlToValidate="txtBigHight"></asp:RangeValidator>
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources: Resource, Admin_ResizePhotos_MiddleProductImages %>" /></span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize16" runat="server" Text='<%$ Resources: Resource, Admin_ResizePhotos_Width %>'></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:TextBox class="niceTextBox shortTextBoxClass" ID="txtMiddleWidth" runat="server"></asp:TextBox> px.
            <asp:RangeValidator ID="RangeValidator3" runat="server" Type="Integer" MinimumValue="0"
                MaximumValue="1900" ControlToValidate="txtMiddleWidth"></asp:RangeValidator>
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize2" runat="server" Text='<%$ Resources: Resource, Admin_ResizePhotos_Height %>'></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:TextBox class="niceTextBox shortTextBoxClass" ID="txtMiddleHeight" runat="server"></asp:TextBox> px.
            <asp:RangeValidator ID="RangeValidator2" runat="server" Type="Integer" MinimumValue="0"
                MaximumValue="1900" ControlToValidate="txtMiddleHeight"></asp:RangeValidator>
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <asp:Localize ID="Localize10" runat="server" Text="<%$ Resources: Resource, Admin_ResizePhotos_SmallProductImages %>" /></span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize4" runat="server" Text='<%$ Resources: Resource, Admin_ResizePhotos_Width %>'></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:TextBox class="niceTextBox shortTextBoxClass" ID="txtSmallWidth" runat="server"></asp:TextBox> px.
            <asp:RangeValidator ID="RangeValidator5" runat="server" Type="Integer" MinimumValue="0"
                MaximumValue="1900" ControlToValidate="txtSmallWidth"></asp:RangeValidator>
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize3" runat="server" Text='<%$ Resources: Resource, Admin_ResizePhotos_Height %>'></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:TextBox class="niceTextBox shortTextBoxClass" ID="txtSmallHeight" runat="server"></asp:TextBox> px.
            <asp:RangeValidator ID="RangeValidator4" runat="server" Type="Integer" MinimumValue="0"
                MaximumValue="1900" ControlToValidate="txtSmallHeight"></asp:RangeValidator>
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <asp:Localize ID="Localize11" runat="server" Text="<%$ Resources: Resource, Admin_ResizePhotos_XSmallProductImages %>" /></span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize8" runat="server" Text='<%$ Resources: Resource, Admin_ResizePhotos_Width %>'></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:TextBox class="niceTextBox shortTextBoxClass" ID="txtXSmallWidth" runat="server"></asp:TextBox> px.
            <asp:RangeValidator ID="RangeValidator7" runat="server" Type="Integer" MinimumValue="0"
                MaximumValue="1900" ControlToValidate="txtXSmallWidth"></asp:RangeValidator>
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize5" runat="server" Text='<%$ Resources: Resource, Admin_ResizePhotos_Height %>'></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:TextBox class="niceTextBox shortTextBoxClass" ID="txtXSmallHeight" runat="server"></asp:TextBox> px.
            <asp:RangeValidator ID="RangeValidator6" runat="server" Type="Integer" MinimumValue="0"
                MaximumValue="1900" ControlToValidate="txtXSmallHeight"></asp:RangeValidator>
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <asp:Localize ID="Localize12" runat="server" Text="<%$ Resources: Resource, Admin_ResizePhotos_ResaveProductPhotos %>" /></span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <div class="dvMailNotify">
                <asp:Localize ID="Localize13" runat="server" Text="<%$ Resources:Resource, Admin_ResizePhotos_NotifyResave %>" /></div>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Button runat="server" ID="btnSave" OnClick="btnSave_Click" Text="<%$ Resources:Resource, Admin_ResizePhoto_Resize %>" />
            <br />
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
                                        url: "../HttpHandlers/ProductPhotoData.ashx",
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
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="linkCancel" EventName="Click" />
                </Triggers>
                <ContentTemplate>
                    <asp:LinkButton ID="linkCancel" runat="server" Text="<%$ Resources:Resource, Admin_ResizePhoto_Cancel%>"
                        OnClick="linkCancel_Click"></asp:LinkButton><br />
                </ContentTemplate>
            </asp:UpdatePanel>
            <br />
            <asp:Label ID="lError" runat="server" ForeColor="Blue" Font-Bold="true" Visible="false"
                EnableViewState="false"></asp:Label>
        </td>
    </tr>
</table>
