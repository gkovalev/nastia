using System;
using System.Linq;
using System.Web.UI;
using AdvantShop.Configuration;
using AdvantShop.Controls;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Modules;
using AdvantShop.Modules.Interfaces;
using AdvantShop.SEO;

public partial class OrderConfirmationModule : AdvantShopPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var ocModule = AttachedModules.GetModules(AttachedModules.EModuleType.OrderConfirmation).FirstOrDefault();
        IOrderConfirmation classInstance;
        if (ocModule != null)
        {
            classInstance = (IOrderConfirmation) Activator.CreateInstance(ocModule, null);
            if (!classInstance.IsActive || !classInstance.CheckAlive())
            {
                Redirect("orderconfirmation.aspx", true);
                return;
            }
        }
        else
        {
            Redirect("orderconfirmation.aspx", true);
            return;
        }

        SetMeta(new MetaInfo(string.Format("{0} - {1}", classInstance.PageName, SettingsMain.ShopName)), string.Empty);
        liPageHead.Text = classInstance.PageName;
        Control c =
            (this).LoadControl(
                UrlService.GetAbsoluteLink(string.Format("/Modules/{0}/{1}", classInstance.ModuleStringId,
                                                         classInstance.FileUserControlOrderConfirmation)));
        if (c != null)
            pnlContent.Controls.Add(c);
    }
}