<%@ WebHandler Language="C#" Class="ExportData" %>

using System;
using System.Web;
using AdvantShop.Statistic;
using Newtonsoft.Json;

class ExportData : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.Cache.SetLastModified(DateTime.UtcNow);
        context.Response.ContentType = "application/json";
        context.Response.Write(JsonConvert.SerializeObject(ExportStatistic.Data));
        context.Response.End(); 
    }

    public bool IsReusable
    {
        get { return true; }
    }
}