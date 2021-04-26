using System;
using System.Web.UI;
using AdvantShop.Configuration;
using AdvantShop.Statistic;
using Resources;

public partial class Admin_UsersOnline : System.Web.UI.Page
{
    protected override void InitializeCulture()
    {
        AdvantShop.Localization.Culture.InitializeCulture();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_OnLineUsers_SubHeader);
        grid.DataSource = ClientInfoService.GetAllInfo();
        grid.DataBind();
    }

    public string RenderOsIcon(string strOs)
    {
        string res = string.Empty;

        const string strFormat = @"<table><tr><td><img src='images/osIcon/{0}' alt ='{1}' /></td><td>{1}</td></tr></table>";

        if (strOs.Contains("Windows"))
        {
            res = string.Format(strFormat, "win.png", strOs);
            if (strOs.Contains("Windows 98")) { res = string.Format(strFormat, "win98.png", strOs); }
            if (strOs.Contains("Windows XP")) { res = string.Format(strFormat, "winxp.png", strOs); }
            if (strOs.Contains("Windows Vista")) { res = string.Format(strFormat, "winvista.png", strOs); }
            if (strOs.Contains("Windows 7")) { res = string.Format(strFormat, "win7.png", strOs); }
            if (strOs.Contains("Windows Server 2003")) { res = string.Format(strFormat, "win2003.png", strOs); }
        }

        if (strOs.Contains("MacOS"))
        {
            res = string.Format(strFormat, "mac.png", strOs);
        }
        if (strOs.Contains("Linux"))
        {
            res = string.Format(strFormat, "linux.png", strOs);
        }
        if (strOs.Contains("UNIX"))
        {
            res = "UNIX";
        }
        if (strOs.Contains("WebTV"))
        {
            res = "WebTV";
        }
        if (strOs.Contains("OpenBSD"))
        {
            res = "OpenBSD";
        }
        if (strOs.Contains("SunOS"))
        {
            res = "SunOS";
        }
        if (strOs.Contains("QNX"))
        {
            res = "QNX";
        }
        if (strOs.Contains("BeOS"))
        {
            res = "BeOS";
        }
        if (strOs.Contains("OS/2"))
        {
            res = "OS/2";
        }
        if (strOs.Contains("Unknown"))
        {
            res = "Unknown";
        }

        return res;
    }

    public string RenderBrowserIcon(string browser)
    {
        string res = string.Empty;
        const string strFormat = @"<table><tr><td><img src='images/browserIcon/{0}' alt ='{1}' /></td><td>{1}</td></tr></table>";

        if (browser.Contains("Internet Explorer"))
        {
            res = string.Format(strFormat, "ie.png", browser);
        }
        if (browser.Contains("Firefox"))
        {
            res = string.Format(strFormat, "firefox.png", browser);
        }
        if (browser.Contains("Chrome"))
        {
            res = string.Format(strFormat, "chrome.png", browser);
        }
        if (browser.Contains("Opera"))
        {
            res = string.Format(strFormat, "opera.png", browser);
        }
        if (browser.Contains("Safari"))
        {
            res = string.Format(strFormat, "safari.png", browser);
        }
        return res;
    }

}