//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Linq;
using AdvantShop.Configuration;
using AdvantShop.Customers;

public partial class UserControls_RecentlyView : System.Web.UI.UserControl
{
    public int ProductsToShow { set; get; }

    public UserControls_RecentlyView()
    {
        ProductsToShow = 3;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Browser.Crawler || !SettingsDesign.RecentlyViewVisibility)
        {
            this.Visible = false;
            return;
        }

        var tempList = RecentlyViewService.LoadViewDataByCustomer(CustomerSession.CustomerId, ProductsToShow);
        if (tempList.Any())
        {
            lvRecentlyView.DataSource = tempList;
            lvRecentlyView.DataBind();
        }
        else
        {
            Visible = false;
        }
    }
}