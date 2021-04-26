//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Linq;
using AdvantShop;
using AdvantShop.Catalog;
using AdvantShop.CMS;
using AdvantShop.Configuration;
using AdvantShop.Controls;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Customers;
using AdvantShop.Modules;
using AdvantShop.Orders;
using Resources;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using AdvantShop.Payment;

public partial class Details : AdvantShopPage
{
    protected Product CurrentProduct;

    protected int ProductId
    {
        get { return Request["productid"].TryParseInt(); }

    }

    // Èíäåêñ ðåäàêòèðóåìîãî ýëåìåíòà
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

        BuyInOneClick.Visible = SettingsOrderConfirmation.BuyInOneClick;
        BuyInOneClick.ProductId = CurrentProduct.ProductId;
        BuyInOneClick.SelectedOptions = productCustomOptions.SelectedOptions;
        BuyInOneClick.CustomOptions = productCustomOptions.CustomOptions;

        if (CurrentProduct.Amount <= 0 || CurrentProduct.Price == 0)
        {
            divAmount.Visible = false;
        }

        CompareControl.Visible = SettingsCatalog.EnableCompareProducts;
        CompareControl.ProductId = ProductId;
        CompareControl.IsSelected = ShoppingCartService.CurrentCompare.Any(p => p.EntityId == ProductId);

        divUnit.Visible = CurrentProduct.Offers[0].Unit.IsNotEmpty();

        sbShareButtons.Visible = SettingsDesign.EnableSocialShareButtons;

        rating.ProductId = CurrentProduct.ID;
        rating.Rating = CurrentProduct.Ratio;
        rating.ShowRating = SettingsCatalog.EnableProductRating;
        rating.ReadOnly = RatingService.DoesUserVote(ProductId, CustomerSession.CustomerId);

        pnlWishlist.Visible = SettingsDesign.WishListVisibility;

        pnlSize.Visible = !string.IsNullOrEmpty(CurrentProduct.Size) && (CurrentProduct.Size != "0|0|0");
        //lblSize.Text = !string.IsNullOrEmpty(_product.Size) ? _product.Size.Replace("|", "x") : string.Empty;
        pnlWeight.Visible = CurrentProduct.Weight != 0;
        //lblWeight.Text = _product.Weight.ToString();
        pnlBrand.Visible = CurrentProduct.Brand != null;

        //Commented by Evgeni
        //productPropertiesView.ProductId = ProductId;
        //productPropertiesSetView.ProductId = ProductId;
        //productPropertiesAddedValueView.ProductId = ProductId;
        //ProductVideoView.ProductID = ProductId;

        //Need to pass to user control List of all product properties and in each set to divide this list into categories
        List<PropertyValue> productProperties = PropertyService.GetPropertyValuesByProductId(ProductId);
        productPropertiesView.ProductProperties = productProperties;
        productPropertiesSetView.ProductProperties = productProperties;
        productPropertiesSetView.ProductId = ProductId;
        productPropertiesSetView.ManufacteruArtNo = CurrentProduct.ManufactureArtNo;
        if (productProperties == null)
        {
            productPropertiesAddedValueView.ProductProperties = new List<PropertyValue>();
        }
        else
        {
            productPropertiesAddedValueView.ProductProperties = productProperties;
        }
        //Added by Evgeni for Stihl Viking
        if (CurrentProduct.Brand == null)
        { }
        else if ((CurrentProduct.Brand.Name.ToLower() == "stihl") || (CurrentProduct.Brand.Name.ToLower() == "viking"))
        {
            productPropertiesAddedValueView.ProductId = ProductId;
        }
        else
        {
            productPropertiesAddedValueView.ProductId = 0;
        }
        ProductVideoView.ProductID = ProductId;

        //Added By Evgeni to change long description into user friendly mode
        if ((CustomerSession.CurrentCustomer.CustomerGroup.GroupName == "Оптовик") || CustomerSession.CurrentCustomer.IsAdmin)
        {
            if (CurrentProduct.Offers.Where(t => t.OfferListId == 16).ToList().Count > 0 && CurrentProduct.Offers.Where(t => t.OfferListId == 16).FirstOrDefault().Price > 0)
            {
                lblSinglePriceTextForOptUser.Text = @"<font size='3'>  Розничная цена:  </font> <br />";
                lblOptPrice.Text = string.Format("<font size='3'>  Минимальная оптовая цена в евро:  </font> <br /> <div class='price'> {0} € </div>", CurrentProduct.Offers.Where(t => t.OfferListId == 16).FirstOrDefault().Price.ToString("F2"));
            }
        }
        DescriptionModificator();
        //////////////////////////////////////////////////////////

        productPhotoView.Product = CurrentProduct;
        relatedProducts.ProductIds.Add(ProductId);
        alternativeProducts.ProductIds.Add(ProductId);
        //Added By Evgeni for Bosch Related Products
        relatedBoschProducts.ProductIds.Add(ProductId);
        //

        breadCrumbs.Items = CategoryService.GetParentCategories(CurrentProduct.CategoryID).Reverse().Select(cat => new BreadCrumbs()
                                                                                                 {
                                                                                                     Name = cat.Name,
                                                                                                     Url = UrlService.GetLink(ParamType.Category, cat.UrlPath, cat.ID)
                                                                                                 }).ToList();
        breadCrumbs.Items.Insert(0, new BreadCrumbs()
                                        {
                                            Name = Resource.Client_MasterPage_MainPage,
                                            Url = UrlService.GetAbsoluteLink("/")
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

        ltrlRightColumnModules.Text = ModulesRenderer.RenderDetailsModulesToRightColumn();

        int reviewsCount = SettingsCatalog.ModerateReviews ? ReviewService.GetCheckedReviewsCount(ProductId, EntityType.Product) : ReviewService.GetReviewsCount(ProductId, EntityType.Product);
        if (reviewsCount > 0)
        {
            lReviewsCount.Text = string.Format("({0})", reviewsCount);
        }
    }

    //Added by Evgeni
    private void DescriptionModificator()
    {
        try
        {
            //Description Added By Evgeni 
            //on change need to change ExportFeedModuleYandex.cs
            // CurrentProduct.Description = CurrentProduct.Description.Replace(", ", "<br>");
            //  CurrentProduct.Description = string.Join("", Regex.Split(CurrentProduct.Description, @"<br>[a-zа-я]"));

            if (!CurrentProduct.Name.ToLower().Contains(CurrentProduct.Brand.Name.ToLower()))
            {
                if (CurrentProduct.Brand.Name.ToLower() == "bosch" && Regex.IsMatch(CurrentProduct.Name, @"[A-Z]"))
                {
                    CurrentProduct.Name = CurrentProduct.Name.Insert(Regex.Match(CurrentProduct.Name, @"[A-Z]").Index, CurrentProduct.Brand.Name + ' ');
                }
                else
                {
                    CurrentProduct.Name = CurrentProduct.Brand.Name + " " + CurrentProduct.Name;
                }
            }
            if (!CurrentProduct.Name.ToLower().Contains(CurrentProduct.ArtNo.Replace(".", "").Replace("-", "").ToLower()))
            {
                CurrentProduct.Name = CurrentProduct.Name + " [" + CurrentProduct.ArtNo.Replace(".", "").Replace("-", "") + "]"; 
            }

            //Added by Evgeni for Stihl and Viking
            if ((CurrentProduct.Brand.Name.ToLower() == "stihl") || (CurrentProduct.Brand.Name.ToLower() == "viking"))
            {
                CurrentProduct.Description = CurrentProduct.Description.Replace(".", ". <br>");
            }
            else if (CurrentProduct.Brand.Name.ToLower() == "bosch" || CurrentProduct.Brand.Name.ToLower() == "skil" || CurrentProduct.Brand.Name.ToLower() == "dremel" || CurrentProduct.Brand.Name.ToLower().Contains("berger"))
            {
                string[] stringSeparators = new string[] { ", ", "; " };
                string[] description = CurrentProduct.Description.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 1; i < description.Length; i++)
                {
                    if (Regex.IsMatch(description[i], @"^[a-zа-я]"))
                    {
                        if (description[i - 1] == string.Empty)
                        {
                            for (int t = i - 2; t > 0; t--)
                            {
                                if (description[t] != string.Empty)
                                {
                                    description[t] = description[t] + "," + description[i];
                                    break;
                                }
                            }
                        }
                        else
                        {
                            description[i - 1] = description[i - 1] + "," + description[i];
                        }
                        description[i] = string.Empty;
                    }

                }
                CurrentProduct.Description = string.Join("<br>", description.Where(p => !p.Equals(string.Empty)));
            }
           
  
        }
        catch { }
    }

    private void GetOffer()
    {
        Offer offer = CurrentProduct.Offers[0];

        if (offer.Amount <= 0)
        {
            lAvailiableAmount.Text = string.Format("<span class=\"not-available\">{0}</span>", Resource.Client_Details_NotAvailable);
        }
        else
        {
            lAvailiableAmount.Text = string.Format("<span class=\"available\">{0}</span>", Resource.Client_Details_Available);
        }

        btnOrderByRequest.Visible = ((offer.Price > 0) && (offer.Amount <= 0) && CurrentProduct.OrderByRequest);

        btnAdd.Visible = CurrentProduct.Price > 0 && CurrentProduct.Amount > 0;
        btnAdd.Attributes["data-cart-add"] = ProductId.ToString();
        btnAdd.Rel = string.Format("productid:{0}", CurrentProduct.ID);

        BuyInOneClick.Visible = SettingsOrderConfirmation.BuyInOneClick && CurrentProduct.Price > 0 && CurrentProduct.Amount > 0;
        btnAddCredit.Visible = false;

        if (btnAdd.Visible && CurrentProduct.Price > 3000)
        {
            var payment = PaymentService.GetPaymentMethodByType(PaymentType.Kupivkredit);

            if (payment != null && payment.Enabled)
            {
                btnAddCredit.Visible = true;
                btnAddCredit.Attributes["data-cart-add"] = ProductId.ToString();
                btnAddCredit.Attributes["data-cart-paiment"] = payment.PaymentMethodID.ToString();
                btnAddCredit.Rel = string.Format("productid:{0}", CurrentProduct.ID);
            }
        }
    }

    protected void btnOrderByRequest_Click(object sender, EventArgs e)
    {
        Redirect("/SendRequestOnProduct.aspx?productid=" + CurrentProduct.ProductId, true);
    }

    protected void btnAddToBasket_Click(object sender, EventArgs e)
    {
        if (!productCustomOptions.IsValid)
        {
            productCustomOptions.ShowValidation = true;
            return;
        }

        if (IsEditItem)
        {
            var returnUrl = "";
            var item = ShoppingCartService.GetShoppingCart(ItemType)[ItemIndex];
            item.AttributesXml = CustomOptionsService.SerializeToXml(productCustomOptions.CustomOptions, productCustomOptions.SelectedOptions);

            switch (ItemType)
            {
                case ShoppingCartType.ShoppingCart:
                    //item.Amount = Int32.Parse(txtAmount.Text);
                    returnUrl = "/shoppingcart.aspx";
                    break;

                case ShoppingCartType.Wishlist:
                    item.Amount = 1;
                    returnUrl = "/wishlist.aspx";
                    break;
            }

            ShoppingCartService.UpdateShoppingCartItem(item);
            Redirect(returnUrl);
        }
        else
        {
            ShoppingCartService.AddShoppingCartItem(new ShoppingCartItem
                {
                    EntityId = ProductId,
                    //Amount = Convert.ToInt32(txtAmount.Text),
                    ShoppingCartType = ShoppingCartType.ShoppingCart,
                    AttributesXml = CustomOptionsService.SerializeToXml(productCustomOptions.CustomOptions, productCustomOptions.SelectedOptions),
                    ItemType = EnumItemType.Product
                });

            Redirect("/shoppingcart.aspx");
        }
    }

    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        if (CustomerSession.CurrentCustomer.IsAdmin)
        {
            hrefAdmin.HRef = GetAbsoluteLink("/admin/Product.aspx?ProductID=" + Request["productid"] +
                             (string.IsNullOrEmpty(Request["categoryid"])
                                  ? string.Empty
                                  : "&categoryid=" + Request["categoryid"]));
            hrefAdmin.Visible = true;
        }

        //Changed By Evgeni to change product name in Static Blok
        try
        {
            Literal footerText = (Literal) stFooterOnDetails.FindControl("ltOutputString");
            footerText.Text = footerText.Text.Replace("#PRODUCT_NAME#", CurrentProduct.Name); 
        }
        catch { }

        GetOffer();
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {

        if (ShoppingCartService.CurrentWishlist.Any(item => item.EntityId == ProductId &&
                    item.AttributesXml == CustomOptionsService.SerializeToXml(productCustomOptions.CustomOptions, productCustomOptions.SelectedOptions)))
        {
            lbtnWishlist.Visible = false;
            alreadyWishList.Visible = true;
        }
        else
        {
            lbtnWishlist.Visible = true;
            alreadyWishList.Visible = false;
        }

        // Meta info ------------------------------------------------------------------------------------------
        //  tabReviews.HeaderText = Reviews.ReviewsCount;
    }

    protected void AddToWishList_Click(object sender, EventArgs e)
    {
        if (!productCustomOptions.IsValid)
        {
            productCustomOptions.ShowValidation = true;
            return;
        }

        ShoppingCartService.AddShoppingCartItem(new ShoppingCartItem
                                                    {
                                                        EntityId = ProductId,
                                                        //Amount = Convert.ToInt32(txtAmount.Text),
                                                        ShoppingCartType = ShoppingCartType.Wishlist,
                                                        AttributesXml = CustomOptionsService.SerializeToXml(productCustomOptions.CustomOptions, productCustomOptions.SelectedOptions)
                                                    });
        lbtnWishlist.Visible = false;
        alreadyWishList.Visible = true;

        Redirect("wishlist.aspx");
    }

    protected string RenderSpinBox()
    {
        return
            string.Format(
                "<input data-plugin=\"spinbox\" type=\"text\" id=\"txtAmount\" value=\"{0}\" data-spinbox-options=\"{{min:{0},max:{1},step:{2}}}\"/>",
                CurrentProduct.Offers.First().MinAmount ?? 1,
                CurrentProduct.Offers.First().MaxAmount ?? Int32.MaxValue,
                CurrentProduct.Offers.First().Multiplicity);
    }
}