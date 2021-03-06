//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using AdvantShop.Helpers;
using AdvantShop.SEO;

namespace AdvantShop.Core.UrlRewriter
{
    public enum ParamType
    {
        None,
        Product,
        Category,
        StaticPage,
        News,
        Brand,
        NewsCategory
    }

    public static class UrlService
    {
        public class UrlStruct
        {
            public int ObjId { get; set; }
            public string UrlPath { get; set; }
            public ParamType Type { get; set; }
        }

        private const string ProductsWord = "products";
        private const string CategoriesWord = "categories";
        private const string PagesWord = "pages";
        private const string NewsWord = "news";
        private const string NewscategoryWord = "newscategory";
        private const string ManufacturersWord = "manufacturers";

        public static readonly Dictionary<string, string> UrlTable = new Dictionary<string, string>
                                                                {
                                                                    {"/news", "~/News.aspx"},
                                                                    {"/feedback", "~/Feedback.aspx"},
                                                                    {"/catalog", "~/Catalog.aspx?CategoryID=0"},
                                                                    {"/manufacturers", "~/Brands.aspx"},
                                                                    {"/admin/", "~/admin/default.aspx"},
                                                                };

        public static readonly List<string> Storages = new List<string> {
                                                                             "/js/plugins/",
                                                                             "/ie6/images/",
                                                                             "/ie6/css/",
                                                                             "/social/css/",
                                                                             "/social/js/",
                                                                             "/social/images/",
                                                                             "/ckeditor/",
                                                                             "/design/backgrounds/",
                                                                             "/design/colors/",
                                                                             "/design/themes/",
                                                                             "/admin/images/",
                                                                             "/info/images/",
                                                                             "/info/images/",
                                                                             "/install/images/", 
                                                                             "/usercontrols/images/",
                                                                             "/images/",
                                                                             "/pictures_extra/",
                                                                             "/pictures/",
                                                                             "/install/js/",
                                                                             "/admin/js/",
                                                                             "/html/js/",
                                                                             "/install/css/",
                                                                             "/admin/css/",
                                                                             "/html/css/",
                                                                             "/js/",
                                                                             "/css/",
                                                                             "/admin/",
                                                                             "/info/",
                                                                             "/httphandlers/",
                                                                             "/price_temp/",
                                                                             "/price_download/",
                                                                             "/pictures_elbuz/",
                                                                             "/usercontrols/",
                                                                             "/userfiles/",
                                                                             "/webservices/"
                                                                            };

        public static readonly List<string> Social = new List<string>
                                                         {
                                                             "adv-vk",
                                                             "adv-fb"
                                                         };

        public static readonly Dictionary<ParamType, string> NamesAndPages = new Dictionary<ParamType, string>
                                                                                         {
                                                                                             {ParamType.StaticPage, "StaticPageView.aspx"},
                                                                                             {ParamType.Category, "Catalog.aspx"},
                                                                                             {ParamType.Product, "Details.aspx"},
                                                                                             {ParamType.NewsCategory, "News.aspx"},
                                                                                             {ParamType.News, "NewsView.aspx"},
                                                                                             {ParamType.Brand, "BrandView.aspx"},
                                                                                             {ParamType.None, string.Empty}
                                                                                         };

        public static readonly Dictionary<ParamType, string> NamesAndIds = new Dictionary<ParamType, string>
                                                                                         {
                                                                                             {ParamType.StaticPage, "StaticPageId"},
                                                                                             {ParamType.Category, "CategoryId"},
                                                                                             {ParamType.Product, "ProductId"},
                                                                                             {ParamType.NewsCategory, "NewsCategoryId"},
                                                                                             {ParamType.News, "NewsId"},
                                                                                             {ParamType.Brand, "BrandId"},
                                                                                             {ParamType.None, string.Empty}
                                                                                         };

        public static readonly Dictionary<ParamType, string> NamesAndWords = new Dictionary<ParamType, string>
                                                                                         {
                                                                                             {ParamType.StaticPage, PagesWord},
                                                                                             {ParamType.Category, CategoriesWord},
                                                                                             {ParamType.Product, ProductsWord},
                                                                                             {ParamType.NewsCategory, NewscategoryWord},
                                                                                             {ParamType.News, NewsWord},
                                                                                             {ParamType.Brand, ManufacturersWord},
                                                                                             {ParamType.None, string.Empty}
                                                                                         };
        public static readonly Dictionary<ParamType, string> NamesAndDb = new Dictionary<ParamType, string>
                                                                                         {
                                                                                             {ParamType.StaticPage, "CMS.StaticPage"},
                                                                                             {ParamType.Category, "Catalog.Category"},
                                                                                             {ParamType.Product, "Catalog.Product"},
                                                                                             {ParamType.NewsCategory, "Settings.NewsCategory"},
                                                                                             {ParamType.News, "Settings.News"},
                                                                                             {ParamType.Brand, "Catalog.Brand"},
                                                                                             {ParamType.None, string.Empty}
                                                                                         };
        public static readonly List<string> UnAvailableWords = new List<string>
                                                                                 {
                                                                                     ProductsWord,
                                                                                     CategoriesWord,
                                                                                     PagesWord,
                                                                                     NewscategoryWord,
                                                                                     NewsWord,
                                                                                     ManufacturersWord,
                                                                                     "techdemos",
                                                                                     "tools",
                                                                                     "info",
                                                                                     "css",
                                                                                     "js",
                                                                                     "webservices",
                                                                                     "userfiles",
                                                                                     "usercontrols",
                                                                                     "admin",
                                                                                     "httphandlers",
                                                                                     "install",
                                                                                     "ckeditor",
                                                                                     "adv-fb",
                                                                                     "adv-vk",
                                                                                     "modules"
                                                                                 };
        /// <summary>
        /// Warning!!! if we can't urlpath on databing
        /// </summary>
        /// <param name="type"></param>
        /// <param name="objId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string GetLinkDB(ParamType type, int objId, string query)
        {
            var objUrl = GetObjUrlFromDb(type, objId);
            return GetLink(type, objUrl, objId, query);
        }

        /// <summary>
        /// Warning!!! if we can't urlpath on databing
        /// </summary>
        /// <param name="type"></param>
        /// <param name="objId"></param>
        /// <returns></returns>
        public static string GetLinkDB(ParamType type, int objId)
        {
            var objUrl = GetObjUrlFromDb(type, objId);
            return GetLink(type, objUrl, objId);
        }

        /// <summary>
        /// get url from db by id and type
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetObjUrlFromDb(ParamType type, int objId)
        {
            if (type == ParamType.None) return string.Empty;
            return SQLDataAccess.ExecuteScalar<string>(string.Format("select urlPath from {0} where {1} =@id", NamesAndDb[type], NamesAndIds[type]),
                                                        CommandType.Text,
                                                        new SqlParameter { ParameterName = "@id", Value = objId });
        }

        /// <summary>
        /// create url-string
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="objUrl"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetLink(ParamType type, string objUrl, int objId)
        {
            return GetLink(type, objUrl, objId, string.Empty);
        }

        /// <summary>
        /// return url link
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="objUrl"></param>
        /// <param name="type"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string GetLink(ParamType type, string objUrl, int objId, string query)
        {
            return string.IsNullOrEmpty(objUrl)
                ? NamesAndPages[type] + '?' + NamesAndIds[type] + "=" + objId + (string.IsNullOrEmpty(query) ? string.Empty : '&' + query)
                : NamesAndWords[type] + '/' + objUrl + (string.IsNullOrEmpty(query) ? string.Empty : '?' + query);
        }

        public static string GetAbsoluteLink(string link)
        {
            if (link.Contains("http://") || link.Contains("https://")) return link;
            return string.Format("{0}/{1}", (HttpContext.Current.Request.ApplicationPath == "/" ? string.Empty : HttpContext.Current.Request.ApplicationPath), link.TrimStart('/'));
        }

        public static string GetAdminAbsoluteLink(string link)
        {
            if (link.Contains("http://") || link.Contains("https://")) return link;
            return string.Format("{0}/{1}", (HttpContext.Current.Request.ApplicationPath == "/" ? "/admin" : HttpContext.Current.Request.ApplicationPath + "/admin"), link.TrimStart('/'));
        }

        /// <summary>
        /// Get if url is avalible
        /// </summary>
        /// <param name="objUrl"></param>
        /// <param name="objId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsAvalibleUrl(int objId, ParamType type, string objUrl)
        {
            if (string.IsNullOrWhiteSpace(objUrl)) return true;
            // find in unavalible words
            if (UnAvailableWords.FirstOrDefault(x => x == objUrl) != null) return false;
            //find  in database
            if (GetUrlCount(objUrl, type, objId) > 0) return false;

            return true;
        }

        public static bool IsAvalibleUrl(ParamType type, string objUrl)
        {
            if (string.IsNullOrWhiteSpace(objUrl)) return true;
            // find in unavalible words
            var temp = objUrl.ToLower();
            if (UnAvailableWords.FirstOrDefault(x => x == temp) != null) return false;
            //find  in database
            if (GetUrlCount(temp, type, 0) > 0) return false;

            return true;
        }

        /// <summary>
        /// Get count of objUrl in database by type
        /// </summary>
        /// <param name="objUrl"></param>
        /// <param name="type"></param>
        /// <param name="objId"></param>
        /// <returns></returns>
        private static int GetUrlCount(string objUrl, ParamType type, int objId)
        {
            return SQLDataAccess.ExecuteScalar<int>(string.Format("SELECT COUNT(*) FROM {0} WHERE UrlPath=@UrlPath AND {1} <> @id", NamesAndDb[type], NamesAndIds[type]),
                                                    CommandType.Text,
                                                    new SqlParameter { ParameterName = "@UrlPath", Value = objUrl },
                                                    new SqlParameter { ParameterName = "@id", Value = objId }
                                                    );
        }

        public static bool CheckDebugAddress(string url)
        {
            // Add here more adress if you need it
            if ((url.ToLower().Contains("/tools/")) ||
                (url.ToLower().Contains("/techdemos/")) ||
                (url.ToLower().Contains("/info/")))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// get obj id's by urlpath and type
        /// </summary>
        /// <param name="url"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static int GetObjIdByUrlAndType(string url, ParamType type)
        {
            if (type == ParamType.None) return 0;
            return SQLDataAccess.ExecuteScalar<int>(string.Format("Select {0} from {1} where UrlPath=@UrlPath ", NamesAndIds[type], NamesAndDb[type]),
                                                    CommandType.Text,
                                                    new SqlParameter { ParameterName = "@UrlPath", Value = url });
        }
        /// <summary>
        /// create UrlStruct
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="url"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static UrlStruct CreateUrlStruct(int objId, string url, ParamType type)
        {
            if (objId == 0) return null;
            return new UrlStruct
            {
                ObjId = objId,
                UrlPath = url,
                Type = type
            };
        }
        /// <summary>
        /// return UrlStruct to rewrite
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static UrlStruct ParseRequest(string path)
        {
            var urlWord = path.Split('/').Last();
            ParamType type = ParamType.None;
            foreach (var pair in NamesAndWords)
            {
                if (path.Contains(pair.Value))
                {
                    type = pair.Key;
                    break;
                }
            }
            var tempId = GetObjIdByUrlAndType(urlWord, type);
            return CreateUrlStruct(tempId, urlWord, type);
        }

        public static bool HasExtention(string strCurrentUrl)
        {
            var urlWithoutRequest = strCurrentUrl.Split('?')[0];
            return Path.GetExtension(urlWithoutRequest).IsNotEmpty();
        }

        public static void RedirectTo(HttpApplication app, UrlStruct param)
        {
            var query = NamesAndIds[param.Type] + "=" + param.ObjId;
            app.Context.RewritePath("~/" + NamesAndPages[param.Type], "", string.IsNullOrEmpty(app.Request.Url.Query) ? query : query + "&" + app.Request.QueryString);
        }

        public static string GetRedirect301(string fromUrl, string absoluteUri)
        {
            absoluteUri = absoluteUri.ToLower();
            if (HasExtention(fromUrl) && !fromUrl.Contains(".aspx"))
            {
                return null;
            }

            var redirect = RedirectSeoService.GetByInputUrl(fromUrl, absoluteUri);
            if (redirect == null)
            {
                return null;
            }

            string location = string.Empty;
            if (!string.IsNullOrEmpty(redirect.ProductArtNo))
            {
                var product = Catalog.ProductService.GetProduct(redirect.ProductArtNo);

                location = product != null ? GetAbsoluteLink(GetLink(ParamType.Product, product.UrlPath, product.ID))
                                           : absoluteUri.Replace(fromUrl, redirect.RedirectTo);
            }
            else
            {
                string absoluteUriEncoded = absoluteUri.Split('/').Select(HttpUtility.UrlDecode).AggregateString('/').ToLower();
                if (absoluteUriEncoded.Contains(redirect.RedirectFrom))
                {
                    location = absoluteUriEncoded.Replace(redirect.RedirectFrom, redirect.RedirectTo);
                }
            }
            return location;
        }

        public static string GetEvalibleValidUrl(int objId, ParamType type, string prevUrl)
        {
            int j = 1;
            string url = StringHelper.TransformUrl(StringHelper.Translit(prevUrl));
            while (!IsAvalibleUrl(objId, type, url))
            {
                url = StringHelper.TransformUrl(StringHelper.Translit(prevUrl)) + "-" + j++;
            }
            return url;
        }
    }
}