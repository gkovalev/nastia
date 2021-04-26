<%@ WebHandler Language="C#" Class="ImportPropertiesElbuzCsv_Data" %>

using System;
using System.Web;

using AdvantShop.Core;

public class ImportPropertiesElbuzCsv_Data : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
       context.Response.Cache.SetLastModified(DateTime.UtcNow);
        
        context.Response.ContentType = "application/json";
        context.Response.Write("{");
        
        context.Response.Write("\"Total\":");
        context.Response.Write(ImportPropertiesElbuzCsvStatistic.TotalRowExcel);
        context.Response.Write(",");
        context.Response.Write("\"Processed\":");
        context.Response.Write(ImportPropertiesElbuzCsvStatistic.RowPosition);
        context.Response.Write(",");
        context.Response.Write("\"Update\":");
        context.Response.Write(ImportPropertiesElbuzCsvStatistic.TotalUpdateRow);
        context.Response.Write(",");
        context.Response.Write("\"Add\":");
        context.Response.Write(ImportPropertiesElbuzCsvStatistic.TotalAddRow);
        context.Response.Write(",");
        context.Response.Write("\"Error\":");
        context.Response.Write(ImportPropertiesElbuzCsvStatistic.TotalErrorRow);
        
        context.Response.Write("}");
    }

    public bool IsReusable
    {
        get { return false; }
    }
}