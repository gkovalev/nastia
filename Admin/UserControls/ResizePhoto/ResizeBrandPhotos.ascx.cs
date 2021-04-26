using System;
using AdvantShop.Configuration;

public partial class Admin_UserControls_ResizePhoto_Brand : System.Web.UI.UserControl
{
    public string ErrMessage = Resources.Resource.Admin_CommonSettings_InvalidBank;
    public bool IsChanged = false;

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
        txtHight.Text = SettingsPictureSize.BrandLogoHeight.ToString();
        txtWidth.Text = SettingsPictureSize.BrandLogoWidth.ToString();
    }
    public bool SaveData()
    {
        if (SettingsPictureSize.BrandLogoHeight != Convert.ToInt32(txtHight.Text))
        {
            SettingsPictureSize.BrandLogoHeight = Convert.ToInt32(txtHight.Text);
            IsChanged = true;
        }
        if (SettingsPictureSize.BrandLogoWidth != Convert.ToInt32(txtWidth.Text))
        {
            SettingsPictureSize.BrandLogoWidth = Convert.ToInt32(txtWidth.Text);
            IsChanged = true;
        }

        LoadData();
        return true;
    }

    private bool ValidateData()
    {
        return true;
    }
}