<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="OrderConfirmation.aspx.cs" Inherits="OrderConfirmation" %>

<%@ MasterType VirtualPath="MasterPage.master" %>
<%@ Register Src="UserControls/OrderConfirmation/StepLogin.ascx" TagName="StepLogin"
    TagPrefix="adv" %>
<%@ Register Src="UserControls/OrderConfirmation/StepAddress.ascx" TagName="StepAddress"
    TagPrefix="adv" %>
<%@ Register Src="UserControls/OrderConfirmation/StepShipping.ascx" TagName="StepShipping"
    TagPrefix="adv" %>
<%@ Register Src="UserControls/OrderConfirmation/StepPayment.ascx" TagName="StepPayment"
    TagPrefix="adv" %>
<%@ Register Src="UserControls/OrderConfirmation/StepConfirm.ascx" TagName="StepConfirm"
    TagPrefix="adv" %>
<%@ Register Src="UserControls/OrderConfirmation/StepSuccess.ascx" TagName="StepSuccess"
    TagPrefix="adv" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderHeader" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="Server">
    <asp:Literal runat="server" ID="ltGaECommerce" />
    <div class="stroke">
        <div class="content-owner">
            <div style="text-align: center;">
                <asp:Label ID="lblMessage" runat="server" Visible="False" ForeColor="red"></asp:Label>
                <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False"></asp:Label>
            </div>
            <h1>
                <%=Resources.Resource.Client_OrderConfirmation_OrderConfirmation%></h1>
            <asp:Literal ID="ltSteps" runat="server"></asp:Literal>
            <div>
                <asp:MultiView ID="mvOrderConfirm" runat="server" ActiveViewIndex="0">
                    <asp:View ID="ViewAuthorizationCheck" runat="server">
                        <adv:StepLogin ID="ZeroStep" runat="server" OnNextStep="ZeroStep_OnNextStep"></adv:StepLogin>
                    </asp:View>
                    <asp:View ID="ViewOrderConfirmationUser" runat="server">
                        <adv:StepAddress ID="FirstStep" runat="server" OnNextStep="FirstStep_OnNextStep"
                            OnBackStep="FirstStep_OnBackStep"></adv:StepAddress>
                    </asp:View>
                    <asp:View ID="ViewOrderConfirmationShipping" runat="server">
                        <adv:StepShipping ID="SecondStep" runat="server" OnNextStep="SecondStep_OnNextStep"
                            OnBackStep="SecondStep_OnBackStep"></adv:StepShipping>
                    </asp:View>
                    <asp:View ID="ViewOrderConfirmationPayment" runat="server">
                        <adv:StepPayment ID="ThirdStep" runat="server" OnNextStep="ThirdStep_OnNextStep"
                            OnBackStep="ThirdStep_OnBackStep"></adv:StepPayment>
                    </asp:View>
                    <asp:View ID="ViewOrderConfirmationSum" runat="server">
                        <adv:StepConfirm ID="FourthStep" runat="server" OnNextStep="FourthStep_OnNextStep"
                            OnBackStep="ThirdStep_OnBackStep"></adv:StepConfirm>
                    </asp:View>
                    <asp:View ID="ViewOrderConfirmationFinal" runat="server">
                        <adv:StepSuccess ID="FifthStep" runat="server"></adv:StepSuccess>
                    </asp:View>
                </asp:MultiView>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderBottom" runat="server">
    <% if (AdvantShop.Customers.CustomerSession.CurrentCustomer.EMail.Contains("@temp"))
       { %>
    <script type="text/javascript">
            $(function () {
                    var mp = $.advModal({
                        title: "<% = Resources.Resource.Client_OrderConfirmation_Attention %>",
                        buttons: [{ textBtn: "Ok",
                            func: function () {
                                if (UpdateCustomerEmail())
                                    mp.modalClose();
                                return false;
                            },
                            classBtn: "pie group-email btn-action"
                        }],
                        htmlContent: "<% = Resources.Resource.Client_OrderConfirmation_EnterContactEmail %><br /><br /><div style='position:relative'>Email:<br /><input type=\"text\" id=\"customerEmail\" class=\"valid-newemail group-email\"/></div>",
                        clickOut: false,
                        cross: false,
                        closeEsc: false
                    });
                    mp.modalShow();
                    validateControlsPos();

                });
            <% } %>

    </script>
</asp:Content>
