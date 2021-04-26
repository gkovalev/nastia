//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.UI;
using AdvantShop;
using AdvantShop.Catalog;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using AdvantShop.Mails;
using AdvantShop.Repository;
using AdvantShop.Repository.Currencies;
using Resources;
using AdvantShop.Configuration;

public partial class Admin_m_Certificate : Page
{
    protected int CertificateId
    {
        get
        {
            int id = 0;
            int.TryParse(Request["id"], out id);
            return id;
        }
    }

    protected static GiftCertificate Certificate;

    protected override void InitializeCulture()
    {
        AdvantShop.Localization.Culture.InitializeCulture();
    }

    private void MsgErr(bool clean)
    {
        if (clean)
        {
            lblError.Visible = false;
            lblError.Text = string.Empty;
        }
        else
        {
            lblError.Visible = false;
        }
    }

    private void MsgErr(string messageText)
    {
        lblError.Visible = true;
        lblError.Text = @"<br/>" + messageText;
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        if (CertificateId != 0)
        {
            SaveCertificate();
        }
        else
        {
            CreateCertificate();
        }

        // Close window
        if (lblError.Visible == false)
        {
            CommonHelper.RegCloseScript(this, string.Empty);
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        AdvantShop.Security.Secure.VerifySessionForErrors();
        AdvantShop.Security.Secure.VerifyAccessLevel();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_m_News_Header);

        if (IsPostBack)
            return;

        cboCountry.DataSource = CountryService.GetAllCountries();
        cboCountry.DataBind();
        //Check item count for region dropDownList
        List<int> ipList = CountryService.GetCountryIdByIp(Request.UserHostAddress);
        string countryId = ipList.Count == 1 ? ipList[0].ToString() : SettingsMain.SalerCountryId.ToString();//CountryService.GetCountryIdByIso3(Resource.Admin_Default_CountryISO3).ToString();
        ICollection<Region> regions = CountryService.GetRegions(Int32.Parse(countryId));
        if (cboCountry.Items.FindByValue(countryId) != null)
            cboCountry.SelectedValue = countryId;

        if (regions.Count > 0)
        {
            MultiViewRegion.ActiveViewIndex = 0;
            cboRegion.DataSource = regions;
            cboRegion.DataBind();
        }
        else
        {
            MultiViewRegion.ActiveViewIndex = 1;
        }

        if (CertificateId != 0)
        {
            btnOK.Text = Resource.Admin_m_News_Save;
            LoadCertificateById(CertificateId);
        }
        else
        {
            lblCertificateCode.Text = GiftCertificateService.GenerateCertificateCode();
        }
    }

    protected void cboCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        ICollection<Region> regions = CountryService.GetRegions(Int32.Parse(cboCountry.SelectedValue));

        if (regions.Count > 0)
        {
            MultiViewRegion.ActiveViewIndex = 0;
            cboRegion.DataSource = regions;
            cboRegion.DataBind();
        }
        else
        {
            txtState.Text = string.Empty;
            cboRegion.SelectedValue = "-1";
            MultiViewRegion.ActiveViewIndex = 1;
        }
    }

    private bool DataValidation()
    {
        bool boolIsValidPast = true;

        if (string.IsNullOrEmpty(txtFromName.Text.Trim()) == false)
        {
            txtFromName.CssClass = "OrderConfirmation_ValidTextBox";
        }
        else
        {
            txtFromName.CssClass = "OrderConfirmation_InvalidTextBox";
            boolIsValidPast = false;
        }

        if (string.IsNullOrEmpty(txtToName.Text.Trim()) == false)
        {
            txtToName.CssClass = "OrderConfirmation_ValidTextBox";
        }
        else
        {
            txtToName.CssClass = "OrderConfirmation_InvalidTextBox";
            boolIsValidPast = false;
        }

        if (string.IsNullOrEmpty(txtSum.Text.Trim()) == false)
        {
            txtSum.CssClass = "OrderConfirmation_ValidTextBox";
        }
        else
        {
            txtSum.CssClass = "OrderConfirmation_InvalidTextBox";
            boolIsValidPast = false;
        }

        decimal sum = 0;
        if (Decimal.TryParse(txtSum.Text.Trim(), out sum))
        {
            txtSum.CssClass = "OrderConfirmation_ValidTextBox";
        }
        else
        {
            txtSum.CssClass = "OrderConfirmation_InvalidTextBox";
            boolIsValidPast = false;
        }

        if (string.IsNullOrEmpty(txtFromEmail.Text) == false)
        {
            txtFromEmail.CssClass = "OrderConfirmation_ValidTextBox";
        }
        else
        {
            txtFromEmail.CssClass = "OrderConfirmation_InvalidTextBox";
            boolIsValidPast = false;
        }
        var rr = new Regex("\\w+([-+.\']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*", RegexOptions.Multiline);
        if (rr.IsMatch(txtFromEmail.Text))
        {
            txtFromEmail.CssClass = "OrderConfirmation_ValidTextBox";
        }
        else
        {
            txtFromEmail.CssClass = "OrderConfirmation_InvalidTextBox";
            boolIsValidPast = false;
        }

        if (rbtnEmail.Checked)
        {
            if (string.IsNullOrEmpty(txtEmail.Text.Trim()) == false)
            {
                txtEmail.CssClass = "OrderConfirmation_ValidTextBox";
            }
            else
            {
                txtEmail.CssClass = "OrderConfirmation_InvalidTextBox";
                boolIsValidPast = false;
            }

            var r = new Regex("\\w+([-+.\']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*", RegexOptions.Multiline);
            if (r.IsMatch(txtEmail.Text))
            {
                txtEmail.CssClass = "OrderConfirmation_ValidTextBox";
            }
            else
            {
                txtEmail.CssClass = "OrderConfirmation_InvalidTextBox";
                boolIsValidPast = false;
            }
        }
        else
        {
            if (string.IsNullOrEmpty(txtCity.Text.Trim()) == false)
            {
                txtCity.CssClass = "OrderConfirmation_ValidTextBox";
            }
            else
            {
                txtCity.CssClass = "OrderConfirmation_InvalidTextBox";
                boolIsValidPast = false;
            }

            if (MultiViewRegion.GetActiveView() == ViewTb)
            {
                if (string.IsNullOrEmpty(txtState.Text.Trim()) == false)
                {
                    txtState.CssClass = "OrderConfirmation_ValidTextBox";
                }
                else
                {
                    txtState.CssClass = "OrderConfirmation_InvalidTextBox";
                    boolIsValidPast = false;
                }
            }

            if (string.IsNullOrEmpty(txtZip.Text.Trim()) == false)
            {
                txtZip.CssClass = "OrderConfirmation_ValidTextBox";
            }
            else
            {
                txtZip.CssClass = "OrderConfirmation_InvalidTextBox";
                boolIsValidPast = false;
            }
            if (string.IsNullOrEmpty(txtAddress.Text.Trim()) == false)
            {
                txtAddress.CssClass = "OrderConfirmation_ValidTextBox";
            }
            else
            {
                txtAddress.CssClass = "OrderConfirmation_InvalidTextBox";
                boolIsValidPast = false;
            }
        }

        if (!boolIsValidPast)
            MsgErr(Resource.Admin_m_Certificate_WrongFormat);

        return boolIsValidPast;
    }

    protected void SaveCertificate()
    {
        if (!DataValidation())
            return;

        try
        {
            // var certificate = GiftCertificateService.GetCertificateByID(CertificateId);

            bool isPaid = !Certificate.Paid && chkPaid.Checked;

            Certificate.CertificateId = CertificateId;
            Certificate.CertificateCode = lblCertificateCode.Text;
            Certificate.FromName = txtFromName.Text;
            Certificate.ToName = txtToName.Text;
            Certificate.Paid = chkPaid.Checked;
            Certificate.Used = chkUsed.Checked;
            Certificate.Enable = chkEnable.Checked;
            Certificate.Type = (rbtnEmail.Checked ? CertificatePostType.Email : CertificatePostType.Mail);
            Certificate.Sum = Convert.ToDecimal(txtSum.Text);
            Certificate.FromEmail = txtFromEmail.Text;

            if (rbtnEmail.Checked)
            {
                Certificate.Email = txtEmail.Text;
            }
            else
            {
                Certificate.Country = cboCountry.SelectedItem.Text;
                Certificate.Zone = MultiViewRegion.ActiveViewIndex == 0 ? cboRegion.SelectedItem.Text : txtState.Text;
                Certificate.City = txtCity.Text;
                Certificate.Zip = txtZip.Text;
                Certificate.Address = txtAddress.Text;
            }

            if (isPaid)
            {
                GiftCertificateService.SendCertificateMails(Certificate);
            }

            GiftCertificateService.UpdateCertificateById(Certificate);
        }
        catch (Exception ex)
        {
            MsgErr(ex.Message + " SaveSertificate error");
            Debug.LogError(ex);
        }
    }

    protected void CreateCertificate()
    {
        if (!DataValidation())
            return;

        MsgErr(true);

        try
        {
            var curCurrency = CurrencyService.CurrentCurrency;
            Certificate = new GiftCertificate
                    {
                        CertificateCode = lblCertificateCode.Text,
                        FromName = txtFromName.Text,
                        ToName = txtToName.Text,
                        Paid = chkPaid.Checked,
                        Used = chkUsed.Checked,
                        Enable = chkEnable.Checked,
                        Type = rbtnEmail.Checked ? CertificatePostType.Email : CertificatePostType.Mail,
                        Sum = Convert.ToDecimal(txtSum.Text),
                        CertificateMessage = txtMessage.Text,
                        CurrencyCode = curCurrency.Symbol,
                        CurrencyValue = curCurrency.Value,
                        FromEmail = txtFromEmail.Text
                    };
            if (rbtnEmail.Checked)
            {
                Certificate.Email = txtEmail.Text;
            }
            else
            {
                Certificate.Country = cboCountry.SelectedItem.Text;
                Certificate.Zone = MultiViewRegion.ActiveViewIndex == 0 ? cboRegion.SelectedItem.Text : txtState.Text;
                Certificate.City = txtCity.Text;
                Certificate.Zip = txtZip.Text;
                Certificate.Address = txtAddress.Text;
            }
            if (Certificate.Paid && !CustomerSession.CurrentCustomer.IsVirtual)
            {
                GiftCertificateService.SendCertificateMails(Certificate);
            }

            GiftCertificateService.AddCertificate(Certificate);

        }
        catch (Exception ex)
        {
            MsgErr(ex.Message + " Create Certificate error");
            Debug.LogError(ex);
        }
    }

    protected void LoadCertificateById(int certificateId)
    {
        Certificate = GiftCertificateService.GetCertificateByID(certificateId);
        if (Certificate == null)
        {
            MsgErr("Certificate with this ID does not exist");
            return;
        }

        lblCertificateCode.Text = Certificate.CertificateCode;
        txtFromName.Text = Certificate.FromName;
        txtToName.Text = Certificate.ToName;
        txtSum.Text = Certificate.Sum.ToString("#0.00");
        chkPaid.Checked = Certificate.Paid;
        chkUsed.Checked = Certificate.Used;
        rbtnEmail.Checked = Certificate.Type == CertificatePostType.Email;
        rbtnMail.Checked = Certificate.Type == CertificatePostType.Mail;
        chkEnable.Checked = Certificate.Enable;
        txtMessage.Text = Certificate.CertificateMessage;
        txtFromEmail.Text = Certificate.FromEmail;

        if (Certificate.Type == CertificatePostType.Mail)
        {
            rbtnMail.Checked = true;
            cboCountry.SelectedItem.Selected = false;
            cboCountry.Items.FindByText(Certificate.Country).Selected = true;
            
            var region = cboRegion.Items.FindByText(Certificate.Zone);
            if (region != null)
            {
                MultiViewRegion.ActiveViewIndex = 0;
                cboRegion.SelectedItem.Selected = false;
                region.Selected = true;
            }
            else
            {
                MultiViewRegion.ActiveViewIndex = 1;
                txtState.Text = Certificate.Zone;
            }
            txtCity.Text = Certificate.City;
            txtZip.Text = Certificate.Zip;
            txtAddress.Text = Certificate.Address;
        }
        else
        {
            rbtnEmail.Checked = true;
            txtEmail.Text = Certificate.Email;
        }
    }


}