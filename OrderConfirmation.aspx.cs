using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Controls;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.Mails;
using AdvantShop.Modules;
using AdvantShop.Modules.Interfaces;
using AdvantShop.Orders;
using AdvantShop.Payment;
using AdvantShop.Repository.Currencies;
using AdvantShop.Security;
using AdvantShop.SEO;
using AdvantShop.Shipping;
using Resources;

public partial class OrderConfirmation : AdvantShopPage
{
    protected AdvantShop.Orders.OrderConfirmation PageData;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            var ocModule = AttachedModules.GetModules(AttachedModules.EModuleType.OrderConfirmation).FirstOrDefault();
            if(ocModule != null)
            {
                var classInstance = (IOrderConfirmation)Activator.CreateInstance(ocModule, null);
                if (classInstance.IsActive && classInstance.CheckAlive() && !string.IsNullOrEmpty(classInstance.FileUserControlOrderConfirmation))
                {
                    Redirect("orderconfirmationmodule.aspx", true);
                }
            }
        }

        var shoppingCart = ShoppingCartService.CurrentShoppingCart;
        PageData = LoadOrderConfirmationData(CustomerSession.CustomerId);

        if (!shoppingCart.CanOrder && (PageData.OrderConfirmationData.ActiveTab != EnActiveTab.FinalTab))
        {
            Redirect("shoppingcart.aspx", true);
        }

        //check current step and save state in db
        var activeT = EnActiveTab.NoTab;

        if (!IsPostBack)
        {
            Enum.TryParse(Request["tab"], true, out activeT);

            if (activeT != PageData.OrderConfirmationData.ActiveTab)
            {
                if (activeT < PageData.OrderConfirmationData.ActiveTab)
                    PageData.OrderConfirmationData.ActiveTab = activeT;
                Redirect(PageData);
            }
        }
        if (PageData.OrderConfirmationData.ActiveTab == EnActiveTab.FinalTab)
        {
            OrderConfirmationService.Delete(CustomerSession.CustomerId);
            LoadOrderConfirmationData(CustomerSession.CustomerId);
        }

        if (PageData.OrderConfirmationData.ActiveTab == EnActiveTab.NoTab)
            //ShowTab(PageData.OrderConfirmationData.UserType == EnUserType.RegistredUser ? EnActiveTab.UserTab : EnActiveTab.DefaultTab);
            //Changed by Evgeni to prevent user registration
            ShowTab(EnActiveTab.UserTab);
        else
            ShowTab(PageData.OrderConfirmationData.ActiveTab);

        SetMeta(new MetaInfo(string.Format("{0} - {1}", Resource.Client_OrderConfirmation_DrawUpOrder, SettingsMain.ShopName)), string.Empty);
    }


    //chose way 
    protected void ZeroStep_OnNextStep(object sender, UserControls_OrderConfirmation_ZeroStep.ZeroStepNextEventArgs e)
    {
        PageData.OrderConfirmationData.UserType = e.UserType;
        PageData.OrderConfirmationData.ActiveTab = EnActiveTab.UserTab;
        Redirect(PageData);
    }

    private void Redirect(AdvantShop.Orders.OrderConfirmation data)
    {
        if (Request.UrlReferrer != null && !Request.UrlReferrer.AbsoluteUri.Contains("orderconfirmation"))
        {
            PageData.OrderConfirmationData.ActiveTab = EnActiveTab.UserTab;
        }
        OrderConfirmationService.Update(PageData);
        Response.Redirect("orderconfirmation.aspx?tab=" + PageData.OrderConfirmationData.ActiveTab.ToString());
    }

    //chose address
    protected void FirstStep_OnNextStep(object sender, UserControls_OrderConfirmation_FirstStep.FirstStepNextEventArgs e)
    {
        PageData.OrderConfirmationData.ShippingContact = e.ShippingContact;
        PageData.OrderConfirmationData.BillingContact = e.BillingContact;
        PageData.OrderConfirmationData.Customer = e.Customer;
        PageData.OrderConfirmationData.BillingIsShipping = e.BillingIsShipping;
        PageData.OrderConfirmationData.CheckSum = ShoppingCartService.CurrentShoppingCart.GetHashCode();

        if (ShoppingCartService.CurrentShoppingCart.All(item => item.ItemType == EnumItemType.Certificate))
        {
            PageData.OrderConfirmationData.ActiveTab = EnActiveTab.PaymentTab;
            PageData.OrderConfirmationData.SelectShippingName = ShoppingCartService.CurrentShoppingCart.First(item => item.ItemType == EnumItemType.Certificate).
                    Certificate.Type == CertificatePostType.Mail
                    ? Resource.Client_GiftCertificate_DeliveryByEmail
                    : Resource.Client_GiftCertificate_DeliveryByPost;
            PageData.OrderConfirmationData.SelectShippingId = 0;
            PageData.OrderConfirmationData.SelectShippingRate = 0;
        }
        else
        {
            PageData.OrderConfirmationData.ActiveTab = EnActiveTab.ShippingTab;
        }

        Redirect(PageData);
    }

    //goto zero step
    protected void FirstStep_OnBackStep(object sender, EventArgs e)
    {
        PageData.OrderConfirmationData.UserType = EnUserType.NoUser;
        PageData.OrderConfirmationData.ActiveTab = EnActiveTab.DefaultTab;
        Redirect(PageData);
    }

    //chose shipping
    protected void SecondStep_OnNextStep(object sender, UserControls_OrderConfirmation_SecondStep.SecondStepNextEventArgs e)
    {
        PageData.OrderConfirmationData.SelectShippingId = e.SelectShippingID;
        PageData.OrderConfirmationData.SelectShippingName = e.SelectedShippingText;
        PageData.OrderConfirmationData.SelectShippingRate = e.SelectedShippingRate;
        PageData.OrderConfirmationData.SelectShippingButtonId = e.SelectShippingButtonID;
        PageData.OrderConfirmationData.ShippingOptionEx = e.SelectedShippingExt;

        PageData.OrderConfirmationData.ActiveTab = EnActiveTab.PaymentTab;
        Redirect(PageData);
    }

    //goto chose address
    protected void SecondStep_OnBackStep(object sender, UserControls_OrderConfirmation_SecondStep.SecondStepNextEventArgs e)
    {
        PageData.OrderConfirmationData.SelectShippingButtonId = e.SelectShippingButtonID;
        PageData.OrderConfirmationData.ActiveTab = EnActiveTab.UserTab;
        Redirect(PageData);
    }

    //chose  payment
    protected void ThirdStep_OnNextStep(object sender, UserControls_OrderConfirmation_ThirdStep.ThirdStepNextEventArgs e)
    {
        PageData.OrderConfirmationData.SelectPaymentId = e.SelectPaymentID;
        PageData.OrderConfirmationData.SelectPaymentName = e.SelectedPaymentText;
        PageData.OrderConfirmationData.SelectedPaymentProcessType = e.SelectedPaymentProcessType;
        PageData.OrderConfirmationData.SelectedPaymentType = e.SelectedPaymentType;
        PageData.OrderConfirmationData.ActiveTab = EnActiveTab.SumTab;
        Redirect(PageData);
    }

    //goto chose address
    protected void ThirdStep_OnBackStep(object sender, UserControls_OrderConfirmation_ThirdStep.ThirdStepNextEventArgs e)
    {

        if (ShoppingCartService.CurrentShoppingCart.Count(item => item.ItemType == EnumItemType.Product) != 0)
        {
            PageData.OrderConfirmationData.SelectPaymentId = e.SelectPaymentID;
            PageData.OrderConfirmationData.ActiveTab = EnActiveTab.ShippingTab;
        }
        else
        {
            PageData.OrderConfirmationData.ActiveTab = EnActiveTab.UserTab;
        }

        Redirect(PageData);
    }

    //create customer if with reg and create order
    protected void FourthStep_OnNextStep(object sender, UserControls_FourthStep.FourthStepNextEventArgs e)
    {
        PageData.OrderConfirmationData.CustomerComment = e.CustomerComment;
        PageData.OrderConfirmationData.PaymentDetails = e.PaymentDetails;
        PageData.OrderConfirmationData.ActiveTab = EnActiveTab.FinalTab;
        Redirect(PageData);
    }

    //goto chose shipping and payment
    protected void ThirdStep_OnBackStep(object sender, EventArgs e)
    {
        //ShowTab(EnActiveTab.ShippingPaymentTab);
        if (ShoppingCartService.CurrentShoppingCart.Count(item => item.ItemType == EnumItemType.Product) != 0)
        {
            PageData.OrderConfirmationData.ActiveTab = EnActiveTab.PaymentTab;
        }
        else
        {
            PageData.OrderConfirmationData.ActiveTab = EnActiveTab.UserTab;
        }
        Redirect(PageData);
    }

    // load or create new data for current state
    private AdvantShop.Orders.OrderConfirmation LoadOrderConfirmationData(Guid id)
    {
        AdvantShop.Orders.OrderConfirmation res;

        if (!OrderConfirmationService.IsExist(id))
        {
            res = new AdvantShop.Orders.OrderConfirmation
                      {
                          CustomerId = id,
                          OrderConfirmationData = new OrderConfirmationData { UserType = EnUserType.NoUser, BillingIsShipping = true }
                      };
            // if we have user 
            if (CustomerSession.CurrentCustomer.RegistredUser)
                res.OrderConfirmationData.UserType = EnUserType.RegistredUser;
            OrderConfirmationService.Add(res);
        }
        else
        {
            res = OrderConfirmationService.Get(id);
        }

        return res;
    }

    private void ShowTab(EnActiveTab tab)
    {
        PageData.OrderConfirmationData.ActiveTab = tab;
        ShowActiveTab(false);
    }

    private void ShowActiveTab(bool innerPostback)
    {
        switch (PageData.OrderConfirmationData.ActiveTab)
        {
            case EnActiveTab.DefaultTab:
                mvOrderConfirm.SetActiveView(ViewAuthorizationCheck);
                break;

            case EnActiveTab.UserTab:
                FirstStep.UserType = PageData.OrderConfirmationData.UserType;
                FirstStep.BillingIsShipping = PageData.OrderConfirmationData.BillingIsShipping;
                FirstStep.ShippingContact = PageData.OrderConfirmationData.ShippingContact;
                FirstStep.BillingContact = PageData.OrderConfirmationData.BillingContact;
                FirstStep.Customer = PageData.OrderConfirmationData.Customer;
                mvOrderConfirm.SetActiveView(ViewOrderConfirmationUser);
                break;

            case EnActiveTab.ShippingTab:
                SecondStep.PageData = PageData.OrderConfirmationData;
                mvOrderConfirm.SetActiveView(ViewOrderConfirmationShipping);
                break;

            case EnActiveTab.PaymentTab:
                ThirdStep.PageData = PageData.OrderConfirmationData;
                mvOrderConfirm.SetActiveView(ViewOrderConfirmationPayment);
                break;

            case EnActiveTab.SumTab:
                FourthStep.PageData = PageData.OrderConfirmationData;
                mvOrderConfirm.SetActiveView(ViewOrderConfirmationSum);
                break;

            case EnActiveTab.FinalTab:
                if (!innerPostback)
                {
                    var order = DoCreateOrder();
                    FifthStep.OrderID = order.OrderID;
                    FifthStep.Number = order.Number;
                    ShippingManager.CurrentShippingRates.Clear();
                }
                mvOrderConfirm.SetActiveView(ViewOrderConfirmationFinal);
                OrderConfirmationService.Delete(CustomerSession.CustomerId);
                break;

            default:
                mvOrderConfirm.SetActiveView(ViewAuthorizationCheck);
                break;
        }

        ltSteps.Text = RenderSteps(PageData.OrderConfirmationData.ActiveTab);
    }


    private Order DoCreateOrder()
    {
        var shoppingCart = ShoppingCartService.CurrentShoppingCart;
        if (shoppingCart.GetHashCode() != PageData.OrderConfirmationData.CheckSum || !shoppingCart.HasItems)
        {
            Redirect("shoppingcart.aspx");
            return null;
        }

        if (PageData.OrderConfirmationData.UserType == EnUserType.JustRegistredUser)
        {
            RegistrationNow();
        }


        var ord = CreateOrder(shoppingCart);

        var paymentMethod = PaymentService.GetPaymentMethod(ord.PaymentMethodId);
        string email = PageData.OrderConfirmationData.Customer.EMail;

        string htmlOrderTable = OrderService.GenerateHtmlOrderTable(ord.OrderItems, CurrencyService.CurrentCurrency,
                                                                    shoppingCart.TotalPrice,
                                                                    shoppingCart.DiscountPercentOnTotalPrice,
                                                                    ord.Coupon, ord.Certificate,
                                                                    shoppingCart.TotalDiscount,
                                                                    ord.ShippingCost,
                                                                    PageData.OrderConfirmationData.TaxesTotal,
                                                                    PageData.OrderConfirmationData.BillingContact,
                                                                    PageData.OrderConfirmationData.ShippingContact);

        // declare class to collect info about what was buy
        var googleAnalystic = new GoogleAnalyticsString();
        var trans = new GoogleAnalyticsTrans
        {
            OrderId = ord.OrderID.ToString(),
            Affiliation = SettingsMain.ShopName,
            Total = shoppingCart.TotalPrice.ToString("F2", System.Globalization.CultureInfo.InvariantCulture),
            Tax = (PageData.OrderConfirmationData.TaxesTotal).ToString("F2", System.Globalization.CultureInfo.InvariantCulture),
            Shipping = ord.ShippingCost.ToString("F2", System.Globalization.CultureInfo.InvariantCulture),
            City = string.Empty,
            State = string.Empty,
            Country = string.Empty,
        };
        googleAnalystic.Trans = trans;
        googleAnalystic.Items = GetListItemForGoogleAnalytics(shoppingCart, ord.OrderID.ToString());
        ltGaECommerce.Text = googleAnalystic.GetGoogleAnalyticsEComerceString();


        // Build a new mail
        var customerSb = new StringBuilder();
        customerSb.AppendFormat(Resource.Client_Registration_Name + ": {0}<br/>", PageData.OrderConfirmationData.Customer.FirstName);
        customerSb.AppendFormat(Resource.Client_Registration_Surname + ": {0}<br/>", PageData.OrderConfirmationData.Customer.LastName);
        customerSb.AppendFormat(Resource.Client_Registration_Phone + ": {0}<br/>", PageData.OrderConfirmationData.Customer.Phone);
        customerSb.AppendFormat(Resource.Client_Registration_Country + ": {0}<br/>", PageData.OrderConfirmationData.ShippingContact.Country);
        customerSb.AppendFormat(Resource.Client_Registration_State + ": {0}<br/>", PageData.OrderConfirmationData.ShippingContact.RegionName);
        customerSb.AppendFormat(Resource.Client_Registration_City + ": {0}<br/>", PageData.OrderConfirmationData.ShippingContact.City);
        customerSb.AppendFormat(Resource.Client_Registration_Zip + ": {0}<br/>", PageData.OrderConfirmationData.ShippingContact.Zip);
        customerSb.AppendFormat(Resource.Client_Registration_Address + ": {0}<br/>", string.IsNullOrEmpty(PageData.OrderConfirmationData.ShippingContact.Address) ? Resource.Client_OrderConfirmation_NotDefined : PageData.OrderConfirmationData.ShippingContact.Address);
        customerSb.AppendFormat("Email: {0}<br/>", PageData.OrderConfirmationData.Customer.EMail);


        string htmlMessage = SendMail.BuildMail(new ClsMailParamOnNewOrder
        {
            CustomerContacts = customerSb.ToString(),
            PaymentType = PageData.OrderConfirmationData.SelectPaymentName,
            ShippingMethod = PageData.OrderConfirmationData.SelectShippingName,
            CurrentCurrencyCode = CurrencyService.CurrentCurrency.Iso3,
            TotalPrice = ord.Sum.ToString(),
            Comments = ord.CustomerComment,
            Email = email,
            OrderTable = htmlOrderTable,
            OrderID = ord.OrderID.ToString(),
            Number = ord.Number
        });

        if (!CustomerSession.CurrentCustomer.IsVirtual)
        {
            if (paymentMethod != null)
            {
                SendMail.SendMailNow(email, Resource.Client_OrderConfirmation_ReceivedOrder + " " + ord.OrderID,
                                     htmlMessage, true);
                SendMail.SendMailNow(SettingsMail.EmailForOrders,
                                     Resource.Client_OrderConfirmation_ReceivedOrder + " " + ord.OrderID, htmlMessage,
                                     true);
            }
            else
            {
                htmlMessage += " ERROR: \'" + "\'";
                SendMail.SendMailNow(SettingsMail.EmailForOrders,
                                     Resource.Client_OrderConfirmation_OrderError + " " + ord.OrderID, htmlMessage, true);
            }
        }

        var certificate = shoppingCart.Certificate;
        if (certificate != null)
        {
            certificate.OrderNumber = ord.Number;
            certificate.Used = true;
            certificate.Enable = true;

            GiftCertificateService.DeleteCustomerCertificate(certificate.CertificateId);
            GiftCertificateService.UpdateCertificateById(certificate);
        }

        var coupon = shoppingCart.Coupon;
        if (coupon != null && shoppingCart.TotalPrice >= coupon.MinimalOrderPrice)
        {
            coupon.ActualUses += 1;
            CouponService.UpdateCoupon(coupon);
            CouponService.DeleteCustomerCoupon(coupon.CouponID);
        }

        ShoppingCartService.ClearShoppingCart(ShoppingCartType.ShoppingCart, PageData.OrderConfirmationData.Customer.Id);
        ShoppingCartService.ClearShoppingCart(ShoppingCartType.ShoppingCart, CustomerSession.CustomerId);
        return ord;
    }

    private Order CreateOrder(ShoppingCart shoppingCart)
    {
        var customerGroup = CustomerSession.CurrentCustomer.CustomerGroup;
        var ord = new Order
        {
            OrderCustomer = new OrderCustomer
            {
                CustomerIP = Request.UserHostAddress,
                CustomerID = PageData.OrderConfirmationData.Customer.Id,
                FirstName = PageData.OrderConfirmationData.Customer.FirstName,
                LastName = PageData.OrderConfirmationData.Customer.LastName,
                Email = PageData.OrderConfirmationData.Customer.EMail,
                MobilePhone = PageData.OrderConfirmationData.Customer.Phone,

            },
            OrderCurrency = new OrderCurrency
            {
                CurrencyCode = CurrencyService.CurrentCurrency.Iso3,
                CurrencyValue = CurrencyService.CurrentCurrency.Value,
                CurrencySymbol = CurrencyService.CurrentCurrency.Symbol,
                CurrencyNumCode = CurrencyService.CurrentCurrency.NumIso3,
                IsCodeBefore = CurrencyService.CurrentCurrency.IsCodeBefore
            },
            OrderStatusId = OrderService.DefaultOrderStatus,
            AffiliateID = 0,
            ShippingCost = PageData.OrderConfirmationData.SelectedPaymentType == PaymentType.CashOnDelivery
                                                                              ? PageData.OrderConfirmationData.ShippingOptionEx != null ? PageData.OrderConfirmationData.ShippingOptionEx.PriceCash : 0
                                                                              : PageData.OrderConfirmationData.SelectShippingRate,
            OrderDate = DateTime.Now,
            CustomerComment = PageData.OrderConfirmationData.CustomerComment,
            ShippingContact = new OrderContact
            {
                Name = PageData.OrderConfirmationData.ShippingContact.Name,
                Country = PageData.OrderConfirmationData.ShippingContact.Country,
                Zone = PageData.OrderConfirmationData.ShippingContact.RegionName,
                City = PageData.OrderConfirmationData.ShippingContact.City,
                Zip = PageData.OrderConfirmationData.ShippingContact.Zip,
                Address = PageData.OrderConfirmationData.ShippingContact.Address,
            },

            GroupName = customerGroup.GroupName,
            GroupDiscount = customerGroup.GroupDiscount,
            OrderDiscount = shoppingCart.DiscountPercentOnTotalPrice
        };



        foreach (var orderItem in shoppingCart.Select(item => (OrderItem)item))
        {
            ord.OrderItems.Add(orderItem);
        }

        if (!PageData.OrderConfirmationData.BillingIsShipping)
        {
            ord.BillingContact = new OrderContact
            {
                Name = PageData.OrderConfirmationData.BillingContact.Name,
                Country = PageData.OrderConfirmationData.BillingContact.Country,
                Zone = PageData.OrderConfirmationData.BillingContact.RegionName,
                City = PageData.OrderConfirmationData.BillingContact.City,
                Zip = PageData.OrderConfirmationData.BillingContact.Zip,
                Address = PageData.OrderConfirmationData.BillingContact.Address,
            };
        }

        ord.ShippingMethodId = PageData.OrderConfirmationData.SelectShippingId;
        ord.ShippingMethodId = shoppingCart.Count(item => item.ItemType == EnumItemType.Product) == 0 ? 0 : PageData.OrderConfirmationData.SelectShippingId;
        ord.PaymentMethodId = PageData.OrderConfirmationData.SelectPaymentId;

        ord.ArchivedShippingName = shoppingCart.Count(item => item.ItemType == EnumItemType.Product) == 0 ? Resource.Client_OrderConfirmation_FreeShipping : PageData.OrderConfirmationData.SelectShippingName;
        ord.ArchivedPaymentName = PageData.OrderConfirmationData.SelectPaymentName;

        ord.PaymentDetails = PageData.OrderConfirmationData.PaymentDetails;

        if (PageData.OrderConfirmationData.ShippingOptionEx != null && !string.IsNullOrEmpty(PageData.OrderConfirmationData.ShippingOptionEx.Pickpointmap))
            ord.OrderPickPoint = new OrderPickPoint
                                     {
                                         PickPointId = PageData.OrderConfirmationData.ShippingOptionEx.PickpointId,
                                         PickPointAddress =
                                             PageData.OrderConfirmationData.ShippingOptionEx.PickpointAddress
                                     };

        ord.Number = OrderService.GenerateNumber(1); // For crash protection

        GiftCertificate certificate = shoppingCart.Certificate;
        Coupon coupon = shoppingCart.Coupon;

        if (certificate != null)
        {
            ord.Certificate = new OrderCertificate()
                {
                    Code = certificate.CertificateCode,
                    Price = certificate.Sum
                };
        }
        if (coupon != null && shoppingCart.TotalPrice >= coupon.MinimalOrderPrice)
        {
            ord.Coupon = new OrderCoupon()
                {
                    Code = coupon.Code,
                    Type = coupon.Type,
                    Value = coupon.Value
                };
        }



        ord.OrderID = OrderService.AddOrder(ord);
        ord.Number = OrderService.GenerateNumber(ord.OrderID); // new number
        OrderService.UpdateNumber(ord.OrderID, ord.Number);
        OrderService.ChangeOrderStatus(ord.OrderID, OrderService.DefaultOrderStatus);

        ModulesRenderer.OrderAdded(ord.OrderID);
        return ord;
    }

    private static List<GoogleAnalyticsItem> GetListItemForGoogleAnalytics(IEnumerable<ShoppingCartItem> table, string orderid)
    {
        if (table == null)
            return new List<GoogleAnalyticsItem>();

        return table.Where(item => item.ItemType == EnumItemType.Product).Select(item => new GoogleAnalyticsItem
        {
            OrderId = orderid,
            Sku = item.Product.ArtNo,
            Name = item.Product.Name,
            Category = ProductService.GetFirstCategoryIdByProductId(item.EntityId) != -1 ? CategoryService.GetCategory(ProductService.GetFirstCategoryIdByProductId(item.EntityId)).Name : "",
            Price = item.Product.Price.ToString("F2", System.Globalization.CultureInfo.InvariantCulture),
            Quantity = item.Amount.ToString()
        }).ToList();
    }

    private void RegistrationNow()
    {
        try
        {
            if (CustomerService.CheckCustomerExist(PageData.OrderConfirmationData.Customer.EMail))
            {
                ShowMessage(Notify.NotifyType.Error, Resource.Client_Registration_CustomerExist);
            }


            Guid id = CustomerService.InsertNewCustomer(new Customer
                                                            {
                                                                CustomerGroupId = CustomerGroupService.DefaultCustomerGroup,
                                                                Password = PageData.OrderConfirmationData.Customer.Password,
                                                                FirstName = PageData.OrderConfirmationData.Customer.FirstName,
                                                                LastName = PageData.OrderConfirmationData.Customer.LastName,
                                                                Phone = PageData.OrderConfirmationData.Customer.Phone,
                                                                SubscribedForNews = false,
                                                                EMail = PageData.OrderConfirmationData.Customer.EMail,
                                                                CustomerRole = Role.User
                                                            });

            if (id == Guid.Empty)
            {
                return;
            }
            PageData.OrderConfirmationData.Customer.Id = id;

            AuthorizeService.AuthorizeTheUser(PageData.OrderConfirmationData.Customer.EMail, PageData.OrderConfirmationData.Customer.Password, false);

            // Shipping contact -----------------------------
            var newContact = PageData.OrderConfirmationData.ShippingContact;
            CustomerService.AddContact(newContact, PageData.OrderConfirmationData.Customer.Id);

            // Billing contact ---------------------------
            if (!PageData.OrderConfirmationData.BillingIsShipping)
            {
                newContact = PageData.OrderConfirmationData.BillingContact;
                CustomerService.AddContact(newContact, PageData.OrderConfirmationData.Customer.Id);
            }

            //------------------------------------------

            var clsParam = new ClsMailParamOnRegistration
            {
                FirstName = PageData.OrderConfirmationData.Customer.FirstName,
                LastName = PageData.OrderConfirmationData.Customer.LastName,
                RegDate = AdvantShop.Localization.Culture.ConvertDate(DateTime.Now),
                Password = PageData.OrderConfirmationData.Customer.Password,
                Subsrcibe = Resource.Client_Registration_No,
                ShopURL = SettingsMain.SiteUrl
            };


            string message = SendMail.BuildMail(clsParam);

            if (CustomerSession.CurrentCustomer.IsVirtual)
            {
                ShowMessage(Notify.NotifyType.Notice, Resource.Client_Registration_Whom + PageData.OrderConfirmationData.Customer.EMail + '\r' + Resource.Client_Registration_Text + message);
            }
            else
            {
                SendMail.SendMailNow(PageData.OrderConfirmationData.Customer.EMail, SettingsMain.SiteUrl + " - " + string.Format(Resource.Client_Registration_RegSuccessful, PageData.OrderConfirmationData.Customer.EMail), message, true);
                SendMail.SendMailNow(SettingsMail.EmailForRegReport, SettingsMain.SiteUrl + " - " + string.Format(Resource.Client_Registration_RegSuccessful, PageData.OrderConfirmationData.Customer.EMail), message, true);
            }

            //------------------------------------------
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
            ShowMessage(Notify.NotifyType.Error, ex.Message + " at registration");
        }
    }

    private static string RenderSteps(EnActiveTab tab)
    {
        if (tab == EnActiveTab.DefaultTab || tab == EnActiveTab.FinalTab)
            return string.Empty;

        var result = new StringBuilder("<div class=\"steps pie\">");
        //ShippingAddress
        result.AppendFormat((tab == EnActiveTab.UserTab)
            ? "<span class=\"selected first pie\">{0}<span class=\"right\"></span><span class=\"left\"></span></span>"
            : "<a href=\"orderconfirmation.aspx?tab=usertab\" class=\"first\">{0}</a>", Resource.Client_OrderConfirmation_Steps_ShippingAddress);
        //ShippingMethods
        if (ShoppingCartService.CurrentShoppingCart.Count(item => item.ItemType == EnumItemType.Product) != 0)
        {
            result.AppendFormat((tab == EnActiveTab.ShippingTab)
                ? "<span class=\"selected\">{0}<span class=\"right\"></span><span class=\"left\"></span></span>"
                : (int)tab > 2 ? "<a href=\"orderconfirmation.aspx?tab=shippingtab\">{0}</a>" : "<span>{0}</span>", Resource.Client_OrderConfirmation_Steps_ShippingMethods);
        }
        //PaymentMethods
        result.AppendFormat((tab == EnActiveTab.PaymentTab)
            ? "<span class=\"selected\">{0}<span class=\"right\"></span><span class=\"left\"></span></span>"
            : (int)tab > 3 ? "<a href=\"orderconfirmation.aspx?tab=paymentTab\">{0}</a>" : "<span>{0}</span>", Resource.Client_OrderConfirmation_Steps_PaymentMethods);
        //SumTab
        result.AppendFormat((tab == EnActiveTab.SumTab)
            ? "<span class=\"selected\">{0}<span class=\"right\"></span><span class=\"left\"></span></span>"
            : "<span>{0}</span>", Resource.Client_OrderConfirmation_Steps_Confirmation);

        result.Append("</div>");
        return result.ToString();
    }
}