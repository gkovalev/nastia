<%@ WebHandler Language="C#" Class="PaymentNotification" %>

using System.Web;
using System.Web.SessionState;
using AdvantShop.Payment;

public class PaymentNotification : IHttpHandler, IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        var method = PaymentService.GetPaymentMethod(GetMethodID(context));
        if (method != null && (method.NotificationType & NotificationType.Handler) == NotificationType.Handler)
            method.ProcessResponse(context);
    }

    private static int GetMethodID(HttpContext context)
    {
        int id;
        var stringID = context.Request["PaymentMethodID"];
        return !string.IsNullOrEmpty(stringID)
               && int.TryParse(stringID, out id)
            ? id
            : 0;
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}