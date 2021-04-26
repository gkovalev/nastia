using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using AdvantShop;
using AdvantShop.Core;
using AdvantShop.Core.FieldFilters;
using AdvantShop.Diagnostics;
using AdvantShop.Orders;

public partial class UserControls_OrdersSearch : System.Web.UI.UserControl
{
    const int RowsPerPage = 30;
    protected SqlPaging Paging
    {
        get
        {
            if (ViewState["Paging"] != null)
                return (SqlPaging)ViewState["Paging"];
            return null;
        }
        set
        {
            ViewState["Paging"] = value;
        }
    }

    public int SelectedOrder
    {
        get
        {
            if (ViewState["SelectedOrder"] != null)
                return (int)ViewState["SelectedOrder"];
            return 0;
        }
        set { ViewState["SelectedOrder"] = value; }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        FilterList();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["statusFilters"] = OrderService.GetOrderStatuses(false).Keys.ToList<string>();
            var paging = new SqlPaging
                             {
                                 TableName =
                                     "[Order].[Order] left join [Order].[OrderCustomer] on [Order].[OrderID] = [OrderCustomer].[OrderID] inner join [Order].[OrderCurrency] on [Order].[OrderID] = [OrderCurrency].[OrderID]"
                             };

            var f = new Field { Name = "[Order].[OrderID]" };
            paging.AddField(f);
            f = new Field { Name = "LastName + ' ' +  FirstName as CustomerName" };
            paging.AddField(f);
            f = new Field { Name = "OrderStatusID", Sorting = SortDirection.Ascending };
            paging.AddField(f);
            f = new Field { Name = "OrderDate", Sorting = SortDirection.Descending };
            paging.AddField(f);

            f = new Field { Name = "Sum" };
            paging.AddField(f);
            f = new Field { Name = "TaxCost" };
            paging.AddField(f);
            f = new Field { Name = "ShippingCost" };
            paging.AddField(f);
            f = new Field { Name = "OrderDiscount" };
            paging.AddField(f);
            f = new Field { Name = "CurrencyCode" };
            paging.AddField(f);
            f = new Field { Name = "CurrencyValue" };
            paging.AddField(f);

            paging.ItemsPerPage = RowsPerPage;
            paging.CurrentPageIndex = 1;
            lbPagePrev.Enabled = false;
            var pageCount = paging.PageCount;
            lbPageNext.Enabled = paging.CurrentPageIndex != pageCount;
            for (int i = 1; i <= pageCount; i++)
            {
                ddlPage.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }

            Paging = paging;
        }
        else
        {
            CalendarExtender1.SelectedDate = null;
            CalendarExtender2.SelectedDate = null;
        }
    }

    public void FilterList()
    {
        if (!IsPostBack)
            rptStatuses.DataBind();
        var statusFilters = (from RepeaterItem item in rptStatuses.Items
                             select (CheckBox)item.FindControl("CheckBox1")
                                 into cb
                                 where cb.Checked
                                 select cb.Attributes["Value"]).ToList();

        if (statusFilters.Count() != 0)
        {
            String statusFilter = statusFilters.Aggregate("(", (current, status) => current + ("'" + status + "',"));
            statusFilter = statusFilter.Remove(statusFilter.LastIndexOf(','));
            statusFilter += ")";
            sdsStatusesFiltered.FilterParameters[0].DefaultValue = statusFilter;
            Paging.Fields["OrderStatusID"].Filter = new InSetFieldFilter { IncludeValues = true, ParamName = "@OrderStatusID", Values = statusFilters.ToArray() };
        }
        else
            sdsStatusesFiltered.FilterParameters[0].DefaultValue = "(0)";
        try
        {
            DateTime dateFrom;
            if (!DateTime.TryParse(txtDateFrom.Text, out dateFrom))
                dateFrom = DateTime.Parse("01/01/1800");
            DateTime dateTo;
            if (!DateTime.TryParse(txtDateTo.Text, out dateTo))
                dateTo = DateTime.Parse("01/01/3000");
            //subtraction and addition made to the boundary dates were included in the search
            Paging.Fields["OrderDate"].Filter = new DateTimeRangeFieldFilter { From = dateFrom.AddDays(-1), To = dateTo.AddDays(1), ParamName = "@OrderDate" };
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
        UpdatePager();
        rptOrderBlocks.DataBind();
        tblPager.Visible = Paging.PageCount >= 2;
    }

    protected void sdsStatuses_Init(object sender, EventArgs e)
    {
        sdsStatuses.ConnectionString = Connection.GetConnectionString();
    }

    protected void sds_Init(object sender, EventArgs e)
    {
        ((SqlDataSource)sender).ConnectionString = Connection.GetConnectionString();
    }

    protected void rptOrderBlocks_ItemBound(object sender, RepeaterItemEventArgs e)
    {
        DataTable table = Paging.PageItems.Clone();
        foreach (DataRow row in Paging.PageItems.Select("OrderStatusID = " + ((HiddenField)e.Item.FindControl("hfStatusID")).Value))
        {
            table.ImportRow(row);
        }
        ((Repeater)e.Item.FindControl("rptOrders")).DataSource = table;
        if (table.Rows.Count > 0)
            ((Repeater)e.Item.FindControl("rptOrders")).DataBind();
        e.Item.FindControl("pnStatusBlock").Visible = (table.Rows.Count > 0);
    }

    protected void rptOrderBlocks_DataBinding(object sender, EventArgs e)
    {

    }

    protected void lbPagePrev_Click(object sender, EventArgs e)
    {
        Paging.CurrentPageIndex--;

    }

    protected void lbPageNext_Click(object sender, EventArgs e)
    {
        Paging.CurrentPageIndex++;

    }

    protected void ddlPage_Selected(object sender, EventArgs e)
    {
        Paging.CurrentPageIndex = Convert.ToInt32(ddlPage.SelectedValue);

    }

    protected void UpdatePager()
    {
        var pageCount = Paging.PageCount;
        lbPageNext.Enabled = Paging.CurrentPageIndex < pageCount;
        lbPagePrev.Enabled = Paging.CurrentPageIndex > 1;
        ddlPage.Items.Clear();
        if (pageCount == 0)
        {
            ddlPage.Enabled = false;
        }
        else
        {
            ddlPage.Enabled = true;
            for (int i = 1; i <= pageCount; i++)
            {
                ddlPage.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
            if (Paging.CurrentPageIndex > pageCount)
                Paging.CurrentPageIndex = pageCount;
            ddlPage.SelectedValue = Paging.CurrentPageIndex.ToString();
        }
    }
    public event EventHandler<SelectOrderArgs> SelectOrder;

    protected void btnFilterClick(object sender, EventArgs e)
    {
        // FilterList();
    }
}

public class SelectOrderArgs : EventArgs
{
    private string _orderID;

    public SelectOrderArgs(string orderID)
    {
        this._orderID = orderID;
    }
    public string OrderID
    {
        get { return _orderID; }
        set { _orderID = value; }
    }
}