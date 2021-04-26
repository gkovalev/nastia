<%@ WebHandler Language="C#" Class="ClearCart" %>

using System.Web;
using AdvantShop.Orders;

public class ClearCart : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        ShoppingCartService.ClearShoppingCart(ShoppingCartType.ShoppingCart);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}