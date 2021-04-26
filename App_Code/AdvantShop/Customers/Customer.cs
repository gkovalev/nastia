//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using AdvantShop.Helpers;

namespace AdvantShop.Customers
{
    [Serializable]
    public class Customer
    {
        public Customer()
        {
            Id = Guid.Empty;
            Password = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            EMail = string.Empty;
            CustomerGroupId = CustomerService.GetCustomerGroupId(CustomerService.InternetUserGuid);
        }

        public static Customer GetFromSqlDataReader(SqlDataReader reader)
        {
            var customer = new Customer
            {
                Id = SQLDataHelper.GetGuid(reader, "CustomerID"),
                CustomerGroupId = SQLDataHelper.GetInt(reader, "CustomerGroupId", 0),
                EMail = SQLDataHelper.GetString(reader, "EMail"),
                FirstName = SQLDataHelper.GetString(reader, "FirstName"),
                LastName = SQLDataHelper.GetString(reader, "LastName"),
                RegistrationDateTime = SQLDataHelper.GetDateTime(reader, "RegistrationDateTime"),
                SubscribedForNews = SQLDataHelper.GetBoolean(reader, "Subscribed4News"),
                Phone = SQLDataHelper.GetString(reader, "Phone"),
                Password = SQLDataHelper.GetString(reader, "Password"),
                CustomerRole = (Role)SQLDataHelper.GetInt(reader, "CustomerRole")
            };

            return customer;
        }

        public Guid Id { get; set; }

        public int CustomerGroupId { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        private CustomerGroup _customerGroup;
        public CustomerGroup CustomerGroup
        {
            get { return _customerGroup ?? (_customerGroup = CustomerGroupService.GetCustomerGroup(CustomerGroupId)); }
        }

        private List<CustomerContact> _contacts;
        public List<CustomerContact> Contacts
        {
            get { return _contacts ?? (_contacts = CustomerService.GetCustomerContacts(Id)); }
        }

        public void ReLoadContacts()
        {
            _contacts = CustomerService.GetCustomerContacts(Id);
        }

        public string Phone { get; set; }

        public DateTime RegistrationDateTime { get; set; }

        public bool SubscribedForNews { get; set; }

        public string EMail { get; set; }

        public bool IsAdmin
        {
            get { return EMail == "admin"; }
        }

        public bool RegistredUser
        {
            get { return EMail != string.Empty; }
        }

        public Role CustomerRole { get; set; }

        public bool IsVirtual { get; set; }
    }
}