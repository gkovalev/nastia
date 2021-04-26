using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop;
using AdvantShop.Controls;
using AdvantShop.Payment;

public partial class Admin_UserControls_PaymentMethods_Check : ParametersControl
{
    public override Dictionary<string, string> Parameters
    {
        get
        {
            bool validateForm = ValidateFormData(new[] {  txtCompanyName,
                          txtPhone,
                          txtCountry,
                          txtState,
                          txtCity,
                          txtAddress,
                          txtFax,
                          txtIntPhone }, null,null);


            if (_valid || validateForm)
            {
                return new Dictionary<string, string>
                           {
                               {CheckTemplate.CompanyName, txtCompanyName.Text},
                               {CheckTemplate.Phone, txtPhone.Text},
                               {CheckTemplate.Country, txtCountry.Text},
                               {CheckTemplate.State, txtState.Text},
                               {CheckTemplate.City, txtCity.Text},
                               {CheckTemplate.Address, txtAddress.Text},
                               {CheckTemplate.Fax, txtFax.Text},
                               {CheckTemplate.IntPhone, txtIntPhone.Text}
                           };
            }

            return null;
        }
        set
        {
            txtCompanyName.Text = value.ElementOrDefault(CheckTemplate.CompanyName);
            txtPhone.Text = value.ElementOrDefault(CheckTemplate.Phone);
            txtCountry.Text = value.ElementOrDefault(CheckTemplate.Country);
            txtState.Text = value.ElementOrDefault(CheckTemplate.State);
            txtCity.Text = value.ElementOrDefault(CheckTemplate.City);
            txtAddress.Text = value.ElementOrDefault(CheckTemplate.Address);
            txtFax.Text = value.ElementOrDefault(CheckTemplate.Fax);
            txtIntPhone.Text = value.ElementOrDefault(CheckTemplate.IntPhone);
        }
    }
   
}