<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PaymentMethodChart.ascx.cs"
    Inherits="Admin_UserControls_PaymentMethodRatingGoogleChart" %>
<asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, Admin_Charts_BillingChart %>"
    Font-Bold="true" Style="display: none; margin-bottom: 5px;"></asp:Label><br />
<asp:Panel ID="pnPaymentChart" runat="server">
    <asp:Image ImageUrl="~/Admin/images/ajax-loader.gif" runat="server" ID="imgLoader" /></asp:Panel>
<script type="text/javascript" src="http://www.google.com/jsapi"></script>
<script type="text/javascript">

    google.load("visualization", "1", { packages: ["piechart"] });
    google.setOnLoadCallback(drawChart);
    function drawChart() {
        var data = new google.visualization.DataTable();
        data.addColumn('string', 'Method');
        data.addColumn('number', 'Count');
        data.addRows([<%= GdataRow %>]);
        var options = {
        <%= string.IsNullOrEmpty(Width) ? "": string.Format("width:{0},",Width)  %>
        <%= string.IsNullOrEmpty(Height) ? "": string.Format("height:{0},",Height)  %>
        title : ''
       // title : '<%= Resources.Resource.Admin_Charts_BillingChart %>'
        };
        options = MergeJSON(options, defaultOptions);
        var chart = new google.visualization.PieChart(document.getElementById('<%= pnPaymentChart.ClientID %>'));
        $("#<%= imgLoader.ClientID %>").hide();
        chart.draw(data, options);
        $("#<%= Label1.ClientID %>").show();
    } 
</script>
