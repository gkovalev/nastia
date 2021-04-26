<%@ WebHandler Language="C#" Class="UpdateCart" %>

using System;
using System.Web;
using AdvantShop;
using AdvantShop.Orders;
using Newtonsoft.Json;

public class UpdateCart : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        if (context.Request["list"].IsNotEmpty())
        {
            
            foreach (var item in context.Request["list"].Split(';'))
            {
                if (item.IsNotEmpty() && item.Contains("_"))
                {
                    var cart = ShoppingCartService.CurrentShoppingCart;
                    var itemid = item.Split('_')[0].TryParseInt(0);
                    var amount = item.Split('_')[1].TryParseInt(0);
                    
                    var shpCartItem = cart.Find(cartitem => cartitem.ItemId == itemid && cartitem.ItemType == EnumItemType.Product);
                    if (shpCartItem != null && amount > 0 && !shpCartItem.Product.CanOrderByRequest)
                    {
                        shpCartItem.Amount = amount;
                        ShoppingCartService.UpdateShoppingCartItem(shpCartItem);
                    }
                    else
                    {
                        context.Response.Write("fail");
                    }
                }
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
