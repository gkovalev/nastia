using System;
using AdvantShop.Configuration;

public partial class Admin_UserControls_ResizePhoto_Carousel : System.Web.UI.UserControl
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
        txtBigHight.Text = SettingsPictureSize.CarouselBigHeight.ToString();
        txtBigWidth.Text = SettingsPictureSize.CarouselBigWidth.ToString();
    }
    public bool SaveData()
    {
        if (SettingsPictureSize.CarouselBigHeight != Convert.ToInt32(txtBigHight.Text))
        {
            SettingsPictureSize.CarouselBigHeight = Convert.ToInt32(txtBigHight.Text);
            IsChanged = true;
        }
        if (SettingsPictureSize.CarouselBigWidth != Convert.ToInt32(txtBigWidth.Text))
        {
            SettingsPictureSize.CarouselBigWidth = Convert.ToInt32(txtBigWidth.Text);
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