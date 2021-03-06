<%@ Control Language="C#" AutoEventWireup="true" CodeFile="StepPayment.ascx.cs" Inherits="UserControls_OrderConfirmation_ThirdStep" %>
<%@ Register TagPrefix="adv" TagName="PaymentMethods" Src="~/UserControls/OrderConfirmation/PaymentMethods.ascx" %>
<adv:PaymentMethods runat="server" ID="pm"></adv:PaymentMethods>
<div class="oc-panel-wr">
    <div class="oc-panel pie">
        <div class="oc-back">
                <adv:Button ID="btnBackFromShipPay" runat="server" Size="Big" Type="Confirm" CssClass="btn-back"
            Text="<%$ Resources:Resource, Client_OrderConfirmation_Back %>" OnClick="btnBackFromShipPay_Click"
            DisableValidation="True" />
        </div>
       <div class="oc-continue">
        
        <adv:Button ID="btnNextFromShipPay" runat="server" Size="Big" Type="Confirm" CssClass="btn-continue"
            Text="<%$ Resources:Resource, Client_OrderConfirmation_ContinueOrder %>" OnClick="btnNextFromShipPay_Click" />
        </div>
        <br class="clear "/>

    </div>
</div>
