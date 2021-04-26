//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using AdvantShop.Catalog;
using AdvantShop.Customers;
using Newtonsoft.Json;

namespace AdvantShop.Orders
{
    public enum EnumItemType
    {
        Product = 0,
        Certificate = 1
    }
    public class ShoppingCartItem
    {
        [JsonIgnore]
        public int ItemId { get; set; }

        [JsonIgnore]
        public int ShoppingCartTypeId { get; set; }


        [JsonIgnore]
        public ShoppingCartType ShoppingCartType
        {
            get
            {
                return (ShoppingCartType)ShoppingCartTypeId;
            }
            set
            {
                ShoppingCartTypeId = (int)value;
            }
        }

        [JsonIgnore]
        public Guid CustomerId { get; set; }

        [JsonIgnore]
        public string AttributesXml { get; set; }

        public int Amount { get; set; }

        [JsonIgnore]
        public DateTime CreatedOn { get; set; }

        [JsonIgnore]
        public DateTime UpdatedOn { get; set; }

        public int EntityId { get; set; }

        public EnumItemType ItemType { get; set; }

        private Coupon _coupon;

        [JsonIgnore]
        public bool IsCouponApplied
        {
            get
            {
                if (_coupon == null)
                    _coupon = CouponService.GetCustomerCoupon(CustomerId);
                return _coupon != null && CouponService.IsCouponAppliedToProduct(_coupon.CouponID, EntityId);
            }
        }

        private Product _product;

        [JsonIgnore]
        public Product Product
        {
            get
            {
                if (ItemType == EnumItemType.Product)
                    return _product ?? (_product = ProductService.GetProduct(EntityId));
                else
                    throw new Exception("Type of item is not product");
            }
        }

        private GiftCertificate _certificate;

        [JsonIgnore]
        public GiftCertificate Certificate
        {
            get
            {
                if (ItemType == EnumItemType.Certificate)
                    return _certificate ?? (_certificate = GiftCertificateService.GetCertificateByID(EntityId));
                else
                    throw new Exception("Type of item is not Certificate");
            }
        }

        [JsonIgnore]
        public IList<EvaluatedCustomOptions> EvaluatedCustomOptions
        {
            get { return CustomOptionsService.DeserializeFromXml(AttributesXml); }
        }
        
        public override int GetHashCode()
        {
            return EntityId ^ Amount ^ AttributesXml.GetHashCode();
        }

        private CustomerGroup _customerGroup;

        private CustomerGroup CustomerGroup
        {
            get { return _customerGroup ?? (_customerGroup = CustomerSession.CurrentCustomer.RegistredUser 
                ? CustomerSession.CurrentCustomer.CustomerGroup 
                : CustomerGroupService.GetCustomerGroup( CustomerGroupService.DefaultCustomerGroup)); }
        }

        public decimal Price
        {
            get
            {
                switch (ItemType)
                {
                    case EnumItemType.Product:
                        return ProductService.CalculateProductPrice(Product.Price, Product.Discount, CustomerGroup, CustomOptionsService.DeserializeFromXml(AttributesXml), true);
                    case EnumItemType.Certificate:
                        return GiftCertificateService.GetCertificatePriceById(EntityId);
                    default:
                        throw new NotImplementedException();
                }
            }
        }
    }
}