//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Linq;
using AdvantShop.Repository.Currencies;

namespace AdvantShop.Configuration
{
    public class SettingsCatalog
    {
        public static int ProductsPerPage
        {
            get { return int.Parse(SettingProvider.Items["ProductsPerPage"]); }
            set { SettingProvider.Items["ProductsPerPage"] = value.ToString(); }
        }

        public static string DefaultCurrencyIso3
        {
            get { return (CurrencyService.Currency(Convert.ToString(SettingProvider.Items["DefaultCurrencyISO3"])) ?? CurrencyService.GetAllCurrencies().FirstOrDefault()).Iso3; }
            set { SettingProvider.Items["DefaultCurrencyISO3"] = value; }
        }

        public static int DefaultCatalogView
        {
            get { return int.Parse(SettingProvider.Items["DefaultCatalogView"]); }
            set { SettingProvider.Items["DefaultCatalogView"] = value.ToString(); }
        }

        public static int DefaultSearchView
        {
            get { return int.Parse(SettingProvider.Items["DefaultSearchView"]); }
            set { SettingProvider.Items["DefaultSearchView"] = value.ToString(); }
        }

        public static bool EnableProductRating
        {
            get { return Convert.ToBoolean(SettingProvider.Items["EnableProductRating"]); }
            set { SettingProvider.Items["EnableProductRating"] = value.ToString(); }
        }

        public static bool ShowProductsCount
        {
            get { return Convert.ToBoolean(SettingProvider.Items["ShowProductsCount "]); }
            set { SettingProvider.Items["ShowProductsCount "] = value.ToString(); }
        }

        public static bool EnableCompareProducts
        {
            get { return Convert.ToBoolean(SettingProvider.Items["EnableCompareProducts "]); }
            set { SettingProvider.Items["EnableCompareProducts "] = value.ToString(); }
        }



        public static bool ShowProductsWithZeroAmount
        {
            get { return Convert.ToBoolean(SettingProvider.Items["ShowProductsWithZeroAmount "]); }
            set { SettingProvider.Items["ShowProductsWithZeroAmount "] = value.ToString(); }
        }

        public static bool EnabledCatalogViewChange
        {
            get { return Convert.ToBoolean(SettingProvider.Items["EnableCatalogViewChange"]); }
            set { SettingProvider.Items["EnableCatalogViewChange"] = value.ToString(); }
        }

        public static bool EnabledSearchViewChange
        {
            get { return Convert.ToBoolean(SettingProvider.Items["EnableSearchViewChange"]); }
            set { SettingProvider.Items["EnableSearchViewChange"] = value.ToString(); }
        }

        public static bool CompressBigImage
        {
            get { return Convert.ToBoolean(SettingProvider.Items["CompressBigImage"]); }
            set { SettingProvider.Items["CompressBigImage"] = value.ToString(); }
        }

        public static string RelatedProductName
        {
            get { return SettingProvider.Items["RelatedProductName"]; }
            set { SettingProvider.Items["RelatedProductName"] = value; }
        }

        public static string AlternativeProductName
        {
            get { return SettingProvider.Items["AlternativeProductName"]; }
            set { SettingProvider.Items["AlternativeProductName"] = value; }
        }

        public static bool AllowReviews
        {
            get { return Convert.ToBoolean(SettingProvider.Items["AllowReviews"]); }
            set { SettingProvider.Items["AllowReviews"] = value.ToString(); }
        }

        public static bool ModerateReviews
        {
            get { return Convert.ToBoolean(SettingProvider.Items["ModerateReviewed"]); }
            set { SettingProvider.Items["ModerateReviewed"] = value.ToString(); }
        }


        public static string GetRelatedProductName(int relatedType)
        {
            return (relatedType == 0) ? RelatedProductName : AlternativeProductName;
        }
    }
}