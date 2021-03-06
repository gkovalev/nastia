//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.IO.Compression;
using AdvantShop.Configuration;
using AdvantShop.Controls;
using AdvantShop.Core.Compress;
using AdvantShop.Helpers;
using AdvantShop.SEO;
using Resources;

public partial class err500 : AdvantShopPage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        CommonHelper.DisableBrowserCache();
        Response.TrySkipIisCustomErrors = true;
        Response.StatusCode = 500;
        Response.Status = "500 Internal Server Error";
        Response.AddHeader(HttpConstants.HttpContentEncoding, HttpConstants.HttpContentEncoding);
    }
}