//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;

namespace AdvantShop.Statistic
{
    [Serializable ]
    public class ClientOnlineInfo
    {
        public string SessionId { get; set; }
        public string Address { get; set; }
        public string UserAgentBrowser { get; set; }
        public string UserAgentOS { get; set; }
        public string CountryByGeoIp { get; set; }
        public string LastAccessedPath { get; set; }
        public DateTime Started { get; set; }
        public DateTime Ended { get; set; }
    }
}