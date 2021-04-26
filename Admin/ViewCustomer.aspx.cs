using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop;
using AdvantShop.Configuration;
using AdvantShop.Customers;
using Resources;

//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

public partial class Admin_ViewCustomer : Page
{
    protected Customer customer;
    protected bool ShowRoleAccess = false;

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
        Page.Title = string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_ViewCustomer_Header);
        MsgErr(true);
        if (string.IsNullOrEmpty(Request["customerid"]))
        {
            Response.Redirect("default.aspx");
        }
        else
        {
            customer = CustomerService.GetCustomer(Request["customerid"].TryParseGuid());
        }

        CustomerAddressBookAdmin.Customer = customer;
        
        if (IsPostBack) return;

        var currentCustomer = CustomerSession.CurrentCustomer;
        if (currentCustomer.CustomerRole == Role.Moderator && customer.CustomerRole != Role.User)
            Response.Redirect("../AccessDenied.aspx");

        lblCustomerName.Text = customer.LastName + @" " + customer.FirstName;

        //lblLogin.Text = Customer.Login;
        lblRegistrationDate.Text = AdvantShop.Localization.Culture.ConvertDate(customer.RegistrationDateTime);
        txtFirstName.Text = customer.FirstName;
        txtLastName.Text = customer.LastName;
        txtWWW.Text = customer.Phone;
        lblEmail.Text = customer.EMail;
        chkSubscribed4News.Checked = customer.SubscribedForNews;
        if(customer.CustomerRole == Role.Administrator)
        {
            lRole.Text = Resource.Admin_ViewCustomer_CustomerRole_Administrator;
            ddlCustomerRole.Visible = false;
            lRole.Visible = true;
        }
        else
        {
            ddlCustomerRole.SelectedValue =((int)customer.CustomerRole).ToString();
            ddlCustomerRole.Visible = true;
            lRole.Visible = false;
        }

        foreach (var group in CustomerGroupService.GetCustomerGroupList())
        {
            ddlCustomerGroup.Items.Add(new ListItem(string.Format("{0} - {1}%", group.GroupName, group.GroupDiscount), group.CustomerGroupId.ToString()));
        }
        ddlCustomerGroup.SelectedValue = customer.CustomerGroupId.ToString();

        ShowRoleAccess = customer.CustomerRole == Role.Moderator;
        CustomerRoleActionsAdmin.Visible = ShowRoleAccess;

        trCustomerRole.Visible = currentCustomer.CustomerRole == Role.Administrator;
        llbChangePassword.Visible = currentCustomer.CustomerRole != Role.Moderator || (currentCustomer.CustomerRole == Role.Moderator && customer.CustomerRole != Role.Administrator);
    }

    protected void btnChangeCommonInfo_Click(object sender, EventArgs e)
    {
        int groupId;
        Int32.TryParse(ddlCustomerGroup.SelectedValue, out groupId);

        customer.FirstName = HttpUtility.HtmlEncode(txtFirstName.Text);
        customer.LastName = txtLastName.Text;
        customer.Phone = txtWWW.Text;
        customer.SubscribedForNews = chkSubscribed4News.Checked;
        customer.EMail = lblEmail.Text;
        customer.CustomerGroupId = groupId;

        if (customer.CustomerRole != Role.Administrator)
        {
            var prevCustomerRole = customer.CustomerRole;
            customer.CustomerRole = (Role)Convert.ToInt32(ddlCustomerRole.SelectedValue);

            if (prevCustomerRole == Role.Moderator && customer.CustomerRole == Role.Moderator)
            {
                CustomerRoleActionsAdmin.SaveRole();
            }
            else if (prevCustomerRole == Role.Moderator && customer.CustomerRole == Role.User)
            {
                RoleActionService.DeleteCustomerRoleActions(customer.Id);
            }
        }

        CustomerService.UpdateCustomer(customer);
        ShowRoleAccess = customer.CustomerRole == Role.Moderator;
        CustomerRoleActionsAdmin.Visible = ShowRoleAccess;
    }

    protected void btnChangePassword_Click(object sender, EventArgs e)
    {
        ulUserRegistarionValidation.InnerHtml = "";
        bool boolIsValidPast = true;

        if (!string.IsNullOrEmpty(txtNewPassword.Text.Trim()) && txtNewPassword.Text.Length > 3)
        {
            txtNewPassword.CssClass = "OrderConfirmation_ValidTextBox";
        }
        else
        {
            txtNewPassword.CssClass = "OrderConfirmation_InvalidTextBox";
            ulUserRegistarionValidation.InnerHtml += string.Format("<li>{0}</li>", Resource.Client_Registration_PasswordLenght);
            boolIsValidPast = false;
        }

        if (string.IsNullOrEmpty(txtNewPasswordConfirm.Text.Trim()) == false)
        {
            txtNewPasswordConfirm.CssClass = "OrderConfirmation_ValidTextBox";
        }
        else
        {
            txtNewPasswordConfirm.CssClass = "OrderConfirmation_InvalidTextBox";
            boolIsValidPast = false;
        }

        if ((string.IsNullOrEmpty(txtNewPasswordConfirm.Text) == false) &&
            (string.IsNullOrEmpty(txtNewPassword.Text) == false) && (txtNewPassword.Text == txtNewPasswordConfirm.Text))
        {
            txtNewPassword.CssClass = "OrderConfirmation_ValidTextBox";
            txtNewPasswordConfirm.CssClass = "OrderConfirmation_ValidTextBox";
        }
        else
        {
            txtNewPassword.CssClass = "OrderConfirmation_InvalidTextBox";
            txtNewPasswordConfirm.CssClass = "OrderConfirmation_InvalidTextBox";
            ulUserRegistarionValidation.InnerHtml += string.Format("<li>{0}</li>", Resource.Client_Registration_PasswordNotMatch);
            boolIsValidPast = false;
        }

        if(boolIsValidPast)
        {
            CustomerService.ChangePassword(customer.Id.ToString(), txtNewPassword.Text, false);
            MsgErr(Resource.Admin_ViewCustomer_PasswordSaved);
            mvCommonInfo.SetActiveView(vCommonInfo);
        }
        else
        {
            ulUserRegistarionValidation.Visible = true;
            MsgErr(Resource.Admin_ViewCustomer_DiffrentPasswords);
        }
    }

    protected void llbChangePassword_Click(object sender, EventArgs e)
    {
        mvCommonInfo.SetActiveView(vChangePassword);
    }
}