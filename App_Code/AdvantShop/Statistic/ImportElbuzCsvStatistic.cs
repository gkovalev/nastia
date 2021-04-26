//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

namespace AdvantShop.Core
{
    public sealed class ImportElbuzCsvStatistic
    {
				
        private static readonly object syncObject = new object();
				
        static long _RowPosition;
        static long _TotalRowExcel;
        static bool _IsRun;
        static long _TotalUpdateRow;
        static long _TotalAddRow;
        static long _TotalErrorRow;
        static System.Threading.Thread _ThreadImport;

        static ImportElbuzCsvStatistic()
        {
        }
				
        static public void Init()
        {
            RowPosition = 0;
            TotalRowExcel = 0;
            IsRun = false;
            TotalUpdateRow = 0;
            TotalAddRow = 0;
            TotalErrorRow = 0;
        }
				
        public static long TotalRowExcel
        {
            get
            {
                lock(syncObject)
                {
                    return _TotalRowExcel;
                }
            }
            set
            {
                lock(syncObject)
                {
                    _TotalRowExcel = value;
                }
            }
        }
				
        public static long RowPosition
        {
            get
            {
                lock(syncObject)
                {
                    return _RowPosition;
                }
            }
            set
            {
                lock(syncObject)
                {
                    _RowPosition = value;
                }
            }
        }
				
        public static bool IsRun
        {
            get
            {
                lock(syncObject)
                {
                    return _IsRun;
                }
            }
            set
            {
                lock(syncObject)
                {
                    _IsRun = value;
                }
            }
        }
				
        public static long TotalUpdateRow
        {
            get
            {
                lock(syncObject)
                {
                    return _TotalUpdateRow;
                }
            }
            set
            {
                lock(syncObject)
                {
                    _TotalUpdateRow = value;
                }
            }
        }
				
        public static long TotalAddRow
        {
            get
            {
                lock(syncObject)
                {
                    return _TotalAddRow;
                }
            }
            set
            {
                lock(syncObject)
                {
                    _TotalAddRow = value;
                }
            }
        }
				
        public static long TotalErrorRow
        {
            get
            {
                lock(syncObject)
                {
                    return _TotalErrorRow;
                }
            }
            set
            {
                lock(syncObject)
                {
                    _TotalErrorRow = value;
                }
            }
        }
				
        public static System.Threading.Thread ThreadImport
        {
            get
            {
                return _ThreadImport;
            }
            set
            {
                _ThreadImport = value;
            }
        }
				
    }
			
}

