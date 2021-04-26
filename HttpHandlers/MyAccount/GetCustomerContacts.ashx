<%@ WebHandler Language="C#" Class="GetCustomerContacts" %>

using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Newtonsoft.Json;

public class GetCustomerContacts : IHttpHandler, IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        if(!AdvantShop.Customers.CustomerSession.CurrentCustomer.RegistredUser)
        {
            context.Response.Write(JsonConvert.SerializeObject(string.Empty));
        }
        
        context.Response.ContentType = "application/json";

        IList<AdvantShop.Customers.CustomerContact> contacts = AdvantShop.Customers.CustomerSession.CurrentCustomer.Contacts;

        var customerContacts = from item in contacts
                               select new
                               {
                                   item.Country,
                                   item.City,
                                   item.Address,
                                   item.Name,
                                   item.CountryId,
                                   item.RegionId,
                                   item.RegionName,
                                   item.Zip,
                                   item.CustomerContactID
                               };

        context.Response.Write(JsonConvert.SerializeObject(customerContacts));
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}
