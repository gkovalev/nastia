using System;
using System.Linq;
using System.Web.UI;
using AdvantShop;
using AdvantShop.Configuration;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Modules;
using AdvantShop.Modules.Interfaces;

public partial class Admin_Module : Page
{
    protected int CurrentControlIndex
    {
        get
        {
            var intval = 0;
            int.TryParse(Request["currentcontrolindex"], out intval);
            return intval;
        }
    }

    protected override void InitializeCulture()
    {
        AdvantShop.Localization.Culture.InitializeCulture();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["module"].IsNullOrEmpty())
        {
            Response.Redirect(UrlService.GetAdminAbsoluteLink(""));
        }

        var module = AttachedModules.GetModules(AttachedModules.EModuleType.All).FirstOrDefault(
            item => ((IModule)Activator.CreateInstance(item, null)).ModuleStringId == Request["module"]);

        if (module == null)
        {
            Response.Redirect(UrlService.GetAdminAbsoluteLink(""));
        }

        var moduleObject = (IModule)Activator.CreateInstance(module, null);
        lblHead.Text = moduleObject.ModuleName;

        if (moduleObject.ModuleControls != null)
        {
            rptTabs.DataSource = moduleObject.ModuleControls;
            rptTabs.DataBind();

            int currentControlIndex = CurrentControlIndex;

            if (currentControlIndex < 0 || currentControlIndex > (moduleObject.ModuleControls.Count - 1))
                currentControlIndex = 0;

            //foreach (var controlName in moduleObject.ModuleControls)
            //{
            Control c = (this).LoadControl(UrlService.GetAbsoluteLink(string.Format("/Modules/{0}/{1}", moduleObject.ModuleStringId, moduleObject.ModuleControls[currentControlIndex].File)));
            if (c != null)
            {
                pnlBody.Controls.Add(c);
            }

            Page.Title = string.Format("{0} - {1} - {2}", SettingsMain.ShopName, moduleObject.ModuleName, moduleObject.ModuleControls[currentControlIndex].NameTab);

            //}
        }
        else
        {
            lblInfo.Text = Resources.Resource.Admin_Module_NotConfigure;
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {

    }
}