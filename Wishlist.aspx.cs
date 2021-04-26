using System;
using System.Linq;
using AdvantShop;
using AdvantShop.Configuration;
using AdvantShop.Customers;
using AdvantShop.FilePath;
using AdvantShop.Orders;
using AdvantShop.Controls;
using AdvantShop.SEO;
using Resources;

public partial class Wishlist : AdvantShopPage
{
    protected CustomerGroup customerGroup = CustomerSession.CurrentCustomer.CustomerGroup;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!SettingsDesign.WishListVisibility)
            Response.Redirect("~/");
        
        SetMeta(new MetaInfo(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Client_Wishlist_Header)), string.Empty);            
    }


    protected void Page_PreRender(object sender, EventArgs e)
    {
        lvList.DataSource = ShoppingCartService.CurrentWishlist;
        lvList.DataBind();
    }

    protected void btnDeleteClick(object sender, EventArgs e)
    {
        int itemId = hfWishListItemID.Value.TryParseInt();
        var wishList = ShoppingCartService.CurrentWishlist;
        if (wishList.Any(item => item.ItemId == itemId))
        {
            ShoppingCartService.DeleteShoppingCartItem(itemId);
        }
    }

    protected void btnAddToCartClick(object sender, EventArgs e)
    {
        int itemId = hfWishListItemID.Value.TryParseInt();
        var wishListItem = ShoppingCartService.CurrentWishlist.Find(item=> item.ItemId  == itemId);
        if (wishListItem != null)
        {

            ShoppingCartService.AddShoppingCartItem(new ShoppingCartItem
                                                        {
                                                            EntityId = wishListItem.EntityId,
                                                            Amount = 1,
                                                            ShoppingCartType = ShoppingCartType.ShoppingCart,
                                                            AttributesXml =wishListItem.AttributesXml,
                                                            ItemType = EnumItemType.Product
                                                        });
            Response.Redirect("shoppingcart.aspx");
        }
    }

    protected string RenderPictureTag(string urlPhoto, string productName, string urlPath)
    {

        return string.Format("<a href=\"{0}\"><img src=\"{1}\" alt=\"{2}\" /></a>", urlPath,
                                  urlPhoto.IsNotEmpty()
                                      ? FoldersHelper.GetImageProductPath(ProductImageType.Small, urlPhoto, false)
                                      : "images/nophoto_small.jpg", productName);

    }
}