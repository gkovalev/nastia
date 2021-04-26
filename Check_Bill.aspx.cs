//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.itmcompany.ru
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using AdvantShop.Orders;
using AdvantShop.Payment;
using AdvantShop.Taxes;
using Resources;

// TODO REWRITE
public partial class Check_Bill : Page
{
    private Bill _bill;
    private Order _order;

    protected bool EmptyCheck
    {
        get { return OrderNumber == null || Bill == null; }
    }

    protected string OrderNumber
    {
        get { return Request["ordernumber"]; }
    }

    protected Order Order
    {
        get { return _order ?? (_order = OrderService.GetOrderByNumber(OrderNumber)); }
    }

   
    protected Bill Bill
    {
        get
        {
            if (_bill != null)
                return _bill;

            if (Order.PaymentMethodId == 0)
                return null;
            PaymentMethod method = PaymentService.GetPaymentMethod(Order.PaymentMethodId);
            if (!(method is Bill))
                return null;
            _bill = (Bill)method;
            return _bill;
        }
    }

    protected decimal ShippingPrice { get; set; }

    //protected int OrderId;
    //protected decimal ShippingPrice;
    //protected Bill2_PaymentModule bill2;

    protected override void InitializeCulture()
    {
        AdvantShop.Localization.Culture.InitializeCulture();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (EmptyCheck || Order == null)
            return;

        string company = HttpUtility.UrlDecode(Request["bill_companyname"]);
        string inn = HttpUtility.UrlDecode(Request["bill_inn"]);

        lblOrderID.Text = Order.OrderID.ToString();

        lblcompanyname.Text = Bill.CompanyName; // bill2.CompanyName;
        lTransactAccount.Text = Bill.TransAccount; // bill2.PayBill;
        lblinn.Text = Bill.INN;
        lblkpp.Text = Bill.KPP;
        lblbank.Text = Bill.BankName;
        lCorrespondentAccount.Text = Bill.CorAccount;
        lblbik.Text = Bill.BIK;

        lblProvider.Text = string.Format("ИНН {0} КПП {1} {2} {3} {4}", Bill.INN, Bill.KPP, Bill.CompanyName,
                                         Bill.Address,
                                         ((string.IsNullOrEmpty(Bill.Telephone)) ? "" : ", тел. " + Bill.Telephone));


        lblDirector.Text = (string.IsNullOrEmpty(Bill.Director)) ? "______________________" : Bill.Director;
        lblAccountant.Text = (string.IsNullOrEmpty(Bill.Accountant)) ? "______________________" : Bill.Accountant;
        lblManager.Text = (string.IsNullOrEmpty(Bill.Manager)) ? "______________________" : Bill.Manager;

        lblDateTime.Text = (Order.OrderDate).ToString("dd.MM.yy");

        string userAddress = Order.BillingContact.Address;

        lblBuyer.Text = string.Format("{0}{1} {2}", (string.IsNullOrEmpty(inn)) ? "" : ("ИНН " + inn + " "), company,
                                      userAddress);

        ShippingPrice = Order.ShippingCost / Order.OrderCurrency.CurrencyValue;
        rptOrderItems.DataBind();
        trShipping.DataBind();
        decimal priceSum = Order.OrderItems.Sum(oi => oi.Price * oi.Amount);
        decimal discount = Order.TotalDiscount;

        lbltotalprice.Text = priceSum.ToString("##,##0.00") + ' ' + @"руб.";
        lTotalDiscount.Text = discount.ToString("##,##0.00") + ' ' + @"руб.";
        lbltotalpricetopay.Text = Order.Sum.ToString("##,##0.00") + ' ' + @"руб.";
        lbltotalprice2.Text = lbltotalpricetopay.Text;

        //NOTE using RUR - unstable!!!!
        literalTaxCost.Text = BuildTaxTable(TaxServices.GetOrderTaxes(Order.OrderID).Where(p => p.TaxSum > 0).ToList());
        
        var intPart = (int)(Math.Floor(Order.Sum));

        int floatPart = Order.Sum != 0 ? Convert.ToInt32(Math.Round(Order.Sum - Math.Floor(Order.Sum), 2) * 100) : 0;

        string script = "<script>num2str(\'" + intPart + "\', \'str\');</script>";
        switch (floatPart % 10)
        {
            case 1:
                lbltotalkop.Text = floatPart.ToString("0#") + @" копейка";
                break;
            case 2:
            case 3:
            case 4:
                lbltotalkop.Text = floatPart.ToString("0#") + @" копейки";
                break;
            default:
                lbltotalkop.Text = floatPart.ToString("0#") + @" копеек";
                break;
        }

        ClientScript.RegisterStartupScript(typeof(String), "A", script);
    }

    private static string BuildTaxTable(ICollection<TaxValue> taxes)
    {
        var sb = new StringBuilder();
        if (taxes.Count != 0)
        {
            foreach (var tax in taxes)
            {
                sb.Append("<tr bgcolor=\"white\" runat=\"server\" id=\"rowNDS\"><td align=\"right\" width=\"82%\"><font class=\"sc\">");
                sb.Append((tax.TaxShowInPrice ? Resource.Core_TaxServices_Include_Tax : "") + " " + tax.TaxName);
                sb.Append(":</font></td><td align=\"right\" width=\"18%\"><font class=\"sc\"><b>");
                sb.Append(Math.Round(tax.TaxSum, 2).ToString("##,##0.00") + ' ' + "руб.");
                sb.Append("</b></font></td></tr>");
            }
        }
        else
        {
            sb.Append("<tr bgcolor=\"white\" runat=\"server\" id=\"rowNDS\"><td align=\"right\" width=\"82%\"><font class=\"sc\">");
            sb.Append(Resource.Client_Bill2_NDSAlreadyIncluded);
            sb.Append("</font></td><td align=\"right\" width=\"18%\"><font class=\"sc\"><b>");
            sb.Append("Без НДС");
            sb.Append("</b></font></td></tr>");
        }

        return sb.ToString();
    }
}