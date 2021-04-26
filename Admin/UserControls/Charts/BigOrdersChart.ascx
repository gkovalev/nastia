<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BigOrdersChart.ascx.cs"
    Inherits="Admin_UserControls_BigOrdersChart" %>
<div style="width: 100%; height: 350; border: 1px #CACACA solid;">
    <center>
        <br />
        <br />
        <asp:Panel ID="pnInProgress" runat="server" CssClass="loader_charts">
            <table width="100%" style="font-weight: bold; height: 100%; text-align: center;">
                <tbody valign="middle">
                    <tr>
                        <td style="vertical-align: middle; height: 100%;" align="center">
                            <img src="images/ajax-loader_charts.gif" alt="" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnProfitChart" runat="server">
        </asp:Panel>
    </center>
</div>
<script type="text/javascript" src="http://www.google.com/jsapi"></script>
<script type="text/javascript">
$(document).ready(function(){
    $("#<%= pnInProgress.ClientID %>").width( <%= Width ?? "defaultOptions.width" %> );
    $("#<%= pnInProgress.ClientID %>").height( <%= Height ?? "defaultOptions.height" %> );

});
    google.load("visualization", "1", { packages: ["columnchart", "piechart", "barchart", "areachart"] });
    

    google.setOnLoadCallback(drawChart);
    function drawChart() {
        var data = new google.visualization.DataTable();
        data.addColumn('string', 'Month');
        data.addColumn('number', '<%= Resources.Resource.Admin_Charts_Sales %>');
        data.addColumn('number', '<%= Resources.Resource.Admin_Charts_Profit %>');
        data.addRows([<%= RenderJGData() %>]);
      var  options = {
        <%= string.IsNullOrEmpty(Width) ? "": string.Format("width:{0},",Width)  %>
        <%= string.IsNullOrEmpty(Height) ? "": string.Format("height:{0},",Height)  %>
//        width:300,
        //backgroundColor: "#EEEEEE",
        titleFontSize: 14,
        legendFontSize: 14,
        axisFontSize: 12,
        title: '<%= Resources.Resource.Admin_Charts_BigChart %>'
    };
        //options.width = $('#<%= pnProfitChart.ClientID %>').closest("table").closest("td").width() - 350;
        var chart = new google.visualization.AreaChart(document.getElementById('<%= pnProfitChart.ClientID %>'));
        $("#<%= pnInProgress.ClientID %>").hide();
        chart.draw(data, options);
        
    } 
</script>
