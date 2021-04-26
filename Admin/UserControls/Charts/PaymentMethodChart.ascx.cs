using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdvantShop.Orders;

public partial class Admin_UserControls_PaymentMethodRatingGoogleChart : System.Web.UI.UserControl
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
    protected string methods;
    protected string dataRow;
    protected string percents;
    protected string GdataRow;
    protected void Page_Load(object sender, EventArgs e)
    {
        var result = new StringBuilder();
        var data = OrderStatisticsService.GetPaymentTypeRating();
        if (data == null)
        {
            dataRow = string.Empty;
            return;
        }
        int total = data.Sum(kv => kv.Value);
        foreach (KeyValuePair<string, int> kv in data)
        {
            GdataRow += string.Format("['{0}', {1}],", kv.Key, kv.Value);
            methods += String.Format("'{0}',", kv.Key);
            result.AppendFormat("[{0}],", kv.Value);
            percents += string.Format("'{0}',", ((decimal)kv.Value / (decimal)total).ToString("P"));
        }
        if (!string.IsNullOrEmpty(GdataRow))
            GdataRow = GdataRow.TrimEnd(',');
        if (!string.IsNullOrEmpty(methods))
            methods = methods.TrimEnd(new char[] { ',' });
        if (!string.IsNullOrEmpty(percents))
            percents = percents.TrimEnd(new char[] { ',' });

        if (result.Length > 0)
            result.Remove(result.ToString().LastIndexOf(','), 1);
        dataRow = result.ToString();
    }
    protected string RenderJGColors()
    {
        return string.Empty;
    }
}