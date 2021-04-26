//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using AdvantShop;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Controls;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Customers;
using AdvantShop.Modules;
using AdvantShop.Modules.Interfaces;
using AdvantShop.Orders;
using AdvantShop.SEO;
using Resources;

public partial class ShoppingCart_Page : AdvantShopPage
{
    protected CustomerGroup customerGroup;

    protected void Page_Load(object sender, EventArgs e)
    {
        customerGroup = CustomerSession.CurrentCustomer.CustomerGroup;
        lDemoWarning.Visible = Demo.IsDemoEnabled || Trial.IsTrialEnabled;

        if (!IsPostBack)
        {
            if(Request["productid"].IsNotEmpty())
            {
                int productId = Request["productid"].TryParseInt();
                int amount = Request["amount"].TryParseInt(1);
                if(productId != 0 && ProductService.IsProductEnabled(productId))
                {
                    IList<EvaluatedCustomOptions> listOptions = null;
                    string selectedOptions = HttpUtility.UrlDecode(Request["AttributesXml"]);
                    try
                    {
                        listOptions = CustomOptionsService.DeserializeFromXml(selectedOptions);
                    }
                    catch (Exception)
                    {
                        listOptions = null;
                    }

                    if (CustomOptionsService.DoesProductHaveRequiredCustomOptions(productId) && listOptions == null)
                    {
                        Response.Redirect(SettingsMain.SiteUrl + UrlService.GetLinkDB(ParamType.Product, productId));
                        return;
                    }

                    ShoppingCartService.AddShoppingCartItem(new ShoppingCartItem
                    {
                        EntityId = productId,
                        Amount = amount,
                        ShoppingCartType = ShoppingCartType.ShoppingCart,
                        AttributesXml = listOptions != null ? selectedOptions : string.Empty,
                        ItemType = EnumItemType.Product
                    });

                    Response.Redirect("shoppingcart.aspx");
                }
            }

            UpdateBasket();
            SetMeta(new MetaInfo(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Client_ShoppingCart_ShoppingCart)), string.Empty);
        }
        //relatedProducts.ProductIds = ShoppingCartService.CurrentShoppingCart.Where(p => p.ItemType == ShoppingCartItem.EnumItemType.Product).Select(p => p.EntityId).ToList();
    }



    public void UpdateBasket()
    {
        var shpCart = ShoppingCartService.CurrentShoppingCart;

        if (shpCart.HasItems)
        {
            lblEmpty.Visible = false;
        }
        else
        {
            dvOrderMerged.Visible = false;
            lblEmpty.Visible = true;
        }
    }
    
 
    protected void btnConfirmOrder_Click(object sender, EventArgs e)
    {
        var shoppingCart = ShoppingCartService.CurrentShoppingCart;

        if (!shoppingCart.CanOrder)
        {
            UpdateBasket();
        }
        else
        {
            Response.Redirect("orderconfirmation.aspx");
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        UpdateBasket();
    }
}