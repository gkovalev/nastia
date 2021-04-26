//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Web.UI;
using AdvantShop.Configuration;
using AdvantShop.Controls;
using AdvantShop.Helpers;
using AdvantShop.SEO;
using Resources;

public partial class err404 : Page //AdvantShopPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //SetMeta(new MetaInfo(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.err404_Title)), string.Empty);
        CommonHelper.DisableBrowserCache();
        Response.TrySkipIisCustomErrors = true; 
        Response.StatusCode = 404;
        Response.Status = "404 No Found";
    }
}