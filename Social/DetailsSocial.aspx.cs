//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Linq;
using System.Web;
using AdvantShop;
using AdvantShop.Catalog;
using AdvantShop.CMS;
using AdvantShop.Configuration;
using AdvantShop.Controls;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Customers;
using AdvantShop.Orders;
using Resources;

public partial class DetailsSocial : AdvantShopPage
{
    protected Product CurrentProduct;

    protected int ProductId
    {
        get { return Request["productid"].TryParseInt(); }
    }

    // Индекс редактируемого элемента
    private int ItemIndex
    {
        get
        {
            return IsEditItem ? Convert.ToInt32(Request["edititem"]) : -1;
        }
    }

    private bool IsEditItem
    {
        get
        {
            return Request["edititem"].IsInt();
        }
    }

    private bool IsCorrectItemType
    {
        get
        {
            try
            {
                var itemType = (ShoppingCartType)(Int32.Parse(Request["type"]));
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    private ShoppingCartType ItemType
    {
        get
        {
            return (ShoppingCartType)(Request["type"].TryParseInt(0));
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (ProductId == 0)
        {
            Error404();
            return;
        }

        //if not have category
        if (ProductService.GetCountOfCategoriesByProductId(ProductId) == 0)
        {
            Error404();
            return;
        }


        // --- Check product exist ------------------------
        CurrentProduct = ProductService.GetProduct(ProductId);
        if (CurrentProduct == null || CurrentProduct.Enabled == false || CurrentProduct.HirecalEnabled == false)//CategoryService.IsEnabledParentCategories(_product.CategoryID) == false)
        {
            Error404();
            return;
        }

        if(CurrentProduct.Amount == 0 || CurrentProduct.Price == 0)
        {
            divAmount.Visible = false;
        }

        rating.ProductId = CurrentProduct.ID;
        rating.Rating = CurrentProduct.Ratio;
        rating.ShowRating = SettingsCatalog.EnableProductRating;
        rating.ReadOnly = RatingService.DoesUserVote(ProductId, CustomerSession.CustomerId);


        pnlSize.Visible = !string.IsNullOrEmpty(CurrentProduct.Size) && (CurrentProduct.Size != "0|0|0");
        //lblSize.Text = !string.IsNullOrEmpty(_product.Size) ? _product.Size.Replace("|", "x") : string.Empty;
        pnlWeight.Visible = CurrentProduct.Weight != 0;
        //lblWeight.Text = _product.Weight.ToString();
        pnlBrand.Visible = CurrentProduct.Brand != null;

        productPropertiesView.ProductId = ProductId;
        productPhotoView.Product = CurrentProduct;
        ProductVideoView.ProductID = ProductId;
        relatedProducts.ProductIds.Add(ProductId);
        alternativeProducts.ProductIds.Add(ProductId);
        breadCrumbs.Items = CategoryService.GetParentCategories(CurrentProduct.CategoryID).Reverse().Select(cat => new BreadCrumbs()
                                                                                                 {
                                                                                                     Name = cat.Name,
                                                                                                     Url = "social/catalogsocial.aspx?categoryid=" + cat.CategoryId
                                                                                                 }).ToList();
        breadCrumbs.Items.Insert(0, new BreadCrumbs()
                                        {
                                            Name = Resource.Client_MasterPage_MainPage,
                                            Url = UrlService.GetAbsoluteLink("social/catalogsocial.aspx")
                                        });

        breadCrumbs.Items.Add(new BreadCrumbs { Name = CurrentProduct.Name, Url = null });

        RecentlyViewService.SetRecentlyView(CustomerSession.CustomerId, ProductId);

        if ((!IsPostBack) && (IsEditItem))
        {
            if (!IsCorrectItemType)
            {
                Redirect(UrlService.GetLink(ParamType.Product, CurrentProduct.UrlPath, ProductId));
                return;
            }

            switch (ItemType)
            {
                case ShoppingCartType.ShoppingCart:
                    //txtAmount.Text = ShoppingCartService.CurrentShoppingCart[ItemIndex].Amount.ToString();
                    break;

                case ShoppingCartType.Wishlist:
                    //txtAmount.Text = "1";
                    break;
            }

        }

        SetMeta(CurrentProduct.Meta, CurrentProduct.Name);

        productReviews.EntityType = EntityType.Product;
        productReviews.EntityId = ProductId;
        //Master.CurrencyChanged += (OnCurrencyChanged);
        int reviewsCount = ReviewService.GetReviewsCount(ProductId, EntityType.Product);
        if (reviewsCount > 0)
        {
            lReviewsCount.Text = string.Format("({0})", reviewsCount);
        }
        GetOffer();

    }

    private void GetOffer()
    {
        Offer offer = CurrentProduct.Offers[0];

        if (Convert.ToInt32(offer.Amount) == 0)
        {
            lAvailiableAmount.Text = string.Format("<span class=\"not-available\">{0}</span>", Resource.Client_Details_NotAvailable);
        }
        else
        {
            lAvailiableAmount.Text = string.Format("<span class=\"available\">{0}</span>", Resource.Client_Details_Available);
        }

        btnOrderByRequest.Visible = ((offer.Price > 0) && (offer.Amount <= 0) && CurrentProduct.OrderByRequest);
        btnAdd.Visible = CurrentProduct.Price > 0 && CurrentProduct.Amount > 0;
        btnAdd.Attributes["data-cart-add"] = CurrentProduct.ID.ToString();
        
    }
    
    protected void btnOrderByRequest_Click(object sender, EventArgs e)
    {
        Redirect("sendrequestonproduct.aspx?productid=" + CurrentProduct.ProductId, true);
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Redirect(string.Format("shoppingcart.aspx?productid={0}&amount={1}&attributesxml={2}",
                                CurrentProduct.ProductId,
                                txtAmount.Value,
                                HttpUtility.UrlEncode(CustomOptionsService.SerializeToXml(productCustomOptions.CustomOptions, productCustomOptions.SelectedOptions))), 
                true);
    }


}