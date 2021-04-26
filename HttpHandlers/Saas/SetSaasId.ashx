<%@ WebHandler Language="C#" Class="SetSaasId" %>

using System.Web;
using AdvantShop;
using AdvantShop.Configuration;
using SaasWebService;

public class SetSaasId : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        if (context.Request["id"].IsNullOrEmpty())
        {
            context.Response.Write("error: empty id");
            return;
        }

        if (context.Request["secretkey"].IsNullOrEmpty())
        {
            context.Response.Write("error: empty secretkey");
            return;
        }

        if (context.Request["secretkey"].Md5() != "ADDFE840BC6708708C4023B0FAA7780B")
        {
            context.Response.Write("error: wrong secretkey");
            return;
        }
        
        var client = new SaasWebServiceSoapClient();
        var newSaasData = client.GetSaasData(context.Request["id"]);
        if (newSaasData != null && newSaasData.IsValid)
        {
            SettingsGeneral.CurrentSaasId = context.Request["id"];
            context.Response.Write("success");
        }
        else
        {
            context.Response.Write("error: wrong id");
        }

    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}