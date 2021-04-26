<%@ WebHandler Language="C#" Class="ImportData" %>

using System;
using System.Web;
using AdvantShop.Statistic;
using Newtonsoft.Json;

public class ImportData : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.Cache.SetLastModified(DateTime.UtcNow);

        context.Response.ContentType = "application/json";
        context.Response.Write(JsonConvert.SerializeObject(ImportStatistic.Data));
        context.Response.End(); // ?
    }

    public bool IsReusable
    {
        get { return false; }
    }
}