<%@ WebHandler Language="C#" Class="ImportElbuzCsv_Data" %>

using System;
using System.Web;

using AdvantShop.Core;

public class ImportElbuzCsv_Data : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
       context.Response.Cache.SetLastModified(DateTime.UtcNow);
        
        context.Response.ContentType = "application/json";
        context.Response.Write("{");
        
        context.Response.Write("\"Total\":");
        context.Response.Write(ImportElbuzCsvStatistic.TotalRowExcel);
        context.Response.Write(",");
        context.Response.Write("\"Processed\":");
        context.Response.Write(ImportElbuzCsvStatistic.RowPosition);
        context.Response.Write(",");
        context.Response.Write("\"Update\":");
        context.Response.Write(ImportElbuzCsvStatistic.TotalUpdateRow);
        context.Response.Write(",");
        context.Response.Write("\"Add\":");
        context.Response.Write(ImportElbuzCsvStatistic.TotalAddRow);
        context.Response.Write(",");
        context.Response.Write("\"Error\":");
        context.Response.Write(ImportElbuzCsvStatistic.TotalErrorRow);
        
        context.Response.Write("}");
    }

    public bool IsReusable
    {
        get { return false; }
    }
}