//'--------------------------------------------------
//' Project: AdvantShop.NET
//' Web site: http:\\www.itmcompany.ru
//'--------------------------------------------------
using System;
using System.IO;
using System.Web.UI;
using AdvantShop.Configuration;
using AdvantShop.ExportImport;
using Resources;

partial class Admin_SiteMappingXML : Page
{
    private const string StrInitFileVirtualPath = "../";
    private const string StrInitFileName = "sitemap.xml";
    private string _strPhysicalFilePath = string.Empty;
    private string _strPhysicalTargetFolder = string.Empty;

    public Admin_SiteMappingXML()
    {
        Load += Page_Load;
        Init += Page_Init;
    }

    protected override void InitializeCulture()
    {
        AdvantShop.Localization.Culture.InitializeCulture();
    }

    private void MsgErr(bool clean)
    {
        if (clean)
        {
            lblErr.Visible = false;
            lblErr.Text = string.Empty;
        }
        else
        {
            lblErr.Visible = false;
        }
    }

    private void MsgErr(string messageText)
    {
        lblErr.Visible = true;
        lblErr.Text = @"<br/>" + messageText;

    }

    protected void Page_Init(object sender, EventArgs e)
    {
        _strPhysicalTargetFolder = Server.MapPath(StrInitFileVirtualPath);
        _strPhysicalFilePath = _strPhysicalTargetFolder + StrInitFileName;
    }

    public string ShowStrLinkToSiteMapFile()
    {
        return SettingsMain.SiteUrl + "/" + StrInitFileName;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Page.Title = string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_SiteMapGenerate_Header);
            RefreshFileDateInfo();
        }
        catch (Exception ex)
        {
            MsgErr(ex.Message + " at load");
            AdvantShop.Diagnostics.Debug.LogError(ex);
        }
    }

    private void RefreshFileDateInfo()
    {
        lastMod.Text = File.Exists(_strPhysicalFilePath) ? AdvantShop.Localization.Culture.ConvertDate((new FileInfo(_strPhysicalFilePath)).LastWriteTime) : @"---";
    }

    protected void btnCreateMap_Click(object sender, EventArgs e)
    {
        try
        {
            MsgErr(true);

            // Directory
            if (!Directory.Exists(_strPhysicalTargetFolder))
            {
                Directory.CreateDirectory(_strPhysicalTargetFolder);
            }

            // Old files
            if (File.Exists(_strPhysicalTargetFolder))
            {
                File.Delete(_strPhysicalTargetFolder);
            }

            // Save new file 
            //SiteMapService.GenerateSiteMap(_strPhysicalFilePath, StrInitFileVirtualPath);

            var temp = new ExportXmlMap(_strPhysicalFilePath, StrInitFileVirtualPath);
            temp.Create();

            // Refresh label info
            RefreshFileDateInfo();

        }
        catch (Exception ex)
        {
            MsgErr(ex.Message + " at btnCreateMap_Click");
            AdvantShop.Diagnostics.Debug.LogError(ex);
        }
    }
}