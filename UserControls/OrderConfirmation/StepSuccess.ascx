<%@ Control Language="C#" AutoEventWireup="true" CodeFile="StepSuccess.ascx.cs" Inherits="UserControls_OrderConfirmation_FifthStep" %>
<%@ Register TagPrefix="adv" TagName="StaticBlock" Src="~/UserControls/StaticBlock.ascx" %>
<script type="text/javascript">
    $(function () {
        getPaymentButton('<%=OrderID%>', 'btnPaymentFunctionality');
    });
    
</script>
<div class="fifthStep">
    <br />
    <br />
   <div class="title">
      <asp:Label ID="lblOrderConfirmation"  runat="server" Text="<%$ Resources:Resource, Client_OrderConfirmation_OrderSuccessfullyConfirmed %>"></asp:Label></div>
      </div>
      <div>
  <!-- Removed by Evgeni  <br />
    <br />
     <asp:Label runat="server" Text="<%$ Resources:Resource, Client_OrderConfirmation_ManagerhvGotOrder %>"></asp:Label>
    <br />
    <br />
    <asp:Label runat="server" Text="<%$ Resources:Resource, Client_OrderConfirmation_MailSent %>"></asp:Label>
    <br />
    <br />-->
    <!--Added by Evgeni to insert static block-->
     <Adv:StaticBlock ID="stOrderConfirmationText" runat="server" SourceKey="OrderConfirmationText" /> 
    <br />
    </div>
    <div class="fifthStep">
    <adv:Button Type="Confirm" Size="Middle" runat="server" ID="lnkToDefault" Text="<%$ Resources:Resource, Client_OrderConfirmation_GotoMain %>" />
<!-- Removed By Evgeni    &nbsp;
    <span id="btnPaymentFunctionality"></span> -->
    &nbsp;
    <adv:Button Type="Submit" Size="Middle" ID="btnPrintOrder" Text="<%$ Resources:Resource,Client_OrderConfirmation_PrintOrder %>" runat="server" />
    <br />
    <br />
    <br />
</div>
