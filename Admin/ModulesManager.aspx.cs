//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using AdvantShop.Configuration;
using AdvantShop.Modules;
using AdvantShop;
using AdvantShop.Core.UrlRewriter;
using Resources;

public partial class Admin_ModulesManager : System.Web.UI.Page
{
    protected override void InitializeCulture()
    {
        AdvantShop.Localization.Culture.InitializeCulture();
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        Page.Title = string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_ModuleManager_Header);

        if (!string.IsNullOrEmpty(Request["installModule"]))
        {
            ModulesService.InstallModule(Convert.ToString(Request["installModule"].ToLower()), Request["version"]);
        }

        LoadData();
    }

    protected void lvModules_ItemCommand(object source, ListViewCommandEventArgs e)
    {

        if (e.CommandName == "InstallLastVersion")
        {
            var moduleVersion = ((HiddenField)e.Item.FindControl("hfLastVersion")).Value;
            var moduleIdOnRemoteServer = ((HiddenField)e.Item.FindControl("hfId")).Value;

            var message = ModulesService.GetModuleArchiveFromRemoteServer(moduleIdOnRemoteServer);

            if (message.IsNullOrEmpty())
            {
                //ModulesService.InstallModule(Convert.ToString(e.CommandArgument));
                HttpRuntime.UnloadAppDomain();

                Context.ApplicationInstance.CompleteRequest();
                Response.Redirect(
                    UrlService.GetAdminAbsoluteLink("modulesmanager.aspx?installModule=" + e.CommandArgument + "&version=" +
                                                    moduleVersion), false);
            }
            else
            {
                //вывести message
            }
        }
        if (e.CommandName == "Install")
        {
            ModulesService.InstallModule(Convert.ToString(e.CommandArgument));
        }
        if (e.CommandName == "Uninstall")
        {
            ModulesService.UninstallModule(Convert.ToString(e.CommandArgument));
            HttpRuntime.UnloadAppDomain();
            Response.Redirect(Request.Url.AbsoluteUri);
        }
    }

    protected void LoadData()
    {
        var modulesBox = ModulesService.GetModules();
        //if (modulesBox.Message.IsNullOrEmpty())
        {
            lvModules.DataSource = modulesBox.Items.OrderBy(t => t.Name);
            lvModules.DataBind();
        }
    }
}