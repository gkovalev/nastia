using System;
using System.Text.RegularExpressions;
using System.Web.UI;
using AdvantShop;
using AdvantShop.Configuration;
using AdvantShop.Customers;
using Resources;

//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

public partial class Admin_CreateCustomer : Page
{
    protected override void InitializeCulture()
    {
        AdvantShop.Localization.Culture.InitializeCulture();
    }

    private void MsgErr(bool clean)
    {
        if (clean)
        {
            Message.Visible = false;
            Message.Text = "";
        }
        else
        {
            Message.Visible = false;
        }
    }

    private void MsgErr(string messageText)
    {
        Message.Visible = true;
        Message.Text = @"<br/>" + messageText;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_CreateCustomer_Header);

        if (!Page.IsPostBack)
        {
            //Check item count for region dropDownList
        }

        userRoleTr.Visible = CustomerSession.CurrentCustomer.IsAdmin;
    }

    protected void btnChangeCommonInfo_Click(object sender, EventArgs e)
    {
        if (DataValidation())
        {
            var roleId = userRoleTr.Visible ? (Role)Convert.ToInt32(ddlCustomerRole.SelectedValue) : Role.User;

            roleId = roleId == Role.Moderator ? Role.Moderator : Role.User;

            var res = CustomerService.ExistsEmail(txtEmail.Text);
            if (res)
            {
                MsgErr(Resource.Admin_CreateCustomer_CustomerErrorEmailExist);
                return;
            }

            var customerId = CustomerService.InsertNewCustomer(new Customer
            {
                Password = txtPassword.Text,
                FirstName = txtFirstName.Text,
                LastName = txtLastName.Text,
                Phone = txtPhone.Text,
                SubscribedForNews = chkSubscribed4News.Checked,
                EMail = txtEmail.Text,
                CustomerRole = roleId,
                CustomerGroupId = 1
            });

            if (!customerId.Equals(Guid.Empty))
            {
                Response.Redirect("ViewCustomer.aspx?CustomerID=" + customerId);
            }
            else
            {
                MsgErr(Resource.Admin_CreateCustomer_CustomerError);
            }
        }
    }

    private bool DataValidation()
    {
        bool boolIsValidPast = true;

        ulUserRegistarionValidation.InnerHtml = string.Empty;


        // begin password
        if (!string.IsNullOrEmpty(txtPassword.Text) && txtPassword.Text.Length > 3)
        {
            txtPassword.CssClass = "OrderConfirmation_ValidTextBox";
        }
        else
        {
            txtPassword.CssClass = "OrderConfirmation_InvalidTextBox";
            ulUserRegistarionValidation.InnerHtml += string.Format("<li>{0}</li>",
                                                                      Resource.Client_Registration_PasswordLenght);
            boolIsValidPast = false;
        }

        if (!string.IsNullOrEmpty(txtPasswordConfirm.Text))
        {
            txtPasswordConfirm.CssClass = "OrderConfirmation_ValidTextBox";
        }
        else
        {
            txtPasswordConfirm.CssClass = "OrderConfirmation_InvalidTextBox";
            boolIsValidPast = false;
        }

        if ((!string.IsNullOrEmpty(txtPasswordConfirm.Text)) &&
            (!string.IsNullOrEmpty(txtPassword.Text)) && (txtPassword.Text == txtPasswordConfirm.Text))
        {
            txtPassword.CssClass = "OrderConfirmation_ValidTextBox";
            txtPasswordConfirm.CssClass = "OrderConfirmation_ValidTextBox";
        }
        else
        {
            txtPassword.CssClass = "OrderConfirmation_InvalidTextBox";
            txtPasswordConfirm.CssClass = "OrderConfirmation_InvalidTextBox";
            ulUserRegistarionValidation.InnerHtml += string.Format("<li>{0}</li>", Resource.Client_Registration_PasswordNotMatch);
            boolIsValidPast = false;
        }
        // begin password

        if (!string.IsNullOrEmpty(txtFirstName.Text))
        {
            txtFirstName.CssClass = "OrderConfirmation_ValidTextBox";
        }
        else
        {
            txtFirstName.CssClass = "OrderConfirmation_InvalidTextBox";
            boolIsValidPast = false;
        }

        if (!string.IsNullOrEmpty(txtLastName.Text))
        {
            txtLastName.CssClass = "OrderConfirmation_ValidTextBox";
        }
        else
        {
            txtLastName.CssClass = "OrderConfirmation_InvalidTextBox";
            boolIsValidPast = false;
        }

        if (!string.IsNullOrEmpty(txtEmail.Text))
        {
            txtEmail.CssClass = "OrderConfirmation_ValidTextBox";

            var r = new Regex("\\w+([-+.\']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*", RegexOptions.Multiline);
            if (r.IsMatch(txtEmail.Text))
            {
                txtEmail.CssClass = "OrderConfirmation_ValidTextBox";
            }
            else
            {
                txtEmail.CssClass = "OrderConfirmation_InvalidTextBox";
                boolIsValidPast = false;
            }
        }
        else
        {
            txtEmail.CssClass = "OrderConfirmation_InvalidTextBox";
            boolIsValidPast = false;
        }

        // ------------------------------------------------------

        if (!boolIsValidPast)
        {
            ulUserRegistarionValidation.Visible = true;
            ulUserRegistarionValidation.InnerHtml += string.Format("<li>{0}</li>", Resource.Client_OrderConfirmation_EnterEmptyField);
            return false;
        }
        ulUserRegistarionValidation.Visible = false;
        return true;
    }

}