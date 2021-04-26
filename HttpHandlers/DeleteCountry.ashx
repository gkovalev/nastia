<%@ WebHandler Language="C#" Class="DeleteCountry" %>

using System;
using System.Web;

public class DeleteCountry : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        try
        {
            int countryId = Convert.ToInt32(context.Request["countryId"]);
            int methodId = Convert.ToInt32(context.Request["methodId"]);
            string subject = context.Request["subject"];
            if (subject == "payment")
            {
                AdvantShop.ShippingPaymentGeoMaping.DeletePaymentCountry(methodId, countryId);
            }
            if (subject == "shipping")
            {
                AdvantShop.ShippingPaymentGeoMaping.DeleteShippingCountry(methodId, countryId);
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