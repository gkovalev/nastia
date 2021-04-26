//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;

namespace AdvantShop.Configuration
{
    public class SettingsDesign
    {
        public enum eSearchBlockLocation
        {
            None = 0,
            TopMenu = 1,
            CatalogMenu = 2
        }
        
        public enum eMainPageMode
        {
            Default = 0,
            TwoColumns = 1,
            ThreeColumns = 2
        }

        public static string Theme
        {
            get { return SettingProvider.Items["Theme"]; }
            set { SettingProvider.Items["Theme"] = value.ToString(); }
        }

        public static string ColorScheme
        {
            get { return SettingProvider.Items["ColorScheme"]; }
            set { SettingProvider.Items["ColorScheme"] = value.ToString(); }
        }

        public static string BackGround
        {
            get { return SettingProvider.Items["BackGround"]; }
            set { SettingProvider.Items["BackGround"] = value.ToString(); }
        }

        public static eSearchBlockLocation SearchBlockLocation
        {
            get { return (eSearchBlockLocation)Convert.ToInt32(SettingProvider.Items["SearchBlockLocation"]); }
            set { SettingProvider.Items["SearchBlockLocation"] = ((int)value).ToString(); }
        }

        public static eMainPageMode MainPageMode
        {
            get { return (eMainPageMode)Convert.ToInt32(SettingProvider.Items["MainPageMode"]); }
            set { SettingProvider.Items["MainPageMode"] = ((int)value).ToString(); }
        }
        
        public static string CarouselAnimation
        {
            get { return SettingProvider.Items["CarouselAnimation"]; }
            set { SettingProvider.Items["CarouselAnimation"] = value; }
        }

        public static int CarouselAnimationSpeed
        {
            get
            {
                int intTempResult = -1;
                Int32.TryParse(SettingProvider.Items["CarouselAnimationSpeed"], out intTempResult);
                return intTempResult;
            }
            set { SettingProvider.Items["CarouselAnimationSpeed"] = value.ToString(); }
        }

        public static int CarouselAnimationDelay
        {
            get
            {
                int intTempResult = -1;
                Int32.TryParse(SettingProvider.Items["CarouselAnimationDelay"], out intTempResult);
                return intTempResult;
            }
            set { SettingProvider.Items["CarouselAnimationDelay"] = value.ToString(); }
        }

        public static bool ShoppingCartVisibility
        {
            get { return Convert.ToBoolean(SettingProvider.Items["ShowShoppingCartOnMainPage"]); }
            set { SettingProvider.Items["ShowShoppingCartOnMainPage"] = value.ToString(); }
        }


        public static bool RecentlyViewVisibility
        {
            get { return Convert.ToBoolean(SettingProvider.Items["ShowSeeProductOnMainPage"]); }
            set { SettingProvider.Items["ShowSeeProductOnMainPage"] = value.ToString(); }
        }
        
        public static bool NewsVisibility
        {
            get { return Convert.ToBoolean(SettingProvider.Items["ShowNewsOnMainPage"]); }
            set { SettingProvider.Items["ShowNewsOnMainPage"] = value.ToString(); }
        }

        public static bool NewsSubscriptionVisibility
        {
            get { return Convert.ToBoolean(SettingProvider.Items["ShowNewsSubscriptionOnMainPage"]); }
            set { SettingProvider.Items["ShowNewsSubscriptionOnMainPage"] = value.ToString(); }
        }

        public static bool VotingVisibility
        {
            get { return Convert.ToBoolean(SettingProvider.Items["ShowVotingOnMainPage"]); }
            set { SettingProvider.Items["ShowVotingOnMainPage"] = value.ToString(); }
        }

        public static bool CheckOrderVisibility
        {
            get { return Convert.ToBoolean(SettingProvider.Items["ShowStatusCommentOnMainPage"]); }
            set { SettingProvider.Items["ShowStatusCommentOnMainPage"] = value.ToString(); }
        }

        public static bool FilterVisibility
        {
            get { return Convert.ToBoolean(SettingProvider.Items["ShowFilterInCatalog"]); }
            set { SettingProvider.Items["ShowFilterInCatalog"] = value.ToString(); }
        }
        
        public static bool CurrencyVisibility
        {
            get { return Convert.ToBoolean(SettingProvider.Items["ShowCurrencyOnMainPage"]); }
            set { SettingProvider.Items["ShowCurrencyOnMainPage"] = value.ToString(); }
        }

        public static bool MainPageProductsVisibility
        {
            get { return Convert.ToBoolean(SettingProvider.Items["ShowMainPageProductsOnMainPage"]); }
            set { SettingProvider.Items["ShowMainPageProductsOnMainPage"] = value.ToString(); }
        }

        public static bool FootMenuSiteMapVisibility
        {
            get { return Convert.ToBoolean(SettingProvider.Items["FootMenuSiteMap"]); }
            set { SettingProvider.Items["FootMenuSiteMap"] = value.ToString(); }
        }
        
        public static bool GiftSertificateVisibility
        {
            get { return Convert.ToBoolean(SettingProvider.Items["GiftSertificateBlock"]); }
            set { SettingProvider.Items["GiftSertificateBlock"] = value.ToString(); }
        }

        public static bool WishListVisibility
        {
            get { return Convert.ToBoolean(SettingProvider.Items["WishList"]); }
            set { SettingProvider.Items["WishList"] = value.ToString(); }
        }

        public static bool CarouselVisibility
        {
            get { return Convert.ToBoolean(SettingProvider.Items["CarouseltVisibility"]); }
            set { SettingProvider.Items["CarouseltVisibility"] = value.ToString(); }
        }

        public static int CountLineOnMainPage
        {
            get
            {
                int intTempResult = 1;
                Int32.TryParse(SettingProvider.Items["CountLineOnMainPage"], out intTempResult);
                return intTempResult;
            }
            set { SettingProvider.Items["CountLineOnMainPage"] = value.ToString(); }
        }

        public static bool EnableZoom
        {
            get { return Convert.ToBoolean(SettingProvider.Items["EnabledZoom"]); }
            set { SettingProvider.Items["EnabledZoom"] = value.ToString(); }
        }

        public static bool EnableSocialShareButtons
        {
            get { return Convert.ToBoolean(SettingProvider.Items["EnableSocialShareButtons"]); }
            set { SettingProvider.Items["EnableSocialShareButtons"] = value.ToString(); }
        }
    }
}