//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Web.UI;
using AdvantShop;
using AdvantShop.Orders;

public partial class UserControls_ShoppingCartSocial : UserControl
{
    protected string Count = "";
    protected void Page_PreRender(object sender, EventArgs e)
    {
        int itemsCount = ShoppingCartService.CurrentShoppingCart.TotalItems;
        Count = string.Format("{0} {1}", itemsCount == 0 ? "" : itemsCount.ToString(),
                                 Strings.Numerals(itemsCount, Resources.Resource.Client_UserControls_ShoppingCart_Empty,
                                                  Resources.Resource.Client_UserControls_ShoppingCart_1Product,
                                                  Resources.Resource.Client_UserControls_ShoppingCart_2Products,
                                                  Resources.Resource.Client_UserControls_ShoppingCart_5Products));
    }
}