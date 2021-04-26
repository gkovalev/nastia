//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Customers;

namespace AdvantShop.Orders
{
    public class ShoppingCart : List<ShoppingCartItem>
    {
        public decimal TotalPrice
        {
            get { return this.Sum(p => p.Price * p.Amount); }
        }

        public decimal TotalProductPrice
        {
            get { return this.Where(p => p.ItemType == EnumItemType.Product).Sum(p => p.Price*p.Amount); }
        }

        public decimal TotalShippingWeight
        {
            get { return this.Sum(p => p.ItemType == EnumItemType.Product ? p.Product.Weight * p.Amount : 0); }
        }


        public int TotalItems
        {
            get { return this.Sum(item => item.Amount); }
        }

        public bool HasItems
        {
            get { return this.Any(); }
        }

        public override int GetHashCode()
        {
            return this.Aggregate(0, (curr, val) => val.GetHashCode()) + (Certificate != null ? Certificate.GetHashCode() : 0) + (Coupon != null? Coupon.GetHashCode() : 0);
        }

        public decimal MinimalPrice
        {
            get { return SettingsOrderConfirmation.MinimalOrderPrice; }
        }


        public bool CanOrder
        {
            get
            {
                if (TotalPrice < SettingsOrderConfirmation.MinimalOrderPrice || !HasItems)
                    return false;
                return !this.Any(p => p.ItemType == EnumItemType.Product && 
                    (p.Product.Enabled == false || (p.Amount > p.Product.Amount && SettingsOrderConfirmation.AmountLimitation && !p.Product.CanOrderByRequest)));
            }
        }

        public decimal DiscountPercentOnTotalPrice
        {
            get
            {
                return (Coupon == null && CustomerSession.CurrentCustomer.CustomerGroupId == CustomerGroupService.DefaultCustomerGroup)
                    ? OrderService.GetDiscount(TotalProductPrice)
                    : 0;
            }
        }

        public decimal TotalDiscount
        {
            get
            {
                if (CustomerSession.CurrentCustomer.CustomerGroupId != CustomerGroupService.DefaultCustomerGroup) 
                    return 0;

                decimal discount = 0;
                discount += DiscountPercentOnTotalPrice > 0 ? DiscountPercentOnTotalPrice * TotalProductPrice / 100 : 0;
                discount += Certificate != null ? Certificate.Sum : 0;

                if (Coupon != null)
                {
                    if (TotalProductPrice >= Coupon.MinimalOrderPrice)
                    {
                        switch (Coupon.Type)
                        {
                            case CouponType.Fixed:
                                var productsPrice = this.Where( p => p.ItemType == EnumItemType.Product && p.IsCouponApplied).Sum(p => p.Price*p.Amount);
                                discount += productsPrice >= Coupon.Value ? Coupon.Value : productsPrice;
                                break;
                            case CouponType.Percent:
                                discount += this.Where(p => p.ItemType == EnumItemType.Product && p.IsCouponApplied).Sum(p => Coupon.Value*p.Price/100*p.Amount);
                                break;
                        }
                    }
                }
                return discount;
            }
        }

        private GiftCertificate _certificate;

        public GiftCertificate Certificate
        {
            get
            {
                if (_certificate == null)
                {
                    _certificate = GiftCertificateService.GetCustomerCertificate();
                }

                if (_certificate != null && _coupon != null)
                    throw new Exception("Coupon and Certificate cant be used together");

                if (_certificate != null && (!_certificate.Paid || _certificate.Used || !_certificate.Enable) )
                {
                    GiftCertificateService.DeleteCustomerCertificate(_certificate.CertificateId);
                    return null;
                }

                return _certificate;
            }
        }

        private Coupon _coupon;
        public Coupon Coupon
        {
            get
            {
                if (_coupon == null)
                {
                    _coupon = CouponService.GetCustomerCoupon();
                }

                if (_coupon != null && _certificate != null)
                    throw new Exception("Coupon and Certificate cant be used together");

                if (_coupon != null && ((_coupon.ExpirationDate != null && _coupon.ExpirationDate < DateTime.Now) || (_coupon.PossibleUses != 0 && _coupon.PossibleUses <= _coupon.ActualUses) || !_coupon.Enabled))
                {
                    CouponService.DeleteCustomerCoupon(_coupon.CouponID);
                }

                return _coupon;
            }
        }

    }
}