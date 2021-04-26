using System;
using AdvantShop.Configuration;

public partial class Tools_core_SaasSettings : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtCurrentSaasId.Text = SettingsGeneral.CurrentSaasId;
        }
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        txtCurrentSaasId.Text = SettingsGeneral.CurrentSaasId;
    }

    protected void btnSetSaasId_Click(object sender, EventArgs e)
    {
        SettingsGeneral.CurrentSaasId = txtCurrentSaasId.Text;
    }
}
