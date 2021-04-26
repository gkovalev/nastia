//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop.CMS;
using AdvantShop.Configuration;
using AdvantShop.Core.Caching;
using AdvantShop.Core.UrlRewriter;

public partial class UserControls_MenuTop : System.Web.UI.UserControl
{
    protected string GetHtml()
    {
        if (!AdvantShop.Customers.CustomerSession.CurrentCustomer.RegistredUser && CacheManager.Contains(CacheNames.GetMainMenuCacheObjectName()))
        {
            return CacheManager.Get<string>(CacheNames.GetMainMenuCacheObjectName());
        }
        if (AdvantShop.Customers.CustomerSession.CurrentCustomer.RegistredUser && CacheManager.Contains(CacheNames.GetMainMenuAuthCacheObjectName()))
        {
            return CacheManager.Get<string>(CacheNames.GetMainMenuAuthCacheObjectName());
        }

        string result = string.Empty;

        foreach (var mItem in MenuService.GetEnabledChildMenuItemsByParentId(0, MenuService.EMenuType.Top, AdvantShop.Customers.CustomerSession.CurrentCustomer.RegistredUser ? EMenuItemShowMode.Authorized : EMenuItemShowMode.NotAuthorized))
        {
            result += string.Format("<a href=\"{0}\"{1}>{2}</a>\n",
                //mItem.MenuItemUrlType != EMenuItemUrlType.Custom
                //                    ? UrlService.GetLinkDB((ParamType)mItem.MenuItemUrlType, Convert.ToInt32(mItem.MenuItemUrlPath))
                //                    : mItem.MenuItemUrlPath,
                mItem.MenuItemUrlPath,
                mItem.Blank ? " target=\"_blank\"" : string.Empty,
                mItem.MenuItemName);
        }
        if (!AdvantShop.Customers.CustomerSession.CurrentCustomer.RegistredUser)
            CacheManager.Insert(CacheNames.GetMainMenuCacheObjectName(), result);
        else if (AdvantShop.Customers.CustomerSession.CurrentCustomer.RegistredUser)
            CacheManager.Insert(CacheNames.GetMainMenuAuthCacheObjectName(), result);

        return result;
    }

}