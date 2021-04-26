using System;
using System.Collections.Generic;
using AdvantShop.Configuration;
using AdvantShop.Helpers;
using AdvantShop.Repository;
using Resources;

public partial class UserControls_MyAccountAddressBook : System.Web.UI.UserControl
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        ICollection<Country> countries = CountryService.GetAllCountries();
        List<int> ipList = CountryService.GetCountryIdByIp(Request.UserHostAddress);
        string countryId = ipList.Count == 1 ? ipList[0].ToString() : SettingsMain.SalerCountryId.ToString();//CountryService.GetCountryIdByIso3(Resource.Admin_Default_CountryISO3).ToString();
        cboCountry.DataSource = countries;
        cboCountry.DataBind();

        if (cboCountry.Items.FindByValue(countryId) != null)
            cboCountry.SelectedValue = countryId;
    }

    protected void llbAddressBook_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.AbsolutePath +
                          QueryHelper.ChangeQueryParam(Request.Url.Query, "View", "AddressBook"));
    }

}