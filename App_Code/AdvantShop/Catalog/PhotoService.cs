//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using AdvantShop.Configuration;
using AdvantShop.Core;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.SaasData;

namespace AdvantShop.Catalog
{
    public class PhotoService
    {
        public static Photo GetPhoto(int photoId)
        {
            return SQLDataAccess.ExecuteReadOne<Photo>("SELECT * FROM [Catalog].[Photo] WHERE [PhotoID] = @PhotoID",
                                                        CommandType.Text,
                                                        GetPhotoFromReader, new SqlParameter("@PhotoID", photoId));
        }

        /// <summary>
        /// return list of photos by type
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<Photo> GetPhotos(int objId, PhotoType type)
        {
            var list = SQLDataAccess.ExecuteReadIEnumerable<Photo>("SELECT * FROM [Catalog].[Photo] WHERE [objId] = @objId and type=@type  ORDER BY [PhotoSortOrder]",
                                                                    CommandType.Text, GetPhotoFromReader,
                                                                    new SqlParameter("@objId", objId),
                                                                    new SqlParameter("@type", type.ToString()));
            return list;
        }

        /// <summary>
        /// return list of filename to delete
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetNamePhotos(int objId, PhotoType type)
        {
            if (objId == 0)
                return SQLDataAccess.ExecuteReadIEnumerable<string>("SELECT PhotoName FROM [Catalog].[Photo] WHERE type=@type",
                                                                    CommandType.Text, reader => SQLDataHelper.GetString(reader, "PhotoName"),
                                                                    new SqlParameter("@type", type.ToString()));

            return SQLDataAccess.ExecuteReadIEnumerable<string>("SELECT PhotoName FROM [Catalog].[Photo] WHERE [objId] = @objId and type=@type",
                                                                    CommandType.Text, reader => SQLDataHelper.GetString(reader, "PhotoName"),
                                                                    new SqlParameter("@objId", objId),
                                                                    new SqlParameter("@type", type.ToString()));
        }

        /// <summary>
        /// return count of photos by type
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int GetCountPhotos(int objId, PhotoType type)
        {
            if (objId == 0)
                return SQLDataAccess.ExecuteScalar<int>("SELECT Count(*) FROM [Catalog].[Photo] WHERE type=@type",
                                                                  CommandType.Text, new SqlParameter("@type", type.ToString()));

            var res = SQLDataAccess.ExecuteScalar<int>("SELECT Count(*) FROM [Catalog].[Photo] WHERE [objId] = @objId and type=@type",
                                                                    CommandType.Text, new SqlParameter("@objId", objId), new SqlParameter("@type", type.ToString()));
            return res;
        }

        public static Photo GetPhotoFromReader(SqlDataReader reader)
        {
            return new Photo(
                SQLDataHelper.GetInt(reader, "PhotoId"),
                SQLDataHelper.GetInt(reader, "ObjId"),
                (PhotoType)Enum.Parse(typeof(PhotoType), SQLDataHelper.GetString(reader, "Type"), true))
                {
                    Description = SQLDataHelper.GetString(reader, "Description"),
                    ModifiedDate = SQLDataHelper.GetDateTime(reader, "ModifiedDate"),
                    PhotoName = SQLDataHelper.GetString(reader, "PhotoName"),
                    OriginName = SQLDataHelper.GetString(reader, "OriginName"),
                    PhotoSortOrder = SQLDataHelper.GetInt(reader, "PhotoSortOrder"),
                    Main = SQLDataHelper.GetBoolean(reader, "Main"),
                };
        }

        /// <summary>
        /// add new photo, return new photo new name
        /// </summary>
        /// <param name="ph"></param>
        /// <returns></returns>
        public static string AddPhoto(Photo ph)
        {
            string photoName = "";

            if (ph.Type == PhotoType.Product && SaasDataService.IsSaasEnabled && GetCountPhotos(ph.ObjId, ph.Type) >= SaasDataService.CurrentSaasData.PhotosCount)
            {
                return photoName;
            }
            photoName = SQLDataAccess.ExecuteScalar<string>("[Catalog].[sp_AddPhoto]", CommandType.StoredProcedure,
                                                               new SqlParameter("@ObjId", ph.ObjId),
                                                               new SqlParameter("@Description", ph.Description ?? string.Empty),
                                                               new SqlParameter("@OriginName", ph.OriginName),
                                                               new SqlParameter("@Type", ph.Type.ToString()),
                                                               new SqlParameter("@Extension", Path.GetExtension(ph.OriginName))
                                                               );
            return photoName;
        }

        public static string GetPathByPhotoId(int id)
        {
            return SQLDataAccess.ExecuteScalar<string>("SELECT [PhotoName] FROM [Catalog].[Photo] WHERE [PhotoId] = @PhotoId", CommandType.Text, new SqlParameter("@PhotoId", id));
        }

        public static Photo GetPhotoByObjId(int objId, PhotoType type)
        {
            return SQLDataAccess.ExecuteReadOne<Photo>("SELECT * FROM [Catalog].[Photo] WHERE [ObjId] = @ObjId and type=@type",
                                                        CommandType.Text, GetPhotoFromReader,
                                                        new SqlParameter("@ObjId", objId),
                                                        new SqlParameter("@type", type.ToString()));
        }


        #region product

        public static void SetProductMainPhoto(int photoId)
        {
            SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_SetProductMainPhoto]", CommandType.StoredProcedure, new SqlParameter("@PhotoId", photoId));
        }

        public static void DeleteProductPhotoWithPath(string photoName)
        {
            SQLDataAccess.ExecuteNonQuery("DELETE FROM [Catalog].[Photo] WHERE PhotoName = @PhotoName", CommandType.Text, new SqlParameter("@PhotoName", photoName));
        }

        /// <summary>
        /// check is product have photo by name
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="originName"></param>
        /// <returns></returns>
        public static bool IsProductHaveThisPhotoByName(int productId, string originName)
        {
            return SQLDataAccess.ExecuteScalar<int>("select Count(PhotoID) from Catalog.Photo where ObjID=@productId and OriginName=@originName and type=@type",
                                                    CommandType.Text,
                                                    new SqlParameter("@productId", productId),
                                                    new SqlParameter("@originName", originName),
                                                    new SqlParameter("@type", PhotoType.Product.ToString())) > 0;
        }

        public static void UpdateProductPhoto(Photo ph)
        {
            SQLDataAccess.ExecuteNonQuery("Update Catalog.Photo set Description=@Description, PhotoSortOrder = @PhotoSortOrder Where PhotoID = @ProductPhotoID",
                                            CommandType.Text,
                                            new SqlParameter("@PhotoID", ph.PhotoId),
                                            new SqlParameter("@PhotoSortOrder", ph.PhotoSortOrder),
                                            new SqlParameter("@Description", ph.Description)
                                            );
        }

        public static void DeleteProductPhoto(int photoId)
        {
            var photoName = GetPathByPhotoId(photoId);
            DeleteFile(PhotoType.Product, photoName);
            DeletePhotoById(photoId);
        }

        public static void DeleteProductPhotos(int productId)
        {
            DeletePhotos(productId, PhotoType.Product);
        }
        #endregion

        public static void DeletePhotos(int objId, PhotoType type)
        {
            foreach (var photoName in GetNamePhotos(objId, type))
            {
                DeleteFile(type, photoName);
            }
            DeletePhotoByOwnerIdAndType(objId, type);
        }

        private static void DeleteFile(PhotoType type, string photoName)
        {
            switch (type)
            {
                case PhotoType.Product:
                    FileHelpers.DeleteFile(FoldersHelper.GetImageProductPathAbsolut(ProductImageType.Original, photoName));
                    FileHelpers.DeleteFile(FoldersHelper.GetImageProductPathAbsolut(ProductImageType.Big, photoName));
                    FileHelpers.DeleteFile(FoldersHelper.GetImageProductPathAbsolut(ProductImageType.Middle, photoName));
                    FileHelpers.DeleteFile(FoldersHelper.GetImageProductPathAbsolut(ProductImageType.Small, photoName));
                    FileHelpers.DeleteFile(FoldersHelper.GetImageProductPathAbsolut(ProductImageType.XSmall, photoName));
                    break;
                case PhotoType.Brand:
                    FileHelpers.DeleteFile(FoldersHelper.GetPathAbsolut(FolderType.BrandLogo, photoName));
                    break;
                case PhotoType.CategoryBig:
                    FileHelpers.DeleteFile(FoldersHelper.GetImageCategoryPathAbsolut(CategoryImageType.Big, photoName));
                    break;
                case PhotoType.CategorySmall:
                    FileHelpers.DeleteFile(FoldersHelper.GetImageCategoryPathAbsolut(CategoryImageType.Small, photoName));
                    break;
                case PhotoType.Carousel:
                    FileHelpers.DeleteFile(FoldersHelper.GetPathAbsolut(FolderType.Carousel, photoName));
                    break;
                case PhotoType.News:
                    FileHelpers.DeleteFile(FoldersHelper.GetPathAbsolut(FolderType.News, photoName));
                    break;
                case PhotoType.StaticPage:
                    FileHelpers.DeleteFile(FoldersHelper.GetPathAbsolut(FolderType.StaticPage, photoName));
                    break;
            }
        }

        private static void DeletePhotoById(int photoId)
        {
            SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_DeletePhoto]", CommandType.StoredProcedure, new SqlParameter("@PhotoId", photoId));
        }

        private static void DeletePhotoByOwnerIdAndType(int objId, PhotoType type)
        {
            SQLDataAccess.ExecuteNonQuery("Delete FROM [Catalog].[Photo] WHERE [ObjId] = @ObjId and type=@type", CommandType.Text,
                                            new SqlParameter("@objId", objId),
                                            new SqlParameter("@type", type.ToString()));
        }


        /// <summary>
        /// Проверяет наличие продукта в базе
        /// </summary>
        /// <param name="fileName">имя файла изображения, ищется продукт с аналогичным артикулом </param>
        /// <returns>ID найденного продукта</returns>
        /// <remarks>если записей не найдено возвращается пустая строка</remarks>
        public static int CheckImageInDataBase(string fileName)
        {
            // без расширения
            int dotPos = fileName.LastIndexOf(".");
            string shortFilename = fileName.Remove(dotPos, fileName.Length - dotPos);

            // 551215_v01_m.jpg
            // Regex regex = new Regex("([\\d\\w^\\-]*)_v([\\d]{2})_m");

            // 8470_1.jpg
            var regex = new Regex("([\\d\\w^\\-]*)_([\\d]*)");
            Match m = regex.Match(shortFilename);

            shortFilename = m.Groups[1].Value;

            return ProductService.GetProductId(shortFilename);
        }

        public static string GetDescription(int photoId)
        {
            if (photoId == 0)
                return string.Empty;
            return SQLDataAccess.ExecuteScalar<string>("SELECT [Description] FROM [Catalog].[Photo] WHERE [PhotoID] = @photoId", CommandType.Text, new SqlParameter("@photoId", photoId));
        }

        public static Size GetImageMaxSize(ProductImageType type)
        {
            switch (type)
            {
                case ProductImageType.Big:
                    return new Size(SettingsPictureSize.BigProductImageWidth, SettingsPictureSize.BigProductImageHeight);
                case ProductImageType.Middle:
                    return new Size(SettingsPictureSize.MiddleProductImageWidth, SettingsPictureSize.MiddleProductImageHeight);
                case ProductImageType.Small:
                    return new Size(SettingsPictureSize.SmallProductImageWidth, SettingsPictureSize.SmallProductImageHeight);
                case ProductImageType.XSmall:
                    return new Size(SettingsPictureSize.XSmallProductImageWidth, SettingsPictureSize.XSmallProductImageHeight);
                default:
                    throw new ArgumentException(@"Parameter must be ProductImageType", "type");
            }
        }
    }
}