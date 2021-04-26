<%@ WebHandler Language="C#" Class="GetCart" %>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using AdvantShop;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Customers;
using AdvantShop.FilePath;
using AdvantShop.Orders;
using Newtonsoft.Json;


public class GetCart : IHttpHandler, IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        AdvantShop.Localization.Culture.InitializeCulture();
        context.Response.ContentType = "application/json";
        var shpCart = ShoppingCartService.CurrentShoppingCart;

        var cartProducts = (from item in shpCart
                            where item.ItemType == EnumItemType.Product
                            select new
                            {
                                Price = CatalogService.GetStringPrice(item.Price),
                                item.Amount,
                                SKU = item.Product.ArtNo,
                                Photo = item.Product.Photo.IsNotEmpty()
                                     ? String.Format("<img src=\"{0}\" alt=\"{1}\" class=\"img-cart\" />", FoldersHelper.GetImageProductPath(ProductImageType.XSmall, item.Product.Photo, false), item.Product.Name)
                                     : "<img src=\"images/nophoto_xsmall.jpg\" alt=\"\" class=\"img-cart\" />",
                                item.Product.Name,
                                //TODO vladimir: разделить получение ссылки для social
                                Link = context.Request.UrlReferrer != null && context.Request.UrlReferrer.ToString().Contains("/social/")
                                    ? "social/detailssocial.aspx?productid=" + item.Product.ID
                                    : UrlService.GetLink(ParamType.Product, item.Product.UrlPath, item.Product.ID),
                                Cost = CatalogService.GetStringPrice(item.Price * item.Amount),
                                item.ItemId,
                                SelectedOptions = CatalogService.RenderSelectedOptions(item.AttributesXml),
                                Avalible = GetAvalible(item),
                                OrderByRequest = item.Product.CanOrderByRequest,
                                MinAmount = item.Product.Offers.First().MinAmount ?? 1,
                                MaxAmount = item.Product.Offers.First().MaxAmount ?? Int32.MaxValue,
                                item.Product.Offers.First().Multiplicity
                            }).ToList();

        var certificate = (from item in shpCart
                           where item.ItemType == EnumItemType.Certificate
                           select new
                           {
                               Name = Resources.Resource.Client_MasterPage_Certificate,
                               SKU = "",
                               Photo = "<img src=\"images/giftcertificate/certifacate_xsmall.jpg\" alt=\"\" />",
                               Price = CatalogService.GetStringPrice(item.Price),
                               item.Amount,
                               Link = "javascript:open_printable_version('PrintCertificate.aspx?CertificateCode=" + item.Certificate.CertificateCode + "');",
                               Cost = CatalogService.GetStringPrice(item.Price * item.Amount),
                               item.ItemId,
                               SelectedOptions = ""
                           });

        var count = ItemsCount(shpCart);
        var totalPrice = shpCart.TotalPrice;
        var totalProductPrice = shpCart.TotalProductPrice;
        var discountOnTotalPrice = shpCart.DiscountPercentOnTotalPrice;
        var totalDiscount = shpCart.TotalDiscount;

        object objects = new
        {
            CartProducts = cartProducts,
            Certificate = certificate,
            TotalPrice = totalPrice,
            TotalProductPrice = totalProductPrice,
            DiscountOnTotalPrice = discountOnTotalPrice,
            TotalDiscount = totalDiscount,
            Summary = GetSummary(shpCart),
            Count = count,
            CountNumber = shpCart.TotalItems,
            Valid = Valid(context, shpCart),
            CouponInputVisible = shpCart.HasItems && shpCart.Coupon == null && shpCart.Certificate == null
                && CustomerSession.CurrentCustomer.CustomerGroup.CustomerGroupId == CustomerGroupService.DefaultCustomerGroup
        };

        context.Response.Write(JsonConvert.SerializeObject(objects));
    }

    public static string Valid(HttpContext context, ShoppingCart shpCart)
    {
        var errorMessage = string.Empty;
        var itemsCount = shpCart.TotalItems;
        var totalPrice = shpCart.TotalPrice;

        if (itemsCount == 0)
        {
            errorMessage = Resources.Resource.Client_ShoppingCart_NoProducts;
        }

        var cartProducts = (from item in shpCart
                            where item.ItemType == EnumItemType.Product
                            select new
                            {
                                Price = CatalogService.GetStringPrice(item.Price),
                                item.Amount,
                                SKU = item.Product.ArtNo,
                                Photo = item.Product.Photo.IsNotEmpty()
                                     ? String.Format("<img src=\"{0}\" alt=\"{1}\" class=\"img-cart\" />", FoldersHelper.GetImageProductPath(ProductImageType.XSmall, item.Product.Photo, false), item.Product.Name)
                                     : "<img src=\"images/nophoto_xsmall.jpg\" alt=\"\" class=\"img-cart\" />",
                                item.Product.Name,
                                //TODO vladimir: разделить получение ссылки для social
                                Link = context.Request.UrlReferrer != null && context.Request.UrlReferrer.ToString().Contains("/social/")
                                    ? "social/detailssocial.aspx?productid=" + item.Product.ID
                                    : UrlService.GetLink(ParamType.Product, item.Product.UrlPath, item.Product.ID),
                                Cost = CatalogService.GetStringPrice(item.Price * item.Amount),
                                item.ItemId,
                                SelectedOptions = CatalogService.RenderSelectedOptions(item.AttributesXml),
                                Avalible = GetAvalible(item),
                                OrderByRequest = item.Product.CanOrderByRequest
                            }).ToList();


        if (totalPrice < SettingsOrderConfirmation.MinimalOrderPrice)
        {
            errorMessage = string.Format(Resources.Resource.Client_ShoppingCart_MinimalOrderPrice,
                                         CatalogService.GetStringPrice(SettingsOrderConfirmation.MinimalOrderPrice),
                                         CatalogService.GetStringPrice(SettingsOrderConfirmation.MinimalOrderPrice - totalPrice));
        }
        else if (cartProducts.Any(item => item.Avalible.IsNotEmpty()))
        {
            errorMessage = string.Format(Resources.Resource.Client_ShoppingCart_NotAvailableProducts);
        }

        return errorMessage;
    }

    private static string ItemsCount(ShoppingCart shpCart)
    {
        int itemsCount = shpCart.TotalItems;
        return string.Format("{0} {1}", shpCart.TotalItems == 0 ? "" : itemsCount.ToString(CultureInfo.InvariantCulture), Strings.Numerals(itemsCount, Resources.Resource.Client_UserControls_ShoppingCart_Empty,
                                                  Resources.Resource.Client_UserControls_ShoppingCart_1Product,
                                                  Resources.Resource.Client_UserControls_ShoppingCart_2Products,
                                                  Resources.Resource.Client_UserControls_ShoppingCart_5Products));
    }

    private static List<object> GetSummary(ShoppingCart shpCart)
    {
        var summary = new List<object>();
        var totalPrice = shpCart.TotalPrice;
        var totalProductPrice = shpCart.TotalProductPrice;
        var discountOnTotalPrice = shpCart.DiscountPercentOnTotalPrice;
        var totalDiscount = shpCart.TotalDiscount;

        if (totalDiscount != 0)
        {
            summary.Add(new { Key = Resources.Resource.Client_UserControls_ShoppingCart_Sum, Value = CatalogService.GetStringPrice(totalPrice) });
        }

        if (discountOnTotalPrice > 0)
        {
            summary.Add(
                new
                    {
                        Key = Resources.Resource.Client_UserControls_ShoppingCart_Discount,
                        Value = string.Format("<span class=\"discount\">{0}</span>",
                                  CatalogService.GetStringDiscountPercent(totalProductPrice, discountOnTotalPrice, true))
                    });
        }

        if (shpCart.Certificate != null)
        {
            summary.Add(new { Key = Resources.Resource.Client_UserControls_ShoppingCart_Certificate, Value = string.Format("-{0}<a class=\"cross\" data-cart-remove-cert=\"true\" title=\"{1}\"></a>", CatalogService.GetStringPrice(shpCart.Certificate.Sum), Resources.Resource.Client_ShoppingCart_DeleteCertificate) });
        }

        if (shpCart.Coupon != null)
        {
            if (totalDiscount == 0)
            {
                summary.Add(
                    new
                        {
                            Key = Resources.Resource.Client_UserControls_ShoppingCart_Coupon,
                            Value = string.Format("-{0} ({1})<a class=\"cross\"  data-cart-remove-cupon=\"true\" title=\"{2}\"></a>",
                                      CatalogService.GetStringPrice(0), shpCart.Coupon.Code,
                                      Resources.Resource.Client_ShoppingCart_DeleteCoupon)
                        });
            }
            else
            {
                switch (shpCart.Coupon.Type)
                {
                    case CouponType.Fixed:
                        summary.Add(new
                                        {
                                            Key = Resources.Resource.Client_UserControls_ShoppingCart_Coupon,
                                            Value = string.Format("-{0} ({1})<a class=\"cross\" data-cart-remove-cupon=\"true\" title=\"{2}\"></a>",
                                                      CatalogService.GetStringPrice(totalDiscount), shpCart.Coupon.Code,
                                                      Resources.Resource.Client_ShoppingCart_DeleteCoupon)
                                        });
                        break;
                    case CouponType.Percent:
                        summary.Add(new
                                        {
                                            Key = Resources.Resource.Client_UserControls_ShoppingCart_Coupon,
                                            Value = string.Format("-{0} ({1}%) ({2})<a class=\"cross\"  data-cart-remove-cupon=\"true\" title=\"{3}\"></a>",
                                                        CatalogService.GetStringPrice(totalDiscount),
                                                        CatalogService.FormatPriceInvariant(shpCart.Coupon.Value),
                                                        shpCart.Coupon.Code, Resources.Resource.Client_ShoppingCart_DeleteCoupon)
                                        });
                        break;
                }
            }

        }

        summary.Add(new
                        {
                            Key = string.Format("<span class=\"sum-result\">{0}</span>",
                                      Resources.Resource.Client_UserControls_ShoppingCart_Total),
                            Value = string.Format("<span class=\"sum-result\">{0}</span>",
                                      CatalogService.GetStringPrice(totalPrice - totalDiscount > 0
                                                                        ? totalPrice - totalDiscount
                                                                        : 0))
                        });
        return summary;
    }


    private static string GetAvalible(ShoppingCartItem item)
    {
        if (item.Product.CanOrderByRequest)
        {
            return string.Empty;
        }
        if (!item.Product.Enabled || !item.Product.HirecalEnabled)
        {
            return Resources.Resource.Client_ShoppingCart_NotAvailable + " 0 " + item.Product.Offers[0].Unit;
        }

        if ((SettingsOrderConfirmation.AmountLimitation) && (item.Amount > item.Product.Offers[0].Amount))
        {
            return Resources.Resource.Client_ShoppingCart_NotAvailable + " " + item.Product.Amount + " " + item.Product.Offers[0].Unit;
        }

        if (item.Amount > item.Product.Offers[0].MaxAmount)
        {
            return Resources.Resource.Client_ShoppingCart_NotAvailable_MaximumOrder + " " + +item.Product.Offers[0].MaxAmount + " " + item.Product.Offers[0].Unit;
        }

        if (item.Amount < item.Product.Offers[0].MinAmount)
        {
            return Resources.Resource.Client_ShoppingCart_NotAvailable_MinimumOrder + " " + +item.Product.Offers[0].MinAmount + " " + item.Product.Offers[0].Unit;
        }

        return string.Empty;
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}
