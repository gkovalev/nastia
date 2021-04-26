<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ShippingRates.ascx.cs"
    Inherits="Admin_UserControls_ShippingRates" %>
<div id="divScripts" runat="server" Visible="false">
    <script type="text/javascript" src="http://pickpoint.ru/select/postamat.js"></script>
    <script type="text/javascript">
        function SetPickPointAnswer(result) {
            document.getElementById('pickpoint_id').value = result['id'];
            document.getElementById('<%= pickpointId.ClientID  %>').value = result['id'];
            document.getElementById('address').innerHTML = result['name'] + '<br />' + result['address'];
            document.getElementById('<%= pickAddress.ClientID  %>').value = result['name'] + '<br />' + result['address'];
        } </script>
</div>
<script type="text/javascript">
    function setValue(obj) {
        $(".HiddenField").attr("value", $(obj).attr("value"));
    }
</script>
<input type="hidden" id="_selectedID" class="HiddenField" runat="server" value="" />
<input type="hidden" id="pickpointId" class="HiddenField" runat="server" value="" />
<input type="hidden" id="pickAddress" class="HiddenField" runat="server" value="" />
<div id="RadioList" runat="server" class="RadioList" visible="true">
</div>
<asp:Label ID="lblNoShipping" runat="server" Style="color: red" Visible="false" Text="<%$ Resources:Resource, Client_ShippingRates_NoShipping %>"></asp:Label>