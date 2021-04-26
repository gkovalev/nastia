using System;
using System.Linq;
using System.Text;
using AdvantShop.CMS;
using AdvantShop.Catalog;
using AdvantShop.Core.Caching;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Customers;

//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

public partial class UserControls_MenuBottom : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!CustomerSession.CurrentCustomer.RegistredUser)
        {
            var cacheName = CacheNames.GetBottomMenuCacheObjectName();
            if (CacheManager.Contains(cacheName))
                ltbottomMenu.Text = CacheManager.Get<string>(cacheName);
            else
            {
                ltbottomMenu.Text = GetHtml();
                CacheManager.Insert<string>(cacheName, ltbottomMenu.Text);
            }
        }
        else
        {
            var cacheName = CacheNames.GetBottomMenuAuthCacheObjectName();
            if (CacheManager.Contains(cacheName))
                ltbottomMenu.Text = CacheManager.Get<string>(cacheName);
            else
            {
                ltbottomMenu.Text = GetHtml();
                CacheManager.Insert<string>(cacheName, ltbottomMenu.Text);
            }
        }
    }

    private string GetHtml()
    {
        var result = new StringBuilder();

        result.Append("<div class=\"block-footer\">");
        result.AppendFormat("<div class=\"block-title\">{0}</div>", Resources.Resource.Client_MasterPage_BottomMenu);

        result.Append("<menu class=\"block-content\">");
        foreach (var cat in CategoryService.GetChildCategoriesByCategoryId(0, false).Where(cat => cat.Enabled && cat.HirecalEnabled))
        {
            result.AppendFormat("<li class=\"menu-bottom-row\"><a href=\"{0}\" class=\"link-footer\">{1}</a></li>",
                UrlService.GetLink(ParamType.Category, cat.UrlPath, cat.CategoryId),
                cat.Name);
        }
        result.Append("</menu>");
        result.Append("</div>");


        foreach (var rootMenuItem in MenuService.GetChildMenuItemsByParentId(0, MenuService.EMenuType.Bottom, CustomerSession.CurrentCustomer.RegistredUser ? EMenuItemShowMode.Authorized : EMenuItemShowMode.NotAuthorized))
        {
            if (!rootMenuItem.Enabled)
                continue;
            result.Append("<div class=\"block-footer\">");
            result.AppendFormat("<div class=\"block-title\">{0}</div>", rootMenuItem.MenuItemName);

            result.Append("<menu class=\"block-content\">");

            foreach (var childMenuItem in MenuService.GetChildMenuItemsByParentId(rootMenuItem.MenuItemID, MenuService.EMenuType.Bottom, CustomerSession.CurrentCustomer.RegistredUser ? EMenuItemShowMode.Authorized : EMenuItemShowMode.NotAuthorized))
            {
                result.Append(RenderChildItem(childMenuItem));
            }
            result.Append("</menu>");
            result.Append("</div>");
        }
        return result.ToString();
    }

    private string RenderChildItem(AdvMenuItem childMenuItem)
    {
        var result = new StringBuilder();
        result.Append("<li class=\"menu-bottom-row\">");
        if (!string.IsNullOrEmpty(childMenuItem.MenuItemIcon))
        {
            result.AppendFormat("<img src=\"{0}\" alt=\"\" class=\"menu-bottom-icon\" />", "pictures/icons/" + childMenuItem.MenuItemIcon);
        }
        result.AppendFormat("<a href=\"{0}\" class=\"link-footer\" {2}>{1}</a>",
            //childMenuItem.MenuItemUrlType != EMenuItemUrlType.Custom
            //                        ? UrlService.GetLinkDB((ParamType)childMenuItem.MenuItemUrlType, Convert.ToInt32(childMenuItem.MenuItemUrlPath))
            //                        : childMenuItem.MenuItemUrlPath,
            childMenuItem.MenuItemUrlPath,
            childMenuItem.MenuItemName,
            childMenuItem.Blank ? "target=\"_blank\"" : string.Empty);

        result.Append("</li>");
        return result.ToString();
    }
}