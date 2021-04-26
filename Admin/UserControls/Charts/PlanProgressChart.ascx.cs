using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI.WebControls;
using AdvantShop.Orders;

public partial class Admin_UserControls_PlanProgressChart : System.Web.UI.UserControl
{
    protected KeyValuePair<decimal, decimal> prog;
    protected decimal plannedSales = OrderStatisticsService.SalesPlan;
    protected decimal plannedProfit = OrderStatisticsService.ProfitPlan;
    protected string ProfitLegend;
    protected string SalesLegend;
    protected string SalesColors;
    protected string ProfitColors;
    [DefaultValue(false)]
    public bool DrawSales { get; set; }
    [DefaultValue(false)]
    public bool DrawProfit { get; set; }

    public int Width { get; set; }
    public int Height { get; set; }

    public Admin_UserControls_PlanProgressChart()
    {
        Width = 300;
        Height = 200;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        pnSales.Width = Width;
        //pnSalesChart.Width = Convert.ToInt32(Width);
        //pnProfitChart.Width = Convert.ToInt32(Width);
        prog = OrderStatisticsService.GetMonthProgress();
    }
    protected string RenderJGColors()
    {
        return string.Empty;
    }
    //protected string RenderJGProfit()
    //{

    //    return String.Format("['" + Resources.Resource.Admin_Charts_Profit + "', {0}, {1}]", prog.Value < planne Math.Min(prog.Value, plannedProfit).ToString(System.Globalization.CultureInfo.InvariantCulture), Math.Abs(plannedProfit - prog.Value).ToString(System.Globalization.CultureInfo.InvariantCulture));
    //}
    protected string RenderJGSales()
    {
        if (prog.Key < plannedSales)
        {
            return String.Format("['" + Resources.Resource.Admin_Charts_Sales + "',{0}, {1}]", prog.Key.ToString("F0"), plannedSales.ToString("F0"));
        }
        return String.Format("['" + Resources.Resource.Admin_Charts_Sales + "',{0}, 0]", prog.Key.ToString("F0"));
    }
}