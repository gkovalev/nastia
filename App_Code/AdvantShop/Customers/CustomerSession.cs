//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Web;
using AdvantShop.Configuration;
using AdvantShop.Helpers;
using System.Collections.Specialized;

namespace AdvantShop.Customers
{
    public static class CustomerSession
    {
        private static readonly string MCookieCollectionName = HttpUtility.UrlEncode(SettingsMain.SiteUrl);

        public static Guid CustomerId
        {
            get
            {
                var collection = CommonHelper.GetCookieCollection(MCookieCollectionName);

                return (collection != null && !string.IsNullOrEmpty(collection["cUserId"])) ? new Guid(collection["cUserId"])
                                                                    : CreateAnonymousCustomerGuid();
            }
        }

        public static Guid CreateAnonymousCustomerGuid()
        {
            if (HttpContext.Current.Session != null)
                HttpContext.Current.Session["isAuthorize"] = false;

            CommonHelper.DeleteCookie(MCookieCollectionName);

            var customerId = Guid.NewGuid();

            // Если база не доступна то тут проблема. 
            while (CustomerService.ExistsCustomer(customerId))
            {
                customerId = Guid.NewGuid();
            }

            CommonHelper.SetCookieCollection(MCookieCollectionName, new NameValueCollection { { "cUserId", customerId.ToString() } }, new TimeSpan(7, 0, 0, 0));
            return customerId;
        }

        public static Customer CurrentCustomer
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    if (HttpContext.Current.Session != null && HttpContext.Current.Session["isDebug"] != null && (bool)HttpContext.Current.Session["isDebug"])
                    {
                        return new Customer
                                   {
                                       CustomerRole = Role.Administrator,
                                       IsVirtual = true
                                   };
                    }

                    var contextCustomer = HttpContext.Current.Items[CustomerId.ToString()] as Customer;
                    if (contextCustomer != null) return contextCustomer;

                    var dbCustomer = CustomerService.GetCustomer(CustomerId);
                    if (dbCustomer.Id == Guid.Empty)
                        dbCustomer.Id = CustomerId;

                    HttpContext.Current.Items[CustomerId.ToString()] = dbCustomer;
                    return dbCustomer;
                }
                return CustomerService.GetCustomer(CustomerId);
            }
        }
    }
}