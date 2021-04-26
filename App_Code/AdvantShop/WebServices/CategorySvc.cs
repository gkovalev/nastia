//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Web.Services;
using System.Xml;
using AdvantShop.Catalog;
using AdvantShop.Security;

// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
// <System.Web.Script.Services.ScriptService()> _
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class CategorySvc : WebService
{
    private const string MsgAddFailed = "Adding failed";
    private const string MsgAdded = "Category {0} added successfull";
    private const string MsgDeleteFailed = "Deleting of category {0} failed";
    private const string MsgDeleted = "Category {0} deleted successfull";
    private const string MsgNotFound = "Category not found";
    private const string MsgParentNotFound = "Error determining category {0} parent category";
    private const string MsgUpdateFailed = "Category {0} update failed";
    private const string MsgUpdated = "Category {0} updated successfull";
    private const string MsgAuthFailed = "Access denied, please login";

    /// <summary>
    /// LogIn as admin and write login data to cookies
    /// </summary>
    /// <param name="login">Admin login</param>
    /// <param name="password">Admin password</param>
    /// <returns>True if successfull login</returns>
    [WebMethod]
    public bool Login(string login, string password)
    {
        return AuthorizeService.LoginAdmin(login, password, false);
    }
    /// <summary>
    /// Logout and delete user cookies
    /// </summary>
    [WebMethod]
    public void Logout()
    {
        AuthorizeService.DeleteCookie();
    }

    /// <summary>
    /// Gets all categories in xml format
    /// </summary>
    /// <returns>Xml formatted list of categories</returns>
    /// <remarks></remarks>
    [WebMethod]
    public XmlDocument GetCategories()
    {
        if (!AuthorizeService.CheckAdminCookies())
            return ErrMsg(MsgAuthFailed);
        var result = new XmlDocument();
        XmlNode root = result.AppendChild(result.CreateElement("Categories"));
        IEnumerable<Category> categories = CategoryService.GetCategories();
        foreach (Category cat in categories)
        {
            root.InnerXml = root.InnerXml + GetCategory(cat.CategoryId).InnerXml;
        }
        return result;
    }

    /// <summary>
    /// Gets category in xml format
    /// </summary>
    /// <param name="id">ID of category</param>
    /// <returns>Xml formatted category</returns>
    /// <remarks></remarks>
    [WebMethod]
    public XmlDocument GetCategory(int id)
    {
        if (!AuthorizeService.CheckAdminCookies())
            return ErrMsg(MsgAuthFailed);

        XmlDocument result = CategoryService.ConvertToXml(id);
        if (result == null)
        {
            return ErrMsg(string.Format(MsgNotFound, id));
        }
        return result;
    }

    /// <summary>
    /// Adding new category
    /// </summary>
    /// <param name="name">Category name</param>
    /// <param name="parent">Parent category ID</param>
    /// <param name="picture">Category picture name</param>
    /// <param name="enabled">If category is enabled (must be "true" or "false")</param>
    /// <param name="sortOrder">Sort order of category (must be parseble to integer value)</param>
    /// <returns>String log message</returns>
    /// <remarks></remarks>
    [WebMethod]
    public string AddCategoryByFields(string name, int parent, string picture, bool enabled, int sortOrder)
    {
        if (!AuthorizeService.CheckAdminCookies())
            return MsgAuthFailed;
        if (string.IsNullOrEmpty(picture))
        {
            picture = null;
        }
        var category = new Category
                           {
                               Name = name,
                               ParentCategoryId = parent,
                               Picture = new Photo(0, 0, PhotoType.CategoryBig) { OriginName = picture },
                               SortOrder = sortOrder,
                               Enabled = enabled
                           };
        int catId = CategoryService.AddCategory(category, true);
        return catId != 0 ? string.Format(MsgAdded, catId) : MsgAddFailed;
    }

    /// <summary>
    /// Adding category
    /// </summary>
    /// <param name="category"></param>
    /// <returns>String message</returns>
    /// <remarks></remarks>
    [WebMethod]
    public string AddCategory(Category category)
    {
        if (!AuthorizeService.CheckAdminCookies())
            return MsgAuthFailed;
        int catId = CategoryService.AddCategory(category, true);
        if (catId != 0)
        {
            return string.Format(MsgAdded, catId);
        }
        else
        {
            return MsgAddFailed;
        }
    }

    /// <summary>
    /// Updates category by ID
    /// </summary>
    /// <param name="id">Category ID</param>
    /// <param name="name">Category name</param>
    /// <param name="parentCategoryId">Parent category ID</param>
    /// <param name="description">Category description</param>
    /// <param name="enabled">If category is enabled (must be "true" or "false")</param>
    /// <param name="sortOrder">Sort order of category (must be parsable to integer value)</param>
    /// <param name="displayStyle">Category display style</param>
    /// <param name="displayThematicTabs">If display thematic tabs (must be "true" or "false")</param>
    /// <param name="picture">Category picture</param>
    /// <param name="miniPicture">Category minipicture</param>
    /// <param name="title">Category title</param>
    /// <param name="metaKeywords">Category meta keywords</param>
    /// <param name="metaDescription">Category meta description</param>
    /// <returns>String log message</returns>
    /// <remarks></remarks>
    [WebMethod]
    public string UpdateCategoryFields(int id, string name, int parentCategoryId, string description,
                                       string enabled, string sortOrder, string displayStyle, string displayThematicTabs,
                                       string picture, string miniPicture, string title, string metaKeywords,
                                       string metaDescription)
    {
        if (!AuthorizeService.CheckAdminCookies())
            return MsgAuthFailed;
        Category c = CategoryService.GetCategory(id);
        if (c == null)
        {
            return string.Format(MsgNotFound, id);
        }
        if (!string.IsNullOrEmpty(name))
        {
            c.Name = name;
        }

        c.ParentCategoryId = parentCategoryId;

        if (!string.IsNullOrEmpty(description))
        {
            c.Description = description;
        }
        bool boolVal;
        if (!string.IsNullOrEmpty(enabled) && bool.TryParse(enabled, out boolVal))
        {
            c.Enabled = boolVal;
        }
        if (!string.IsNullOrEmpty(displayStyle))
        {
            c.DisplayStyle = displayStyle;
        }

        if (!string.IsNullOrEmpty(picture))
        {
            c.Picture.PhotoName = picture;
        }
        if (!string.IsNullOrEmpty(miniPicture))
        {
            c.MiniPicture.PhotoName = miniPicture;
        }

        if (!string.IsNullOrEmpty(title))
        {
            c.Meta.Title = title;
        }
        if (!string.IsNullOrEmpty(metaKeywords))
        {
            c.Meta.MetaKeywords = metaKeywords;
        }
        if (!string.IsNullOrEmpty(metaDescription))
        {
            c.Meta.MetaDescription = metaDescription;
        }
        bool intVal;
        if (!bool.TryParse(sortOrder, out intVal))
        {
            intVal = false;
        }
        var res = CategoryService.UpdateCategory(c, intVal);
        return string.Format(res ? MsgUpdated : MsgUpdateFailed, id);
    }

    /// <summary>
    /// Updates given category
    /// </summary>
    /// <param name="category"></param>
    /// <returns></returns>
    /// <remarks></remarks>
    [WebMethod]
    public string UpdateCategory(Category category)
    {
        if (!AuthorizeService.CheckAdminCookies())
            return MsgAuthFailed;
        if (category == null)
        {
            return string.Format(MsgNotFound);
        }
        var res = CategoryService.UpdateCategory(category, false);
        return string.Format(res ? MsgUpdated : MsgUpdateFailed, category.CategoryId);
    }


    /// <summary>
    /// Deletes category by ID
    /// </summary>
    /// <param name="id">Deleting category ID</param>
    /// <returns>String log message</returns>
    /// <remarks></remarks>
    [WebMethod]
    public string DeleteCategory(int id)
    {
        if (!AuthorizeService.CheckAdminCookies())
            return MsgAuthFailed;
        if (CategoryService.GetCategory(id) == null)
        {
            return string.Format(MsgNotFound, id);
        }
        CategoryService.DeleteCategoryAndPhotos(id);
        return string.Format(CategoryService.GetCategory(id) == null ? MsgDeleted : MsgDeleteFailed, id);
    }

    /// <summary>
    /// Gets child categories by parent category ID
    /// </summary>
    /// <param name="parentId"></param>
    /// <returns></returns>
    /// <remarks></remarks>
    [WebMethod]
    public XmlDocument GetChildCategiries(int parentId)
    {
        if (!AuthorizeService.CheckAdminCookies())
            return ErrMsg(MsgAuthFailed);
        var result = new XmlDocument();
        var root = (XmlElement)result.AppendChild(result.CreateElement("Categories"));
        var cats = (List<Category>)CategoryService.GetChildCategoriesByCategoryId(parentId, true);
        foreach (Category cat in cats)
        {
            root.InnerXml = root.InnerXml + CategoryService.ConvertToXml(cat).InnerXml;
        }
        return result;
    }

    /// <summary>
    /// Gets parent category by current ID
    /// </summary>
    /// <param name="categoryId"></param>
    /// <returns></returns>
    /// <remarks></remarks>
    [WebMethod]
    public XmlDocument GetParentCategory(int categoryId)
    {
        if (!AuthorizeService.CheckAdminCookies())
            return ErrMsg(MsgAuthFailed);
        if (CategoryService.GetCategory(categoryId) == null)
        {
            return ErrMsg(string.Format(MsgNotFound, categoryId));
        }
        var cats = (List<Category>)CategoryService.GetParentCategories(categoryId);
        if (cats.Count <= 1)
        {
            return ErrMsg(string.Format(MsgParentNotFound, categoryId));
        }
        Category parent = cats[1];
        return CategoryService.ConvertToXml(parent);
    }

    private static XmlDocument ErrMsg(string errTxt)
    {
        var result = new XmlDocument();
        XmlElement errorXml = result.CreateElement("Error");
        errorXml.InnerText = errTxt;
        result.AppendChild(errorXml);
        return result;
    }
}