<%@ Control Language="C#" AutoEventWireup="true" CodeFile="OrdersDWYCountChart.ascx.cs" Inherits="Admin_UserControls_OrdersDWYCountChart" %>
<asp:Panel ID="pnOrdersChart" runat="server"></asp:Panel>
    <script type="text/javascript">
        var api = new jGCharts.Api();
        var opt = {
            size: <%= Width %>+ 'x' + <%= Height %>,
            data: [<%= RenderJGData() %>],
            type: 'bvg',
            colors: [[<%= RenderJGColors() %>]],
            axis_labels: [ <%= string.Format("'{0}' , '{1}', '{2}'", Resources.Resource.Admin_Charts_Day, Resources.Resource.Admin_Charts_Week, Resources.Resource.Admin_Charts_Year) %>],
            //bar_auto : true,
            bar_width: "r,0.5"
          //  grid: true,
          //  grid_x: 100.0 / 3.0
        };
        jQuery('<img>')
        .attr('src', api.make(opt))
        .appendTo("#<%= pnOrdersChart.ClientID %>");
    </script>