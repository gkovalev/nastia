<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="PaymentMethod.aspx.cs" Inherits="Admin_PaymentMethod" %>

<%@ Reference Control="~/Admin/UserControls/PaymentMethods/MasterControl.ascx" %>
<%@ Import Namespace="System.Drawing" %>
<%@ Register Src="~/Admin/UserControls/PaymentMethods/MasterControl.ascx" TagName="PaymentMethod"
    TagPrefix="adv" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Head" runat="Server">
    <script type="text/javascript">
        $(function () {
            $(".paymentType").hover(
                function () {
                    $(this).addClass("ptHovered");
                },
                function () {
                    $(this).removeClass("ptHovered");
                });

            $.advModal({
                title: "<%= Resources.Resource.Admin_PaymentMethod_Adding %>",
                control: $("#<%= btnAddMethod.ClientID%>"),
                htmlContent: $("#<%= modal.ClientID%>"),
                beforeOpen: clearModal
            });

        });

        function clearModal() {
            $("#<%= txtName.ClientID %>").val("");
            $("#<%= txtDescription.ClientID %>").val("");
            $("#<%= txtSortOrder.ClientID %>").val("");
            $("#<%= txtName.ClientID %>").focus();
        }
        function showElement(span) {
            var method_id = $("input:hidden", $(span)).val();
            if ($("hfPayment" + method_id).length == 0)
                location = "PaymentMethod.aspx?PaymentMethodID=" + method_id;
        }
        function hoverElement(span) {
            if (span.className == "selected") {
                span.className = "selected_hovered";
            } else {
                span.className = "hovered";
            }
        }

        function outElement(span) {
            if (span.className == "selected_hovered" || span.className == "selected") {
                span.className = "selected";
            } else {
                span.className = "free";
            }
        }    
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="Server">
    <div id="divModal" style="display: none;">
        <asp:Panel ID="modal" runat="server">
            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnOk" />
                </Triggers>
                <ContentTemplate>
                    <div style="background-color: white; padding-top: 10px; padding-bottom: 10px;">
                        <div style="text-align: center;">
                            <table class="form-payment-shipping">
                                <tr>
                                    <td style="width: 150px;">
                                        <asp:Label ID="Label1" runat="server" Text="<%$  Resources:Resource, Admin_PaymentMethods_Name %>"></asp:Label><span
                                            class="required">*</span>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtName" Width="300"></asp:TextBox>
                                        <asp:RequiredFieldValidator Display="Dynamic" ID="RequiredFieldValidator1" runat="server"
                                            ControlToValidate="txtName" EnableClientScript="true" Style="display: inline;"
                                            ErrorMessage='<%$ Resources: Resource, Admin_PaymentMethod_NameRequired %>'></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text="<%$  Resources:Resource, Admin_PaymentMethods_Type %>"></asp:Label><span
                                            class="required">*</span>
                                    </td>
                                    <td>
                                        <adv:EnumDataSource runat="server" ID="edsTypes" EnumTypeName="AdvantShop.Payment.PaymentType" />
                                        <asp:DropDownList runat="server" ID="ddlType" DataTextField="name" DataValueField="value" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Text="<%$  Resources:Resource, Admin_PaymentMethod_Description %>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtDescription" TextMode="MultiLine" Width="300"
                                            Height="100"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label4" runat="server" Text="<%$  Resources:Resource, Admin_PaymentMethod_SortOrder %>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtSortOrder" Width="300"></asp:TextBox>
                                        <asp:RegularExpressionValidator Display="Dynamic" ID="RegularExpressionValidator1"
                                            runat="server" ControlToValidate="txtSortOrder" EnableClientScript="true" ValidationExpression="[0-9]*"
                                            ErrorMessage="<%$ Resources: Resource, Admin_SortOrder_MustBeNumeric %>"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align: right;">
                                        <asp:LinkButton ID="btnOk" runat="server" OnClientClick="return Page_ClientValidate();"
                                            OnClick="btnOk_Click" Text="<%$ Resources: Resource, Admin_PaymentMethod_Create %>"></asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </div>
    <div style="text-align: center;">
        <table style="width: 98%; table-layout: fixed;" cellpadding="0" cellspacing="0">
            <tbody>
                <tr>
                    <td style="width: 72px;">
                        <img src="images/customers_ico.gif" alt="" />
                    </td>
                    <td>
                        <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_Header %>"></asp:Label><br />
                        <asp:Label ID="lblPaymentMethod" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_PaymentMethod_SubHeader %>"></asp:Label>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblMessage" Visible="false" ForeColor="Blue"></asp:Label>
                    </td>
                    <td>
                        <div style="float: right; margin-left: 7px; position: relative; width: 180px; text-align: left;">
                            <adv:OrangeRoundedButton Font-Bold="true" Width="170" ID="btnAddMethod" runat="server"
                                Text="<%$ Resources:Resource, Admin_Add %>" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top; width: 100%" colspan="4">
                        <table cellpadding="0px" cellspacing="0px" style="width: 100%;">
                            <tr>
                                <td style="vertical-align: top; width: 225px;">
                                    <ul class="tabs" id="tabs-headers">
                                        <asp:Repeater runat="server" ID="rptTabs">
                                            <ItemTemplate>
                                                <li runat="server" onclick="javascript:showElement(this)" class='<%# (int)Eval("PaymentMethodID") == PaymentMethodId ? "selected" : "" %>'>
                                                    <asp:HiddenField runat="server" Value='<%# Eval("PaymentMethodID") %>' />
                                                    <asp:Label ForeColor='<%# (bool)Eval("Enabled") ? Color.Black : Color.Gray %>' ID="Literal4"
                                                        runat="server" Text='<%# Eval("Name") %>' />
                                                </li>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ul>
                                </td>
                                <td class="tabContainer" id="tabs-contents">
                                    <asp:Label ID="lblError" runat="server" CssClass="mProductLabelInfo" ForeColor="Red"
                                        Visible="False" Font-Names="Verdana" Font-Size="14px" EnableViewState="false"></asp:Label>
                                    <asp:Panel runat="server" ID="pnMethods">
                                        <adv:PaymentMethod runat="server" ID="ucSberBank" PaymentType="SberBank" OnSaved="PaymentMethod_Saved"
                                            OnErr="PaymentMethod_Error" />
                                        <adv:PaymentMethod runat="server" ID="ucBill" PaymentType="Bill" OnSaved="PaymentMethod_Saved"
                                            OnErr="PaymentMethod_Error" />
                                        <adv:PaymentMethod runat="server" ID="ucCash" PaymentType="Cash" OnSaved="PaymentMethod_Saved"
                                            OnErr="PaymentMethod_Error" />
                                        <adv:PaymentMethod runat="server" ID="ucMailRu" PaymentType="MailRu" OnSaved="PaymentMethod_Saved"
                                            OnErr="PaymentMethod_Error" />
                                        <adv:PaymentMethod runat="server" ID="ucWebMoney" PaymentType="WebMoney" OnSaved="PaymentMethod_Saved"
                                            OnErr="PaymentMethod_Error" />
                                        <adv:PaymentMethod runat="server" ID="ucRobokassa" PaymentType="Robokassa" OnSaved="PaymentMethod_Saved"
                                            OnErr="PaymentMethod_Error" />
                                        <adv:PaymentMethod runat="server" ID="ucYandexMoney" PaymentType="YandexMoney" OnSaved="PaymentMethod_Saved"
                                            OnErr="PaymentMethod_Error" />
                                        <adv:PaymentMethod runat="server" ID="ucAuthorizeNet" PaymentType="AuthorizeNet"
                                            OnSaved="PaymentMethod_Saved" OnErr="PaymentMethod_Error" />
                                        <adv:PaymentMethod runat="server" ID="ucGoogleCheckout" PaymentType="GoogleCheckout"
                                            OnSaved="PaymentMethod_Saved" OnErr="PaymentMethod_Error" />
                                        <adv:PaymentMethod runat="server" ID="uceWAY" PaymentType="eWAY" OnSaved="PaymentMethod_Saved"
                                            OnErr="PaymentMethod_Error" />
                                        <adv:PaymentMethod runat="server" ID="ucCheck" PaymentType="Check" OnSaved="PaymentMethod_Saved"
                                            OnErr="PaymentMethod_Error" />
                                        <adv:PaymentMethod runat="server" ID="ucPayPal" PaymentType="PayPal" OnSaved="PaymentMethod_Saved"
                                            OnErr="PaymentMethod_Error" />
                                        <adv:PaymentMethod runat="server" ID="ucTwoCheckout" PaymentType="TwoCheckout" OnSaved="PaymentMethod_Saved"
                                            OnErr="PaymentMethod_Error" />
                                        <adv:PaymentMethod runat="server" ID="ucAssist" PaymentType="Assist" OnSaved="PaymentMethod_Saved"
                                            OnErr="PaymentMethod_Error" />
                                        <adv:PaymentMethod runat="server" ID="ucZPayment" PaymentType="ZPayment" OnSaved="PaymentMethod_Saved"
                                            OnErr="PaymentMethod_Error" />
                                        <adv:PaymentMethod runat="server" ID="ucPlatron" PaymentType="Platron" OnSaved="PaymentMethod_Saved"
                                            OnErr="PaymentMethod_Error" />
                                        <adv:PaymentMethod runat="server" ID="ucRbkmoney" PaymentType="Rbkmoney" OnSaved="PaymentMethod_Saved"
                                            OnErr="PaymentMethod_Error" />
                                        <adv:PaymentMethod runat="server" ID="ucCyberPlat" PaymentType="CyberPlat" OnSaved="PaymentMethod_Saved"
                                            OnErr="PaymentMethod_Error" />
                                        <adv:PaymentMethod runat="server" ID="ucAmazonSimplePay" PaymentType="AmazonSimplePay"
                                            OnSaved="PaymentMethod_Saved" OnErr="PaymentMethod_Error" />
                                        <adv:PaymentMethod runat="server" ID="ucMoneybookers" PaymentType="Moneybookers"
                                            OnSaved="PaymentMethod_Saved" OnErr="PaymentMethod_Error" />
                                        <adv:PaymentMethod runat="server" ID="ucChronoPay" PaymentType="ChronoPay" OnSaved="PaymentMethod_Saved"
                                            OnErr="PaymentMethod_Error" />
                                        <adv:PaymentMethod runat="server" ID="ucPayOnline" PaymentType="PayOnline" OnSaved="PaymentMethod_Saved"
                                            OnErr="PaymentMethod_Error" />
                                        <adv:PaymentMethod runat="server" ID="ucPSIGate" PaymentType="PSIGate" OnSaved="PaymentMethod_Saved"
                                            OnErr="PaymentMethod_Error" />
                                        <adv:PaymentMethod runat="server" ID="ucPayPoint" PaymentType="PayPoint" OnSaved="PaymentMethod_Saved"
                                            OnErr="PaymentMethod_Error" />
                                        <adv:PaymentMethod runat="server" ID="ucSagePay" PaymentType="SagePay" OnSaved="PaymentMethod_Saved"
                                            OnErr="PaymentMethod_Error" />
                                        <adv:PaymentMethod runat="server" ID="ucWorldPay" PaymentType="WorldPay" OnSaved="PaymentMethod_Saved"
                                            OnErr="PaymentMethod_Error" />
                                        <adv:PaymentMethod runat="server" ID="ucOnPay" PaymentType="OnPay" OnSaved="PaymentMethod_Saved"
                                            OnErr="PaymentMethod_Error" />
                                        <adv:PaymentMethod runat="server" ID="ucPickPoint" PaymentType="PickPoint" OnSaved="PaymentMethod_Saved"
                                            OnErr="PaymentMethod_Error" />
                                        <adv:PaymentMethod runat="server" ID="ucCashOnDelivery" PaymentType="CashOnDelivery"
                                            OnSaved="PaymentMethod_Saved" OnErr="PaymentMethod_Error" />
                                        <adv:PaymentMethod runat="server" ID="ucGiftCertificate" PaymentType="GiftCertificate"
                                            OnSaved="PaymentMethod_Saved" OnErr="PaymentMethod_Error" />
                                        <adv:PaymentMethod runat="server" ID="ucMasterBank" PaymentType="MasterBank" OnSaved="PaymentMethod_Saved"
                                            OnErr="PaymentMethod_Error" />
                                        <adv:PaymentMethod runat="server" ID="ucWalletOneCheckout" PaymentType="WalletOneCheckout"
                                            OnSaved="PaymentMethod_Saved" OnErr="PaymentMethod_Error" />
                                        <adv:PaymentMethod runat="server" ID="ucQiwi" PaymentType="QIWI" OnSaved="PaymentMethod_Saved"
                                            OnErr="PaymentMethod_Error" />
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</asp:Content>
