using System;

using AdvantShop;
using AdvantShop.Configuration;
using Resources;

public partial class Admin_UserControls_Settings_DesignSettings : System.Web.UI.UserControl
{
    public string ErrMessage = Resources.Resource.Admin_CommonSettings_InvalidDesign;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            LoadData();
    }

    private void LoadData()
    {
        CheckBoxSeeProduct.Checked = SettingsDesign.RecentlyViewVisibility;
        chkEnableZoom.Checked = SettingsDesign.EnableZoom;
        CheckBoxShowFilter.Checked = SettingsDesign.FilterVisibility;
        CheckBoxNews.Checked = SettingsDesign.NewsVisibility;
        CheckBoxStatusComment.Checked = SettingsDesign.CheckOrderVisibility;
        CheckBoxNewsSubscription.Checked = SettingsDesign.NewsSubscriptionVisibility;
        CheckBoxVoting.Checked = SettingsDesign.VotingVisibility;
        CheckBoxCurrency.Checked = SettingsDesign.CurrencyVisibility;
        CheckBoxGiftCertificate.Checked = SettingsDesign.GiftSertificateVisibility;
        CheckBoxWishList.Checked = SettingsDesign.WishListVisibility;
        chkMainPageProducts.Checked = SettingsDesign.MainPageProductsVisibility;
        chkEnableSocialShareButtons.Checked = SettingsDesign.EnableSocialShareButtons;
        txtCountLineOnMainPage.Text = SettingsDesign.CountLineOnMainPage.ToString();

        spanAlert.Visible = SettingsDesign.FilterVisibility;

        ddlCarouselAnimation.SelectedValue = SettingsDesign.CarouselAnimation;
        txtCarouselAnimationSpeed.Text = SettingsDesign.CarouselAnimationSpeed.ToString();
        txtCarouselAnimationDelay.Text = SettingsDesign.CarouselAnimationDelay.ToString();
        CheckBoxCarousel.Checked = SettingsDesign.CarouselVisibility;
        ddlSearchBlockLocation.SelectedValue = ((int)SettingsDesign.SearchBlockLocation).ToString();


        ddlMainPageMode.SelectedValue = ((int)SettingsDesign.MainPageMode).ToString();
    }

    public bool SaveData()
    {
        if (!ValidateData())
            return false;

        SettingsDesign.RecentlyViewVisibility = CheckBoxSeeProduct.Checked;
        SettingsDesign.EnableZoom = chkEnableZoom.Checked;
        SettingsDesign.FilterVisibility = CheckBoxShowFilter.Checked;
        SettingsDesign.NewsVisibility = CheckBoxNews.Checked;
        SettingsDesign.CheckOrderVisibility = CheckBoxStatusComment.Checked;
        SettingsDesign.NewsSubscriptionVisibility = CheckBoxNewsSubscription.Checked;
        SettingsDesign.VotingVisibility = CheckBoxVoting.Checked;
        SettingsDesign.CurrencyVisibility = CheckBoxCurrency.Checked;
        SettingsDesign.GiftSertificateVisibility = CheckBoxGiftCertificate.Checked;
        SettingsDesign.WishListVisibility = CheckBoxWishList.Checked;
        SettingsDesign.MainPageProductsVisibility = chkMainPageProducts.Checked;
        SettingsDesign.EnableSocialShareButtons = chkEnableSocialShareButtons.Checked;
        SettingsDesign.CountLineOnMainPage = txtCountLineOnMainPage.Text.TryParseInt();

        SettingsDesign.CarouselAnimation = ddlCarouselAnimation.SelectedValue;
        SettingsDesign.CarouselAnimationSpeed = txtCarouselAnimationSpeed.Text.TryParseInt();
        SettingsDesign.CarouselAnimationDelay = txtCarouselAnimationDelay.Text.TryParseInt();
        SettingsDesign.CarouselVisibility = CheckBoxCarousel.Checked;

        SettingsDesign.SearchBlockLocation = (SettingsDesign.eSearchBlockLocation)ddlSearchBlockLocation.SelectedValue.TryParseInt();
        SettingsDesign.MainPageMode = (SettingsDesign.eMainPageMode)ddlMainPageMode.SelectedValue.TryParseInt();

        AdvantShop.Core.Caching.CacheManager.Remove("MenuCatalog0");

        LoadData();
        return true;
    }

    private bool ValidateData()
    {
        int ti;

        if (!int.TryParse(txtCarouselAnimationDelay.Text, out ti))
        {
            ErrMessage = Resource.Admin_CommonSettings_NoCarouselAnimationDelay;
            return false;
        }

        if (!int.TryParse(txtCarouselAnimationSpeed.Text, out ti))
        {
            ErrMessage = Resource.Admin_CommonSettings_NoCarouselAnimationSpeed;
            return false;
        }
        return true;
    }
}