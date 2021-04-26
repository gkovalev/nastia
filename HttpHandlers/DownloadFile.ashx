<%@ WebHandler Language="C#" Class="DownloadFile" %>

using System;
using System.Web;
using AdvantShop.Helpers;
using AdvantShop.Configuration;

public class DownloadFile : IHttpHandler {
    
    public void ProcessRequest (HttpContext context)
    {
        var file = context.Request["file"];
        if (string.IsNullOrEmpty(file))
            return;
        if (!System.IO.File.Exists(SettingsGeneral.AbsolutePath + file))
            return;
        CommonHelper.WriteResponseFile(SettingsGeneral.AbsolutePath + file, file);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}