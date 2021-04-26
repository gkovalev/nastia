using System;

public partial class Admin_UserControls_GoogleCharts : System.Web.UI.UserControl
{

    public Admin_UserControls_GoogleCharts()
    {
        ChartsWidth = 400;
        ChartsHeight = 150;
    }

    public int ChartsWidth
    {
        get;
        set;
    }
    public int ChartsHeight
    {
        get;
        set;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //progChart.Width = ChartsWidth;
    }
}