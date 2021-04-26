using System;
using AdvantShop.CMS;
using AdvantShop.Configuration;
using AdvantShop.Core.Caching;
using AdvantShop.Core.UrlRewriter;
using System.Text;
using System.Linq;

//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

public partial class UserControls_MenuTopAlternative : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        searchBlock.Visible = SettingsDesign.SearchBlockLocation == SettingsDesign.eSearchBlockLocation.CatalogMenu;
    }

    public string GetMenu()
    {
        var useCache = !Request.Url.AbsolutePath.Contains("err404.aspx");

        var rawUrl = Request.RawUrl;
        var cachename = CacheNames.GetMainMenuCacheObjectName() + "Alternative_" + rawUrl;

        if (useCache && !AdvantShop.Customers.CustomerSession.CurrentCustomer.RegistredUser && CacheManager.Contains(cachename))
        {
            return CacheManager.Get<string>(cachename);
        }
        if (useCache && AdvantShop.Customers.CustomerSession.CurrentCustomer.RegistredUser && CacheManager.Contains(cachename))
        {
            return CacheManager.Get<string>(cachename);
        }
        
        var result = new StringBuilder();

        var rootCategories = MenuService.GetEnabledChildMenuItemsByParentId(0, MenuService.EMenuType.Top, AdvantShop.Customers.CustomerSession.CurrentCustomer.RegistredUser ? EMenuItemShowMode.Authorized : EMenuItemShowMode.NotAuthorized).ToList();

        for (int rootIndex = 0; rootIndex < rootCategories.Count; ++rootIndex)
        {
            result.AppendFormat("<div class=\"{0}\"><div class=\"tree-item-inside\">", rawUrl.EndsWith(rootCategories[rootIndex].MenuItemUrlPath) ? "tree-item-selected" : "tree-item");

            result.AppendFormat("<a href=\"{0}\" class=\"{1}\">{2}</a>",
                                    rootCategories[rootIndex].MenuItemUrlPath,
                                    rootCategories[rootIndex].HasChild ? "tree-item-link tree-parent" : "tree-item-link",
                                    rootCategories[rootIndex].MenuItemName);

            if (rootCategories[rootIndex].HasChild)
            {
                result.AppendFormat("<div class=\"tree-submenu\">\r\n");
                result.Append("<div class=\"tree-submenu-category\">\r\n<div class=\"tree-submenu-column\">");

                foreach (var children in MenuService.GetEnabledChildMenuItemsByParentId(rootCategories[rootIndex].MenuItemID, MenuService.EMenuType.Top, AdvantShop.Customers.CustomerSession.CurrentCustomer.RegistredUser ? EMenuItemShowMode.Authorized : EMenuItemShowMode.NotAuthorized))
                {
                    result.AppendFormat("<a href=\"{0}\">{1}</a>", children.MenuItemUrlPath, children.MenuItemName);
                }
                result.Append("</div></div>\r\n");
                result.AppendFormat("</div>");
            }

            //Пункт в главном меню закрывается
            result.AppendFormat("</div></div>");

            if (rootIndex != rootCategories.Count - 1)
            {
                result.AppendFormat("<div class=\"tree-item-split\"></div>");
            }
        }

        string resultstring = result.ToString();

        if (useCache && !AdvantShop.Customers.CustomerSession.CurrentCustomer.RegistredUser)
            CacheManager.Insert(cachename, resultstring);
        else if (useCache && AdvantShop.Customers.CustomerSession.CurrentCustomer.RegistredUser)
            CacheManager.Insert(cachename, resultstring);

        return resultstring;
    }
}