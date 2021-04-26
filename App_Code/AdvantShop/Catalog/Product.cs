//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using AdvantShop.SEO;

namespace AdvantShop.Catalog
{
    public enum RelatedType
    {
        Related = 0,
        Alternative = 1,
        BoschRelated = 3

    }

    public class Product //: IMetaContainer
    {
        public int ProductId { get; set; }
        public string ArtNo { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public string PhotoDesc { get; set; }
        public double Ratio { get; set; }
        public decimal Discount { get; set; }
        public decimal Weight { get; set; }
        public string Size { get; set; }
        public bool IsFreeShipping { get; set; }
        public int ItemsSold { get; set; }
        public string BriefDescription { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public bool Recomended { get; set; }
        public bool New { get; set; }
        public bool BestSeller { get; set; }
        public bool OnSale { get; set; }
        public bool OrderByRequest { get; set; }
        public bool HirecalEnabled { get; set; }
        //Added by Evgeni to add EAN and SubBrandId
        public string EAN { get; set; }
        public int SubBrandId { get; set; }
        public string ManufactureArtNo { get; set; }
        //

        public bool CanOrderByRequest
        {
            get { return ((ProductService.IsExists(ProductId)) && (OrderByRequest) && (Offers[0].Amount <= 0)); }
        }

        public int BrandId { get; set; }

        private Brand _brand;
        public Brand Brand
        {
            get { return _brand ?? (_brand = BrandService.GetBrandById(BrandId)); }
        }


        public decimal Price
        {
            get { return (Offers == null) || (Offers.Count == 0) ? 0 : Offers[0].Price; }
        }

        public decimal Amount
        {
            get { return (Offers == null) || (Offers.Count == 0) ? 0 : Offers[0].Amount; }
        }

        private string _urlPath;
        public string UrlPath
        {
            get { return _urlPath; }
            set { _urlPath = value.ToLower(); }
        }

        /// <summary>
        /// Get from DB collection of Offer and set collection
        /// </summary>
        /// 
        private List<Offer> _offers;
        public List<Offer> Offers
        {
            get { return _offers ?? (_offers = (List<Offer>)OfferService.GetOffersByProductId(ProductId)); }
            set
            {
                _offers = value;
            }
        }

        public MetaType MetaType
        {
            get { return MetaType.Product; }
        }

        private MetaInfo _meta;
        public MetaInfo Meta
        {
            get
            {
                return _meta ??
                       (_meta =
                        MetaInfoService.GetMetaInfo(ProductId, MetaType) ??
                        MetaInfoService.GetDefaultMetaInfo(MetaType));
            }
            set
            {
                _meta = value;
            }
        }

        /// <summary>
        /// return collection of ProductPhoto
        /// </summary>
        private List<Photo> _productphotos;
        public List<Photo> ProductPhotos
        {
            get { return _productphotos ?? (_productphotos = PhotoService.GetPhotos(ProductId, PhotoType.Product).ToList()); }
        }
        private List<PropertyValue> _productPropertyValues;
        public List<PropertyValue> ProductPropertyValues
        {
            get
            {
                return _productPropertyValues ??
                       (_productPropertyValues = PropertyService.GetPropertyValuesByProductId(ProductId));
            }
        }

        private List<int> _productCategoryIDs;
        public List<int> ProductCategoryIDs
        {
            get
            {
                return _productCategoryIDs ??
                       (_productCategoryIDs = ProductService.GetCategoriesIDsByProductId(ProductId).ToList());
            }
        }

        private int _categoryID;

        public int CategoryID
        {
            get { return _categoryID == 0 || _categoryID == CategoryService.DefaultNonCategoryId ? _categoryID = ProductService.GetFirstCategoryIdByProductId(ProductId) : _categoryID; }
        }

        public bool InCategory
        {
            get { return CategoryID != CategoryService.DefaultNonCategoryId; }
        }

        private List<Category> _productCategories;
        [SoapIgnore]
        [XmlIgnoreAttribute]
        public List<Category> ProductCategories
        {
            get { return _productCategories ?? (_productCategories = ProductService.GetCategoriesByProductId(ProductId)); }
        }

        private List<Product> _relatedProducts;
        public List<Product> RelatedProducts
        {
            get
            {
                return _relatedProducts ??
                       (_relatedProducts = ProductService.GetRelatedProducts(ProductId, RelatedType.Related));
            }
        }

        private List<Product> _alternativeProducts;
        public List<Product> AlternativeProduct
        {
            get
            {
                return _alternativeProducts ??
                       (_alternativeProducts = ProductService.GetRelatedProducts(ProductId, RelatedType.Alternative));
            }
        }

        public int ID
        {
            get { return ProductId; }
        }

        public bool EnabledZoom { get; set; }
    }
}