using System;
using System.Web;
using AdvantShop.Orders;
using System.Collections.Generic;
//using QiwiServiceReference;

namespace AdvantShop.Payment
{
    public class Qiwi : PaymentMethod
    {
        public string From { get; set; }
        public int Lifetime { get; set; }
        public decimal CurrencyValue { get; set; }

        public override PaymentType Type
        {
            get { return PaymentType.QIWI; }
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
                               {QiwiTemplate.From, From},
                               {QiwiTemplate.Lifetime, Lifetime.ToString()},
                               {QiwiTemplate.CurrencyValue, CurrencyValue.ToString()}
                           };
            }
            set
            {
                From = value.ElementOrDefault(QiwiTemplate.From);

                int intVal;
                Lifetime = value.ContainsKey(QiwiTemplate.Lifetime) &&
                                Int32.TryParse(value[QiwiTemplate.Lifetime], out intVal)
                                    ? intVal
                                    : 1;
                decimal decVal;
                CurrencyValue = value.ContainsKey(QiwiTemplate.CurrencyValue) &&
                                decimal.TryParse(value[QiwiTemplate.CurrencyValue], out decVal)
                                    ? decVal
                                    : 1;
            }
        }

        public override void ProcessForm(Order order)
        {
            new PaymentFormHandler
            {
                Url = "http://w.qiwi.ru/setInetBill_utf.do", //_utf
                InputValues = new Dictionary<string, string>
                                      {
                                          {"from", From},
                                          {"to", order.PaymentDetails.Phone},
                                          {"summ", (order.Sum / CurrencyValue).ToString("F2").Replace(",",".")},
                                          {"com", GetOrderDescription(order.Number)},
                                          {"lifetime", Lifetime.ToString()},
                                          {"check_agt", "false"},
                                          {"txn_id", order.OrderID.ToString()}
                                      }
            }.Post();
        }

        public override string ProcessFormString(Order order, PaymentService.PageWithPaymentButton page)
        {
            return new PaymentFormHandler
            {
                Url = "http://w.qiwi.ru/setInetBill_utf.do", //_utf
                InputValues = new Dictionary<string, string>
                                      {
                                          {"from", From},
                                          {"to", order.PaymentDetails.Phone},
                                          {"summ", (order.Sum / CurrencyValue).ToString("F2").Replace(",",".")},
                                          {"com", GetOrderDescription(order.Number)},
                                          {"lifetime", Lifetime.ToString()},
                                          {"check_agt", "false"},
                                          {"txn_id", order.OrderID.ToString()}
                                      }
            }.ProcessRequest();
        }

        public bool CheckBill(int billId)
        {
            throw new Exception("payment not complete");
            //string amount;
            //string date;
            //string lifetime;
            //int status = -1;
            //var qiwi = new ShopServerWSClient();
            //qiwi.checkBill(Login, Password, billId.ToString(), out amount, out date, out lifetime, out status);
            //if (status == (int)QiwiTemplate.CheckCode.Paid)
            //{
            //    return true;
            //}
            //return false;
        }

        public override string ProcessResponse(HttpContext context)
        {
            return base.ProcessResponse(context);
        }
    }
}