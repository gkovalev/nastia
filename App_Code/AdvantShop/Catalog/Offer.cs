//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

namespace AdvantShop.Catalog
{
    public class Offer
    {
        public Offer()
        {
            Multiplicity = 1;
        }

        public int OfferId { get; set; }
        public int OfferListId { get; set; }
        public int ProductId { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public decimal SupplyPrice { get; set; }
        public decimal ShippingPrice { get; set; }
        public string Unit { get; set; }

        public int? MinAmount { get; set; }
        public int? MaxAmount { get; set; }
        public int Multiplicity { get; set; }
    }
}
