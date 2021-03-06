<%@ Control Language="C#" AutoEventWireup="true" CodeFile="StepShipping.ascx.cs" Inherits="UserControls_OrderConfirmation_SecondStep" %>
<%@ Register TagPrefix="adv" TagName="ShippingRates" Src="~/UserControls/OrderConfirmation/ShippingMethods.ascx" %>
<adv:ShippingRates ID="ShippingRates" runat="server" />
<div class="oc-panel-wr">
    <div class="oc-panel pie">
        <div class="oc-back">
            <adv:Button ID="btnBackFromShipPay" runat="server" Text="<%$ Resources:Resource, Client_OrderConfirmation_Back %>"
                Size="Big" Type="Confirm" OnClick="btnBackFromShipPay_Click" DisableValidation="True" />
        </div>
        <div class="oc-continue">
            <adv:Button ID="btnNextFromShipPay" runat="server" Text="<%$ Resources:Resource, Client_OrderConfirmation_ContinueOrder %>"
                Size="Big" Type="Confirm" OnClick="btnNextFromShipPay_Click" />
        </div>
        <br class="clear" />
    </div>
</div>
