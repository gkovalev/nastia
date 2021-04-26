//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;

namespace AdvantShop.Customers
{
    [Serializable]
    public struct RecentlyViewRecord
    {
        public int ProductID { get; set; }

        public DateTime ViewTime { get; set; }

        public string ImgPath { get; set; }

        public decimal Price { get; set; }

        public string Name { get; set; }

        public string UrlPath { get; set; }

        public decimal  Discount { get; set; }
    }
}