//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Web;
using AdvantShop;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Customers;
using AdvantShop.FilePath;
using AdvantShop.Helpers;

public partial class UserControls_MainPageProduct : System.Web.UI.UserControl
{
    public SettingsDesign.eMainPageMode Mode { set; get; }
    protected bool EnableRating = SettingsCatalog.EnableProductRating;
    protected CustomerGroup customerGroup = CustomerSession.CurrentCustomer.CustomerGroup;
    protected int ItemsCount = 2;

    protected void Page_Load(object sender, EventArgs e)
    {
        SettingsDesign.eMainPageMode currentMode = !Demo.IsDemoEnabled || !CommonHelper.GetCookieString("structure").IsNotEmpty()
                      ? SettingsDesign.MainPageMode
                      : (SettingsDesign.eMainPageMode)Enum.Parse(typeof(SettingsDesign.eMainPageMode), CommonHelper.GetCookieString("structure"));


        if (Mode != currentMode)
        {
            this.Visible = false;
            return;
        }

        const byte itemsPerLine = 4;
        const byte itemsPerLineThin = 3;

        var countNew = ProductOnMain.GetProductCountByType(ProductOnMain.TypeFlag.New);
        var countDiscount = ProductOnMain.GetProductCountByType(ProductOnMain.TypeFlag.Discount);
        var countBestseller = ProductOnMain.GetProductCountByType(ProductOnMain.TypeFlag.Bestseller);

        switch (Mode)
        {
            case SettingsDesign.eMainPageMode.Default:

                mvMainPageProduct.SetActiveView(viewDefault);

                if (countBestseller == 0)
                {
                    ItemsCount = 3;
                }
                if (countNew == 0)
                {
                    ItemsCount = ItemsCount == 2 ? 3 : 6;
                }
                if (countDiscount == 0)
                {
                    ItemsCount = ItemsCount == 2 ? 3 : 6;
                }

                if (countBestseller > 0)
                {
                    liBestsellers.Attributes.Add("class", "block width-for-" + ItemsCount);
                    lvBestSellers.DataSource = ProductOnMain.GetProductsByType(ProductOnMain.TypeFlag.Bestseller, ItemsCount * SettingsDesign.CountLineOnMainPage);
                    lvBestSellers.DataBind();
                }
                else
                {
                    liBestsellers.Visible = false;
                }
                if (countNew > 0)
                {
                    liNew.Attributes.Add("class", "block width-for-" + ItemsCount);
                    lvNew.DataSource = ProductOnMain.GetProductsByType(ProductOnMain.TypeFlag.New, ItemsCount * SettingsDesign.CountLineOnMainPage);
                    lvNew.DataBind();
                }
                else
                {
                    liNew.Visible = false;
                }

                if (countDiscount > 0)
                {
                    liDiscount.Attributes.Add("class", "block block-last width-for-" + ItemsCount);
                    lvDiscount.DataSource = ProductOnMain.GetProductsByType(ProductOnMain.TypeFlag.Discount, ItemsCount * SettingsDesign.CountLineOnMainPage);
                    lvDiscount.DataBind();
                }
                else
                {
                    liDiscount.Visible = false;
                }

                break;
            case SettingsDesign.eMainPageMode.TwoColumns:
                mvMainPageProduct.SetActiveView(viewAlternative);

                if (countBestseller > 0)
                {
                    lvBestSellersAltervative.DataSource = ProductOnMain.GetProductsByType(ProductOnMain.TypeFlag.Bestseller, itemsPerLine * SettingsDesign.CountLineOnMainPage);
                    lvBestSellersAltervative.DataBind();
                }
                else
                {
                    pnlBest.Visible = false;
                }

                if (countNew > 0)
                {
                    lvNewAlternative.DataSource = ProductOnMain.GetProductsByType(ProductOnMain.TypeFlag.New, itemsPerLine * SettingsDesign.CountLineOnMainPage);
                    lvNewAlternative.DataBind();
                }
                else
                {
                    pnlNew.Visible = false;
                }

                if (countDiscount > 0)
                {
                    lvDiscountAlternative.DataSource = ProductOnMain.GetProductsByType(ProductOnMain.TypeFlag.Discount, itemsPerLine * SettingsDesign.CountLineOnMainPage);
                    lvDiscountAlternative.DataBind();
                }
                else
                {
                    pnlDiscount.Visible = false;
                }
                break;
            case SettingsDesign.eMainPageMode.ThreeColumns:
                mvMainPageProduct.SetActiveView(viewAlternative);

                if (countBestseller > 0)
                {
                    lvBestSellersAltervative.DataSource = ProductOnMain.GetProductsByType(ProductOnMain.TypeFlag.Bestseller, itemsPerLineThin * SettingsDesign.CountLineOnMainPage);
                    lvBestSellersAltervative.DataBind();
                }
                else
                {
                    pnlBest.Visible = false;
                }

                if (countNew > 0)
                {
                    lvNewAlternative.DataSource = ProductOnMain.GetProductsByType(ProductOnMain.TypeFlag.New, itemsPerLineThin * SettingsDesign.CountLineOnMainPage);
                    lvNewAlternative.DataBind();
                }
                else
                {
                    pnlNew.Visible = false;
                }

                if (countDiscount > 0)
                {
                    lvDiscountAlternative.DataSource = ProductOnMain.GetProductsByType(ProductOnMain.TypeFlag.Discount, itemsPerLineThin * SettingsDesign.CountLineOnMainPage);
                    lvDiscountAlternative.DataBind();
                }
                else
                {
                    pnlDiscount.Visible = false;
                }
                break;

            default:
                throw new NotImplementedException();
        }
    }

    protected string RenderPictureTag(int productId, string strPhoto, string urlpath, string photoDesc)
    {
        return string.Format("<a href=\"{0}\"><img src=\"{1}\" alt=\"{2}\" title=\"{2}\" class=\"pv-photo\" /></a>",
            UrlService.GetLink(ParamType.Product, urlpath, productId), strPhoto.IsNotEmpty()
                ? FoldersHelper.GetImageProductPath(ProductImageType.Small, strPhoto, false)
                : "images/nophoto_small.jpg",
                HttpUtility.HtmlEncode(photoDesc));
    }
}