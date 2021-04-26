using System;
using System.Collections.Generic;
using System.Web.UI;
using AdvantShop.Catalog;
using AdvantShop.Configuration;

//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

public partial class UserControls_CatalogView : UserControl
{
    protected Category HeadCategory;
    protected bool DisplayProductsCount = SettingsCatalog.ShowProductsCount;
    public int CategoryID;

    protected void Page_Load(object sender, EventArgs e)
    {
        IList<Category> childs = CategoryService.GetChildCategoriesByCategoryIdForMenu(CategoryID);
        HeadCategory = CategoryService.GetCategory(childs.Count > 0 ? childs[0].ParentCategoryId : 0);
        
        lvChilds.DataSource = childs;
        lvChilds.DataBind();
    }
}
