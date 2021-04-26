//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop.Configuration;
using AdvantShop.Controls;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Customers;
using AdvantShop.Modules;
using AdvantShop.Modules.Interfaces;
using AdvantShop.SEO;
using Resources;

public partial class MyAccount : AdvantShopPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!CustomerSession.CurrentCustomer.RegistredUser)
        {
            Response.Redirect("default.aspx");
        }

        //if (!IsPostBack)
        //{
            SetMeta(new MetaInfo(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Client_MyAccount_MyAccount)),
                string.Empty);

            //int countSmsModulesAcvive = AttachedModules.GetModules(AttachedModules.EModuleType.SMS).Select(type => (IModuleSms) Activator.CreateInstance(type, null)).Count(moduleInst => moduleInst.IsActive());
            //smsnotifications.Visible = countSmsModulesAcvive > 0;
            foreach (var type in AttachedModules.GetModules(AttachedModules.EModuleType.MyAccountControls))
            {
                var mac = (IMyAccountControls)Activator.CreateInstance(type, null);
                if (mac.IsActiveModule)
                {
                    //foreach (var macontrol in mac.Controls)
                    for (int i = 0; i < mac.Controls.Count; i++)
                    {
                        Control c = (this).LoadControl(UrlService.GetAbsoluteLink(string.Format("/Modules/{0}/{1}", mac.ModuleStringId, mac.Controls[i].File)));
                        if (c != null)
                        {
                            var div = new Panel() {CssClass = "tab-content"};
                            div.Attributes.Add("data-tabs-content", "true");
                            div.Controls.Add(c);
                            tabscontents.Controls.Add(div);

                            liDopTabs.Text +=
                                string.Format("<div class=\"tab-header\" id=\"{0}{1}\" data-tabs-header=\"true\"><span class=\"tab-inside\">{2}</span><span class=\"right\"></span></div>",
                                    mac.ModuleStringId.ToLower(), i + 1, mac.Controls[i].NameTab);
                        }
                    }
                }
            }
        //}
    }
}