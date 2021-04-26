<%@ WebHandler Language="C#" Class="CallbackHandler" %>

using System.Web;
using System.Web.SessionState;
using AdvantShop.Modules;

public class CallbackHandler : IHttpHandler, IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        var valid = true;
        valid &= !string.IsNullOrEmpty(context.Request["name"]);
        valid &= !string.IsNullOrEmpty(context.Request["phone"]);
        
        if (!valid)
        {
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(false));
            return;
        }

        var callbackCustomer = new CallbackCustomer
            {
                Name = context.Request["name"],
                Phone = context.Request["phone"],
                Comment = context.Request["comment"]
            };

        CallbackRepository.AddCallbackCustomer(callbackCustomer);
        CallbackRepository.SendEmail(callbackCustomer);

        context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(true));
        
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}
