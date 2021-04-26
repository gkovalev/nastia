using System;
using System.Web.Services;
using AdvantShop.Core.Caching;
using AdvantShop.Customers;

namespace AdvantShop
{
    public class SecuredWebServiceHeader : System.Web.Services.Protocols.SoapHeader
    {
        public string Username;
        public string Password;
        public string AuthenticatedToken;
    }

    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class SecuredWebService : WebService
    {

        public SecuredWebServiceHeader SoapHeader;
        [WebMethod]
        [System.Web.Services.Protocols.SoapHeader("SoapHeader")]
        public string AuthenticateUser()
        {
            if (SoapHeader == null)
                return "Please provide a Username and Password";
            if (string.IsNullOrEmpty(SoapHeader.Username) || string.IsNullOrEmpty(SoapHeader.Password))
                return "Please provide a Username and Password";

            // Are the credentials valid?
            if (!IsUserValid(SoapHeader.Username, SoapHeader.Password))
                return "Invalid Username or Password";
            // Create and store the AuthenticatedToken before returning it
            string token = Guid.NewGuid().ToString();
            CacheManager.Insert(token, SoapHeader.Username);
            //HttpRuntime.Cache.Add(token,SoapHeader.Username,null,System.Web.Caching.Cache.NoAbsoluteExpiration,TimeSpan.FromMinutes(60),System.Web.Caching.CacheItemPriority.NotRemovable,null);
            return token;
        }

        private bool IsUserValid(string username, string password)
        {
            var customer = CustomerService.GetCustomerByEmailAndPassword(username, password, false);
            return customer != null;
        }

        protected bool IsUserValid(SecuredWebServiceHeader soapHeader)
        {
            if (SoapHeader == null)
                return false;
            // Does the token exists in our Cache?
            if (!string.IsNullOrEmpty(soapHeader.AuthenticatedToken))
                return (!string.IsNullOrWhiteSpace(CacheManager.Get<string>(soapHeader.AuthenticatedToken)));
            //return (HttpRuntime.Cache[soapHeader.AuthenticatedToken] != null);
            return false;
        }


        //[WebMethod]
        //[System.Web.Services.Protocols.SoapHeader("SoapHeader")]
        //public string HelloWorld()
        //{
        //    if (!IsUserValid(SoapHeader))
        //        return "Please call AuthenitcateUser() first.";
        //    return "Hello " + HttpRuntime.Cache[SoapHeader.AuthenticatedToken];
        //}
    }
}