<%@ WebHandler Language="C#" Class="DownloadLog" %>

using System.Web;
using AdvantShop.Helpers;

public class DownloadLog : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        //AdvantShop.Helpers.CommonHelper.WriteResponseTxt(AdvantShop.Statistic.ImportStatistic.FileLog, AdvantShop.Statistic.ImportStatistic.VirtualFileLogPath);
        if (!System.IO.File.Exists(AdvantShop.Statistic.ImportStatistic.FileLog))
        {
            FileHelpers.CreateFile(AdvantShop.Statistic.ImportStatistic.FileLog);
        }
        CommonHelper.WriteResponseFile(AdvantShop.Statistic.ImportStatistic.FileLog, AdvantShop.Statistic.ImportStatistic.VirtualFileLogPath);
        //    HttpContext.Current.Response.End();
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}