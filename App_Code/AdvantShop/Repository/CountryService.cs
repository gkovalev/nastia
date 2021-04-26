//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdvantShop.Core;
using AdvantShop.Helpers;


namespace AdvantShop.Repository
{
    public class CountryService
    {
        public static ICollection<Country> GetAllCountries()
        {
            List<Country> result = SQLDataAccess.ExecuteReadList<Country>("SELECT * FROM [Customers].[Country] ORDER BY [CountryName] ASC",
                                                                 CommandType.Text,
                                                                 reader => new Country
                                                                               {
                                                                                   CountryID = SQLDataHelper.GetInt(reader, "CountryID"),
                                                                                   Iso2 = SQLDataHelper.GetString(reader, "CountryISO2"),
                                                                                   Iso3 = SQLDataHelper.GetString(reader, "CountryISO3"),
                                                                                   Name = SQLDataHelper.GetString(reader, "CountryName")
                                                                               });
            return result;
        }

        public static List<Country> GetAllCountryIdAndName()
        {
            List<Country> ids = SQLDataAccess.ExecuteReadList<Country>("SELECT CountryID,CountryName FROM [Customers].[Country]", CommandType.Text,
                                                          reader => new Country
                                                          {
                                                              CountryID = SQLDataHelper.GetInt(reader, "CountryID"),
                                                              Name = SQLDataHelper.GetString(reader, "CountryName")
                                                          });
            return ids;
        }

        public static List<int> GetAllCountryID()
        {
            List<int> ids = SQLDataAccess.ExecuteReadList<int>("SELECT CountryID FROM [Customers].[Country]", CommandType.Text,
                                                          reader => SQLDataHelper.GetInt(reader, "CountryID"));
            return ids;
        }

        public static void DeleteCountry(int idCountry)
        {
            SQLDataAccess.ExecuteNonQuery("DELETE FROM [Customers].[Country] where CountryID = @idCountry",
                                            CommandType.Text,
                                            new SqlParameter("@idCountry", idCountry));
        }

        public static void InsertCountry(Country country)
        {
            country.CountryID = SQLDataAccess.ExecuteScalar<int>("INSERT INTO [Customers].[Country] (CountryName, CountryISO2, CountryISO3) VALUES (@Name, @ISO2, @ISO3); SELECT scope_identity();",
                                             CommandType.Text,
                                             new SqlParameter("@Name", country.Name),
                                             new SqlParameter("@ISO2", country.Iso2),
                                             new SqlParameter("@ISO3", country.Iso3));
        }
        public static void UpdateCountry(Country country)
        {
            SQLDataAccess.ExecuteNonQuery("Update [Customers].[Country] set CountryName=@name, CountryISO2=@ISO2, CountryISO3=@ISO3 where CountryID = @id",
                                            CommandType.Text,
                                            new SqlParameter("@id", country.CountryID),
                                            new SqlParameter("@name", country.Name),
                                            new SqlParameter("@ISO2", country.Iso2),
                                            new SqlParameter("@ISO3", country.Iso3));
        }

        public static ICollection<Region> GetRegions(int countryId)
        {
            List<Region> result = SQLDataAccess.ExecuteReadList<Region>("SELECT * FROM [Customers].[Region] WHERE [CountryID] = @countryID ORDER BY [RegionName] ASC",
                                                        CommandType.Text, RegionService.ReadRegion, new SqlParameter("@countryID", countryId));
            return result;
        }

        public static ICollection<City> GetCities(int countryId)
        {
            List<City> result = SQLDataAccess.ExecuteReadList<City>("Select * From Customers.City where RegionID in (SELECT * FROM [Customers].[Region] WHERE [CountryID] = @countryID) ORDER BY [CityName] ASC",
                                                              CommandType.Text, CityService.GetFromReader, new SqlParameter("@countryID", countryId));
            return result;
        }

        public static string GetIso2(string name)
        {
            var iso2 = SQLDataAccess.ExecuteScalar<string>("SELECT [CountryISO2] FROM [Customers].[Country] WHERE CountryName = @CountryName",
                                                              CommandType.Text,
                                                              new SqlParameter("@CountryName", name));
            return iso2;
        }

        public static string GetIso2(int id)
        {

            var iso2 = SQLDataAccess.ExecuteScalar<string>("SELECT [CountryISO2] FROM [Customers].[Country] WHERE CountryID = @id",
                                                              CommandType.Text,
                                                              new SqlParameter("@id", id));
            return iso2;
        }

        public static Country GetCountry(int id)
        {
            return SQLDataAccess.ExecuteReadOne<Country>("SELECT * FROM [Customers].[Country] WHERE CountryID = @id",
                                                CommandType.Text,
                                                GetCountryFromReader,
                                                new SqlParameter("@id", id)
                );
        }

        public static Country GetCountryByISO3(string iso3)
        {

            return SQLDataAccess.ExecuteReadOne<Country>("SELECT * FROM [Customers].[Country] WHERE ISO3 = @iso3",
                                                CommandType.Text,
                                                GetCountryFromReader,
                                                new SqlParameter("@iso3", iso3)
                );
        }

        public static Country GetCountryByName(string countryName)
        {

            return SQLDataAccess.ExecuteReadOne<Country>("SELECT * FROM [Customers].[Country] WHERE CountryName = @CountryName",
                                                CommandType.Text,
                                                GetCountryFromReader,
                                                new SqlParameter("@CountryName", countryName)
                );
        }

        public static Country GetCountryByISO2(string iso2)
        {

            return SQLDataAccess.ExecuteReadOne<Country>("SELECT * FROM [Customers].[Country] WHERE ISO2 = @iso2",
                                                CommandType.Text,
                                                GetCountryFromReader,
                                                new SqlParameter("@iso2", iso2)
                );
        }
        public static Country GetCountryFromReader(SqlDataReader reader)
        {
            return new Country
                {
                    CountryID = SQLDataHelper.GetInt(reader, "CountryID"),
                    Iso2 = SQLDataHelper.GetString(reader, "CountryISO2"),
                    Iso3 = SQLDataHelper.GetString(reader, "CountryISO3"),
                    Name = SQLDataHelper.GetString(reader, "CountryName")
                };
        }


        public static string GetCountryNameById(int countryId)
        {
            var strRes = SQLDataAccess.ExecuteScalar<string>("SELECT CountryName FROM Customers.Country WHERE CountryID = @id",
                                                                CommandType.Text,
                                                                new SqlParameter("@id", countryId));
            return strRes;
        }

        public static string GetCountryISO3ById(int countryId)
        {
            var strRes = SQLDataAccess.ExecuteScalar<string>("SELECT CountryISO3 FROM Customers.Country WHERE CountryID = @id",
                                                                CommandType.Text,
                                                                new SqlParameter("@id", countryId));
            return strRes;
        }

        public static string GetCountryISO2ById(int countryId)
        {
            var strRes = SQLDataAccess.ExecuteScalar<string>("SELECT CountryISO2 FROM Customers.Country WHERE CountryID = @id",
                                                                CommandType.Text,
                                                                new SqlParameter("@id", countryId));
            return strRes;
        }

        public static int GetCountryIdByIso3(string iso3)
        {
            var intRes = SQLDataAccess.ExecuteScalar<int>("SELECT CountryID FROM Customers.Country WHERE CountryISO3 = @CountryISO3",
                                                                CommandType.Text,
                                                                new SqlParameter("@CountryISO3", iso3));
            return intRes;
        }

        public static List<int> GetCountryIdByIp(string Ip)
        {
            long ipDec;
            try
            {
                if (Ip == "::1")
                    ipDec = 127 * 16777216 + 1;
                else
                {
                    string[] ip = Ip.Split('.');
                    ipDec = (Int32.Parse(ip[0])) * 16777216 + (Int32.Parse(ip[1])) * 65536 + (Int32.Parse(ip[2])) * 256 + Int32.Parse(ip[3]);
                }
            }
            catch (Exception)
            {
                ipDec = 127 * 16777216 + 1;
            }
            List<int> ids = SQLDataAccess.ExecuteReadList<int>("SELECT CountryID FROM Customers.Country WHERE CountryISO2 = (SELECT country_code FROM Customers.GeoIP WHERE begin_num <= @IP AND end_num >= @IP)",
                                                         CommandType.Text,
                                                         reader => SQLDataHelper.GetInt(reader, "CountryID"), new SqlParameter("@IP", ipDec));
            return ids;
        }

        public static List<string> GetCountryNameByIp(string Ip)
        {
            long ipDec;
            try
            {
                if (Ip == "::1")
                    ipDec = 127 * 16777216 + 1;
                else
                {
                    string[] ip = Ip.Split('.');
                    ipDec = (Int32.Parse(ip[0])) * 16777216 + (Int32.Parse(ip[1])) * 65536 + (Int32.Parse(ip[2])) * 256 + Int32.Parse(ip[3]);
                }
            }
            catch (Exception)
            {
                ipDec = 127 * 16777216 + 1;
            }

            List<string> listNames = SQLDataAccess.ExecuteReadList<string>("SELECT * FROM Customers.Country WHERE CountryISO2 = (SELECT country_code FROM Customers.GeoIP WHERE begin_num <= @IP AND end_num >= @IP)",
                                                                           CommandType.Text, reader => SQLDataHelper.GetString(reader, "CountryName"),
                                                                           new SqlParameter("@IP", ipDec)) ?? new List<string> { { "local" } };

            if (listNames.Count == 0)
                listNames.Add("local");

            return listNames;
        }

        public static int GetCountryIdByName(string countryName)
        {
            var intRes = SQLDataAccess.ExecuteScalar<int>("SELECT CountryID FROM Customers.Country WHERE CountryName = @name",
                                                          CommandType.Text,
                                                          new SqlParameter("@name", countryName));
            return intRes;
        }

        public static List<String> GetCountriesByName(string name)
        {
            List<string> list = SQLDataAccess.ExecuteReadList<string>("Select CountryName From Customers.Country WHERE CountryName like @name + '%'",
                                                              CommandType.Text,
                                                              reader => SQLDataHelper.GetString(reader, "CountryName"), new SqlParameter("@name", name));
            return list;
        }
    }
}