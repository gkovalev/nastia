using System;
using System.Data;
using AdvantShop.Core;
using AdvantShop.Diagnostics;

public partial class Admin_UserControls_OrdersDWYCountChart : System.Web.UI.UserControl
{
    protected int _width;
    protected int _height;
    public string Width
    {
        get { return _width.ToString(); }
        set
        {
            if (!int.TryParse(value, out _width))
            {
                _width = 500;
            }
        }
    }
    public string Height
    {
        get { return _height.ToString(); }
        set
        {
            if (!int.TryParse(value, out _height))
            {
                _height = 200;
            }
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected string RenderJGData()
    {
        try
        {
            using (var db = new SQLDataAccess())
            {
                db.cmd.CommandText = "select count (*) from [Order].[Order] WHERE datepart(Week, [OrderDate]) = datepart(Week,getdate()) and datepart(year, [OrderDate]) = datepart(year,getdate())";
                db.cmd.CommandType = CommandType.Text;
                db.cmd.Parameters.Clear();
                db.cnOpen();
                int perWeek = (int)db.cmd.ExecuteScalar();
                db.cmd.CommandText = "select count (*) from [Order].[Order] WHERE datepart(dayofyear, [OrderDate]) = datepart(dayofyear,getdate()) and datepart(year, [OrderDate]) = datepart(year,getdate())";
                int perDay = (int)db.cmd.ExecuteScalar();
                db.cmd.CommandText = "select count (*) from [Order].[Order] WHERE datepart(year, [OrderDate]) = datepart(year,getdate())";
                int perYear = (int)db.cmd.ExecuteScalar();
                db.cnClose();
                return String.Format("{0}, {1}, {2}", perDay, perWeek, perYear);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
            return string.Empty;
        }
    }
    protected string RenderJGColors()
    {
        return string.Empty;
    }
}