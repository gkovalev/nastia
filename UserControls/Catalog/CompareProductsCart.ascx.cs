//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Web.UI;
using AdvantShop.Core.UrlRewriter;

public partial class UserControls_CompareProductsCart : UserControl
{
    protected string GetAbsoluteLink(string link)
    {
        return UrlService.GetAbsoluteLink(link);
    }
}