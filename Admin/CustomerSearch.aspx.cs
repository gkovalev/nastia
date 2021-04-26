using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop.Configuration;
using AdvantShop.Core;
using AdvantShop.Core.FieldFilters;
using AdvantShop.Customers;
using Resources;
using AdvantShop;

//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------


public partial class CustomerSearch : Page
{
    private bool _inverseSelection;
    private SqlPaging _paging;
    private InSetFieldFilter _selectionFilter;

    private List<Pair> _customerGroupList;
    private List<Pair> CustomerGroupList
    {
        get
        {
            if (_customerGroupList == null)
            {
                _customerGroupList = new List<Pair>();
                foreach (var group in CustomerGroupService.GetCustomerGroupList())
                {
                    _customerGroupList.Add(new Pair(string.Format("{0} - {1}%", group.GroupName, group.GroupDiscount), group.CustomerGroupId.ToString()));
                }
            }

            return _customerGroupList;
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
            Message.Visible = false;
            Message.Text = "";
        }
        else
        {
            Message.Visible = false;
        }
    }

    private void MsgErr(string messageText)
    {
        Message.Visible = true;
        Message.Text = "<br/>" + messageText;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        MsgErr(true);

        Page.Title = string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_CustomerSearch_SubHeader);

        if (!IsPostBack)
        {
            _paging = new SqlPaging { TableName = "[Customers].[Customer]", ItemsPerPage = 10 };

            var f = new Field { Name = "CustomerID as ID", IsDistinct = true };


            _paging.AddField(f);

            f = new Field { Name = "Email" };

            _paging.AddField(f);

            f = new Field { Name = "Firstname", Sorting = SortDirection.Ascending };
            _paging.AddField(f);

            f = new Field { Name = "Lastname" };
            _paging.AddField(f);

            f = new Field { Name = "CustomerGroupId" };
            _paging.AddField(f);

            //Changed By Evgeni
            f = new Field { Name = "RegistrationDateTime", Sorting = SortDirection.Descending };
            _paging.AddField(f);

            f = new Field { Name = "CustomerRole" };
            _paging.AddField(f);

            if (CustomerSession.CurrentCustomer.CustomerRole == Role.Moderator)
            {
                _paging.Fields["CustomerRole"].Filter = new EqualFieldFilter { ParamName = "@CustomerRole", Value = ((int)Role.User).ToString() };
            }

            //Changed by Evgeni
            advCustomers.ChangeHeaderImageUrl("arrowRegistrationDateTime", "images/arrowdown.gif");

            _paging.ItemsPerPage = 10;

            pageNumberer.CurrentPageIndex = 1;
            _paging.CurrentPageIndex = 1;
            ViewState["Paging"] = _paging;
        }
        else
        {
            _paging = (SqlPaging)(ViewState["Paging"]);
            _paging.ItemsPerPage = Convert.ToInt32(ddRowsPerPage.SelectedValue);

            if (_paging == null)
            {
                throw (new Exception("Paging lost"));
            }

            var strIds = Request.Form["SelectedIds"];

            if (!string.IsNullOrEmpty(strIds))
            {
                strIds = strIds.Trim();
                var arrids = strIds.Split(' ');

                var ids = new string[arrids.Length ];
                _selectionFilter = new InSetFieldFilter { IncludeValues = true };
                for (int idx = 0; idx <= ids.Length - 1; idx++)
                {
                    var t = arrids[idx];
                    if (t != "-1")
                    {
                        ids[idx] = t;
                    }
                    else
                    {
                        _selectionFilter.IncludeValues = false;
                        _inverseSelection = true;
                    }
                }

                _selectionFilter.Values = ids;
            }
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (advCustomers.UpdatedRow != null)
        {
            var customer = CustomerService.GetCustomer(new Guid(advCustomers.UpdatedRow["ID"]));
            customer.CustomerGroupId = Convert.ToInt32(advCustomers.UpdatedRow["CustomerGroup"]);
            CustomerService.UpdateCustomer(customer);
        }

        DataTable data = _paging.PageItems;
        while (data.Rows.Count < 1 && _paging.CurrentPageIndex > 1)
        {
            _paging.CurrentPageIndex--;
            data = _paging.PageItems;
        }

        var clmn = new DataColumn("IsSelected", typeof(bool)) { DefaultValue = _inverseSelection };
        data.Columns.Add(clmn);
        if ((_selectionFilter != null) && (_selectionFilter.Values != null))
        {
            for (int i = 0; i <= data.Rows.Count - 1; i++)
            {
                var intIndex = i;
                if (Array.Exists(_selectionFilter.Values, c => c == data.Rows[intIndex]["ID"].ToString()))
                {
                    data.Rows[i]["IsSelected"] = !_inverseSelection;
                }
            }
        }
        if (data.Rows.Count < 1)
        {
            goToPage.Visible = false;
        }
        advCustomers.DataSource = data;
        advCustomers.DataBind();
        pageNumberer.PageCount = _paging.PageCount;
        lblFound.Text = _paging.TotalRowsCount.ToString();
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        if (ddSelect.SelectedIndex != 0)
        {
            if (ddSelect.SelectedIndex == 2)
            {
                if (_selectionFilter != null)
                {
                    _selectionFilter.IncludeValues = !_selectionFilter.IncludeValues;
                }
                else
                {
                    _selectionFilter = null;
                }
            }
            _paging.Fields["ID"].Filter = _selectionFilter;
        }
        else
        {
            _paging.Fields["ID"].Filter = null;
        }

        //----Firstname filter
        if (!string.IsNullOrEmpty(txtSearchFirstName.Text))
        {
            var nfilter = new CompareFieldFilter { Expression = txtSearchFirstName.Text, ParamName = "@firstname" };
            _paging.Fields["Firstname"].Filter = nfilter;
        }
        else
        {
            _paging.Fields["Firstname"].Filter = null;
        }

        //----lastname filter
        if (!string.IsNullOrEmpty(txtSearchLastname.Text))
        {
            var nfilter = new CompareFieldFilter { Expression = txtSearchLastname.Text, ParamName = "@lastname" };
            _paging.Fields["Lastname"].Filter = nfilter;
        }
        else
        {
            _paging.Fields["Lastname"].Filter = null;
        }

        //----email filter
        if (!string.IsNullOrEmpty(txtSearchEmail.Text))
        {
            var nfilter = new CompareFieldFilter { Expression = txtSearchEmail.Text, ParamName = "@email" };
            _paging.Fields["Email"].Filter = nfilter;
        }
        else
        {
            _paging.Fields["Email"].Filter = null;
        }

        //----RegDate filter
        if (!string.IsNullOrEmpty(txtDateFrom.Text) || !string.IsNullOrEmpty(txtDateTo.Text))
        {
            var nfilter = new DateTimeRangeFieldFilter();
            try
            {
                nfilter.From = DateTime.Parse(txtDateFrom.Text);
            }
            catch (Exception)
            {
                nfilter.From = DateTime.Parse("01.01.1900");
            }

            try
            {
                nfilter.To = DateTime.Parse(txtDateTo.Text);
            }
            catch (Exception)
            {
                nfilter.To = DateTime.MaxValue;
            }
            nfilter.ParamName = "@RegistrationDateTime";
            _paging.Fields["RegistrationDateTime"].Filter = nfilter;
        }
        else
        {
            _paging.Fields["RegistrationDateTime"].Filter = null;
        }

        _paging.Fields["CustomerGroupId"].Filter = ddlCustomerGroup.SelectedValue != "0"
                                                        ? new EqualFieldFilter
                                                        {
                                                            ParamName = "@CustomerGroupId",
                                                            Value = ddlCustomerGroup.SelectedValue
                                                        }
                                                        : null;


        pageNumberer.CurrentPageIndex = 1;
        _paging.CurrentPageIndex = 1;
    }

    protected void agv_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "DeleteAll")
        {
            CustomerService.DeleteCustomer(Guid.Parse((string)e.CommandArgument));
        }
    }

    protected void lbDeleteSelected1_Click(object sender, EventArgs e)
    {
        if (!_inverseSelection)
        {
            if (_selectionFilter != null)
            {
                foreach (var customerID in _selectionFilter.Values)
                {
                    if (CustomerService.GetCustomer(customerID.TryParseGuid()).IsAdmin)
                    {
                        if (Message.Visible)
                        {
                            Message.Text = Message.Text + @"<br />" + Resource.Admin_CustomersSearch_ErrAdmin;
                        }
                        else
                        {
                            MsgErr(Resource.Admin_CustomersSearch_ErrDeleting + @"<br />" +
                                   Resource.Admin_CustomersSearch_ErrAdmin);
                        }
                    }
                    else if (customerID.Equals(SettingsGeneral.QrUserId))
                    {
                        if (Message.Visible)
                        {
                            Message.Text = Message.Text + @"<br />" + Resource.Admin_CustomersSearch_ErrReg;
                        }
                        else
                        {
                            MsgErr(Resource.Admin_CustomersSearch_ErrDeleting + @"<br />" +
                                   Resource.Admin_CustomersSearch_ErrReg);
                        }
                    }
                    else
                    {
                        CustomerService.DeleteCustomer(Guid.Parse(customerID));
                    }
                }
            }
        }
        else
        {
            var itemsIds = _paging.ItemsIds("CustomerID as ID");
            foreach (var customerID in itemsIds.Where(certificateId => !_selectionFilter.Values.Contains(certificateId.ToString())))
            {
                if (!(CustomerService.GetCustomer(customerID).IsAdmin || customerID.Equals(SettingsGeneral.QrUserId)))
                {
                    CustomerService.DeleteCustomer(customerID);
                }
            }
        }
    }

    protected void lbChangeCustomerGroup_Click(object sender, EventArgs e)
    {
        if ((_selectionFilter != null) && (_selectionFilter.Values != null))
        {
            if (!_inverseSelection)
            {
                foreach (var id in _selectionFilter.Values)
                {
                    CustomerService.ChangeCustomerGroup(id, Convert.ToInt32(ddlChangeCustomerGroup.SelectedValue));
                }
            }
            else
            {
                var itemsIds = _paging.ItemsIds("CustomerID as ID");
                foreach (var id in itemsIds.Where(nId => !_selectionFilter.Values.Contains(nId.ToString())))
                {
                    CustomerService.ChangeCustomerGroup(id.ToString(), Convert.ToInt32(ddlChangeCustomerGroup.SelectedValue));
                }
            }
        }
    }

    protected void linkGO_Click(object sender, EventArgs e)
    {
        int pagen;
        try
        {
            pagen = int.Parse(txtPageNum.Text);
        }
        catch (Exception)
        {
            pagen = -1;
        }
        if (pagen < 1 || pagen > _paging.PageCount)
            return;
        pageNumberer.CurrentPageIndex = pagen;
        _paging.CurrentPageIndex = pagen;
    }

    protected void advCustomers_Sorting(object sender, GridViewSortEventArgs e)
    {
        var arrows = new Dictionary<string, string>
                         {
                             {"Lastname", "arrowLastname"},
                             {"Firstname", "arrowFirstname"},
                             {"Email", "arrowEmail"},
                             {"RegistrationDateTime", "arrowRegistrationDateTime"}
                         };

        const string urlArrowUp = "images/arrowup.gif";
        const string urlArrowDown = "images/arrowdown.gif";
        const string urlArrowGray = "images/arrowdownh.gif";

        var csf = (from Field f in _paging.Fields.Values where f.Sorting.HasValue select f).First();
        var nsf = _paging.Fields[e.SortExpression];

        if (nsf.Name.Equals(csf.Name))
        {
            csf.Sorting = csf.Sorting == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
            advCustomers.ChangeHeaderImageUrl(arrows[csf.Name],
                                              (csf.Sorting == SortDirection.Ascending ? urlArrowUp : urlArrowDown));
        }
        else
        {
            csf.Sorting = null;

            advCustomers.ChangeHeaderImageUrl(arrows[csf.Name], urlArrowGray);

            nsf.Sorting = SortDirection.Ascending;
            advCustomers.ChangeHeaderImageUrl(arrows[nsf.Name], urlArrowUp);
        }

        pageNumberer.CurrentPageIndex = 1;
        _paging.CurrentPageIndex = 1;
        UpdatePanel2.Update();
    }
    
    protected void DataListCustomers_ItemCommand(object source, DataListCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "ShowCustomerInfo":
                Response.Redirect("ViewCustomer.aspx?CustomerID=" + e.CommandArgument);
                break;
            case "DeleteCustomer":
                CustomerService.DeleteCustomer(Guid.Parse((string)e.CommandArgument));
                break;
        }
    }

    protected void pn_SelectedPageChanged(object sender, EventArgs e)
    {
        _paging.CurrentPageIndex = pageNumberer.CurrentPageIndex;
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        btnFilter_Click(sender, e);
        advCustomers.ChangeHeaderImageUrl(null, null);
    }

    protected void btnCreateCustomer_Click(object sender, EventArgs e)
    {
        Response.Redirect("CreateCustomer.aspx");
    }

    protected void sds_Init(object sender, EventArgs e)
    {
        ((SqlDataSource)sender).ConnectionString = Connection.GetConnectionString();
    }

    protected void ddlCustomerGroup_DataBound(object sender, EventArgs e)
    {
        ddlCustomerGroup.Items.Insert(0, new ListItem(Resource.Admin_Catalog_Any, "0"));
    }

    protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;

        var customerId = new Guid((((DataRowView)e.Row.DataItem)["ID"].ToString()));
        var customerGroup = CustomerService.GetCustomer(customerId).CustomerGroup;
        var dropDownList = (DropDownList)e.Row.FindControl("ddlCustomerGroup");

        foreach (var group in CustomerGroupList)
        {
            dropDownList.Items.Add(new ListItem(group.First.ToString(), group.Second.ToString()));
        }

        dropDownList.SelectedValue = customerGroup.CustomerGroupId.ToString();
    }


}