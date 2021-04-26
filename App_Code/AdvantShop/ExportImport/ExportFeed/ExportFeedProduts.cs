namespace AdvantShop.ExportImport
{
    public class ExportFeedProduts
    {
        public int ProductID { get; set; }
        public int Amount { get; set; }
        public string UrlPath { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public int ParentCategory { get; set; }
        public string Name { get; set; }
        public string BriefDescription { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
        //Added by Evgeni to encrease YandexMArket output abilities
        public string BrandName { get; set; }
        public string ArtNo { get; set; }
    }
}