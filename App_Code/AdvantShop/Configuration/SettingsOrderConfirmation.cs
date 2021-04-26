//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;

namespace AdvantShop.Configuration
{
    public class SettingsOrderConfirmation
    {
        public static bool AmountLimitation
        {
            get { return Convert.ToBoolean(SettingProvider.Items["AmountLimitation"]); }
            set { SettingProvider.Items["AmountLimitation"] = value.ToString(); }
        }
        public static bool DecrementProductsCount
        {
            get { return Convert.ToBoolean(SettingProvider.Items["DecrementProductsCount"]); }
            set { SettingProvider.Items["DecrementProductsCount"] = value.ToString(); }
        }

        public static string QrUserId
        {
            get { return SettingProvider.Items["QRUserID"]; }
            set { SettingProvider.Items["QRUserID"] = value; }
        }

        public static decimal MinimalOrderPrice
        {
            get
            {
                decimal minimalPrice = 0;
                decimal.TryParse(SettingProvider.Items["MinimalOrderPrice"], out minimalPrice);
                return minimalPrice;
            }
            set { SettingProvider.Items["MinimalOrderPrice"] = value.ToString("#0.00") ?? "0.00"; }
        }

        public static decimal MinimalPriceCertificate
        {
            get
            {
                decimal minimalPriceCertificate = 0;
                decimal.TryParse(SettingProvider.Items["MinimalPriceCertificate"], out minimalPriceCertificate);
                return minimalPriceCertificate;
            }
            set { SettingProvider.Items["MinimalPriceCertificate"] = value.ToString("#0.00") ?? "0.00"; }
        }

        public static decimal MaximalPriceCertificate
        {
            get
            {
                decimal maximalPriceCertificate = 0;
                decimal.TryParse(SettingProvider.Items["MaximalPriceCertificate"], out maximalPriceCertificate);
                return maximalPriceCertificate;
            }
            set { SettingProvider.Items["MaximalPriceCertificate"] = value.ToString("#0.00") ?? "0.00"; }
        }

        public static bool EnableGiftCertificateService
        {
            get { return Convert.ToBoolean(SettingProvider.Items["EnableGiftCertificateService"]); }
            set { SettingProvider.Items["EnableGiftCertificateService"] = value.ToString(); }
        }


        public static bool EnableDiscountModule
        {
            get { return Convert.ToBoolean(SettingProvider.Items["EnableDiscountModule"]); }
            set { SettingProvider.Items["EnableDiscountModule"] = value.ToString(); }
        }
        public static bool BuyInOneClick
        {
            get { return Convert.ToBoolean(SettingProvider.Items["BuyInOneClick"]); }
            set { SettingProvider.Items["BuyInOneClick"] = value.ToString(); }
        }

        public static string BuyInOneClick_FirstText
        {
            get { return SettingProvider.Items["BuyInOneClick_FirstText"]; }
            set { SettingProvider.Items["BuyInOneClick_FirstText"] = value; }
        }

        public static string BuyInOneClick_FinalText
        {
            get { return SettingProvider.Items["BuyInOneClick_FinalText"]; }
            set { SettingProvider.Items["BuyInOneClick_FinalText"] = value; }
        }
    }
}