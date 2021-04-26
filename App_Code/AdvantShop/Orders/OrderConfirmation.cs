//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop.Customers;
using AdvantShop.Payment;
using AdvantShop.Shipping;

namespace AdvantShop.Orders
{
    #region  enum

    public enum EnActiveTab
    {
        NoTab = 0,
        DefaultTab = 1,
        UserTab = 2,
        ShippingTab = 3,
        PaymentTab = 4,
        SumTab = 5,
        FinalTab = 6
    }

    public enum EnUserType
    {
        NoUser,
        NewUserWithOutRegistration,
        JustRegistredUser,
        RegistredUser
    }

    #endregion

    public class OrderConfirmationData
    {
        public EnUserType UserType { get; set; }
        public EnActiveTab ActiveTab { get; set; }

        public int SelectPaymentId { get; set; }
        public string SelectPaymentName { get; set; }
        public ProcessType SelectedPaymentProcessType { get; set; }
        public PaymentType SelectedPaymentType { get; set; }

        public int SelectShippingId { get; set; }
        public string SelectShippingName { get; set; }
        public decimal SelectShippingRate { get; set; }
        public string SelectShippingButtonId { get; set; }
        public ShippingOptionEx ShippingOptionEx { get; set; }

        public int CheckSum { get; set; }
        public bool BillingIsShipping { get; set; }
        public CustomerContact ShippingContact { get; set; }
        private CustomerContact _billingContact;
        public CustomerContact BillingContact { get { return BillingIsShipping ? ShippingContact : _billingContact; } set { _billingContact = value; } }

        public Customer Customer { get; set; }
        public decimal TaxesTotal { get; set; }

        public string CustomerComment { get; set; }

        public PaymentDetails PaymentDetails { get; set; }
    }

    public class OrderConfirmation
    {
        public Guid CustomerId { get; set; }
        public OrderConfirmationData OrderConfirmationData { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}