using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop;
using AdvantShop.Shipping;
using AdvantShop.Configuration;

public partial class Admin_ShippingMethod : System.Web.UI.Page
{
    private int _shippingMethodID;
    protected int ShippingMethodID
    {
        get
        {
            if (_shippingMethodID != 0)
                return _shippingMethodID;
            var intval = 0;
            int.TryParse(Request["shippingmethodid"], out intval);
            return intval;
        }
        set
        {
            _shippingMethodID = value;
        }
    }

    protected void Msg(string message)
    {
        lblMessage.Text = message;
        lblMessage.Visible = true;
    }

    protected void ClearMsg()
    {
        lblMessage.Visible = false;
    }

    protected static readonly Dictionary<ShippingType, string> UcIds = new Dictionary<ShippingType, string>
                                                                          {
                                                                              {ShippingType.FedEx, "ucFeedEx"},
                                                                              {ShippingType.Usps, "ucUsps"},
                                                                              {ShippingType.Ups, "ucUPS"},
                                                                              {ShippingType.FixedRate, "ucFixedRate"},
                                                                              {ShippingType.FreeShipping, "ucFreeShipping"},
                                                                              {ShippingType.ShippingByWeight, "ucByWeight"},
                                                                              {ShippingType.eDost, "ucEdost"},
                                                                              {ShippingType.ShippingByShippingCost, "ucByShippingCost"},
                                                                              {ShippingType.ShippingByOrderPrice, "ucByOrderPrice"},
                                                                          };


    protected override void InitializeCulture()
    {
        AdvantShop.Localization.Culture.InitializeCulture();
    }
    protected void Page_Init(object sender, EventArgs e)
    {

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = string.Format("{0} - {1}", SettingsMain.ShopName, Resources.Resource.Admin_ShippingMethod_Header);

        ClearMsg();
        if (!IsPostBack)
            LoadMethods();
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        ddlType.DataSource = AdvantShop.Core.AdvantshopConfigService.GetDropdownShippings();
        ddlType.DataBind();
    }

    protected void LoadMethods()
    {
        var methods = ShippingMethodService.GetAllShippingMethods().Where(item => item.ShippingMethodId != 1).ToList();
        if (methods.Count > 0)
        {
            if (ShippingMethodID == 0)
                ShippingMethodID = methods.First().ShippingMethodId;
            rptTabs.DataSource = methods;
            rptTabs.DataBind();

        }
        ShowMethod(ShippingMethodID);
    }

    protected void ShowMethod(int methodID)
    {
        var method = ShippingMethodService.GetShippingMethod(methodID);
        foreach (var ucId in UcIds)
        {
            var uc = (Admin_UserControls_ShippingMethods_MasterControl)pnMethods.FindControl(ucId.Value);
            if (method == null)
            {
                uc.Visible = false;
                continue;
            }
            if (ucId.Key == method.Type)
                uc.Method = method;
            uc.Visible = ucId.Key == method.Type;
        }
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        var type = (ShippingType)int.Parse(ddlType.SelectedValue);
        var method = new ShippingMethod
                         {
                             Type = type,
                             Name = txtName.Text,
                             Description = txtDescription.Text,
                             Enabled = type == ShippingType.FreeShipping,
                             SortOrder = txtSortOrder.Text.TryParseInt()
                         };
        var id = ShippingMethodService.InsertShippingMethod(method);
        if (id != 0)
            Response.Redirect("~/Admin/ShippingMethod.aspx?ShippingMethodID=" + id);
    }

    protected void ShippingMethod_Saved(object sender, Admin_UserControls_ShippingMethods_MasterControl.SavedEventArgs args)
    {
        LoadMethods();
        Msg(string.Format(Resources.Resource.Admin_ShippingMethod_Saved, args.Name));
    }
}