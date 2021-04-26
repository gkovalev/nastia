<%@ WebHandler Language="C#" Class="DeleteProduct" %>

using System;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using AdvantShop.Catalog;
using AdvantShop.Orders;
using Newtonsoft.Json;

public class DeleteProduct : IHttpHandler, IRequiresSessionState
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
            var compareCart = ShoppingCartService.CurrentCompare;
            ShoppingCartItem item;
            if ((item = compareCart.Find(p => p.ItemType == EnumItemType.Product && p.EntityId == productId)) != null)
            {
                ShoppingCartService.DeleteShoppingCartItem(item.ItemId);
                ReturnResult(context, true);
                return;
            }
        }

        ReturnResult(context, false);
    }

    private static void ReturnResult(HttpContext context, bool result)
    {
        context.Response.ContentType = "application/json";
        context.Response.Write(result ? JsonConvert.True : JsonConvert.False);
        context.Response.End();
    }

    public bool IsReusable
    {
        get { return true;}
    }
}