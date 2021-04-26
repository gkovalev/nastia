//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.IO;
using System.Web;
using Advantshop_Tools;

public partial class Tools_core_IsCustom : System.Web.UI.Page
{
    private static readonly string ShopCodeMaskFile = HttpContext.Current.Server.MapPath("~/App_Data/shopCodeMaskFile.txt");
    private static readonly string ShopBaseMaskFile = HttpContext.Current.Server.MapPath("~/App_Data/shopBaseMaskFile.txt");

    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (File.Exists(ShopCodeMaskFile))
        {
            lnkFileCode.Text = @"code mask (" + new FileInfo(ShopCodeMaskFile).LastWriteTime + @")";
        }

        if (File.Exists(ShopBaseMaskFile))
        {
            lnkFileSql.Text = @"base mask (" + new FileInfo(ShopBaseMaskFile).LastWriteTime + @")";
        }
    }
    
    protected void btnCompareCode_OnClick(object sender, EventArgs e)
    {
        var report = UpdaterService.CompareCodeVersions();
        ltrlReport.Text = !string.IsNullOrEmpty(report) ? report : "Versions of the code are the same";
    }

    protected void btnCompareBase_OnClick(object sender, EventArgs e)
    {
        var report = UpdaterService.CompareBaseVersions();
        ltrlReport.Text = !string.IsNullOrEmpty(report) ? report : "Versions of the base are the same";
    }
    
    protected void lnkFileSql_Click(object sender, EventArgs e)
    {
        UpdaterService.CreateBaseMaskFile();
        Page.Response.Clear();
        Page.Response.AppendHeader("content-disposition", "attachment; filename=\"adv_sql_mask.sql\"");
        Page.Response.TransmitFile(ShopBaseMaskFile);
        Page.Response.Flush();
        Page.Response.End();
    }

    protected void lnkFileCode_Click(object sender, EventArgs e)
    {
        UpdaterService.CreateCodeMaskFile();
        Page.Response.Clear();
        Page.Response.AppendHeader("content-disposition", "attachment; filename=\"adv_code_mask.txt\"");
        Page.Response.TransmitFile(ShopCodeMaskFile);
        Page.Response.Flush();
        Page.Response.End();
    }
}