<%@ WebHandler Language="C#" Class="AddProduct" %>

using System;
using System.Web;
using System.Web.SessionState;
using AdvantShop.Catalog;
using AdvantShop.Orders;
using Newtonsoft.Json;

public class AddProduct : IHttpHandler, IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        if (string.IsNullOrEmpty(context.Request["productid"]))
        {
            return;
        }

        int productId = 0;
        Int32.TryParse(context.Request["productid"], out productId);

        if (productId != 0 && ProductService.IsExists(productId))
        {
            ShoppingCartService.AddShoppingCartItem(new ShoppingCartItem
                {
                    EntityId = productId,
                    Amount = 0,
                    ShoppingCartType = ShoppingCartType.Compare
                });

            ReturnValue(context, true);
            return;
        }

        ReturnValue(context, false);
    }

    private static void ReturnValue(HttpContext context, bool result)
    {
        context.Response.ContentType = "application/json";
        context.Response.Write(result ? JsonConvert.True : JsonConvert.False);
        context.Response.End();
    }

    public bool IsReusable
    {
        get { return true; }
    }
}