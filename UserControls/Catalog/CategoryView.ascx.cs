//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using AdvantShop;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.FilePath;

public partial class UserControls_Catalog_CategoryView : UserControl
{
    public int CategoryID { set; get; }
    protected bool DisplayProductsCount = SettingsCatalog.ShowProductsCount;

    protected void Page_Load(object sender, EventArgs e)
    {
        var categories = CategoryService.GetChildCategoriesByCategoryId(CategoryID, false).Where(cat => cat.Enabled && cat.HirecalEnabled);
        lvCategory.DataSource = categories;
        lvCategory.DataBind();
    }

    protected string RenderCategoryImage(string imageUrl, int categoryId, string urlPath, string categoryName)
    {
        if (imageUrl.IsNullOrEmpty())
            return string.Empty;
        return string.Format("<a href=\"{0}\"><img src=\"{1}\" class=\"cat-image\" alt=\"{2}\" title=\"{2}\" /></a>",
                             UrlService.GetLink(ParamType.Category, urlPath, Convert.ToInt32(categoryId)),
                             FoldersHelper.GetImageCategoryPath(CategoryImageType.Small, imageUrl, false),
                             HttpUtility.HtmlEncode(categoryName));
    }
}