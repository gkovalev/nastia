//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Linq;
using System.Web.UI;
using AdvantShop;
using AdvantShop.Configuration;
using AdvantShop.Customers;
using AdvantShop.FilePath;

public partial class UserControls_ProductViewSocial : UserControl
{
    public enum ProductViewMode
    {
        Tiles = 0,
        List = 1,
        Table = 2
    }

    public object DataSource { set; get; }
    public ProductViewMode ViewMode { set; get; }
    public bool HasProducts { private set; get; }

    protected bool EnableRating = SettingsCatalog.EnableProductRating;
    protected bool EnableCompare = SettingsCatalog.EnableCompareProducts;
    protected CustomerGroup customerGroup = CustomerSession.CurrentCustomer.CustomerGroup;

    protected string RenderPictureTag(string urlPhoto, string productName, string urlPath)
    {
        string strFormat = string.Empty;

        switch (ViewMode)
        {
            case ProductViewMode.Tiles:
                strFormat = string.Format("<a href=\"{0}\"><img src=\"{1}\" alt=\"{2}\" /></a>", urlPath,
                                             FoldersHelper.GetImageProductPath(ProductImageType.Small, urlPhoto, false), productName);
                break;
            case ProductViewMode.List:
                strFormat = string.Format("<a href=\"{0}\"><img src=\"{1}\" alt=\"{2}\" /></a>", urlPath,
                                            FoldersHelper.GetImageProductPath(ProductImageType.Small, urlPhoto, false), productName);
                break;
            case ProductViewMode.Table:
                if (urlPhoto.IsNotEmpty())
                {
                    strFormat = string.Format("abbr=\"{0}\"",
                                              FoldersHelper.GetImageProductPath(ProductImageType.Small, urlPhoto, false));
                }
                break;
        }
        return strFormat;
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        switch (ViewMode)
        {
            case ProductViewMode.Tiles:
                mvProducts.SetActiveView(viewTile);
                lvTile.DataSource = DataSource;
                lvTile.DataBind();
                HasProducts = lvTile.Items.Any();
                break;
            case ProductViewMode.List:
                mvProducts.SetActiveView(viewList);
                lvList.DataSource = DataSource;
                lvList.DataBind();
                HasProducts = lvList.Items.Any();
                break;

            case ProductViewMode.Table:
                mvProducts.SetActiveView(viewTable);
                lvTable.DataSource = DataSource;
                lvTable.DataBind();
                HasProducts = lvTable.Items.Any();
                break;
        }
    }
}