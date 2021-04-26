//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop;
using AdvantShop.Configuration;
using AdvantShop.Controls;
using AdvantShop.Helpers;

public partial class _Default : AdvantShopPage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        SettingsDesign.eMainPageMode currentMode = !Demo.IsDemoEnabled || !CommonHelper.GetCookieString("structure").IsNotEmpty()
                      ? SettingsDesign.MainPageMode
                      : (SettingsDesign.eMainPageMode)Enum.Parse(typeof(SettingsDesign.eMainPageMode), CommonHelper.GetCookieString("structure"));

        switch (currentMode)
        {
            case SettingsDesign.eMainPageMode.Default:
                mvDefaultPage.SetActiveView(defaultView);
                news.Visible = SettingsDesign.NewsVisibility;
                mainPageProduct.Visible = SettingsDesign.MainPageProductsVisibility;
                //Voting remareked by Evgeni
              //  voting.Visible = SettingsDesign.VotingVisibility;
                checkOrder.Visible = SettingsDesign.CheckOrderVisibility;
                giftCertificate.Visible = SettingsDesign.GiftSertificateVisibility && SettingsOrderConfirmation.EnableGiftCertificateService;
                BoschCarousel1.Visible = SettingsDesign.CarouselVisibility;
                break;
            case SettingsDesign.eMainPageMode.TwoColumns:
                mvDefaultPage.SetActiveView(twoColumnsView);
                newsTwoColumns.Visible = SettingsDesign.NewsVisibility;
                votingTwoColumns.Visible = SettingsDesign.VotingVisibility;
                checkOrderTwoColumns.Visible = SettingsDesign.CheckOrderVisibility;
                giftCertificateTwoColumns.Visible = SettingsDesign.GiftSertificateVisibility && SettingsOrderConfirmation.EnableGiftCertificateService;
                carouselTwoColumns.Visible = SettingsDesign.CarouselVisibility;
                mainPageProductTwoColumns.Visible = SettingsDesign.MainPageProductsVisibility;
                break;
            case SettingsDesign.eMainPageMode.ThreeColumns:
                mvDefaultPage.SetActiveView(threeColumnsView);
                newsThreeColumns.Visible = SettingsDesign.NewsVisibility;
                votingThreeColumns.Visible = SettingsDesign.VotingVisibility;
                CheckOrderThreeColumns.Visible = SettingsDesign.CheckOrderVisibility;
                giftCertificateThreeColumns.Visible = SettingsDesign.GiftSertificateVisibility && SettingsOrderConfirmation.EnableGiftCertificateService;
                carouselThreeColumns.Visible = SettingsDesign.CarouselVisibility;
                mainPageProductThreeColumn.Visible = SettingsDesign.MainPageProductsVisibility;
                break;
        }
        
        SetMeta(null, string.Empty);
    }
}
