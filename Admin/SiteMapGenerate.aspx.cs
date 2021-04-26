using System;
using System.IO;
using System.Text;
using AdvantShop.Configuration;
using AdvantShop.ExportImport;
using Resources;
//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.itmcompany.ru
//-------------------------------------------------

partial class Admin_SiteMapping : System.Web.UI.Page
{
    // Leave empty if you don't need subfolders
    private const string StrTargetFolder = "";
    private readonly string _strFileName = "SiteMap.html".ToLower();
    private string _strPhysicalFilePath;
    private string _prefUrl;
    private string _strPhysicalTargetFolder;

    protected override void InitializeCulture()
    {
        AdvantShop.Localization.Culture.InitializeCulture();
    }

    private void MsgErr(bool clean)
    {
        if (clean)
        {
            lblError.Visible = false;
            lblError.Text = "";
        }
        else
        {
            lblError.Visible = false;
        }

    }
    private void MsgErr(string messageText)
    {
        lblError.Visible = true;
        lblError.Text = "<br/>" + messageText;

    }
    public Admin_SiteMapping()
    {
        Init += Page_Init;
        Load += Page_Load;
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        _strPhysicalTargetFolder = Server.MapPath("~/" + StrTargetFolder);
        _strPhysicalFilePath = _strPhysicalTargetFolder + _strFileName;
    }

    public string ShowStrLinkToSiteMapFile()
    {
        return SettingsMain.SiteUrl + "/" + StrTargetFolder + _strFileName;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        _prefUrl = SettingsMain.SiteUrl;
        _prefUrl = _prefUrl.Contains("http://") ? _prefUrl : "http://" + _prefUrl;
        Page.Title = _prefUrl + @" - " + Resource.Admin_SiteMapGenerate_Header;
        RefreshFileInfo();
    }

    private void RefreshFileInfo()
    {
        lastMod.Text = File.Exists(_strPhysicalFilePath) ? AdvantShop.Localization.Culture.ConvertDate((new FileInfo(_strPhysicalFilePath).LastWriteTime)) : @"---";
    }

    protected void createMap_Click(object sender, EventArgs e)
    {
        try
        {
            var exportHtmlMap = new ExportHtmlMap(_strPhysicalFilePath, SettingsMain.SiteUrl + "/", Encoding.UTF8);
            exportHtmlMap.Create();
            // Update label info
            RefreshFileInfo();
            //--------------------------------------------------------------------------------
        }
        catch (Exception ex)
        {
            MsgErr(ex.Message + " at SiteMapGenerate");
            AdvantShop.Diagnostics.Debug.LogError(ex);
        }
    }
}
