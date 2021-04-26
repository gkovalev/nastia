//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.IO;
using System.Text;
using System.Threading;
using AdvantShop.FilePath;
using AdvantShop.Helpers;

namespace AdvantShop.Statistic
{
    public sealed class ImportStatistic
    {
        private static readonly object SyncObject = new object();
        private static ImportStatisticData _data = new ImportStatisticData();
        public static readonly string VirtualFileLogPath = FoldersHelper.GetPath(FolderType.PriceTemp, "ImportLog.txt", true);
        public static readonly string FileLog = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp, "ImportLog.txt");

        static public void Init()
        {
            RowPosition = 0; // так как строка в экселя начинаеться с 1
            TotalRow = 0;
            IsRun = false;
            TotalUpdateRow = 0;
            TotalAddRow = 0;
            TotalErrorRow = 0;
            FileHelpers.DeleteFile(FileLog);
        }

        public static ImportStatisticData Data
        {
            get { return _data; }
        }

        public static long TotalRow
        {
            get
            {
                lock (SyncObject)
                {
                    return _data.Total;
                }
            }
            set
            {
                lock (SyncObject)
                {
                    _data.Total = value;
                }
            }
        }

        public static long RowPosition
        {
            get
            {
                lock (SyncObject)
                {
                    return _data.Processed;
                }
            }
            set
            {
                lock (SyncObject)
                {
                    _data.Processed = value;
                }
            }
        }

        public static bool IsRun
        {
            get
            {
                lock (SyncObject)
                {
                    return _data.IsRun;
                }
            }
            set
            {
                lock (SyncObject)
                {
                    _data.IsRun = value;
                }
            }
        }

        public static long TotalUpdateRow
        {
            get
            {
                lock (SyncObject)
                {
                    return _data.Update;
                }
            }
            set
            {
                lock (SyncObject)
                {
                    _data.Update = value;
                }
            }
        }

        public static long TotalAddRow
        {
            get
            {
                lock (SyncObject)
                {
                    return _data.Add;
                }
            }
            set
            {
                lock (SyncObject)
                {
                    _data.Add = value;
                }
            }
        }

        public static long TotalErrorRow
        {
            get
            {
                lock (SyncObject)
                {
                    return _data.Error;
                }
            }
            set
            {
                lock (SyncObject)
                {
                    _data.Error = value;
                }
            }
        }

        public static Thread ThreadImport { get; set; }


        public static void WriteLog(string message)
        {
            lock (SyncObject)
            {
                using (var fs = new FileStream(FileLog, FileMode.Append, FileAccess.Write))
                using (var sw = new StreamWriter(fs, Encoding.UTF8))
                    sw.WriteLine(message);
            }
        }
    }
}