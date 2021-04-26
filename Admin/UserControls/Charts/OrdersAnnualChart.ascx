<%@ Control Language="C#" AutoEventWireup="true" CodeFile="OrdersAnnualChart.ascx.cs"
    Inherits="Admin_UserControls_OrdersAnnualChart" %>
<asp:Panel ID="pnOrdersChart" runat="server">
    <asp:Image ImageUrl="~/Admin/images/ajax-loader.gif" runat="server" ID="imgLoader" />
</asp:Panel>
<script type="text/javascript">
    google.setOnLoadCallback(drawChart);
    function drawChart() {
        var data = new google.visualization.DataTable();
        data.addColumn('string', 'Month');
        data.addColumn('number', 'Sum');
        data.addRows([<%= RenderJGData() %>]);
        var options = {
        <%= string.IsNullOrEmpty(Width) ? "": string.Format("width:{0},",Width)  %>
        <%= string.IsNullOrEmpty(Height) ? "": string.Format("height:{0},",Height)  %>
        legend : 'none',
        title : '<%= Resources.Resource.Admin_Charts_AnnualChart %>'
        };
        options = MergeJSON(options, defaultOptions);
        var chart = new google.visualization.ColumnChart(document.getElementById('<%= pnOrdersChart.ClientID %>'));
        
        chart.draw(data, options);
        $("#<%= imgLoader.ClientID %>").hide();
    } 
</script>
