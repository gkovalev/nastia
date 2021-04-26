<%@ WebHandler Language="C#" Class="DeleteReview" %>

using System;
using System.Web;
using System.Web.SessionState;
using AdvantShop.CMS;
using AdvantShop.Customers;
using Newtonsoft.Json;

public class DeleteReview : IHttpHandler, IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        var _customer = CustomerSession.CurrentCustomer;
        if (_customer.CustomerRole == Role.Moderator)
        {
            var action = RoleActionService.GetCustomerRoleActionByKeyAndCustomerId(_customer.Id, RoleActionKey.DisplayComments.ToString());
            if (action != null && !action.Enabled)
            {
                ReturnValue(context, false);
                return;
            }
        }
        else if (_customer.CustomerRole != Role.Administrator)
        {
            ReturnValue(context, false);
            return;
        }
        

        int entityid;
        if (!Int32.TryParse(context.Request["entityid"], out entityid))
        {
            ReturnValue(context, false);
            return;
        }

        ReviewService.DeleteReview(entityid);
        
        ReturnValue(context, true);
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