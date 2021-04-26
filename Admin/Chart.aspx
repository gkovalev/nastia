<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Chart.aspx.cs" Inherits="Admin_Chart" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
        <script type="text/javascript" src="/AdvantShop27/admin/js/jquery-1.6.1.js"></script>
        <script type="text/javascript" src="/AdvantShop27/js/jq/jquery.metadata.js"></script>
        <script type="text/javascript" src="https://www.google.com/jsapi"></script>
        <script type="text/javascript" src="js/googleCharts.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div class="chartOrder"></div>
    </div>
    <input type="hidden" id="hfChartOrder" value="{sales:'<%=  Resources.Resource.Admin_Chart_Sales %>', profit:'<%=  Resources.Resource.Admin_Chart_Profit %>', rows: [['09.06', 0, 0],['10.06', 0, 0],['11.06', 0, 0],['12.06', 0, 0],['13.06', 0, 0],['14.06', 0, 0],['15.06', 0, 0],['16.06', 0, 0],['17.06', 0, 0],['18.06', 0, 0],['19.06', 0, 0],['20.06', 0, 0],['21.06', 0, 0],['22.06', 0, 0],['23.06', 0, 0],['24.06', 0, 0],['25.06', 0, 0],['26.06', 0, 0],['27.06', 0, 0],['28.06', 0, 0],['29.06', 0, 0],['30.06', 0, 0],['01.07', 0, 0],['02.07', 0, 0],['03.07', 0, 0],['04.07', 0, 0],['05.07', 0, 0],['06.07', 0, 0],['07.07', 0, 0],['08.07', 0, 0],['09.07', 0, 0],['10.07', 0, 0]], title:'<%=  Resources.Resource.Admin_Chart_SalesProfitByDays %>'  }" />
    </form>
</body>
</html>
