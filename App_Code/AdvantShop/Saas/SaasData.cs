//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;

namespace AdvantShop.SaasData
{
    public class SaasData
    {
        public SaasData()
        {
            Name = "";
            Price = 0;
            ProductsCount = 0;
            PhotosCount = 0;
            HaveExcel = false;
            Have1C = false;
            HaveExportFeeds = false;
            HavePriceRegulating = false;
            HaveBankIntegration = false;
            IsWork = false;
            PaidTo = DateTime.Now.AddDays(-7);
            LastUpdate = DateTime.Now.AddDays(-7);
        }

        public string Name { get; set; }
        public int Price { get; set; }
        public int ProductsCount { get; set; }
        public int PhotosCount { get; set; }
        public bool HaveExcel { get; set; }
        public bool Have1C { get; set; }
        public bool HaveExportFeeds { get; set; }
        public bool HavePriceRegulating { get; set; }
        public bool HaveBankIntegration { get; set; }
        public bool IsWork { get; set; }
        public DateTime PaidTo { get; set; }
        public DateTime LastUpdate { get; set; }

        public bool IsWorkingNow
        {
            get
            {
                return (IsWork && (PaidTo > DateTime.Now));
            }
        }
    }
}
