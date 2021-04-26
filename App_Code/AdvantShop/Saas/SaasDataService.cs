//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Data;
using System.Data.SqlClient;
using AdvantShop.Configuration;
using AdvantShop.Core;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using SaasWebService;

namespace AdvantShop.SaasData
{
    public class SaasDataService
    {
        public static bool IsSaasEnabled
        {
            get { return ModeConfigService.IsModeEnabled(ModeConfigService.Modes.SaasMode); }
        }

        public static bool IsExist()
        {
            bool isExist = SQLDataAccess.ExecuteScalar<int>("SELECT COUNT(*) FROM [dbo].[SaasData]", CommandType.Text) > 0;

            return isExist;
        }

        public static SaasData GetSaasDataFromDB()
        {
            var saasData = new SaasData()
            {
                LastUpdate = DateTime.Now.AddDays(-2)
            };
            using (var db = new SQLDataAccess())
            {
                db.cmd.CommandText = "SELECT * FROM [dbo].[SaasData]";
                db.cmd.CommandType = CommandType.Text;
                db.cmd.Parameters.Clear();

                db.cnOpen();
                using (var reader = db.cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        saasData = GetSaasDataFromReader(reader);
                    }
                    reader.Close();
                }
                db.cnClose();
            }

            return saasData;
        }

        private static SaasData GetSaasDataFromReader(SqlDataReader reader)
        {
            return new SaasData
            {
                Name = SQLDataHelper.GetString(reader, "Name"),
                ProductsCount = SQLDataHelper.GetInt(reader, "ProductsCount"),
                PhotosCount = SQLDataHelper.GetInt(reader, "PhotosCount"),
                HaveExcel = SQLDataHelper.GetBoolean(reader, "HaveExcel"),
                Have1C = SQLDataHelper.GetBoolean(reader, "Have1C"),
                HaveExportFeeds = SQLDataHelper.GetBoolean(reader, "HaveExportFeeds"),
                HavePriceRegulating = SQLDataHelper.GetBoolean(reader, "HavePriceRegulating"),
                HaveBankIntegration = SQLDataHelper.GetBoolean(reader, "HaveBankIntegration"),
                IsWork = SQLDataHelper.GetBoolean(reader, "IsWork"),
                PaidTo = SQLDataHelper.GetDateTime(reader, "PaidTo"),
                LastUpdate = SQLDataHelper.GetDateTime(reader, "LastUpdate")
            };
        }


        public static void AddSaasDataToDB(SaasData saasData)
        {
            using (var db = new SQLDataAccess())
            {
                db.cmd.CommandText = "INSERT INTO [dbo].[SaasData] " +
                    "(Name, ProductsCount, PhotosCount, HaveExcel, Have1C, HaveExportFeeds, HavePriceRegulating, HaveBankIntegration, IsWork, PaidTo, LastUpdate) VALUES " +
                    "(@Name, @ProductsCount, @PhotosCount, @HaveExcel, @Have1C, @HaveExportFeeds, @HavePriceRegulating, @HaveBankIntegration, @IsWork, @PaidTo, GETDATE()) ";
                db.cmd.CommandType = CommandType.Text;
                db.cmd.Parameters.Clear();

                db.cmd.Parameters.AddWithValue("@Name", saasData.Name);
                db.cmd.Parameters.AddWithValue("@ProductsCount", saasData.ProductsCount);
                db.cmd.Parameters.AddWithValue("@PhotosCount ", saasData.PhotosCount);
                db.cmd.Parameters.AddWithValue("@HaveExcel", saasData.HaveExcel);
                db.cmd.Parameters.AddWithValue("@Have1C", saasData.Have1C);
                db.cmd.Parameters.AddWithValue("@HaveExportFeeds", saasData.HaveExportFeeds);
                db.cmd.Parameters.AddWithValue("@HavePriceRegulating", saasData.HavePriceRegulating);
                db.cmd.Parameters.AddWithValue("@HaveBankIntegration", saasData.HaveBankIntegration);
                db.cmd.Parameters.AddWithValue("@IsWork", saasData.IsWork);
                db.cmd.Parameters.AddWithValue("@PaidTo", saasData.PaidTo);

                db.cnOpen();
                db.cmd.ExecuteNonQuery();
                db.cnClose();
            }
        }

        public static void UpdateSaasDataToDB(SaasData saasData)
        {
            using (var db = new SQLDataAccess())
            {
                db.cmd.CommandText = " UPDATE [dbo].[SaasData] SET Name = @Name, ProductsCount = @ProductsCount, PhotosCount = @PhotosCount, HaveExcel = @HaveExcel, Have1C = @Have1C, HaveExportFeeds = @HaveExportFeeds, HavePriceRegulating = @HavePriceRegulating, HaveBankIntegration = @HaveBankIntegration, IsWork = @IsWork, PaidTo = @PaidTo, LastUpdate = GETDATE()";
                db.cmd.CommandType = CommandType.Text;
                db.cmd.Parameters.Clear();

                db.cmd.Parameters.AddWithValue("@Name", saasData.Name);
                db.cmd.Parameters.AddWithValue("@ProductsCount", saasData.ProductsCount);
                db.cmd.Parameters.AddWithValue("@PhotosCount ", saasData.PhotosCount);
                db.cmd.Parameters.AddWithValue("@HaveExcel", saasData.HaveExcel);
                db.cmd.Parameters.AddWithValue("@Have1C", saasData.Have1C);
                db.cmd.Parameters.AddWithValue("@HaveExportFeeds", saasData.HaveExportFeeds);
                db.cmd.Parameters.AddWithValue("@HavePriceRegulating", saasData.HavePriceRegulating);
                db.cmd.Parameters.AddWithValue("@HaveBankIntegration", saasData.HaveBankIntegration);
                db.cmd.Parameters.AddWithValue("@IsWork", saasData.IsWork);
                db.cmd.Parameters.AddWithValue("@PaidTo", saasData.PaidTo);

                db.cnOpen();
                db.cmd.ExecuteNonQuery();
                db.cnClose();
            }
        }

        public static SaasData CurrentSaasData
        {
            get
            {
                return GetSaasDataFromService();
            }
        }

        private static SaasData GetSaasDataFromService()
        {
            var saasData = GetSaasDataFromDB();

            if ((saasData.LastUpdate.AddHours(1) <= DateTime.Now) || (!saasData.IsWorkingNow))
            {
                saasData = UpdateSaasDataFromService();
            }

            if (saasData != null)
            {
                return saasData;
            }

            if (IsExist())
            {
                return GetSaasDataFromDB();
            }

            return new SaasData();
        }

        public static SaasData UpdateSaasDataFromService()
        {
            var saasData = GetSaasDataFromDB();

            var client = new SaasWebServiceSoapClient();
            try
            {
                var newSaasData = client.GetSaasData(SettingsGeneral.CurrentSaasId);

                if (newSaasData.IsValid)
                {
                    saasData.Name = newSaasData.Name;
                    saasData.ProductsCount = newSaasData.ProductsCount;
                    saasData.PhotosCount = newSaasData.PhotosCount;
                    saasData.HaveExcel = newSaasData.HaveExcel;
                    saasData.Have1C = newSaasData.Have1C;
                    saasData.HaveExportFeeds = newSaasData.HaveExportFeeds;
                    saasData.HavePriceRegulating = newSaasData.HavePriceRegulating;
                    saasData.HaveBankIntegration = newSaasData.HaveBankIntegration;
                    saasData.IsWork = newSaasData.IsWork;
                    saasData.PaidTo = newSaasData.PaidTo;
                    saasData.LastUpdate = DateTime.Now;

                    if (IsExist())
                    {
                        UpdateSaasDataToDB(saasData);
                    }
                    else
                    {
                        AddSaasDataToDB(saasData);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                return null;
            }

            return saasData;
        }
    }
}
