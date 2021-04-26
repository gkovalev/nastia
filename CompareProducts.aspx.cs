using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Controls;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Orders;
using AdvantShop.SEO;
using Resources;

public partial class CompareProducts_Page : AdvantShopPage
{
    protected List<ProductItem> ProductItems = new List<ProductItem>();
    protected List<string> PropertyNames = new List<string>();

    protected void Page_Load(object sender, EventArgs e)
    {

        SetMeta(new MetaInfo(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Client_CompareProducts_Header)), string.Empty);
        Logo.ImgSource = FoldersHelper.GetPath(FolderType.Pictures, SettingsMain.LogoImageName, false);

        var compareProducts = ShoppingCartService.CurrentCompare;
        if ((compareProducts == null) || (compareProducts.Count == 0))
        {
            Response.Redirect("default.aspx");
            return;
        }

        var propertyNames = new List<string>();
        foreach (var item in compareProducts)
        {
            propertyNames.AddRange(PropertyService.GetPropertyValuesByProductId(item.EntityId).Select(p => p.Property.Name));
        }

        PropertyNames = new List<string>();
        PropertyNames.AddRange(propertyNames.Distinct());

        ProductItems = new List<ProductItem>();
        foreach (ShoppingCartItem item in compareProducts)
        {
            Product product = ProductService.GetProduct(item.EntityId);
            if (product == null) continue;
            ProductItems.Add(new ProductItem(product, PropertyNames));
        }
    }

    protected string RenderPictureTag(ProductItem item)
    {
        string strFormat = "";
        string strResult = "";

        if (string.IsNullOrEmpty(item.Photo))
        {
            strFormat =
                "<div><div onclick=\"location='{0}'\" style=\"cursor:pointer;position:relative;height:90px;width:120px\" title=\"Фотография не доступна\"><img border=\"0\" src=\"images/nophoto_small.jpg\" alt=\"Фотография не доступна\" /></div></div><br/>";
            strResult = string.Format(strFormat, UrlService.GetLinkDB(ParamType.Product, item.ProductId));
        }
        else
        {
            strFormat =
                "<div><div onclick=\"location='{4}'\" style=\"cursor:pointer;position:relative;height:90px;width:120px\" title=\"{0}\"><img class=\"imgPhoto\" border=\"0\" src=\"{1}\" alt=\"{0}\" /></div></div><br/>";
            strResult = string.Format(strFormat, Server.HtmlEncode(item.Name),
                                      FoldersHelper.GetImageProductPath(ProductImageType.Small, item.Photo, false),
                                      item.Photo, "", UrlService.GetLinkDB(ParamType.Product, item.ProductId));

        }
        return strResult;
    }

    protected string GetProductLink(ProductItem item)
    {
        return UrlService.GetLinkDB(ParamType.Product, item.ProductId);
    }

    protected string RenderPrice(decimal price, decimal discount)
    {
        if (price == 0)
        {
            return "<span class=\'price\'>" + Resource.Client_Catalog_ContactWithUs + "</span><br />";
        }

        string res;

        string priceLiteral = CatalogService.GetStringPrice(price);

        if (discount == 0)
        {
            res = "<span class=\'price\'>" + priceLiteral + "</span><br />";
        }
        else
        {
            string priceWithDiscountLiteral =
                CatalogService.GetStringPrice(price - price * discount / 100);

            res =
                string.Format(
                    "<span class=\"OldPrice\">{0}</span><br /><span class=\"PriceWithDiscount\">{1}</span><br /><div class=\"Discount\">" +
                    Resource.Client_Catalog_Discount + " {2}%</div>", priceLiteral, priceWithDiscountLiteral, discount);
        }

        return res;
    }

    protected void btnDeleteProduct_Click(object sender, EventArgs e)
    {
        int productId = 0;
        Int32.TryParse(hiddenProductID.Value, out productId);

        if (productId != 0 && ProductService.IsExists(productId))
        {
            ShoppingCart compareProducts = ShoppingCartService.CurrentCompare;
            ShoppingCartItem deleteItem = compareProducts.FirstOrDefault(p => p.EntityId == productId);

            if (deleteItem != null)
            {
                ShoppingCartService.DeleteShoppingCartItem(deleteItem.ItemId);
                compareProducts.RemoveAll(p => p.ItemId == productId);

                if (compareProducts.Count == 0)
                {
                    CommonHelper.RegCloseScript(this, string.Empty);
                }
            }
        }
    }

    protected void btnBuyProduct_Click(object sender, EventArgs e)
    {
        try
        {
            int productId = 0;
            Int32.TryParse(hiddenProductID.Value, out productId);

            if (CustomOptionsService.DoesProductHaveRequiredCustomOptions(productId))
            {
                Response.Redirect(GetProductLink(ProductItems.First(p => p.ProductId == productId)));
            }


            List<CustomOption> customOptions = CustomOptionsService.GetCustomOptionsByProductId(Convert.ToInt32(productId));
            if ((customOptions != null) && (customOptions.Count != 0))
            {
                Page.Response.Redirect(UrlService.GetLinkDB(ParamType.Product, productId));
            }

            ShoppingCartService.AddShoppingCartItem(new ShoppingCartItem
                                                        {
                                                            EntityId = productId,
                                                            Amount = 1,
                                                            ShoppingCartType = ShoppingCartType.ShoppingCart,
                                                            AttributesXml = string.Empty,
                                                            ItemType = EnumItemType.Product
                                                        });

        }
        catch(ThreadAbortException) { }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }

        Page.Response.Redirect(GetAbsoluteLink("/shoppingcart.aspx"), true);
    }
}

public class ProductItem
{
    public ProductItem(Product product, IEnumerable<string> propertyNames)
    {
        ProductId = product.ProductId;
        CategoryId = ProductService.GetFirstCategoryIdByProductId(ProductId);
        Name = product.Name;
        ArtNo = product.ArtNo;
        Photo = product.Photo;
        //changed by Evgeni
       // Price = OfferService.GetOfferPrice(product.ID, CatalogService.DefaultOfferListId);
        var firstOrDefault = product.Offers.FirstOrDefault();
        if (firstOrDefault != null)
        {
            Price = firstOrDefault.Price;
            Amount = firstOrDefault.Amount;
        }


        Discount = product.Discount;

        Properties = new List<ProductProperty>();

        var properties = PropertyService.GetPropertyValuesByProductId(product.ProductId);
        foreach (var propertyName in propertyNames)
        {
            if (properties.Count(p => p.Property.Name == propertyName) > 0)
            {
                Properties.Add(new ProductProperty
                                   {
                                       Name = propertyName,
                                       //Changed By Evgeni to insert different values for same propertie
                                       //  Value = properties.Find(p => p.Property.Name == propertyName).Value
                                       Value = string.Join("<br>", properties.Where(t => t.Property.Name.Equals(propertyName)).Select(p => p.Value).ToList())

                                   });
            }
            else
            {
                Properties.Add(new ProductProperty
                                   {
                                       Name = propertyName,
                                       Value = " - "
                                   });
            }
        }
    }

    public int ProductId { get; set; }
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public string ArtNo { get; set; }
    public string Photo { get; set; }
    public decimal Price { get; set; }
    public decimal Discount { get; set; }
    public List<ProductProperty> Properties { get; set; }
    //Added by Evgeni to resolve defect with adding products with 0 price or without amoun to the basket
    public int Amount { get; set; }
}