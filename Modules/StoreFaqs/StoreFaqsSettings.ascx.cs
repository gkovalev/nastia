using System;
using System.Drawing;

namespace Advantshop.UserControls.Modules.StoreFaqs
{
    public partial class Admin_StoreFaqsSettings : System.Web.UI.UserControl
    {
        private const string _moduleName = "StoreFaqs";

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ckbEnableStoreFaqs.Checked = ModuleSettingsProvider.GetSettingValue<bool>("EnableStoreFaqs", _moduleName);
            chkShowRatio.Checked = ModuleSettingsProvider.GetSettingValue<bool>("ShowRatio", _moduleName);
            ckbActiveModerate.Checked = ModuleSettingsProvider.GetSettingValue<bool>("ActiveModerateStoreFaqs", _moduleName);
            txtPageSize.Text = ModuleSettingsProvider.GetSettingValue<string>("PageSize", _moduleName);

            txtPageTitle.Text = ModuleSettingsProvider.GetSettingValue<string>("PageTitle", _moduleName);
            txtMetaDescription.Text = ModuleSettingsProvider.GetSettingValue<string>("MetaDescription", _moduleName);
            txtMetaKeyWords.Text = ModuleSettingsProvider.GetSettingValue<string>("MetaKeyWords", _moduleName);
        }

        protected void Save()
        {
            ModuleSettingsProvider.SetSettingValue("EnableStoreFaqs", ckbEnableStoreFaqs.Checked, _moduleName);
            ModuleSettingsProvider.SetSettingValue("ShowRatio", chkShowRatio.Checked, _moduleName);
            ModuleSettingsProvider.SetSettingValue("ActiveModerateStoreFaqs", ckbActiveModerate.Checked, _moduleName);
            ModuleSettingsProvider.SetSettingValue("PageSize", txtPageSize.Text, _moduleName);

            ModuleSettingsProvider.SetSettingValue("PageTitle", txtPageTitle.Text, _moduleName);
            ModuleSettingsProvider.SetSettingValue("MetaDescription", txtMetaDescription.Text, _moduleName);
            ModuleSettingsProvider.SetSettingValue("MetaKeyWords", txtMetaKeyWords.Text, _moduleName);

            lblMessage.Text = (string)GetLocalResourceObject("StoreFaqs_ChangesSaved");
            lblMessage.ForeColor = Color.Blue;
            lblMessage.Visible = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int pageSize;
            bool resultParsePageSize = int.TryParse(txtPageSize.Text, out pageSize);

            if (!resultParsePageSize)
            {
                lblMessage.Text = (string)GetLocalResourceObject("StoreFaqs_SaveErrorPageSize");
                lblMessage.ForeColor = Color.Red;
                lblMessage.Visible = true;
            }
            else
            {
                Save();
            }
        }
    }
}