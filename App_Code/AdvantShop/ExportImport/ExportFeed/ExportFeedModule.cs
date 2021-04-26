//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdvantShop.Configuration;
using AdvantShop.Core;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.Helpers;

namespace AdvantShop.ExportImport
{
    public abstract class ExportFeedModule
    {
        protected string _shopUrl;
        protected abstract string ModuleName { get; }
        public abstract void GetExportFeedString(string filenameAndPath);

        protected ExportFeedModule()
        {
            _shopUrl = GetShopUrl();
        }

        protected int GetCategoriesCount(string moduleName)
        {
            return SQLDataAccess.ExecuteScalar<int>("[Settings].[sp_GetExportFeedCategories]",
                                                                               CommandType.StoredProcedure,
                                                                               new SqlParameter("@moduleName", moduleName),
                                                                               new SqlParameter("@onlyCount", true));
        }

        protected IEnumerable<ExportFeedCategories> GetCategories(string moduleName)
        {
            return SQLDataAccess.ExecuteReadIEnumerable<ExportFeedCategories>("[Settings].[sp_GetExportFeedCategories]",
                                                                               CommandType.StoredProcedure,
                                                                               reader => new ExportFeedCategories
                                                                                             {
                                                                                                 Id = SQLDataHelper.GetInt(reader, "CategoryID"),
                                                                                                 ParentCategory = SQLDataHelper.GetInt(reader, "ParentCategory"),
                                                                                                 Name = SQLDataHelper.GetString(reader, "Name")
                                                                                             },
                                                                               new SqlParameter("@moduleName", moduleName),
                                                                               new SqlParameter("@onlyCount", false));
        }

        protected int GetProdutsCount(string moduleName)
        {
            return SQLDataAccess.ExecuteScalar<int>("[Settings].[sp_GetExportFeedProducts]",
                                                                               CommandType.StoredProcedure,
                                                                               new SqlParameter("@moduleName", moduleName),
                                                                               new SqlParameter("@selectedCurrency", ExportFeed.GetModuleSetting(moduleName, "Currency")),
                                                                               new SqlParameter("@onlyCount", true));
        }
        protected IEnumerable<ExportFeedProduts> GetProduts(string moduleName)
        {
            return SQLDataAccess.ExecuteReadIEnumerable<ExportFeedProduts>("[Settings].[sp_GetExportFeedProducts]",
                                                                               CommandType.StoredProcedure,
                                                                               reader => new ExportFeedProduts
                                                                               {
                                                                                   ProductID = SQLDataHelper.GetInt(reader, "ProductID"),
                                                                                   Amount = SQLDataHelper.GetInt(reader, "Amount"),
                                                                                   UrlPath = SQLDataHelper.GetString(reader, "UrlPath"),
                                                                                   Price = SQLDataHelper.GetDecimal(reader, "Price"),
                                                                                   Discount = SQLDataHelper.GetDecimal(reader, "Discount"),
                                                                                   ParentCategory = SQLDataHelper.GetInt(reader, "ParentCategory"),
                                                                                   Name = SQLDataHelper.GetString(reader, "Name"),
                                                                                   Description = SQLDataHelper.GetString(reader, "Description"),
                                                                                   BriefDescription = SQLDataHelper.GetString(reader, "BriefDescription"),
                                                                                   Photo = SQLDataHelper.GetString(reader, "Photo"),
                                                                                   //Added by Evgeni
                                                                                   BrandName = SQLDataHelper.GetString(reader, "BrandName"),
                                                                                   ArtNo = SQLDataHelper.GetString(reader, "ArtNo"),

                                                                               },
                                                                               new SqlParameter("@moduleName", moduleName),
                                                                               new SqlParameter("@selectedCurrency", ExportFeed.GetModuleSetting(moduleName, "Currency")),
                                                                               new SqlParameter("@onlyCount", false));
        }

        protected string GetShopName()
        {
            var result = string.Empty;
            try
            {
                result = SQLDataAccess.ExecuteScalar<string>("SELECT Value FROM [Settings].[Settings] WHERE Name = \'ShopName\';", CommandType.Text);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
            return result;
        }

        protected string GetShopUrl()
        {
            return ExportFeed.GetShopUrl();
        }

        protected void SetShopUrlToNull()
        {
            ExportFeed.SetShopUrlToNull();
        }

        protected DataTable GetCurrencies()
        {
            var dataTable = new DataTable();
            try
            {
                dataTable = SQLDataAccess.ExecuteTable("SELECT CurrencyValue, CurrencyIso3 FROM [Catalog].[Currency];", CommandType.Text);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
            return dataTable;
        }

        protected string GetDefaultCurrencyISO3()
        {
            var result = string.Empty;
            try
            {
                result = SQLDataAccess.ExecuteScalar<string>("SELECT Value FROM [Settings].[Settings] WHERE [Name] = \'DefaultCurrencyISO3\';", CommandType.Text);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
            return result;
        }

        protected static string GetImageProductPath(string photoPath)
        {
            if (photoPath.ToLower().Contains("http://"))
                return photoPath;

            //const ProductImageType type = ProductImageType.Middle;
            //var appPath = SettingsMain.SiteUrl;
            
            //Changed By Evgeni gor full photo name in YandexMarket and others
            return (SettingsMain.SiteUrl + "/" + FoldersHelper.GetImageProductPath(ProductImageType.Middle, photoPath, false));
            //appPath + "/" + ImageFolders.ProductImagePath +ImageFolders.ProductPhotoPrefix[type] +photoPath + ImageFolders.ProductPhotoPostfix[type];
        }
    }
}