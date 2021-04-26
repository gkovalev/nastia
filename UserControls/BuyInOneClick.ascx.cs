//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop.Catalog;
using AdvantShop.Customers;
using System.Collections.Generic;
using AdvantShop.Orders;
using AdvantShop.Payment;
using System.Reflection;
using System.Linq;
using AdvantShop.Shipping;

public partial class UserControls_BuyInOneClick : System.Web.UI.UserControl
{
    public int ProductId;
    public List<CustomOption> CustomOptions;
    public List<OptionItem> SelectedOptions;
    public bool isShoppingCart = false;
    public OrderConfirmationService.BuyInOneclickPage pageEnum;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!isShoppingCart)
        {
            hfProductId.Value = ProductId.ToString();
            txtPhone.Text = CustomerSession.CurrentCustomer.Id != CustomerService.InternetUser.Id
                                ? CustomerSession.CurrentCustomer.Phone
                                : string.Empty;
            txtName.Text = CustomerSession.CurrentCustomer.Id != CustomerService.InternetUser.Id
                                ? CustomerSession.CurrentCustomer.FirstName 
                                : string.Empty;
            txtfamilyName.Text = CustomerSession.CurrentCustomer.Id != CustomerService.InternetUser.Id
                                 ? CustomerSession.CurrentCustomer.LastName
                                 : string.Empty;

            try
            {
                txtAdres.Text = CustomerSession.CurrentCustomer.Id != CustomerService.InternetUser.Id
                         ? CustomerSession.CurrentCustomer.Contacts[0].Address
                         : string.Empty;
                txtCity.Text = CustomerSession.CurrentCustomer.Id != CustomerService.InternetUser.Id
                     ? CustomerSession.CurrentCustomer.Contacts[0].City
                     : string.Empty;



            }
            catch
            {
                txtAdres.Text = string.Empty;
            }

            try
            {
                txtPayment.Text += PaymentService.GetAllPaymentMethods(true).FirstOrDefault().Name;
                txtShipping.Text += ShippingMethodService.GetAllShippingMethods(true).FirstOrDefault().Name;
            }
            catch { }


            txtMail.Text = CustomerSession.CurrentCustomer.Id != CustomerService.InternetUser.Id
                    ? CustomerSession.CurrentCustomer.EMail
                    : string.Empty;

          
        }

        if (Request.Url.AbsolutePath.ToLower().Contains("details"))
        {
            pageEnum = OrderConfirmationService.BuyInOneclickPage.details;
        }
        else if (Request.Url.AbsolutePath.ToLower().Contains("shoppingcart"))
        {
            pageEnum = OrderConfirmationService.BuyInOneclickPage.shoppingcart;
        }

    }
}