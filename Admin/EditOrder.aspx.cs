//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.Mails;
using AdvantShop.Modules;
using AdvantShop.Orders;
using AdvantShop.Payment;
using AdvantShop.Repository;
using AdvantShop.Repository.Currencies;
using AdvantShop.Shipping;
using AdvantShop.Taxes;
using Resources;

public partial class Admin_EditOrder : Page
{

    protected string OrderNumber
    {
        get { return (string)ViewState["OrderNumber"] ?? string.Empty; }
        set { ViewState["OrderNumber"] = value; }
    }

    protected int OrderID
    {
        get
        {
            if (ViewState["OrderID"] != null)
            {
                return (int)ViewState["OrderID"];
            }
            return 0;
        }
        set
        {
            ViewState["OrderID"] = value;
        }
    }

    protected bool AddingNewOrder
    {
        get { return (Request["orderid"] != null && Request["orderid"].ToLower() == "addnew"); }
    }

    protected string RenderSplitter()
    {
        var str = new StringBuilder();
        str.Append("<td class=\'splitter\'  onclick=\'togglePanel();return false;\' >");
        str.Append("<div class=\'leftPanelTop\'></div>");
        switch (Resource.Admin_Catalog_SplitterLang)
        {
            case "rus":
                str.Append("<div id=\'divHide\' class=\'hide_rus\'></div>");
                str.Append("<div id=\'divShow\' class=\'show_rus\'></div>");
                break;
            case "eng":
                str.Append("<div id=\'divHide\' class=\'hide_en\'></div>");
                str.Append("<div id=\'divShow\' class=\'show_en\'></div>");
                break;
        }
        str.Append("</td>");
        return str.ToString();
    }

    protected override void InitializeCulture()
    {
        AdvantShop.Localization.Culture.InitializeCulture();
    }

    protected void btnSearchID_Click(object sender, EventArgs e)
    {
        string ordID = string.Empty;
        try
        {
            ordID = int.Parse(txtSearchID.Text).ToString();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
            lblNotFound.Text = Resource.Admin_EditOrder_Id_must_be_int;
        }
        Response.Redirect("EditOrder.aspx?OrderID=" + ordID);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        MsgErr.Text = string.Empty;
        CalendarExtender1.Format = SettingsMain.AdminDateFormat;

        if (!IsPostBack)
        {
            if (AddingNewOrder)
            {
                pnCustomerSelect.Visible = true;
                btnSave.Text = Resource.Admin_OrderSearch_AddOrder;
                btnSaveBottom.Text = Resource.Admin_OrderSearch_AddOrder;
                cellPrint1.Visible = false;
                cellPrint2.Visible = false;
                cellPrint3.Visible = false;
                cellPrint4.Visible = false;
                lblOrderID.Text = Resource.Admin_OrderSearch_CreateNew;
                ddlBillingCountry.DataBind();
                ddlShippingCountry.DataBind();
                if (ddlShippingCountry.Items.FindByValue(SettingsMain.SalerCountryId.ToString()) != null)
                    ddlShippingCountry.SelectedValue = SettingsMain.SalerCountryId.ToString();//CountryService.GetCountryIdByIso3(Resource.Admin_Default_CountryISO3).ToString();
                if (ddlBillingCountry.Items.FindByValue(SettingsMain.SalerCountryId.ToString()) != null)
                    ddlBillingCountry.SelectedValue = SettingsMain.SalerCountryId.ToString();//CountryService.GetCountryIdByIso3(Resource.Admin_Default_CountryISO3).ToString();
                lblOrderStatus.Text = string.Format("({0})", OrderService.GetStatusName(OrderService.DefaultOrderStatus));
                lOrderDate.Text = AdvantShop.Localization.Culture.ConvertDate(DateTime.Now);
                lCustomerIP.Text = Request.UserHostAddress;

                chkCopyAddress.Checked = true;
                txtBillingAddress.Enabled = false;
                txtBillingCity.Enabled = false;
                txtBillingName.Enabled = false;
                txtBillingZip.Enabled = false;
                txtBillingZone.Enabled = false;
                ddlBillingCountry.Enabled = false;
                lblGroupDiscount.Text = "";

                List<PaymentMethod> listPayments = PaymentService.GetAllPaymentMethods(true).ToList();
                ddlPaymentMethod.DataSource = listPayments;
                ddlPaymentMethod.DataBind();
            }
            else
            {
                int id;
                if (!string.IsNullOrEmpty(Request["orderid"]) && int.TryParse(Request["orderid"], out id))
                {
                    OrderID = id;
                    LoadOrder();
                }
                else if (OrderID == 0)
                {
                    int ordId = OrderService.GetLastOrderId();
                    if (ordId == 0)
                    {
                        OrderID = 0;
                        pnOrder.Visible = false;
                        pnEmpty.Visible = true;
                    }
                    else
                    {
                        OrderID = ordId;
                        LoadOrder();
                    }
                }
            }
        }
        else
        {
            SetEnabled();
            CalendarExtender1.SelectedDate = null;
        }

        Page.Title = string.Format("{0} - {1} {2}", SettingsMain.ShopName, Resource.Admin_ViewOrder_ItemNum, OrderID);
    }

    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        if (PopupGridCustomers.SelectedCustomers != null && PopupGridCustomers.SelectedCustomers.Count > 0)
        {
            LoadCustomer(PopupGridCustomers.SelectedCustomers[0], null);
        }
    }

    protected void sds_Init(object sender, EventArgs e)
    {
        ((SqlDataSource)sender).ConnectionString = Connection.GetConnectionString();
    }

    private void LoadTotal()
    {
        Order ord = OrderService.GetOrder(OrderID);

        decimal currencyValue = orderItems.CurrencyValue;
        string currencyCode = orderItems.CurrencyCode;
        decimal orderDiscount = orderItems.OrderDiscount;
        string currencySymbol = orderItems.CurrencySymbol;
        bool isCodeBefore = orderItems.IsCodeBefore;
        var tempCur = CurrencyService.Currency(orderItems.CurrencyCode);


        lblCurrencySymbol.Text = tempCur != null ? CurrencyService.Currency(orderItems.CurrencyCode).Symbol : @"Get currency error";

        decimal shippingCost;
        
        if(decimal.TryParse(txtShippingPrice.Text, out shippingCost))
        {
            shippingCost = shippingCost*currencyValue;
            lblShippingPrice.Text = string.Format("+{0}", CatalogService.GetStringPrice(shippingCost, currencyValue, currencyCode));
        }
        else
        {
            lblShippingPrice.Text =string.Format("+{0}", CatalogService.GetStringPrice(0, currencyValue, currencyCode));
        }
        
        int region = RegionService.GetRegionIdByName(txtShippingZone.Text);
        var shippingContact = new CustomerContact
                                  {
                                      CountryId = int.Parse(ddlShippingCountry.SelectedValue),
                                      RegionId = region != 0 ? (int?)region : null
                                  };
        region = RegionService.GetRegionIdByName(txtBillingZone.Text);
        var billingContact = new CustomerContact
                                  {
                                      CountryId = int.Parse(ddlBillingCountry.SelectedValue),
                                      RegionId = region != 0 ? (int?)region : null
                                  };

        decimal taxCost = orderItems.OrderItems.Sum(oi => TaxServices.CalculateTaxesTotal(oi, shippingContact, billingContact));
        var taxedItems = new List<TaxValue>();

        foreach (OrderItem item in orderItems.OrderItems)
        {
            ICollection<TaxElement> t = TaxServices.GetTaxesForProduct(item.EntityId, billingContact, shippingContact);
            foreach (TaxElement tax in t)
            {
                TaxValue taxedItem = taxedItems.Find(tv => tv.TaxID == tax.TaxId);
                if (taxedItem != null)
                {
                    taxedItem.TaxSum += TaxServices.CalculateTax(item, tax);
                }
                else
                {
                    taxedItems.Add(new TaxValue
                                       {
                                           TaxID = tax.TaxId,
                                           TaxName = tax.Name,
                                           TaxShowInPrice = tax.ShowInPrice,
                                           TaxSum = TaxServices.CalculateTax(item, tax)
                                       });
                }
            }
        }
        literalTaxCost.Text = TaxServices.BuildTaxTable(taxedItems, currencyValue, currencyCode, Resource.Admin_ViewOrder_Taxes);

        decimal prodTotal = orderItems.OrderItems.Sum(oi => oi.Price * oi.Amount);
        lblTotalOrderPrice.Text = CatalogService.GetStringPrice(prodTotal, currencyValue, currencyCode);

        lblOrderDiscount.Text = string.Format("-{0}", CatalogService.GetStringDiscountPercent(prodTotal, orderDiscount,
                                                                                      currencyValue, currencySymbol, isCodeBefore,
                                                                                      CurrencyService.CurrentCurrency.PriceFormat, false));
        trDiscount.Visible = orderDiscount != 0;

        decimal totalDiscount = 0;
        totalDiscount += orderDiscount > 0 ? orderDiscount * prodTotal / 100 : 0;
        if (ord != null && ord.Certificate != null)
        {
            trCertificatePrice.Visible = ord.Certificate.Price != 0;
            lblCertificatePrice.Text = string.Format("-{0}", CatalogService.GetStringPrice(ord.Certificate.Price));
            totalDiscount += ord.Certificate.Price;
        }

        if (ord != null && totalDiscount == 0 && ord.Coupon != null)
        {
            trCoupon.Visible = ord.Coupon.Value != 0;
            switch (ord.Coupon.Type)
            {
                case CouponType.Fixed:
                    var productsPrice =
                        orderItems.OrderItems.Where(p => p.IsCouponApplied).Sum(p => p.Price * p.Amount);
                    totalDiscount += productsPrice >= ord.Coupon.Value ? ord.Coupon.Value : productsPrice;
                    lblCoupon.Text = String.Format("-{0} ({1})", CatalogService.GetStringPrice(totalDiscount),
                                                   ord.Coupon.Code);
                    break;
                case CouponType.Percent:
                    totalDiscount +=
                        orderItems.OrderItems.Where(p => p.IsCouponApplied).Sum(
                            p => ord.Coupon.Value * p.Price / 100 * p.Amount);
                    lblCoupon.Text = String.Format("-{0} ({1}%) ({2})", CatalogService.GetStringPrice(totalDiscount),
                                                   CatalogService.FormatPriceInvariant(ord.Coupon.Value),
                                                   ord.Coupon.Code);
                    break;
            }
        }

        decimal sum = taxCost + prodTotal + shippingCost - totalDiscount;
        lblTotalPrice.Text = CatalogService.GetStringPrice(sum < 0 ? 0 : sum, currencyValue, currencyCode);
        upItems.Update();
    }

    private void LoadOrder()
    {
        orderList.SelectedOrder = OrderID;

        hlExport.NavigateUrl = "../HttpHandlers/ExportOrderExcel.ashx?OrderID=" + OrderID;
        hlExport2.NavigateUrl = "../HttpHandlers/ExportOrderExcel.ashx?OrderID=" + OrderID;
        Order ord = OrderService.GetOrder(OrderID);
        if (ord != null)
        {
            lblOrderID.Text = ord.OrderCustomer != null
                                  ? string.Format("{0}{1} - {2} {3}", Resource.Admin_ViewOrder_ItemNum, OrderID,
                                                  ord.OrderCustomer.LastName, ord.OrderCustomer.FirstName)
                                  : string.Format("{0}{1}", Resource.Admin_ViewOrder_ItemNum, OrderID);

            lblGroupDiscount.Text = ord.GroupDiscountString;
            chkCopyAddress.Checked = ord.ShippingContactID == ord.BillingContactID;

            if (ord.OrderCustomer != null)
            {
                LoadCustomer(ord.OrderCustomer.CustomerID, ord.OrderCustomer);
                lCustomerIP.Text = ord.OrderCustomer.CustomerIP;
            }

            OrderNumber = ord.Number;
            Dictionary<string, string> statusesList = OrderService.GetOrderStatuses(true);
            if (statusesList != null && statusesList.ContainsKey(ord.OrderStatus.StatusName))
            {
                cboOrderStatus.SelectedValue = statusesList[ord.OrderStatus.StatusName];
            }
            else
            {
                cboOrderStatus.Items.Add(new ListItem(ord.OrderStatus.StatusName, ord.OrderStatus.StatusID.ToString()));
                cboOrderStatus.SelectedValue = ord.OrderStatus.StatusID.ToString();
            }

            lOrderDate.Text = AdvantShop.Localization.Culture.ConvertDate(ord.OrderDate);
            lNumber.Text = ord.Number;
            chkPayed.Checked = ord.PaymentDate != null;
            lblOrderStatus.Text = string.Format("({0})", ord.OrderStatus.StatusName);
            orderItems.SetCurrency(ord.OrderCurrency.CurrencyCode, ord.OrderCurrency.CurrencyValue, ord.OrderCurrency.CurrencyNumCode, ord.OrderCurrency.CurrencySymbol, ord.OrderCurrency.IsCodeBefore);
            orderItems.OrderDiscount = ord.OrderDiscount;
            orderItems.GroupDiscount = ord.GroupDiscount;
            hforderShipName.Value = ord.ArchivedShippingName;
            // Billing ------------------------

            var billingCustomerContact = new CustomerContact
                                             {
                                                 Name = ord.BillingContact.Name,
                                                 Address = ord.BillingContact.Address,
                                                 City = ord.BillingContact.City,
                                                 Country = ord.BillingContact.Country,
                                                 RegionName = ord.BillingContact.Zone,
                                                 Zip = ord.BillingContact.Zip,
                                                 CustomerGuid = ord.OrderCustomer.CustomerID
                                             };
            LoadBilling(billingCustomerContact);
            if (ord.Certificate != null)
            {
                lCertificateCode.Text = ord.Certificate.Code;
                pnlCertificateCode.Visible = !string.IsNullOrEmpty(ord.Certificate.Code);
            }

            if (ord.OrderCustomer != null)
            {
                hfBillingID.Value = (CustomerService.GetContactId(billingCustomerContact) ?? "New");
            }
            else
            {
                hfBillingID.Value = "New";
            }

            // Shipping ----------------------------------
            //TODO: deal with countries and contacts
            var shippingCustomerContact = new CustomerContact
                                              {
                                                  Name = ord.ShippingContact.Name,
                                                  Address = ord.ShippingContact.Address,
                                                  City = ord.ShippingContact.City,
                                                  Country = ord.ShippingContact.Country,
                                                  RegionName = ord.ShippingContact.Zone,
                                                  Zip = ord.ShippingContact.Zip,
                                                  CustomerGuid = ord.OrderCustomer == null ? Guid.Empty : ord.OrderCustomer.CustomerID
                                              };
            txtShippingMethod.Text = ord.ArchivedShippingName;
            hfOrderShippingId.Value = ord.ShippingMethodId.ToString();
            txtShippingPrice.Text = (ord.ShippingCost / ord.OrderCurrency.CurrencyValue).ToString("F2");

            if (ord.OrderPickPoint != null)
            {
                ltPickPointID.Text = ord.OrderPickPoint.PickPointId;
                ltPickPointAddress.Text = ord.OrderPickPoint.PickPointAddress;
            }
            LoadShipping(shippingCustomerContact);

            if (ord.OrderCustomer != null)
            {
                hfShippingID.Value = (CustomerService.GetContactId(shippingCustomerContact) ?? "New");
            }
            else
            {
                hfShippingID.Value = "New";
            }

            List<PaymentMethod> listPayments = PaymentService.GetAllPaymentMethods(true).ToList();
            //PaymentMethod cashMethod = new CashOnDelivery(null);

            //if (ord.PaymentMethodId == cashMethod.PaymentMethodID)
            //    listPayments.Add(cashMethod);

            //PaymentMethod pickPointMethod = new PickPoint();

            //if (ord.PaymentMethodId == pickPointMethod.PaymentMethodID)
            //    listPayments.Add(pickPointMethod);

            ddlPaymentMethod.DataSource = listPayments;
            ddlPaymentMethod.DataBind();

            // TODO сделать textbox
            ddlPaymentMethod.SelectedValue = ord.PaymentMethodId.ToString();

            //NOTE: Узкое место. проверить отображение в различных ситуациях
            if (ord.Payed)
            {
                ddlPaymentMethod.Visible = false;
                txtPaymentMethod.Text = ord.PaymentMethodName;
                txtPaymentMethod.Visible = true;
            }
            else
            {
                ddlPaymentMethod.Visible = true;
                if (ord.PaymentMethod != null)
                    txtPaymentMethod.Visible = false;
            }
            if (ord.PaymentMethod == null)
            {
                ddlPaymentMethod.Items.Insert(0, new ListItem(Resource.Admin_NotSet, "0"));
            }

            lblUserComment.Text = string.IsNullOrEmpty(ord.CustomerComment)
                                      ? Resource.Admin_OrderSearch_NoComment
                                      : ord.CustomerComment;
            txtAdminOrderComment.Text = string.Format("{0}", ord.AdminOrderComment);
            txtStatusComment.Text = string.Format("{0}", ord.StatusComment);


            PaperPaymentType pm = ord.PaymentMethod == null ? PaperPaymentType.NonPaperMethod : ord.PaymentMethod.Type.ToEnum<PaperPaymentType>();

            var printButtonText = new Dictionary<PaperPaymentType, string>
                                      {
                                          {PaperPaymentType.SberBank, Resource.Client_OrderConfirmation_PrintLuggage},
                                          {PaperPaymentType.Bill, Resource.Client_OrderConfirmation_PrintBill},
                                          {PaperPaymentType.Check, Resource.Client_OrderConfirmation_PrintCheck},
                                      };

            paymentDetails.Visible = false;
            btnPrintPaymentDetails.Visible = false;

            if (pm != PaperPaymentType.NonPaperMethod)
            {
                if (pm == PaperPaymentType.SberBank)
                {
                    LocalizeClient_OrderConfirmation_OrganizationName.Text = Resource.Admin_EditOrder_CustomerName;
                }
                printPaymentDetails.Visible = true;
                btnPrintPaymentDetails.Visible = true;
                btnPrintPaymentDetails.Value = printButtonText[pm];

                btnPrintPaymentDetails.Attributes.Add("onclick",
                                                      string.Format(
                                                          "javascript:open_printable_version(\'../Check_{0}.aspx?ordernumber={1}{2}",
                                                          pm, ord.Number,
                                                          pm != PaperPaymentType.Check
                                                              ? (string.Format(
                                                                  "&bill_CompanyName=\' + escape(document.getElementById(\'{0}\').value) + \'&bill_INN=\' + escape(document.getElementById(\'{1}\').value));",
                                                                  txtCompanyName.ClientID, txtINN.ClientID))
                                                              : "\');"));

                if (pm == PaperPaymentType.Bill || pm == PaperPaymentType.SberBank)
                {
                    paymentDetails.Visible = true;
                    btnPrintPaymentDetails.Visible = true;
                    if (ord.PaymentDetails != null)
                    {
                        txtCompanyName.Text = ord.PaymentDetails.CompanyName;
                        txtINN.Text = ord.PaymentDetails.INN;
                    }
                }
            }

            orderItems.OrderItems = (List<OrderItem>)ord.OrderItems;

            LoadTotal();


            pnEmpty.Visible = false;
            pnOrder.Visible = true;
        }
        else
        {
            Response.Redirect("OrderSearch.aspx");
            pnEmpty.Visible = true;
            lblNotFound.Text = @"Not found";
        }
        UpdatePanel1.Update();
    }

    private void LoadCustomer(Guid customerId, OrderCustomer orderCustomer)
    {
        hfContactID.Value = customerId.ToString();
        var customer = CustomerService.GetCustomer(customerId);

        if (customer.RegistredUser && customer.Id != CustomerService.InternetUserGuid)
        {
            hlCustomer.Text = string.Format("{0} {1}", customer.FirstName, customer.LastName);
            hlCustomer.NavigateUrl = "ViewCustomer.aspx?CustomerID=" + customerId;
            hlCustomer.Visible = true;

            lblGroupDiscount.Text = customer.CustomerGroup != null ? customer.CustomerGroup.GroupName : string.Empty;
            lblChosingCustomer.Visible = false;
        }
        else
        {
            lblCustomer.Text = string.Format("{0} {1} - {2} - {3}", orderCustomer.FirstName, orderCustomer.LastName, orderCustomer.Email, orderCustomer.MobilePhone);
            lblCustomer.Visible = true;
            lblChosingCustomer.Visible = false;
            lblChosingCustomer.Visible = false;
        }

        LoadContacts(customerId, false);
    }

    protected string RenderSelectedOptions(IList<EvaluatedCustomOptions> evlist)
    {
        var html = new StringBuilder();
        html.Append("<ul>");

        foreach (EvaluatedCustomOptions ev in evlist)
        {
            html.Append(string.Format("<li>{0}: {1}</li>", ev.CustomOptionTitle, ev.OptionTitle));
        }

        html.Append("</ul>");
        return html.ToString();
    }

    protected void SqlDataSource1_Init(object sender, EventArgs e)
    {
        SqlDataSource1.ConnectionString = Connection.GetConnectionString();
    }

    public string RenderDivHeader()
    {
        string divHeader;
        if (Request.Browser.Browser == "IE")
        {
            var c = new CultureInfo("en-us");
            divHeader = double.Parse(Request.Browser.Version, c.NumberFormat) < 7
                            ? "<div class=\'mtree_ie6\'>"
                            : "<div class=\'mtree_ie\'>";
        }
        else
        {
            divHeader = "<div class=\'mtree\'>";
        }
        return divHeader;
    }

    public string RenderDivBottom()
    {
        return "</div>";
    }

    protected void FillView(CustomerContact contact)
    {
        if (contact != null)
        {
            LoadBilling(contact);
            LoadShipping(contact);
        }
        else
        {
            CleanBilling();
            CleanShipping();
        }
    }

    private void LoadBilling(CustomerContact contact)
    {
        hfBillingID.Value = contact.CustomerContactID.ToString();
        txtBillingAddress.Text = HttpUtility.HtmlDecode(contact.Address);
        txtBillingCity.Text = HttpUtility.HtmlDecode(contact.City);
        txtBillingName.Text = HttpUtility.HtmlDecode(contact.Name);
        txtBillingZip.Text = HttpUtility.HtmlDecode(contact.Zip);
        ddlBillingCountry.DataBind();
        if (!string.IsNullOrEmpty(contact.Country))
        {
            ListItem temp = ddlBillingCountry.Items.FindByText(contact.Country);
            if (temp != null)
            {
                ddlBillingCountry.SelectedValue = temp.Value;
            }
            else
            {
                ddlBillingCountry.Items.Add(new ListItem(contact.Country, "0"));
                ddlBillingCountry.SelectedValue = "0";
            }
        }

        txtBillingZone.Text = HttpUtility.HtmlDecode(contact.RegionName);
        SetEnabled();
    }

    private void SetEnabled()
    {
        txtBillingAddress.Enabled = !chkCopyAddress.Checked;
        txtBillingCity.Enabled = !chkCopyAddress.Checked;
        txtBillingName.Enabled = !chkCopyAddress.Checked;
        txtBillingZip.Enabled = !chkCopyAddress.Checked;
        txtBillingZone.Enabled = !chkCopyAddress.Checked;
        ddlBillingCountry.Enabled = !chkCopyAddress.Checked;
    }

    private void CleanBilling()
    {
        hfBillingID.Value = string.Empty;
        txtBillingAddress.Text = string.Empty;
        txtBillingCity.Text = string.Empty;
        txtBillingName.Text = string.Empty;
        txtBillingZip.Text = string.Empty;
        if (ddlBillingCountry.Items.FindByValue(SettingsMain.SalerCountryId.ToString()) != null)
            ddlBillingCountry.SelectedValue = SettingsMain.SalerCountryId.ToString();//CountryService.GetCountryIdByIso3(Resource.Admin_Default_CountryISO3).ToString();
    }

    private void LoadShipping(CustomerContact contact)
    {
        hfShippingID.Value = contact.CustomerContactID.ToString();
        txtShippingAddress.Text = HttpUtility.HtmlDecode(contact.Address);
        txtShippingCity.Text = HttpUtility.HtmlDecode(contact.City);
        txtShippingName.Text = HttpUtility.HtmlDecode(contact.Name);
        txtShippingZip.Text = HttpUtility.HtmlDecode(contact.Zip);

        ddlShippingCountry.DataBind();
        if (!string.IsNullOrEmpty(contact.Country))
        {
            ListItem item = ddlShippingCountry.Items.FindByText(contact.Country);
            if (item != null)
            {
                ddlShippingCountry.SelectedValue = item.Value;
            }
            else
            {
                ddlShippingCountry.Items.Add(new ListItem(contact.Country, "0"));
                ddlShippingCountry.SelectedValue = "0";
            }
        }
        txtShippingZone.Text = HttpUtility.HtmlDecode(contact.RegionName);
    }

    private void CleanShipping()
    {
        hfShippingID.Value = string.Empty;
        txtShippingAddress.Text = string.Empty;
        txtShippingCity.Text = string.Empty;
        txtShippingName.Text = string.Empty;
        txtShippingZip.Text = string.Empty;
        if (ddlShippingCountry.Items.FindByValue(SettingsMain.SalerCountryId.ToString()) != null)
            ddlShippingCountry.SelectedValue = SettingsMain.SalerCountryId.ToString();//CountryService.GetCountryIdByIso3(Resource.Admin_Default_CountryISO3).ToString();

        txtShippingZone.Text = string.Empty;
    }

    protected bool CheckBillingAndShipping()
    {
        var error = new StringBuilder();
        if (!chkCopyAddress.Checked)
        {
            if (string.IsNullOrEmpty(txtBillingName.Text))
                error.AppendFormat("{0} <br />", Resource.Admin_ViewOrder_NoBillingName);
            if (string.IsNullOrEmpty(txtBillingCity.Text))
                error.AppendFormat("{0} <br />", Resource.Admin_ViewOrder_NoBillingCity);
            if (string.IsNullOrEmpty(txtBillingAddress.Text))
                error.AppendFormat("{0} <br />", Resource.Admin_ViewOrder_NoBillingAddress);
        }
        if (string.IsNullOrEmpty(txtShippingName.Text))
            error.AppendFormat("{0} <br />", Resource.Admin_ViewOrder_NoShippingName);
        if (string.IsNullOrEmpty(txtShippingCity.Text))
            error.AppendFormat("{0} <br />", Resource.Admin_ViewOrder_NoShippingCity);
        if (string.IsNullOrEmpty(txtShippingAddress.Text))
            error.AppendFormat("{0} <br />", Resource.Admin_ViewOrder_NoShippingAddress);
        if (error.Length <= 0)
        {
            return true;
        }
        msgErr(error.ToString());
        return false;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(hfContactID.Value))
        {
            msgErr(Resource.Admin_ViewOrder_NoUserError);
            return;
        }
        try
        {
            new Guid(hfContactID.Value);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
            msgErr(Resource.Admin_ViewOrder_NoUserError);
            return;
        }
        if (!CheckBillingAndShipping())
            return;


        try
        {
            Decimal.Parse(txtShippingPrice.Text);
        }
        catch (Exception ex)
        {
            //if error we don't save
            msgErr(Resource.Admin_OrderSearch_ErrorParseShipping);
            return;
        }

        int shipId = 0;
        if (!string.IsNullOrWhiteSpace(hfOrderShippingId.Value))
        {
            shipId = Convert.ToInt32(hfOrderShippingId.Value);
            var shippingMethod = ShippingMethodService.GetShippingMethod(shipId);
            if (shippingMethod == null && !AddingNewOrder)
            {
                msgErr(Resource.Admin_OrderSearch_SelectShippingMethod);
                return;
            }
        }
        else
        {
            msgErr(Resource.Admin_OrderSearch_SelectShippingMethod);
            return;
        }

        if (orderItems.OrderItems.Count == 0)
        {
            msgErr(Resource.Admin_EditOrder_NoOrderItems);
            return;
        }

        if (ddlPaymentMethod.SelectedValue == "0")
        {
            msgErr(Resource.Admin_EditOrder_SelectPayment);
            return;
        }

        // -- Order starting here
        bool shippingRefresh;
        Order order = BuildOrder(out shippingRefresh, shipId);
        if (order == null)
        {
            msgErr("Order ID invalid");
            return;
        }
        if (AddingNewOrder)
        {
            CreateOrder(order);
        }
        else
        {
            SaveOrder(order, shippingRefresh);
        }

        if (order.OrderCustomer.CustomerID != CustomerService.InternetUserGuid)
        {
            UpdateCustomerContacts();
        }

        if (AddingNewOrder)
        {
            Response.Redirect("EditOrder.aspx?OrderID=" + order.OrderID);
        }
        else
        {
            LoadOrder();
        }
    }

    private void CreateOrder(Order order)
    {
        Customer customer = CustomerService.GetCustomer(new Guid(hfContactID.Value));
        order.OrderCustomer = new OrderCustomer
                                  {
                                      CustomerID = new Guid(hfContactID.Value),
                                      CustomerIP = Request.UserHostAddress,
                                      FirstName = string.IsNullOrEmpty(customer.FirstName) ? order.BillingContact.Name : customer.FirstName,
                                      LastName = customer.LastName,
                                      Email = customer.EMail,
                                      MobilePhone = customer.Phone
                                  };
        order.GroupName = customer.CustomerGroup != null ? customer.CustomerGroup.GroupName : string.Empty;

        order.OrderStatusId = OrderService.DefaultOrderStatus;
        order.OrderDate = DateTime.Now;
        order.AffiliateID = 0;
        order.Number = OrderService.GenerateNumber(1); // For crash protection
        order.OrderID = OrderService.AddOrder(order);
        if (order.OrderID == 0)
        {
            msgErr(Resource.Admin_ViewOrder_CreateError);
            return;
        }
        order.Number = OrderService.GenerateNumber(order.OrderID); // new number
        OrderService.UpdateNumber(order.OrderID, order.Number);
        OrderID = order.OrderID;
        SaveOrderCart(order.OrderID);

        ModulesRenderer.OrderAdded(order.OrderID);


        //int region;
        //var shippingContact = new CustomerContact
        //                          {
        //                              CountryId = int.Parse(ddlShippingCountry.SelectedValue),
        //                              RegionId =
        //                                  int.TryParse(ddlShippingZone.SelectedValue, out region)
        //                                      ? new int?(region)
        //                                      : null
        //                          };
        //var billingContact = new CustomerContact
        //                         {
        //                             CountryId = int.Parse(ddlBillingCountry.SelectedValue),
        //                             RegionId =
        //                                 int.TryParse(ddlBillingZone.SelectedValue, out region)
        //                                     ? new int?(region)
        //                                     : null
        //                         };



        var clsParam = new ClsMailParamOnNewOrder
                           {
                               CurrentCurrencyCode = order.OrderCurrency.CurrencyCode,
                               OrderID = order.OrderID.ToString(),
                               Email = order.OrderCustomer.Email,
                               Number = order.Number,
                               ShippingMethod = order.ShippingMethodName,
                               PaymentType = order.PaymentMethodName,
                               TotalPrice = order.Sum.ToString(),
                               Comments = order.CustomerComment,
                               CustomerContacts = BuildCustomerContacts(customer, order.ShippingContact),
                               //OrderTable = OrderService.GenerateHtmlOrderTable(order.OrderItems, order.Sum, order.OrderDiscount, order.ShippingCost, 0, null, order.TaxCost, billingContact,
                               //                         shippingContact)
                           };

        string htmlMessage = SendMail.BuildMail(clsParam);

        SendMail.SendMailNow(customer.EMail, Resource.Client_OrderConfirmation_ReceivedOrder + " " + order.OrderID,
                             htmlMessage, true);
        SendMail.SendMailNow(SettingsMail.EmailForOrders,
                             Resource.Client_OrderConfirmation_ReceivedOrder + " " + order.OrderID, htmlMessage,
                             true);
    }

    private static string BuildCustomerContacts(Customer customer, OrderContact contact)
    {
        var customerSb1 = new StringBuilder();

        customerSb1.AppendFormat(Resource.Client_Registration_Name + " {0}<br/>", customer.FirstName);
        customerSb1.AppendFormat(Resource.Client_Registration_Surname + " {0}<br/>", customer.LastName);
        customerSb1.AppendFormat(Resource.Client_Registration_Country + " {0}<br/>", contact.Country);
        customerSb1.AppendFormat(Resource.Client_Registration_State + " {0}<br/>", contact.Zone);
        customerSb1.AppendFormat(Resource.Client_Registration_City + " {0}<br/>", contact.City);
        customerSb1.AppendFormat(Resource.Client_Registration_Zip + " {0}<br/>", contact.Zip);
        customerSb1.AppendFormat(Resource.Client_Registration_Address + ": {0}<br/>", string.IsNullOrEmpty(contact.Address)
                                     ? Resource.Client_OrderConfirmation_NotDefined
                                     : contact.Address);
        return customerSb1.ToString();
    }


    private static Dictionary<TaxElement, decimal> GetTaxItems(IEnumerable<OrderItem> items, CustomerContact shippingContact, CustomerContact billingContact)
    {
        var taxesItems = new Dictionary<TaxElement, decimal>();

        foreach (OrderItem item in items)
        {
            var t = (List<TaxElement>)TaxServices.GetTaxesForProduct(item.EntityId, billingContact, shippingContact);
            foreach (TaxElement tax in t)
            {
                if (taxesItems.ContainsKey(tax))
                {
                    taxesItems[tax] += TaxServices.CalculateTax(item, tax);
                }
                else
                {
                    taxesItems.Add(tax, TaxServices.CalculateTax(item, tax));
                }
            }
        }

        return taxesItems;
    }

    private void SaveOrder(Order order, bool shippingRefresh)
    {
        order.OrderID = Convert.ToInt32(OrderID);
        order.Number = OrderNumber;
        bool needSendMessage = order.OrderStatusId != Convert.ToInt32(cboOrderStatus.SelectedValue);

        var oldOrderStatusId = order.OrderStatusId;

        order.OrderStatusId = Convert.ToInt32(cboOrderStatus.SelectedValue);
        // -- Save main info
        OrderService.UpdateOrderMain(order);

        if (chkPayed.Checked != order.Payed)
        {
            OrderService.PayOrder(order.OrderID, chkPayed.Checked);
        }

        try
        {
            lblInfo.Text = string.Empty;
            OrderService.ChangeOrderStatus(order.OrderID, int.Parse(cboOrderStatus.SelectedValue));

            if (chkSendConfirmation.Checked && needSendMessage)
            {
                SendMail.SendMailNow(
                    order.OrderCustomer.Email,
                    Resource.Admin_ViewOrder_OrderhasbeenChanged,
                    SendMail.BuildMail(new ClsMailParamOnChangeOrderStatus
                                           {
                                               OrderID = OrderID.ToString(),
                                               OrderStatus = cboOrderStatus.SelectedItem.Text,
                                               StatusComment = order.StatusComment,
                                               Number = order.Number
                                           }),
                    true);
                lblInfo.Text += string.Format("<br/><br/>" + Resource.Admin_ViewOrder_ToEmail + " \'{0}\' " + Resource.Admin_ViewOrder_NotifySent, order.OrderCustomer.Email);
            }
        }
        catch (Exception ex)
        {
            msgErr(ex.Message + " at ChangeStatus");
            Debug.LogError(ex);
        }

        OrderID = order.OrderID;
        // -- Order contacts

        OrderService.UpdateOrderContacts(order.OrderID, order.ShippingContact, order.BillingContact);
        // -- Order currency
        OrderService.UpdateOrderCurrency(order.OrderID, order.OrderCurrency.CurrencyCode, order.OrderCurrency.CurrencyValue);

        // -- Ordered items

        shippingRefresh |= orderItems.OrderItems.AggregateHash() != order.OrderItems.AggregateHash();

        SaveOrderCart(order.OrderID, order.OrderItems);

        if (oldOrderStatusId != order.OrderStatusId)
        {
            ModulesRenderer.OrderChangeStatus(OrderID);
        }
        ModulesRenderer.OrderUpdated(OrderID);

        if (shippingRefresh)
            modalRecheckShipping.Show();
    }

    private void UpdateCustomerContacts()
    {
        // update shipping
        if (hfShippingID.Value != string.Empty)
        {
            var contact = new CustomerContact
                              {
                                  Address = HttpUtility.HtmlEncode(txtShippingAddress.Text),
                                  City = HttpUtility.HtmlEncode(txtShippingCity.Text),
                                  Name = HttpUtility.HtmlEncode(txtShippingName.Text),
                                  Zip = HttpUtility.HtmlEncode(txtShippingZip.Text),
                                  CountryId = Convert.ToInt32(ddlShippingCountry.SelectedValue),
                                  Country = ddlShippingCountry.SelectedItem.Text,
                                  RegionName = HttpUtility.HtmlEncode(txtShippingZone.Text)
                              };


            int regionid = RegionService.GetRegionIdByName(txtShippingZone.Text);
            contact.RegionId = regionid != 0 ? (int?)regionid : null;
            contact.CustomerGuid = new Guid(hfContactID.Value);

            if (CustomerService.ExistsCustomer(contact.CustomerGuid))
            {
                if (hfShippingID.Value != "New")
                {
                    //update contact
                    contact.CustomerContactID = new Guid(hfShippingID.Value);

                    //If contact isnot in contactbook
                    if (CustomerService.GetContactId(contact) != contact.CustomerContactID.ToString())
                        CustomerService.UpdateContact(contact);
                }
                else
                {
                    //If contact isnot in contactbook
                    if (CustomerService.GetContactId(contact) == null)
                        CustomerService.AddContact(contact, contact.CustomerGuid);
                }
            }
        }

        // update contact by billing address
        if (!chkCopyAddress.Checked)
        {
            if (hfBillingID.Value != string.Empty)
            {
                var contact = new CustomerContact
                                  {
                                      Address = HttpUtility.HtmlEncode(txtBillingAddress.Text),
                                      City = HttpUtility.HtmlEncode(txtBillingCity.Text),
                                      Name = HttpUtility.HtmlEncode(txtBillingName.Text),
                                      Zip = HttpUtility.HtmlEncode(txtBillingZip.Text),
                                      CountryId = Convert.ToInt32(ddlBillingCountry.SelectedValue),
                                      Country = ddlBillingCountry.SelectedItem.Text,
                                      RegionName = HttpUtility.HtmlEncode(txtBillingZone.Text)
                                  };

                int regionId = RegionService.GetRegionIdByName(txtBillingZone.Text);
                contact.RegionId = regionId != 0 ? (int?)regionId : null;
                contact.CustomerGuid = new Guid(hfContactID.Value);

                if (hfBillingID.Value != "New" && hfShippingID.Value != hfBillingID.Value)
                {
                    //update contact
                    contact.CustomerContactID = new Guid(hfBillingID.Value);

                    //If contact isnot in contactbook
                    if (CustomerService.GetContactId(contact) != contact.CustomerContactID.ToString())
                        CustomerService.UpdateContact(contact);
                }
                else
                {
                    //If contact isnot in contactbook
                    if (CustomerService.GetContactId(contact) == null)
                        CustomerService.AddContact(contact, contact.CustomerGuid);
                }
            }
        }
    }

    private Order BuildOrder(out bool shippingRefresh, int shippingMethodId)
    {

        Order order = AddingNewOrder ? new Order() : OrderService.GetOrder(OrderID);
        order.PaymentMethodId = Convert.ToInt32(ddlPaymentMethod.SelectedValue);
        order.ShippingMethodId = shippingMethodId;
        order.AdminOrderComment = txtAdminOrderComment.Text;
        order.StatusComment = txtStatusComment.Text;
        //order.ShippingCost = txtShippingPrice.Text.TryParseDecimal() * orderItems.CurrencyValue;
        order.ShippingCost = txtShippingPrice.Text.Replace(",", ".").TryParseDecimal() * orderItems.CurrencyValue;
        order.ArchivedShippingName = txtShippingMethod.Text;
        if (!string.IsNullOrEmpty(ltPickPointID.Text) || !string.IsNullOrEmpty(ltPickPointAddress.Text))
            order.OrderPickPoint = new OrderPickPoint { PickPointId = ltPickPointID.Text, PickPointAddress = ltPickPointAddress.Text };

        order.OrderCurrency = new OrderCurrency
                                  {
                                      CurrencyCode = orderItems.CurrencyCode,
                                      CurrencyValue = orderItems.CurrencyValue,
                                      CurrencyNumCode = orderItems.CurrencyNumCode,
                                      CurrencySymbol = orderItems.CurrencySymbol
                                  };
        order.OrderDiscount = orderItems.OrderDiscount;

        var shippingContact = new OrderContact
                                  {
                                      Address = HttpUtility.HtmlEncode(txtShippingAddress.Text),
                                      City = HttpUtility.HtmlEncode(txtShippingCity.Text),
                                      Country = ddlShippingCountry.SelectedItem.Text,
                                      Name = HttpUtility.HtmlEncode(txtShippingName.Text),
                                      Zip = HttpUtility.HtmlEncode(txtShippingZip.Text),
                                      Zone = HttpUtility.HtmlEncode(txtShippingZone.Text),
                                  };
        shippingRefresh = !AddingNewOrder && !ContactChanged(order.ShippingContact, shippingContact);

        order.ShippingContact = shippingContact;
        order.BillingContact = chkCopyAddress.Checked
                                   ? order.ShippingContact
                                   : new OrderContact
                                         {
                                             Address = HttpUtility.HtmlEncode(txtBillingAddress.Text),
                                             City = HttpUtility.HtmlEncode(txtBillingCity.Text),
                                             Country = ddlBillingCountry.SelectedItem.Text,
                                             Name = HttpUtility.HtmlEncode(txtBillingName.Text),
                                             Zip = HttpUtility.HtmlEncode(txtBillingZip.Text),
                                             Zone = HttpUtility.HtmlEncode(txtBillingZone.Text)
                                         };

        order.OrderDate = GetDate();

        Guid customerId = Guid.Empty;
        if (order.OrderCustomer != null)
        {
            customerId = order.OrderCustomer.CustomerID;
        }
        orderItems.SetCustomerDiscount(customerId);

        return order;
    }

    private bool ContactChanged(OrderContact shippingContact, OrderContact orderContact)
    {
        return shippingContact.Country == orderContact.Country
               && shippingContact.City == orderContact.City
               && shippingContact.Zip == orderContact.Zip;
    }

    protected void agv_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "SelectContact")
        {
            string[] values = ((string)e.CommandArgument).Split('^');
            SelectCustomer(values[0].TryParseGuid(), values[1], true);
        }
    }

    protected void SelectCustomer(Guid customerID, string customerEmail, bool fillView)
    {
        hfContactID.Value = customerID.ToString();
        lblChosingCustomer.Text = customerEmail;
        LoadContacts(customerID, fillView);
    }

    private void LoadContacts(Guid customerID, bool fillView)
    {
        List<CustomerContact> contacts = CustomerService.GetCustomerContacts(customerID);
        if (fillView)
        {
            FillView(contacts.Count > 0 ? contacts.First() : null);
        }

        if (contacts.Count == 0)
        {
            ErrMes.Text = string.Empty;
            ErrMes.Visible = true;
        }

        CustomerContacts.Items.Clear();
        var liNew = new ListItem
                        {
                            Value = "New",
                            Text = Resource.Admin_OrderSearch_NewAddress
                        };

        CustomerContacts.Items.Add(liNew);
        const string format = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>{0}</b>&nbsp;{1}<br />";
        foreach (var customerRow in contacts)
        {
            var liText = new StringBuilder();
            liText.AppendFormat(
                "&nbsp;<b>{0}:</b>&nbsp;{1}<br />",
                Resource.Admin_ViewCustomer_ContactPerson, customerRow.Name);
            liText.AppendFormat(format,
                                Resource.Admin_ViewCustomer_ContactCountry, customerRow.Country);
            liText.AppendFormat(format,
                                Resource.Admin_ViewCustomer_ContactCity, customerRow.City);

            if (!string.IsNullOrEmpty(customerRow.RegionName.Trim()))
            {
                liText.AppendFormat(format,
                                    Resource.Admin_ViewCustomer_ContactZone, customerRow.RegionName);
            }

            if (!string.IsNullOrEmpty(customerRow.Zip.Trim()))
            {
                liText.AppendFormat(format,
                                    Resource.Admin_ViewCustomer_ContactZip, customerRow.Zip);
            }
            liText.AppendFormat(format,
                                Resource.Admin_ViewCustomer_ContactAddress, customerRow.Address);

            var li = new ListItem { Text = liText.ToString(), Value = customerRow.CustomerContactID.ToString() };
            CustomerContacts.Items.Add(li);
        }

        var customer = CustomerService.GetCustomer(customerID);
        var group = CustomerGroupService.GetCustomerGroup(customer.CustomerGroupId);

        // Апдейтим имя группы и скидку группы
        Order order = OrderService.GetOrder(OrderID);
        if (order != null)
        {
            order.GroupName = group.GroupName;
            order.GroupDiscount = group.GroupDiscount;
            OrderService.UpdateOrderMain(order);
        }
        orderItems.SetCustomerDiscount(customerID);

        UpdatePanel4.Update();
    }

    private void msgErr_createUser(string messageText)
    {
        pnlMsgErr.Visible = true;
        Message.Text = "<br/>" + messageText;
    }

    private void msgErr(string messageText)
    {
        pnlMsgErr.Visible = true;
        MsgErr.Text = "<br/>" + messageText;
    }

    protected void btnCreateUser_Click(object sender, EventArgs e)
    {
        if (ValidateCustomer())
        {
            Guid customerID = CustomerService.InsertNewCustomer(new Customer
                                                                    {
                                                                        CustomerGroupId = 1,
                                                                        Password = txtPassword.Text,
                                                                        FirstName = txtFirstName.Text,
                                                                        LastName = txtLastName.Text,
                                                                        Phone = txtPhone.Text,
                                                                        SubscribedForNews = chkSubscribed4News.Checked,
                                                                        EMail = txtEmail.Text.Trim(),
                                                                        CustomerRole = Role.User
                                                                    });
            if (!customerID.Equals(Guid.Empty))
            {
                ClearCustomer();
                Customer customer = CustomerService.GetCustomer(customerID);
                SelectCustomer(customer.Id, customer.EMail, true);

                ModalPopupExtender2.Hide();
                UpdatePanel1.Update();

                // Bind user grid
                hfBillingID.Value = "New";
                hfShippingID.Value = "New";
            }
            else
            {
                msgErr_createUser(Resource.Admin_ViewOrder_UserCreateError);
                //bad thing happens. notify user about this
            }
        }
        else
        {
            msgErr_createUser(Resource.Admin_ViewOrder_PwdConfirmError);
        }
    }

    private void ClearCustomer()
    {
        txtEmail.Text = string.Empty;
        txtPassword.Text = string.Empty;
        txtPasswordConfirm.Text = string.Empty;
        txtFirstName.Text = string.Empty;
        txtLastName.Text = string.Empty;
        txtPhone.Text = string.Empty;
        chkSubscribed4News.Checked = false;
    }

    private bool ValidateCustomer()
    {
        bool boolIsValidPast = true;

        ulUserRegistarionValidation.InnerHtml = "";

        // ------------------------------------------------------

        string email = txtEmail.Text.Trim();
        var r = new Regex("\\w+([-+.\']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*", RegexOptions.Multiline);

        if ((!string.IsNullOrEmpty(email)) && (r.IsMatch(email)) && (!CustomerService.ExistsEmail(email)))
        {
            txtEmail.CssClass = "OrderConfirmation_ValidTextBox";
        }
        else
        {
            txtEmail.CssClass = "OrderConfirmation_InvalidTextBox";
            boolIsValidPast = false;
        }

        if (!string.IsNullOrEmpty(txtPassword.Text) && txtPassword.Text.Length > 3)
        {
            txtPassword.CssClass = "OrderConfirmation_ValidTextBox";
        }
        else
        {
            txtPassword.CssClass = "OrderConfirmation_InvalidTextBox";
            boolIsValidPast = false;
        }

        if (string.IsNullOrEmpty(txtPasswordConfirm.Text) == false)
        {
            txtPasswordConfirm.CssClass = "OrderConfirmation_ValidTextBox";
        }
        else
        {
            txtPasswordConfirm.CssClass = "OrderConfirmation_InvalidTextBox";
            boolIsValidPast = false;
        }

        if ((string.IsNullOrEmpty(txtPasswordConfirm.Text) == false) &&
            (string.IsNullOrEmpty(txtPassword.Text) == false) && (txtPassword.Text == txtPasswordConfirm.Text))
        {
            txtPassword.CssClass = "OrderConfirmation_ValidTextBox";
            txtPasswordConfirm.CssClass = "OrderConfirmation_ValidTextBox";
        }
        else
        {
            txtPassword.CssClass = "OrderConfirmation_InvalidTextBox";
            txtPasswordConfirm.CssClass = "OrderConfirmation_InvalidTextBox";
            boolIsValidPast = false;
        }

        if (string.IsNullOrEmpty(txtFirstName.Text) == false)
        {
            txtFirstName.CssClass = "OrderConfirmation_ValidTextBox";
        }
        else
        {
            txtFirstName.CssClass = "OrderConfirmation_InvalidTextBox";
            boolIsValidPast = false;
        }

        if (string.IsNullOrEmpty(txtLastName.Text) == false)
        {
            txtLastName.CssClass = "OrderConfirmation_ValidTextBox";
        }
        else
        {
            txtLastName.CssClass = "OrderConfirmation_InvalidTextBox";
            boolIsValidPast = false;
        }

        // ------------------------------------------------------

        if (!boolIsValidPast)
        {
            ulUserRegistarionValidation.Visible = true;
            ulUserRegistarionValidation.InnerHtml += string.Format("<li>{0}</li>", Resource.Client_OrderConfirmation_EnterEmptyField);
        }
        else
            ulUserRegistarionValidation.Visible = false;
        return boolIsValidPast;
    }

    protected void btnSelectAddress_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(CustomerContacts.SelectedValue))
        {
            switch (hfTypeBindAddress.Value)
            {
                case "billing":
                    chkCopyAddress.Checked = false;
                    if (CustomerContacts.SelectedValue != "New")
                    {
                        LoadBilling(CustomerService.GetCustomerContact(CustomerContacts.SelectedValue));
                    }
                    else
                    {
                        CleanBilling();
                        hfBillingID.Value = "New";
                    }
                    break;
                case "shipping":
                    if (CustomerContacts.SelectedValue != "New")
                    {
                        LoadShipping(CustomerService.GetCustomerContact(CustomerContacts.SelectedValue));
                        if (chkCopyAddress.Checked)
                        {
                            LoadBilling(CustomerService.GetCustomerContact(CustomerContacts.SelectedValue));
                        }
                    }
                    else
                    {
                        CleanShipping();
                        hfShippingID.Value = "New";
                    }
                    break;
            }
            UpdatePanel1.Update();
        }
    }

    protected void SaveOrderCart(int orderId)
    {
        OrderService.AddUpdateOrderItems(orderItems.OrderItems, orderId);
    }

    protected void SaveOrderCart(int orderId, IList<OrderItem> oldItems)
    {
        OrderService.AddUpdateOrderItems(orderItems.OrderItems, oldItems, orderId);
    }

    protected void orderItems_Updated(object sender, EventArgs args)
    {
        LoadTotal();
        txtShippingPrice.Text = (Convert.ToDecimal(txtShippingPrice.Text) / (orderItems.CurrencyValue / orderItems.OldCurrencyValue)).ToString("F2");
    }

    private DateTime GetDate()
    {
        DateTime d;
        if (DateTime.TryParse(lOrderDate.Text, out d))
        {
            return new DateTime(d.Year, d.Month, d.Day, d.TimeOfDay.Hours, d.TimeOfDay.Minutes, d.TimeOfDay.Seconds);
        }

        return DateTime.Now;
    }

    protected void btnSelectShipping_Click(object sender, EventArgs e)
    {
        if (ShippingRates.SelectedID.IsNotEmpty())
        {
            hfOrderShippingId.Value = ShippingRates.SelectedMethodID.ToString();
            decimal rate = ShippingRates.SelectedRate / orderItems.CurrencyValue;
            txtShippingPrice.Text = rate.ToString("F2");
            txtShippingMethod.Text = ShippingRates.SelectedName;
            if (ShippingRates.SelectShippingOptionEx != null)
            {
                ltPickPointID.Text = ShippingRates.SelectShippingOptionEx.PickpointId;
                ltPickPointAddress.Text = ShippingRates.SelectShippingOptionEx.PickpointAddress;
            }
            LoadTotal();
            modalShipping.Hide();
        }
    }

    protected void lbChangeShipping_Click(object sender, EventArgs e)
    {
        ShippingRates.CountryId = Convert.ToInt32(ddlShippingCountry.SelectedValue);
        ShippingRates.Region = txtShippingZone.Text;
        ShippingRates.City = txtShippingCity.Text;
        ShippingRates.Zip = txtShippingZip.Text;
        ShippingRates.Currency = orderItems.Currency;
        

        var shoppingCart = new ShoppingCart();

        if ((orderItems.OrderItems != null) && (orderItems.OrderItems.Count != 0))
        {
            foreach (OrderItem item in orderItems.OrderItems)
            {
                {
                    shoppingCart.Add(new ShoppingCartItem
                                         {
                                             EntityId = item.EntityId,
                                             Amount = item.Amount,
                                             ItemType = item.ItemType
                                         });
                }
            }
        }

        ShippingRates.ShoppingCart = shoppingCart;
        ShippingRates.LoadMethods();
        modalShipping.Show();
    }
}