//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Threading;

namespace AdvantShop.Statistic
{
    public sealed class SendMessageStatistic
    {
        private static readonly object SyncObject = new object();

        static public void Init()
        {
            _sendEmails = 0;
            _totalEmails = 0;
            IsRun = false;
            IsAbort = false;
        }

        private static long _sendEmails;
        public static long SendEmails
        {
            get
            {
                lock(SyncObject)
                {
                    return _sendEmails;
                }
            }
            set
            {
                lock(SyncObject)
                {
                    _sendEmails = value;
                }
            }
        }

        private static long _totalEmails;
        public static long TotalEmails
        {
            get
            {
                lock (SyncObject)
                {
                    return _totalEmails;
                }
            }
            set
            {
                lock (SyncObject)
                {
                    _totalEmails = value;
                }
            }
        }

        public static bool IsRun { get; set; }

        public static bool IsAbort { get; set; }

        public static Thread ThreadImport { get; set; }
    }
}