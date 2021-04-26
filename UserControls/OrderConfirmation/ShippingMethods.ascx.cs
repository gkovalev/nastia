using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.UI;
using AdvantShop.Orders;
using AdvantShop.Repository.Currencies;
using AdvantShop.Shipping;
using Resources;

public partial class Admin_UserControls_ShippingRates : UserControl
{
    protected const string PefiksId = "RadioShipRate_";

    private List<ShippingListItem> _shippingRates;
    public List<ShippingListItem> ShippingRates
    {
        get { return _shippingRates; }
    }

    public ShippingOptionEx SelectShippingOptionEx
    {
        get
        {
            var temp = ShippingManager.CurrentShippingRates.Find(sm => sm.Id.ToString() == SelectedID).Ext;
            if (temp != null && temp.Type == ExtendedType.Pickpoint)
            {
                temp.PickpointId = pickpointId.Value;
                temp.PickpointAddress = pickAddress.Value;
            }
            return temp;
        }
        set
        {
            if (value != null)
            {
                pickpointId.Value = value.PickpointId;
                pickAddress.Value = value.PickpointAddress;
            }
        }
    }

    public decimal SelectedRate
    {
        get { return ShippingManager.CurrentShippingRates.Find(sm => sm.Id.ToString() == SelectedID).Rate; }
    }

    public string SelectedName
    {
        get { return ShippingManager.CurrentShippingRates.Find(sm => sm.Id.ToString() == SelectedID).MethodNameRate; }
    }

    public int SelectedMethodID
    {
        get
        {
            if (ShippingManager.CurrentShippingRates.Count == 0)
            {
                Response.Redirect("~/orderconfirmation.aspx");
            }
            return ShippingManager.CurrentShippingRates.Find(sm => sm.Id.ToString() == SelectedID).MethodId;
        }
    }

    public string SelectedID
    {
        get { return _selectedID.Value.Replace(PefiksId, string.Empty); }
        set
        {
            if (_shippingRates.Count > 0)
            {
                if (_shippingRates.Find(s => s.Id.ToString() == value) != null)
                {
                    _selectedID.Value = PefiksId + value;
                }
                else
                {
                    _selectedID.Value = PefiksId + _shippingRates[0].Id.ToString();
                }
            }
        }
    }

    public int CountryId { get; set; }
    public string Zip { get; set; }
    public string City { get; set; }
    public string Region { get; set; }
    public ShoppingCart ShoppingCart { get; set; }

    private Currency _currency = CurrencyService.CurrentCurrency;
    public Currency Currency { get { return _currency; } set { if (value != null)_currency = value; } }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (_shippingRates == null)
            _shippingRates = new List<ShippingListItem>();
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
    }

    public void ClearRates()
    {
        RadioList.Controls.Clear();
        RadioList.Visible = false;
        ShippingManager.CurrentShippingRates = new List<ShippingListItem>();
        _shippingRates = new List<ShippingListItem>();
    }

    public void Update()
    {
        if (_shippingRates.Count > 0)
        {
            ClearRates();
            CalculateShippingRates();
        }
    }

    private void CalculateShippingRates()
    {
        if (_shippingRates == null) _shippingRates = new List<ShippingListItem>();
        var shippingManager = new ShippingManager { Currency = Currency };
        _shippingRates.AddRange(shippingManager.GetShippingRates(CountryId, Zip, City, Region, ShoppingCart));
        ShippingManager.CurrentShippingRates = _shippingRates;
    }

    #region private

    public void LoadMethods()
    {
        CalculateShippingRates();
    }

    public void LoadMethods(string selectedId)
    {
        CalculateShippingRates();
        SelectedID = selectedId;

        lvShippingRates.DataSource = _shippingRates;

        lvShippingRates.SelectedIndex = _shippingRates.FindIndex(p => p.MethodId == SelectedMethodID && p.Id.ToString() == SelectedID);
        lvShippingRates.DataBind();
    }

    protected string RenderPickPoint(ShippingOptionEx Ext)
    {
        
        var result = string.Empty;
        if (Ext != null && Ext.Type == ExtendedType.Pickpoint)
        {
            string temp;
            divScripts.Visible = true;
            if (!String.IsNullOrEmpty(Ext.Pickpointmap))
                temp = string.Format(",{{city:'{0}', ids:null}}", Ext.Pickpointmap);
            else
                temp = string.Empty;
            result +=
                string.Format(
                    "<br/><div id=\"address\">{0}</div><a href=\"javascript:void(0);\" class='pickpoint' onclick=\"PickPoint.open(SetPickPointAnswer{1});\">" +
                        "{2}</a><input type=\"hidden\" name=\"pickpoint_id\" id=\"pickpoint_id\" value=\"\" /><br />",
                    pickAddress.Value, temp, Resource.Client_OrderConfirmation_ShippingRates_Select);
        }
        return result;
    }


    #endregion
}