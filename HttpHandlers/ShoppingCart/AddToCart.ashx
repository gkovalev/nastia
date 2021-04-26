<%@ WebHandler Language="C#" Class="AddToCart" %>

using System;
using System.Collections.Generic;
using System.Web;
using AdvantShop;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Orders;


public class AddToCart : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        if (context.Request["productid"].IsInt() && context.Request["amount"].IsInt())
        {
            int productId = int.Parse(context.Request["productid"]);
            int amount = int.Parse(context.Request["amount"]);
            if (amount < 1) amount = 1;
            if(!ProductService.IsProductEnabled(productId))
            {
                context.Response.Write("fail");
                return;
            }
            
            IList<EvaluatedCustomOptions> listOptions = null;
            string selectedOptions = HttpUtility.UrlDecode(context.Request["AttributesXml"]);
            try 
            {
                listOptions = CustomOptionsService.DeserializeFromXml(selectedOptions);
            }
            catch(Exception)
            {
                listOptions = null;
            }

            if (CustomOptionsService.DoesProductHaveRequiredCustomOptions(productId) && listOptions == null)
            {
                context.Response.Write("redirect");
                return;
            }
            
            ShoppingCartService.AddShoppingCartItem(new ShoppingCartItem
                                                        {
                                                            EntityId = productId,
                                                            Amount = amount,
                                                            ShoppingCartType = ShoppingCartType.ShoppingCart,
                                                            AttributesXml = listOptions != null ? selectedOptions : string.Empty,
                                                            ItemType = EnumItemType.Product
                                                        });
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
