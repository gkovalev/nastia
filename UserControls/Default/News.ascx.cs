//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Linq;
using AdvantShop.Configuration;
using AdvantShop.News;

public partial class UserControls_News : System.Web.UI.UserControl
{
    public SettingsDesign.eMainPageMode Mode { set; get; }

    protected void Page_Load(object sender, EventArgs e)
    {

        var news = NewsService.GetNewsForMainPage();
        if (!news.Any())
        {
            Visible = false;
            return;
        }

        lvNews.DataSource = news;
        lvNews.DataBind();



    }
}
