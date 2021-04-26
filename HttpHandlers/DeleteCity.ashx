<%@ WebHandler Language="C#" Class="DeleteCity" %>

using System;
using System.Web;

public class DeleteCity : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        try
        {
            int cityID = Convert.ToInt32(context.Request["cityID"]);
            int methodId = Convert.ToInt32(context.Request["methodId"]);
            string subject = context.Request["subject"];
            if (subject == "payment")
            {
                AdvantShop.ShippingPaymentGeoMaping.DeletePaymentCity(methodId, cityID);
            }
            if (subject == "shipping")
            {
                AdvantShop.ShippingPaymentGeoMaping.DeleteShippingCity(methodId, cityID);
            }

        }
        catch (Exception e)
        {
            AdvantShop.Diagnostics.Debug.LogError(e);
        }

        context.Response.ContentType = "text/plain";
        context.Response.Write("sussecce");
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}