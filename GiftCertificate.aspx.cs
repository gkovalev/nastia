//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using AdvantShop;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Controls;
using AdvantShop.Customers;
using AdvantShop.Helpers;
using AdvantShop.Orders;
using AdvantShop.Repository;
using AdvantShop.Repository.Currencies;
using AdvantShop.SEO;
using Resources;

public partial class GiftCertificate_Page : AdvantShopPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!SettingsOrderConfirmation.EnableGiftCertificateService)
            Response.Redirect("~/");

        SetMeta(new MetaInfo(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Client_GiftCertificate_Header)), string.Empty);
        cboCountry.DataSource = CountryService.GetAllCountries();
        cboCountry.DataBind();

        if (!Page.IsPostBack)
        {
            List<int> ipList = CountryService.GetCountryIdByIp(Request.UserHostAddress);
            string countryId = ipList.Count == 1
                                   ? ipList[0].ToString(CultureInfo.InvariantCulture)
                                   : SettingsMain.SalerCountryId.ToString(CultureInfo.InvariantCulture);//CountryService.GetCountryIdByIso3(Resource.Admin_Default_CountryISO3).ToString(CultureInfo.InvariantCulture);
            
            cboCountry.SelectedValue = countryId;
            hfCountry.Value = countryId;
        }
        else
        {
            cboCountry.SelectedValue = hfCountry.Value;
        }
    }

    protected int CreateCertificate()
    {
        if (!DataValidation())
            return 0;

        var curCurrency = CurrencyService.CurrentCurrency;
        var certificate = new GiftCertificate
                              {
                                  CertificateCode = GiftCertificateService.GenerateCertificateCode(),
                                  ToName = txtTo.Text,
                                  FromName = txtFrom.Text,
                                  Sum = Convert.ToDecimal(txtSum.Text),
                                  CertificateMessage = txtMessage.Text,
                                  Type = rbtnEmail.Checked ? CertificatePostType.Email : CertificatePostType.Mail,
                                  Enable = true,
                                  CurrencyCode = curCurrency.Symbol,
                                  CurrencyValue = curCurrency.Value,
                                  FromEmail = CustomerSession.CurrentCustomer.RegistredUser ? CustomerSession.CurrentCustomer.EMail : string.Empty
                              };
        if (rbtnEmail.Checked)
        {
            certificate.Email = txtEmail.Text;
        }
        else
        {
            certificate.Country = cboCountry.SelectedItem.Text;
            certificate.Zone = txtState.Text;
            certificate.City = txtCity.Text;
            certificate.Zip = txtZip.Text;
            certificate.Address = txtAdress.Text;
        }
        var certificateId = GiftCertificateService.AddCertificate(certificate);

        return certificateId;
    }

    protected void btnBuy_Click(object sender, EventArgs e)
    {
        var id = CreateCertificate();
        if (id != 0)
        {
            ShoppingCartService.AddShoppingCartItem(new ShoppingCartItem
                                                        {
                                                            EntityId = id,
                                                            Amount = 1,
                                                            ShoppingCartType = ShoppingCartType.ShoppingCart,
                                                            AttributesXml = null,
                                                            ItemType = EnumItemType.Certificate
                                                        });
            Page.Response.Redirect("~/shoppingcart.aspx");
        }
    }

    private bool DataValidation()
    {
        bool boolIsValidPast = txtFrom.Text.IsNotEmpty() && txtSum.Text.IsNotEmpty();

        decimal sum;
        if (!Decimal.TryParse(txtSum.Text.Trim(), out sum) || sum < SettingsOrderConfirmation.MinimalPriceCertificate || sum > SettingsOrderConfirmation.MaximalPriceCertificate)
        {
            boolIsValidPast = false;
            ShowMessage(Notify.NotifyType.Error, Resource.Client_GiftCertificate_WrongSum);
        }
        
        if (rbtnEmail.Checked)
        {
            boolIsValidPast &= txtFrom.Text.IsNotEmpty() && ValidationHelper.IsValidEmail(txtEmail.Text);
        }
        else
        {
            boolIsValidPast &= txtCity.Text.IsNotEmpty() && txtState.Text.IsNotEmpty() && txtZip.Text.IsNotEmpty() &&
                               txtAdress.Text.IsNotEmpty();
        }
        
        if (!validShield.IsValid())
        {
            ShowMessage(Notify.NotifyType.Error, Resource.Client_GiftCertificate_WrongCode);
            boolIsValidPast = false;
        }
        if (!boolIsValidPast)
            validShield.TryNew();

        return boolIsValidPast;
    }
}