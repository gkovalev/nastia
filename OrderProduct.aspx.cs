//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Linq;
using System.Drawing;
using AdvantShop.Catalog;
using AdvantShop.Controls;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Orders;

public partial class OrderProduct : AdvantShopPage
{
    private string Code
    {
        get { return Request["code"]; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Code))
        {
            RedirectToMainPage();
            return;
        }

        // Если код правильный, и такого же товара нет в корзине - то всё ок.
        var orderByRequest = OrderByRequestService.GetOrderByRequest(Code);
        if (orderByRequest != null && orderByRequest.IsValidCode && ProductService.IsExists(orderByRequest.ProductId) &&
            ShoppingCartService.CurrentShoppingCart.Count(p => p.ItemType == EnumItemType.Product && p.EntityId == orderByRequest.ProductId) == 0)
        {
            ShoppingCartService.AddShoppingCartItem(new ShoppingCartItem
                                                        {
                                                            EntityId = orderByRequest.ProductId,
                                                            Amount = orderByRequest.Quantity,
                                                            ShoppingCartType = ShoppingCartType.ShoppingCart
                                                        });

            Response.Redirect(UrlService.GetAbsoluteLink("/shoppingcart.aspx"));
            return;
        }
        
        lblMessage.Text = Resources.Resource.Client_OrderProduct_Message;
        lblMessage.ForeColor = Color.Red;
    }

    protected string GetMainPageLink()
    {
        return UrlService.GetAbsoluteLink("/default.aspx");
    }
}