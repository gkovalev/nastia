<%@ WebHandler Language="C#" Class="ExportRitmZProducts" %>

using System.Web;
using AdvantShop.ExportImport;

public class ExportRitmZProducts : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.Charset = "utf-8";
        context.Response.ContentType = "text/xml;";
        ExportProductsRitmZ.Export();
        ExportProductsRitmZ.WriteToResponce(context.Response, context.Server.MapPath(ExportProductsRitmZ.ExporDir + "/" + ExportProductsRitmZ.ExportFile));
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}