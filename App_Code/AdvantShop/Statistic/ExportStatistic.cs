//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Threading;

namespace AdvantShop.Statistic
{
    public static class ExportStatistic
    {
        private static readonly object SyncObject = new object();
        private static ExportStatisticData _data = new ExportStatisticData();

        static public void Init()
        {
            _data.Processed = 0;
            _data.Total = 0;
            _data.IsRun = false;
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

        public static ExportStatisticData Data
        {
            get
            {
                lock (SyncObject)
                {
                    return _data;
                }
            }
        }

        public static bool IsRun
        {
            get
            {
                return _data.IsRun;
            }
            set
            {
                _data.IsRun = value;
            }
        }

        public static Thread ThreadImport { get; set; }
    }
}