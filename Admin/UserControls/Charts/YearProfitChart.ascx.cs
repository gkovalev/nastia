using System;
using System.Collections.Generic;
using System.Text;
using AdvantShop.Orders;

public partial class Admin_UserControls_YearProfitChart : System.Web.UI.UserControl
{
    protected int _width = 0;
    protected int _height = 0;
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
    protected DateTime minDate = DateTime.Parse(String.Format("Dec 31, {0}", DateTime.Now.Year - 1));
    protected DateTime maxDate = DateTime.Parse(String.Format("Jan 01, {0}", DateTime.Now.Year + 1));
    public string MinDate
    {
        get { return minDate.ToString(); }
        set
        {
            if (!DateTime.TryParse(value, out minDate))
            {
                minDate = DateTime.Parse(String.Format("Dec 15, {0}", DateTime.Now.Year - 1));
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
                maxDate = DateTime.Parse(String.Format("Dec 19, {0}", DateTime.Now.Year));
            }
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected string RenderJGData()
    {
        var result = new StringBuilder();
        Dictionary<DateTime, decimal> sums = OrderStatisticsService.GetOrdersSumByPeriod(minDate, maxDate);
        Dictionary<DateTime, decimal> profits = OrderStatisticsService.GetOrdersProfitByPeriod(minDate, maxDate);
        if (sums == null)
        {
            return "";
        }
        string[] monthsArr = Resources.Resource.Admin_Charts_Months.Split(',');
        for (var i = new DateTime(minDate.AddMonths(1).Year, minDate.AddMonths(1).Month, 1); i < maxDate; )
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
            result.AppendFormat("[{0}, {1}, {2}],", monthsArr[i.Month - 1], sum.ToString(System.Globalization.CultureInfo.InvariantCulture), profit.ToString(System.Globalization.CultureInfo.InvariantCulture));
            i = i.AddMonths(1);
        }
        result.Remove(result.ToString().LastIndexOf(','), 1);
        return result.ToString();
    }
}