using System;
using System.Drawing;

namespace Advantshop.UserControls.Modules
{
    public partial class Admin_CallbackModule : System.Web.UI.UserControl
    {
        private const string _moduleName = "Callback";

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtEmail.Text = ModuleSettingsProvider.GetSettingValue<string>("email4notify", _moduleName);
                txtMailSubject.Text = ModuleSettingsProvider.GetSettingValue<string>("emailSubject", _moduleName);
                txtMailFormat.Text = ModuleSettingsProvider.GetSettingValue<string>("emailFormat", _moduleName);
                txtWindowTitle.Text = ModuleSettingsProvider.GetSettingValue<string>("windowTitle", _moduleName);
                txtWindowText.Text = ModuleSettingsProvider.GetSettingValue<string>("windowText", _moduleName);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            ModuleSettingsProvider.SetSettingValue("email4notify", txtEmail.Text, _moduleName);
            ModuleSettingsProvider.SetSettingValue("emailSubject", txtMailSubject.Text, _moduleName);
            ModuleSettingsProvider.SetSettingValue("emailFormat", txtMailFormat.Text, _moduleName);
            ModuleSettingsProvider.SetSettingValue("windowTitle", txtWindowTitle.Text, _moduleName);
            ModuleSettingsProvider.SetSettingValue("windowText", txtWindowText.Text, _moduleName);

            lblMessage.Text = (String)GetLocalResourceObject("Callback_Message");
            lblMessage.ForeColor = Color.Blue;
            lblMessage.Visible = true;
        }
    }
}