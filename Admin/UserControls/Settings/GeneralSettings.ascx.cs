using System;
using System.IO;
using System.Web.UI.WebControls;
using AdvantShop;
using AdvantShop.Configuration;
using AdvantShop.Customers;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Localization;
using AdvantShop.Security;
using Resources;


public partial class Admin_UserControls_Settings_GeneralSettings : System.Web.UI.UserControl
{
    public string ErrMessage = Resources.Resource.Admin_CommonSettings_InvalidGeneral;
    public bool FlagRedirect;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            LoadData();
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        FlagRedirect = false;

        if (!string.IsNullOrWhiteSpace(SettingsMain.LogoImageName))
        {
            pnlLogo.Visible = true;
            Logo.ImgSource = FoldersHelper.GetPath(FolderType.Pictures, SettingsMain.LogoImageName, true);
        }
        else
        {
            pnlLogo.Visible = false;
        }

        if (!string.IsNullOrWhiteSpace(SettingsMain.FaviconImageName))
        {
            pnlFavicon.Visible = true;
            Favicon.ImgSource = FoldersHelper.GetPath(FolderType.Pictures, SettingsMain.FaviconImageName, true);
        }
        else
        {
            pnlFavicon.Visible = false;
        }

    }

    private void LoadData()
    {
        ddlLanguage.SelectedValue = ((int)Culture.Language).ToString();
        hfLanguage.Value = ((int)Culture.Language).ToString();

        txtShopURL.Text = SettingsMain.SiteUrl;
        lSocialLinkVk.Text = SettingsMain.SiteUrl + "/adv-vk/";
        lSocialLinkFb.Text = SettingsMain.SiteUrl + "/adv-fb/";

        txtShopName.Text = SettingsMain.ShopName;
        txtImageAlt.Text = SettingsMain.LogoImageAlt;
        txtFormat.Text = SettingsMain.AdminDateFormat;
        txtShortFormat.Text = SettingsMain.ShortDateFormat;
        ddlCountry.DataBind();
        ddlCountry.SelectedValue = SettingsMain.SalerCountryId.ToString();
        txtPhone.Text = SettingsMain.Phone;
        txtCity.Text = SettingsMain.City;
        ChbEnableUserOnline.Checked = SettingsMain.EnableUserOnline;

        int value = SettingsMain.SalerRegionId;
        if (SettingsMain.SalerRegionId != 0)
        {
            ddlRegion.DataBind();
            if (ddlRegion.Items.Count > 0)
            {
                if (ddlRegion.Items.FindByValue(value.ToString()) != null)
                    ddlRegion.SelectedValue = value.ToString();
            }
            else
                ddlRegion.Enabled = false;
        }
        trChangeAdminPass.Visible = CustomerSession.CurrentCustomer.CustomerRole == Role.Administrator;
    }

    public bool SaveData()
    {
        if (!ValidateData())
            return false;

        //SettingsMain.SiteUrl = txtShopURL.Text.StartsWith("http://") ? txtShopURL.Text : "http://" + txtShopURL.Text;
        SettingsMain.SiteUrl = txtShopURL.Text.StartsWith("http://") || txtShopURL.Text.StartsWith("https://") ? txtShopURL.Text : "http://" + txtShopURL.Text;
        SettingsMain.ShopName = txtShopName.Text;
        SettingsMain.LogoImageAlt = txtImageAlt.Text;
        SettingsMain.AdminDateFormat = txtFormat.Text;
        SettingsMain.ShortDateFormat = txtShortFormat.Text;
        SettingsMain.SalerCountryId = ddlCountry.SelectedValue != string.Empty ? Convert.ToInt32(ddlCountry.SelectedValue) : 0;
        SettingsMain.Phone = txtPhone.Text;
        SettingsMain.City = txtCity.Text;
        SettingsMain.EnableUserOnline = ChbEnableUserOnline.Checked;

        if (ddlRegion.Enabled)
        {
            SettingsMain.SalerRegionId = ddlRegion.SelectedValue != string.Empty ? Convert.ToInt32(ddlRegion.SelectedValue) : 0;
        }

        if (fuLogoImage.HasFile)
        {
            FileHelpers.CreateDirectory(FoldersHelper.GetPathAbsolut(FolderType.ImageTemp));
            FileHelpers.DeleteFile(FoldersHelper.GetPathAbsolut(FolderType.Pictures, SettingsMain.LogoImageName));
            SettingsMain.LogoImageName = fuLogoImage.FileName;
            fuLogoImage.SaveAs(FoldersHelper.GetPathAbsolut(FolderType.Pictures, fuLogoImage.FileName));
        }

        if (fuFaviconImage.HasFile)
        {
            FileHelpers.CreateDirectory(FoldersHelper.GetPathAbsolut(FolderType.ImageTemp));
            const string extensions = ".ico.png.gif";

            if (extensions.Contains(Path.GetExtension(fuFaviconImage.FileName)))
            {
                FileHelpers.DeleteFile(FoldersHelper.GetPathAbsolut(FolderType.Pictures, SettingsMain.FaviconImageName));
                SettingsMain.FaviconImageName = fuFaviconImage.FileName;
                fuFaviconImage.SaveAs(FoldersHelper.GetPathAbsolut(FolderType.Pictures, fuFaviconImage.FileName));
            }
            else
            {
                ErrMessage = Resource.Admin_CommonSettings_InvalidFaviconFormat;
                return false;
            }
        }

        Culture.Language = (Culture.ListLanguage)Convert.ToInt32(ddlLanguage.SelectedValue);
        LoadData();
        return true;
    }

    private bool ValidateData()
    {
        if (string.IsNullOrEmpty(txtShopURL.Text))
        {
            return false;
        }
        if (txtShopURL.Text.Substring(txtShopURL.Text.Length - 1).Equals("/"))
        {
            txtShopURL.Text = txtShopURL.Text.Substring(0, txtShopURL.Text.Length - 1);
        }
        return true;
    }

    protected void DeleteLogo_Click(object sender, EventArgs e)
    {
        FileHelpers.DeleteFile(FoldersHelper.GetPathAbsolut(FolderType.Pictures, SettingsMain.LogoImageName));
        SettingsMain.LogoImageName = string.Empty;
        pnlLogo.Visible = false;
    }

    protected void DeleteFavicon_Click(object sender, EventArgs e)
    {
        FileHelpers.DeleteFile(FoldersHelper.GetPathAbsolut(FolderType.Pictures, SettingsMain.FaviconImageName));
        SettingsMain.FaviconImageName = string.Empty;
        pnlFavicon.Visible = false;
    }

    
    protected void btnChangeAdminPassword_Click(object sender, EventArgs e)
    {
        if (rfvPassword.IsValid && cvPasswords.IsValid)
        {
            CustomerService.ChangePassword("835d82df-aaa6-4d54-a870-8d353e541af9", txtPassword.Text, false);
            AuthorizeService.AuthorizeTheUser("admin", txtPassword.Text, false);
        }
    }

    protected void ddlLanguage_SelectedIndexChanged(object sender, EventArgs e)
    {
        FlagRedirect = true;
    }

    protected void sds_Init(object sender, EventArgs e)
    {
        ((SqlDataSource)sender).ConnectionString = Connection.GetConnectionString();
    }

    protected void ddlCountry_SelectedChanged(object sender, EventArgs e)
    {
        ddlRegion.DataBind();
        ddlRegion.Enabled = ddlRegion.Items.Count != 0;
    }
}