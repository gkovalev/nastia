using System;
using System.Web.UI;
using AdvantShop.Configuration;
using Resources;

//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

public partial class Admin_ResizePhoto : Page
{
    protected override void InitializeCulture()
    {
        AdvantShop.Localization.Culture.InitializeCulture();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_ResizePhoto_Header);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        ResizeBrandPhotos.SaveData();

        ResizeCarouselPhotos.SaveData();
        
        ResizeCategoryPhotos.SaveData();

        ResizeNewsPhotos.SaveData();

        ResizeProductPhotos.SaveData();
    }
}
