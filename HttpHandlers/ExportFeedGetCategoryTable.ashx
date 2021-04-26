<%@ WebHandler Language="C#" Class="ExportFeedGetCategoryTable" %>

using System;
using System.Web;
using System.Web.SessionState;
using AdvantShop.Catalog;
using AdvantShop.ExportImport;
using Newtonsoft.Json;

public class ExportFeedGetCategoryTable : IHttpHandler, IReadOnlySessionState
{
    public void ProcessRequest(HttpContext context)
    {
        // removed from patch 3.0.0.12
        
        //int catId = Convert.ToInt32(context.Request["catId"]);
        //int depth = Convert.ToInt32(context.Request["depth"]);
        //bool recurse = Boolean.Parse(context.Request["recurse"]);
        //var cat = CategoryService.GetCategory(catId);

        //context.Response.ContentType = "application/json";
        //context.Response.Write(JsonConvert.SerializeObject(ExportFeed.ProcessCategoryRow(cat, depth, recurse)));
        //context.Response.End();
    }

    public bool IsReusable
    {
        get { return false; }
    }
}