//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Threading;

namespace AdvantShop.Tools
{
    public sealed class ResizePhotoStatistic
    {
        private static readonly object SyncObject = new object();

        static long _currentPhotoNumber;
        static long _totalCurrentPhotosCount;
        static bool _isRun;

        static public void Init()
        {
            _currentPhotoNumber = 0;
            _totalCurrentPhotosCount = 0;
            _isRun = false;
        }

        public static long Count
        {
            get
            {
                lock (SyncObject)
                {
                    return _totalCurrentPhotosCount;
                }
            }
            set
            {
                lock (SyncObject)
                {
                    _totalCurrentPhotosCount = value;
                }
            }
        }

        public static long Index
        {
            get
            {
                lock (SyncObject)
                {
                    return _currentPhotoNumber;
                }
            }
            set
            {
                lock (SyncObject)
                {
                    _currentPhotoNumber = value;
                }
            }
        }

        public static bool IsRun
        {
            get
            {
                return _isRun;
            }
            set
            {
                _isRun = value;
            }
        }

        public static Thread ThreadImport { get; set; }
    }
}