<%@ WebHandler Language="C#" Class="UploadZipPhoto" %>

using System;
using System.Web;
using AdvantShop;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using Resources;

public class UploadZipPhoto : IHttpHandler
{
    static void Msg(HttpContext context, string msg)
    {
        context.Response.Write(msg);
    }

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/html";

        if (context.Request.Files.Count < 1 || context.Request.Files[0].FileName.IsNullOrEmpty())
        {
            context.Response.Write(Resource.Admin_ImportCsv_NoFile);
            return;
        }

        HttpPostedFile pf = context.Request.Files[0];

        if (!FileHelpers.CheckArchiveExtension(pf.FileName))
        {
            context.Response.Write(Resource.Admin_ImportCsv_WrongFileExtention);
            return;
        }

        FileHelpers.CreateDirectory(FoldersHelper.GetPathAbsolut(FolderType.ImageTemp));
        var path = FoldersHelper.GetPathAbsolut(FolderType.ImageTemp, pf.FileName);
        try
        {
            FileHelpers.DeleteFilesFromPath(FoldersHelper.GetPathAbsolut(FolderType.ImageTemp));
            pf.SaveAs(path);
            var res = FileHelpers.UnZipFile(path);
            FileHelpers.DeleteFile(path);
            if (!res)
            {
                context.Response.Write(Resource.Admin_ImportCsv_ErrorAtUnZip);
                return;
            }
            context.Response.Write("OK");
        }
        catch (Exception ex)
        {
            FileHelpers.DeleteFile(path);
            Debug.LogError(ex);
            context.Response.Write(Resource.Admin_ImportCsv_ErrorAtUploadFile);
        }
    }

    public bool IsReusable
    {
        get
        {
            return true;
        }
    }
}