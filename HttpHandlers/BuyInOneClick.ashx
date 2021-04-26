<%@ WebHandler Language="C#" Class="BuyInOneClick" %>

using System;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;
using AdvantShop;
using AdvantShop.Catalog;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.Mails;
using AdvantShop.Modules;
using AdvantShop.Orders;
using AdvantShop.Repository.Currencies;

public class BuyInOneClick : IHttpHandler, IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {

        try
        {
            var amount = 0;
            var productId = 0;
            var page = OrderConfirmationService.BuyInOneclickPage.details;
            
            var valid = true;
            valid &= context.Request["page"].IsNotEmpty() && Enum.TryParse(context.Request["page"], true, out page);
            valid &= context.Request["productId"].IsNotEmpty() && Int32.TryParse(context.Request["productId"], out productId);
            if (page == OrderConfirmationService.BuyInOneclickPage.details)
            {
                valid &= context.Request["amount"].IsNotEmpty() && Int32.TryParse(context.Request["amount"], out amount);
            }
            
            valid &= context.Request["name"].IsNotEmpty();

            
            valid &= context.Request["adres"].IsNotEmpty();
           
            valid &= context.Request["phone"].IsNotEmpty();

            valid &= context.Request["familyName"].IsNotEmpty();
            valid &= context.Request["payment"].IsNotEmpty();
            valid &= context.Request["shipping"].IsNotEmpty();
            valid &= context.Request["city"].IsNotEmpty();
          

            if (!valid)
            {
                ReturnResult(context, "error");
            }

            var OrderItems = new List<OrderItem>();
            decimal DiscountPercentOnTotalPrice = 0;
            decimal totalPrice = 0;

            OrderCertificate orderCertificate = null;
            OrderCoupon orderCoupon = null;

            if (page == OrderConfirmationService.BuyInOneclickPage.details)
            {
                IList<EvaluatedCustomOptions> listOptions = null;
                string selectedOptions = HttpUtility.UrlDecode(context.Request["customOptions"]);
                try
                {
                    listOptions = CustomOptionsService.DeserializeFromXml(selectedOptions);
                }
                catch (Exception)
                {
                    listOptions = null;
                }

                var product = ProductService.GetProduct(productId);


                DiscountPercentOnTotalPrice = OrderService.GetDiscount(product.Price * amount);
                totalPrice = (product.Price - (product.Price * product.Discount / 100)) * amount;
                OrderItems = new List<OrderItem>
                {
                    new OrderItem
                        {
                            EntityId = product.ProductId,
                            Amount = amount,
                            SelectedOptions = listOptions,
                            Price = product.Price - (product.Price * product.Discount/100),
                            Name = product.Name,
                            ArtNo = product.ArtNo,
                            Weight = product.Weight
                        }
                };
            }
            else if (page == OrderConfirmationService.BuyInOneclickPage.shoppingcart)
            {
                var shoppingCart = ShoppingCartService.CurrentShoppingCart;
                DiscountPercentOnTotalPrice = shoppingCart.DiscountPercentOnTotalPrice;
                totalPrice = shoppingCart.TotalPrice;

                foreach (var orderItem in shoppingCart.Select(item => (OrderItem)item))
                {
                    OrderItems.Add(orderItem);
                }

                GiftCertificate certificate = shoppingCart.Certificate;
                Coupon coupon = shoppingCart.Coupon;

                if (certificate != null)
                {
                    orderCertificate = new OrderCertificate()
                    {
                        Code = certificate.CertificateCode,
                        Price = certificate.Sum
                    };
                }
                if (coupon != null && shoppingCart.TotalPrice >= coupon.MinimalOrderPrice)
                {
                    orderCoupon = new OrderCoupon()
                    {
                        Code = coupon.Code,
                        Type = coupon.Type,
                        Value = coupon.Value
                    };
                }

            }

            var orderContact = new OrderContact
                {
                    Address = context.Request["adres"],
                    City = context.Request["city"],
                    Country = "Беларусь",
                    Name = context.Request["name"] + ' ' +  context.Request["familyName"],
                    //Zip = context.Request["familyName"],
                    Zone = string.Empty
                };

            var customerGroup = CustomerSession.CurrentCustomer.CustomerGroup;
            var order = new Order
                {
                    CustomerComment = context.Request["comment"],
                    OrderDate = DateTime.Now,
                    OrderCurrency = new OrderCurrency
                        {
                            CurrencyCode = CurrencyService.CurrentCurrency.Iso3,
                            CurrencyNumCode = CurrencyService.CurrentCurrency.NumIso3,
                            CurrencySymbol = CurrencyService.CurrentCurrency.Symbol,
                            CurrencyValue = CurrencyService.CurrentCurrency.Value,
                            IsCodeBefore = CurrencyService.CurrentCurrency.IsCodeBefore
                        },
                    OrderCustomer = new OrderCustomer
                        {
                            CustomerID = CustomerSession.CurrentCustomer.Id,
                           // Email = CustomerSession.CurrentCustomer.EMail,
                            Email = context.Request["email"],
                            FirstName = context.Request["name"] ,
                            LastName =  context.Request["familyName"],
                            MobilePhone = context.Request["phone"],
                            CustomerIP = HttpContext.Current.Request.UserHostAddress
                        },

                    OrderStatusId = OrderService.DefaultOrderStatus,
                    AffiliateID = 0,
                    GroupName = customerGroup.GroupName,
                    GroupDiscount = customerGroup.GroupDiscount,


                    OrderItems = OrderItems,
                    OrderDiscount = DiscountPercentOnTotalPrice,
                    Number = OrderService.GenerateNumber(1),
                    ShippingContact = orderContact,
                    BillingContact = orderContact,
                    Certificate = orderCertificate,
                    Coupon = orderCoupon,
                    AdminOrderComment = Resources.Resource.Client_BuyInOneClick_Header,
                    PaymentMethodId  = AdvantShop.Payment.PaymentService.GetAllPaymentMethods(true).Where(t => t.Name.Contains(context.Request["payment"])).FirstOrDefault().PaymentMethodID
                    //ShippingMethodId = AdvantShop.Shipping.ShippingMethodService.GetAllShippingMethods(true).Where(t => t.Name.Contains(context.Request["shipping"])).FirstOrDefault().ShippingMethodId
                  
                };

            order.ShippingMethodId = AdvantShop.Shipping.ShippingMethodService.GetAllShippingMethods(true).Where(t => t.Name.Contains(context.Request["shipping"])).FirstOrDefault().ShippingMethodId;
            order.ArchivedShippingName = AdvantShop.Shipping.ShippingMethodService.GetAllShippingMethods(true).Where(t => t.Name.Contains(context.Request["shipping"])).FirstOrDefault().Name;
            
            //Added by Evgeni Kovalev for instrument-opt.by need to change 
            if (totalPrice < 150)
            {
                order.ShippingCost = 15;
            }
            else if (totalPrice >= 150)
            {
                order.ShippingCost = 0;
            }
            
                
                
            /////
            
            order.OrderID = OrderService.AddOrder(order);
            order.Number = OrderService.GenerateNumber(order.OrderID);
            
            OrderService.UpdateNumber(order.OrderID, order.Number);
            
            OrderService.ChangeOrderStatus(order.OrderID, OrderService.DefaultOrderStatus);

            ModulesRenderer.OrderAdded(order.OrderID);

            if (order.OrderID != 0)
            {
                try
                {
                  var  ordTable = OrderService.GenerateHtmlOrderTable(
                               order.OrderItems,
                               CurrencyService.CurrentCurrency,
                               totalPrice,
                               DiscountPercentOnTotalPrice,
                               orderCoupon,
                               orderCertificate,
                               DiscountPercentOnTotalPrice > 0 ? DiscountPercentOnTotalPrice * totalPrice / 100 : 0,
                               order.ShippingCost,
                               0,
                               CustomerSession.CurrentCustomer.Contacts.Count > 0
                                   ? CustomerSession.CurrentCustomer.Contacts[0]
                                   : new CustomerContact(),
                               CustomerSession.CurrentCustomer.Contacts.Count > 0
                                   ? CustomerSession.CurrentCustomer.Contacts[0]
                                   : new CustomerContact());
                    
                    string message = SendMail.BuildMail(new ClsMailParamOnBuyInOneClick
                        {
                            Name = context.Request["name"] + ' ' + context.Request["familyName"],
                            Adres = context.Request["adres"],
                            Email = context.Request["email"],
                            Phone = context.Request["phone"],
                            Comment = context.Request["comment"],
                            OrderTable = ordTable,
                        });
                   
                    //Send mail to Admin
                    SendMail.SendMailNow(AdvantShop.Configuration.SettingsMail.EmailForOrders, AdvantShop.Configuration.SettingsMain.SiteUrl + " - " + Resources.Resource.Client_BuyInOneClick_Header, message, true);
                   
                   
                    //Send mail to Customer
                    string htmlMessage = SendMail.BuildMail(new ClsMailParamOnNewOrder
                    {
                        CustomerContacts = context.Request["name"] + ' ' + context.Request["familyName"],
                        PaymentType = order.PaymentMethodName,
                        ShippingMethod = order.ArchivedShippingName,
                        CurrentCurrencyCode = CurrencyService.CurrentCurrency.Iso3,
                        TotalPrice = order.Sum.ToString(),
                        Comments = order.CustomerComment,
                        Email = context.Request["email"],
                        OrderTable = ordTable,
                        OrderID = order.OrderID.ToString(),
                        Number = order.Number
                    });


                    SendMail.SendMailNow(AdvantShop.Configuration.SettingsMail.EmailForOrders + "," + context.Request["email"], Resources.Resource.Client_OrderConfirmation_ReceivedOrder + " " + order.OrderID, htmlMessage, true);
                   
  
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                }
            }

            if (order.OrderID != 0 && page == OrderConfirmationService.BuyInOneclickPage.shoppingcart)
            {
                ShoppingCartService.ClearShoppingCart(ShoppingCartType.ShoppingCart, CustomerSession.CustomerId);
                ReturnResult(context, "reload");
                return;
            }
        }
        catch (Exception ex) {
            int i = 0;
            i++;
        }
        
    }

    private static void ReturnResult(HttpContext context, string result)
    {
        context.Response.ContentType = "application/json";
        context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { result }));
        context.Response.End();
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}
