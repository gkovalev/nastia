<%@ WebHandler Language="C#" Class="DebugJS" %>

using System.Web;
using AdvantShop;

public class DebugJS : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        
        if (context.Request["message"].IsNotEmpty())
        {
            AdvantShop.Diagnostics.Debug.LogError(HttpUtility.HtmlEncode(context.Request["message"]), false);  
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}