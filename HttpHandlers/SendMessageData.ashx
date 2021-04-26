<%@ WebHandler Language="C#" Class="SendMessageData" %>

using System;
using System.Web;
using AdvantShop.Statistic;

class SendMessageData : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.Cache.SetLastModified(DateTime.UtcNow);
        
        context.Response.ContentType = "application/json";
        context.Response.Write("{");
        context.Response.Write("\"Total\":" + SendMessageStatistic.TotalEmails + ", ");
        context.Response.Write("\"Processed\":" + SendMessageStatistic.SendEmails + ", ");
        context.Response.Write("\"IsAbort\":\"" + (SendMessageStatistic.IsAbort ? 1 : 0) + "\"");
        context.Response.Write("}");
    }

    public bool IsReusable
    {
        get { return true;}
    }
}