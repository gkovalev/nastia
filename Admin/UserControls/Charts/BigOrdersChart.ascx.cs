using System;
using System.Collections.Generic;
using System.Text;
using AdvantShop.Orders;

public partial class Admin_UserControls_BigOrdersChart : System.Web.UI.UserControl
{
    protected int _width = 0;
    protected int _height = 0;
    public string Width
    {
        get
        {
            return _width == 0 ? null : _width.ToString();
        }
        set
        {
            if (!int.TryParse(value, out _width))
            {
                _width = 0;
            }
        }
    }
    public string Height
    {
        get
        {
            return _height == 0 ? null : _height.ToString();
        }
        set
        {
            if (!int.TryParse(value, out _height))
            {
                _height = 0;
            }
        }
    }
    protected DateTime minDate = DateTime.Now.AddMonths(-1).Date;
    protected DateTime maxDate = DateTime.Now.Date;
    public string MinDate
    {
        get { return minDate.ToString(); }
        set
        {
            if (!DateTime.TryParse(value, out minDate))
            {
                minDate = DateTime.Now.AddMonths(-1).Date;
            }
        }
    }
    public string MaxDate
    {
        get { return maxDate.ToString(); }
        set
        {
            if (!DateTime.TryParse(value, out maxDate))
            {
                maxDate = DateTime.Now.Date;
            }
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected string RenderJGData()
    {
        var result = new StringBuilder();
        Dictionary<DateTime, decimal> sums = OrderStatisticsService.GetOrdersSumByDays(minDate, maxDate);
        Dictionary<DateTime, decimal> profits = OrderStatisticsService.GetOrdersProfitByDays(minDate, maxDate);
        if (sums == null)
        {
            return "";
        }
        string[] monthsArr = Resources.Resource.Admin_Charts_Months.Split(',');
        for (DateTime i = minDate.Date; i < maxDate; )
        {
            decimal sum = 0;
            decimal profit = 0;
            if (sums.ContainsKey(i))
            {
                sum = sums[i];
            }
            if (profits.ContainsKey(i))
            {
                profit = profits[i];
            }
            result.AppendFormat("['{0}', {1}, {2}],", i.Date.ToString("dd.MM"), sum.ToString(System.Globalization.CultureInfo.InvariantCulture), profit.ToString(System.Globalization.CultureInfo.InvariantCulture));
            i = i.AddDays(1);
        }
        result.Remove(result.ToString().LastIndexOf(','), 1);
        return result.ToString();
    }
}