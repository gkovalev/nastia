//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Web;
using System.IO;

public partial class Tools_core_Backuper : System.Web.UI.Page
{
    private readonly string _codeFile = "App_Data\\dak_code.zip";
    private readonly string _sqlFile = HttpContext.Current.Server.MapPath("~/App_Data/bak.sql");

    protected void Page_Load(object sender, EventArgs e)
    {
        if (File.Exists(HttpContext.Current.Server.MapPath("~/" + _codeFile)))
        {
            lnkFileCode.Text = @"source archiv (" + new FileInfo(HttpContext.Current.Server.MapPath("~/" + _codeFile)).LastWriteTime + @")";
        }

        if (File.Exists(_sqlFile))
        {
            lnkFileSql.Text = @"base backup (" + new FileInfo(_sqlFile).LastWriteTime + @")";
        }
    }

    protected void lnkFileSql_Click(object sender, EventArgs e)
    {
        Page.Response.Clear();
        Page.Response.AppendHeader("content-disposition", "attachment; filename=\"adv_sql_backup.sql\"");
        Page.Response.TransmitFile(_sqlFile);
        Page.Response.Flush();
        Page.Response.End();
    }

    protected void lnkFileCode_Click(object sender, EventArgs e)
    {
        Page.Response.Clear();
        Page.Response.AppendHeader("content-disposition", "attachment; filename=\"adv_code_backup.sql\"");
        Page.Response.TransmitFile(HttpContext.Current.Server.MapPath("~/" + _codeFile));
        Page.Response.Flush();
        Page.Response.End();
    }

    protected void btnBackupSql_Click(object sender, EventArgs e)
    {
        CreateBackupSql();
    }

    protected void btnBackupCode_Click(object sender, EventArgs e)
    {
        CreateBackUpFile();
    }

    protected void CreateBackUpFile()
    {
        Advantshop_Tools.UpdaterService.CreateCodeBackup();
    }

    protected void CreateBackupSql()
    {
        Advantshop_Tools.UpdaterService.CreateBaseBackup();
    }
}