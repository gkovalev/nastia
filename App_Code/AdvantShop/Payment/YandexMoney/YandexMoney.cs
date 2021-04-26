//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Web;
using AdvantShop.Orders;

namespace AdvantShop.Payment
{

    public class YandexMoney : PaymentMethod
    {
        public string ShopID { get; set; }
        public string ScID { get; set; }
        public string BankID { get; set; }
        public string CustomerNumber { get; set; }
        public string MethodID { get; set; }
        public decimal CurrencyValue { get; set; }
        public override PaymentType Type
        {
            get { return PaymentType.YandexMoney; }
        }
        public override ProcessType ProcessType
        {
            get { return ProcessType.FormPost; }
        }
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>
                           {
                               {YandexMoneyTemplate.ShopID, ShopID},
                               {YandexMoneyTemplate.ScID, ScID},
                               {YandexMoneyTemplate.BankID, BankID},
                               {YandexMoneyTemplate.CustomerNumber, CustomerNumber},
                               {YandexMoneyTemplate.MethodID, MethodID},
                               {YandexMoneyTemplate.CurrencyValue, CurrencyValue.ToString()}
                           };
            }
            set
            {
                ShopID = value.ElementOrDefault(YandexMoneyTemplate.ShopID);
                ScID = value.ElementOrDefault(YandexMoneyTemplate.ScID);
                BankID = value.ElementOrDefault(YandexMoneyTemplate.BankID);
                CustomerNumber = value.ElementOrDefault(YandexMoneyTemplate.CustomerNumber);
                MethodID = value.ElementOrDefault(YandexMoneyTemplate.MethodID);
                decimal decVal;
                CurrencyValue = value.ContainsKey(YandexMoneyTemplate.CurrencyValue) &&
                                decimal.TryParse(value[YandexMoneyTemplate.CurrencyValue], out decVal)
                                    ? decVal
                                    : 1;
            }
        }
        public override void ProcessForm(Order order)
        {
            new PaymentFormHandler
                {
                    Url = "http://money.yandex.ru/eshop.xml",
                    InputValues =
                        {
                            {"scid", ScID},
                            {"shopId", ShopID},
                            {"BankId", BankID},
                            {"CustomerNumber", CustomerNumber},
                            {"method_id", MethodID},
                            {"Sum", (order.Sum / CurrencyValue).ToString("F2").Replace(",",".")}
                        }
                }.Post();
        }

        public override string ProcessFormString(Order order, PaymentService.PageWithPaymentButton page)
        {
            return new PaymentFormHandler
             {
                 Url = "http://money.yandex.ru/eshop.xml",
                 Page = page,
                 InputValues =
                        {
                            {"scid", ScID},
                            {"shopId", ShopID},
                            {"BankId", BankID},
                            {"CustomerNumber", CustomerNumber},
                            {"method_id", MethodID},
                            {"Sum", (order.Sum / CurrencyValue).ToString("F2").Replace(",",".")}
                        }
             }.ProcessRequest();

        }

        public new static void ProcessResponse(HttpContext context)
        {
            throw new NotImplementedException();
        }
    }
}