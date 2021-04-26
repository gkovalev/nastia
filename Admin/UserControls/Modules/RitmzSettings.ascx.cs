using System;
using AdvantShop.Configuration;

public partial class Admin_UserControls_Settings_RitmzSettings : System.Web.UI.UserControl
{
    public string ErrMessage = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            LoadData();
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {

    }

    private void LoadData()
    {
        txtRitmzLogin.Text = SettingsRitmz.RitmzLogin;
        txtRitmzPassword.Text = SettingsRitmz.RitmzPassword;
    }

    public bool SaveData()
    {
        SettingsRitmz.RitmzLogin = txtRitmzLogin.Text;
        SettingsRitmz.RitmzPassword = txtRitmzPassword.Text;

        LoadData();
        return true;
    }
}