//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdvantShop.Core;
using AdvantShop.Helpers;


namespace AdvantShop.Repository
{
    public class CityService
    {
        public static int GetCityIdByName(string cityName)
        {
            int id = SQLDataAccess.ExecuteScalar<int>("SELECT CityID FROM Customers.City WHERE CityName = @name", CommandType.Text, new SqlParameter("@name", cityName));
            return id;
        }

        public static List<int> GetAllCityIDByRegion(int regionID)
        {
            List<int> ids = SQLDataAccess.ExecuteReadList<int>("SELECT CityID FROM [Customers].[City] WHERE [RegionID] = @RegionID",
                                                          CommandType.Text,
                                                          reader => SQLDataHelper.GetInt(reader, "CityID"),
                                                          new SqlParameter("@RegionID", regionID));
            return ids;
        }

        public static void UpdateCity(City city)
        {
            SQLDataAccess.ExecuteNonQuery("Update [Customers].[City] set CityName=@name, CitySort=@CitySort, RegionId=@RegionId where CityID = @id",
                                            CommandType.Text,
                                            new SqlParameter("@id", city.CityID),
                                            new SqlParameter("@name", city.Name),
                                            new SqlParameter("@CitySort", city.CitySort),
                                            new SqlParameter("@RegionID", city.RegionID));
        }

        public static void InsertCity(City city)
        {
            city.CityID = SQLDataAccess.ExecuteScalar<int>("INSERT INTO [Customers].[City] (CityName, RegionID, CitySort) VALUES (@Name, @RegionID, @CitySort);SELECT scope_identity();",
                                                CommandType.Text,
                                                new SqlParameter("@Name", city.Name),
                                                new SqlParameter("@CitySort", city.CitySort),
                                                new SqlParameter("@RegionID", city.RegionID));
        }

        public static void DeleteCity(int cityId)
        {
            SQLDataAccess.ExecuteNonQuery("DELETE FROM Customers.City WHERE CityID = @CityID", CommandType.Text, new SqlParameter("@CityID", cityId));
        }


        public static City GetCity(int cityId)
        {
            City result = SQLDataAccess.ExecuteReadOne("SELECT * FROM [Customers].[City] WHERE [CityID] = @CityId",
                                                       CommandType.Text, GetFromReader, new SqlParameter("@CityId", cityId));
            return result;
        }

        public static City GetFromReader(SqlDataReader reader)
        {
            return new City
                        {
                            CityID = SQLDataHelper.GetInt(reader, "CityID"),
                            Name = SQLDataHelper.GetString(reader, "CityName"),
                            RegionID = SQLDataHelper.GetInt(reader, "RegionID"),
                            CitySort = SQLDataHelper.GetInt(reader, "CitySort")
                        };
        }

        public static List<string> GetCitiesByName(string name)
        {
            List<string> list = SQLDataAccess.ExecuteReadList<string>("Select CityName From Customers.City WHERE CityName like @name + '%'",
                                                              CommandType.Text, reader => SQLDataHelper.GetString(reader, "CityName"),
                                                              new SqlParameter("@name", name));
            return list;
        }

        public static City GetCityByName(string name)
        {
            return SQLDataAccess.ExecuteReadOne<City>("Select * from Customers.City where CityName=@CityName", CommandType.Text, GetFromReader,
                                                      new SqlParameter { ParameterName = "@CityName", Value = name });
        }

        public static bool IsCityInCountry(int cityId, int countryId)
        {
            return SQLDataAccess.ExecuteScalar<int>("Select count(*) From Customers.City where RegionID in (SELECT RegionID FROM [Customers].[Region] WHERE [CountryID] = @countryID) and CityID=@CityID",
                                                    CommandType.Text,
                                                    new SqlParameter { ParameterName = "@CityID", Value = cityId },
                                                    new SqlParameter { ParameterName = "@countryID", Value = countryId }
                                                    ) > 0;
        }
    }
}