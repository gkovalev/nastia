<%@ WebHandler Language="C#" Class="ExportRitmZ" %>

using System;
using System.Collections.Specialized;
using System.Web;
using AdvantShop.ExportImport;

public class ExportRitmZ : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        DateTime? start = null;
        DateTime? end = null;
        NameValueCollection nvc = context.Request.Form;

        if (!string.IsNullOrEmpty(nvc["b_date"]))
        {
            try
            {
                start = Convert.ToDateTime(nvc["b_date"]);
            }
            catch
            {
                start = null;
            }
        }

        if (!string.IsNullOrEmpty(nvc["e_date"]))
        {
            try
            {
                end = Convert.ToDateTime(nvc["e_date"]);
            }
            catch
            {
                end = null;
            }
        }

        context.Response.Charset = "utf-8";
        context.Response.ContentType = "text/xml;";
        ExportOrderRitmZ.Export(start, end, context.Server.MapPath(ExportOrderRitmZ.ExporDir + "/" + ExportOrderRitmZ.ExportFile));
        ExportOrderRitmZ.WriteToResponce(context.Response, context.Server.MapPath(ExportOrderRitmZ.ExporDir + "/" + ExportOrderRitmZ.ExportFile));
        //context.Response.Write("Hello World");
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}