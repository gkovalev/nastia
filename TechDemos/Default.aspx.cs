using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop.Helpers;

public partial class TechDemos_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var s = StringHelper.MakeASCIIUrl("http://скайсофт.рф");
        var m = s == "xn--80asbsldip.xn--p1ai";
    }
    protected void Button1_Click(object sender, EventArgs e)
    {

        var s = AdvantShop.Helpers.StringHelper.Translit("Привет");

        // Label1.Text = AdvantShop.Helpers.StringHelper.GetReSpacedString(TextBox1.Text, 5);

        Label1.Text = "";

        object obj = null;

        // Case1111

        try
        {
            Label1.Text = "Case1";
            Label1.Text += string.IsNullOrEmpty(obj.ToString()).ToString();
        }
        catch (Exception ex)
        {
            Label1.Text += "Case1 ERR: " + ex.Message;
        }

        // Case2222

        try
        {
            Label1.Text = "Case2";
            Label1.Text += string.IsNullOrEmpty((string)obj).ToString();
            Label1.Text += "'" + (string)obj + "'";

        }
        catch (Exception ex)
        {
            Label1.Text += "Case1 ERR: " + ex.Message;
        }
    }
}