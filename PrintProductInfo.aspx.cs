//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop.Catalog;

public partial class PrintProductInfo : System.Web.UI.Page
{
    int _productId = -1;
    protected override void InitializeCulture()
    {
        AdvantShop.Localization.Culture.InitializeCulture();
    }

    private void MsgErr(string messageText)
    {
        lblError.Visible = true;
        lblError.Text += @"<br/>" + messageText + @"<br/>";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Request["productid"]))
            Response.Redirect("default.aspx");

        Int32.TryParse(Request["productid"], out _productId);
        if (_productId == -1) Response.Redirect("default.aspx");

        lblShopName.Text = AdvantShop.Configuration.SettingsMain.ShopName;

        Image1.ImageUrl = "~/images/print_logo.gif";

        pnlDescription.Visible = true;

        try
        {
            var product = ProductService.GetProduct(_productId);
            foreach (var p in product.ProductPhotos)
            {
                if (string.IsNullOrEmpty(p.Description))
                {
                    p.Description = Resources.Resource.Client_PrintProductInfo_FullSize;
                }
            }
            if (product.ProductPhotos.Count > 1)
            {
                DataListPhotos.DataSource = product.ProductPhotos;
                DataListPhotos.DataBind();
            }

            //var temp = new List<ProductProperty>(); //ProductService.GetProductProperties(productId);
            if (product.ProductPropertyValues.Count >= 1)
            {
                DataListProperties.DataSource = product.ProductPropertyValues;
                DataListProperties.DataBind();
            }

            lblProductName.Text = product.Name;
            Page.Title = product.Name;

            lblDescription.Text = product.Description;
            pnlDescription.Visible = string.IsNullOrEmpty(product.Description);
            imgMiddle.ImageUrl = product.Photo;
            pnlMidPic.Visible = string.IsNullOrEmpty(imgMiddle.ImageUrl);
            lblPrice.Text = string.Format("( {0} )", CatalogService.GetStringPrice(product.Offers[0].Price, product.Discount));

        }
        catch (Exception ex)
        {
            AdvantShop.Diagnostics.Debug.LogError(ex, ex.Message + " at Load");
            MsgErr(ex.Message + " at Load");
        }
    }
}