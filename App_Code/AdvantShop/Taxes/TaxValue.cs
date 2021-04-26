//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

namespace AdvantShop.Taxes
{
    public class TaxValue
    {
        public string TaxName { get; set; }
        public decimal TaxSum { get; set; }
        public int TaxID { get; set; }
        public bool TaxShowInPrice { get; set; }
    }
}