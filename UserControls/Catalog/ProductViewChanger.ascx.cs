using System;
using AdvantShop.Configuration;

public partial class UserControls_Catalog_ProductViewChanger : System.Web.UI.UserControl
{
    public enum eCurrentPage
    {
        Catalog = 0,
        Search = 1
    }

    public int CatalogViewMode { get; set; }
    public int SearchViewMode { get; set; }
    public eCurrentPage CurrentPage { get; set; }
    public static int CurrentViewMode { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if(Session["ViewMode"]!=null)
        {
            CatalogViewMode = Convert.ToInt32(Session["ViewMode"]);
            SearchViewMode = Convert.ToInt32(Session["ViewMode"]);
        }else
        {
            CatalogViewMode = SettingsCatalog.DefaultCatalogView;
            SearchViewMode = SettingsCatalog.DefaultSearchView;
        }

        if ((CurrentPage == eCurrentPage.Catalog && !SettingsCatalog.EnabledCatalogViewChange) || ((CurrentPage == eCurrentPage.Search && !SettingsCatalog.EnabledSearchViewChange)))
        {
            Visible = false;
        }
    }

    protected bool IsSelectedView(int view)
    {
        return (CurrentPage == eCurrentPage.Catalog && CatalogViewMode == view) ||
               (CurrentPage == eCurrentPage.Search && SearchViewMode == view);
    }

    protected void lbTiles_Click(object sender, EventArgs e)
    {
        Session["ViewMode"] = 0;
        CatalogViewMode = 0;
        SearchViewMode = 0;
    }

    protected void lbList_Click(object sender, EventArgs e)
    {
        Session["ViewMode"] = 1;
        CatalogViewMode = 1;
        SearchViewMode = 1;
    }

    protected void lbTable_Click(object sender, EventArgs e)
    {
        Session["ViewMode"] = 2;
        CatalogViewMode = 2;
        SearchViewMode = 2;
    }
}
