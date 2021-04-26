using System;
using System.Linq;
using System.Web.UI;
using AdvantShop.Catalog;

public partial class UserControls_ProductPhotoView : UserControl
{
    public Product Product { set; get; }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Product.ProductPhotos.Any())
        {
            lvFancyBox.DataSource = Product.ProductPhotos;
            lvFancyBox.DataBind();

            lvPhotos.DataSource = Product.ProductPhotos;
            lvPhotos.DataBind();

            carouselDetails.Visible = lvPhotos.Items.Count > 1;
            pnlPhoto.Visible = true;
            pnlNoPhoto.Visible = false;

            //Show 3 years warranty
            if (Product.ProductPhotos[0].PhotoName.ToLower().Contains("bosch_blue_pt"))
            {
                imgWarranty.Visible = true;
            }
            else if (Product.BrandId == 47)
            {
                imgDremel.Visible = true;
            }
        }
        else
        {
            pnlPhoto.Visible = false;
            pnlNoPhoto.Visible = true;
        }
    }
}