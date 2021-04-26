using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using AdvantShop;
using AdvantShop.Configuration;
using AdvantShop.Controls;
using AdvantShop.Customers;
using AdvantShop.Helpers;
using AdvantShop.Orders;
using AdvantShop.Repository;
using Resources;

public partial class UserControls_OrderConfirmation_FirstStep : System.Web.UI.UserControl
{
    public OrderConfirmationData PageData { get; set; }

    // Input data
    public EnUserType UserType { get; set; }
    public bool BillingIsShipping { get; set; }
    public CustomerContact ShippingContact { get; set; }
    public CustomerContact BillingContact { get; set; }
    public Customer Customer { get; set; }

    //Output data
    public class FirstStepNextEventArgs
    {
        public bool BillingIsShipping { get; set; }
        public CustomerContact ShippingContact { get; set; }
        public CustomerContact BillingContact { get; set; }
        public Customer Customer { get; set; }
    }

    public event Action<object, FirstStepNextEventArgs> NextStep;
    protected virtual void OnNextStep(FirstStepNextEventArgs e)
    {
        if (NextStep != null) NextStep(this, e);
    }

    public event Action<object, EventArgs> BackStep;
    protected virtual void OnBackStep(EventArgs e)
    {
        if (BackStep != null) BackStep(this, e);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            //return;
        }

        if (UserType == EnUserType.RegistredUser)
        {
            DivNoReg.Visible = false;
            divButtonsForNoRegUser.Visible = false;
            if (CustomerSession.CurrentCustomer.Contacts.Count != 0)
            {
                DivRegWithoutAddress.Visible = false;
                divButtonsForRegUserWithoutAddress.Visible = false;
                LoadDataToRegistredUser(true);
            }
            else
            {
                DivReg.Visible = false;
                divButtonsForRegUser.Visible = false;
                LoadDataToRegistredUser(false);
            }
        }
        else if (UserType != EnUserType.RegistredUser)
        {
            cboCountry.DataSource = CountryService.GetAllCountries();
            List<int> ipList = CountryService.GetCountryIdByIp(Request.UserHostAddress);
            string countryId = ipList.Count == 1
                                   ? ipList[0].ToString(CultureInfo.InvariantCulture)
                                   : SettingsMain.SalerCountryId.ToString(CultureInfo.InvariantCulture);// CountryService.GetCountryIdByIso3(Resource.Admin_Default_CountryISO3).ToString(CultureInfo.InvariantCulture);
            cboCountry.DataBind();
            if (cboCountry.Items.FindByValue(countryId) != null)
                cboCountry.SelectedValue = countryId;

            cboBillingCountry.DataSource = CountryService.GetAllCountries();
            cboBillingCountry.SelectedValue = countryId;

            cboBillingCountry.DataBind();

            DivReg.Visible = false;
            DivRegWithoutAddress.Visible = false;
            divButtonsForRegUser.Visible = false;
            divButtonsForRegUserWithoutAddress.Visible = false;

            tblLoginTable.Visible = UserType == EnUserType.JustRegistredUser;
            dvDemoDataUserNotificationLoginPass.Visible = UserType == EnUserType.JustRegistredUser;

            if (Demo.IsDemoEnabled)
            {
                if ((ViewState["IsDemoDataLoaded"] != null) && ((bool)ViewState["IsDemoDataLoaded"] == false))
                {
                    dvDemoDataUserNotification.Visible = true;

                    if (Convert.ToBoolean(Session["isRegNewUser"]))
                    {
                        txtEmail.Text = Demo.GetRandomEmail();
                        txtPassword.CssClass = "OrderConfirmation_InvalidTextBox";
                        txtPasswordConfirm.CssClass = "OrderConfirmation_InvalidTextBox";
                    }

                    txtFirstName.Text = Demo.GetRandomName();
                    txtLastName.Text = Demo.GetRandomLastName();
                    txtCity.Text = Demo.GetRandomCity();
                    txtAdress.Text = Demo.GetRandomAdress();
                    txtPhone.Text = Demo.GetRandomPhone();

                    ViewState["IsDemoDataLoaded"] = true;
                }
            }
            else
            {
                SetCustomer(Customer);
                SetShippingContact(ShippingContact);
                SetBillingContact(BillingContact);
                dvDemoDataUserNotification.Visible = false;
                chkBillingIsShipping.Checked = Customer == null || BillingIsShipping;
                hfBillingIsShippingOc.Value = BillingIsShipping ? "1" : "0";
            }
        }
    }

    protected void btn_GoBack_Click(object sender, EventArgs e)
    {
        OnBackStep(new EventArgs());
    }

    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        if (!ValidateFormData())
            return;
        OnNextStep(new FirstStepNextEventArgs
        {
            BillingIsShipping = chkBillingIsShipping.Checked,
            ShippingContact = GetShippingContact(),
            BillingContact = GetBillingContact(),
            Customer = GetCustomer()
        });
    }

    protected void btnRegUserGoNext_Click(object sender, EventArgs e)
    {
        OnNextStep(new FirstStepNextEventArgs
           {
               BillingIsShipping = hfBillingIsShippingOc.Value == "1",
               ShippingContact = CustomerSession.CurrentCustomer.Contacts.Find(
                   contact => contact.CustomerContactID.ToString() == hfOcContactShippingId.Value),
               BillingContact = hfBillingIsShippingOc.Value == "0"
                                    ? CustomerSession.CurrentCustomer.Contacts.Find(
                                        contact =>
                                        contact.CustomerContactID.ToString() ==
                                        hfOcContactBillingId.Value)
                                    : null,
               Customer = CustomerSession.CurrentCustomer
           });
    }

    protected void btnRegUserWithounAddressGoNext_Click(object sender, EventArgs e)
    {
        CustomerService.AddContact(GetShippingContactForReg(), CustomerSession.CurrentCustomer.Id);
        if (!ckbBillingIsShippingReg.Checked)
            CustomerService.AddContact(GetBillingContactForReg(), CustomerSession.CurrentCustomer.Id);

        OnNextStep(new FirstStepNextEventArgs
           {
               BillingIsShipping = ckbBillingIsShippingReg.Checked,
               ShippingContact = GetShippingContactForReg(),
               BillingContact = !ckbBillingIsShippingReg.Checked
                                    ? GetBillingContactForReg()
                                    : null,
               Customer = CustomerSession.CurrentCustomer
           });
    }

    private void SetCustomer(Customer customer)
    {
        if (customer == null) return;
        txtPassword.Text = customer.Password;
        txtFirstName.Text = customer.FirstName;
        txtLastName.Text = customer.LastName;
        txtEmail.Text = customer.EMail;
        txtPhone.Text = customer.Phone;
    }

    private Customer GetCustomer()
    {
        return new Customer
                    {
                        Id = new Guid(SettingsGeneral.QrUserId),
                        EMail = txtEmail.Text,
                        Password = txtPassword.Text,
                        FirstName = HttpUtility.HtmlEncode(txtFirstName.Text),
                        LastName = HttpUtility.HtmlEncode(txtLastName.Text),
                        Phone = HttpUtility.HtmlEncode(txtPhone.Text),
                    };
    }

    protected bool ValidateLogin()
    {
        var boolIsValidPast = true;
        if (UserType != EnUserType.RegistredUser)
        {
            if (txtEmail.Text.Trim().IsNullOrEmpty() || !ValidationHelper.IsValidEmail(txtEmail.Text))
            {
                boolIsValidPast = false;
                ((AdvantShopPage)Page).ShowMessage(Notify.NotifyType.Error, Resource.Client_OrderConfirmation_EnterValidEmail);
            }
        }
        if (UserType == EnUserType.JustRegistredUser)
        {
            if (txtPassword.Text.Trim().IsNullOrEmpty() || txtPassword.Text.Length < 6)
            {
                ((AdvantShopPage)Page).ShowMessage(Notify.NotifyType.Error, Resource.Client_Registration_PasswordLenght);
                boolIsValidPast = false;
            }
            else if ((txtPassword.Text != txtPasswordConfirm.Text))
            {
                ((AdvantShopPage)Page).ShowMessage(Notify.NotifyType.Error, Resource.Client_Registration_PasswordNotMatch);
                boolIsValidPast = false;
            }
        }
        return boolIsValidPast;
    }

    protected bool ValidateUserData()
    {
        return txtFirstName.Text.IsNotEmpty() && txtLastName.Text.IsNotEmpty() && chkAgree.Checked;
    }

    protected bool ValidateShipping()
    {
        return txtCity.Text.IsNotEmpty() && txtState.Text.IsNotEmpty() && txtAdress.Text.IsNotEmpty() && txtPhone.Text.IsNotEmpty();
    }

    protected bool ValidateBilling()
    {
        return txtBillingName.Text.IsNotEmpty() && txtBillingCity.Text.IsNotEmpty() && txtBillingState.Text.IsNotEmpty()
               && txtBillingAddress.Text.IsNotEmpty();
    }

    protected bool ValidateFormData()
    {
        bool boolIsValidPast = ValidateLogin() && ValidateUserData() && ValidateShipping();

        if (!chkBillingIsShipping.Checked)
        {
            boolIsValidPast &= ValidateBilling();
        }

        if (!boolIsValidPast)
        {
            ((AdvantShopPage)Page).ShowMessage(Notify.NotifyType.Error, Resource.Client_OrderConfirmation_EnterEmptyField);
        }

        return boolIsValidPast;

    }

    private void SetShippingContact(CustomerContact customerContact)
    {
        if (customerContact == null) return;
        cboCountry.SelectedValue = customerContact.CountryId.ToString(CultureInfo.InvariantCulture);
        txtCity.Text = customerContact.City;
        txtState.Text = customerContact.RegionName;
        txtAdress.Text = customerContact.Address;
        txtZip.Text = customerContact.Zip;
    }

    private CustomerContact GetShippingContact()
    {
        var country = hfSelectedCountry.Value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        return new CustomerContact
        {
            CustomerGuid = Guid.Empty,
            Name = HttpUtility.HtmlEncode(txtFirstName.Text + " " + txtLastName.Text),
            Country = HttpUtility.HtmlEncode(country[1]),
            CountryId = country[0].TryParseInt(),
            City = HttpUtility.HtmlEncode(txtCity.Text),
            RegionName = HttpUtility.HtmlEncode(txtState.Text),
            Address = HttpUtility.HtmlEncode(txtAdress.Text),
            Zip = HttpUtility.HtmlEncode(txtZip.Text),
        };
    }

    private void SetBillingContact(CustomerContact customerContact)
    {
        if (customerContact == null) return;
        txtBillingName.Text = customerContact.Name;
        cboBillingCountry.SelectedValue = customerContact.CountryId.ToString(CultureInfo.InvariantCulture);
        txtBillingCity.Text = customerContact.City;
        txtBillingState.Text = customerContact.RegionName;
        txtBillingAddress.Text = customerContact.Address;
        txtBillingZip.Text = customerContact.Zip;
    }

    private CustomerContact GetBillingContact()
    {
        var country = hfSelectedCountryBilling.Value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        return new CustomerContact
        {
            CustomerGuid = Guid.Empty,
            Name = HttpUtility.HtmlEncode(txtBillingName.Text),
            Country = country[1],
            CountryId = Int32.Parse(country[0]),
            City = HttpUtility.HtmlEncode(txtBillingCity.Text),
            RegionName = HttpUtility.HtmlEncode(txtBillingState.Text),
            Address = HttpUtility.HtmlEncode(txtBillingAddress.Text),
            Zip = HttpUtility.HtmlEncode(txtBillingZip.Text),
        };
    }

    private CustomerContact GetShippingContactForReg()
    {
        var country = hfSelectedCountry.Value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        return new CustomerContact
        {
            CustomerGuid = CustomerSession.CurrentCustomer.Id,
            Name = CustomerSession.CurrentCustomer.FirstName + " " + CustomerSession.CurrentCustomer.LastName,
            Country = HttpUtility.HtmlEncode(country[1]),
            CountryId = country[0].TryParseInt(),
            City = HttpUtility.HtmlEncode(txtCityReg.Text),
            RegionName = HttpUtility.HtmlEncode(txtRegionReg.Text),
            Address = HttpUtility.HtmlEncode(txtAddressReg.Text),
            Zip = HttpUtility.HtmlEncode(txtZipReg.Text),
        };
    }

    private CustomerContact GetBillingContactForReg()
    {
        var country = hfSelectedCountryBilling.Value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        return new CustomerContact
        {
            CustomerGuid = CustomerSession.CurrentCustomer.Id,
            Name = HttpUtility.HtmlEncode(txtBillingNameReg.Text),
            Country = HttpUtility.HtmlEncode(country[1]),
            CountryId = country[0].TryParseInt(),
            City = HttpUtility.HtmlEncode(txtBillingCityReg.Text),
            RegionName = HttpUtility.HtmlEncode(txtBillingRegionReg.Text),
            Address = HttpUtility.HtmlEncode(txtBillingAddressReg.Text),
            Zip = HttpUtility.HtmlEncode(txtBillingZipReg.Text),
        };
    }

    private void LoadDataToRegistredUser(bool haveAddress)
    {
        if (haveAddress)
        {
            dvLoginPanel.Visible = false;

            cboCountryOc.DataSource = CountryService.GetAllCountries();
            List<int> ipList = CountryService.GetCountryIdByIp(Request.UserHostAddress);
            string countryId = ipList.Count == 1
                                   ? ipList[0].ToString(CultureInfo.InvariantCulture)
                                   : SettingsMain.SalerCountryId.ToString(CultureInfo.InvariantCulture);//CountryService.GetCountryIdByIso3(Resource.Admin_Default_CountryISO3).ToString(CultureInfo.InvariantCulture);

            cboCountryOc.DataBind();

            if (cboCountry.Items.FindByValue(countryId) != null)
                cboCountryOc.SelectedValue = countryId;

            if (CustomerSession.CurrentCustomer.Contacts.Count > 0)
            {
                hfOcContactShippingId.Value = CustomerSession.CurrentCustomer.Contacts[0].CustomerContactID.ToString();
            }
        }
        else
        {
            dvLoginPanel.Visible = false;

            ddlCountryReg.DataSource = CountryService.GetAllCountries();
            ddlBillingCountryReg.DataSource = ddlCountryReg.DataSource;

            List<int> ipList = CountryService.GetCountryIdByIp(Request.UserHostAddress);
            string countryId = ipList.Count == 1
                                   ? ipList[0].ToString(CultureInfo.InvariantCulture)
                                   : SettingsMain.SalerCountryId.ToString(CultureInfo.InvariantCulture);//CountryService.GetCountryIdByIso3(Resource.Admin_Default_CountryISO3).ToString(CultureInfo.InvariantCulture);


            ddlCountryReg.DataBind();
            if (ddlCountryReg.Items.FindByValue(countryId) != null)
                ddlCountryReg.SelectedValue = countryId;

            ddlBillingCountryReg.DataBind();
            if (ddlBillingCountryReg.Items.FindByValue(countryId) != null)
                ddlBillingCountryReg.SelectedValue = countryId;

            if (CustomerSession.CurrentCustomer.Contacts.Count > 0)
            {
                hfOcContactShippingId.Value = CustomerSession.CurrentCustomer.Contacts[0].CustomerContactID.ToString();
            }
        }
    }
}