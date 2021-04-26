//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;

namespace AdvantShop.Customers
{
    [Serializable]
    public class CustomerGroup
    {
        public int CustomerGroupId { get; set; }
        public int OfferListId { get; set; }
        public string GroupName { get; set; }
        public decimal GroupDiscount { get; set; }
    }
}