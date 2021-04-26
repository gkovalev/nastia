//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop;
using AdvantShop.CMS;
using AdvantShop.Controls;
using AdvantShop.Core.UrlRewriter;
using Resources;

public partial class StaticPageView : AdvantShopPage
{
    protected StaticPage page;
    protected bool hasSubPages;

    protected void Page_Load(object sender, EventArgs e)
    {
        int pageId = Page.Request["staticpageid"].TryParseInt();
        page = StaticPageService.GetStaticPage(pageId);
        if (pageId == 0 || page == null || (page != null && !page.Enabled))
        {
            Error404();
            return;
        }
        SetMeta(page.Meta, page.PageName);
        

        sbShareButtons.Visible = AdvantShop.Configuration.SettingsDesign.EnableSocialShareButtons;

        ucBreadCrumbs.Items = StaticPageService.GetParentStaticPages(pageId).Select(StaticPageService.GetStaticPage).Select(stPage => new BreadCrumbs
        {
            Name = stPage.PageName,
            Url = UrlService.GetLink(ParamType.StaticPage, stPage.UrlPath, stPage.StaticPageId)
        }).Reverse().ToList();

        ucBreadCrumbs.Items.Insert(0, new BreadCrumbs
        {
            Name = Resource.Client_MasterPage_MainPage,
            Url = UrlService.GetAbsoluteLink("/")
        });
    }


    protected void Page_PreRender(object sender, EventArgs e)
    {
        lvSubPages.DataSource = StaticPageService.GetChildStaticPages(page.ID, true);
        lvSubPages.DataBind();
        hasSubPages = rightBlock.Visible = lvSubPages.Items.Any();
        
    }
}