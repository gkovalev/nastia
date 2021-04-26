using System;
using AdvantShop;
using AdvantShop.Controls;
using AdvantShop.Customers;
using AdvantShop.Security;
using Resources;

public partial class UserControls_MyAccountChangePassword : System.Web.UI.UserControl
{
    protected void btnChangePassword_Click(object sender, EventArgs e)
    {
        if (ValidateFormData())
        {
            CustomerService.ChangePassword(CustomerSession.CurrentCustomer.Id.ToString(), txtNewPassword.Text, false);
            var customer = CustomerSession.CurrentCustomer;
            AuthorizeService.AuthorizeTheUser(customer.EMail, customer.Password, true);
            ((AdvantShopPage)this.Page).ShowMessage(Notify.NotifyType.Notice, Resource.Client_MyAccount_PasswordSaved);
            txtNewPassword.Text = string.Empty;
            txtNewPasswordConfirm.Text = string.Empty;
        }
        //Response.Redirect("myaccount.aspx#tabid=changepassword");
    }
    protected bool ValidateFormData()
    {
        if ( txtNewPassword.Text.Length < 6)
        {
            txtNewPassword.Text = string.Empty;
            txtNewPasswordConfirm.Text = string.Empty;
            ((AdvantShopPage)this.Page).ShowMessage(Notify.NotifyType.Error, Resource.Client_Registration_PasswordLenght);
            return false;
        }
        if ((txtNewPasswordConfirm.Text.IsNotEmpty()) && (txtNewPassword.Text.IsNotEmpty()) && (txtNewPassword.Text != txtNewPasswordConfirm.Text))
        {
            txtNewPassword.Text = string.Empty;
            txtNewPasswordConfirm.Text = string.Empty;
            ((AdvantShopPage)this.Page).ShowMessage(Notify.NotifyType.Error, Resource.Client_Registration_PasswordNotMatch);
            return false;
        }
        return true;
    }
}