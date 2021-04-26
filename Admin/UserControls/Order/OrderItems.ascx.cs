using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using AdvantShop;
using AdvantShop.Catalog;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.Orders;
using AdvantShop.Repository.Currencies;
using Resources;

public partial class Admin_UserControls_Order_OrderItems : System.Web.UI.UserControl
{
    public decimal CurrencyValue { get; set; }
    public Currency Currency { get; set; }
    public string CurrencyCode { get; set; }
    public string CurrencySymbol { get; set; }
    public int CurrencyNumCode { get; set; }
    public decimal OldCurrencyValue { get; set; }
    public bool IsCodeBefore { get; set; }

    public decimal OrderDiscount
    {
        get { return txtDiscount.Text.TryParseDecimal(); }
        set { txtDiscount.Text = CatalogService.FormatPriceInvariant(value); }
    }

    public decimal GroupDiscount
    {
        get { return ViewState["GroupDiscount"] == null ? 0 : (decimal)ViewState["GroupDiscount"]; }
        set { ViewState["GroupDiscount"] = value; }
    }

    private List<OrderItem> _orderItems;

    public List<OrderItem> OrderItems
    {
        get { return _orderItems ?? (_orderItems = LoadOrderItems()); }
        set { _orderItems = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        lblError.Text = string.Empty;
        lblError.Visible = false;

        txtDiscount.Text = CatalogService.FormatPriceInvariant(OrderDiscount);
        if (!IsPostBack)
        {
            LoadProducts();
        }

        if (string.IsNullOrEmpty(CurrencyCode))
        {
            if (IsPostBack)
                UpdateCurrency();
            else
                LoadDefaultCurrency();
        }
    }

    private void LoadDefaultCurrency()
    {
        ddlCurrs.DataBind();
        UpdateCurrency();
    }

    public void SetCurrency(string currencyCode, decimal currencyValue, int currencyNumCode, string currencySymbol, bool isCodeBefore)
    {
        ddlCurrs.DataBind();
        ddlCurrs.SelectedValue = currencyCode;
        lcurrency.Text = ddlCurrs.SelectedItem.Text;
        hfOldCurrencyValue.Value = currencyValue.ToString();
        OldCurrencyValue = currencyValue;
        CurrencyCode = currencyCode;
        CurrencyNumCode = currencyNumCode;
        CurrencySymbol = currencySymbol;
        Currency = CurrencyService.Currency(currencyCode);
        CurrencyValue = currencyValue;
        IsCodeBefore = isCodeBefore;

    }

    private List<OrderItem> LoadOrderItems()
    {
        return ViewState["OrderItems"] == null
                   ? new List<OrderItem>()
                   : ((OrderItem[])ViewState["OrderItems"]).ToList();
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        SaveOrderItems();
    }

    private void SaveOrderItems()
    {
        ViewState["OrderItems"] = OrderItems.ToArray();
    }

    private void LoadProducts()
    {
        DataListOrderItems.DataSource = OrderItems;
        DataListOrderItems.DataBind();
    }

    private void UpdateCurrency()
    {
        var cur = CurrencyService.Currency(ddlCurrs.SelectedValue);
        CurrencyValue = cur.Value;
        CurrencyCode = cur.Iso3;
        CurrencySymbol = cur.Symbol;
        CurrencyNumCode = cur.NumIso3;
        IsCodeBefore = cur.IsCodeBefore;
        Currency = cur;
        OldCurrencyValue = !string.IsNullOrEmpty(hfOldCurrencyValue.Value) ? Convert.ToDecimal(hfOldCurrencyValue.Value) : CurrencyValue;
        hfOldCurrencyValue.Value = cur.Value.ToString();
    }

    protected void ddlCurrs_SelectedChanged(object sender, EventArgs e)
    {
        LoadProducts();
        ItemsUpdated(this, new EventArgs());
    }

    protected void sds_Init(object sender, EventArgs e)
    {
        ((SqlDataSource)sender).ConnectionString = Connection.GetConnectionString();
    }

    protected void btnAddProduct_Click(object sender, EventArgs e)
    {
        pTreeProduct.Show();
    }

    protected void dlItems_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        //Added By Evgeni to change product single price SaveSinglePrice
        if (e.CommandName == "SaveSinglePrice")
        {
            try
            {
                decimal singleprice = 0;
                if (decimal.TryParse(((TextBox)e.Item.FindControl("txtSinglePrice")).Text.Replace(CurrencyCode,""), out singleprice))
                {
                    if (singleprice > 0)
                        OrderItems.Find(oi => oi.Id == Convert.ToInt32(e.CommandArgument)).Price = singleprice;
                    else
                        OrderItems.RemoveAll(oi => oi.Id == Convert.ToInt32(e.CommandArgument));
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                lblError.Text = ex.Message;
                lblError.Visible = true;
            }
            LoadProducts();
            ItemsUpdated(this, new EventArgs());
        }

        //
        if (e.CommandName == "SaveQuantity")
        {
            try
            {
                int quantity = 0;
                if (int.TryParse(((TextBox)e.Item.FindControl("txtQuantity")).Text, out quantity))
                {
                    if (quantity > 0)
                        OrderItems.Find(oi => oi.Id == Convert.ToInt32(e.CommandArgument)).Amount = quantity;
                    else
                        OrderItems.RemoveAll(oi => oi.Id == Convert.ToInt32(e.CommandArgument));
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                lblError.Text = ex.Message;
                lblError.Visible = true;
            }
            LoadProducts();
            ItemsUpdated(this, new EventArgs());
        }
        if (e.CommandName == "Delete")
        {
            try
            {
                OrderItems.RemoveAll(oi => oi.Id == Convert.ToInt32(e.CommandArgument));
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                lblError.Text = ex.Message;
                lblError.Visible = true;
            }

            LoadProducts();
            ItemsUpdated(this, new EventArgs());
        }
    }

    protected string RenderSelectedOptions(IList<EvaluatedCustomOptions> evlist)
    {
        var html = new StringBuilder();
        html.Append("<ul>");

        foreach (EvaluatedCustomOptions ev in evlist)
        {
            html.Append(string.Format("<li>{0}: {1} - {2}</li>", ev.CustomOptionTitle, ev.OptionTitle, CatalogService.GetStringPrice(ev.OptionPriceBc, Currency)));
        }

        html.Append("</ul>");
        return html.ToString();
    }

    protected void pTreeProduct_NodeSelected(object sender, Admin_UserControls_PopupTreeView.TreeNodeSelectedArgs args)
    {

        var items = new List<OrderItem>();
        foreach (var val in args.SelectedValues)
        {
            int treeSelectedValue;
            int.TryParse(val, out treeSelectedValue);
            var product = ProductService.GetProduct(Convert.ToInt32(treeSelectedValue));
            items.Add(new OrderItem
                          {
                              Name = product.Name,
                              Price = product.Price * (100 - Math.Max(product.Discount, GroupDiscount)) / 100,
                              SupplyPrice = product.Offers[0].SupplyPrice,
                              EntityId = product.ProductId,
                              Amount = 1,
                              ArtNo = product.ArtNo,
                              ItemType = EnumItemType.Product
                          });
        }

        if (OrderItems == null)
            OrderItems = new List<OrderItem>();

        foreach (var item in items)
        {
            item.Id = GenItemId();
            item.SelectedOptions = new List<EvaluatedCustomOptions>();
            if (OrderItems.Contains(item))
            {
                OrderItems[OrderItems.IndexOf(item)].Amount += 1;
            }
            else
            {
                OrderItems.Add(item);
            }
        }
        LoadProducts();
        ItemsUpdated(this, new EventArgs());
        pTreeProduct.UnSelectAll();
        pTreeProduct.Hide();
    }
    private int GenItemId()
    {
        if (OrderItems.Count == 0)
            return 1;
        return OrderItems.Max(oi => oi.Id) + 1;
    }
    protected void pTreeProduct_Hiding(object sender, EventArgs args)
    {
        ItemsUpdated(this, new EventArgs());
    }

    public event Action<object, EventArgs> ItemsUpdated;

    protected void dlItems_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {

        var item = e.Item;
        if ((item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem) && ((HiddenField)item.FindControl("hfItemType")).Value == EnumItemType.Product.ToString())
        {
            var strQuantity = ((TextBox)item.FindControl("txtQuantity")).Text;
            int quantity = 0;
            if (int.TryParse(strQuantity, out quantity))
            {
                var product = ProductService.GetProduct(((Literal)item.FindControl("ltArtNo")).Text);
                if ((product == null) || (quantity > product.Offers.First().Amount))
                {
                    var avail = product == null ? 0 : product.Offers.First().Amount;
                    ((Label)(item.FindControl("lbMaxCount"))).Text = string.Format("{0}: {1} {2}",
                                                                                    Resource.
                                                                                        Client_ShoppingCart_NotAvailable,
                                                                                    avail,
                                                                                    Resource.
                                                                                        Client_ShoppingCart_NotAvailable_End);
                    ((Label)(item.FindControl("lbMaxCount"))).ForeColor = Color.Red;
                }
                else if (quantity <= product.Offers.First().Amount)
                {
                    ((Label)(item.FindControl("lbMaxCount"))).Text = Resource.Client_ShoppingCart_Available;
                    ((Label)(item.FindControl("lbMaxCount"))).ForeColor = Color.Green;
                }
            }
        }
    }

    protected string RenderPicture(int productId)
    {
        var p = ProductService.GetProduct(productId);
        if (p == null || string.IsNullOrEmpty(p.Photo))
        {
            return string.Format("<img src='{0}' alt=\"\"/>", AdvantShop.Core.UrlRewriter.UrlService.GetAbsoluteLink("images/nophoto_small.jpg"));
        }

        return String.Format("<img src='{0}'/>", FoldersHelper.GetImageProductPath(ProductImageType.Small, p.Photo, true));
    }

    public void SetCustomerDiscount(Guid customerId)
    {
        var groupId = CustomerService.GetCustomerGroupId(customerId);
        var group = CustomerGroupService.GetCustomerGroup(groupId);

        if (group.CustomerGroupId != 0)
        {
            GroupDiscount = group.GroupDiscount;
        }
    }

    public void SetCustomerDiscount(string customerId)
    {
        SetCustomerDiscount(new Guid(customerId));
    }
}