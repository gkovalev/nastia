//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.IO;
using AdvantShop.Configuration;
using System;

namespace AdvantShop.FilePath
{
    public enum ProductImageType
    {
        Big,
        Middle,
        Small,
        XSmall,
        Original
    }

    public enum CategoryImageType
    {
        Big,
        Small,
    }

    public enum FolderType
    {
        Pictures,
        MenuIcons,
        Product,
        Carousel,
        Category,
        News,
        StaticPage,
        BrandLogo,
        PaymentLogo,
        ShippingLogo,
        PriceTemp,
        ImageTemp
    }

    public class FoldersHelper
    {
        public static readonly Dictionary<FolderType, string> PhotoFoldersPath = new Dictionary<FolderType, string>
                                                                                      {
                                                                                          {FolderType.Pictures, "pictures/"},
                                                                                          {FolderType.MenuIcons, "pictures/icons/"},
                                                                                          {FolderType.Product, "pictures/product/"},
                                                                                          {FolderType.Carousel, "pictures/carousel/"},
                                                                                          {FolderType.News, "pictures/news/"},
                                                                                          {FolderType.Category, "pictures/category/"},
                                                                                          {FolderType.BrandLogo, "pictures/brand/"},
                                                                                          {FolderType.PaymentLogo, "pictures/payment/"},
                                                                                          {FolderType.ShippingLogo, "pictures/shipping/"},
                                                                                          {FolderType.StaticPage, "pictures/staticpage/"},
                                                                                          {FolderType.PriceTemp, "price_temp/"},
                                                                                          {FolderType.ImageTemp, "upload_images/"},
                                                                                      };

        public static readonly Dictionary<CategoryImageType, string> CategoryPhotoPrefix = new Dictionary<CategoryImageType, string>
                                                                                            {
                                                                                                {CategoryImageType.Small, @"small/"},
                                                                                                {CategoryImageType.Big, @""},
                                                                                            };

        public static readonly Dictionary<ProductImageType, string> ProductPhotoPrefix = new Dictionary<ProductImageType, string>
                                                                                            {
                                                                                                {ProductImageType.XSmall, @"xsmall/"},
                                                                                                {ProductImageType.Small, @"small/"},
                                                                                                {ProductImageType.Middle, @"middle/"},
                                                                                                {ProductImageType.Big, @"big/"},
                                                                                                {ProductImageType.Original, @"original/"}
                                                                                            };
        public static readonly Dictionary<ProductImageType, string> ProductPhotoPostfix = new Dictionary<ProductImageType, string>
                {
                    {ProductImageType.XSmall, @"_xsmall"},
                    {ProductImageType.Small, @"_small"},
                    {ProductImageType.Middle, @"_middle"},
                    {ProductImageType.Big, @"_big"},
                    {ProductImageType.Original, @"_original"}
                };

        private static string GetPath(string imagePathBase, bool isForAdministration)
        {
            return (isForAdministration ? "../" : string.Empty) + imagePathBase;
        }

        private static string GetPathAbsolut(string imagePathBase)
        {
            return SettingsGeneral.AbsolutePath + imagePathBase;
        }

        //_____________
        public static string GetPath(FolderType type, string photoPath, bool isForAdministration)
        {
            if (string.IsNullOrWhiteSpace(photoPath))
                return GetPath(PhotoFoldersPath[type], isForAdministration);
            return GetPath(PhotoFoldersPath[type], isForAdministration) + photoPath;
        }

        public static string GetPathAbsolut(FolderType type, string photoPath = "")
        {
            if (string.IsNullOrWhiteSpace(photoPath))
                return GetPathAbsolut(PhotoFoldersPath[type]);
            return GetPathAbsolut(PhotoFoldersPath[type]) + photoPath;
        }
        //_____________


        #region ProductImage
        public static string GetImageProductPath(ProductImageType type, string photoPath, bool isForAdministration)
        {
            /*if (string.IsNullOrWhiteSpace(photoPath))
                return "images/nophoto" + ProductPhotoPostfix[type] + ".jpg";
            return GetPath(PhotoFoldersPath[FolderType.Product], isForAdministration) + ProductPhotoPrefix[type] + Path.GetFileNameWithoutExtension(photoPath) + ProductPhotoPostfix[type] + Path.GetExtension(photoPath);
             * */
            if (string.IsNullOrWhiteSpace(photoPath))
                return "images/nophoto" + ProductPhotoPostfix[type] + ".jpg";
            
            else 
            {
                //changed by Evgeni to get pictures from various folders when changing need to change Yamarket also
                if (photoPath.ToLower().Contains("bosch_blue_pt"))
                {
                    return String.Format(@"{0}BoschBluePt/{1}/{2}{3}", (GetPath(PhotoFoldersPath[FolderType.Product], isForAdministration).ToLower()).Replace(type.ToString().ToLower() + @"/", ""), type, Path.GetFileNameWithoutExtension(photoPath), ProductPhotoPostfix[type] + Path.GetExtension(photoPath));
                }
                else if (photoPath.ToLower().Contains("bosch_blue_ac"))
                {
                    return String.Format(@"{0}BoschBlueAc/{1}/{2}{3}", (GetPath(PhotoFoldersPath[FolderType.Product], isForAdministration).ToLower()).Replace(type.ToString().ToLower() + @"/", ""), type, Path.GetFileNameWithoutExtension(photoPath), ProductPhotoPostfix[type] + Path.GetExtension(photoPath));
                }
                else if (photoPath.ToLower().Contains("bosch_green_pt"))
                {
                    return String.Format(@"{0}BoschGreenPt/{1}/{2}{3}", (GetPath(PhotoFoldersPath[FolderType.Product], isForAdministration).ToLower()).Replace(type.ToString().ToLower() + @"/", ""), type, Path.GetFileNameWithoutExtension(photoPath), ProductPhotoPostfix[type] + Path.GetExtension(photoPath));
                }
                else if (photoPath.ToLower().Contains("bosch_green_ac"))
                {
                    return String.Format(@"{0}BoschGreenAc/{1}/{2}{3}", (GetPath(PhotoFoldersPath[FolderType.Product], isForAdministration).ToLower()).Replace(type.ToString().ToLower() + @"/", ""), type, Path.GetFileNameWithoutExtension(photoPath), ProductPhotoPostfix[type] + Path.GetExtension(photoPath));
                }
                else if (photoPath.ToLower().Contains("bosch_garden_pt"))
                {
                    return String.Format(@"{0}BoschGarden/{1}/{2}{3}", (GetPath(PhotoFoldersPath[FolderType.Product], isForAdministration).ToLower()).Replace(type.ToString().ToLower() + @"/", ""), type, Path.GetFileNameWithoutExtension(photoPath), ProductPhotoPostfix[type] + Path.GetExtension(photoPath));
                }
                else if (photoPath.ToLower().Contains("skill_diy_pt"))
                {
                    return String.Format(@"{0}SkillDiy/{1}/{2}{3}", (GetPath(PhotoFoldersPath[FolderType.Product], isForAdministration).ToLower()).Replace(type.ToString().ToLower() + @"/", ""), type, Path.GetFileNameWithoutExtension(photoPath), ProductPhotoPostfix[type] + Path.GetExtension(photoPath));
                    //GetPath(PhotoFoldersPath[FolderType.Product], isForAdministration) + ProductPhotoPrefix[type] + Path.GetFileNameWithoutExtension(photoPath) + ProductPhotoPostfix[type] + Path.GetExtension(photoPath)
                }
                else if (photoPath.ToLower().Contains("skill_master_pt"))
                {
                    return String.Format(@"{0}SkillMasters/{1}/{2}{3}", (GetPath(PhotoFoldersPath[FolderType.Product], isForAdministration).ToLower()).Replace(type.ToString().ToLower() + @"/", ""), type, Path.GetFileNameWithoutExtension(photoPath), ProductPhotoPostfix[type] + Path.GetExtension(photoPath));
                }
                else if (photoPath.ToLower().Contains("dremel"))
                {
                    return String.Format(@"{0}Dremel/{1}/{2}{3}", (GetPath(PhotoFoldersPath[FolderType.Product], isForAdministration).ToLower()).Replace(type.ToString().ToLower() + @"/", ""), type, Path.GetFileNameWithoutExtension(photoPath), ProductPhotoPostfix[type] + Path.GetExtension(photoPath));
                }
                else if (photoPath.ToLower().Contains("cstberger"))
                {
                    return String.Format(@"{0}CstBerger/{1}/{2}{3}", (GetPath(PhotoFoldersPath[FolderType.Product], isForAdministration).ToLower()).Replace(type.ToString().ToLower() + @"/", ""), type, Path.GetFileNameWithoutExtension(photoPath), ProductPhotoPostfix[type] + Path.GetExtension(photoPath));
                }
                else if (photoPath.ToLower().Contains("bosch_blue_mt"))
                {
                    return String.Format(@"{0}BoschMtPRO/{1}/{2}{3}", (GetPath(PhotoFoldersPath[FolderType.Product], isForAdministration).ToLower()).Replace(type.ToString().ToLower() + @"/", ""), type, Path.GetFileNameWithoutExtension(photoPath), ProductPhotoPostfix[type] + Path.GetExtension(photoPath));
                }
                else if (photoPath.ToLower().Contains("bosch_green_mt"))
                {
                    return String.Format(@"{0}BoschMtDiy/{1}/{2}{3}", (GetPath(PhotoFoldersPath[FolderType.Product], isForAdministration).ToLower()).Replace(type.ToString().ToLower() + @"/", ""), type, Path.GetFileNameWithoutExtension(photoPath), ProductPhotoPostfix[type] + Path.GetExtension(photoPath));
                }
                else
                {
                    return GetPath(PhotoFoldersPath[FolderType.Product], isForAdministration) + ProductPhotoPrefix[type] + Path.GetFileNameWithoutExtension(photoPath) + ProductPhotoPostfix[type] + Path.GetExtension(photoPath);
                }

            }
           
        }

        public static string GetImageProductPathAbsolut(ProductImageType type, string photoPath)
        {
            if (string.IsNullOrWhiteSpace(photoPath))
                return GetPathAbsolut(PhotoFoldersPath[FolderType.Product]) + ProductPhotoPrefix[type];
            return GetPathAbsolut(PhotoFoldersPath[FolderType.Product]) + ProductPhotoPrefix[type] + Path.GetFileNameWithoutExtension(photoPath) + ProductPhotoPostfix[type] + Path.GetExtension(photoPath);
        }
        #endregion


        #region CategoryImage
        public static string GetImageCategoryPathAbsolut(CategoryImageType type, string photoPath)
        {
            if (string.IsNullOrWhiteSpace(photoPath))
                return GetPathAbsolut(PhotoFoldersPath[FolderType.Category]) + CategoryPhotoPrefix[type];

            return GetPathAbsolut(PhotoFoldersPath[FolderType.Category]) + CategoryPhotoPrefix[type] + photoPath;
        }

        public static string GetImageCategoryPath(CategoryImageType type, string photoPath, bool isForAdministration)
        {
            return GetPath(PhotoFoldersPath[FolderType.Category], isForAdministration) + CategoryPhotoPrefix[type] + photoPath;
        }
        #endregion
    }
}