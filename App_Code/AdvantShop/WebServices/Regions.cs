//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Text.RegularExpressions;
using System.Web.Script.Services;
using System.Web.Services;
using AdvantShop.Repository;
using AjaxControlToolkit;



// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
[ScriptService]
[WebService(Namespace = "http://microsoft.com/webservices/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class Regions : WebService
{
    [WebMethod]
    public CascadingDropDownNameValue[] GetDropDownCountries(string knownCategoryValues, string category)
    {
        return RegionService.GetCoutries().ToArray();
    }

    [WebMethod]
    public CascadingDropDownNameValue[] GetDropDownRegions(string knownCategoryValues, string category)
    {
        var reg = new Regex("(\\d+)");
        Match countryId = reg.Match(knownCategoryValues);
        return RegionService.GetRegions(countryId.ToString()).ToArray();
    }

    [WebMethod]
    public int GetRegionsCount(string countryId)
    {
        return RegionService.GetRegions(countryId).ToArray().Length;
    }
}