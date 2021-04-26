<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ShippingMethodChart.ascx.cs"
    Inherits="Admin_UserControls_ShippingRatingGoogleChart" %>
<asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, Admin_Charts_ShippingChart %>"
    Font-Bold="true" Style="display: none; margin-bottom: 5px;"></asp:Label>
<br />
<asp:Panel ID="pnShippingChart" runat="server">
</asp:Panel>
<script type="text/javascript">
    google.setOnLoadCallback(drawChart);
    function drawChart() {
        var data = new google.visualization.DataTable();
        data.addColumn('string', 'Method');
        data.addColumn('number', 'Count');
        data.addRows([<%= GdataRow %>]);
        var options = {
        <%= string.IsNullOrEmpty(Width) ? "": string.Format("width:{0},",Width)  %>
        <%= string.IsNullOrEmpty(Height) ? "": string.Format("height:{0}",Height)  %>
       // title : '<%= Resources.Resource.Admin_Charts_ShippingChart %>'};
        options = MergeJSON(options, defaultOptions);
        var chart = new google.visualization.PieChart(document.getElementById('<%= pnShippingChart.ClientID %>'));
        chart.draw(data, options);
        $("#<%= Label1.ClientID %>").show();
    } 
</script>
