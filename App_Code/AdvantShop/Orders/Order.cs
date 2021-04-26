//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Linq;
using System.Collections.Generic;
using AdvantShop.Catalog;
using AdvantShop.Payment;
using AdvantShop.Shipping;
using AdvantShop.Taxes;
using AdvantShop.Repository.Currencies;

namespace AdvantShop.Orders
{
    public class Order : IOrder
    {
        public int OrderID { get; set; }
        public string Number { get; set; }

        private PaymentDetails _paymentDetails;
        public PaymentDetails PaymentDetails
        {
            get { return _paymentDetails ?? (_paymentDetails = OrderService.GetPaymentDetails(OrderID)); }
            set { _paymentDetails = value; }
        }
        public bool Payed
        {
            get { return PaymentDate != null; }
        }
        private IList<OrderItem> _orderItems;
        public IList<OrderItem> OrderItems
        {
            get { return _orderItems ?? (_orderItems = OrderService.GetOrderItems(OrderID)); }
            set { _orderItems = value; }
        }

        //-------------------------------
        private OrderCustomer _orderCustomer;
        public OrderCustomer OrderCustomer
        {
            get { return _orderCustomer ?? (_orderCustomer = OrderService.GetOrderCustomer(OrderID)); }
            set { _orderCustomer = value; }
        }

        public IOrderCustomer GetOrderCustomer()
        {
            return OrderCustomer;
        }

        private OrderCurrency _orderCurrency;
        public OrderCurrency OrderCurrency
        {
            get { return _orderCurrency ?? (_orderCurrency = OrderService.GetOrderCurrency(OrderID)); }
            set { _orderCurrency = value; }
        }

        private OrderPickPoint _orderPickPoint;
        public OrderPickPoint OrderPickPoint
        {
            get { return _orderPickPoint ?? (_orderPickPoint = OrderService.GetOrderPickPoint(OrderID)); }
            set { _orderPickPoint = value; }
        }

        private OrderContact _shippingContact;

        public OrderContact ShippingContact
        {
            get
            {
                return _shippingContact ??
                       (_shippingContact = OrderService.GetOrderContact(ShippingContactID));
            }
            set { _shippingContact = value; }
        }

        private OrderContact _billingContact;

        public OrderContact BillingContact
        {
            get
            {
                return _billingContact ??
                       (_billingContact = OrderService.GetOrderContact(BillingContactID));
            }
            set { _billingContact = value; }
        }

        private List<TaxValue> _taxes;
        public List<TaxValue> Taxes
        {
            get { return _taxes ?? (_taxes = TaxServices.GetOrderTaxes(OrderID)); }
            set { _taxes = value; }
        }

        private string _shippingMethod;
        public string ShippingMethod
        {
            get
            {
                if (_shippingMethod != null)
                    return _shippingMethod;
                var module = ShippingMethodService.GetShippingMethod(ShippingMethodId);
                return _shippingMethod = module == null ? string.Empty : module.Name;
            }
        }

        private string _paymentMethodName;
        public string PaymentMethodName
        {
            get
            {
                return _paymentMethodName ??
                       (_paymentMethodName =
                        PaymentMethod != null
                            ? PaymentMethod.Name
                            : ArchivedPaymentName);
            }
        }

        public string ArchivedPaymentName { private get; set; }

        private string _shippingMethodName;
        public string ShippingMethodName
        {
            get
            {
                return _shippingMethodName ??
                    (_shippingMethodName = !string.IsNullOrEmpty(ShippingMethod) ? ShippingMethod : ArchivedShippingName);
            }
        }

        public string ArchivedShippingName { get; set; }

        //PaymentMethodId = 0 наложеный платеж
        private PaymentMethod _paymentMethod;
        public PaymentMethod PaymentMethod
        {
            get
            {
                //var tempCash = new CashOnDelivery(null);
                //if (PaymentMethodId == tempCash.PaymentMethodID)
                //    return tempCash;

                //var tempP = new PickPoint();
                //if (PaymentMethodId == tempP.PaymentMethodID)
                //    return tempP;

                return _paymentMethod ?? (_paymentMethod = PaymentService.GetPaymentMethod(PaymentMethodId));
            }
        }

        private OrderStatus _orderStatus;

        public OrderStatus OrderStatus { get { return _orderStatus ?? (_orderStatus = OrderService.GetOrderStatus(OrderStatusId)); } set { _orderStatus = value; } }

        public IOrderStatus GetOrderStatus()
        {
            return OrderStatus;
        }

        public int ShippingContactID { get; set; }

        public int BillingContactID { get; set; }

        public int ShippingMethodId { get; set; }

        public int PaymentMethodId { get; set; }

        public int AffiliateID { get; set; }

        public decimal OrderDiscount { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime? PaymentDate { get; set; }

        public string CustomerComment { get; set; }

        public string StatusComment { get; set; }

        public string AdditionalTechInfo { get; set; }

        public string AdminOrderComment { get; set; }

        public bool Decremented { get; set; }

        public decimal ShippingCost { get; set; }

        public decimal TaxCost { get; set; }

        public decimal SupplyTotal { get; set; }

        public int OrderStatusId { get; set; }

        public decimal Sum { get; set; }

        public string GroupName { get; set; }

        public decimal GroupDiscount { get; set; }

        public OrderCertificate Certificate { get; set; }

        public OrderCoupon Coupon { get; set; }

        public string GroupDiscountString { get { return GroupName + " (" + GroupDiscount + ")"; } }


        public decimal TotalDiscount
        {
            get
            {
                decimal discount = 0;
                discount += OrderDiscount > 0 ? OrderDiscount * OrderItems.Sum(item => item.Price * item.Amount) / 100 : 0;
                if (Certificate != null)
                {
                    discount += Certificate.Price != 0 ? Certificate.Price : 0;
                }

                if (Coupon != null)
                {
                    switch (Coupon.Type)
                    {
                        case CouponType.Fixed:
                            var productsPrice = OrderItems.Where(p => p.IsCouponApplied).Sum(p => p.Price*p.Amount);
                            discount += productsPrice >= Coupon.Value ? Coupon.Value : productsPrice;
                            break;
                        case CouponType.Percent:
                            discount +=
                                OrderItems.Where(p => p.IsCouponApplied).Sum(p => Coupon.Value*p.Price/100*p.Amount);
                            break;
                    }
                }
                return discount;
            }
        }

    }

    public enum OrderStatusCommand
    {
        None,
        Increment,
        Decrement
    }

    public interface IOrderStatus
    {
        int StatusID { get; set; }
        string StatusName { get; set; }
        bool IsDefault { get; set; }
        bool Canceled { get; set; }
    }

    public class OrderStatus : IOrderStatus
    {
        public int StatusID { get; set; }
        public string StatusName { get; set; }
        public OrderStatusCommand Command { get; set; }
        public bool IsDefault { get; set; }
        public bool Canceled { get; set; }
    }

    public enum OrderContactType
    {
        ShippingContact,
        BillingContact
    }

    [Serializable]
    public class OrderContact
    {
        public int OrderContactId { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Zone { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string Address { get; set; }
    }

    public class OrderCurrency
    {
        public static implicit operator OrderCurrency(Currency cur)
        {
            return new OrderCurrency
                       {
                           CurrencyCode = cur.Iso3,
                           CurrencyNumCode = cur.NumIso3,
                           CurrencyValue = cur.Value,
                           CurrencySymbol = cur.Symbol,
                           IsCodeBefore = cur.IsCodeBefore
                       };
        }

        public static implicit operator Currency(OrderCurrency cur)
        {
            try
            {
                var currency = CurrencyService.Currency(cur.CurrencyCode);
                currency.Value = cur.CurrencyValue;
                return currency;
            }
            catch (Exception)
            {
                return new Currency
                {
                    Iso3 = cur.CurrencyCode,
                    Value = cur.CurrencyValue,
                    IsCodeBefore = false,
                    PriceFormat = CurrencyService.DefaultPriceFormat,
                    Symbol = cur.CurrencyCode
                };
            }
        }

        public string CurrencyCode { get; set; }
        public int CurrencyNumCode { get; set; }
        public decimal CurrencyValue { get; set; }
        public string CurrencySymbol { get; set; }
        public bool IsCodeBefore { get; set; }
    }

    public class OrderCoupon
    {
        public string Code { get; set; }
        public CouponType Type { get; set; }
        public decimal Value { get; set; }
    }

    public class OrderCertificate
    {
        public string Code { get; set; }
        public decimal Price { get; set; }

    }

}