//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using AdvantShop.Configuration;
using AdvantShop.Core;
using AdvantShop.Core.Scheduler;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using AdvantShop.SEO;

namespace AdvantShop.ExportImport
{
    public class ExportXmlMap
    {
        private readonly string _strPhysicalTargetFolder;
        private readonly string _filenameAndPath;

        private const string DefaultChangeFreq = "daily";

        private const float DefaultPriority = 0.5f;
        private const int MaxUrlCount = 50000;
        private const int SepFileLength = 30000;


        public ExportXmlMap(string filenameAndPath, string strPhysicalTargetFolder)
        {
            _filenameAndPath = filenameAndPath;
            _strPhysicalTargetFolder = strPhysicalTargetFolder;
        }

        public void Create()
        {
            DeleteOldFiles();
            GenerateSiteMap();
        }

        private void DeleteOldFiles()
        {
            var path = Path.GetDirectoryName(_filenameAndPath);
            if (path != null)
            {
                var dir = new DirectoryInfo(path);
                foreach (var item in dir.GetFiles())
                {
                    if (item.Name.Contains("sitemap") && item.Name.Contains(".xml"))
                    {
                        FileHelpers.DeleteFile(item.FullName);
                    }
                }
            }
        }

        private void GenerateSiteMap()
        {
            int totalCount = 0;
            totalCount += SQLDataAccess.ExecuteScalar<int>("SELECT Count([CategoryId]) FROM [Catalog].[Category] WHERE [Enabled] = 1 and HirecalEnabled =1 ", CommandType.Text);
            totalCount += SQLDataAccess.ExecuteScalar<int>("SELECT Count([Product].[ProductID]) FROM [Catalog].[Product] INNER JOIN [Catalog].[ProductCategories] ON [Catalog].[Product].[ProductID] = [Catalog].[ProductCategories].[ProductID] INNER JOIN [Catalog].[Category] ON [Catalog].[Category].[CategoryID] = [Catalog].[ProductCategories].[CategoryID] AND [Catalog].[Category].[Enabled] = 1 WHERE [Product].[Enabled] = 1 and Product.[HirecalEnabled]=1 and (select Count(CategoryID) from Catalog.ProductCategories where ProductID=[Product].[ProductID])<> 0", CommandType.Text);
            totalCount += SQLDataAccess.ExecuteScalar<int>("SELECT Count([NewsID]) FROM [Settings].[News]", CommandType.Text);
            totalCount += SQLDataAccess.ExecuteScalar<int>("SELECT Count([StaticPageID]) FROM [CMS].[StaticPage] where IndexAtSiteMap=1 and enabled=1", CommandType.Text);
            totalCount += SQLDataAccess.ExecuteScalar<int>("SELECT Count([BrandID]) FROM [Catalog].[Brand] where enabled=1", CommandType.Text);

            if (totalCount > MaxUrlCount)
            {
                CreateMultipleXml(totalCount, _filenameAndPath, _strPhysicalTargetFolder);
            }
            else
            {
                CreateSimpleXml(_filenameAndPath);
            }
        }

        private void CreateMultipleXml(int totalCount, string strFinalFilePath, string strInitFileVirtualPath)
        {
            int intervals = totalCount / SepFileLength;
            if (totalCount % SepFileLength > 0)
                intervals += 1;

            CreateXmlMap(intervals, strFinalFilePath, strInitFileVirtualPath);
            CreateXmlFiles(intervals, strFinalFilePath);
        }

        /// <summary>
        /// create xml file of all catalog
        /// </summary>
        private void CreateXmlFiles(int intervals, string strFinalFilePath)
        {
            string fname = strFinalFilePath.Replace(".xml", "");
            int currentFile = 0;
            string filePath = string.Format("{0}_{1}.xml", fname, currentFile);

            var outputFile = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);

            var writer = XmlWriter.Create(outputFile);
            try
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("sitemapindex", "http://www.sitemaps.org/schemas/sitemap/0.9");

                string sqlcommand =
                    "SELECT [CategoryId], [UrlPath] FROM [Catalog].[Category] WHERE [Enabled] = 1 and HirecalEnabled=1 and CategoryID <> 0;";
                sqlcommand +=
                    @"SELECT [Product].[ProductID], [Product].[UrlPath], [Product].[DateModified]" +
                    " FROM [Catalog].[Product] INNER JOIN [Catalog].[ProductCategories] ON [Catalog].[Product].[ProductID] = [Catalog].[ProductCategories].[ProductID]" +
                    " INNER JOIN [Catalog].[Category] ON [Catalog].[Category].[CategoryID] = [Catalog].[ProductCategories].[CategoryID] " +
                    " AND [Catalog].[Category].[Enabled] = 1 WHERE [Product].[Enabled] = 1 and [Product].[HirecalEnabled]=1 and (select Count(CategoryID) from Catalog.ProductCategories where ProductID=[Product].[ProductID]) <> 0;";

                sqlcommand += "SELECT [NewsID], [UrlPath], [AddingDate] FROM [Settings].[News];";
                sqlcommand += "SELECT [StaticPageID], [UrlPath],[ModifyDate] FROM [CMS].[StaticPage] where IndexAtSiteMap=1 and enabled=1;";

                sqlcommand += "SELECT [BrandID], [UrlPath] FROM [Catalog].[Brand] where enabled=1;";

                var i = 0;
                int recordInfile = 0;
                const int countNextResult = 4;
                using (var db = new SQLDataAccess())
                {
                    db.cmd.CommandText = sqlcommand;
                    db.cnOpen();
                    using (var reader = db.cmd.ExecuteReader())
                    {
                        while (i < countNextResult)
                        {
                            while (reader.Read())
                            {
                                if (recordInfile < SepFileLength)
                                {
                                    WriteLine(GetSiteMapDataFromReader(reader), writer);
                                    recordInfile++;
                                }
                                else
                                {
                                    writer.Flush();
                                    writer.Close();
                                    outputFile.Dispose();
                                    currentFile++;
                                    recordInfile = 0;
                                    string filePathLoop = string.Format("{0}_{1}.xml", fname, currentFile);
                                    outputFile = new FileStream(filePathLoop, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                                    writer = XmlWriter.Create(outputFile);
                                    writer.WriteStartDocument();
                                    writer.WriteStartElement("sitemapindex", "http://www.sitemaps.org/schemas/sitemap/0.9");
                                }
                            }
                            reader.NextResult();
                            i++;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex, TaskManager.TaskManagerInstance().GetTasks());
            }
            finally
            {
                writer.Flush();
                writer.Close();
                outputFile.Dispose();
            }
        }


        /// <summary>
        /// create xml mapping
        /// </summary>
        /// <param name="intervals"></param>
        /// <param name="strFinalFilePath"></param>
        /// <param name="strInitFileVirtualPath"></param>
        private void CreateXmlMap(int intervals, string strFinalFilePath, string strInitFileVirtualPath)
        {
            string fname = strFinalFilePath.Replace(".xml", "");
            using (var outputFile = new FileStream(strFinalFilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
            {
                using (var writer = XmlWriter.Create(outputFile))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("sitemapindex", "http://www.sitemaps.org/schemas/sitemap/0.9");

                    for (int i = 0; i < intervals; i++)
                    {
                        string filePath = string.Format("{0}_{1}.xml", fname, i);
                        writer.WriteStartElement("sitemap");

                        writer.WriteStartElement("loc");
                        writer.WriteString(SettingsMain.SiteUrl + "/" +  filePath.Split('\\').Last());
                        writer.WriteEndElement();

                        writer.WriteStartElement("lastmod");
                        writer.WriteString(DateTime.Now.ToString("yyyy-MM-dd"));
                        writer.WriteEndElement();

                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                    writer.Flush();
                    writer.Close();
                }
            }
        }

        private void CreateSimpleXml(string fileName)
        {
            try
            {
                using (var outputFile = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (var writer = XmlWriter.Create(outputFile))
                    {
                        writer.WriteStartDocument();
                        writer.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");

                        //**************data
                        string sqlcommand = "SELECT [CategoryId], [UrlPath] FROM [Catalog].[Category] WHERE [Enabled] = 1 and HirecalEnabled =1 and CategoryID <> 0;";
                        sqlcommand += @"SELECT [Product].[ProductID], [Product].[UrlPath], [Product].[DateModified] FROM [Catalog].[Product] " +
                                        "INNER JOIN [Catalog].[ProductCategories] ON [Catalog].[Product].[ProductID] = [Catalog].[ProductCategories].[ProductID] " +
                                        "INNER JOIN [Catalog].[Category] ON [Catalog].[Category].[CategoryID] = [Catalog].[ProductCategories].[CategoryID] " +
                                        "AND [Catalog].[Category].[Enabled] = 1 WHERE [Product].[Enabled] = 1 and [Product].[HirecalEnabled] = 1 and (select Count(CategoryID) from Catalog.ProductCategories where ProductID=[Product].[ProductID])<> 0; ";
                        sqlcommand += "SELECT [NewsID], News.[UrlPath], [AddingDate] FROM [Settings].[News];";
                        sqlcommand += "SELECT [StaticPageID], [UrlPath],[ModifyDate] FROM [CMS].[StaticPage] where IndexAtSiteMap=1 and enabled=1;";
                        sqlcommand += "SELECT [BrandID], [UrlPath] FROM [Catalog].[Brand] where enabled=1;";

                        const int countNextResult = 4;
                        var i = 0;
                        using (var db = new SQLDataAccess())
                        {
                            db.cmd.CommandText = sqlcommand;
                            db.cnOpen();
                            using (var reader = db.cmd.ExecuteReader())
                            {
                                while (i <= countNextResult)
                                {
                                    while (reader.Read())
                                    {
                                        WriteLine(GetSiteMapDataFromReader(reader), writer);
                                    }
                                    if (i != 0)
                                        reader.NextResult();
                                    i++;
                                }
                            }
                        }
                        //**************

                        writer.WriteEndElement();
                        writer.WriteEndDocument();

                        writer.Flush();
                        writer.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex, TaskManager.TaskManagerInstance().GetTasks());
            }
        }

        /// <summary>
        /// write kine to xml
        /// </summary>
        /// <param name="item"></param>
        /// <param name="writer"></param>
        private void WriteLine(SiteMapData item, XmlWriter writer)
        {
            writer.WriteStartElement("url");
            // url -------------

            writer.WriteStartElement("loc");
            writer.WriteString(item.Loc);
            writer.WriteEndElement();

            writer.WriteStartElement("lastmod");
            writer.WriteString(item.Lastmod.ToString("yyyy-MM-dd"));
            writer.WriteEndElement();

            writer.WriteStartElement("changefreq");
            writer.WriteString(item.Changefreq);
            writer.WriteEndElement();

            writer.WriteStartElement("priority");
            writer.WriteString(item.Priority.ToString(CultureInfo.InvariantCulture));
            writer.WriteEndElement();

            // url -------------
            writer.WriteEndElement();
        }

        /// <summary>
        /// return data from reader
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static SiteMapData GetSiteMapDataFromReader(SqlDataReader reader)
        {
            var prefUrl = SettingsMain.SiteUrl + "/";
            var siteMapData = new SiteMapData
            {
                Changefreq = DefaultChangeFreq,
                Priority = DefaultPriority
            };

            if (reader.GetName(0).ToLower() == "categoryid")
            {
                siteMapData.Loc = prefUrl + UrlService.GetLink(ParamType.Category, SQLDataHelper.GetString(reader, "UrlPath"), SQLDataHelper.GetInt(reader, "CategoryId"));
                siteMapData.Lastmod = DateTime.Now;
            }
            else if (reader.GetName(0).ToLower() == "productid")
            {
                siteMapData.Loc = prefUrl + UrlService.GetLink(ParamType.Product, SQLDataHelper.GetString(reader, "UrlPath"), SQLDataHelper.GetInt(reader, "Productid"));
                siteMapData.Lastmod = SQLDataHelper.GetDateTime(reader, "DateModified");
            }
            else if (reader.GetName(0).ToLower() == "newsid")
            {
                siteMapData.Loc = prefUrl + UrlService.GetLink(ParamType.News, SQLDataHelper.GetString(reader, "UrlPath"), SQLDataHelper.GetInt(reader, "NewsID"));
                siteMapData.Lastmod = SQLDataHelper.GetDateTime(reader, "AddingDate");
            }
            else if (reader.GetName(0).ToLower() == "staticpageid")
            {
                siteMapData.Loc = prefUrl + UrlService.GetLink(ParamType.StaticPage, SQLDataHelper.GetString(reader, "UrlPath"), SQLDataHelper.GetInt(reader, "StaticPageID"));
                siteMapData.Lastmod = SQLDataHelper.GetDateTime(reader, "ModifyDate");
            }

            else if (reader.GetName(0).ToLower() == "brandid")
            {
                siteMapData.Loc = prefUrl + UrlService.GetLink(ParamType.Brand, SQLDataHelper.GetString(reader, "UrlPath"), SQLDataHelper.GetInt(reader, "BrandID"));
                siteMapData.Lastmod = DateTime.Now;
            }

            return siteMapData;
        }

    }
}