//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Controls;
using AdvantShop.Core;
using AdvantShop.Core.FieldFilters;
using AdvantShop.News;
using AdvantShop.SEO;
using AdvantShop.CMS;
using AdvantShop.Core.UrlRewriter;

public partial class News : AdvantShopPage
{
    SqlPaging _paging;
    protected void Page_Load(object sender, EventArgs e)
    {
        bool categoryIdIsNum = false;
        int categoryId = -1;
        if (!string.IsNullOrEmpty(Request["newscategoryid"]) && Int32.TryParse(Request["newscategoryid"], out categoryId))
        {
            categoryIdIsNum = categoryId != -1;
        }

        _paging = new SqlPaging { TableName = "Settings.News left join Catalog.Photo on Photo.objId=News.NewsID and Type=@Type", ItemsPerPage = SettingsNews.NewsPerPage };

        _paging.AddField(new Field { Name = "NewsID" });

        _paging.AddField(new Field { Name = "AddingDate", Sorting = SortDirection.Descending });

        _paging.AddField(new Field { Name = "Title" });

        _paging.AddField(new Field { Name = "PhotoName as Picture" });

        _paging.AddField(new Field { Name = "TextToPublication" });

        _paging.AddField(new Field { Name = "TextToEmail" });

        _paging.AddField(new Field { Name = "TextAnnotation" });

        _paging.AddField(new Field { Name = "UrlPath" });

        _paging.AddParam(new SqlParam { ParamName = "@Type", ParamValue = PhotoType.News.ToString() });

        if (categoryIdIsNum)
        {
            var f = new Field { Name = "NewsCategoryID", NotInQuery = true };
            var filter = new EqualFieldFilter { ParamName = "@NewsCategoryID", Value = categoryId.ToString(CultureInfo.InvariantCulture) };
            f.Filter = filter;
            _paging.AddField(f);
        }


        // MetaInfo ------------------------------------------------
        MetaInfo nmeta = null;
        if ((SettingsNews.NewsMetaTitle != null) || (SettingsNews.NewsMetaKeywords != null) || (SettingsNews.NewsMetaDescription != null))
        {
            nmeta = new MetaInfo
                        {
                            Type = MetaType.News,
                            Title =
                                (string.IsNullOrEmpty(SettingsNews.NewsMetaTitle))
                                    ? null
                                    : SettingsMain.ShopName + " - " + SettingsNews.NewsMetaTitle,
                            MetaKeywords =
                                (string.IsNullOrEmpty(SettingsNews.NewsMetaKeywords))
                                    ? null
                                    : SettingsNews.NewsMetaKeywords,
                            MetaDescription =
                                (string.IsNullOrEmpty(SettingsNews.NewsMetaDescription))
                                    ? null
                                    : SettingsNews.NewsMetaDescription
                        };
        }

        // MetaInfo ------------------------------------------------

        var category = NewsService.GetNewsCategoryById(categoryId);
        header.Text = category != null ? string.Format("{0} / {1}", Resources.Resource.Client_News_News, category.Name) : Resources.Resource.Client_News_News;


        if (category != null)
        {
            ucBreadCrumbs.Items = new List<BreadCrumbs>
                                      {
                                          new BreadCrumbs
                                              {
                                                  Name = Resources.Resource.Client_News_News,
                                                  Url = UrlService.GetAbsoluteLink("news")
                                              },
                                          new BreadCrumbs
                                              {
                                                  Name = category.Name,
                                                  Url = UrlService .GetLink(ParamType.NewsCategory ,category .UrlPath, category .NewsCategoryID)
                                              }
                                      };
            nmeta = new MetaInfo
                        {
                            Title = SettingsMain.ShopName + " - " + category.Name,
                            MetaDescription = SettingsNews.NewsMetaKeywords,
                            MetaKeywords = SettingsNews.NewsMetaDescription,
                            Type = MetaType.News
                        };
        }
        else
        {
            ucBreadCrumbs.Visible = false;
        }

        SetMeta(nmeta, string.Empty);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        _paging.ItemsPerPage = paging.CurrentPage != 0 ? SettingsNews.NewsPerPage : int.MaxValue;
        _paging.CurrentPageIndex = paging.CurrentPage != 0 ? paging.CurrentPage : 1;
        var totalCount = _paging.TotalRowsCount;
        paging.TotalPages = (int)(Math.Ceiling((double)totalCount / _paging.ItemsPerPage));

        if (paging.TotalPages < paging.CurrentPage || paging.CurrentPage < 0)
        {
            Error404();
            return;
        }

        lvNews.DataSource = _paging.PageItems;
        lvNews.DataBind();
        paging.TotalPages = _paging.PageCount;


        lvNewsCategories.DataSource = NewsService.GetNewsCategories().Where(item => item.CountNews > 0);
        lvNewsCategories.DataBind();
    }
}