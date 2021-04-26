<%@ WebHandler Language="C#" Class="GetStringPrice" %>

using System;
using System.Web;
using AdvantShop;
using AdvantShop.Catalog;
using Newtonsoft.Json;

public class GetStringPrice : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        
        AdvantShop.Localization.Culture.InitializeCulture();
        context.Response.ContentType = "text/html";
        
        string price = context.Request["price"];
        
        if (price.IsNotEmpty())
        {
            context.Response.Write(CatalogService.GetStringPrice(price.TryParseDecimal(), true));
        }
        else
        {
            context.Response.Write(context.Request["price"]);
        } 
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}