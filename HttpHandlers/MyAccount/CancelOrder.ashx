<%@ WebHandler Language="C#" Class="ChangeOrderStatus" %>
using System.Web;
using System.Web.SessionState;
using Newtonsoft.Json;
using AdvantShop;

public class ChangeOrderStatus : IHttpHandler, IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        if (!AdvantShop.Customers.CustomerSession.CurrentCustomer.RegistredUser || context.Request["ordernumber"].IsNullOrEmpty())
        {
            context.Response.Write(JsonConvert.SerializeObject(string.Empty));
            return;
        }
        
        var customer = AdvantShop.Orders.OrderService.GetOrderCustomer(context.Request["ordernumber"]);
        if (customer == null || customer.CustomerID != AdvantShop.Customers.CustomerSession.CurrentCustomer.Id)
        {
            context.Response.Write(JsonConvert.SerializeObject(string.Empty));
        }
        context.Response.ContentType = "application/json";

        AdvantShop.Orders.OrderService.CancelOrder(context.Request["ordernumber"]);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}
