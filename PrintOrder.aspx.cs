//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using AdvantShop;
using AdvantShop.Catalog;
using AdvantShop.Diagnostics;
using AdvantShop.Orders;
using AdvantShop.Repository.Currencies;
using AdvantShop.Taxes;
using Resources;

public partial class PrintOrder : Page
{
    protected override void InitializeCulture()
    {
        AdvantShop.Localization.Culture.InitializeCulture();
    }

    protected OrderCurrency ordCurrency = null;

    protected string RenderSelectedOptions(IList<EvaluatedCustomOptions> evlist)
    {
        if (evlist == null || evlist.Count == 0)
            return "&nbsp;";

        var html = new StringBuilder();
        html.Append("<ul>");

        foreach (EvaluatedCustomOptions ev in evlist)
        {
            html.Append(string.Format("<li>{0}: {1}</li>", ev.CustomOptionTitle, ev.OptionTitle));
        }

        html.Append("</ul>");
        return html.ToString();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Request["ordernumber"]))
        {
            Response.Redirect("default.aspx");
        }

        try
        {
            int orderId = OrderService.GetOrderIdByNumber(Request["ordernumber"]);

            lblOrderID.Text = Resource.Admin_ViewOrder_ItemNum + orderId;
            //Added by Evgeni
            lOrder0.Text = orderId.ToString();

            if (!IsPostBack)
            {
                Order ord = OrderService.GetOrder(orderId);

                if (ord != null)
                {
                    ordCurrency = ord.OrderCurrency;
                    lOrderDate.Text = AdvantShop.Localization.Culture.ConvertDate(ord.OrderDate);

                   
                    lNumber.Text  = ord.Number;

                    lblOrderStatus.Text = @"(" + ord.OrderStatus.StatusName + @")";

                    lblShipping.Text = @"<b>" + Resource.Admin_ViewOrder_Name + @"</b>&nbsp;" + ord.ShippingContact.Name;
                    lblShipping.Text += @"<br />";
                    lblShipping.Text += @"&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>" + Resource.Admin_ViewOrder_Country +
                                        @"</b>&nbsp;" + ord.ShippingContact.Country + @"<br />";
                    lblShipping.Text += @"&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>" + Resource.Admin_ViewOrder_City +
                                        @"</b>&nbsp;" + ord.ShippingContact.City + @"<br />";

                    if (ord.ShippingContact.Zone != null)
                    {
                        lblShipping.Text += @"&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>" + Resource.Admin_ViewOrder_Zone +
                                            @"</b>&nbsp;" + ord.ShippingContact.Zone.Trim() + @"<br />";
                    }

                    if (ord.ShippingContact.Zip != null)
                    {
                        lblShipping.Text += @"&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>" + Resource.Admin_ViewOrder_Zip +
                                            @"</b>&nbsp;" + ord.ShippingContact.Zip.Trim() + @"<br />";
                    }

                    lblShipping.Text += @"&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>" + Resource.Admin_ViewOrder_Address +
                                        @"</b>&nbsp;" + ord.ShippingContact.Address + @"<br />";


                    lblBilling.Text = @"<b>" + Resource.Admin_ViewOrder_Name + @"</b>&nbsp;" + ord.BillingContact.Name;
                    lblBilling.Text += @"<br />";
                    lblBilling.Text += @"&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>" + Resource.Admin_ViewOrder_Country +
                                       @"</b>&nbsp;" + ord.BillingContact.Country + @"<br />";
                    lblBilling.Text += @"&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>" + Resource.Admin_ViewOrder_City +
                                       @"</b>&nbsp;" + ord.BillingContact.City + @"<br />";

                    if (ord.BillingContact.Zone != null)
                    {
                        lblBilling.Text += @"&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>" + Resource.Admin_ViewOrder_Zone +
                                           @"</b>&nbsp;" + ord.BillingContact.Zone.Trim() + @"<br />";
                    }

                    if (ord.BillingContact.Zip != null)
                    {
                        lblBilling.Text += @"&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>" + Resource.Admin_ViewOrder_Zip +
                                           @"</b>&nbsp;" + ord.BillingContact.Zip.Trim() + @"<br />";
                    }

                    lblBilling.Text += @"&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>" + Resource.Admin_ViewOrder_Address +
                                       @"</b>&nbsp;" + ord.BillingContact.Address + @"<br />";
                    
                    lblShippingMethod.Text = @"<b>" + ord.ArchivedShippingName + @"</b>";

                    if (ord.OrderPickPoint != null)
                    {
                        lblShippingMethod.Text += @"</br><b>" + ord.OrderPickPoint.PickPointAddress + @"</b>";
                    }

                    lblPaymentType.Text = @"<b>" + ord.PaymentMethodName + @"</b>";

                    // ------------------------------------------------------------------------------------


                    decimal productPrice = ord.OrderItems.Sum(item => item.Amount * item.Price);
                    decimal totalDiscount = ord.OrderDiscount;

                    lblProductPrice.Text = CatalogService.GetStringPrice(productPrice, ordCurrency.CurrencyValue, ordCurrency.CurrencyCode);

                    trDiscount.Visible = ord.OrderDiscount != 0;
                    lblOrderDiscount.Text = string.Format("-{0}", CatalogService.GetStringDiscountPercent(productPrice, totalDiscount,
                                                                              ordCurrency.CurrencyValue, ord.OrderCurrency.CurrencySymbol,
                                                                              ord.OrderCurrency.IsCodeBefore, CurrencyService.CurrentCurrency.PriceFormat, false));

                    if (ord.Certificate != null)
                    {
                        trCertificate.Visible = true;
                        lblCertificate.Text = string.Format("-{0}",
                                                            CatalogService.GetStringPrice(ord.Certificate.Price,
                                                                                          ordCurrency.CurrencyValue,
                                                                                          ordCurrency.CurrencyCode));
                    }
                    else
                    {
                        trCertificate.Visible = false;
                    }

                    if (ord.Coupon != null)
                    {
                        trCoupon.Visible = true;
                        switch (ord.Coupon.Type)
                        {
                            case CouponType.Fixed:
                                lblCoupon.Text = String.Format("-{0} ({1})",
                                                               CatalogService.GetStringPrice(ord.Coupon.Value,
                                                                                             ordCurrency.CurrencyValue,
                                                                                             ordCurrency.CurrencyCode),
                                                               ord.Coupon.Code);
                                break;
                            case CouponType.Percent:
                                lblCoupon.Text = String.Format("-{0} ({1}%) ({2})",
                                                               CatalogService.GetStringPrice(ord.Coupon.Value,
                                                                                             ordCurrency.CurrencyValue,
                                                                                             ordCurrency.CurrencyCode),
                                                               CatalogService.FormatPriceInvariant(ord.Coupon.Value),
                                                               ord.Coupon.Code);
                                break;
                        }
                    }
                    else
                    {
                        trCoupon.Visible = false;
                    }

                    lblShippingPrice.Text = string.Format("+{0}", CatalogService.GetStringPrice(ord.ShippingCost, ord.OrderCurrency.CurrencyValue, ord.OrderCurrency.CurrencyCode));

                    List<TaxValue> taxedItems = TaxServices.GetOrderTaxes(ord.OrderID);
                    literalTaxCost.Text = TaxServices.BuildTaxTable(taxedItems, ord.OrderCurrency.CurrencyValue, ord.OrderCurrency.CurrencyCode, Resource.Admin_ViewOrder_Taxes);
                    //Added By Evgeni
                    lblTotalPrice.Text = lblTotalPrice0 .Text  = CatalogService.GetStringPrice(ord.Sum, ord.OrderCurrency.CurrencyValue, ord.OrderCurrency.CurrencyCode);

                    // ------------------------------------------------------------------------------------

                    if (string.IsNullOrEmpty(ord.CustomerComment))
                    {
                        lblUserComments.Visible = false;
                    }
                    else
                    {
                        lblUserComments.Text = Resource.Client_PrintOrder_YourComment + @" <br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + ord.CustomerComment;
                        lblUserComments.Visible = true;
                    }

                    lStatusComment.Text = ord.StatusComment;

                    DataListOrderItems.DataSource = ord.OrderItems;
                    DataListOrderItems.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
    }
}