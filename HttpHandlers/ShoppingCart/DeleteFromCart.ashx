<%@ WebHandler Language="C#" Class="DeleteFromCart" %>

using System;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using AdvantShop;
using AdvantShop.Customers;
using AdvantShop.Orders;


public class DeleteFromCart : IHttpHandler, IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        if (context.Request["itemId"].IsInt())
        {
            int itemId = int.Parse(context.Request["itemId"]);
            var carts = ShoppingCartService.GetAllShoppingCarts(CustomerSession.CustomerId);
            if (carts.Any(item => item.ItemId == itemId))
            {
                ShoppingCartService.DeleteShoppingCartItem(itemId);
            }
            context.Response.Write("success");
        }
        else
        {
            context.Response.Write("fail");
        }
        
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}
