using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AdvantShop.Orders;
using System.Text;
using System.Security.Cryptography;
using AdvantShop.Diagnostics;
using System.Xml.Linq;

namespace AdvantShop.Payment
{
    public class LiqPay: PaymentMethod
    {
        public string MerchantId { get; set; }
        public string MerchantSig { get; set; }
        public string MerchantISO { get; set; }

        private string response = "<request>" +
                                        "<version>1.2</version>" +
                                        "<merchant_id></merchant_id>" +
                                        "<result_url>{0}</result_url>" +
                                        "<server_url>{1}</server_url>" +
                                        "<order_id>ORDER_{2}</order_id>" +
                                        "<amount>{3}</amount>" +
                                        "<currency>{4}</currency>" +
                                        "<description></description>" +
                                        "<default_phone></default_phone>" +
                                        "<pay_way>card</pay_way>" +
                                        "<goods_id></goods_id>" +
                                    "</request>";

        public override PaymentType Type
        {
            get { return PaymentType.LiqPay; }
        }

        public override ProcessType ProcessType
        {
            get { return ProcessType.FormPost; }
        }

        public override NotificationType NotificationType
        {
            get { return NotificationType.ReturnUrl | NotificationType.Handler; }
        }
        public override UrlStatus ShowUrls
        {
            get { return UrlStatus.CancelUrl | UrlStatus.ReturnUrl | UrlStatus.NotificationUrl; }
        }

        public override Dictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>
                           {
                               {LiqPayTemplate.MerchantId, MerchantId},
                               {LiqPayTemplate.MerchantSig, MerchantSig},
                               {LiqPayTemplate.MerchantISO, MerchantISO},
                           };
            }
            set
            {
                MerchantId = value.ElementOrDefault(LiqPayTemplate.MerchantId);
                MerchantSig = value.ElementOrDefault(LiqPayTemplate.MerchantSig);
                MerchantISO = value.ElementOrDefault(LiqPayTemplate.MerchantISO);
            }
        }

        private string GetOperationXml(Order order)
        {
            return string.Format(response, SuccessUrl, SuccessUrl, order.OrderID, order.Sum, MerchantISO);
        }

        private string GetSignature(string xml)
        {
            var sign = MerchantSig + xml + MerchantSig;

            return Convert.ToBase64String(new SHA1CryptoServiceProvider().ComputeHash(Encoding.GetEncoding("utf-8").GetBytes(sign)));   // 1251
        }

        private string GetOperation(string xml)
        {
            return Convert.ToBase64String(Encoding.GetEncoding("utf-8").GetBytes(xml));
        }

        public override void ProcessForm(Order order)
        {
            var xml = GetOperationXml(order);

            new PaymentFormHandler
            {
                Url = "https://www.liqpay.com/?do=clickNbuy",
                InputValues = new Dictionary<string, string>
                                      {
                                          {"operation_xml", GetOperation(xml)},
                                          {"signature", GetSignature(xml)}
                                      }
            }.Post();
        }

        public override string ProcessFormString(Order order, PaymentService.PageWithPaymentButton page)
        {
            var xml = GetOperationXml(order);

            return new PaymentFormHandler
            {
                Url = "https://www.liqpay.com/?do=clickNbuy",
                InputValues = new Dictionary<string, string>
                                      {
                                          {"operation_xml", GetOperation(xml)},
                                          {"signature", GetSignature(xml)}
                                      }
            }.ProcessRequest();
        }

        public override string ProcessResponse(HttpContext context)
        {
            var req = context.Request;
            var orderID = 0;           

            try
            {
                if (!string.IsNullOrEmpty(req["operation_xml"]) && !string.IsNullOrEmpty(req["signature"]))
                {
                    var xml = Encoding.UTF8.GetString(Convert.FromBase64String(req["operation_xml"]));

                    if (xml.IsNotEmpty())
                    {
                        var xdoc = XDocument.Parse(xml);

                        var elOrderId = xdoc.Root.Element("order_id").Value;
                        var elStatus = xdoc.Root.Element("status").Value;

                        if (int.TryParse(elOrderId.Replace("ORDER_", ""), out orderID) && elStatus == "success")
                        {
                            OrderService.PayOrder(orderID, true);
                        }
                        else
                        {
                            Debug.LogError("LiqPay: status " + xdoc.Root.Element("status").Value
                                                 + ", code " + xdoc.Root.Element("code").Value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
            return NotificationMessahges.InvalidRequestData;
        }
        
    }
}