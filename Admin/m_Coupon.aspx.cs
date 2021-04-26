//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using AdvantShop;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using Resources;

public partial class Admin_m_Coupon : Page
{
    protected int CouponId
    {
        get
        {
            int id = 0;
            int.TryParse(Request["id"], out id);
            return id;
        }
    }

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
        if (CouponId != 0)
        {
            SaveCoupon();
        }
        else
        {
            CreateCoupon();
        }

        // Close window
        if (lblError.Visible == false)
        {
            CommonHelper.RegCloseScript(this, string.Empty);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_m_Coupon_Header);
        popTree.CheckChildrenNodes = true;

        if (!IsPostBack)
        {
            if (CouponId != 0)
            {
                btnOK.Text = Resource.Admin_m_Coupon_Save;
                LoadCouponById(CouponId);
            }
            else
            {
                btnOK.Text = Resource.Admin_m_Coupon_Add;
                txtCode.Text = Strings.GetRandomString(8);
                txtValue.Text = "0";
                txtPosibleUses.Text = "0";
                txtExpirationDate.Text = string.Empty;
                txtMinimalOrderPrice.Text = "0";
                chkExpirationDate.Checked = true;
                chkPosibleUses.Checked = true;
                chkEnabled.Checked = true;
                lProducts.Text = Resource.Admin_Coupon_AllProducts;
                lCategories.Text = Resource.Admin_Coupon_AllCategories;
                ddlCouponType.SelectedValue = "2";
            }
        }
        else
        {
            popTree.SelectedCategoriesIds = (List<int>)ViewState["SelectedCategories"];
        }
    }

    private bool ValidateData()
    {
        //
        // Validation
        //
        MsgErr(true);

        if (!String.IsNullOrEmpty(txtExpirationDate.Text))
        {
            try
            {
                Convert.ToDateTime(txtExpirationDate.Text);
            }
            catch (Exception ex)
            {
                MsgErr(Resource.Admin_m_Coupon_WrongDateFormat + " - " + ex.Message);
                return false;
            }
        }

        if (string.IsNullOrEmpty(txtValue.Text))
        {
            MsgErr(Resource.Admin_m_Coupon_NoValue);
            return false;
        }


        if (string.IsNullOrEmpty(txtMinimalOrderPrice.Text))
        {
            MsgErr(Resource.Admin_m_Coupon_NoMinimalOrderPrice);
            return false;
        }


        try
        {
            decimal dec = Decimal.Parse(txtValue.Text.Replace(".", ","));
            dec = Decimal.Parse(txtMinimalOrderPrice.Text.Replace(".", ","));
            int uses = int.Parse(txtPosibleUses.Text);
        }
        catch (Exception)
        {
            MsgErr(Resource.Admin_m_Coupon_WrongNumberFormat);
            return false;
        }


        try
        {
            decimal val = Decimal.Parse(txtValue.Text.Replace(".", ","));
            if (val <= 0)
            {
                MsgErr(Resource.Admin_m_Coupon_WrongValue);
                return false;
            }

            if ((CouponType)Enum.Parse(typeof(CouponType), ddlCouponType.SelectedValue) == CouponType.Percent && val > 100)
            {
                MsgErr(Resource.Admin_m_Coupon_ValueCantBeLager100);
                return false;
            }
        }
        catch (Exception)
        {
            MsgErr(Resource.Admin_m_Coupon_WrongValue);
            return false;
        }


        MsgErr(true);
        return true;
    }

    protected void SaveCoupon()
    {

        if (ValidateData())
        {

            try
            {
                Coupon coupon = CouponService.GetCoupon(CouponId);
                coupon.Value = decimal.Parse(txtValue.Text.Replace(".", ","));
                coupon.Type = (CouponType)Enum.Parse(typeof(CouponType), ddlCouponType.SelectedValue);
                coupon.ExpirationDate = txtExpirationDate.Text.IsNotEmpty() ? (DateTime?)DateTime.Parse(txtExpirationDate.Text).AddHours(23).AddMinutes(59).AddSeconds(59) : null;
                coupon.PossibleUses = int.Parse(txtPosibleUses.Text);
                coupon.MinimalOrderPrice = decimal.Parse(txtMinimalOrderPrice.Text.Replace(".", ","));
                coupon.Enabled = chkEnabled.Checked; 
                CouponService.UpdateCoupon(coupon);

                CouponService.DeleteAllCategoriesFromCoupon(CouponId);
                if (ViewState["SelectedCategories"] != null)
                {
                    List<int> list = (List<int>)ViewState["SelectedCategories"];
                    foreach (var catID in list)
                    {
                        CouponService.AddCategoryToCoupon(CouponId, catID);
                    }
                }


                CouponService.DeleteAllProductsFromCoupon(CouponId);
                List<int> listproducts = (List<int>)ViewState["SelectedProducts"];
                if (listproducts != null)
                {
                    foreach (var productid in listproducts)
                    {
                        CouponService.AddProductToCoupon(coupon.CouponID, productid);
                    }
                }

            }
            catch (Exception ex)
            {
                MsgErr(ex.Message + " SaveCoupon");
                Debug.LogError(ex);
            }
        }
    }


    protected void CreateCoupon()
    {
        if (ValidateData())
        {
            try
            {
                var coupon = new Coupon()
                                 {
                                     Code = txtCode.Text,
                                     Enabled = chkEnabled.Checked,
                                     Type = (CouponType)Enum.Parse(typeof(CouponType), ddlCouponType.SelectedValue),
                                     Value = decimal.Parse(txtValue.Text.Replace(".", ",")),
                                     ExpirationDate =
                                         txtExpirationDate.Text.IsNotEmpty()
                                             ? (DateTime?)DateTime.Parse(txtExpirationDate.Text).AddHours(23).AddMinutes(59).AddSeconds(59)
                                             : null,
                                     PossibleUses = int.Parse(txtPosibleUses.Text),
                                     MinimalOrderPrice = decimal.Parse(txtMinimalOrderPrice.Text.Replace(".", ",")),
                                     ActualUses = 0,
                                     AddingDate = DateTime.Now,
                                 };

                CouponService.AddCoupon(coupon);


                if (ViewState["SelectedCategories"] != null)
                {
                    List<int> list = (List<int>)ViewState["SelectedCategories"];
                    foreach (var catID in list)
                    {
                        CouponService.AddCategoryToCoupon(coupon.CouponID, catID);
                    }
                }

                List<int> listproducts = (List<int>)ViewState["SelectedProducts"];
                if (listproducts != null)
                {
                    foreach (var productid in listproducts)
                    {
                        CouponService.AddProductToCoupon(coupon.CouponID, productid);
                    }
                }
            }
            catch (Exception ex)
            {
                MsgErr(ex.Message + " CreateCoupon main");
                Debug.LogError(ex);
            }
        }
    }

    protected void LoadCouponById(int CouponId)
    {
        Coupon coupon = CouponService.GetCoupon(CouponId);
        if (coupon == null)
        {
            MsgErr("Coupon with this ID not exist");
            return;
        }

        txtValue.Text = CatalogService.FormatPriceInvariant(coupon.Value);
        txtExpirationDate.Text = coupon.ExpirationDate != null ? ((DateTime)coupon.ExpirationDate).ToString(SettingsMain.ShortDateFormat) : "";
        chkExpirationDate.Checked = coupon.ExpirationDate == null;
        txtMinimalOrderPrice.Text = CatalogService.FormatPriceInvariant(coupon.MinimalOrderPrice);
        txtPosibleUses.Text = coupon.PossibleUses.ToString();
        chkPosibleUses.Checked = coupon.PossibleUses == 0;
          
        lblAddingDate.Text = coupon.AddingDate.ToString(SettingsMain.ShortDateFormat);
        trAddingDate.Visible = true;
        txtCode.Text = coupon.Code;

        ddlCouponType.SelectedValue = ((int)coupon.Type).ToString();
        chkEnabled.Checked = coupon.Enabled;


        var categories = CouponService.GetCategoriesIDsByCoupon(CouponId);
        
        ViewState["SelectedCategories"] = categories;
        popTree.SelectedCategoriesIds = categories;

        var products = CouponService.GetProductsIDsByCoupon(CouponId);
        PopupGridProductMultiSelect.SelectedProducts = products;
        ViewState["SelectedProducts"] = products;

        if (!products.Any() && !categories.Any())
        {
            lProducts.Text = Resource.Admin_Coupon_AllProducts;
            lCategories.Text = Resource.Admin_Coupon_AllCategories;
        }
        else
        {
            lProducts.Text = products.Count() + " " + Resource.Admin_Coupon_Products;
            lCategories.Text = categories.Count +" " + Resource.Admin_Coupon_Categories;
        }


    }


    protected void popTree_Selected(object sender, Admin_UserControls_PopupTreeView.TreeNodeSelectedArgs args)
    {
        ViewState["SelectedCategories"] = args.SelectedValues.Select(val => int.Parse(val)).ToList();
        lCategories.Text = args.SelectedValues.Count() + " " + Resources.Resource.Admin_Coupon_Categories;
    }

    protected void grid_Selected(object sender, Admin_UserControls_PopupGridProductMultiSelect.GridSelectedArgs args)
    {
        ViewState["SelectedProducts"] = args.SelectedValues;
        lProducts.Text = args.SelectedValues.Count() + " " + Resources.Resource.Admin_Coupon_Products;
    }


    protected void lbAddCategory_Click(object sender, EventArgs e)
    {
        popTree.Show();
    }
}