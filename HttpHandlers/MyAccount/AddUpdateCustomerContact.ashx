<%@ WebHandler Language="C#" Class="AddUpdateCustomerContact" %>

using System;
using System.Web;
using System.Web.SessionState;
using AdvantShop;

public class AddUpdateCustomerContact : IHttpHandler, IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        var valid = true;
        valid &= context.Request["fio"].IsNotEmpty();
        valid &= context.Request["countryId"].IsNotEmpty() && context.Request["countryId"].IsInt();
        valid &= context.Request["country"].IsNotEmpty();
        valid &= context.Request["region"].IsNotEmpty();
        valid &= context.Request["city"].IsNotEmpty();
        valid &= context.Request["address"].IsNotEmpty();
        //valid &= context.Request["zip"].IsNotEmpty();

        if (!valid || !AdvantShop.Customers.CustomerSession.CurrentCustomer.RegistredUser)
        {
            ReturnResult(context, false);
        }

        if (context.Request["contactid"].IsNullOrEmpty())
        {
            var contact = new AdvantShop.Customers.CustomerContact
                              {
                                  Name = context.Request["fio"],
                                  CountryId = context.Request["countryId"].TryParseInt(),
                                  Country = context.Request["country"],
                                  RegionName = context.Request["region"],
                                  City = context.Request["city"],
                                  Address = context.Request["address"],
                                  Zip = context.Request["zip"].IsNotEmpty() ? context.Request["zip"] : "",
                              };

            var regionId = AdvantShop.Repository.RegionService.GetRegionIdByName(HttpContext.Current.Server.HtmlDecode(context.Request["region"]));
            contact.RegionId = regionId != 0 ? regionId : -1;

            var id = AdvantShop.Customers.CustomerService.AddContact(contact, AdvantShop.Customers.CustomerSession.CurrentCustomer.Id);
            ReturnResult(context, id != Guid.Empty);
        }
        else
        {
            var contact = AdvantShop.Customers.CustomerService.GetCustomerContact(HttpContext.Current.Server.HtmlDecode(context.Request["contactid"]));
            
            contact.Name = context.Request["fio"];
            contact.CountryId = context.Request["countryId"].TryParseInt();
            contact.Country = context.Request["country"];
            contact.RegionName = context.Request["region"];
            contact.City = context.Request["city"];
            contact.Address = context.Request["address"];
            contact.Zip = context.Request["zip"].IsNotEmpty() ? context.Request["zip"] : "";
            
            var regionId = AdvantShop.Repository.RegionService.GetRegionIdByName(HttpContext.Current.Server.HtmlDecode(context.Request["region"]));
            contact.RegionId = regionId != 0 ? regionId : -1;

            var id = AdvantShop.Customers.CustomerService.UpdateContact(contact);
            ReturnResult(context, id != 0);
        }
    }

    private static void ReturnResult(HttpContext context, bool result)
    {
        context.Response.ContentType = "application/json";
        context.Response.Write(result ? Newtonsoft.Json.JsonConvert.True : Newtonsoft.Json.JsonConvert.False);
        context.Response.End();
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}
