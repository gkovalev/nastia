<%@ WebHandler Language="C#" Class="GetCompareHtml" %>

using System.Linq;
using System.Web;
using System.Web.SessionState;
using AdvantShop.Orders;
using Newtonsoft.Json;

public class GetCompareHtml : IHttpHandler, IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        var compareCount = ShoppingCartService.CurrentCompare.Count();
                      
        context.Response.ContentType = "application/json";
        context.Response.Write(JsonConvert.SerializeObject(compareCount));
    }

    public bool IsReusable
    {
        get { return true;}
    }
}