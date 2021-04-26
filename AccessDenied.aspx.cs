//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using AdvantShop.Controls;
using AdvantShop.Helpers;

public partial class AccessDenied : AdvantShopPage
{
     protected void Page_Load(object sender, System.EventArgs e)
     {
         CommonHelper.DisableBrowserCache();
     }
}
