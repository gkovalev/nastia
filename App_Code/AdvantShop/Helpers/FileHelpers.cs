//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.Modules;
using ICSharpCode.SharpZipLib.Zip;
using System.Net;

// class for work with file, managed
namespace AdvantShop.Helpers
{
    public class FileHelpers
    {
        /// <summary>
        /// Delete file if it is exist
        /// </summary>
        /// <param name="fullname"></param>
        public static void DeleteFile(string fullname)
        {
            if (File.Exists(fullname))
            {
                File.Delete(fullname);
            }
        }

        public static void DeleteDirectory(string directoryName)
        {
            try
            {
                if (Directory.Exists(directoryName))
                {
                    string[] files = Directory.GetFiles(directoryName);
                    string[] dirs = Directory.GetDirectories(directoryName);

                    foreach (string file in files)
                    {
                        File.SetAttributes(file, FileAttributes.Normal);
                        File.Delete(file);
                    }

                    foreach (string dir in dirs)
                    {
                        DeleteDirectory(dir);
                    }
                    Directory.Delete(directoryName, false);
                }
            }
            catch (IOException)  // thx MS!
            {
                System.Threading.Thread.Sleep(0);
                Directory.Delete(directoryName, false);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex, directoryName);
            }
        }

        public static void CreateDirectory(string strPath)
        {
            if (!Directory.Exists(strPath))
            {
                Directory.CreateDirectory(strPath);
            }
        }

        public static void CreateFile(string filename)
        {
            if (!File.Exists(filename))
            {
                File.Create(filename).Dispose();
            }
        }

        public static void DeleteFilesFromPath(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                foreach (string file in Directory.GetFiles(directoryPath))
                {
                    System.Threading.Thread.Sleep(0);
                    File.Delete(file);
                }
            }
        }

        public static void DeleteFilesFromImageTemp()
        {
            DeleteFilesFromPath(FoldersHelper.GetPathAbsolut(FolderType.ImageTemp));
        }

        public static void DeleteFilesFromImageTempInBackground()
        {
            var temp = new System.Threading.Thread(DeleteFilesFromImageTemp) { IsBackground = true };
            temp.Start();
        }

        public static void UpdateDirectories()
        {
            var pictDirs = new List<string>
                                       {                                          
                                           FoldersHelper.GetPathAbsolut(FolderType .Product),
                                           FoldersHelper.GetPathAbsolut(FolderType .News),
                                           FoldersHelper.GetPathAbsolut(FolderType .Category),
                                           FoldersHelper.GetPathAbsolut(FolderType .BrandLogo),
                                           FoldersHelper.GetPathAbsolut(FolderType .Carousel),
                                       };
            pictDirs.AddRange(FoldersHelper.ProductPhotoPrefix.Select(kvp => FoldersHelper.GetImageProductPathAbsolut(kvp.Key, string.Empty)));
            foreach (var directory in pictDirs.Where(dir => (!Directory.Exists(dir) && dir.Trim().Length != 0)))
            {
                Directory.CreateDirectory(directory);
            }
        }

        public static void SaveFile(string fullname, Stream serverFileStream)
        {
            try
            {
                const int length = 1024;
                var buffer = new Byte[length];
                serverFileStream.Position = 0;
                // write the required bytes
                using (var fs = new FileStream(fullname, FileMode.Create))
                {
                    int bytesRead;
                    do
                    {
                        bytesRead = serverFileStream.Read(buffer, 0, length);
                        fs.Write(buffer, 0, bytesRead);
                    }
                    while (bytesRead == length);
                }

                //serverFileStream.Dispose();
            }
            catch (Exception ex)
            {
                //serverFileStream.Dispose();
                Debug.LogError(ex, "at SaveFile");
            }
        }

        public static void SaveResizePhotoFile(string resultPath, int maxWidth, int maxHeight, Image image)
        {
            UpdateDirectories();

            double resultWidth = image.Width;  // 0;
            double resultHeight = image.Height; // 0;

            if ((maxHeight != 0) && (image.Height > maxHeight))
            {
                resultHeight = maxHeight;
                resultWidth = (image.Width * resultHeight) / image.Height;
            }

            if ((maxWidth != 0) && (resultWidth > maxWidth))
            {
                resultHeight = (resultHeight * maxWidth) / resultWidth; // (resultHeight * resultWidth) / resultHeight;
                resultWidth = maxWidth;
            }

            try
            {
                using (var result = new Bitmap((int)resultWidth, (int)resultHeight))
                {
                    result.MakeTransparent();
                    using (var graphics = Graphics.FromImage(result))
                    {
                        graphics.CompositingQuality = CompositingQuality.HighQuality;
                        graphics.SmoothingMode = SmoothingMode.HighQuality;
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        graphics.DrawImage(image, 0, 0, (int)resultWidth, (int)resultHeight);

                        graphics.Flush();
                        var ext = Path.GetExtension(resultPath);
                        var encoder = GetEncoder(ext);
                        using (var myEncoderParameters = new EncoderParameters(3))
                        {
                            myEncoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, 100L);
                            myEncoderParameters.Param[1] = new EncoderParameter(Encoder.ScanMethod, (int)EncoderValue.ScanMethodInterlaced);
                            myEncoderParameters.Param[2] = new EncoderParameter(Encoder.RenderMethod, (int)EncoderValue.RenderProgressive);
                            using (var stream = new FileStream(resultPath, FileMode.Create))
                            {
                                result.Save(stream, encoder, myEncoderParameters);
                                stream.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex, "Error on upload " + resultPath);
            }
        }

        private static ImageCodecInfo GetEncoder(string fileExt)
        {
            fileExt = fileExt.TrimStart(".".ToCharArray()).ToLower().Trim();
            string res;
            switch (fileExt)
            {
                case "jpg":
                case "jpeg":
                    res = "image/jpeg";
                    break;
                case "png":
                    res = "image/png";
                    break;
                case "gif":
                    //if need transparency
                    //res = "image/png";
                    res = "image/gif";
                    break;
                default:
                    res = "image/jpeg";
                    break;
            }

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            return codecs.FirstOrDefault(codec => codec.MimeType == res);
        }

        public static void SaveResizePhotoFile(ProductImageType type, Image image, string destName)
        {
            var size = PhotoService.GetImageMaxSize(type);
            SaveResizePhotoFile(FoldersHelper.GetImageProductPathAbsolut(type, destName), size.Width, size.Height, image);
        }

        public static void SaveProductImageUseCompress(string destName, Image image, bool skipOriginal = false)
        {
            UpdateDirectories();

            if (!skipOriginal)
                image.Save(FoldersHelper.GetImageProductPathAbsolut(ProductImageType.Original, destName));

            ModulesRenderer.ProcessPhoto(image);

            if (SettingsCatalog.CompressBigImage)
            {
                SaveResizePhotoFile(ProductImageType.Big, image, destName);
            }
            else
            {
                image.Save(FoldersHelper.GetImageProductPathAbsolut(ProductImageType.Big, destName));
            }

            SaveResizePhotoFile(ProductImageType.Middle, image, destName);
            SaveResizePhotoFile(ProductImageType.Small, image, destName);
            SaveResizePhotoFile(ProductImageType.XSmall, image, destName);
        }

       
        //zipfolder
        public static bool ZipFiles(string inputFolderPath, string outputPathAndFile, string password, bool recurse)
        {
            try
            {
                var itemsList = GenerateFileList(inputFolderPath, recurse); // generate file list
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
                Debug.LogError(ex);
                return false;
            }
        }

        private static List<string> GenerateFileList(string dir, bool recurse)
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
                    files.AddRange(GenerateFileList(dirs, true));
                }
            return files; // return file list
        }

        public static bool CanUnZipFile(string inputPathOfZipFile)
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

        public static string RemoveInvalidFileNameChars(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || !Path.GetInvalidFileNameChars().Any(fileName.Contains))
                return fileName;

            return Path.GetInvalidFileNameChars().Where(item=>item.ToString() != "\\" && item.ToString() != "/" ).Aggregate(fileName, (current, charInvalid) => current.Replace(charInvalid.ToString(), string.Empty));
        }

        public static string RemoveInvalidPathChars(string path)
        {
            List<char> invalidChars = Path.GetInvalidPathChars().ToList();
            //invalidChars.Add(Path.DirectorySeparatorChar);
            //invalidChars.Add(Path.AltDirectorySeparatorChar);
            //invalidChars.Add(Path.PathSeparator);
            //invalidChars.Add(Path.VolumeSeparatorChar);

            if (string.IsNullOrEmpty(path) || !invalidChars.Any(path.Contains))
                return path;

            return invalidChars.Aggregate(path, (current, charInvalid) => current.Replace(charInvalid.ToString(), string.Empty));
        }

        //unzip in same folder
        public static bool UnZipFile(string inputPathOfZipFile)
        {
            return UnZipFile(inputPathOfZipFile, inputPathOfZipFile);
        }

        public static bool UnZipFile(string inputPathOfZipFile, string outputPathOfZipFile)
        {
            bool ret = true;
            try
            {
                if (File.Exists(inputPathOfZipFile))
                {
                    string baseDirectory = Path.GetDirectoryName(outputPathOfZipFile);

                    using (var zipStream = new ZipInputStream(File.OpenRead(inputPathOfZipFile)))
                    {
                        //check Available unzip, also can chack with zipStream.CanDecompressEntry
                        if (!CanUnZipFile(inputPathOfZipFile))
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
                                    string strNewFile = @"" + baseDirectory + @"\" + RemoveInvalidFileNameChars(theEntry.Name);
                                    DeleteFile(strNewFile);
                                    
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
                                string strNewDirectory = @"" + baseDirectory + @"\" + RemoveInvalidPathChars(theEntry.Name);
                                CreateDirectory(strNewDirectory);
                            }
                        }
                        zipStream.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ret = false;
                Debug.LogError(ex);
            }
            return ret;
        }

        // Unzip with folders and files
        public static bool UnZipFilesAndFolders(string inputPathOfZipFile)
        {
            bool result = true;

            try
            {
                string baseDirectory = Path.GetDirectoryName(inputPathOfZipFile);

                using (var s = new ZipInputStream(File.OpenRead(inputPathOfZipFile)))
                {
                    ZipEntry theEntry;
                    while ((theEntry = s.GetNextEntry()) != null)
                    {
                        string directoryName = Path.GetDirectoryName(baseDirectory + @"\" + theEntry.Name);
                        string fileName = Path.GetFileName(baseDirectory + @"\" + theEntry.Name);

                        if (directoryName.Length > 0)
                            Directory.CreateDirectory(directoryName);

                        if (fileName != String.Empty)
                            using (FileStream streamWriter = File.Create(baseDirectory + @"\" + theEntry.Name))
                            {
                                int size = 2048;
                                byte[] data = new byte[2048];
                                while (true)
                                {
                                    size = s.Read(data, 0, data.Length);
                                    if (size > 0)
                                        streamWriter.Write(data, 0, size);
                                    else
                                        break;
                                }
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                Debug.LogError(ex);
            }
            return result;
        }

        public static void DeleteCombineCssJs(bool forAdmin)
        {
            const string pattern = "combined_";
            foreach (string file in Directory.GetFiles(SettingsGeneral.AbsolutePath + (forAdmin ? "admin/" : string.Empty) + "css"))
            {
                if (file.Contains(pattern))
                {
                    DeleteFile(file);
                }
            }
            foreach (string file in Directory.GetFiles(SettingsGeneral.AbsolutePath + (forAdmin ? "admin/" : string.Empty) + "js"))
            {
                if (file.Contains(pattern))
                {
                    DeleteFile(file);
                }
            }
        }

        public static bool IsCombineCssJsExsist(bool forAdmin)
        {
            const string pattern = "combined_";
            bool isExist = false;
            foreach (string file in Directory.GetFiles(SettingsGeneral.AbsolutePath + (forAdmin ? "admin/" : string.Empty) + "css"))
            {
                if (file.Contains(pattern))
                {
                    isExist = true;
                    break;
                }
            }
            foreach (string file in Directory.GetFiles(SettingsGeneral.AbsolutePath + (forAdmin ? "admin/" : string.Empty) + "js"))
            {
                if (file.Contains(pattern))
                {
                    isExist = isExist & true;
                    break;
                }
            }
            return isExist;
        }

        public static bool DownloadRemoteImageFile(string uri, string fileName)
        {
            var request = (HttpWebRequest)WebRequest.Create(uri);
            var response = (HttpWebResponse)request.GetResponse();

            // Check that the remote file was found. The ContentType 
            // check is performed since a request for a non-existent 
            // image file might be redirected to a 404-page, which would 
            // yield the StatusCode "OK", even though the image was not found. 
            if ((response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Moved || response.StatusCode == HttpStatusCode.Redirect)
                && response.ContentType.StartsWith("image", StringComparison.OrdinalIgnoreCase))
            {

                // if the remote file was found, download it 
                using (Stream inputStream = response.GetResponseStream())
                using (Stream outputStream = File.Create(fileName))
                {
                    var buffer = new byte[4096];
                    int bytesRead;
                    do
                    {
                        bytesRead = inputStream.Read(buffer, 0, buffer.Length);
                        outputStream.Write(buffer, 0, bytesRead);
                    } while (bytesRead != 0);
                }
                return true;
            }
            return false;
        }

        public static bool CheckImageExtension(string fileName)
        {
            var allowedImageExtensions = new List<string> { ".jpg", ".gif", ".png", ".bmp", ".jpeg" };
            if (fileName != null && fileName.Contains("."))
            {
                string extension = fileName.ToLower().Substring(fileName.LastIndexOf(".", StringComparison.Ordinal));
                return allowedImageExtensions.Contains(extension);
            }
            return false;
        }


        public static bool CheckArchiveExtension(string fileName)
        {
            var allowedArchiveExtensions = new List<string> { ".zip" };
            if (fileName != null && fileName.Contains("."))
            {
                string extension = fileName.ToLower().Substring(fileName.LastIndexOf(".", StringComparison.Ordinal));
                return allowedArchiveExtensions.Contains(extension);
            }
            return false;
        }

        public static bool CheckRootFileExtension(string fileName)
        {
            var allowedFileExtensions = new List<string> { ".txt", ".html", ".htm", ".xml" };
            if (fileName != null && fileName.Contains("."))
            {
                string extension = fileName.ToLower().Substring(fileName.LastIndexOf(".", StringComparison.Ordinal));
                return allowedFileExtensions.Contains(extension);
            }
            return false;
        }

        public static List<string> GetFilesInRootDirectory()
        {
            var filesWithPath = Directory.GetFiles(SettingsGeneral.AbsolutePath);
            return filesWithPath.Where(file => !string.IsNullOrEmpty(file) && file.Contains("\\")).Select(
                file => file.Substring(file.LastIndexOf("\\", StringComparison.Ordinal)).Replace("\\", "")).Where(name => CheckRootFileExtension(name) && IsNotSystemFile(name)).ToList();
        }

        private static bool IsNotSystemFile(string fileName)
        {
            var systemFiles = new List<string> { "app_offline", "cmsmagazine", "robots.txt" };
            if (!string.IsNullOrEmpty(fileName))
            {
                return systemFiles.All(name => !fileName.Contains(name));
            }
            return true;
        }
    }
}
