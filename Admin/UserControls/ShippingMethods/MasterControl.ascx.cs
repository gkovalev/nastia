using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using AdvantShop;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Controls;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Payment;
using AdvantShop.Repository;
using AdvantShop.Shipping;

public partial class Admin_UserControls_ShippingMethods_MasterControl : System.Web.UI.UserControl
{
    private bool _valid = false;
    protected int ShippingMethodID
    {
        get { return (int)(ViewState["MethodID"] ?? 0); }
        set { ViewState["MethodID"] = value; }
    }
    private ParametersControl _ucSpecific;
    public ShippingMethod Method { get; set; }
    public ShippingType ShippingType { get; set; }


    protected void Page_Init(object sender, EventArgs e)
    {
        if (!Visible)
            return;
        //Dynamic user control load
        var fileName = string.Format("{0}.ascx", ShippingType);
        if (File.Exists(Server.MapPath("~/Admin/UserControls/ShippingMethods/" + fileName)))
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
            if (Method.ShippingMethodId == 1)
                return;
            ShippingMethodID = Method.ShippingMethodId;
            cbeDelete.ConfirmText = string.Format(Resources.Resource.Admin_ShippingMethod_DeleteConfirm, Method.Name);
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

            if (Method.IconFileName != null && File.Exists(FoldersHelper.GetPathAbsolut(FolderType.ShippingLogo, Method.IconFileName.PhotoName)))
            {
                imgIcon.ImageUrl = FoldersHelper.GetPath(FolderType.ShippingLogo, Method.IconFileName.PhotoName, true);
                imgIcon.Visible = true;
                btnDeleteIcon.Visible = true;
            }
            else
            {
                imgIcon.Visible = false;
                btnDeleteIcon.Visible = false;
            }

            if (_ucSpecific != null && loadParameters)
                _ucSpecific.Parameters = Method.Params;

            rptrPayments.DataSource = ShippingMethodService.GetShippingPayments(ShippingMethodID);
            rptrPayments.DataBind();
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
                var method = new ShippingMethod
                                 {
                                     Type = ShippingType,
                                     ShippingMethodId = ShippingMethodID,
                                     Name = txtName.Text,
                                     Description = txtDescription.Text,
                                     SortOrder = txtSortOrder.Text.TryParseInt(),
                                     Enabled = chkEnabled.Checked && (_ucSpecific == null || _ucSpecific.Parameters != null)
                                 };
                if (ShippingMethodService.UpdateShippingMethod(method))
                {
                    if (ShippingType == ShippingType.eDost)
                    {
                        //COD
                        if (Convert.ToBoolean(parameters[EdostTemplate.EnabledCOD]))
                        {
                            int idShip = 0;
                            Int32.TryParse(parameters[EdostTemplate.ShipIdCOD], out idShip);
                            var payment = PaymentService.GetPaymentMethod(idShip);
                            if (payment == null)
                            {
                                var payMethod = PaymentMethod.Create(PaymentType.CashOnDelivery);
                                payMethod.Name = Resources.Resource.CashOnDeliveryName;
                                payMethod.Enabled = true;
                                if (payMethod.Parameters.ContainsKey(CashOnDelivery.ShippingMethodTemplate))
                                {
                                    payMethod.Parameters[CashOnDelivery.ShippingMethodTemplate] = ShippingMethodID.ToString();
                                }
                                else
                                {
                                    payMethod.Parameters.Add(CashOnDelivery.ShippingMethodTemplate,
                                                             ShippingMethodID.ToString());

                                }

                                var id = PaymentService.AddPaymentMethod(payMethod);
                                parameters[EdostTemplate.ShipIdCOD] = id.ToString();
                            }
                        }
                        else
                        {
                            int idShip = 0;
                            Int32.TryParse(parameters[EdostTemplate.ShipIdCOD], out idShip);
                            PaymentService.DeletePaymentMethod(idShip);
                        }

                        //PickPoint
                        if (Convert.ToBoolean(parameters[EdostTemplate.EnabledPickPoint]))
                        {
                            int idShip = 0;
                            Int32.TryParse(parameters[EdostTemplate.ShipIdPickPoint], out idShip);
                            var payment = PaymentService.GetPaymentMethod(idShip);
                            if (payment == null)
                            {
                                var payMethod = PaymentMethod.Create(PaymentType.PickPoint);
                                payMethod.Name = Resources.Resource.OrderPickPointMessage;
                                payMethod.Enabled = true;
                                if (payMethod.Parameters.ContainsKey(PickPoint.ShippingMethodTemplate))
                                {
                                    payMethod.Parameters[PickPoint.ShippingMethodTemplate] = ShippingMethodID.ToString();
                                }
                                else
                                {
                                    payMethod.Parameters.Add(PickPoint.ShippingMethodTemplate, ShippingMethodID.ToString());
                                }
                                var id = PaymentService.AddPaymentMethod(payMethod);
                                parameters[EdostTemplate.ShipIdPickPoint] = id.ToString();
                            }
                        }
                        else
                        {
                            int idShip = 0;
                            Int32.TryParse(parameters[EdostTemplate.ShipIdPickPoint], out idShip);
                            PaymentService.DeletePaymentMethod(idShip);
                        }
                    }

                    var payments = new System.Collections.Generic.List<int>();
                    foreach (RepeaterItem item in rptrPayments.Items)
                    {
                        if (!((CheckBox)item.FindControl("ckbUsePayment")).Checked)
                        {
                            payments.Add(Convert.ToInt32(((HiddenField)item.FindControl("hfPaymentId")).Value));
                        }
                    }
                    ShippingMethodService.UpdateShippingPayments(ShippingMethodID, payments);

                    if (ShippingMethodService.UpdateShippingParams(method.ShippingMethodId, parameters))
                    {
                        Method = ShippingMethodService.GetShippingMethod(method.ShippingMethodId);
                        LoadFormData(_ucSpecific != null && _ucSpecific.Parameters != null);
                        OnSaved(new SavedEventArgs { Enabled = method.Enabled, Name = method.Name });
                    }
                }


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
        return _valid;
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        ShippingMethodService.DeleteShippingMethod(ShippingMethodID);
        if (ShippingMethodService.GetShippingMethod(ShippingMethodID) == null)
            Response.Redirect("~/Admin/ShippingMethod.aspx");
    }
    public event Action<object, SavedEventArgs> Saved;

    public void OnSaved(SavedEventArgs args)
    {
        Saved(this, args);
    }

    public class SavedEventArgs : EventArgs
    {
        public bool Enabled { get; set; }
        public string Name { get; set; }
    }

    protected void btnAddCountry_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtCountry.Text))
        {
            var country = CountryService.GetCountryByName(txtCountry.Text);
            if (country != null)
            {
                if (!ShippingPaymentGeoMaping.IsExistShippingCountry(ShippingMethodID, country.CountryID))
                {
                    ShippingPaymentGeoMaping.AddShippingCountry(ShippingMethodID, country.CountryID);
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
            ShippingPaymentGeoMaping.DeleteShippingCountry(ShippingMethodID, id);
        }
    }

    protected void btnAddCity_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtCity.Text))
        {
            var city = CityService.GetCityByName(txtCity.Text);
            if (city != null)
            {
                if (!ShippingPaymentGeoMaping.IsExistShippingCity(ShippingMethodID, city.CityID))
                {
                    ShippingPaymentGeoMaping.AddShippingCity(ShippingMethodID, city.CityID);
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
            ShippingPaymentGeoMaping.DeleteShippingCity(ShippingMethodID, id);
        }
    }

    protected void btnUploadIcon_Click(object sender, EventArgs e)
    {
        if (fuIcon.HasFile)
        {
            PhotoService.DeletePhotos(ShippingMethodID, PhotoType.Shipping);
            var tempName = PhotoService.AddPhoto(new Photo(0, ShippingMethodID, PhotoType.Shipping) { OriginName = fuIcon.FileName });
            if (!string.IsNullOrWhiteSpace(tempName))
            {
                using (System.Drawing.Image image = System.Drawing.Image.FromStream(fuIcon.FileContent))
                {
                    FileHelpers.SaveResizePhotoFile(FoldersHelper.GetPathAbsolut(FolderType.ShippingLogo, tempName), SettingsPictureSize.ShippingIconWidth, SettingsPictureSize.ShippingIconHeight, image);
                }
                imgIcon.ImageUrl = FoldersHelper.GetPath(FolderType.ShippingLogo, tempName, true);
                imgIcon.Visible = true;
                btnDeleteIcon.Visible = true;
            }
        }
    }

    protected void btnDeleteIcon_Click(object sender, EventArgs e)
    {
        PhotoService.DeletePhotos(ShippingMethodID, PhotoType.Shipping);
        imgIcon.Visible = false;
        btnDeleteIcon.Visible = false;
    }
}