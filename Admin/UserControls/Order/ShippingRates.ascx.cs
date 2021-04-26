using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AdvantShop;
using AdvantShop.Catalog;
using AdvantShop.Orders;
using AdvantShop.Repository.Currencies;
using AdvantShop.Shipping;

public partial class Admin_UserControls_ShippingRates : UserControl
{
    private const string PefiksId = "RadioShipRate_";

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
        get { return ShippingManager.CurrentShippingRates.Find(sm => sm.Id.ToString() == SelectedID).MethodId; }
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
        GenerateShippingRates();
    }

    public void LoadMethods(string selectedId)
    {
        CalculateShippingRates();

        SelectedID = selectedId;

        GenerateShippingRates();
    }

    private void GenerateShippingRates()
    {
        if ((_shippingRates != null) && (_shippingRates.Count != 0))
        {
            RadioList.Visible = true;
            int id = 1;
            string shippingRateGroup = string.Empty;

            var table = new HtmlTable();
            table.Style.Add(HtmlTextWriterStyle.Width, "100%");
            foreach (var shippingListItem in _shippingRates)
            {
                if (shippingRateGroup != shippingListItem.MethodName && shippingListItem.Id != 1)
                {
                    if (id != 1)
                    {
                        table.Controls.Add(GetTableLabel("<br />", id++));
                    }

                    shippingRateGroup = shippingListItem.MethodName;
                    table.Controls.Add(GetTableLabel(shippingRateGroup, id++));
                }

                table.Controls.Add(GetTableRadioButton(shippingListItem));
            }

            RadioList.Controls.Add(table);
        }
        else
        {
            lblNoShipping.Visible = true;
        }
    }

    private Control GetTableRadioButton(ShippingListItem shippingListItem)
    {
        var tr = new HtmlTableRow();
        var td = new HtmlTableCell();

        if (divScripts.Visible == false)
            divScripts.Visible = shippingListItem.Ext != null && shippingListItem.Ext.Type == ExtendedType.Pickpoint;

        var radioButton = new RadioButton
                              {
                                  GroupName = "ShippingRateGroup",
                                  ID = PefiksId + shippingListItem.Id //+ "|" + shippingListItem.Rate
                              };
        if (String.IsNullOrEmpty(_selectedID.Value.Replace(PefiksId, string.Empty)))
        {
            _selectedID.Value = radioButton.ID;
        }

        radioButton.Checked = radioButton.ID == _selectedID.Value;
        radioButton.Attributes.Add("onclick", "setValue(this)");

        string strShippingPrice = CatalogService.GetStringPrice(shippingListItem.Rate,
                                                                Currency.Value,
                                                                Currency.Iso3);
        radioButton.Text = string.Format("{0} <span class='price'>{1}</span>",
                                         shippingListItem.MethodNameRate, strShippingPrice);

        if (shippingListItem.Ext != null && shippingListItem.Ext.Type == ExtendedType.Pickpoint)
        {
            string temp;
            if (shippingListItem.Ext.Pickpointmap.IsNotEmpty())
                temp = string.Format(",{{city:'{0}', ids:null}}", shippingListItem.Ext.Pickpointmap);
            else
                temp = string.Empty;
            radioButton.Text +=
                string.Format(
                    "<br/><div id=\"address\">{0}</div><a href=\"#\" onclick=\"PickPoint.open(SetPickPointAnswer{1});return false\">" +
                    "{2}</a><input type=\"hidden\" name=\"pickpoint_id\" id=\"pickpoint_id\" value=\"\" /><br />",
                    pickAddress.Value, temp, Resources.Resource.Client_OrderConfirmation_Select);
        }
        using (var img = new Image { ImageUrl = ShippingIcons.GetShippingIcon(shippingListItem.Type, shippingListItem.IconName, shippingListItem.MethodNameRate) })
        {
            td.Controls.Add(img);
        }

        td.Controls.Add(radioButton);
        tr.Controls.Add(td);

        return tr;
    }

    private static Control GetTableLabel(string name, int id)
    {
        var tr = new HtmlTableRow();
        var td = new HtmlTableCell();

        var label = new Label { Text = @"<b>" + name + @"</b>", ID = "Label" + id };

        td.Controls.Add(label);
        tr.Controls.Add(td);

        return tr;
    }
    #endregion
}