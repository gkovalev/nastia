//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Newtonsoft.Json;
using Rule = Microsoft.SqlServer.Management.Smo.Rule;
using ICSharpCode.SharpZipLib.Zip;

//using AdvantShop.Helpers;
//using AdvantShop.Configuration;

namespace Advantshop_Tools
{
    public class VersionInformations
    {
        public string lastVersion = string.Empty;
        public string versionHistory = string.Empty;
    }

    public class UpdaterService
    {
        private static readonly string ShopCodeMaskFile = HttpContext.Current.Server.MapPath("~/App_Data/shopCodeMaskFile.txt");
        private static readonly string ShopBaseMaskFile = HttpContext.Current.Server.MapPath("~/App_Data/shopBaseMaskFile.txt");
        private static readonly string UpdateSqlFile = HttpContext.Current.Server.MapPath("~/advantshop_sql.txt");
        private static readonly string UpdateSourceZipFile =
            HttpContext.Current.Server.MapPath("~/App_Data/lastVersionCode.zip");

        private static readonly string RootDirectory = HttpContext.Current.Server.MapPath("~/");
        private const string ServiceUrl = "http://update2.advantshop.net/";

        private static readonly List<string> ExclusionFoldersAndFiles =
            new List<string>{
                ".svn\\",
                "exports\\",
                "Export\\",
                "pictures\\",
                "pictures_default\\",
                "pictures_elbuz\\",
                "pictures_en\\",
                "picturesextra\\",
                "price_download\\",
                "price_temp\\",
                "upload_images\\",
                "images\\",
                "userfiles\\",
                "Lucene",
                "App_Data\\",
                "App_WebReferences",
                "ckeditor\\",
                "Web.ConnectionStrings.config",
                "Web.AppSettings.config",
                "Yamarket.xml",
                "robots.txt",
                "sitemap.html",
                "sitemap.xml" };

        #region Code mask-file
        public static bool CreateCodeMaskFile()
        {
            try
            {
                using (var outputFile = new StreamWriter(ShopCodeMaskFile))
                {
                    foreach (var advFileName in Directory.GetFiles(RootDirectory, "*.*", SearchOption.AllDirectories))
                    {
                        var fileName = advFileName.Replace(HttpContext.Current.Request.PhysicalApplicationPath, "");

                        var isExclusion = ExclusionFoldersAndFiles.Any(exclusion => fileName.Contains(exclusion));
                        if (isExclusion) continue;

                        using (HashAlgorithm hashAlg = new SHA1Managed())
                        {
                            using (Stream file = new FileStream(advFileName, FileMode.Open, FileAccess.Read))
                            {
                                byte[] hash = hashAlg.ComputeHash(file);
                                outputFile.WriteLine(fileName + ";" + BitConverter.ToString(hash));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        private static string CompareCodeMaskFiles()
        {
            var result = string.Empty;

            try
            {
                using (var streamReaderThisMaskFile = new StreamReader(ShopCodeMaskFile))
                {
                    var request = WebRequest.Create(
                        string.Format("{0}HttpHandlers/GetCodeMaskFileByVersion.ashx?version={1}&license={2}",
                        ServiceUrl,
                        (new AppSettingsReader()).GetValue("Version", typeof(String)),
                        Tools_Updater_ExecuteScalar<string>("[Settings].[sp_GetSettingValue]", CommandType.StoredProcedure, new SqlParameter { ParameterName = "@SettingName", Value = "LicKey" })));

                    request.Method = "GET";

                    var response = request.GetResponse();
                    if (response != null)
                    {
                        using (var streamReaderEtalonMaskFile = new StreamReader(response.GetResponseStream()))// todo: get file from remoute server
                        {
                            long count = 0;
                            while (streamReaderThisMaskFile.Peek() >= 0 && streamReaderEtalonMaskFile.Peek() >= 0)
                            {
                                var stringThisMaskFile = streamReaderThisMaskFile.ReadLine();
                                var stringEtalonMaskFile = streamReaderEtalonMaskFile.ReadLine();
                                ++count;

                                var isExclusion = ExclusionFoldersAndFiles.Any(exclusion => stringThisMaskFile.Contains(exclusion) || stringEtalonMaskFile.Contains(exclusion));
                                if (isExclusion) continue;

                                if (stringThisMaskFile != null && stringEtalonMaskFile != null && stringEtalonMaskFile != stringThisMaskFile)
                                {
                                    var tempStr = stringThisMaskFile.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                                    if (tempStr.Count() == 2)
                                    {
                                        result += string.Format("discrepancy in string {0}: {1}<br/>", count, tempStr[0]);
                                    }
                                    else
                                    {
                                        result += string.Format("discrepancy in string {0}: {1}<br/>", count, stringThisMaskFile);
                                    }
                                    //break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = "Error matching: " + ex;
            }

            return result;
        }

        public static string CompareCodeVersions()
        {
            if (CreateCodeMaskFile())
            {
                return CompareCodeMaskFiles();
            }

            return "error CreateCodeMaskFile";
        }
        #endregion

        #region Base mask file

        public static bool CreateBaseMaskFile()
        {
            var remoteServerName = string.Empty;
            var instanceName = string.Empty;
            var login = string.Empty;
            var password = string.Empty;
            var keyValueConnection = ConfigurationManager.ConnectionStrings["AdvantConnectionString"].ConnectionString
                                              .Replace("'", "")
                                              .Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var keyValue in keyValueConnection)
            {
                var pair = keyValue.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                if (pair.Length > 1)
                {
                    switch (pair[0].ToLower().Replace(" ", ""))
                    {
                        case "datasource":
                            remoteServerName = pair[1];
                            break;
                        case "initialcatalog":
                            instanceName = pair[1];
                            break;
                        case "userid":
                            login = pair[1];
                            break;
                        case "password":
                            password = pair[1];
                            break;
                    }
                }
            }


            CreateBaseBackup(remoteServerName, instanceName, login, password, false, ShopBaseMaskFile);

            return true;
        }

        private static string CompareBaseMaskFiles()
        {
            var result = string.Empty;

            try
            {
                using (var streamReaderThisMaskFile = new StreamReader(ShopBaseMaskFile))
                {
                    var request = WebRequest.Create(
                        string.Format("{0}HttpHandlers/GetBaseMaskFileByVersion.ashx?version={1}&license={2}",
                        ServiceUrl,
                        (new AppSettingsReader()).GetValue("Version", typeof(String)),
                        (new AppSettingsReader()).GetValue("LicKey", typeof(String))));

                    request.Method = "GET";

                    var response = request.GetResponse();
                    if (response != null)
                    {
                        using (var streamReaderEtalonMaskFile = new StreamReader(response.GetResponseStream()))// todo: get file from remoute server
                        {
                            long count = 0;
                            while (streamReaderThisMaskFile.Peek() >= 0 && streamReaderEtalonMaskFile.Peek() >= 0)
                            {
                                var stringThisMaskFile = streamReaderThisMaskFile.ReadLine();
                                var stringEtalonMaskFile = streamReaderEtalonMaskFile.ReadLine();
                                ++count;

                                var isExclusion = ExclusionFoldersAndFiles.Any(exclusion => stringThisMaskFile.Contains(exclusion) || stringEtalonMaskFile.Contains(exclusion));
                                if (isExclusion) continue;

                                if (stringThisMaskFile != null && stringEtalonMaskFile != null && stringEtalonMaskFile != stringThisMaskFile)
                                {
                                    var tempStr = stringThisMaskFile.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                                    if (tempStr.Count() == 2)
                                    {
                                        result += string.Format("discrepancy in string {0}: {1}<br/>", count, tempStr[0]);
                                    }
                                    else
                                    {
                                        result += string.Format("discrepancy in string {0}: {1}<br/>", count, stringThisMaskFile);
                                    }
                                    //break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = "Error matching: " + ex;
            }

            return result;
        }

        public static string CompareBaseVersions()
        {
            if (CreateBaseMaskFile())
            {
                return CompareBaseMaskFiles();
            }

            return "error CreateBaseMaskFile";
        }

        #endregion

        public static VersionInformations GetLastVersionInformation()
        {
            var result = new VersionInformations();
            try
            {
                var request = WebRequest.Create(string.Format("{0}/HttpHandlers/GetVersionsHistory.ashx?license={1}&version={2}",
                    ServiceUrl,
                    Tools_Updater_ExecuteScalar<string>("[Settings].[sp_GetSettingValue]", CommandType.StoredProcedure, new SqlParameter { ParameterName = "@SettingName", Value = "LicKey" }),
                    ConfigurationManager.AppSettings["Version"]));

                request.Method = "GET";

                var response = request.GetResponse();

                result = JsonConvert.DeserializeObject<VersionInformations>((new StreamReader(response.GetResponseStream())).ReadToEnd());
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public static void CreateBaseBackup()
        {
            var remoteServerName = string.Empty;
            var instanceName = string.Empty;
            var login = string.Empty;
            var password = string.Empty;
            var keyValueConnection = ConfigurationManager.ConnectionStrings["AdvantConnectionString"].ConnectionString
                                              .Replace("'", "")
                                              .Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var keyValue in keyValueConnection)
            {
                var pair = keyValue.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                if (pair.Length > 1)
                {
                    switch (pair[0].ToLower().Replace(" ", ""))
                    {
                        case "datasource":
                            remoteServerName = pair[1];
                            break;
                        case "initialcatalog":
                            instanceName = pair[1];
                            break;
                        case "userid":
                            login = pair[1];
                            break;
                        case "password":
                            password = pair[1];
                            break;
                    }
                }
            }
            CreateBaseBackup(remoteServerName, instanceName, login, password, true, HttpContext.Current.Server.MapPath("~/App_Data/bak.sql"));
        }

        public static void CreateBaseBackup(string remoteServerName, string instanceName, string login, string password, bool includeData, string outputFile)
        {
            var srvConn2 = new ServerConnection
            {
                ServerInstance = remoteServerName,
                LoginSecure = false,
                Login = login,
                Password = password
            };

            var srv3 = new Server(srvConn2);
            var database = srv3.Databases[instanceName];
            
            using (var sw = new StreamWriter(outputFile, false, System.Text.Encoding.UTF8))
            {                
                
                var options = new ScriptingOptions
                {
                    ScriptData = includeData,
                    ScriptDrops = false,
                    EnforceScriptingOptions = true,
                    ScriptSchema = true,
                    IncludeHeaders = false,
                    Indexes = true,
                    WithDependencies = true,
                    DriAllKeys = true
                };

                foreach (Schema schema in database.Schemas)
                {
                    if (!schema.IsSystemObject)
                    {
                        foreach (var st in schema.Script())
                        {
                            sw.WriteLine("GO\r\n" + st);
                        }
                    }
                }

                foreach (Table table in database.Tables)
                {
                    if (!table.IsSystemObject)
                    {
                        foreach (var st in table.EnumScript(options))
                        {
                            sw.WriteLine("GO\r\n" + st);
                        }
                    }
                }

                foreach (UserDefinedFunction function in database.UserDefinedFunctions)
                {
                    if (!function.IsSystemObject)
                    {
                        foreach (var st in function.Script())
                        {
                            sw.WriteLine("GO\r\n" + st);
                        }
                    }
                }

                foreach (Trigger trigger in database.Triggers)
                {
                    if (!trigger.IsSystemObject)
                    {
                        foreach (var st in trigger.Script())
                        {
                            sw.WriteLine("GO\r\n" + st);
                        }
                    }
                }

                foreach (View view in database.Views)
                {
                    if (!view.IsSystemObject)
                    {
                        foreach (var st in view.Script())
                        {
                            sw.WriteLine("GO\r\n" + st);
                        }
                    }
                }

                foreach (Rule rule in database.Rules)
                {
                    foreach (var st in rule.Script())
                    {
                        sw.WriteLine("GO\r\n" + st);
                    }
                }

                foreach (StoredProcedure storedProcedure in database.StoredProcedures)
                {
                    if (!storedProcedure.IsSystemObject)
                    {
                        foreach (var st in storedProcedure.Script())
                        {
                            sw.WriteLine("GO\r\n" + st);
                        }
                    }
                }
            }
        }

        public static void CreateCodeBackup()
        {
            AdvantShop_Helpers_FileHelpers_ZipFiles(HttpContext.Current.Server.MapPath("~/"), "App_Data\\dak_code.zip", string.Empty, true);
        }

        public static void CreateAdvantshopBackups()
        {
            CreateBaseBackup();
            CreateCodeBackup();
        }

        public static bool UpdateAvantshop()
        {
            //1. Download zip file
            new WebClient().DownloadFile(
                string.Format("{0}/HttpHandlers/GetLastVersion.ashx?license={1}",
                ServiceUrl,
                Tools_Updater_ExecuteScalar<string>("[Settings].[sp_GetSettingValue]", CommandType.StoredProcedure, new SqlParameter { ParameterName = "@SettingName", Value = "LicKey" })),
                UpdateSourceZipFile);

            //2. Check available unzip
            if (!File.Exists(UpdateSourceZipFile) || !AdvantShop_Helpers_FileHelpers_CanUnZipFile(UpdateSourceZipFile))
                return false;

            //3. Delete current version
            foreach (var file in Directory.GetFiles(HttpContext.Current.Server.MapPath("~/"), "*.*", SearchOption.AllDirectories))
            {
                var isExclusion = ExclusionFoldersAndFiles.Any(exclusion => file.Contains(exclusion));
                if (isExclusion) continue;

                File.Delete(file);
            }

            //4. Unzip files
            AdvantShop_Helpers_FileHelpers_UnZipFile(UpdateSourceZipFile, HttpContext.Current.Server.MapPath("~/"));

            if (File.Exists(UpdateSqlFile))
            {
                using (var sr = new StreamReader(UpdateSqlFile))
                {
                    var sqlCommand = sr.ReadToEnd();
                    if (!string.IsNullOrEmpty(sqlCommand))
                    {
                        Tools_Updater_ExecuteNonQuery(sqlCommand, CommandType.Text);
                    }
                }
                File.Delete(UpdateSqlFile);
            }

            //5. Delete zip file (удаление скаченного zip файла исходников, чтобы место не занимал)
            File.Delete(UpdateSourceZipFile);

            return true;
        }

        //*********************************************************************************************************************************************
        public static bool AdvantShop_Helpers_FileHelpers_CanUnZipFile(string inputPathOfZipFile)
        {
            int result;
            if (File.Exists(inputPathOfZipFile))
            {
                using (var zipStream = new ZipInputStream(File.OpenRead(inputPathOfZipFile)))
                {
                    zipStream.GetNextEntry();
                    result = zipStream.Available;
                }
            }
            else
            {
                return false;
            }
            return result == 1;
        }

        public static bool AdvantShop_Helpers_FileHelpers_UnZipFile(string inputPathOfZipFile, string outputPathOfZipFile)
        {
            try
            {
                if (File.Exists(inputPathOfZipFile))
                {
                    string baseDirectory = Path.GetDirectoryName(outputPathOfZipFile);

                    using (var zipStream = new ZipInputStream(File.OpenRead(inputPathOfZipFile)))
                    {
                        //check Available unzip, also can chack with zipStream.CanDecompressEntry
                        if (!AdvantShop_Helpers_FileHelpers_CanUnZipFile(inputPathOfZipFile))
                        {
                            return false;
                        }

                        ZipEntry theEntry;
                        while ((theEntry = zipStream.GetNextEntry()) != null)
                        {

                            if (theEntry.IsFile)
                            {
                                if (!string.IsNullOrEmpty(theEntry.Name))
                                {
                                    string strNewFile = @"" + baseDirectory + @"\" + theEntry.Name;
                                    if (File.Exists(strNewFile))
                                    {
                                        File.Delete(strNewFile);
                                        //continue;
                                    }

                                    using (FileStream streamWriter = File.Create(strNewFile))
                                    {
                                        int size = 2048;
                                        var data = new byte[size];
                                        while (true)
                                        {
                                            size = zipStream.Read(data, 0, data.Length);
                                            if (size > 0)
                                                streamWriter.Write(data, 0, size);
                                            else
                                                break;
                                        }
                                        streamWriter.Close();
                                    }
                                }
                            }
                            else if (theEntry.IsDirectory)
                            {
                                string strNewDirectory = @"" + baseDirectory + @"\" + theEntry.Name;
                                if (!Directory.Exists(strNewDirectory))
                                {
                                    Directory.CreateDirectory(strNewDirectory);
                                }
                            }
                        }
                        zipStream.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public static bool AdvantShop_Helpers_FileHelpers_ZipFiles(string inputFolderPath, string outputPathAndFile, string password, bool recurse)
        {
            try
            {
                var itemsList = AdvantShop_Helpers_FileHelpers_GenerateFileList(inputFolderPath, recurse); // generate file list
                int trimLength = (Directory.GetParent(inputFolderPath)).ToString().Length;
                // find number of chars to remove     // from orginal file path
                trimLength += 1; //remove '\'
                string outPath = inputFolderPath + @"\" + outputPathAndFile;
                using (var zipStream = new ZipOutputStream(File.Create(outPath))) // create zip stream
                {
                    if (!string.IsNullOrEmpty(password)) zipStream.Password = password;
                    zipStream.SetLevel(9); // maximum compression
                    var buffer = new byte[4096];
                    foreach (string item in itemsList) // for each file, generate a zipentry
                    {
                        var entry = new ZipEntry(item.Remove(0, trimLength)) { IsUnicodeText = true, DateTime = DateTime.Now };
                        zipStream.PutNextEntry(entry);

                        if (!item.EndsWith(@"/")) // if a file ends with '/' its a directory
                        {
                            using (FileStream fs = File.OpenRead(item))
                            {
                                int sourceBytes;
                                do
                                {
                                    sourceBytes = fs.Read(buffer, 0,
                                    buffer.Length);
                                    zipStream.Write(buffer, 0, sourceBytes);

                                } while (sourceBytes > 0);
                            }
                        }
                    }
                    zipStream.Finish();
                    zipStream.Close();
                    itemsList.Clear();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private static List<string> AdvantShop_Helpers_FileHelpers_GenerateFileList(string dir, bool recurse)
        {
            var files = new List<string>();
            bool empty = true;
            foreach (string file in Directory.GetFiles(dir)) // add each file in directory
            {
                files.Add(file);
                empty = false;
            }

            if (empty)
            {
                // if directory is completely empty, add it
                if (Directory.GetDirectories(dir).Length == 0)
                {
                    files.Add(dir + @"/");
                }
            }

            if (recurse)
                foreach (string dirs in Directory.GetDirectories(dir)) // recursive
                {
                    files.AddRange(AdvantShop_Helpers_FileHelpers_GenerateFileList(dirs, true));
                }
            return files; // return file list
        }

        public static void Tools_Updater_ExecuteNonQuery(string commandText, CommandType commandType, params SqlParameter[] parameters)
        {
            using (var cmd = new SqlCommand())
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AdvantConnectionString"].ConnectionString);
                cmd.CommandText = commandText;
                cmd.CommandType = commandType;
                cmd.Parameters.Clear();
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
        }

        public static TResult Tools_Updater_ExecuteScalar<TResult>(string commandText, CommandType commandType,
                                                     params SqlParameter[] parameters) where TResult : IConvertible
        {
            using (var cmd = new SqlCommand())
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["AdvantConnectionString"].ConnectionString);
                cmd.CommandText = commandText;
                cmd.CommandType = commandType;
                cmd.Parameters.Clear();
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                cmd.Connection.Open();
                object o = cmd.ExecuteScalar();
                return o is IConvertible ? (TResult)Convert.ChangeType(o, typeof(TResult)) : default(TResult);
            }
        }
        //*********************************************************************************************************************************************
    }
}