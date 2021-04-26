//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop.Configuration;
using AdvantShop.Controls;
using AdvantShop.Helpers;
using AdvantShop.Customers;
using AdvantShop.Mails;
using AdvantShop.SEO;
using Resources;

public partial class FogotPassword : AdvantShopPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SetMeta(new MetaInfo(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Client_FogotPassword_PasswordRecovery)), null);

        if (!string.IsNullOrEmpty(Request["email"]) && !string.IsNullOrEmpty(Request["recoverycode"]))
        {
            var customer = CustomerService.GetCustomerByEmail(Request["email"]);
            if (customer == null) return;

            if (ValidationHelper.DeleteSigns(SecurityHelper.GetPasswordHash(customer.Password)) != Request["recoverycode"])
            {
                MultiView1.SetActiveView(ViewRecoveryError);
            }
            else
            {
                MultiView1.SetActiveView(ViewRecovery);
            }
        }
        else
        {
            MultiView1.SetActiveView(ViewDataCollecting);
        }
    }

    protected void btnSendPasswordByEmail_Click(object sender, EventArgs e)
    {
        var customer = CustomerService.GetCustomerByEmail(txtEmail.Text);

        if (!customer.RegistredUser)
        {
            MultiView1.SetActiveView(ViewEmailSendError);
            return;
        }

        string strLink = AdvantShop.Configuration.SettingsMain.SiteUrl + "/FogotPassword.aspx?Email=" + customer.EMail + "&RecoveryCode=" + ValidationHelper.DeleteSigns(SecurityHelper.GetPasswordHash(customer.Password));
        var clsParam = new ClsMailParamOnPwdRepair
                                               {
                                                   Email = customer.EMail,
                                                   RecoveryCode =
                                                       ValidationHelper.DeleteSigns(
                                                           SecurityHelper.GetPasswordHash(customer.Password)),
                                                   Link = strLink
                                               };

        string message = SendMail.BuildMail(clsParam);

        SendMail.SendMailNow(customer.EMail, Resources.Resource.Client_FogotPassword_PasswordRecovery, message, true);

        MultiView1.ActiveViewIndex = 1;
    }


    protected void btnChangePassword_Click(object sender, EventArgs e)
    {
        if (txtNewPassword.Text == txtNewPasswordConfirm.Text)
        {
            var customer = CustomerService.GetCustomerByEmail(Request["email"]);
            if (customer == null) return;

            if (ValidationHelper.DeleteSigns(SecurityHelper.GetPasswordHash(customer.Password)) == Request["recoverycode"])
            {
                CustomerService.ChangePassword(customer.Id.ToString(), txtNewPassword.Text, false);
                MultiView1.SetActiveView(ViewPasswordChanged);

                //ShowMessage(Notify.NotifyType.Notice, Resources.Resource.Client_FogotPassword_PasswordChanged);
            }
            else
            {
                MultiView1.SetActiveView(ViewRecoveryError);
            }
        }
        else
        {
            ShowMessage(Notify.NotifyType.Notice, Resources.Resource.Client_FogotPassword_PasswordDiffrent);
        }
    }

}