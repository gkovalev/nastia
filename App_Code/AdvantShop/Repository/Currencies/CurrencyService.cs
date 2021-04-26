//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using AdvantShop.Configuration;
using AdvantShop.Core;
using AdvantShop.Core.Caching;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using CBR;

namespace AdvantShop.Repository.Currencies
{
    public class CurrencyService
    {
        public static string DefaultPriceFormat
        {
            get { return @"##,##0.00 "; }
        }

        //static CurrencyService()
        //{
        //    RefreshCurrency();
        //}

        public static void RefreshCurrency()
        {
            var currencies = SQLDataAccess.ExecuteReadList("SELECT * FROM Catalog.Currency",
                                                        CommandType.Text, GetCurrencyFromReader);
            foreach (Currency t in currencies)
                HttpContext.Current.Application["Currency_" + t.Iso3] = t;
        }

        private static Currency GetCurrencyFromReader(IDataReader reader)
        {
            return new Currency
                {
                    CurrencyID = SQLDataHelper.GetInt(reader, "CurrencyID"),
                    Value = SQLDataHelper.GetDecimal(reader, "CurrencyValue"),
                    Iso3 = SQLDataHelper.GetString(reader, "CurrencyIso3"),
                    Symbol = SQLDataHelper.GetString(reader, "Code"),
                    Name = SQLDataHelper.GetString(reader, "Name"),
                    PriceFormat = SQLDataHelper.GetString(reader, "PriceFormat", string.Empty),
                    IsCodeBefore = SQLDataHelper.GetBoolean(reader, "IsCodeBefore"),
                    NumIso3 = SQLDataHelper.GetInt(reader, "CurrencyNumIso3")
                };
        }

        public static Currency CurrentCurrency
        {
            get
            {
                var collection = CommonHelper.GetCookieCollection(HttpUtility.UrlEncode( SettingsMain.SiteUrl)+ "_Currency");
                var code = (collection != null && !string.IsNullOrEmpty(collection["cCurrencyISO"])) ? collection["cCurrencyISO"] : "MissingCurrency";
                return (Currency)HttpContext.Current.Application["Currency_" + code] ?? GetAllCurrencies().FirstOrDefault();
            }
            set
            {
                CommonHelper.SetCookieCollection(HttpUtility.UrlEncode(SettingsMain.SiteUrl) + "_Currency", new NameValueCollection { { "cCurrencyISO", value.Iso3 } }, new TimeSpan(7, 0, 0, 0));
            }
        }

        public static Currency Currency(string iso3)
        {
            return (Currency)HttpContext.Current.Application["Currency_" + iso3];
        }

        public static List<Currency> GetAllCurrencies(bool fromCache)
        {
            if (fromCache)
            {
                string strCacheName = CacheNames.GetCurrenciesCacheObjectName();
                List<Currency> res;
                if (CacheManager.Contains(strCacheName))
                {
                    res = CacheManager.Get<List<Currency>>(strCacheName);
                    if (res != null)
                        return res;
                }
                res = GetAllCurrencies();

                if (res != null)
                    CacheManager.Insert(strCacheName, res);
                else
                    CacheManager.Remove(strCacheName);

                return res;
            }
            return GetAllCurrencies();
        }

        public static List<Currency> GetAllCurrencies()
        {
            return SQLDataAccess.ExecuteReadList<Currency>("SELECT Catalog.Currency.* FROM Catalog.Currency", CommandType.Text, GetCurrencyFromReader);
        }

        public static Currency GetCurrency(int idCurrency)
        {
            return SQLDataAccess.ExecuteReadOne<Currency>("SELECT Catalog.Currency.* FROM Catalog.Currency where  CurrencyID = @id", CommandType.Text, GetCurrencyFromReader, new SqlParameter("@id", idCurrency));
        }

        public static IEnumerable<int> GetAllCurrencyId()
        {

            return SQLDataAccess.ExecuteReadColumnIEnumerable<int>("SELECT CurrencyID FROM [Catalog].[Currency]", CommandType.Text, "CurrencyID");
        }

        public static void DeleteCurrency(int idCurrency)
        {
            SQLDataAccess.ExecuteNonQuery("DELETE FROM [Catalog].[Currency] where CurrencyID = @id", CommandType.Text, new SqlParameter("@id", idCurrency));
        }

        public static void UpdateCurrency(Currency currency)
        {
            SQLDataAccess.ExecuteNonQuery(
                "Update [Catalog].[Currency] set Name=@name, Code=@code, CurrencyValue=@value, CurrencyIso3=@ISO3, CurrencyNumIso3=@CurrencyNumIso3, IsCodeBefore=@isCodeBefore, PriceFormat=@format where CurrencyID = @id",
                CommandType.Text,
                new SqlParameter("@id", currency.CurrencyID),
                new SqlParameter("@name", currency.Name),
                new SqlParameter("@code", currency.Symbol),
                new SqlParameter("@value", currency.Value),
                new SqlParameter("@ISO3", currency.Iso3),
                new SqlParameter("@CurrencyNumIso3", currency.NumIso3 != 0 ? (object)currency.NumIso3 : DBNull.Value),
                new SqlParameter("@isCodeBefore", currency.IsCodeBefore),
                new SqlParameter("@format", currency.PriceFormat));
        }

        public static void InsertCurrency(Currency currency)
        {
            currency.CurrencyID = SQLDataAccess.ExecuteScalar<int>(
                 "INSERT INTO [Catalog].[Currency] (Name, Code, CurrencyValue, CurrencyIso3, CurrencyNumIso3, IsCodeBefore, PriceFormat) VALUES (@Name, @Code, @CurrencyValue, @CurrencyIso3, @CurrencyNumIso3, @IsCodeBefore, @PriceFormat);SELECT scope_identity();",
                 CommandType.Text,
                 new SqlParameter("@Name", currency.Name),
                 new SqlParameter("@Code", currency.Symbol),
                 new SqlParameter("@CurrencyValue", currency.Value),
                 new SqlParameter("@CurrencyIso3", currency.Iso3),
                 new SqlParameter("@CurrencyNumIso3", currency.NumIso3 != 0 ? (object)currency.NumIso3 : DBNull.Value),
                 new SqlParameter("@isCodeBefore", currency.IsCodeBefore),
                 new SqlParameter("@PriceFormat", currency.PriceFormat));
        }

        public static void UpdateCurrenciesFromCentralBank()
        {
            var di = new DailyInfo();
            DataSet ds = di.GetCursOnDate(DateTime.Today);

            try
            {
                using (var db = new SQLDataAccess())
                {
                    db.cmd.CommandText = "SELECT CurrencyIso3 FROM Catalog.Currency";
                    db.cmd.CommandType = CommandType.Text;
                    db.cmd.Parameters.Clear();

                    using (var dbUpd = new SQLDataAccess())
                    {
                        dbUpd.cmd.CommandText = "UPDATE Catalog.Currency SET CurrencyValue=@CurrencyValue, CurrencyNumIso3=@CurrencyNumIso3 WHERE CurrencyIso3=@CurrencyIso3";
                        dbUpd.cmd.CommandType = CommandType.Text;

                        db.cnOpen();
                        using (SqlDataReader read = db.cmd.ExecuteReader())
                        {
                            while (read.Read())
                            {
                                foreach (DataRow row in ds.Tables["ValuteCursOnDate"].Rows)
                                {
                                    if (read["CurrencyIso3"].ToString().ToLower() != row["VchCode"].ToString().ToLower())
                                        continue;
                                    dbUpd.cmd.Parameters.Clear();
                                    dbUpd.cmd.Parameters.AddWithValue("@CurrencyValue",
                                                                      Convert.ToDecimal(row["Vcurs"]) /
                                                                      Convert.ToDecimal(row["Vnom"]));
                                    dbUpd.cmd.Parameters.AddWithValue("@CurrencyIso3", row["VchCode"].ToString());
                                    dbUpd.cmd.Parameters.AddWithValue("@CurrencyNumIso3", row["VCode"].ToString());
                                    dbUpd.cnOpen();
                                    dbUpd.cmd.ExecuteNonQuery();
                                    dbUpd.cnClose();
                                }
                            }
                            read.Close();
                        }
                    }
                    db.cnClose();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }

        public static Decimal ConvertCurrency(decimal sum, string newCurrencyCode, string oldCurrencyCode)
        {
            return sum * Currency(oldCurrencyCode).Value / Currency(newCurrencyCode).Value;
        }
        public static Decimal ConvertCurrency(decimal sum, decimal newCurrencyValue, decimal oldCurrencyValue)
        {
            return sum * oldCurrencyValue / newCurrencyValue;
        }
    }
}

