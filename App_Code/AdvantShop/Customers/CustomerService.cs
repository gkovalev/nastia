//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdvantShop.Configuration;
using AdvantShop.Core;
using AdvantShop.Core.Caching;
using AdvantShop.Helpers;

namespace AdvantShop.Customers
{
    public class CustomerService
    {
        public static readonly Guid InternetUserGuid = new Guid("1E684980-E140-49B9-830B-C462D4A84041");

        private static Customer _internetUser;
        public static Customer InternetUser { get { return _internetUser ?? (_internetUser = GetCustomer(InternetUserGuid)); } }

        public static List<string> GetCustomersIDs()
        {
            List<string> result = SQLDataAccess.ExecuteReadList<string>("SELECT [CustomerID] FROM [Customers].[Customer]",
                                                                CommandType.Text, reader => SQLDataHelper.GetGuid(reader, "CustomerID").ToString());
            return result;
        }

        public static string GetCustomerEmailById(Guid custId)
        {
            var result = SQLDataAccess.ExecuteReadOne<string>("[Customers].[sp_GetCustomerByID]", CommandType.StoredProcedure,
                                                                 reader => SQLDataHelper.GetString(reader, "Email"), new SqlParameter("@CustomerID", custId));
            return result;
        }

        public static int DeleteCustomer(Guid customerId)
        {
            //var cacheName = CacheNames.GetCustomerCacheObjectName(customerId);
            //CacheManager.Remove(cacheName);

         
            if (SettingsMailChimp.MailChimpActive)
            {
                Mails.MailChimp.UnsubscribeListMember(SettingsMailChimp.MailChimpId,
                                                      SettingsMailChimp.MailChimpRegUsersList,
                                                      GetCustomerEmailById(customerId));
                //Changed by Evgeni to unsubscribe from unregcutomers
                Mails.MailChimp.UnsubscribeListMember(SettingsMailChimp.MailChimpId,
                                                    SettingsMailChimp.MailChimpUnRegUsersList,
                                                    GetCustomerEmailById(customerId));
                Mails.MailChimp.UnsubscribeListMember(SettingsMailChimp.MailChimpId,
                                                      SettingsMailChimp.MailChimpAllUsersList,
                                                      GetCustomerEmailById(customerId));
            }
            SQLDataAccess.ExecuteNonQuery("[Customers].[sp_DeleteCustomer]", CommandType.StoredProcedure, new SqlParameter("@CustomerID", customerId));
            return 0;
        }

        public static int DeleteContact(Guid contactId)
        {
            SQLDataAccess.ExecuteNonQuery("[Customers].[sp_DeleteCustomerContact]", CommandType.StoredProcedure,
                                          new SqlParameter { ParameterName = "@ContactID", Value = contactId });
            return 0;
        }

        public static int GetCustomerGroupId(Guid customerId)
        {
            return SQLDataHelper.GetInt(SQLDataAccess.ExecuteScalar("SELECT [CustomerGroupId] FROM [Customers].[Customer] WHERE [CustomerID] = @CustomerID", CommandType.Text, new SqlParameter { ParameterName = "@CustomerID", Value = customerId }), CustomerGroupService.DefaultCustomerGroup);

        }

        public static Customer GetCustomer(Guid customerId)
        {
            //var cacheName = CacheNames.GetCustomerCacheObjectName(customerId.ToString());
            //if (CacheManager.Contains(cacheName))
            //{
            //    var item = CacheManager.Get<Customer>(cacheName);
            //    if (item != null)
            //        return item;
            //}
            var customer = SQLDataAccess.ExecuteReadOne<Customer>("SELECT * FROM [Customers].[Customer] WHERE [CustomerID] = @CustomerID", CommandType.Text,
                                                                  Customer.GetFromSqlDataReader, new SqlParameter { ParameterName = "@CustomerID", Value = customerId });
            if (customer == null)
                customer = new Customer();

            //CacheManager.Insert<Customer>(cacheName, customer);

            return customer;
        }

        public static bool ExistsCustomer(Guid customerId)
        {
            return SQLDataAccess.ExecuteScalar<int>
                ("SELECT COUNT([CustomerID]) FROM [Customers].[Customer] WHERE [CustomerID] = @CustomerID",
                    CommandType.Text,
                    new SqlParameter { ParameterName = "@CustomerID", Value = customerId }) > 0;
        }

        public static Customer GetCustomerByEmail(string email)
        {
            var customer = SQLDataAccess.ExecuteReadOne<Customer>(
                "[Customers].[sp_GetCustomerByEmail]", CommandType.StoredProcedure,
                Customer.GetFromSqlDataReader, new SqlParameter { ParameterName = "@Email", Value = email });

            return customer ?? new Customer();
        }

        public static Customer GetCustomerByOpenAuthIdentifier(string identifier)
        {
            var customer = SQLDataAccess.ExecuteReadOne<Customer>(
                "[Customers].[sp_GetCustomerByOpenAuthIdentifier]", CommandType.StoredProcedure,
                Customer.GetFromSqlDataReader, new SqlParameter { ParameterName = "@Identifier", Value = identifier });

            return customer ?? new Customer();
        }

        public static CustomerContact GetContactFromSqlDataReader(SqlDataReader reader)
        {
            var contact = new CustomerContact
            {
                CustomerContactID = SQLDataHelper.GetGuid(reader, "ContactID"),
                Address = SQLDataHelper.GetString(reader, "Address"),
                City = SQLDataHelper.GetString(reader, "City"),
                Country = SQLDataHelper.GetString(reader, "Country"),
                Name = SQLDataHelper.GetString(reader, "Name"),
                Zip = SQLDataHelper.GetString(reader, "Zip"),
                RegionName = SQLDataHelper.GetString(reader, "Zone"),
                CountryId = SQLDataHelper.GetInt(reader, "CountryID"),
                RegionId = SQLDataHelper.GetNullableInt(reader, "RegionID"),
                CustomerGuid = SQLDataHelper.GetGuid(reader, "CustomerID")
            };

            return contact;
        }

        public static CustomerContact GetCustomerContact(string contactId)
        {
            var contact = SQLDataAccess.ExecuteReadOne<CustomerContact>(
                "SELECT * FROM [Customers].[Contact] WHERE [ContactID] = @id",
                CommandType.Text,
                GetContactFromSqlDataReader,
                new SqlParameter { ParameterName = "@id", Value = contactId });

            return contact;
        }

        public static List<CustomerContact> GetCustomerContacts(Guid customerId)
        {
            return SQLDataAccess.ExecuteReadList<CustomerContact>(
                "[Customers].[sp_GetCustomerContact]",
                CommandType.StoredProcedure, GetContactFromSqlDataReader,
                new SqlParameter { ParameterName = "@CustomerID", Value = customerId });
        }

        public static IList<Customer> GetCustomers()
        {
            return SQLDataAccess.ExecuteReadList<Customer>("SELECT * FROM [Customers].[Customer]", CommandType.Text, Customer.GetFromSqlDataReader);
        }

        public static List<string> GetCustomersEmails()
        {
            return SQLDataAccess.ExecuteReadColumn<string>("SELECT Email FROM [Customers].[Customer]", CommandType.Text, "Email");
        }

        public static Guid AddContact(CustomerContact contact, Guid customerId)
        {
            return SQLDataHelper.GetGuid(SQLDataAccess.ExecuteScalar("[Customers].[sp_AddCustomerContact]",
                                                                    CommandType.StoredProcedure,
                                                                    new SqlParameter("@CustomerID", customerId),
                                                                    new SqlParameter("@Name", contact.Name),
                                                                    new SqlParameter("@Country", contact.Country),
                                                                    new SqlParameter("@City", contact.City),
                                                                    new SqlParameter("@Zone", contact.RegionName),
                                                                    new SqlParameter("@Address", contact.Address),
                                                                    new SqlParameter("@Zip", contact.Zip),
                                                                    new SqlParameter("@CountryID", contact.CountryId),
                                                                    new SqlParameter("@RegionID",
                                                                                     contact.RegionId.HasValue &&
                                                                                     contact.RegionId != -1
                                                                                         ? (object) contact.RegionId
                                                                                         : DBNull.Value)
                                            ));
        }

        public static int UpdateContact(CustomerContact contact)
        {
            SQLDataAccess.ExecuteNonQuery("[Customers].[sp_UpdateCustomerContact]", CommandType.StoredProcedure,
                                          new SqlParameter("@ContactID", contact.CustomerContactID),
                                          new SqlParameter("@Name", contact.Name),
                                          new SqlParameter("@Country", contact.Country),
                                          new SqlParameter("@City", contact.City),
                                          new SqlParameter("@Zone", contact.RegionName),
                                          new SqlParameter("@Address", contact.Address),
                                          new SqlParameter("@Zip", contact.Zip),
                                          new SqlParameter("@CountryID", contact.CountryId),
                                          new SqlParameter("@RegionID", contact.RegionId.HasValue && contact.RegionId != -1
                                              ? (object)contact.RegionId
                                              : DBNull.Value)
                                          );
            return 0;
        }

        public static int UpdateCustomer(Customer customer)
        {
            //  // Changed by Evgeni to devide Optovok and other users
            if (SettingsMailChimp.MailChimpActive)// && CustomerSubscribe(customer.Id) != customer.SubscribedForNews)
            {
                // Changed by Evgeni to devide Optovok and other users
                if (customer.SubscribedForNews)
                {
                    if (customer.CustomerGroup.GroupName.ToLower() != "оптовик")
                    {
                        Mails.MailChimp.SubscribeListMember(SettingsMailChimp.MailChimpId,
                                                            SettingsMailChimp.MailChimpRegUsersList, customer.EMail);
                        Mails.MailChimp.UnsubscribeListMember(SettingsMailChimp.MailChimpId,
                                                             SettingsMailChimp.MailChimpUnRegUsersList, customer.EMail);
                    }
                    else //"оптовик"
                    {
                        Mails.MailChimp.SubscribeListMember(SettingsMailChimp.MailChimpId,
                                                         SettingsMailChimp.MailChimpUnRegUsersList, customer.EMail);
                        Mails.MailChimp.UnsubscribeListMember(SettingsMailChimp.MailChimpId,
                                                            SettingsMailChimp.MailChimpRegUsersList, customer.EMail);
                    }
                    Mails.MailChimp.SubscribeListMember(SettingsMailChimp.MailChimpId,
                                                      SettingsMailChimp.MailChimpAllUsersList, customer.EMail);
                }
                else
                {
                    Mails.MailChimp.UnsubscribeListMember(SettingsMailChimp.MailChimpId,
                        SettingsMailChimp.MailChimpRegUsersList, customer.EMail);
                    Mails.MailChimp.UnsubscribeListMember(SettingsMailChimp.MailChimpId,
                        SettingsMailChimp.MailChimpAllUsersList, customer.EMail);
                }
            }

            SQLDataAccess.ExecuteNonQuery("[Customers].[sp_UpdateCustomerInfo]", CommandType.StoredProcedure,
                                            new SqlParameter("@CustomerID", customer.Id),
                                            new SqlParameter("@FirstName", customer.FirstName),
                                            new SqlParameter("@LastName", customer.LastName),
                                            new SqlParameter("@Phone", customer.Phone),
                                            new SqlParameter("@Email", customer.EMail),
                                            new SqlParameter("@Subscribed4News", customer.SubscribedForNews),
                                            new SqlParameter("@CustomerGroupId", customer.CustomerGroupId == 0 ? (object)DBNull.Value : customer.CustomerGroupId),
                                            new SqlParameter("@CustomerRole", customer.CustomerRole)
                                            );
            return 0;
        }

        public static int UpdateCustomerEmail(Guid id, string email)
        {
            SQLDataAccess.ExecuteNonQuery("Update Customers.Customer Set Email = @Email Where CustomerID = @CustomerID",
                                            CommandType.Text, new SqlParameter("@CustomerID", id), new SqlParameter("@Email", email));
            return 0;
        }

        public static bool CustomerSubscribe(Guid customerId)
        {
            return SQLDataAccess.ExecuteScalar<bool>(
                "Select Subscribed4News From Customers.Customer Where CustomerID = @CustomerID", CommandType.Text,
                                            new SqlParameter("@CustomerID", customerId));


        }

        public static Customer GetCustomerByEmailAndPassword(string email, string password, bool isHash)
        {
            var customer = SQLDataAccess.ExecuteReadOne<Customer>("[Customers].[sp_GetCustomerByEmailAndPassword]",
                                                                       CommandType.StoredProcedure, Customer.GetFromSqlDataReader,
                                                                       new SqlParameter { ParameterName = "@Email", Value = email },
                                                                       new SqlParameter { ParameterName = "@Password", Value = isHash ? password : SecurityHelper.GetPasswordHash(password) });
            return customer;
        }

        public static string ConvertToLinedAddress(CustomerContact cc)
        {
            string address = string.Empty;

            if (!String.IsNullOrEmpty(cc.Country.Trim()))
            {
                address += cc.Country + ", ";
            }

            if (cc.RegionName.Trim() != "-")
            {
                address += cc.RegionName + ", ";
            }

            if (!String.IsNullOrEmpty(cc.City.Trim()))
            {
                address += cc.City + ", ";
            }

            if (cc.Zip.Trim() != "-")
            {
                address += cc.Zip + ", ";
            }

            if (!String.IsNullOrEmpty(cc.Address.Trim()))
            {
                address += cc.Address + ", ";
            }

            return address;
        }

        public static bool ExistsEmail(string strUserEmail)
        {
            if (string.IsNullOrEmpty(strUserEmail))
            {
                return false;
            }

            bool boolRes = SQLDataAccess.ExecuteScalar<int>("SELECT COUNT(CustomerID) FROM [Customers].[Customer] WHERE [Email] = @Email;", CommandType.Text, new SqlParameter("@Email", strUserEmail)) > 0;

            return boolRes;
        }

        public static void ChangePassword(string customerId, string strNewPassword, bool isPassHashed)
        {
            SQLDataAccess.ExecuteNonQuery("[Customers].[sp_ChangePassword]", CommandType.StoredProcedure,
                                                new SqlParameter { ParameterName = "@CustomerID", Value = customerId },
                                                new SqlParameter { ParameterName = "@Password", Value = isPassHashed ? strNewPassword : SecurityHelper.GetPasswordHash(strNewPassword) }
                                                );
        }

        public static Guid InsertNewCustomer(Customer customer)
        {
            if (CheckCustomerExist(customer.EMail))
            {
                return Guid.Empty;
            }
            var temp = SQLDataAccess.ExecuteScalar("[Customers].[sp_AddCustomer]", CommandType.StoredProcedure,
                                                new SqlParameter("@CustomerGroupID", customer.CustomerGroupId),
                                                new SqlParameter("@Password", SecurityHelper.GetPasswordHash(customer.Password)),
                                                new SqlParameter("@FirstName", customer.FirstName),
                                                new SqlParameter("@LastName", customer.LastName),
                                                new SqlParameter("@Phone", string.IsNullOrEmpty(customer.Phone) ? (object)DBNull.Value : customer.Phone),
                                                new SqlParameter("@RegistrationDateTime", DateTime.Now),
                                                new SqlParameter("@Subscribed4News", customer.SubscribedForNews),
                                                new SqlParameter("@Email", customer.EMail),
                                                new SqlParameter("@CustomerRole", customer.CustomerRole)
                                                ).ToString();
            if (SettingsMailChimp.MailChimpActive && customer.SubscribedForNews)
            {
                Mails.MailChimp.SubscribeListMember(SettingsMailChimp.MailChimpId, SettingsMailChimp.MailChimpAllUsersList, customer.EMail);
                Mails.MailChimp.SubscribeListMember(SettingsMailChimp.MailChimpId, SettingsMailChimp.MailChimpRegUsersList, customer.EMail);
            }

            customer.Id = new Guid(temp);
            //var cacheName = CacheNames.GetCustomerCacheObjectName(temp);
            //just clean
            //CacheManager.Remove(cacheName);
            return customer.Id;
        }

        public static string GetContactId(CustomerContact contact)
        {
            var res = SQLDataHelper.GetNullableGuid(SQLDataAccess.ExecuteScalar("[Customers].[sp_GetContactIDByContent]", CommandType.StoredProcedure,
                                         new SqlParameter("@Name", contact.Name),
                                         new SqlParameter("@Country", contact.Country),
                                         new SqlParameter("@City", contact.City),
                                         new SqlParameter("@Zone", contact.RegionName ?? ""),
                                         new SqlParameter("@Zip", contact.Zip ?? ""),
                                         new SqlParameter("@Address", contact.Address),
                                         new SqlParameter("@CustomerID", contact.CustomerGuid)
                                         ));
            return res == null ? null : res.ToString();
        }

        public static bool CheckCustomerExist(string email)
        {
            return SQLDataAccess.ExecuteScalar<int>("SELECT COUNT([CustomerID]) FROM [Customers].[Customer] WHERE [Email] = @Email",
                CommandType.Text, new SqlParameter("@Email", email)) != 0;
        }

        public static bool AddOpenIdLinkCustomer(Guid customerGuid, string identifier)
        {
            return Convert.ToInt32(SQLDataAccess.ExecuteScalar(
                @"Insert Into [Customers].[OpenIdLinkCustomer] (CustomerID, OpenIdIdentifier) Values (@CustomerID, @OpenIdIdentifier)",
                CommandType.Text,
                new SqlParameter("@CustomerID", customerGuid),
                new SqlParameter("@OpenIdIdentifier", identifier))) != 0;
        }

        public static bool IsExistOpenIdLinkCustomer(string identifier)
        {
            return SQLDataAccess.ExecuteScalar<int>(
                @"SELECT COUNT([CustomerID]) FROM [Customers].[OpenIdLinkCustomer] WHERE [OpenIdIdentifier] = @OpenIdIdentifier",
                CommandType.Text,
                new SqlParameter("@OpenIdIdentifier", identifier)) != 0;
        }

        public static void ChangeCustomerGroup(string customerId, int customerGroupId)
        {
            SQLDataAccess.ExecuteNonQuery("Update [Customers].[Customer] Set CustomerGroupId = @CustomerGroupId WHERE CustomerID = @CustomerID",
                                            CommandType.Text, new SqlParameter("@CustomerID", customerId), new SqlParameter("@CustomerGroupId", customerGroupId));
        }
    }
}