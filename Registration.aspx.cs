//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Web;
using AdvantShop;
using AdvantShop.Configuration;
using AdvantShop.Controls;
using AdvantShop.Customers;
using AdvantShop.Mails;
using AdvantShop.SEO;
using AdvantShop.Security;
using Resources;

public partial class Registration : AdvantShopPage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack && CustomerSession.CurrentCustomer.RegistredUser)
        {
            Response.Redirect("default.aspx");
        }

        SetMeta(new MetaInfo(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Client_Registration_Registration)), string.Empty);
        if (!Page.IsPostBack && Demo.IsDemoEnabled)
        {
            txtEmail.Text = Demo.GetRandomEmail();
            dvDemoDataUserNotification.Visible = true;
            txtFirstName.Text = Demo.GetRandomName();
            txtLastName.Text = Demo.GetRandomLastName();
            txtPhone.Text = Demo.GetRandomPhone();

        }
    }

    private bool DataValidation()
    {
        bool boolIsValidPast = true;

        boolIsValidPast &= txtPassword.Text.Trim().IsNotEmpty();
        boolIsValidPast &= txtPasswordConfirm.Text.Trim().IsNotEmpty();

        boolIsValidPast &= txtPasswordConfirm.Text.IsNotEmpty() && txtPassword.Text.IsNotEmpty() &&
                            txtPassword.Text == txtPasswordConfirm.Text;

        boolIsValidPast &= txtPassword.Text.Length >= 6;

        if ((string.IsNullOrEmpty(txtPasswordConfirm.Text)) || (string.IsNullOrEmpty(txtPassword.Text)) ||
        (txtPassword.Text != txtPasswordConfirm.Text))
        {
            ShowMessage(Notify.NotifyType.Error, Resource.Client_Registration_PasswordNotMatch);
        }
        if (txtPassword.Text.Length < 6)
        {
            ShowMessage(Notify.NotifyType.Error, Resource.Client_Registration_PasswordLenght);
        }

        boolIsValidPast &= txtFirstName.Text.Trim().IsNotEmpty();
        boolIsValidPast &= txtLastName.Text.Trim().IsNotEmpty();
        boolIsValidPast &= AdvantShop.Helpers.ValidationHelper.IsValidEmail(txtEmail.Text);

        if (!dnfValid.IsValid())
        {
            boolIsValidPast = false;
            ShowMessage(Notify.NotifyType.Error, Resource.Client_Registration_CodeDiffrent);
        }

        if (CustomerService.CheckCustomerExist(txtEmail.Text))
        {
            boolIsValidPast = false;
            ShowMessage(Notify.NotifyType.Error, Resource.Client_Registration_CustomerExist);
        }

        if (!chkAgree.Checked)
        {
            boolIsValidPast = false;
            ShowMessage(Notify.NotifyType.Error, Resource.Client_Registration_MustAgree);
        }

        if (!boolIsValidPast)
        {
            dnfValid.TryNew();
            return false;
        }
        return true;
    }

    protected void btnRegister_Click(object sender, EventArgs e)
    {
        if (!DataValidation()) return;

        lblMessage.Visible = false;

        CustomerService.InsertNewCustomer(new Customer
        {
            CustomerGroupId = CustomerGroupService.DefaultCustomerGroup,
            Password = HttpUtility.HtmlEncode(txtPassword.Text),
            FirstName = HttpUtility.HtmlEncode(txtFirstName.Text),
            LastName = HttpUtility.HtmlEncode(txtLastName.Text),
            Phone = HttpUtility.HtmlEncode(txtPhone.Text),
            SubscribedForNews = chkSubscribed4News.Checked,
            EMail = HttpUtility.HtmlEncode(txtEmail.Text),
            CustomerRole = Role.User
        });

        AuthorizeService.AuthorizeTheUser(txtEmail.Text, txtPassword.Text, false);

        //------------------------------------------

        var clsParam = new ClsMailParamOnRegistration
                           {
                               FirstName = HttpUtility.HtmlEncode(txtFirstName.Text),
                               LastName = HttpUtility.HtmlEncode(txtLastName.Text),
                               RegDate = AdvantShop.Localization.Culture.ConvertDate(DateTime.Now),
                               Password = HttpUtility.HtmlEncode(txtPassword.Text),
                               Subsrcibe = chkSubscribed4News.Checked
                                               ? Resource.Client_Registration_Yes
                                               : Resource.Client_Registration_No,
                               ShopURL = SettingsMain.SiteUrl
                           };

        string message = SendMail.BuildMail(clsParam);

        if (CustomerSession.CurrentCustomer.IsVirtual)
        {
            ShowMessage(Notify.NotifyType.Error, Resource.Client_Registration_Whom + txtEmail.Text + '\r' + Resource.Client_Registration_Text + message);
        }
        else
        {
            SendMail.SendMailNow(txtEmail.Text,
                                      SettingsMain.SiteUrl + " - " +
                                      string.Format(Resource.Client_Registration_RegSuccessful, txtEmail.Text),
                                      message, true);
            SendMail.SendMailNow(SettingsMail.EmailForRegReport,
                                      SettingsMain.SiteUrl + " - " +
                                      string.Format(Resource.Client_Registration_RegSuccessful, txtEmail.Text),
                                      message, true);
        }

        Response.Redirect("myaccount.aspx");
    }
}