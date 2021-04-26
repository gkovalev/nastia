//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Web.Services;
using AdvantShop;
using AdvantShop.Customers;
using AdvantShop.Security;


// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
// <System.Web.Script.Services.ScriptService()> _
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class CustomerSvc : WebService
{
    private const string MsgAddContactFailed = "Adding contact {0} failed with sql error {1}";
    private const string MsgAddContactSuccess = "Adding contact {0} successed";
    private const string MsgAddCustomerFailed = "Adding customer {0} failed";
    private const string MsgAddCustomerSuccess = "Adding customer {0} successed";
    private const string MsgDeleteContactFailed = "Deleting contact {0} failed with sql error {1}";
    private const string MsgDeleteContactSuccess = "Deleting contact {0} successed";
    private const string MsgDeleteCustomerFailed = "Deleting customer {0} failed with sql error {1}";
    private const string MsgDeleteCustomerSuccess = "Deleting customer {0} successed";
    private const string MsgUpdateContactFailed = "Updating contact {0} failed with sql error {1}";
    private const string MsgUpdateContactSuccess = "Updating contact {0} successed";
    private const string MsgUpdateCustomerFailed = "Updating customer {0} failed with sql error {1}";
    private const string MsgUpdateCustomerSuccess = "Updating customer {0} successed";
    private const string MsgAuthFailed = "Access denied, please login";

    /// <summary>
    /// LogIn as admin and write login data to cookies
    /// </summary>
    /// <param name="login">Admin login</param>
    /// <param name="password">Admin password</param>
    /// <returns>True if successfull login</returns>
    [WebMethod]
    public bool Login(string login, string password)
    {
        return AuthorizeService.LoginAdmin(login, password, false);
    }
    /// <summary>
    /// Logout and delete user cookies
    /// </summary>
    [WebMethod]
    public void Logout()
    {
        AuthorizeService.DeleteCookie();
    }
    [WebMethod]
    public List<Customer> GetCustomers()
    {
        if (!AuthorizeService.CheckAdminCookies())
            return null;
        return new List<Customer>(CustomerService.GetCustomers());
    }

    [WebMethod]
    public Customer GetCustomer(string customerId)
    {
        if (!AuthorizeService.CheckAdminCookies())
            return null;
        return CustomerService.GetCustomer(customerId.TryParseGuid());
    }

    [WebMethod]
    public CustomerContact GetCustomerContact(string contactId)
    {
        if (!AuthorizeService.CheckAdminCookies())
            return null;
        return CustomerService.GetCustomerContact(contactId);
    }

    [WebMethod]
    public List<CustomerContact> GetCustomerContacts(string customerId)
    {
        if (!AuthorizeService.CheckAdminCookies())
            return null;
        return CustomerService.GetCustomerContacts(customerId.TryParseGuid());
    }

    [WebMethod]
    public string AddCustomer(Customer customer, string login, string password)
    {
        if (!AuthorizeService.CheckAdminCookies())
            return MsgAuthFailed;
        Guid customerID = CustomerService.InsertNewCustomer(customer);
        return string.Format(!customerID.Equals(Guid.Empty) ? MsgAddCustomerSuccess : MsgAddCustomerFailed, customerID);
    }

    [WebMethod]
    public string AddContact(CustomerContact contact, string customerId)
    {
        if (!AuthorizeService.CheckAdminCookies())
            return MsgAuthFailed;
        Guid errCode = CustomerService.AddContact(contact, new Guid(customerId));
        return errCode != Guid.Empty
                   ? string.Format(MsgAddContactSuccess, contact.CustomerContactID)
                   : string.Format(MsgAddContactFailed, contact.CustomerContactID, errCode);
    }

    [WebMethod]
    public string UpdateCustomer(Customer customer)
    {
        if (!AuthorizeService.CheckAdminCookies())
            return MsgAuthFailed;
        int errCode = CustomerService.UpdateCustomer(customer);
        return errCode == 0
                   ? string.Format(MsgUpdateCustomerSuccess, customer.Id)
                   : string.Format(MsgUpdateCustomerFailed, customer.Id, errCode);
    }

    [WebMethod]
    public string UpdateContact(CustomerContact contact)
    {
        if (!AuthorizeService.CheckAdminCookies())
            return MsgAuthFailed;
        int errCode = CustomerService.UpdateContact(contact);
        return errCode == 0
                   ? string.Format(MsgUpdateContactSuccess, contact.CustomerContactID)
                   : string.Format(MsgUpdateContactFailed, contact.CustomerContactID, errCode);
    }

    [WebMethod]
    public string DeleteCustomer(string customerId)
    {
        if (!AuthorizeService.CheckAdminCookies())
            return MsgAuthFailed;
        int errCode = CustomerService.DeleteCustomer(Guid.Parse(customerId));
        return errCode == 0
                   ? string.Format(MsgDeleteCustomerSuccess, customerId)
                   : string.Format(MsgDeleteCustomerFailed, customerId, errCode);
    }

    [WebMethod]
    public string DeleteContact(Guid contactId)
    {
        if (!AuthorizeService.CheckAdminCookies())
            return MsgAuthFailed;
        int errCode = CustomerService.DeleteContact(contactId);
        return errCode == 0
                   ? string.Format(MsgDeleteContactSuccess, contactId)
                   : string.Format(MsgDeleteContactFailed, contactId, errCode);
    }
}