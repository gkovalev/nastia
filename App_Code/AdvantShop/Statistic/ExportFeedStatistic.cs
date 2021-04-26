//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Threading;

namespace AdvantShop.Statistic
{
    public sealed class ExportFeedStatistic
    {
        public static ExportFeedStatisticData Data = new ExportFeedStatisticData();

        private static readonly object SyncObject = new object();

        public static void Init()
        {
            Data = new ExportFeedStatisticData();
        }

        public static string FileName
        {
            get { lock (SyncObject) { return Data.FileName; } }
            set { lock (SyncObject) { Data.FileName = value; } }
        }

        public static string FileSize
        {
            get { lock (SyncObject) { return Data.FileSize; } }
            set { lock (SyncObject) { Data.FileSize = value; } }
        }

        public static int TotalProducts
        {
            get { lock (SyncObject) { return Data.TotalProducts; } }
            set { lock (SyncObject) { Data.TotalProducts = value; } }
        }

        public static int CurrentProduct
        {
            get { lock (SyncObject) { return Data.CurrentProduct; } }
            set { lock (SyncObject) { Data.CurrentProduct = value; } }
        }

        public static int CurrentCategory
        {
            get { lock (SyncObject) { return Data.CurrentCategory; } }
            set { lock (SyncObject) { Data.CurrentCategory = value; } }
        }

        public static int TotalCategories
        {
            get { lock (SyncObject) { return Data.TotalCategories; } }
            set { lock (SyncObject) { Data.TotalCategories = value; } }
        }

        public static bool IsRun
        {
            get { lock (SyncObject) { return Data.IsRun; } }
            set { lock (SyncObject) { Data.IsRun = value; } }
        }

        public static Thread ThreadImport { get; set; }
    }

    public class ExportFeedStatisticData
    {
        public int CurrentProduct;
        public int TotalProducts;
        public int CurrentCategory;
        public int TotalCategories;
        public string FileName;
        public string FileSize;
        public bool IsRun;

        public ExportFeedStatisticData()
        {
            CurrentProduct = 0;
            TotalProducts = 0;
            CurrentCategory = 0;
            TotalCategories = 0;
            FileName = "";
            FileSize = "";
            IsRun = false;
        }
    }
}
