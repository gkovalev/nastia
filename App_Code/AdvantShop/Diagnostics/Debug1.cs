//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Xml;
using AdvantShop.Helpers;

namespace AdvantShop.Diagnostics
{
    public class Debug1
    {
        #region eLogErrorType enum

        public enum ELogErrorType
        {
            Error404,
            Error500,
            OtherError
        }

        #endregion
        
        private const string STR_CRITICAL_SUBJECT_FORMAT = "A critical error at {0} {1}";
        private const string STR_DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";

        private static string _subjectFormat = "On site: \'{0}\' error // {1}";

        private static string[] _logFileNames;
        private static string _pathToLogArchive;
        private static string _absolutePathToLog = "\\App_Data\\errlog\\";
        private static string _pathToRoot;

        private static readonly bool[] MailNotify;
        private static readonly string[] EmailAddress;
        private static readonly string[] ErrorZipPrefix = new[] { "Log_Err404_", "Log_Err500_", "Log_ErrHttp_" };
        private static readonly int[] MaxArchiveSize;
        private static readonly int[] MaxLogFileSize;

        private static string _version;
        private static string _configProductName;

        static Debug1()
        {
            MailNotify = new[] { EnableErr404MailNotification, EnableErr500MailNotification, EnableErrOtherMailNotification };
            EmailAddress = new[] { EmailErr404NotificationSetting, EmailErr500NotificationSetting, EmailErrOtherNotificationSetting };
            MaxArchiveSize = new[] { MaxErr404ArchiveSizeKb, MaxErr500ArchiveSizeKb, MaxErrOtherArchiveSizeKb };
            MaxLogFileSize = new[] { MaxErr404CurrentLogSizeKb, MaxErr500CurrentLogSizeKb, MaxErrOtherCurrentLogSizeKb };
        }

        public static bool ReThrowException { get; set; }

        public static void InitializeDebug(HttpContext ctx)
        {
            _pathToRoot = string.Format("{0}{1}", ctx.Server.MapPath("~"), _absolutePathToLog);
            _absolutePathToLog = string.Format("~{0}", _absolutePathToLog.Replace('\\', '/'));
            _logFileNames = new string[4];
            _logFileNames[(int)ELogErrorType.Error404] = ctx.Server.MapPath(GetVirtualLogFileName(ELogErrorType.Error404));
            _logFileNames[(int)ELogErrorType.Error500] = ctx.Server.MapPath(GetVirtualLogFileName(ELogErrorType.Error500));
            _logFileNames[(int)ELogErrorType.OtherError] = ctx.Server.MapPath(GetVirtualLogFileName(ELogErrorType.OtherError));
            _pathToLogArchive = ctx.Server.MapPath(VirtualPathToLogArchive);
            // var config = new AppSettingsReader();
            //_version = config.GetValue("Version", typeof(String)).ToString();
            _version = GetCoreConfigSettingValue("Version");
            _configProductName = GetCoreConfigSettingValue("PublicConfigProductName");
            ReThrowException = ctx.IsDebuggingEnabled;
        }

        public static void LogError(Exception ex)
        {
            LogErrorAction(ex, false);
            //LogError(new ErrorState{Exception = ex, AddMethodName = false, ParamObj = null});
        }
        public static void LogError(Exception ex, params object[] paramObj)
        {
            LogErrorAction(ex, false, paramObj);
            //LogError(new ErrorState{Exception = ex, AddMethodName = false, ParamObj = paramObj});
        }
        public static void LogError(Exception ex, bool addMethodName)
        {
            LogErrorAction(ex, addMethodName);
            //LogError(new ErrorState{Exception = ex, AddMethodName = addMethodName, ParamObj = null});
        }

        /// <summary>
        /// Get exception data from params object. CONTEXT INDEPENDENT! CAN BE USED IN THREAD!!
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="addMethodName"></param>
        /// <param name="paramObj"></param>
        public static void GetExceptionData(Exception ex, bool addMethodName, params object[] paramObj)
        {
            MethodBase method = null;
            if (addMethodName || paramObj != null && paramObj.Length > 0)
            {
                var stack = new StackTrace(ex);
                StackFrame stackfrm = stack.GetFrame(0);
                method = stackfrm.GetMethod();
                if (ex.Data.Contains("Method name") && addMethodName)
                {
                    ex.Data.Remove("Method name");
                    ex.Data.Add("Method name", method.Name);
                }
                else if (!ex.Data.Contains("Method name"))
                {
                    ex.Data.Add("Method name", method.Name);
                }
            }

            if (paramObj != null && paramObj.Length > 0)
            {
                string tmpStr;

                //  First Params
                if (method != null)
                {
                    ParameterInfo[] paramsinfo = method.GetParameters();

                    foreach (ParameterInfo p in paramsinfo)
                    {
                        tmpStr = string.Format("Parameter1 {0} name", p.Position); //  name
                        while (ex.Data.Contains(tmpStr))
                        {
                            tmpStr += '*';
                        }
                        ex.Data.Add(tmpStr, p.Name);

                        tmpStr = string.Format("Parameter1 {0} type", p.Position); //  type
                        while (ex.Data.Contains(tmpStr))
                        {
                            tmpStr += '*';
                        }
                        ex.Data.Add(tmpStr, p.ParameterType);

                        tmpStr = string.Format("Parameter1 {0} value", p.Position); //  value
                        while (ex.Data.Contains(tmpStr))
                        {
                            tmpStr += '*';
                        }
                        if (p.Position < paramObj.Length - 1)
                        {
                            ex.Data.Add(tmpStr, paramObj[p.Position] != null ? paramObj[p.Position].ToString() : "n/a");
                        }
                        else
                        {
                            ex.Data.Add(tmpStr, "n/a");
                        }
                    }
                }

                //  Second Params
                for (Int32 i = paramObj.GetLowerBound(0); i <= paramObj.GetUpperBound(0); i++)
                {
                    tmpStr = string.Format("Parameter2 {0} type", i); //  type
                    while (ex.Data.Contains(tmpStr))
                    {
                        tmpStr += '*';
                    }
                    ex.Data.Add(tmpStr, paramObj[i] != null ? paramObj[i].GetType().ToString() : "n/a");

                    tmpStr = string.Format("Parameter2 {0} value", i); //  value
                    while (ex.Data.Contains(tmpStr))
                    {
                        tmpStr += '*';
                    }
                    ex.Data.Add(tmpStr, paramObj[i] != null ? paramObj[i].ToString() : "Nothing");
                }
            }
            // return ex;
        }

        public static void LogErrorAction(Exception ex, bool addMethodName, params object[] paramObj)
        {
            //if (paramObj == null)
            //{
            //    // If there no data
            //    paramObj = new object[0];
            //}
            if (addMethodName || paramObj != null && paramObj.Length > 0)
                GetExceptionData(ex, addMethodName, paramObj);

            LogErrorAction(ex);

            if ((ReThrowException))
            {
                //throw ex;
            }
        }

        public static void LogCriticalError(Exception ex)
        {
            LogCriticalError(ex, null);
        }

        public static void LogCriticalError(Exception ex, params object[] paramObj)
        {
            string oldsubj = _subjectFormat;
            _subjectFormat = STR_CRITICAL_SUBJECT_FORMAT;
            LogError(ex, paramObj);
            _subjectFormat = oldsubj;
        }

        protected static void LogErrorAction(Exception ex)
        {
            HttpContext ctx = HttpContext.Current;

            ELogErrorType errtype = ELogErrorType.OtherError;
            if (ex is HttpException)
            {
                var hex = (HttpException)ex;

                if (hex.GetHttpCode() == 500)
                {
                    //    If ex.InnerException IsNot Nothing Then
                    //        LogError(hex.InnerException)
                    //        Exit Sub
                    //    End If
                    errtype = ELogErrorType.Error500;
                }

                if (hex.GetHttpCode() == 404)
                {
                    var refferrer = ctx.Request.UrlReferrer;
                    StartWorker(Log404Thread, new Pair(ctx.Request.Url.ToString(), refferrer == null ? null : refferrer.ToString()));

                    return;
                }
            }

            var xError = SerializeToXml(ex, ctx);

            StartWorker(LogErrorThread, new ErrorThreadState { Type = errtype, XmlError = xError });
        }

        private static void StartWorker(WaitCallback callBack, object state)
        {
            int workers;
            int ports;
            ThreadPool.GetAvailableThreads(out workers, out ports);
            if (workers > 0)
                ThreadPool.QueueUserWorkItem(callBack, state);
        }

        internal class ErrorThreadState
        {
            public string XmlError;
            public ELogErrorType Type;
        }

        private static void LogErrorThread(object state)
        {
            var state1 = (ErrorThreadState)state;
            var xError = state1.XmlError;
            var errtype = state1.Type;

            if (EnableMailNotification(errtype))
            {
                using (var memstream = new MemoryStream())
                {
                    var indwrtr = new XmlTextWriter(memstream, null) { Formatting = Formatting.Indented };
                    indwrtr.WriteString(xError);
                    indwrtr.Close();
                    byte[] buff = memstream.GetBuffer();
                    string msg = Encoding.UTF8.GetString(buff);
                    memstream.Close();
                    SendMail(EmailAddress[(int)errtype], string.Format(_subjectFormat, GetPublicConfigProductName(), ConvertDate(DateTime.Now)), msg, false);
                }
            }

            string path = GetLogFileName(errtype);
            lock (path)
            {
                if (File.Exists(path))
                {
                    var fi = new FileInfo(path);
                    if ((fi.Length / 1024) > MaxLogFileSize[(int)ELogErrorType.Error500])
                    {
                        Archivate(errtype);
                        ReduceArchive(errtype);
                        fi.Delete();
                        using (var stream = new FileStream(fi.FullName, FileMode.Create))
                            WriteSingleRecord(stream, xError);
                    }
                    else
                    {
                        using (var stream = new FileStream(path, FileMode.Open))
                        {
                            if (stream.Length > 10)
                                stream.Seek(-("</Log>".Length), SeekOrigin.End);
                            else
                            {
                                WriteSingleRecord(stream, xError);
                                return;
                            }

                            using (var wr = new StreamWriter(stream))
                            {
                                wr.Write(xError);
                                wr.Write("</Log>");
                                wr.Flush();
                                wr.Close();
                            }
                            stream.Close();
                        }
                    }



                }
                else
                {
                    using (var stream = File.Create(path))
                    {
                        WriteSingleRecord(stream, xError);
                        stream.Close();
                    }
                }
            }
        }

        private static void WriteSingleRecord(FileStream stream, string xError)
        {
            stream.Seek(0, SeekOrigin.Begin);
            var initWriter = new XmlTextWriter(stream, Encoding.UTF8);
            initWriter.WriteStartDocument();
            initWriter.WriteStartElement("Log");
            initWriter.WriteRaw(xError);
            initWriter.WriteEndElement();
            initWriter.Flush();
            initWriter.Close();
            stream.Close();
        }

        private static void Log404Thread(object urls)
        {
            var data = Get404Data((Pair)urls);
            string path = GetLogFileName(ELogErrorType.Error404);
            lock (path)
            {

                if (File.Exists(path))
                {
                    var fi = new FileInfo(path);
                    if ((fi.Length / 1024) > MaxLogFileSize[(int)ELogErrorType.Error404])
                    {
                        Archivate(ELogErrorType.Error404);
                        ReduceArchive(ELogErrorType.Error404);
                        fi.Delete();
                    }
                }

                using (var writer = new StreamWriter(File.Open(path, FileMode.Append)))
                {
                    writer.WriteLine(data);
                }
            }
        }

        private static string Get404Data(Pair contextUrls)
        {

            var url = (string)contextUrls.First;
            var referrer = (string)contextUrls.Second;
            var sbline = new StringBuilder(DateTime.Now.ToString(CultureInfo.InvariantCulture));
            sbline.Append("\t");
            sbline.Append("Type: \'HTTP 404 Error\'");
            sbline.Append("\t");
            sbline.Append("SiteName: \'");
            sbline.Append(GetPublicConfigProductName());
            sbline.Append("\'");
            sbline.Append("\t");
            sbline.Append("Url: \'");
            if (!string.IsNullOrEmpty(url))
            {
                sbline.Append(url);
            }
            sbline.Append("\' not found");
            sbline.Append("\t");
            sbline.Append("UrlReferrer: \'");
            sbline.Append(!string.IsNullOrEmpty(referrer) ? referrer : "Nothing");
            sbline.Append("\'");

            if (EnableMailNotification(ELogErrorType.Error404))
            {
                SendMail(EmailAddress[(int)ELogErrorType.Error404],
                         string.Format(_subjectFormat, GetPublicConfigProductName(), ConvertDate(DateTime.Now)),
                         sbline.ToString(), false);
            }
            return sbline.ToString();
        }

        /// <summary>
        /// Delete the oldest record from archive directory
        /// </summary>
        /// <param name="errtype"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static bool ReduceArchive(ELogErrorType errtype)
        {
            var adir = new DirectoryInfo(PathToLogArchive);
            FileInfo[] zips = adir.GetFiles(ErrorZipPrefix[(int)errtype] + "*.zip");

            long sumsize = 0;
            FileInfo oldzip = zips[0];
            foreach (FileInfo cfi in zips)
            {
                sumsize += cfi.Length;
                if (cfi.CreationTime < oldzip.CreationTime)
                {
                    oldzip = cfi;
                }
            }

            if ((sumsize / 1024) > MaxArchiveSize[(int)errtype])
            {
                oldzip.Delete();
                return true;
            }

            return false;
        }

        private static void Archivate(ELogErrorType errtype)
        {
            string path = GetLogFileName(errtype);
            var sbfilename = new StringBuilder(PathToLogArchive);

            //checking if archive directory exists
            FileHelpers.CreateDirectory(sbfilename.ToString());

            sbfilename.Append("\\");
            sbfilename.Append(ErrorZipPrefix[(int)errtype]);
            sbfilename.Append(DateTime.Now.Year.ToString());
            sbfilename.Append("_");
            sbfilename.Append(DateTime.Now.Month.ToString());
            sbfilename.Append("_");
            sbfilename.Append(DateTime.Now.Day.ToString());
            sbfilename.Append("_");
            sbfilename.Append(DateTime.Now.Hour.ToString());
            sbfilename.Append("_");
            sbfilename.Append(DateTime.Now.Minute.ToString());
            sbfilename.Append("_");
            sbfilename.Append(DateTime.Now.Second.ToString());
            sbfilename.Append(".zip");

            //stream for reading
            using (var fs = new FileStream(path, FileMode.Open))
            {
                var buffer = new byte[fs.Length + 1];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();

                //stream for writing and stream for compressing
                using (var zipfs = new FileStream(sbfilename.ToString(), FileMode.CreateNew))
                {
                    var comprZipStream = new GZipStream(zipfs, CompressionMode.Compress, true);
                    comprZipStream.Write(buffer, 0, buffer.Length);
                    comprZipStream.Close();
                    zipfs.Close();
                }
            }
        }

        private static string SerializeToXml(Exception ex, HttpContext ctxt)
        {
            var sBuilder = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(sBuilder, new XmlWriterSettings { OmitXmlDeclaration = true, Encoding = Encoding.UTF8 }))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("LogEntry");


                writer.WriteElementString("Date", ConvertDate(DateTime.Now));

                writer.WriteStartElement("SiteInfo");

                writer.WriteElementString("TitleProductName", GetPublicConfigProductName());

                writer.WriteEndElement();

                if (ex.InnerException != null)
                {
                    writer.WriteElementString("InerExceptionMessage", ex.InnerException.Message);

                    writer.WriteElementString("StInerExceptionStackTraceackTrace", ex.InnerException.StackTrace);
                }

                writer.WriteElementString("Message", ex.Message);

                writer.WriteElementString("StackTrace", ex.StackTrace);



                writer.WriteStartElement("Data");
                foreach (object key in ex.Data.Keys)
                {
                    writer.WriteStartElement("Parameter");

                    writer.WriteAttributeString("Name", key.ToString());
                    writer.WriteAttributeString("Value", ex.Data[key].ToString());

                    writer.WriteEndElement();
                }
                writer.WriteEndElement();

                if (ctxt != null)
                {
                    //Request
                    writer.WriteStartElement("Request");

                    PropertyInfo[] properties = null;
                    try
                    {
                        properties = ctxt.Request.GetType().GetProperties();
                    }
                    catch (Exception)
                    {
                    }
                    if (properties != null)
                        foreach (var p in properties)
                        {
                            if (p.Name == "Params")
                            {
                                continue;
                            }
                            writer.WriteStartElement("RequestParam");
                            writer.WriteAttributeString("Name", p.Name);

                            object o;
                            try
                            {
                                o = p.GetValue(ctxt.Request, null);
                            }
                            catch (Exception)
                            {
                                o = "";
                            }
                            if (o is NameValueCollection)
                            {
                                foreach (string key in ((NameValueCollection)o))
                                {
                                    if (key != "ALL_HTTP" && key != "ALL_RAW")
                                    {
                                        writer.WriteStartElement("Parameter");
                                        writer.WriteAttributeString("Name", key);
                                        writer.WriteAttributeString("Value", ctxt.Request.Params[key]);
                                        writer.WriteEndElement();
                                    }
                                }
                            }
                            else
                            {
                                try
                                {
                                    var val = Convert.ToString(o);
                                    writer.WriteString(val);
                                }
                                catch (Exception)
                                {
                                }
                            }


                            writer.WriteEndElement();


                        }

                    writer.WriteEndElement();

                    //Browser
                    writer.WriteStartElement("Browser");

                    properties = null;
                    try
                    {
                        properties = ctxt.Request.Browser.GetType().GetProperties();
                    }
                    catch (Exception)
                    {
                    }

                    if (properties != null)
                    {
                        foreach (PropertyInfo p in properties)
                        {
                            writer.WriteStartElement("Parameter");
                            writer.WriteAttributeString("Name", p.Name);

                            string val = string.Empty;
                            try
                            {
                                val = Convert.ToString(p.GetValue(ctxt.Request.Browser, null));
                            }
                            catch (Exception)
                            {
                            }
                            writer.WriteAttributeString("Value", val);


                            writer.WriteEndElement();
                        }
                    }

                    writer.WriteEndElement();

                    //Session
                    if (ctxt.Session != null)
                    {
                        writer.WriteStartElement("Session");

                        writer.WriteStartElement("SessionProperties");
                        properties = ctxt.Session.GetType().GetProperties();
                        foreach (var p in properties)
                        {
                            writer.WriteStartElement("Parameter");
                            writer.WriteAttributeString("Name", p.Name);

                            string val = string.Empty;
                            try
                            {
                                val = Convert.ToString(p.GetValue(ctxt.Session, null));
                            }
                            catch (Exception)
                            {
                            }
                            writer.WriteAttributeString("Value", val);



                            writer.WriteEndElement();

                        }
                        writer.WriteEndElement();

                        writer.WriteStartElement("SessionVariables");

                        foreach (string key in ctxt.Session.Keys)
                        {
                            writer.WriteStartElement("Parameter");
                            writer.WriteAttributeString("Name", key);

                            string val = string.Empty;
                            try
                            {
                                val = Convert.ToString(ctxt.Session[key]);
                            }
                            catch (Exception)
                            {
                            }
                            writer.WriteAttributeString("Value", val);


                            writer.WriteEndElement();
                        }

                        writer.WriteEndElement();


                        writer.WriteEndElement();

                    }
                }

                writer.WriteStartElement("ConfigSettings");

                writer.WriteStartElement("Parameter");
                writer.WriteAttributeString("Name", "SiteVersion");
                writer.WriteAttributeString("Value", _version);
                writer.WriteEndElement();

                writer.WriteEndElement();

                foreach (ConnectionStringSettings cs in ConfigurationManager.ConnectionStrings)
                {
                    writer.WriteStartElement("ConnectionString");
                    writer.WriteAttributeString("Name", cs.Name);
                    writer.WriteAttributeString("Value", cs.ConnectionString);
                    writer.WriteEndElement();
                }


                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();
                writer.Close();
            }
            return sBuilder.ToString();
        }

        /// <summary>
        /// Return the absolute path with filename by ErrLogType
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetLogFileName(ELogErrorType type)
        {
            if (_logFileNames != null)
            {
                return _logFileNames[(int)type];
            }
            return HttpContext.Current.Server.MapPath(GetVirtualLogFileName(type));
        }

        /// <summary>
        /// Return the absolute path with filename by ErrLogType
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static string GetVirtualLogFileName(ELogErrorType type)
        {
            if (!Directory.Exists(_pathToRoot))
            {
                Directory.CreateDirectory(_pathToRoot);
                Directory.CreateDirectory(string.Format("{0}{1}", _pathToRoot, "archive"));
            }

            switch (type)
            {
                case ELogErrorType.Error404:
                    return string.Format("{0}{1}", _absolutePathToLog, "Log_Err404.log");
                case ELogErrorType.Error500:
                    return string.Format("{0}{1}", _absolutePathToLog, "Log_Err500.xml");
                case ELogErrorType.OtherError:
                    return string.Format("{0}{1}", _absolutePathToLog, "Log_ErrHttp.xml");
            }
            return string.Format("{0}{1}", _absolutePathToLog, "Log.log");
        }

        /// <summary>
        /// Return the last activity date from log file
        /// </summary>
        /// <param name="errType"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DateTime GetLastActivityDate(ELogErrorType errType)
        {
            string path = GetLogFileName(errType);

            //if file doesn't exist return min date
            if (!File.Exists(path))
            {
                return DateTime.MinValue;
            }

            return File.GetLastWriteTime(path);
        }

        /// <summary>
        /// Return the last error (xml format)
        /// </summary>
        /// <param name="errtype"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetLastError(ELogErrorType errtype)
        {
            string path;

            if (errtype == ELogErrorType.Error404)
            {
                path = GetLogFileName(errtype);
                if (!File.Exists(path))
                {
                    return "No error";
                }
                string str = "No error";
                string lastline = "";
                using (TextReader reader = new StreamReader(path))
                {
                    while (str != null)
                    {
                        lastline = str;
                        str = reader.ReadLine();
                    }
                    reader.Close();
                }
                return lastline;
            }

            if (errtype == ELogErrorType.Error500 || errtype == ELogErrorType.OtherError)
            {
                path = GetLogFileName(errtype);
                if (!File.Exists(path))
                {
                    return "No error";
                }
                var xml = new XmlDocument();
                try
                {
                    xml.Load(path);
                    using (var memstream = new MemoryStream())
                    {
                        var xError = (XmlElement)xml.FirstChild.LastChild;
                        var indwrtr = new XmlTextWriter(memstream, null) { Formatting = Formatting.Indented };
                        xError.WriteTo(indwrtr);
                        indwrtr.Close();
                        var buff = memstream.GetBuffer();
                        memstream.Close();
                        var msg = Encoding.UTF8.GetString(buff);
                        return msg;
                    }
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
                finally
                {
                    xml = null;
                }


            }

            return "No error";
        }

        //public static TResult WithLog<TResult>(Func<TResult> code, Func<Exception, TResult> handles, params object[] pars)
        //{
        //    try
        //    {
        //        return code();
        //    }
        //    catch (Exception ex)
        //    {
        //        LogError(ex, pars);
        //        return handles(ex);
        //    }
        //}
        //public static TResult WithLog<TResult>(Func<TResult> code, Func<Exception, object[], TResult> handles, params object[] pars)
        //{
        //    return WithLog(code, ex => handles(ex, pars), pars);
        //}

        //public static TResult WithLog<TResult>(Func<TResult> code, TResult returnVal, params object[] pars)
        //{
        //    return WithLog(code, ex => returnVal, pars);
        //}
        //public static void WithLog(Action code, Action<Exception> handles, params object[] pars)
        //{
        //    try
        //    {
        //        code();
        //    }
        //    catch (Exception ex)
        //    {
        //        LogError(ex, pars);
        //        handles(ex);
        //    }
        //}
        //public static void WithLog(Action code, Action<Exception, object[]> handles, params object[] pars)
        //{
        //    WithLog(code, ex => handles(ex, pars), pars);
        //}
        //public static void WithLog(Action code, params object[] pars)
        //{
        //    WithLog(code, ex => { }, pars);
        //}


        #region  Public Properties

        public static string PathToLogArchive
        {
            get
            {
                if (_pathToLogArchive != null)
                {
                    return _pathToLogArchive;
                }
                return HttpContext.Current.Server.MapPath(VirtualPathToLogArchive);
            }
        }

        public static string VirtualPathToLogArchive
        {
            get { return string.Format("{0}{1}", _absolutePathToLog, "archive"); }
        }

        public static int MaxErr404ArchiveSizeKb
        {
            get { return int.Parse(GetCoreConfigSettingValue("errls_MaxArchiveSumSize_KBytes_Err404")); }
            set { SetCoreConfigSettingValue("errls_MaxArchiveSumSize_KBytes_Err404", value.ToString()); }
        }

        public static int MaxErr500ArchiveSizeKb
        {
            get { return int.Parse(GetCoreConfigSettingValue("errls_MaxArchiveSumSize_KBytes_Err500")); }
            set { SetCoreConfigSettingValue("errls_MaxArchiveSumSize_KBytes_Err500", value.ToString()); }
        }

        public static int MaxErrOtherArchiveSizeKb
        {
            get { return int.Parse(GetCoreConfigSettingValue("errls_MaxArchiveSumSize_KBytes_ErrOtherHttp")); }
            set { SetCoreConfigSettingValue("errls_MaxArchiveSumSize_KBytes_ErrOtherHttp", value.ToString()); }
        }

        public static int MaxErr404CurrentLogSizeKb
        {
            get { return int.Parse(GetCoreConfigSettingValue("errls_MaxCurrentLogSize_KBytes_Err404")); }
            set { SetCoreConfigSettingValue("errls_MaxCurrentLogSize_KBytes_Err404", value.ToString()); }
        }

        public static int MaxErr500CurrentLogSizeKb
        {
            get { return int.Parse(GetCoreConfigSettingValue("errls_MaxCurrentLogSize_KBytes_Err500")); }
            set { SetCoreConfigSettingValue("errls_MaxCurrentLogSize_KBytes_Err500", value.ToString()); }
        }

        public static int MaxErrOtherCurrentLogSizeKb
        {
            get { return int.Parse(GetCoreConfigSettingValue("errls_MaxCurrentLogSize_KBytes_ErrOtherHttp")); }
            set { SetCoreConfigSettingValue("errls_MaxCurrentLogSize_KBytes_ErrOtherHttp", value.ToString()); }
        }

        public static bool EnableErr404MailNotification
        {
            get { return bool.Parse(GetCoreConfigSettingValue("errls_EnableMailNotification_Err404")); }
            set { SetCoreConfigSettingValue("errls_EnableMailNotification_Err404", value.ToString()); }
        }

        public static bool EnableErr500MailNotification
        {
            get { return bool.Parse(GetCoreConfigSettingValue("errls_EnableMailNotification_Err500")); }
            set { SetCoreConfigSettingValue("errls_EnableMailNotification_Err500", value.ToString()); }
        }

        public static bool EnableErrOtherMailNotification
        {
            get { return bool.Parse(GetCoreConfigSettingValue("errls_EnableMailNotification_ErrOtherHttp")); }
            set { SetCoreConfigSettingValue("errls_EnableMailNotification_ErrOtherHttp", value.ToString()); }
        }

        public static string EmailErr404NotificationSetting
        {
            get { return GetCoreConfigSettingValue("errls_EmailNotificationAdress_Err404"); }
            set { SetCoreConfigSettingValue("errls_EmailNotificationAdress_Err404", value); }
        }

        public static string EmailErr500NotificationSetting
        {
            get { return GetCoreConfigSettingValue("errls_EmailNotificationAdress_Err500"); }
            set { SetCoreConfigSettingValue("errls_EmailNotificationAdress_Err500", value); }
        }

        public static string EmailErrOtherNotificationSetting
        {
            get { return GetCoreConfigSettingValue("errls_EmailNotificationAdress_ErrOtherHttp"); }
            set { SetCoreConfigSettingValue("errls_EmailNotificationAdress_ErrOtherHttp", value); }
        }

        public static bool EnableMailNotification(ELogErrorType errtype)
        {
            return MailNotify[(int)errtype];
        }

        public static void SetEnableMailNotification(bool value, ELogErrorType errtype)
        {
            MailNotify[(int)errtype] = value;
        }

        /// <summary>
        /// Get PublicProductName value from web.config file
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetPublicConfigProductName()
        {
            return _configProductName;
        }

        public class MailProperties
        {
            /// <summary>
            /// Get the name of the SMTP server.
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public static string Smtp
            {
                get
                {
                    System.Configuration.Configuration cfg = WebConfigurationManager.OpenWebConfiguration("~/");
                    // HttpContext.Current.Request.ApplicationPath - Can't be used at ThreadPool.QueueUserWorkItem
                    var cfgSettings = (MailSettingsSectionGroup)(cfg.GetSectionGroup("system.net/mailSettings"));
                    return cfgSettings.Smtp.Network.Host;
                }
            }

            /// <summary>
            /// Get the port that SMTP clients use to connect to an SMTP mail server. The default value is 25.
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public static int Port
            {
                get
                {
                    System.Configuration.Configuration cfg = WebConfigurationManager.OpenWebConfiguration("~/");
                    // HttpContext.Current.Request.ApplicationPath - Can't be used at ThreadPool.QueueUserWorkItem
                    var cfgSettings = (MailSettingsSectionGroup)(cfg.GetSectionGroup("system.net/mailSettings"));
                    return cfgSettings.Smtp.Network.Port;
                }
            }

            /// <summary>
            /// Get the user password to use to connect to SMTP mail server.
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public static string Password
            {
                get
                {
                    System.Configuration.Configuration cfg = WebConfigurationManager.OpenWebConfiguration("~/");
                    // HttpContext.Current.Request.ApplicationPath - Can't be used at ThreadPool.QueueUserWorkItem
                    var cfgSettings = (MailSettingsSectionGroup)(cfg.GetSectionGroup("system.net/mailSettings"));
                    return cfgSettings.Smtp.Network.Password;
                }
            }

            /// <summary>
            /// Get the login to use to connect to an SMTP mail server.
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public static string Login
            {
                get
                {
                    System.Configuration.Configuration cfg = WebConfigurationManager.OpenWebConfiguration("~/");
                    // HttpContext.Current.Request.ApplicationPath - Can't be used at ThreadPool.QueueUserWorkItem
                    var cfgSettings = (MailSettingsSectionGroup)(cfg.GetSectionGroup("system.net/mailSettings"));
                    return cfgSettings.Smtp.Network.UserName;
                }
            }

            /// <summary>
            /// Get the default value that indicates who the email message is from.
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public static string From
            {
                get
                {
                    System.Configuration.Configuration cfg = WebConfigurationManager.OpenWebConfiguration("~/");
                    // HttpContext.Current.Request.ApplicationPath - Can't be used at ThreadPool.QueueUserWorkItem
                    var cfgSettings = (MailSettingsSectionGroup)(cfg.GetSectionGroup("system.net/mailSettings"));
                    return cfgSettings.Smtp.From;
                }
            }
        }

        #endregion

        #region  Setting Provider

        public static string GetCoreConfigSettingValue(string strKey)
        {
            var config = new AppSettingsReader();
            return (string)config.GetValue(strKey, typeof(String));
        }

        public static bool SetCoreConfigSettingValue(string strKey, string strValue)
        {
            throw (new NotImplementedException());
        }

        #endregion

        #region  Helper

        public static string ConvertDate(DateTime d)
        {
            return d.ToString(STR_DATE_FORMAT);
        }

        #endregion

        #region  Send Mail

        public static bool SendMail(string strTo, string strSubject, string strText, bool isBodyHtml)
        {
            ThreadPool.QueueUserWorkItem(
                (a) =>
                SendMailAsync(strTo, strSubject, strText, isBodyHtml, MailProperties.Smtp, MailProperties.Port,
                              MailProperties.Login, MailProperties.Password, MailProperties.From));

            return true;
        }

        private static bool SendMailAsync(string strTo, string strSubject, string strText, bool isBodyHtml,
                                          string transSmtp, int transPort, string transLogin, string transPassword,
                                          string transEmailFrom)
        {
            try
            {
                using (var mail = new MailMessage())
                {
                    mail.Subject = strSubject;
                    mail.Body = strText;
                    mail.IsBodyHtml = isBodyHtml;
                    mail.From = new MailAddress(transEmailFrom);

                    string[] strEmailCollection = strTo.Split(';');
                    foreach (string toAddress in strEmailCollection)
                    {
                        mail.To.Add(new MailAddress(toAddress.Trim()));
                    }

                    var smtp = new SmtpClient
                                   {
                                       UseDefaultCredentials = false,
                                       Host = transSmtp,
                                       Port = transPort,
                                       DeliveryMethod = SmtpDeliveryMethod.Network,
                                       Credentials = new NetworkCredential(transLogin, transPassword)
                                   };

                    smtp.Send(mail);
                }
            }
            catch (Exception ex)
            {
                SetEnableMailNotification(false, ELogErrorType.Error500);
                SetEnableMailNotification(false, ELogErrorType.Error404);
                SetEnableMailNotification(false, ELogErrorType.OtherError);
                LogError(ex, strTo, strSubject);
                SetEnableMailNotification(EnableErr500MailNotification, ELogErrorType.Error500);
                SetEnableMailNotification(EnableErr404MailNotification, ELogErrorType.Error404);
                SetEnableMailNotification(EnableErrOtherMailNotification, ELogErrorType.OtherError);
            }
            return true;
        }

        #endregion

    }
}