using System;
using System.Drawing;

namespace Advantshop.UserControls.Modules
{
    public partial class Admin_AdvQrCodeModule : System.Web.UI.UserControl
    {
        private const string _moduleName = "advqrcode";
        
        
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            {
                ckbEnableQrCode.Checked = ModuleSettingsProvider.GetSettingValue<bool>("EnableQrcode", _moduleName);
            }
        }

        protected void Save()
        {
            ModuleSettingsProvider.SetSettingValue("EnableQrcode", ckbEnableQrCode.Checked, _moduleName);
            
            lblMessage.Text = @"Изменения сохранены";
            lblMessage.ForeColor = Color.Blue;
            lblMessage.Visible = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }
    }
}