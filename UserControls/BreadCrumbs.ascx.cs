using System;
using System.Collections.Generic;
using AdvantShop.CMS;
using System.Text;

public partial class UserControls_BreadCrumbs : System.Web.UI.UserControl
{

    public List<BreadCrumbs> Items = new List<BreadCrumbs>();


    protected void Page_Load(object sender, EventArgs e)
    {
        if (Items.Count == 0)
        {
            this.Visible = false;
        }

        var result = new StringBuilder();
        result.Append("<div class=\"crumbs\">");


        for (int i = 0; i < Items.Count; ++i)
        {

            if (!string.IsNullOrEmpty(Items[i].Url) && i != Items.Count - 1)
                result.AppendFormat("<a href=\"{0}\">{1}</a>", Items[i].Url, Items[i].Name);
            else
                result.Append(Items[i].Name);
            if (i != Items.Count - 1)
            {
                result.Append("<span class=\"arrow\"></span>");
            }
        }
        result.Append("</div>");
        ltrlNavi.Text = result.ToString();
    }
}