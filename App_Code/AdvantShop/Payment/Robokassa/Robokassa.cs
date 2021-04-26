//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Web;
using AdvantShop.Localization;
using AdvantShop.Orders;

namespace AdvantShop.Payment
{
    /// <summary>
    /// Summary description for Robokassa
    /// </summary>
    public class Robokassa : PaymentMethod
    {
        public static Dictionary<string, string> GetCurrencies()
        {
            return Currencies;
        }
        public static readonly Dictionary<string, string> Currencies = new Dictionary<string, string>
                                                                           {
                                                                               {"MtsR", "МТС"},
                                                                               {"MPBeelineR", "Билайн"},
                                                                               {"BANKR", "RUR Банковская карта"},
                                                                               {"OceanBankR", "RUR Океан Банк"},
                                                                               {"TerminalsPinpayR", "Pinpay"},
                                                                               {"TerminalsComepayR", "Кампэй"},
                                                                               {"TerminalsMElementR", "Мобил Элемент"},
                                                                               {"TerminalsNovoplatR", "Новоплат"},
                                                                               {"TerminalsUnikassaR", "Уникасса"},
                                                                               {"ElecsnetR", "Элекснет"},
                                                                               {"ContactR", "RUR Contact"},
                                                                               {"IFreeR", "RUR SMS"},
                                                                               {"VTB24R", "RUR ВТБ24"},
                                                                               {"TerminalsPkbR", "Петрокоммерц"},
                                                                               {"RapidaInR", "RUR Евросеть"},
                                                                               {"AlfaBankR", "Альфа-Клик"},
                                                                               {"EasyPayB", "EasyPay"},
                                                                               {"QiwiR", "QIWI Кошелек"},
                                                                               {"MoneyMailR", "RUR MoneyMail"},
                                                                               {"RuPayR", "RUR RBK Money"},
                                                                               {"TeleMoneyR", "RUR TeleMoney"},
                                                                               {"WebCredsR", "RUR WebCreds"},
                                                                               {"ZPaymentR", "RUR Z-Payment"},
                                                                               {"VKontakteMerchantR", "RUR ВКонтакте"},
                                                                               {"W1R", "RUR Единый Кошелек"},
                                                                               {"WMBM", "WMB"},
                                                                               {"WMEM", "WME"},
                                                                               {"WMGM", "WMG"},
                                                                               {"WMRM", "WMR"},
                                                                               {"WMUM", "WMU"},
                                                                               {"WMZM", "WMZ"},
                                                                               {"MailRuR", "Деньги@Mail.Ru"},
                                                                               {"PCR", "Яндекс.Деньги"}
                                                                           };

        public override PaymentType Type
        {
            get { return PaymentType.Robokassa; }
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
        public string MerchantLogin { get; set; }
        public string Password { get; set; }
        public string CurrencyLabel { get; set; }
        public decimal CurrencyValue { get; set; }

        public override Dictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>
                           {
                               {RobokassaTemplate.MerchantLogin, MerchantLogin},
                               {RobokassaTemplate.CurrencyLabel, CurrencyLabel},
                               {RobokassaTemplate.CurrencyValue, CurrencyValue.ToString()},
                               {RobokassaTemplate.Password, Password}
                           };
            }
            set
            {
                if (value.ContainsKey(RobokassaTemplate.MerchantLogin))
                    MerchantLogin = value[RobokassaTemplate.MerchantLogin];
                Password = value.ElementOrDefault(RobokassaTemplate.Password);
                CurrencyLabel = !value.ContainsKey(RobokassaTemplate.CurrencyLabel)
                                    ? "RUB"
                                    : value[RobokassaTemplate.CurrencyLabel];
                decimal decVal = 0;
                CurrencyValue = value.ContainsKey(RobokassaTemplate.CurrencyValue) &&
                                decimal.TryParse(value[RobokassaTemplate.CurrencyValue], out decVal)
                                    ? decVal
                                    : 1;
            }
        }

        public override void ProcessForm(Order order)
        {
            string sum = (order.Sum * CurrencyValue).ToString().Replace(",", ".");
            new PaymentFormHandler
                {
                    FormName = "_xclick",
                    Method = FormMethod.GET,
                    Url = "https://merchant.roboxchange.com/Index.aspx",
                    InputValues = new Dictionary<string, string>
                                      {
                                          {"MrchLogin", MerchantLogin},
                                          {"OutSum", sum},
                                          {"InvId", order.OrderID.ToString()},
                                          {"Desc", GetOrderDescription(order.Number)},
                                          {"IncCurrLabel", CurrencyLabel},
                                          {"Culture", Culture.Language == Culture.ListLanguage.Russian ? "ru" : "en"},
                                          {
                                              "SignatureValue",
                                              (MerchantLogin + ":" + sum + ":" + order.OrderID + ":" + Password).Md5()
                                              }
                                      }
                }.Post();
        }

        public override string ProcessFormString(Order order, PaymentService.PageWithPaymentButton page)
        {
            string sum = (order.Sum * CurrencyValue).ToString().Replace(",", ".");
            return new PaymentFormHandler
             {
                 FormName = "_xclick",
                 Method = FormMethod.GET,
                 Page = page,
                 Url = "https://merchant.roboxchange.com/Index.aspx",
                 InputValues = new Dictionary<string, string>
                                      {
                                          {"MrchLogin", MerchantLogin},
                                          {"OutSum", sum},
                                          {"InvId", order.OrderID.ToString()},
                                          {"Desc", GetOrderDescription(order.Number)},
                                          {"IncCurrLabel", CurrencyLabel},
                                          {"Culture", Culture.Language == Culture.ListLanguage.Russian ? "ru" : "en"},
                                          {
                                              "SignatureValue",
                                              (MerchantLogin + ":" + sum + ":" + order.OrderID + ":" + Password).Md5()
                                              }
                                      }
             }.ProcessRequest();
        }

        public override string ProcessResponse(HttpContext context)
        {
            HttpRequest req = context.Request;
            int orderID = 0;
            if (CheckFields(req) && int.TryParse(req["InvId"], out orderID))
            {
                Order order = OrderService.GetOrder(orderID);
                if (order != null)
                {
                    OrderService.PayOrder(orderID, true);
                    return NotificationMessahges.SuccessfullPayment(orderID.ToString());
                }
            }
            return NotificationMessahges.InvalidRequestData;
        }

        private bool CheckFields(HttpRequest req)
        {
            if (string.IsNullOrEmpty(req["OutSum"]) || string.IsNullOrEmpty(req["InvId"]) || string.IsNullOrEmpty(req["Culture"]) ||
                string.IsNullOrEmpty(req["SignatureValue"]))
                return false;
            if (req["SignatureValue"].ToLower() !=
                (req["OutSum"].Trim() + ":" + req["InvId"] + ":" + Password).Md5(false))
                return false;
            return true;
        }
    }
}