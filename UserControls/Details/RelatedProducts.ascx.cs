//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using AdvantShop.Catalog;
using AdvantShop.Customers;
using AdvantShop.FilePath;
using Resources;

public partial class UserControls_RelatedProducts : System.Web.UI.UserControl
{
    public List<int> ProductIds { get; set; }
    public bool HasProducts { get; private set; }
    public RelatedType RelatedType { get; set; }
    protected CustomerGroup customerGroup = CustomerSession.CurrentCustomer.CustomerGroup;
    
    public UserControls_RelatedProducts()
    {
        ProductIds = new List<int>();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (ProductIds.Count == 0)
        {
            return;
        }
        var finalList = new List<Product>();

        foreach (var id in ProductIds)
        {
            finalList.AddRange(ProductService.GetRelatedProducts(id, RelatedType).Where(product => !ProductIds.Contains(product.ProductId) && finalList.All(item => product.ID != item.ID)));
        }
        
        lvRelatedProducts.DataSource = finalList.Distinct();
        lvRelatedProducts.DataBind();
        HasProducts = lvRelatedProducts.Items.Any();
    }

    protected string RenderPictureTag(string urlPhoto, string productName, string photoDesc)
    {
        if (string.IsNullOrEmpty(urlPhoto))
            return "<img src=\"images/nophoto_small.jpg\" alt=\"\" />";
        return string.Format("<img src=\"{0}\" alt=\"{1}\" title=\"{1}\" />",
                             FoldersHelper.GetImageProductPath(ProductImageType.Small, urlPhoto, false),
                             HttpUtility.HtmlEncode(photoDesc));
    }

    protected string RenderPrice(decimal productPrice, decimal discount)
    {
        if (productPrice == 0)
        {
            return string.Format("<div class=\'price\'>{0}</div>", Resource.Client_Catalog_ContactWithUs);
        }

        string res;

        decimal price = ProductService.CalculateProductPrice(productPrice, discount, customerGroup, null, false);
        decimal priceWithDiscount = ProductService.CalculateProductPrice(productPrice, discount, customerGroup, null, true);

        //Changed by Evgeni to calculate final discount
        decimal groupDiscount = customerGroup.CustomerGroupId == 0 ? 0 : customerGroup.GroupDiscount;
        discount = Math.Max(discount, groupDiscount);
        //

        if (price.Equals(priceWithDiscount))
        {
            res = string.Format("<div class=\'price\'>{0}</div>", CatalogService.GetStringPrice(price));
        }
        else
        {
            res = string.Format("<div class=\"price-old\">{0}</div><div class=\"price\">{1}</div><div class=\"price-benefit\">{2} {3} {4} {5}% </div>",
                                CatalogService.GetStringPrice(productPrice),
                                CatalogService.GetStringPrice(priceWithDiscount),
                                Resource.Client_Catalog_Discount_Benefit,
                                CatalogService.GetStringPrice(price - priceWithDiscount),
                                Resource.Client_Catalog_Discount_Or,
                                CatalogService.FormatPriceInvariant(discount));
        }

        return res;
    }

}
