using System;
using AdvantShop.Configuration;
using AdvantShop.Controls;
using AdvantShop.Customers;
using AdvantShop.SEO;
using AdvantShop.Security;

public partial class Login : AdvantShopPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (CustomerSession.CurrentCustomer.RegistredUser)
        {
            Response.Redirect("~/");
        }
        SetMeta(new MetaInfo(string.Format("{0} - {1}", SettingsMain.ShopName, Resources.Resource.Client_Login_Header)), string.Empty);
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtEmail.Text.Trim()) || string.IsNullOrEmpty(txtPassword.Text.Trim()) || !AuthorizeService.AuthorizeTheUser(txtEmail.Text, txtPassword.Text, false))
        {
            ShowMessage(Notify.NotifyType.Error, Resources.Resource.Client_MasterPage_WrongPassword);
        }
        else
        {
            Response.Redirect("~/");
        }
    }
}