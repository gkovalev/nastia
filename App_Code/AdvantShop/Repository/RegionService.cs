//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdvantShop.Core;
using AdvantShop.Helpers;
using AjaxControlToolkit;

namespace AdvantShop.Repository
{
    public class RegionService
    {
        public static List<CascadingDropDownNameValue> GetCoutries()
        {
            List<CascadingDropDownNameValue> list = SQLDataAccess.ExecuteReadList<CascadingDropDownNameValue>("SELECT [CountryName], [CountryID] FROM [Customers].[Country] ORDER BY [CountryName]",
                                                                                  CommandType.Text,
                                                                                  reader => new CascadingDropDownNameValue(SQLDataHelper.GetString(reader, "CountryName"),
                                                                                                                            SQLDataHelper.GetInt(reader, "CountryID").ToString()));
            return list;
        }

        public static List<CascadingDropDownNameValue> GetRegions(string countryID)
        {
            List<CascadingDropDownNameValue> list = SQLDataAccess.ExecuteReadList("SELECT [RegionName], [RegionID] FROM [Customers].[Region] WHERE CountryID = @Id ORDER BY [RegionSort]",
                                                      CommandType.Text,
                                                      reader => new CascadingDropDownNameValue(SQLDataHelper.GetString(reader, "RegionName"),
                                                                                               SQLDataHelper.GetInt(reader, "RegionID").ToString()),
                                                      new SqlParameter("@Id", countryID));
            return list;
        }

        public static int GetRegionIdByName(string regionName)
        {
            var id = SQLDataAccess.ExecuteScalar<int>("SELECT RegionID FROM Customers.Region WHERE RegionName = @name",
                                                      CommandType.Text, new SqlParameter("@name", regionName));
            return id;
        }

        public static List<int> GetAllRegionIDByCountry(int countryID)
        {
            List<int> ids = SQLDataAccess.ExecuteReadList<int>("SELECT RegionID FROM [Customers].[Region] WHERE [CountryID] = @CountryID",
                                                          CommandType.Text,
                                                          reader => SQLDataHelper.GetInt(reader, "RegionID"),
                                                          new SqlParameter("@CountryID", countryID));
            return ids;
        }

        public static void UpdateRegion(Region region)
        {
            SQLDataAccess.ExecuteNonQuery("Update [Customers].[Region] set RegionName=@name, RegionCode=@RegionCode, RegionSort=@RegionSort where RegionID = @id",
                                            CommandType.Text,
                                            new SqlParameter("@id", region.RegionID),
                                            new SqlParameter("@name", region.Name),
                                            new SqlParameter("@RegionCode", region.RegionCode),
                                            new SqlParameter("@RegionSort", region.SortOrder));
        }

        public static void InsertRegion(Region region)
        {
            region.RegionID = SQLDataAccess.ExecuteScalar<int>("INSERT INTO [Customers].[Region] (RegionName, RegionCode, RegionSort, CountryID) VALUES (@Name, @RegionCode, @Sort, @CountryID);SELECT scope_identity();",
                                                            CommandType.Text, new SqlParameter("@Name", region.Name),
                                                            new SqlParameter("@RegionCode", region.RegionCode),
                                                            new SqlParameter("@Sort", region.SortOrder),
                                                            new SqlParameter("@CountryID", region.CountryID));
        }

        public static void DeleteRegion(int regionId)
        {
            SQLDataAccess.ExecuteNonQuery("DELETE FROM Customers.Region WHERE RegionID = @RegionID",
                                            CommandType.Text,
                                            new SqlParameter("@regionID", regionId));
        }

        public static Region GetRegion(int regionId)
        {
            var result = SQLDataAccess.ExecuteReadOne<Region>("SELECT * FROM [Customers].[Region] WHERE [RegionID] = @regionId",
                                                                 CommandType.Text, ReadRegion,
                                                                 new SqlParameter("@regionId", regionId));
            return result;
        }

        public static List<string> GetRegionsByName(string name)
        {
            List<string> list = SQLDataAccess.ExecuteReadList<string>("Select RegionName From [Customers].[Region] WHERE RegionName like @name + '%'",
                                                              CommandType.Text, reader => SQLDataHelper.GetString(reader, "RegionName"),
                                                              new SqlParameter("@name", name));
            return list;
        }

        public static Region ReadRegion(SqlDataReader reader)
        {
            var r = new Region
                        {
                            CountryID = SQLDataHelper.GetInt(reader, "CountryID"),
                            Name = SQLDataHelper.GetString(reader, "RegionName"),
                            RegionCode = SQLDataHelper.GetString(reader, "RegionCode"),
                            RegionID = SQLDataHelper.GetInt(reader, "RegionID")
                        };
            return r;
        }
    }
}