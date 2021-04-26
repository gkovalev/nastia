//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;

namespace AdvantShop.Configuration
{
    public class SettingsPictureSize
    {
        public static int NewsImageWidth
        {
            get { return Convert.ToInt32(SettingProvider.Items["NewsImageWidth"]); }
            set { SettingProvider.Items["NewsImageWidth"] = value.ToString(); }
        }

        public static int NewsImageHeight
        {
            get { return Convert.ToInt32(SettingProvider.Items["NewsImageHeight"]); }
            set { SettingProvider.Items["NewsImageHeight"] = value.ToString(); }
        }

        public static int BrandLogoWidth
        {
            get { return Convert.ToInt32(SettingProvider.Items["BrandLogoWidth"]); }
            set { SettingProvider.Items["BrandLogoWidth"] = value.ToString(); }
        }

        public static int BrandLogoHeight
        {
            get { return Convert.ToInt32(SettingProvider.Items["BrandLogoHeight"]); }
            set { SettingProvider.Items["BrandLogoHeight"] = value.ToString(); }
        }

        public static int CarouselBigWidth
        {
            get { return Convert.ToInt32(SettingProvider.Items["CarouselBigWidth"]); }
            set { SettingProvider.Items["CarouselBigWidth"] = value.ToString(); }
        }

        public static int CarouselBigHeight
        {
            get { return Convert.ToInt32(SettingProvider.Items["CarouselBigHeight"]); }
            set { SettingProvider.Items["CarouselBigHeight"] = value.ToString(); }
        }

        public static int SmallProductImageHeight
        {
            get { return Convert.ToInt32(SettingProvider.Items["SmallProductImageHeight"]); }
            set { SettingProvider.Items["SmallProductImageHeight"] = value.ToString(); }
        }

        public static int SmallProductImageWidth
        {
            get { return Convert.ToInt32(SettingProvider.Items["SmallProductImageWidth"]); }
            set { SettingProvider.Items["SmallProductImageWidth"] = value.ToString(); }
        }

        public static int BigProductImageHeight
        {
            get { return Convert.ToInt32(SettingProvider.Items["BigProductImageHeight"]); }
            set { SettingProvider.Items["BigProductImageHeight"] = value.ToString(); }
        }

        public static int BigProductImageWidth
        {
            get { return Convert.ToInt32(SettingProvider.Items["BigProductImageWidth"]); }
            set { SettingProvider.Items["BigProductImageWidth"] = value.ToString(); }
        }

        public static int MiddleProductImageHeight
        {
            get { return Convert.ToInt32(SettingProvider.Items["MiddleProductImageHeight"]); }
            set { SettingProvider.Items["MiddleProductImageHeight"] = value.ToString(); }
        }

        public static int MiddleProductImageWidth
        {
            get { return Convert.ToInt32(SettingProvider.Items["MiddleProductImageWidth"]); }
            set { SettingProvider.Items["MiddleProductImageWidth"] = value.ToString(); }
        }

        public static int XSmallProductImageHeight
        {
            get { return Convert.ToInt32(SettingProvider.Items["XSmallProductImageHeight"]); }
            set { SettingProvider.Items["XSmallProductImageHeight"] = value.ToString(); }
        }

        public static int XSmallProductImageWidth
        {
            get { return Convert.ToInt32(SettingProvider.Items["XSmallProductImageWidth"]); }
            set { SettingProvider.Items["XSmallProductImageWidth"] = value.ToString(); }
        }

        public static int BigCategoryImageWidth
        {
            get { return Convert.ToInt32(SettingProvider.Items["BigCategoryImageWidth"]); }
            set { SettingProvider.Items["BigCategoryImageWidth"] = value.ToString(); }
        }

        public static int BigCategoryImageHeight
        {
            get { return Convert.ToInt32(SettingProvider.Items["BigCategoryImageHeight"]); }
            set { SettingProvider.Items["BigCategoryImageHeight"] = value.ToString(); }
        }

        public static int SmallCategoryImageWidth
        {
            get { return Convert.ToInt32(SettingProvider.Items["SmallCategoryImageWidth"]); }
            set { SettingProvider.Items["SmallCategoryImageWidth"] = value.ToString(); }
        }

        public static int SmallCategoryImageHeight
        {
            get { return Convert.ToInt32(SettingProvider.Items["SmallCategoryImageHeight"]); }
            set { SettingProvider.Items["SmallCategoryImageHeight"] = value.ToString(); }
        }


        public static int PaymentIconWidth
        {
            get { return Convert.ToInt32(SettingProvider.Items["PaymentIconWidth"]); }
            set { SettingProvider.Items["PaymentIconWidth"] = value.ToString(); }
        }

        public static int PaymentIconHeight
        {
            get { return Convert.ToInt32(SettingProvider.Items["PaymentIconHeight"]); }
            set { SettingProvider.Items["PaymentIconHeight"] = value.ToString(); }
        }

        public static int ShippingIconWidth
        {
            get { return Convert.ToInt32(SettingProvider.Items["ShippingIconWidth"]); }
            set { SettingProvider.Items["ShippingIconWidth"] = value.ToString(); }
        }

        public static int ShippingIconHeight
        {
            get { return Convert.ToInt32(SettingProvider.Items["ShippingIconHeight"]); }
            set { SettingProvider.Items["ShippingIconHeight"] = value.ToString(); }
        }

    }
}
