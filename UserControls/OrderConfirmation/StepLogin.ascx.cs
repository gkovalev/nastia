using System;
using AdvantShop;
using AdvantShop.Customers;
using AdvantShop.Orders;
using AdvantShop.Security;
using Resources;
using AdvantShop.Controls;

public partial class UserControls_OrderConfirmation_ZeroStep : System.Web.UI.UserControl
{

    public class ZeroStepNextEventArgs
    {
        public EnUserType UserType { get; set; }
    }

    public event Action<object, ZeroStepNextEventArgs> NextStep;

    protected virtual void OnNextStep(ZeroStepNextEventArgs e)
    {
        if (NextStep != null) NextStep(this, e);
    }

    protected void bntGoQReg_Click(object sender, EventArgs e)
    {
        OnNextStep(new ZeroStepNextEventArgs { UserType = EnUserType.NewUserWithOutRegistration });
    }

    protected void btnGoWithReg_Click(object sender, EventArgs e)
    {
        OnNextStep(new ZeroStepNextEventArgs { UserType = EnUserType.JustRegistredUser });
    }

    protected void btnAuthGO_Click(object sender, EventArgs e)
    {
        if (!ValidateFormData())
        {
            return;
        }

        if (!AuthorizeService.AuthorizeTheUser(txtAuthLogin.Text, txtAuthPWD.Text, false))
        {
            ((AdvantShopPage)this.Page).ShowMessage(Notify.NotifyType.Error, Resource.Client_MasterPage_WrongPassword);
            txtAuthPWD.Text = string.Empty;
        }
        else
        {
            OnNextStep(new ZeroStepNextEventArgs { UserType = EnUserType.RegistredUser });
        }
    }

    protected bool ValidateFormData()
    {
        bool boolIsValidPast = true;

        if (txtAuthLogin.Text.Trim().IsNullOrEmpty())
        {
            ((AdvantShopPage)this.Page).ShowMessage(Notify.NotifyType.Error, Resource.Client_OrderConfirmation_EnterEmail);
            boolIsValidPast = false;
        }
        
        if (txtAuthPWD.Text.Trim().IsNullOrEmpty())
        {
            ((AdvantShopPage)this.Page).ShowMessage(Notify.NotifyType.Error, Resource.Client_OrderConfirmation_EnterPassword);
            boolIsValidPast = false;
        }
        return boolIsValidPast;
    }
}