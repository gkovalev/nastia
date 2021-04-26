//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop.Orders;

namespace AdvantShop.Customers
{
    [Serializable]
    public class CustomerContact
    {
        public Guid CustomerContactID { get; set; }

        public Guid CustomerGuid { get; set; }

        public string Name { get; set; }

        public int CountryId { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public int? RegionId { get; set; }

        public string RegionName { get; set; }

        public string Address { get; set; }

        public string Zip { get; set; }
        
        public CustomerContact()
        {
            CustomerContactID = Guid.Empty;
            Address = string.Empty;
            City = string.Empty;
            Country = string.Empty;
            CustomerGuid = Guid.Empty;
            Name = string.Empty;
            Zip = string.Empty;
            RegionName = string.Empty;
        }

        public static explicit operator OrderContact(CustomerContact contact)
        {
            return new OrderContact
                       {
                           Name = contact.Name,
                           Country = contact.Country,
                           Zone = contact.RegionName.IsNullOrEmpty() ? null : contact.RegionName,
                           City = contact.City,
                           Zip = contact.Zip.IsNullOrEmpty() ? null : contact.Zip,
                           Address = contact.Address,
                       };
        }
    }
}