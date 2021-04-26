using System;
using System.Web.UI;
using AdvantShop.Catalog;
using AdvantShop.Core;

//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------


public partial class UserControls_CatalogDataTreeView : UserControl
{
    //private const int Selected = 0;
    
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        TotalRecordsLiteral.InnerText = CategoryService.GetHierarchyProductsCount(0).ToString();
    }
}