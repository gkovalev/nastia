using System.Collections.Generic;
using AdvantShop.Catalog;

//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

public partial class UserControls_BestSellers : System.Web.UI.UserControl
{
    public List<Product> DtBestSellers;

    #region  Public help functions

    private void MsgErr(bool clean)
    {
        if (clean)
        {
            lblErr.Visible = false;
            lblErr.Text = string.Empty;
        }
        else
        {
            lblErr.Visible = false;
        }
    }

    private bool MsgErr()
    {
        return lblErr.Visible;
    }

    private void MsgErr(string messageText)
    {
        lblErr.Visible = true;
        lblErr.Text += @"<br/>" + messageText + @"<br/>";
    }

    #endregion

    protected override void OnLoad(System.EventArgs e)
    {
        MsgErr(true);
    }

    protected override void OnPreRender(System.EventArgs e)
    {
        if (Request.Browser.Crawler) return;
        if (this.Visible)
        {
            //DtBestSellers = ProductService.GetBestSellersWithCache(Settings.UseFakeBestSellers);
            //repeater.DataSource = DtBestSellers;// DtBestSellers.Where(pr => pr.Enabled && pr.InCategory);
            //repeater.DataBind();
        }
    }
}
