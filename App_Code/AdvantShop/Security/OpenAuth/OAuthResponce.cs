//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Web;
using AdvantShop.Customers;

namespace AdvOpenAuth
{
    public class OAuthResponce
    {
        public enum Providers
        {
            Empty,
            Google,
            Yandex,
            Rambler,
            Mail
        }

        private static readonly Dictionary<Providers, string> _providerEndPoint = new Dictionary<Providers, string>
                                                                    {
                                                                        {Providers.Google, "https://www.google.com/accounts/o8/ud"},
                                                                        {Providers.Yandex, "http://openid.yandex.ru/server/"},
                                                                        {Providers.Rambler, "http://rambler.ru"},
                                                                        {Providers.Mail, "http://openid.mail.ru/login"}
                                                                    };
        private Providers _provider;
        public Providers Provider
        {
            get { return _provider; }
            set { _provider = value; }
        }

        public OAuthResponce()
        {
            _provider = Providers.Empty;
        }

        public static bool IsOpenIdResponce(HttpRequest request)
        {
            return _providerEndPoint.ContainsValue(HttpUtility.UrlDecode(request["openid.op_endpoint"]));
        }

        public static Customer GetCustomerParameters(HttpRequest request)
        {
            if (!string.IsNullOrEmpty(request["openid.op_endpoint"]) && request["openid.op_endpoint"].Contains("google"))
            {
                var customer = new Customer { CustomerGroupId = 1, Password = Guid.NewGuid().ToString() };
                if (!string.IsNullOrEmpty(request["openid.ext1.value.firstname"]))
                {
                    customer.FirstName = HttpUtility.UrlDecode(request["openid.ext1.value.firstname"]);
                }
                if (!string.IsNullOrEmpty(request["openid.ext1.value.lastname"]))
                {
                    customer.LastName = HttpUtility.UrlDecode(request["openid.ext1.value.lastname"]);
                }
                if (!string.IsNullOrEmpty(request["openid.ext1.value.email"]))
                {
                    customer.EMail = HttpUtility.UrlDecode(request["openid.ext1.value.email"]);
                }
                else
                {
                    if (!string.IsNullOrEmpty(HttpUtility.UrlDecode(request["openid.sreg.firstname"])))
                    {
                        customer.EMail =
                           HttpUtility.UrlDecode(request["openid.sreg.firstname"]).Replace(" ", "_") + "@temp.google";
                    }
                    else
                    {
                        customer.EMail = AdvantShop.Strings.GetRandomString(8) + "@temp.google";
                    }
                }

                return customer;
            }
            if (!string.IsNullOrEmpty(request["openid.op_endpoint"]) && request["openid.op_endpoint"].Contains("yandex"))
            {
                var customer = new Customer { CustomerGroupId = 1, Password = Guid.NewGuid().ToString() };

                if (!string.IsNullOrEmpty(request["openid.sreg.fullname"]))
                {
                    customer.FirstName = HttpUtility.UrlDecode(request["openid.sreg.fullname"]);
                }
                if (!string.IsNullOrEmpty(request["openid.sreg.email"]))
                {
                    customer.EMail = HttpUtility.UrlDecode(request["openid.sreg.email"]);
                }
                else
                {
                    if (!string.IsNullOrEmpty(HttpUtility.UrlDecode(request["openid.sreg.fullname"])))
                    {
                        customer.EMail = HttpUtility.UrlDecode(request["openid.sreg.fullname"]).Replace(" ", "_") + "@temp.yandex";
                    }
                    else
                    {
                        customer.EMail = AdvantShop.Strings.GetRandomString(8) + "@temp.yandex";
                    }
                }
                return customer;
            }
            if (!string.IsNullOrEmpty(request["openid.op_endpoint"]) && request["openid.op_endpoint"].Contains("mail"))
            {
                var customer = new Customer { CustomerGroupId = 1, Password = Guid.NewGuid().ToString() };

                if (!string.IsNullOrEmpty(request["openid.sreg.fullname"]))
                {
                    customer.FirstName = HttpUtility.UrlDecode(request["openid.sreg.fullname"]);
                }
                if (!string.IsNullOrEmpty(request["openid.sreg.email"]))
                {
                    customer.EMail = HttpUtility.UrlDecode(request["openid.sreg.email"]);
                }
                else
                {
                    if (!string.IsNullOrEmpty(HttpUtility.UrlDecode(request["openid.sreg.fullname"])))
                    {
                        customer.EMail =
                        HttpUtility.UrlDecode(request["openid.sreg.fullname"]).Replace(" ", "_") + "@temp.mail";
                    }
                    else
                    {
                        customer.EMail = AdvantShop.Strings.GetRandomString(8) + "@temp.mail";
                    }
                }
                return customer;
            }
            return null;
        }

        public static void AuthOrRegCustomer(Customer customer)
        {
            AuthOrRegCustomer(customer, customer.EMail);
        }

        public static void AuthOrRegCustomer(Customer customer, string identifier)
        {
            if (!CustomerService.IsExistOpenIdLinkCustomer(identifier))
            {
                if (!CustomerService.CheckCustomerExist(customer.EMail))
                {
                    customer.Id = CustomerService.InsertNewCustomer(customer);
                }
                else
                {
                    customer = CustomerService.GetCustomerByEmail(customer.EMail);
                }
                CustomerService.AddOpenIdLinkCustomer(customer.Id, identifier);
                customer = CustomerService.GetCustomerByEmail(customer.EMail);
            }
            else
            {
               customer = CustomerService.GetCustomerByOpenAuthIdentifier(identifier);
            }

            AdvantShop.Security.AuthorizeService.AuthorizeTheUser(customer.EMail, customer.Password, true);
        }

        public static bool OAuthUser(HttpRequest request)
        {
            if (IsOpenIdResponce(request))
            {
                AuthOrRegCustomer(GetCustomerParameters(request));
                return true;
            }
            return false;
        }
    }
}