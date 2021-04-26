<%@ WebHandler Language="C#" Class="ProductPhotoData" %>

using System;
using System.Web;
using AdvantShop.Tools;

class ProductPhotoData : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.Cache.SetLastModified(DateTime.UtcNow);
        
        context.Response.ContentType = "application/json";
        context.Response.Write("{");
        
        context.Response.Write("\"Total\":");
        context.Response.Write(ResizePhotoStatistic.Count);
        context.Response.Write(",");
        context.Response.Write("\"Processed\":");
        context.Response.Write(ResizePhotoStatistic.Index);
       
        context.Response.Write("}");
        context.Response.End();
    }

    public bool IsReusable
    {
        get { return true;}
    }
}