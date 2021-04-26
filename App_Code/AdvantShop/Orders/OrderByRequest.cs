//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;

namespace AdvantShop.Orders
{
    public class OrderByRequest
    {
        public int OrderByRequestId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ArtNo { get; set; }
        public int Quantity { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Comment { get; set; }
        public bool IsComplete { get; set; }
        public DateTime RequestDate { get; set; }

        public string Code { get; set; }
        public DateTime CodeCreateDate { get; set; }

        public bool IsValidCode
        {
            get { return (OrderByRequestId != 0) && (RequestDate.AddDays(1) >= DateTime.Now); }
        }
    }
}