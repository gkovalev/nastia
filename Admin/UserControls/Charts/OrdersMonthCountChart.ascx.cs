using System;
using System.Collections.Generic;
using System.Text;
using AdvantShop.Orders;

public partial class Admin_UserControls_OrdersMonthCountChart : System.Web.UI.UserControl
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
    protected string RenderJGColors()
    {
        return string.Empty;
    }
    protected string RenderJGData()
    {
        var result = new StringBuilder();
        Dictionary<DateTime, int> data = OrderStatisticsService.GetOrdersCountByPeriod(minDate, maxDate);
        if (data == null)
        {
            return string.Empty;
        }
        string[] monthsArr = Resources.Resource.Admin_Charts_Months.Split(',');
        for (var i = new DateTime(minDate.AddMonths(1).Year, minDate.AddMonths(1).Month, 1); i < maxDate; )
        {
            result.AppendFormat("[{0}, {1}],", monthsArr[i.Month - 1], data.ContainsKey(i) ? data[i] : 0);
            i = i.AddMonths(1);
        }
        result.Remove(result.ToString().LastIndexOf(','), 1);
        return result.ToString();
    }
    protected string RenderMonths(string months, int firstMonth)
    {
        string[] monthsArr = months.Split(',');
        var result = new StringBuilder();
        result.Append(months.Substring(months.IndexOf(monthsArr[firstMonth - 1])));
        result.Append(',');
        result.Append(months.Substring(0, months.IndexOf(monthsArr[firstMonth - 1])));
        result.Remove(result.Length - 1, 1);
        return result.ToString();
    }
}