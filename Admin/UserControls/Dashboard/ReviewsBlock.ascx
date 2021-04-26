<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ReviewsBlock.ascx.cs"
    Inherits="UserControls_ReviewsBlock" %>
<%@ Import Namespace="System.Globalization" %>
<asp:SqlDataSource ID="sdsReviews" SelectCommand="SELECT TOP 5 ReviewId, AddDate, EntityId, Type, Name, Email, Text, IP FROM [CMS].[Review] WHERE Checked = 0 ORDER BY [AddDate] DESC"
    runat="server" OnInit="sdsReviews_Init"></asp:SqlDataSource>
<table cellpadding="0" cellspacing="0" width="100%" style="padding-left: 0px;">
    <tbody>
        <tr>
            <td class="formheader_order">
                <span style="font-weight: bold">
                    <% =Resources.Resource.Admin_MaterPageAdmin_ReviewsBlock%></span>
            </td>
        </tr>
    </tbody>
</table>
<asp:LinkButton ID="lbPopup" runat="server" Style="display: none;" />
<ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="lbPopup"
    PopupControlID="modalPopup" BackgroundCssClass="blackopacitybackground" BehaviorID="ModalBehaviourReviews">
</ajaxToolkit:ModalPopupExtender>
<asp:Panel ID="modalPopup" CssClass="modal-admin" runat="server">
    <asp:UpdatePanel ID="loginUpdatePanel" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnOk" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
        </Triggers>
        <ContentTemplate>
            <div style="padding: 6px; text-align: left;">
                <asp:Label runat="server" Font-Size="12" Font-Bold="true" Text='<%$ Resources: Resource, Admin_ReviewsBlock_ReviewEdit %>'></asp:Label>
                <br />
                <br />
                <asp:HiddenField runat="server" ID="hfID" />
                <asp:Label runat="server" Text="<%$ Resources: Resource, Admin_ReviewsBlock_UserName %>"></asp:Label>
                <asp:TextBox Width="200" runat="server" ID="txtName" Text=""></asp:TextBox>
                <br />
                <br />
                <asp:Label runat="server" Text="<%$ Resources: Resource, Admin_ReviewsBlock_Email %>"></asp:Label>
                <asp:TextBox Width="200" runat="server" ID="txtEmail" Text=""></asp:TextBox>
                <br />
                <br />
                <asp:Label ID="Label3" runat="server" Text="<%$ Resources: Resource, Admin_ReviewsBlock_Text %>"></asp:Label>
                <br />
                <asp:TextBox Width="95%" Height="70px" runat="server" ID="txtText" TextMode="MultiLine"
                    Text=""></asp:TextBox>
                <br />
                <br />
                <div style="height: 5px; font-size: 5px">
                    &nbsp;</div>
                <asp:Button ID="btnOk" OnClick="btnOk_Click" runat="server" Text="<%$ Resources:Resource,Admin_ReviewsBlock_Save %>" />
                <asp:Button runat="server" ID="btnCancel" Text="<%$Resources:Resource,Admin_ReviewsBlock_Cancel %>"
                    OnClick="btnCancel_Click" OnClientClick="HideModalPopup()" />
                <div style="height: 5px; font-size: 5px">
                    &nbsp;</div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<asp:UpdatePanel runat="server" ID="upReviews" UpdateMode="Conditional">
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="rptReviews" EventName="ItemCommand" />
        <asp:AsyncPostBackTrigger ControlID="rptReviews" EventName="ItemDataBound" />
    </Triggers>
    <ContentTemplate>
        <div style="width: 100%;">
            <asp:Repeater ID="rptReviews" runat="server" DataSourceID="sdsReviews" OnItemDataBound="rptReviews_ItemDataBind"
                OnItemCommand="rptReviews_Command">
                <ItemTemplate>
                    <table width="100%" class="tblReview">
                        <tr>
                            <td rowspan="2" style="width: 120px; padding-right: 15px;">
                                <asp:HyperLink runat="server" CssClass="imgtooltip" ID="ibPhoto" />
                            </td>
                            <td valign="top">
                                <input type="hidden" class="hiddenID" id="hfID1" runat="server" value='<%# Eval("ReviewId") %>' />
                                <asp:HyperLink ID="requestHyperLink" Font-Underline="false" Style="text-transform: uppercase;"
                                    runat="server" ForeColor="#545454"></asp:HyperLink>
                                &nbsp;
                                <asp:Label ID="lblName" CssClass="nameTxt" ForeColor="#929292" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="lblEmail" CssClass="emailTxt" ForeColor="#929292" runat="server" Text='<%# Eval("Email") %>'></asp:Label>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="lblIP" ForeColor="#929292" runat="server" Text='<%# Eval("IP") %>'></asp:Label>
                            </td>
                            <td align="right" style="width: 200px;">
                                <a class="editlink" href="javascript:void(0)">
                                    <asp:Label Font-Bold="true" runat="server" ID="lblEdit" Text='<%$ Resources: Resource, Admin_ReviewsBlock_Edit %>'></asp:Label>
                                </a>
                                <asp:ImageButton runat="server" ID="ibAccept" CommandArgument='<%# Eval("ReviewId") %>'
                                    ToolTip='<%$ Resources: Resource, Admin_ReviewsBlock_Checked %>' CommandName="Accept"
                                    ImageUrl="~/admin/images/ok.gif" />
                                <asp:ImageButton runat="server" ID="ibDelete" CommandArgument='<%# Eval("ReviewId") %>'
                                    ToolTip='<%$ Resources: Resource, Admin_ReviewsBlock_Delete %>' CommandName="Delete"
                                    ImageUrl="~/Admin/images/deletebtn.png" />
                                <ajaxToolkit:ConfirmButtonExtender TargetControlID="ibDelete" runat="server" ConfirmText="<%$ Resources: Resource, Admin_ReviewsBlock_Confirm %>">
                                </ajaxToolkit:ConfirmButtonExtender>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" valign="top" class="tdReviewBottom">
                                <asp:Label runat="server" CssClass="editTxt" ID="lblText" Text='<%# Eval("Text") %>'></asp:Label>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:Repeater>
            <asp:HyperLink CssClass="Link" Font-Bold="true" runat="server" Text="<%$ Resources:Resource, Admin_ReviewsBlock_AllReviews %>"
                NavigateUrl="~/Admin/Reviews.aspx"></asp:HyperLink>
        </div>
        <asp:Label runat="server" ID="Message"></asp:Label>
    </ContentTemplate>
</asp:UpdatePanel>
<div>
    <%= Resources.Resource.Admin_ReviewsBlock_ModerateReviewsNote %></div>
<script type="text/javascript">
    function esc_press(D) {
        D = D || window.event;
        var A = D.keyCode;
        if (A == 27) {
            HideModalPopup();
        }
    }

    document.onkeydown = esc_press;

    function ShowModalPopup() {
        document.body.style.overflowX = 'hidden';
        $find('ModalBehaviourReviews').show();
        document.getElementById('ModalBehaviourReviews_backgroundElement').onclick = HideModalPopup;
        document.getElementById('<%= txtName.ClientID %>').focus();

    }
    function HideModalPopup() {
        $find("ModalBehaviourReviews").hide();
        $('select', 'object', 'embed').each(function () {
            $(this).show();
        });
    }

    $(document).ready(function () {
        $(".imgtooltip").tooltip({
            delay: 10,
            showURL: false,
            bodyHandler: function () {
                return $("<img/>").attr("src", $(this).attr("abbr"));
            }
        });
        $(".editlink").live("click", function () {
            var item = $(this).closest("table");

            $("#<%= hfID.ClientID %>").val(item.find(".hiddenID").val());
            $("#<%= txtEmail.ClientID %>").val(item.find(".emailTxt").text());
            $("#<%= txtName.ClientID %>").val(item.find(".nameTxt").text());
            $("#<%= txtText.ClientID %>").val(item.find(".editTxt").text());
            ShowModalPopup();
        });
    });
    
</script>
