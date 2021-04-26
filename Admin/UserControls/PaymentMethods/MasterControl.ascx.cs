using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Controls;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Payment;
using AdvantShop.Repository;

public partial class Admin_UserControls_PaymentMethods_MasterControl : UserControl
{
    private bool _valid = false;
    public int PaymentMethodID
    {
        get { return (int)(ViewState["MethodID"] ?? 0); }
        set { ViewState["MethodID"] = value; }
    }
    private ParametersControl _ucSpecific;
    public PaymentMethod Method { get; set; }
    public PaymentType PaymentType { get; set; }

    protected void Page_Init(object sender, EventArgs e)
    {
        if (!Visible)
            return;

        //Dynamic user control load
        var fileName = string.Format("{0}.ascx", PaymentType);
        if (File.Exists(Server.MapPath("~/Admin/UserControls/PaymentMethods/" + fileName)))
        {
            _ucSpecific = (ParametersControl)LoadControl(fileName);
            if (_ucSpecific != null)
            {
                _ucSpecific.ID = "ucSpecific";
                pnlSpecific.Controls.Add(_ucSpecific);
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Visible)
            return;
        LoadFormData(true);
    }

    private void LoadFormData(bool loadParameters)
    {
        if (Method != null)
        {
            PaymentMethodID = Method.PaymentMethodID;
            cbeDelete.ConfirmText = string.Format(Resources.Resource.Admin_PaymentMethod_DeleteConfirm, Method.Name);
            txtName.Text = Method.Name;
            txtDescription.Text = Method.Description;
            txtSortOrder.Text = Method.SortOrder.ToString();
            chkEnabled.Checked = Method.Enabled;
            if (chkEnabled.Checked)
            {
                chkEnabled.Text = Resources.Resource.Admin_Checkbox_Enabled;
                chkEnabled.ForeColor = Color.Blue;
            }
            else
            {
                chkEnabled.Text = Resources.Resource.Admin_Checkbox_Disabled;
                chkEnabled.ForeColor = Color.Red;
            }

            if (Method.IconFileName != null && File.Exists(FoldersHelper.GetPathAbsolut(FolderType.PaymentLogo, Method.IconFileName.PhotoName)))
            {
                imgIcon.ImageUrl = FoldersHelper.GetPath(FolderType.PaymentLogo, Method.IconFileName.PhotoName, true);
                imgIcon.Visible = true;
            }
            else
            {
                imgIcon.Visible = false;
            }


            if (_ucSpecific != null && loadParameters)
                _ucSpecific.Parameters = Method.Parameters;
            litReturnUrl.Text = Method.SuccessUrl;
            litFailUrl.Text = Method.FailUrl;
            litCancelUrl.Text = Method.CancelUrl;
            litNotificationUrl.Text = Method.NotificationUrl;
            trReturnUrl.Visible = (Method.ShowUrls & UrlStatus.ReturnUrl) == UrlStatus.ReturnUrl;
            trFailUrl.Visible = (Method.ShowUrls & UrlStatus.FailUrl) == UrlStatus.FailUrl;
            trCancelUrl.Visible = (Method.ShowUrls & UrlStatus.CancelUrl) == UrlStatus.CancelUrl;
            trNotificationUrl.Visible = (Method.ShowUrls & UrlStatus.NotificationUrl) == UrlStatus.NotificationUrl;

        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!Visible)
            return;

        if (ValidateFormData())
        {
            var parameters = _ucSpecific == null ? null : _ucSpecific.Parameters;

            if (parameters != null)
            {
                var method = PaymentMethod.Create(PaymentType);

                method.PaymentMethodID = PaymentMethodID;
                method.Name = txtName.Text;
                method.Description = txtDescription.Text;
                method.SortOrder = txtSortOrder.Text.TryParseInt();
                method.Enabled = chkEnabled.Checked && (_ucSpecific == null || _ucSpecific.Parameters != null);

                method.Parameters = parameters;

                PaymentService.UpdatePaymentMethod(method);
                Method = PaymentService.GetPaymentMethod(method.PaymentMethodID);
                LoadFormData(_ucSpecific != null && _ucSpecific.Parameters != null);
                OnSaved(new SavedEventArgs { Enabled = method.Enabled, Name = method.Name });
            }
        }
    }

    private void MsgErr(Label lbl, string message)
    {
        if (lbl == null) { _valid = false; return; } lbl.Visible = true;
        lbl.Text = message;
        _valid = false;
    }

    protected bool ValidateFormData()
    {
        _valid = true;
        new[] { txtName, txtSortOrder }
            .Where(textBox => string.IsNullOrEmpty(textBox.Text))
            .ForEach(textBox => MsgErr((Label)FindControl("msg" + textBox.ID.Substring(3)), Resources.Resource.Admin_Messages_EnterValue));
        if (!txtSortOrder.Text.IsInt())
            MsgErr(msgSortOrder, Resources.Resource.Admin_Messages_IsInt);
        // if (_ucSpecific != null && _ucSpecific.Parameters == null)
        //   _valid = false;
        return _valid;
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        var payment = PaymentService.GetPaymentMethod(PaymentMethodID);
        if (payment != null && payment.Type == PaymentType.GiftCertificate && SettingsOrderConfirmation.EnableGiftCertificateService)
        {
            OnErr(new ErrorEventArgs { Message = Resources.Resource.Admin_PaymentMethods_DisableGiftCertificateService });
        }

        PaymentService.DeletePaymentMethod(PaymentMethodID);
        if (PaymentService.GetPaymentMethod(PaymentMethodID) == null)
            Response.Redirect("~/Admin/PaymentMethod.aspx", true);
        else
        {
            OnErr(new ErrorEventArgs { Message = Resources.Resource.Admin_PaymentMethods_OrdersExist });
        }
    }

    public event Action<object, SavedEventArgs> Saved;
    public event Action<object, ErrorEventArgs> Err;

    public void OnErr(ErrorEventArgs args)
    {
        Err(this, args);
    }

    public void OnSaved(SavedEventArgs args)
    {
        Saved(this, args);
    }

    public class SavedEventArgs : EventArgs
    {
        public bool Enabled { get; set; }
        public string Name { get; set; }
    }
    public class ErrorEventArgs : EventArgs
    {
        public string Message { get; set; }
    }

    protected void btnAddCountry_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtCountry.Text))
        {
            var country = CountryService.GetCountryByName(txtCountry.Text);
            if (country != null)
            {
                if (!ShippingPaymentGeoMaping.IsExistPaymentCountry(PaymentMethodID, country.CountryID))
                {
                    ShippingPaymentGeoMaping.AddPaymentCountry(PaymentMethodID, country.CountryID);
                }
                txtCountry.Text = string.Empty;
            }
        }
    }

    protected void repeaterCountry_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "DelCountry")
        {
            var id = Convert.ToInt32(e.CommandArgument);
            ShippingPaymentGeoMaping.DeletePaymentCountry(PaymentMethodID, id);
        }
    }

    protected void btnAddCity_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtCity.Text))
        {
            var city = CityService.GetCityByName(txtCity.Text);
            if (city != null)
            {
                if (!ShippingPaymentGeoMaping.IsExistPaymentCity(PaymentMethodID, city.CityID))
                {
                    ShippingPaymentGeoMaping.AddPaymentCity(PaymentMethodID, city.CityID);
                }
                txtCity.Text = string.Empty;
            }
        }
    }

    protected void repeaterCity_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "DelCity")
        {
            var id = Convert.ToInt32(e.CommandArgument);
            ShippingPaymentGeoMaping.DeletePaymentCity(PaymentMethodID, id);
        }
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (fuIcon.HasFile)
        {
            PhotoService.DeletePhotos(PaymentMethodID, PhotoType.Payment);
            var tempName = PhotoService.AddPhoto(new Photo(0, PaymentMethodID, PhotoType.Payment) { OriginName = fuIcon.FileName });
            if (!string.IsNullOrWhiteSpace(tempName))
            {
                using (System.Drawing.Image image = System.Drawing.Image.FromStream(fuIcon.FileContent))
                {
                    FileHelpers.SaveResizePhotoFile(FoldersHelper.GetPathAbsolut(FolderType.PaymentLogo, tempName), SettingsPictureSize.PaymentIconWidth, SettingsPictureSize.PaymentIconHeight, image);
                }
                imgIcon.ImageUrl = FoldersHelper.GetPath(FolderType.PaymentLogo, tempName, true);
                imgIcon.Visible = true;
            }
        }
    }

    protected void btnDeleteIcon_Click(object sender, EventArgs e)
    {
        PhotoService.DeletePhotos(PaymentMethodID, PhotoType.Payment);
        imgIcon.Visible = false;
        btnDeleteIcon.Visible = false;
    }
}
