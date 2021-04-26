<%@ WebHandler Language="C#" Class="ChangePaymentMethod" %>

using System.Web;
using System.Web.SessionState;
using Newtonsoft.Json;
using AdvantShop;

public class ChangePaymentMethod : IHttpHandler, IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        if (!AdvantShop.Customers.CustomerSession.CurrentCustomer.RegistredUser)
        {
            context.Response.Write(JsonConvert.SerializeObject(string.Empty));
            return;
        }

        if (context.Request["paymentId"].IsNullOrEmpty() && context.Request["paymentName"].IsNullOrEmpty() && System.Convert.ToString(context.Request["orderNumber"]).IsNullOrEmpty())
        {
            context.Response.Write(JsonConvert.SerializeObject(string.Empty));
            return;
        }

        var order = AdvantShop.Orders.OrderService.GetOrderByNumber(System.Convert.ToString(context.Request["orderNumber"]));
        if (order == null)
        {
            context.Response.Write(JsonConvert.SerializeObject(string.Empty));
            return;
        }
        context.Response.ContentType = "application/json";
        
        if (AdvantShop.Payment.PaymentService.GetPaymentMethod(System.Convert.ToInt32(context.Request["paymentId"])) == null)
        {
            context.Response.Write(JsonConvert.SerializeObject(string.Empty));
            return;
        }
        order.PaymentMethodId = System.Convert.ToInt32(context.Request["paymentId"]);
        order.ArchivedPaymentName = System.Convert.ToString(context.Request["paymentName"]);
        AdvantShop.Orders.OrderService.UpdateOrderMain(order);

        AdvantShop.Modules.ModulesRenderer.OrderUpdated(order.OrderID);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}
