<%@ WebHandler Language="C#" Class="FileDownloader" %>

using System.IO;
using System.Web;
using AdvantShop.Helpers;

public class FileDownloader : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        var filePath = context.Request["file"];
        if (filePath == null || !filePath.Contains("Export/") && !filePath.Contains("price_temp"))
        {
            context.Response.End();
            return;
        }
        if (!File.Exists(context.Server.MapPath(filePath)))
        {
            context.Response.End();
            return;
        }
        var fileName = filePath.Substring(filePath.LastIndexOf("/") + 1, filePath.Length - filePath.LastIndexOf("/") - 1);
        CommonHelper.WriteResponseFile(context.Server.MapPath(filePath), fileName);
    }

    public bool IsReusable
    {
        get
        {
            return true;
        }
    }
}