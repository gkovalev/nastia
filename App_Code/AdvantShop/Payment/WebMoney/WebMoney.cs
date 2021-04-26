//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using AdvantShop.Orders;

namespace AdvantShop.Payment
{
    /// <summary>
    /// Summary description for WebMiney
    /// </summary>
    public class WebMoney : PaymentMethod
    {
        public string Purse { get; set; }
        public string WmID { get; set; }
        public string SecretKey { get; set; }
        public decimal CurrencyValue { get; set; }

        //public string ResultUrl { get; set; }
        //public string SuccessUrl { get; set; }
        //public FormMethod SuccessUrlMethod { get; set; }

        ////public string FailUrl { get; set; }
        //public FormMethod FailUrlMethod { get; set; }

        public override UrlStatus ShowUrls
        {
            get
            {
                return UrlStatus.NotificationUrl | UrlStatus.ReturnUrl | UrlStatus.FailUrl;
            }
        }
        public override PaymentType Type
        {
            get { return PaymentType.WebMoney; }
        }
        public override ProcessType ProcessType
        {
            get { return ProcessType.FormPost; }
        }
        public override NotificationType NotificationType
        {
            get { return NotificationType.ReturnUrl; }
        }
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>
                           {
                               {WebMoneyTemplate.CurrencyValue, CurrencyValue.ToString()},
                               {WebMoneyTemplate.Purse, Purse},
                               {WebMoneyTemplate.WmID, WmID},
                               {WebMoneyTemplate.SecretKey, SecretKey},
                               //{WebMoneyTemplate.ResultUrl, ResultUrl},
                               //{WebMoneyTemplate.SuccessUrl, SuccessUrl},
                               //{WebMoneyTemplate.SuccessUrlMethod, SuccessUrlMethod.ToString()},
                               //{WebMoneyTemplate.FailUrl, FailUrl},
                               //{WebMoneyTemplate.FailUrlMethod, FailUrlMethod.ToString()}
                           };
            }
            set
            {
                Purse = value.ElementOrDefault(WebMoneyTemplate.Purse);
                WmID = value.ElementOrDefault(WebMoneyTemplate.WmID);
                SecretKey = value.ElementOrDefault(WebMoneyTemplate.SecretKey);
                //ResultUrl = value.ContainsKey(WebMoneyTemplate.ResultUrl) ? value[WebMoneyTemplate.ResultUrl] : "";
                //FailUrl = value.ContainsKey(WebMoneyTemplate.FailUrl) ? value[WebMoneyTemplate.FailUrl] : "";
                //SuccessUrl = value.ContainsKey(WebMoneyTemplate.SuccessUrl) ? value[WebMoneyTemplate.SuccessUrl] : "";
                decimal decVal;
                CurrencyValue = value.ContainsKey(WebMoneyTemplate.CurrencyValue) &&
                                decimal.TryParse(value[WebMoneyTemplate.CurrencyValue], out decVal)
                                    ? decVal
                                    : 1;
                //SuccessUrlMethod = value.ContainsKey(WebMoneyTemplate.SuccessUrlMethod)
                //                       ? (FormMethod)Enum.Parse(typeof(FormMethod), value[WebMoneyTemplate.SuccessUrlMethod])
                //                       : FormMethod.POST;
                //SuccessUrlMethod = value.ContainsKey(WebMoneyTemplate.FailUrlMethod)
                //                       ? (FormMethod)Enum.Parse(typeof(FormMethod), value[WebMoneyTemplate.FailUrlMethod])
                //                       : FormMethod.POST;


            }
        }
        public override void ProcessForm(Order order)
        {
            new PaymentFormHandler
                {
                    Url = "https://merchant.webmoney.ru/lmi/payment.asp",
                    InputValues = new Dictionary<string, string>
                                      {
                                          {"LMI_PAYEE_PURSE", Purse},
                                          {"LMI_PAYMENT_NO", order.OrderID.ToString()},
                                          {"LMI_PAYMENT_DESC", Resources.Resource.Client_OrderConfirmation_PayOrder + " #" + order.OrderID},
                                          {"LMI_PAYMENT_AMOUNT", (order.Sum / CurrencyValue).ToString("F2").Replace(",",".")},
                                          {"LMI_RESULT_URL", SuccessUrl},
                                          {"LMI_SUCCESS_URL", SuccessUrl},
                                          {"LMI_SUCCESS_METHOD", "POST"},
                                          {"LMI_FAIL_URL", FailUrl},
                                          {"LMI_FAIL_METHOD", "POST"}
                                      }
                }.Post();
        }

        public override string ProcessFormString(Order order, PaymentService.PageWithPaymentButton page)
        {
            return new PaymentFormHandler
             {
                 Url = "https://merchant.webmoney.ru/lmi/payment.asp",
                 Page = page,
                 InputValues = new Dictionary<string, string>
                                      {
                                          {"LMI_PAYEE_PURSE", Purse},
                                          {"LMI_PAYMENT_NO", order.OrderID.ToString()},
                                          {"LMI_PAYMENT_DESC", Resources.Resource.Client_OrderConfirmation_PayOrder + " #" + order.OrderID},
                                          {"LMI_PAYMENT_AMOUNT", (order.Sum / CurrencyValue).ToString("F2").Replace(",",".")},
                                          {"LMI_RESULT_URL", SuccessUrl},
                                          {"LMI_SUCCESS_URL", SuccessUrl},
                                          {"LMI_SUCCESS_METHOD", "POST"},
                                          {"LMI_FAIL_URL", FailUrl},
                                          {"LMI_FAIL_METHOD", "POST"}
                                      }
             }.ProcessRequest();
        }

        public override string ProcessResponse(HttpContext context)
        {
            var req = context.Request;


            if (!CheckData(req))
                return NotificationMessahges.InvalidRequestData;


            var paymentNumber = req["lmi_payment_no"];
            int orderID;
            if (int.TryParse(paymentNumber, out orderID) &&
                OrderService.GetOrder(orderID) != null)
            {
                var order = OrderService.GetOrder(orderID);
                if (order != null && req["LMI_PAYMENT_AMOUNT"] == string.Format("{0:0.00}", order.Sum / CurrencyValue))
                {
                    OrderService.PayOrder(orderID, true);
                    return NotificationMessahges.SuccessfullPayment(order.Number);
                }
            }
            return NotificationMessahges.Fail;


        }
        public bool CheckData(HttpRequest req)
        {
            var fields = new string[]
                             {
                                 "LMI_PAYEE_PURSE",
                                 "LMI_PAYMENT_AMOUNT",
                                 "LMI_PAYMENT_NO",
                                 "LMI_MODE",
                                 "LMI_SYS_INVS_NO",
                                 "LMI_SYS_TRANS_NO",
                                 "LMI_SYS_TRANS_DATE",
                                 "LMI_SECRET_KEY",
                                 "LMI_PAYER_PURSE",
                                 "WMIdLMI_PAYER_WM"
                             };

            ;
            return (!fields.Any(val => string.IsNullOrEmpty(req[val]))
                && fields.Aggregate<string, StringBuilder, string>(new StringBuilder(), (str, field) => str.Append(field == "LMI_SECRET_KEY" ? SecretKey : req[field]), Strings.ToString).Md5(true) != req["LMI_HASH"]);
        }
    }
}