//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------
using System;
using System.Collections.Generic;

namespace AdvantShop.Payment
{
    /// <summary>
    /// Summary description for Bill
    /// </summary>
    public class Bill : PaymentMethod
    {

        public decimal CurrencyValue { get; set; }
        public string CompanyName { get; set; }
        public string TransAccount { get; set; }
        public string CorAccount { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string INN { get; set; }
        public string KPP { get; set; }
        public string BIK { get; set; }
        public string BankName { get; set; }
        public string Director { get; set; }
        public string Accountant { get; set; }
        public string Manager { get; set; }

        public override PaymentType Type
        {
            get { return PaymentType.Bill; }
        }
        public override ProcessType ProcessType
        {
            get { return ProcessType.Javascript; }
        }
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>
                           {
                               {BillTemplate.CurrencyValue, CurrencyValue.ToString()},
                               {BillTemplate.CompanyName, CompanyName},
                               {BillTemplate.TransAccount, TransAccount},
                               {BillTemplate.CorAccount, CorAccount},
                               {BillTemplate.Address, Address},
                               {BillTemplate.Telephone, Telephone},
                               {BillTemplate.INN, INN},
                               {BillTemplate.KPP, KPP},
                               {BillTemplate.BIK, BIK},
                               {BillTemplate.BankName, BankName},
                               {BillTemplate.Director, Director},
                               {BillTemplate.Accountant, Accountant},
                               {BillTemplate.Manager, Manager}
                           };
            }
            set
            {
                CompanyName = value.ElementOrDefault(BillTemplate.CompanyName);
                Accountant = value.ElementOrDefault(BillTemplate.Accountant);
                TransAccount = value.ElementOrDefault(BillTemplate.TransAccount);
                CorAccount = value.ElementOrDefault(BillTemplate.CorAccount);
                Address = value.ElementOrDefault(BillTemplate.Address);
                Telephone = value.ElementOrDefault(BillTemplate.Telephone);
                INN = value.ElementOrDefault(BillTemplate.INN);
                KPP = value.ElementOrDefault(BillTemplate.KPP);
                BIK = value.ElementOrDefault(BillTemplate.BIK);
                BankName = value.ElementOrDefault(BillTemplate.BankName);
                Director = value.ElementOrDefault(BillTemplate.Director);
                Manager = value.ElementOrDefault(BillTemplate.Manager);
                decimal decVal;
                CurrencyValue = value.ContainsKey(BillTemplate.CurrencyValue) &&
                                decimal.TryParse(value[BillTemplate.CurrencyValue], out decVal)
                                    ? decVal
                                    : 1;
            }
        }

        public override string ProcessJavascriptButton(Orders.Order order)
        {
            string[] companyAccount = {
                                          order.PaymentDetails != null ? order.PaymentDetails.CompanyName : string.Empty,
                                          order.PaymentDetails != null ? order.PaymentDetails.INN : string.Empty
                                      };

            string companyName = string.Empty;
            string inn = string.Empty;
            if (companyAccount.Length > 0)
                companyName = "&bill_CompanyName=" + companyAccount[0];
            if (companyAccount.Length > 1)
                inn = "&bill_INN=" + companyAccount[1];
            return String.Format("javascript:open_printable_version('Check_{0}.aspx?ordernumber={1}&methodid={2}{3}{4}');", Type.ToString(), order.Number, PaymentMethodID, companyName, inn);
        }
    }
}