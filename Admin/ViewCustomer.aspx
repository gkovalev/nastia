<%@ Page AutoEventWireup="true" CodeFile="ViewCustomer.aspx.cs" Inherits="Admin_ViewCustomer"
    Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" Title="<%$ Resources:Resource, Admin_ViewCustomer_Title %>" %>

<%@ Register Src="UserControls/FindCustomers.ascx" TagName="FindCustomers" TagPrefix="adv" %>
<%@ Register Src="UserControls/CustomerAddressBookAdmin.ascx" TagName="CustomerAddressBookAdmin"
    TagPrefix="adv" %>
<%@ Register Src="UserControls/CustomerOrderHistoryAdmin.ascx" TagName="CustomerOrderHistoryAdmin"
    TagPrefix="adv" %>
<%@ Register Src="UserControls/CustomerRoleActionsAdmin.ascx" TagName="CustomerRoleActionsAdmin"
    TagPrefix="adv" %>
<asp:Content ID="ContentViewCustomer" ContentPlaceHolderID="cphMain" runat="Server">
    <center>
        <asp:Label ID="Message" runat="server" Font-Bold="True" ForeColor="Red" Visible="False" />
    </center>
    <table cellpadding="0px" cellspacing="0px" style="width: 100%;">
        <tbody>
            <tr>
                <td style="width: 10px;">
                    <div style="width: 10px">
                    </div>
                </td>
                <td style="vertical-align: top; width: 100%">
                    <div style="width: 800px; font-size: 0px;">
                    </div>
                    <table style="width: 100%; table-layout: fixed;" cellpadding="0" cellspacing="0">
                        <tbody>
                            <tr>
                                <td style="width: 72px;">
                                    <img src="images/customers_ico.gif" alt="" />
                                </td>
                                <td>
                                    <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_Header %>" /><br />
                                    <asp:Label ID="lblCustomerName" CssClass="AdminSubHead" runat="server" />
                                </td>
                                <td>
                                    <div style="float: right; padding-right: 10px;">
                                        <adv:AdvButton ID="btnChangeCommonInfo" runat="server" Width="170" Text="<%$ Resources:Resource, Admin_Update %>"
                                            CssMain="adv_OrangeButton_Main" CssInput="adv_OrangeButton_Input" CssInputMozz="adv_OrangeButton_Input-mozz"
                                            CssLeftDiv="adv_OrangeButton_LeftDiv" CssRightDiv="adv_OrangeButton_RightDiv"
                                            CssCenterDiv="adv_OrangeButton_CenterDiv" ValidationGroup="0" Visible="true"
                                            OnClick="btnChangeCommonInfo_Click" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top; width: 100%" colspan="3">
                                    <table id="tabs">
                                        <tr>
                                            <td style="width: 225px;">
                                                <ul id="tabs-headers">
                                                    <li id="common">
                                                        <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_CommonInfo %>" />
                                                        <img id="itab1floppy" class="floppy" src="images/floppy.gif" />
                                                    </li>
                                                    <li id="address">
                                                        <asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_AddressBook%>" />
                                                    </li>
                                                    <li id="history">
                                                        <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_History  %>" />
                                                        <asp:Image ID="imgExcl2" ImageUrl="images/excl.gif" runat="server" Visible="false"
                                                            CssClass="exclamation" />
                                                        <img id="itab2floppy" class="floppy" src="images/floppy.gif" alt="" />
                                                    </li>
                                                    <% if (ShowRoleAccess)
                                                       { %>
                                                    <li id="roles">
                                                        <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_Roles %>" />
                                                    </li>
                                                    <% } %>
                                                </ul>
                                                <input type="hidden" runat="server" id="tabid" class="tabid"/>
                                            </td>
                                            <td id="tabs-contents">
                                                <asp:Label ID="lblError" runat="server" CssClass="mProductLabelInfo" ForeColor="Red"
                                                    Visible="False" Font-Names="Verdana" Font-Size="14px" EnableViewState="false" />
                                                <div class="tab-content">
                                                    <asp:UpdatePanel ID="upCommonInfo" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                                        <ContentTemplate>

                                                                <asp:MultiView ID="mvCommonInfo" runat="server" ActiveViewIndex="0">
                                                                    <asp:View ID="vCommonInfo" runat="server">
                                                                        <table class="tabsformtable" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td class="formheader" colspan="2">
                                                                                    <h4 style="display: inline; font-size: 10pt;">
                                                                                        <%=Resources.Resource.Admin_ViewCustomer_CommonInfo%>
                                                                                    </h4>
                                                                                    <asp:Label ID="lblMessage" runat="server" Visible="False" ForeColor="Blue" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="formheaderfooter">
                                                                                <td class="first">
                                                                                </td>
                                                                                <td>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_Email %>" />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label ID="lblEmail" runat="server" />
                                                                                    (<asp:LinkButton ID="llbChangePassword" CssClass="Link" OnClick="llbChangePassword_Click"
                                                                                        runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_ChangePasswordWithDots %>"></asp:LinkButton>)
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_RegistrationDate %>" />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label ID="lblRegistrationDate" runat="server" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_LastName %>" />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtLastName" runat="server" Width="200" />
                                                                                    <asp:RequiredFieldValidator ValidationGroup="0" ID="RequiredFieldValidator4" runat="server"
                                                                                        ControlToValidate="txtLastName" ErrorMessage="*" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_Name %>" />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtFirstName" runat="server" Width="200" />
                                                                                    <asp:RequiredFieldValidator ValidationGroup="0" ID="RequiredFieldValidator5" runat="server"
                                                                                        ControlToValidate="txtFirstName" ErrorMessage="*" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_WWW %>" />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtWWW" runat="server" Width="200" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="Label12" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_Subscribed4News %>" />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:CheckBox ID="chkSubscribed4News" runat="server" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_CustomerGroup %>" />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="ddlCustomerGroup" runat="server" Style="width: 200px;">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trCustomerRole" runat="server">
                                                                                <td>
                                                                                    <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_CustomerRole %>" />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label runat="server" ID="lRole"></asp:Label>
                                                                                    <asp:DropDownList ID="ddlCustomerRole" runat="server" Style="width: 200px;">
                                                                                        <asp:ListItem Text="<%$ Resources:Resource, Admin_ViewCustomer_CustomerRole_User %>"
                                                                                            Value="0" />
                                                                                        <asp:ListItem Text="<%$ Resources:Resource, Admin_ViewCustomer_CustomerRole_Moderator %>"
                                                                                            Value="50" />
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:View>
                                                                    <asp:View ID="vChangePassword" runat="server">
                                                                        <br />
                                                                        <ul id="ulUserRegistarionValidation" runat="server" visible="false" class="ulValidFaild"
                                                                            style="text-align: left; color: Red;">
                                                                            <li>Error1</li>
                                                                            <li>Error2</li>
                                                                        </ul>
                                                                        <table class="tabsformtable" cellspacing="0" cellpadding="0">
                                                                            <tr>
                                                                                <td class="first">
                                                                                    <asp:Label ID="Label15" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_NewPassword %>" />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" />&nbsp;
                                                                                    <asp:RequiredFieldValidator ValidationGroup="0" ID="RequiredFieldValidator2" runat="server"
                                                                                        ControlToValidate="txtNewPassword" ErrorMessage="*" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="first">
                                                                                    <asp:Label ID="Label16" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_NewPasswordConfirm %>" />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtNewPasswordConfirm" runat="server" TextMode="Password" />&nbsp;
                                                                                    <asp:RequiredFieldValidator ValidationGroup="0" ID="RequiredFieldValidator7" runat="server"
                                                                                        ControlToValidate="txtNewPasswordConfirm" ErrorMessage="*" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <br />
                                                                        <center>
                                                                            <asp:Button ID="btnChangePassword" runat="server" Text="<%$ Resources:Resource, Admin_ViewCustomer_ChangePassword %>"
                                                                                OnClick="btnChangePassword_Click" />
                                                                        </center>
                                                                    </asp:View>
                                                                </asp:MultiView>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="tab-content">
                                                    <adv:CustomerAddressBookAdmin ID="CustomerAddressBookAdmin" runat="server" />
                                                </div>
                                                <div class="tab-content">
                                                    <adv:CustomerOrderHistoryAdmin ID="CustomerOrderHistoryAdmin" runat="server" />
                                                </div>
                                                <% if (ShowRoleAccess)
                                                   { %>
                                                <div class="tab-content">
                                                    <adv:CustomerRoleActionsAdmin ID="CustomerRoleActionsAdmin" runat="server" />
                                                </div>
                                                <% } %>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
                <%=AdvantShop.Helpers.HtmlHelper.RenderSplitter()%>
                <td class="rightNavigation">
                    <div id="rightPanel" class="rightPanel">
                        <adv:FindCustomers ID="FindCustomers" runat="server" />
                    </div>
                </td>
                <td style="width: 10px; background-color: White;">
                    <div style="width: 10px; height: 1px;">
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
    <br />
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            &nbsp; &nbsp;
            <asp:Image ID="imgUpdating" runat="server" AlternateText="<%$ Resources:Resource, Admin_Loading %>"
                ImageUrl="images/loading.gif" />
            <asp:Label ID="lblUpdating" runat="server" Text="<%$ Resources:Resource, Admin_Loading %>" />
        </ProgressTemplate>
    </asp:UpdateProgress>
    <script type="text/javascript">

        function toggleRightPanel() {
            if ($.cookie("isVisibleRightPanel") == "true") {
                $("div:.rightPanel").hide("fast");
                $("div:.right_hide_rus").hide("fast");
                $("div:.right_show_rus").show("fast");
                $("div:.right_hide_en").hide("fast");
                $("div:.right_show_en").show("fast");
                $.cookie("isVisibleRightPanel", "false", { expires: 7 });
            } else {
                $("div:.rightPanel").show("fast");
                $("div:.right_show_rus").hide("fast");
                $("div:.right_hide_rus").show("fast");
                $("div:.right_show_en").hide("fast");
                $("div:.right_hide_en").show("fast");
                $.cookie("isVisibleRightPanel", "true", { expires: 7 });
            }
        }
    </script>
</asp:Content>
