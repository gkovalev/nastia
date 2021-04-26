<%@ WebHandler Language="C#" Class="GetCompareHtml" %>

using System.Linq;
using System.Web;
using System.Web.SessionState;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Orders;
using Newtonsoft.Json;

public class GetCompareHtml : IHttpHandler, IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        var compare = from item in ShoppingCartService.CurrentCompare
                      select new
                                 {
                                     item.Product.ID,
                                     item.Product.Name,
                                     Link = UrlService.GetLink(ParamType.Product, item.Product.UrlPath, item.Product.ID)
                                 };
        
        context.Response.ContentType = "application/json";
        context.Response.Write(JsonConvert.SerializeObject(compare));
    }

    public bool IsReusable
    {
        get { return true;}
    }
}