//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop.Configuration;
using AdvantShop.Permission;

public partial class LicCheck : System.Web.UI.Page
{
    protected override void InitializeCulture()
    {
        AdvantShop.Localization.Culture.InitializeCulture();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        hlGo.Visible = false;
    }
    protected void btnCheck_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtKey.Text))
            return;
        var res = PermissionAccsess.ActiveLic(txtKey.Text, SettingsMain.SiteUrl, SettingsMain.ShopName, SettingsGeneral.SiteVersion);
        if (res)
        {
            SettingsLic.ActiveLic = true;
            hlGo.Visible = true;
            SettingsLic.LicKey = txtKey.Text;
        }
        else
        {
            SettingsLic.ActiveLic = false;
            lblMsg.Text = @"key is wrong";
        }
    }
}