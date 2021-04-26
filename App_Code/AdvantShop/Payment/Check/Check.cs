//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;

namespace AdvantShop.Payment
{
    public class Check : PaymentMethod
    {
        public string CompanyName { get; set; }
        public string Phone { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Adress { get; set; }
        public string Fax { get; set; }
        public string IntPhone { get; set; }

        public override PaymentType Type
        {
            get { return PaymentType.Check; }
        }
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>
                            {
                                {CheckTemplate.Address, Adress},
                                {CheckTemplate.City, City},
                                {CheckTemplate.CompanyName, CompanyName},
                                {CheckTemplate.Country, Country},
                                {CheckTemplate.Fax, Fax},
                                {CheckTemplate.IntPhone, IntPhone},
                                {CheckTemplate.Phone, Phone},
                                {CheckTemplate.State, State}
                            };
            }
            set
            {
                Adress = value.ElementOrDefault(CheckTemplate.Address);
                City = value.ElementOrDefault(CheckTemplate.City);
                CompanyName = value.ElementOrDefault(CheckTemplate.CompanyName);
                Country = value.ElementOrDefault(CheckTemplate.Country);
                Fax = value.ElementOrDefault(CheckTemplate.Fax);
                IntPhone = value.ElementOrDefault(CheckTemplate.IntPhone);
                Phone = value.ElementOrDefault(CheckTemplate.Phone);
                State = value.ElementOrDefault(CheckTemplate.State);
            }
        }
        public override ProcessType ProcessType
        {
            get { return ProcessType.Javascript; }
        }
    }
}