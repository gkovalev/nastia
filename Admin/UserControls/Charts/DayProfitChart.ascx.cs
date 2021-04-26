using System;
using System.Collections.Generic;
using System.Text;
using AdvantShop.Orders;

public partial class Admin_UserControls_DayProfitChart : System.Web.UI.UserControl
{
    protected int _width;
    protected int _height;
    public string Width
    {
        get
        {
            return _width == 0 ? "300" : _width.ToString();
        }
        set
        {
            if (!int.TryParse(value, out _width))
            {
                _width = 300;
            }
        }
    }

    public string Height
    {
        get
        {
            return _height == 0 ? "200" : _height.ToString();
        }
        set
        {
            if (!int.TryParse(value, out _height))
            {
                _height = 200;
            }
        }
    }
    protected DateTime minDate = DateTime.Now.Date.AddDays(-7);
    protected DateTime maxDate = DateTime.Now.Date.AddDays(1);
    public string MinDate
    {
        get { return minDate.ToString(); }
        set
        {
            if (!DateTime.TryParse(value, out minDate))
            {
                minDate = DateTime.Now.Date.AddDays(-7);
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
                maxDate = DateTime.Now.Date.AddDays(1);
            }
        }
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
        for (DateTime i = minDate; i < maxDate; i = i.AddDays(1))
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
            result.AppendFormat("['{0}', {1}, {2}],", i.ToString("dd.MM"), sum.ToString(System.Globalization.CultureInfo.InvariantCulture), profit.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }
        result.Remove(result.ToString().LastIndexOf(','), 1);
        return result.ToString();
    }
}