//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Linq;
using AdvantShop.Controls;
using AdvantShop.News;
using System.Collections.Generic;
using AdvantShop.CMS;
using AdvantShop.Core.UrlRewriter;
using Resources;

public partial class NewsView : AdvantShopPage
{
    public string StrMainText = string.Empty;
    public string StrTitleText = string.Empty;
    public string StrAnnotationText = string.Empty;
    protected string SPhoto;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Page.Request["newsid"]))
        {
            this.Error404();
            return;
        }

        int tempId;
        Int32.TryParse(Page.Request["newsid"], out tempId);

        var currentNews = NewsService.GetNewsById(tempId);
        if (currentNews == null)
        {
            this.Error404();
            return;
        }

        sbShareButtons.Visible = AdvantShop.Configuration.SettingsDesign.EnableSocialShareButtons;

        SetMeta(currentNews.Meta, currentNews.Title);
        StrTitleText = currentNews.Title;
        StrAnnotationText = currentNews.TextAnnotation;
        StrMainText = currentNews.TextToPublication;
        var category = NewsService.GetNewsCategoryById(currentNews.NewsCategoryID);
        ucBreadCrumbs.Items = new List<BreadCrumbs>
                                  {
                                      new BreadCrumbs
                                          {
                                              Name = Resource.Client_MasterPage_MainPage,
                                              Url = UrlService.GetAbsoluteLink("/")
                                          },

                                      new BreadCrumbs
                                          {
                                              Name = Resource.Client_News_News,
                                              Url = "news"
                                          },
                                      new BreadCrumbs
                                          {
                                              Name = category.Name,
                                              Url = category.UrlPath
                                          }
                                  };
    }


    protected void Page_PreRender(object sender, EventArgs e)
    {
        lvNewsCategories.DataSource = NewsService.GetNewsCategories().Where(item => item.CountNews > 0);
        lvNewsCategories.DataBind();
    }
}