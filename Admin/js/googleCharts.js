
google.load('visualization', '1.0', { 'packages': ['corechart'] });

google.setOnLoadCallback(chartInit);



function chartInit() {
    var advantshop = Advantshop;
    var scriptsManager = advantshop.ScriptsManager;
    
    if ($("div.chartOrder").length) {
        var progressOrder = new scriptsManager.Progress.prototype.Init("div.chartOrder");
        progressOrder.Show();

        chartOrder();
        progressOrder.Hide();
    }

    if ($("div.chartSales").length) {
        var progressSales = new scriptsManager.Progress.prototype.Init("div.chartSales");
        progressSales.Show();
        chartProgress();
        progressSales.Hide();
    }

    if ($("div.chartYearChart").length) {
        var progressYear = new scriptsManager.Progress.prototype.Init("div.chartYearChart");
        progressYear.Show();
        chartProfit();
        progressYear.Hide();
    }
    if ($("div.chartProfitDay").length) {
        var progressProfitday = new scriptsManager.Progress.prototype.Init("div.chartProfitDay");
        progressProfitday.Show();
        chartProfitDay();
        progressProfitday.Hide();
    }
}

function chartOrder() {
    var chartObj = $("#hfChartOrder").metadata({ type: 'attr', name: 'value' });
    var data = new google.visualization.DataTable();

    data.addColumn('string', 'Month');
    data.addColumn('number', chartObj.sales);
    data.addColumn('number', chartObj.profit);
    data.addRows(chartObj.rows);

    var options = {
        width: 700,
        height: 300,
        titleFontSize: 14,
        legendFontSize: 14,
        axisFontSize: 12,
        title: chartObj.title
    };

    var bigChart = new google.visualization.AreaChart($("div.chartOrder")[0]);

    bigChart.draw(data, options);
}

function chartProgress() {
    var chartObj = $("#hfChartProgress").metadata({ type: 'attr', name: 'value' });
    var data = new google.visualization.DataTable();

    data.addColumn('string', 'Plan');
    data.addColumn('number', chartObj.done);
    data.addColumn('number', chartObj.plan);
    data.addRows(chartObj.rows);

    var options = {
        width: chartObj.width || 300,
        height: chartObj.height || 200,
        isStacked: true,
        backgroundColor: "#FFFFFF",
        axisColor: "#FFFFFF",
        chartArea: { width: "70%" },
        legend: { alignment: "end", position: "bottom" },
        axisFontSize: 0,
        hAxis: { textPosition: "in" },
        colors: [{ color: '#4A83EE', darker: '#2861CC' }, { color: '#FA9C00', darker: '#D87A00'}],
        is3D: true
    };

    var chart = new google.visualization.BarChart($(".chartSales")[0]);

    chart.draw(data, options);

}

function chartProfit() {
    var chartObj = $("#hfChartProfit").metadata({ type: 'attr', name: 'value' });
    var data = new google.visualization.DataTable();

    data.addColumn('string', 'Month');
    data.addColumn('number', chartObj.sales);
    data.addColumn('number', chartObj.profit);
    data.addRows(chartObj.rows);

    var options = {
        backgroundColor: "#EEEEEE",
        width: chartObj.width || 300,
        height: chartObj.height || 200,
        is3D: true,
        titleFontSize: 12,
        legendFontSize: 10,
        axisFontSize: 10
    };

    var chart = new google.visualization.AreaChart($(".chartYearChart")[0]);

    chart.draw(data, options);

}

function chartProfitDay() {
    var chartObj = $("#hfChartProfitDay").metadata({ type: 'attr', name: 'value' });
    var data = new google.visualization.DataTable();

    data.addColumn('string', 'Month');
    data.addColumn('number', chartObj.sales);
    data.addColumn('number', chartObj.profit);
    data.addRows(chartObj.rows);

    var options = {
        backgroundColor: "#EEEEEE",
        width: chartObj.width || 300,
        height: chartObj.height || 200,
        titleFontSize: 12,
        legendFontSize: 10,
        axisFontSize: 10,
        is3D: false
    };

    var chart = new google.visualization.ColumnChart($(".chartProfitDay")[0]);

    chart.draw(data, options);

}