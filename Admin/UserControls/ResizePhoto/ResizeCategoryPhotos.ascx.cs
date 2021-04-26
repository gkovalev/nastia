using System;
using AdvantShop.Configuration;

public partial class Admin_UserControls_ResizePhoto_Category : System.Web.UI.UserControl
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
        txtBigHeight.Text = SettingsPictureSize.BigCategoryImageHeight.ToString();
        txtBigWidth.Text = SettingsPictureSize.BigCategoryImageWidth.ToString();

        txtSmallHeight.Text = SettingsPictureSize.SmallCategoryImageHeight.ToString();
        txtSmallWidth.Text = SettingsPictureSize.SmallCategoryImageWidth.ToString();


    }
    public bool SaveData()
    {
        if (SettingsPictureSize.BigCategoryImageWidth != Convert.ToInt32(txtBigHeight.Text))
        {
            SettingsPictureSize.BigCategoryImageWidth = Convert.ToInt32(txtBigHeight.Text);
            IsChanged = true;
        }
        if (SettingsPictureSize.BigCategoryImageWidth != Convert.ToInt32(txtBigWidth.Text))
        {
            SettingsPictureSize.BigCategoryImageWidth = Convert.ToInt32(txtBigWidth.Text);
            IsChanged = true;
        }

        if (SettingsPictureSize.SmallCategoryImageWidth != Convert.ToInt32(txtSmallHeight.Text))
        {
            SettingsPictureSize.SmallCategoryImageWidth = Convert.ToInt32(txtSmallHeight.Text);
            IsChanged = true;   
        }
        if (SettingsPictureSize.SmallCategoryImageWidth != Convert.ToInt32(txtSmallWidth.Text))
        {
            SettingsPictureSize.SmallCategoryImageWidth = Convert.ToInt32(txtSmallWidth.Text);
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