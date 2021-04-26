//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Specialized;
using System.Web;
using AdvantShop.Configuration;
using AdvantShop.Customers;
using AdvantShop.Helpers;
using AdvantShop.Orders;

namespace AdvantShop.Security
{
    [Serializable]
    public class AuthorizeService
    {
        static readonly string MCookieCollectionName = HttpUtility.UrlEncode(SettingsMain.SiteUrl);

        public static bool CheckAdminCookies()
        {
            if (HttpContext.Current.Request.Cookies[MCookieCollectionName] != null)
            {
                var newCookie = HttpContext.Current.Request.Cookies[MCookieCollectionName];

                if (newCookie != null)
                    return LoginAdmin(newCookie["cUserName"], newCookie["cUserPWD"], true);
            }
            return false;
        }

        public static bool LoginAdmin(string email, string password, bool isHash)
        {
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
                var customer = CustomerService.GetCustomerByEmailAndPassword(email, password, isHash);
                if (customer == null)
                    return false;

                if (!customer.IsAdmin)
                    return false;

                DeleteCookie();
                WriteCookie(customer);
                return true;
            }
            return false;
        }

        public static void LoadUserCookies()
        {
            var collection = CommonHelper.GetCookieCollection(MCookieCollectionName);
            if (collection != null)
            {
                if (!AuthorizeTheUser(collection["cUserName"], collection["cUserPWD"], true))
                {
                    if (String.IsNullOrEmpty(collection["cUserId"]) || !CheckGuid(collection["cUserId"]))
                    {
                        CustomerSession.CreateAnonymousCustomerGuid();
                    }
                }
            }
            else
            {
                CustomerSession.CreateAnonymousCustomerGuid();
            }
        }

        private static bool CheckGuid(string guid)
        {
            try
            {
                var newGuid = new Guid(guid);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void DeleteCookie()
        {
            CommonHelper.DeleteCookie(MCookieCollectionName);
        }

        public static void WriteCookie(Customer customer)
        {
            var collection = new NameValueCollection
                                    {
                                        {"cUserName", customer.EMail},
                                        {"cUserPWD", customer.Password},
                                        {"cUserId", customer.Id.ToString()}
                                    };
            CommonHelper.SetCookieCollection(MCookieCollectionName, collection, new TimeSpan(7, 0, 0, 0));
        }

        public static bool AuthorizeTheUser(string email, string password, bool isHash)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return false;

            if (HttpContext.Current.Session["isAuthorize"] != null && (bool)HttpContext.Current.Session["isAuthorize"]) return true;

            if (Secure.IsDebugAccount(email, password))//, false, false))
            {
                HttpContext.Current.Session["isDebug"] = true;
                HttpContext.Current.Session["isAuthorize"] = true;
                Secure.AddUserLog("sa", true, true);
                return true;
            }

            var oldCustomerId = CustomerSession.CustomerId;
            var customer = CustomerService.GetCustomerByEmailAndPassword(email, password, isHash);
            if (customer != null)
            {
                HttpContext.Current.Session["isAuthorize"] = true;
                DeleteCookie();
                WriteCookie(customer);
                Secure.AddUserLog(customer.EMail, true, customer.EMail == "admin");

                MergeShoppingCarts(oldCustomerId, customer.Id);
                return true;
            }
            else
            {
                DeleteCookie();
                CustomerSession.CreateAnonymousCustomerGuid();
                return false;
            }
        }

        public static void MergeShoppingCarts(Guid oldCustomerId, Guid currentCustomerId)
        {
            if (oldCustomerId != currentCustomerId)
                foreach (var item in ShoppingCartService.GetAllShoppingCarts(oldCustomerId))
                {
                    ShoppingCartService.AddShoppingCartItem(item, currentCustomerId);
                }
        }
    }
}