using System;
using AdvantShop.Configuration;

public partial class Admin_UserControls_ResizePhoto_PaymentShipping : System.Web.UI.UserControl
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
        txtPaymentHeight.Text = SettingsPictureSize.PaymentIconHeight.ToString();
        txtPaymentWidth.Text = SettingsPictureSize.PaymentIconWidth.ToString();

        txtShippingHeight.Text = SettingsPictureSize.ShippingIconHeight.ToString();
        txtShippingWidth.Text = SettingsPictureSize.ShippingIconWidth.ToString();



    }
    public bool SaveData()
    {
        if (SettingsPictureSize.BigCategoryImageWidth != Convert.ToInt32(txtPaymentHeight.Text))
        {
            SettingsPictureSize.BigCategoryImageWidth = Convert.ToInt32(txtPaymentHeight.Text);
            IsChanged = true;
        }
        if (SettingsPictureSize.BigCategoryImageWidth != Convert.ToInt32(txtPaymentWidth.Text))
        {
            SettingsPictureSize.BigCategoryImageWidth = Convert.ToInt32(txtPaymentWidth.Text);
            IsChanged = true;
        }

        if (SettingsPictureSize.SmallCategoryImageWidth != Convert.ToInt32(txtShippingHeight.Text))
        {
            SettingsPictureSize.SmallCategoryImageWidth = Convert.ToInt32(txtShippingHeight.Text);
            IsChanged = true;   
        }
        if (SettingsPictureSize.SmallCategoryImageWidth != Convert.ToInt32(txtShippingWidth.Text))
        {
            SettingsPictureSize.SmallCategoryImageWidth = Convert.ToInt32(txtShippingWidth.Text);
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