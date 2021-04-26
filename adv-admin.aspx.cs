//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using AdvantShop.Controls;
using AdvantShop.Customers;
using AdvantShop.Security;

public partial class adv_Admin : AdvantShopPage
{
    #region  Private help function

    private void MsgErr(bool clean)
    {

        if (clean)
        {
            lblError.Visible = false;
            lblError.Text = string.Empty;
        }
        else
        {
            lblError.Visible = false;
        }

    }

    private void MsgErr(string messageText)
    {
        lblError.Visible = true;
        lblError.Text = messageText;
    }

    private bool MsgErr()
    {
        return lblError.Visible;
    }

    #endregion

    protected void Page_Load(object sender, System.EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            txtLogin.Focus();
        }

        // Redirect If user already admin
        if (!Page.IsPostBack)
        {
            if (CustomerSession.CurrentCustomer.CustomerRole == Role.Administrator || CustomerSession.CurrentCustomer.CustomerRole == Role.Moderator)
            {
                Page.Response.Redirect("~/admin/default.aspx");
            }
        }

    }

    protected void btnLogIn_Click(object sender, System.EventArgs e)
    {

        // Validation
        var login = txtLogin.Text.Trim();
        var pass = txtPassword.Text.Trim();

        bool boolIsSuccessValidation = true;

        if (string.IsNullOrEmpty(login))
        {
            boolIsSuccessValidation = false;
            txtLogin.CssClass = "Admin_InvalidTextBox"; // Faild
        }
        else
        {
            txtLogin.CssClass = "Admin_ValidTextBox"; // OK
        }


        if (string.IsNullOrEmpty(pass))
        {
            boolIsSuccessValidation = false;
            txtPassword.CssClass = "Admin_InvalidTextBox"; // Faild
        }
        else
        {
            txtPassword.CssClass = "Admin_ValidTextBox"; // OK
        }


        if (!validShield.IsValid())
        {
            // Capcha faild
            boolIsSuccessValidation = false;
            validShield.TextBoxCss = "Admin_InvalidTextBox";
        }


        if (boolIsSuccessValidation == false)
        {
            validShield.TryNew();
            MsgErr("Не верно введены данные");
            return;
        }

        // ---------------------------------------------------------------------------

        if (Secure.IsDebugAccount(login, pass))//, false))
        {
            Session["isDebug"] = true;

            Secure.AddUserLog("sa", true, true);

            Page.Response.Redirect("~/admin/default.aspx"); // Hard coded!

            return;
        }

        // ---------------------------------------------------------------------------

        var user = CustomerService.GetCustomerByEmailAndPassword(login, pass, false);
        // ---------------------------------------------------------------------------

        if (user != null && (user.CustomerRole == Role.Administrator || user.CustomerRole == Role.Moderator))
        {
            AuthorizeService.WriteCookie(user);
            Page.Response.Redirect("~/admin/default.aspx");
        }
        else
        {
            MsgErr(Resources.Resource.Client_Admin_WrongPass);

            txtPassword.Text = string.Empty;
            txtLogin.Text = string.Empty;
            txtLogin.Focus();
            validShield.TryNew();
        }

        // ---------------------------------------------------------------------------
    }
}
