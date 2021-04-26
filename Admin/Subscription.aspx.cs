using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using AdvantShop.Core;
using AdvantShop.Core.FieldFilters;
using AdvantShop.Diagnostics;

//--------------------------------------------------
// Project: AdvantShop.NET                          
// Web site: http:\\www.advantshop.net              
//--------------------------------------------------


public partial class Admin_Subscription : System.Web.UI.Page
{
    SqlPaging _paging;
    InSetFieldFilter _selectionFilter;
    bool _inverseSelection;

    protected override void InitializeCulture()
    {
        AdvantShop.Localization.Culture.InitializeCulture();
    }

    public Admin_Subscription()
    {
        _inverseSelection = false;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = string.Format("{0} - {1}", AdvantShop.Configuration.SettingsMain.ShopName, Resources.Resource.Admin_Subscription_Header);
        if (!IsPostBack)
        {
            _paging = new SqlPaging { TableName = "[Customers].[Customer]", ItemsPerPage = 10 };

            var f = new Field { Name = "CustomerID as ID", IsDistinct = true, Filter = _selectionFilter };
            _paging.AddField(f);


            //f = new Field { Name = "Login", Sorting = SortDirection.Ascending };
            //_paging.AddField(f);

            f = new Field { Name = "FirstName" };
            _paging.AddField(f);

            f = new Field { Name = "LastName" };
            _paging.AddField(f);

            f = new Field { Name = "Subscribed4News" };
            _paging.AddField(f);

            f = new Field { Name = "Email", Sorting = SortDirection.Ascending };
            _paging.AddField(f);

            f = new Field { Name = "RegistrationDateTime" };
            _paging.AddField(f);

            grid.ChangeHeaderImageUrl("arrowEmail", "images/arrowup.gif");

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

            string strIds = Request.Form["SelectedIds"];

            if (!string.IsNullOrEmpty(strIds))
            {
                strIds = strIds.Trim();
                var arrids = strIds.Split(' ');

                var ids = new string[arrids.Length ];
                _selectionFilter = new InSetFieldFilter { IncludeValues = true };
                for (int idx = 0; idx <= ids.Length - 1; idx++)
                {
                    var t = arrids[idx];
                    if (t != "")
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

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        //-----Selection filter
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
            _paging.Fields["ID"].Filter = _selectionFilter;
        }


        //----FirstName filter
        if (!string.IsNullOrEmpty(txtFirstName.Text))
        {
            var nfilter = new CompareFieldFilter { Expression = txtFirstName.Text, ParamName = "@FirstName" };
            _paging.Fields["FirstName"].Filter = nfilter;
        }
        else
        {
            _paging.Fields["FirstName"].Filter = null;
        }

        //----Login filter
        //if (!string.IsNullOrEmpty(txtLogin.Text))
        //{
        //    var nfilter = new CompareFieldFilter { Expression = txtLogin.Text, ParamName = "@Login" };
        //    _paging.Fields["Login"].Filter = nfilter;
        //}
        //else
        //{
        //    _paging.Fields["Login"].Filter = null;
        //}


        //---LastName
        if (!string.IsNullOrEmpty(txtLastName.Text))
        {
            var nfilter = new CompareFieldFilter { Expression = txtLastName.Text, ParamName = "@LastName" };
            _paging.Fields["LastName"].Filter = nfilter;
        }
        else
        {
            _paging.Fields["LastName"].Filter = null;
        }

        //---Email Filter
        if (!string.IsNullOrEmpty(txtEmail.Text))
        {
            var nfilter = new CompareFieldFilter { Expression = txtEmail.Text, ParamName = "@Email" };
            _paging.Fields["Email"].Filter = nfilter;
        }
        else
        {
            _paging.Fields["Email"].Filter = null;
        }

        //----Subscribed4News
        if (subscribed4News.SelectedIndex != 0)
        {
            var beforeFilter = new EqualFieldFilter { ParamName = "@Subscribed4News" };
            if (subscribed4News.SelectedIndex == 1)
            {
                beforeFilter.Value = "1";
            }
            if (subscribed4News.SelectedIndex == 2)
            {
                beforeFilter.Value = "0";
            }
            _paging.Fields["Subscribed4News"].Filter = beforeFilter;
        }
        else
        {
            _paging.Fields["Subscribed4News"].Filter = null;
        }


        //---OrderDate filter
        var dfilter = new DateTimeRangeFieldFilter { ParamName = "@RegistrationDateTime" };
        if (!string.IsNullOrEmpty(txtDateFrom.Text))
        {
            DateTime? d = null;
            try
            {
                d = DateTime.Parse(txtDateFrom.Text);
            }
            catch (Exception)
            {
            }
            if (d.HasValue)
            {
                var dt = new DateTime(d.Value.Year, d.Value.Month, d.Value.Day, 0, 0, 0, 0);
                dfilter.From = dt;
            }
        }

        if (!string.IsNullOrEmpty(txtDateTo.Text))
        {
            DateTime? d = null;
            try
            {
                d = DateTime.Parse(txtDateTo.Text);
            }
            catch (Exception)
            {
            }
            if (d.HasValue)
            {
                var dt = new DateTime(d.Value.Year, d.Value.Month, d.Value.Day, 23, 59, 59, 99);
                dfilter.To = dt;
            }
        }

        if (dfilter.From.HasValue || dfilter.To.HasValue)
        {
            _paging.Fields["RegistrationDateTime"].Filter = dfilter;
        }
        else
        {
            _paging.Fields["RegistrationDateTime"].Filter = null;
        }

        pageNumberer.CurrentPageIndex = 1;

    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        btnFilter_Click(sender, e);
        grid.ChangeHeaderImageUrl(null, null);
    }

    protected void pn_SelectedPageChanged(object sender, EventArgs e)
    {
        _paging.CurrentPageIndex = pageNumberer.CurrentPageIndex;
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
        if (pagen >= 1 && pagen <= _paging.PageCount)
        {
            pageNumberer.CurrentPageIndex = pagen;
            _paging.CurrentPageIndex = pagen;
        }
    }

    protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "CancelAdd")
        {
            grid.ShowFooter = false;
        }
    }

    protected void grid_Sorting(object sender, GridViewSortEventArgs e)
    {
        var arrows = new Dictionary<string, string>
                                {
                                   {"FirstName", "arrowFirstName"},
                                   {"LastName", "arrowLastName"},
                                   {"RegistrationDateTime", "arrowRegistrationDateTime"},
                                   {"Email", "arrowEmail"},
                                   {"Subscribed4News", "arrowSubscribed4News"}
                                };
        const string urlArrowUp = "images/arrowup.gif";
        const string urlArrowDown = "images/arrowdown.gif";
        const string urlArrowGray = "images/arrowdownh.gif";

        var csf = (from Field f in _paging.Fields.Values where f.Sorting.HasValue select f).First();
        var nsf = _paging.Fields[e.SortExpression];

        if (nsf.Name.Equals(csf.Name))
        {
            csf.Sorting = csf.Sorting == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
            grid.ChangeHeaderImageUrl(arrows[csf.Name], (csf.Sorting == SortDirection.Ascending ? urlArrowUp : urlArrowDown));
        }
        else
        {
            csf.Sorting = null;
            grid.ChangeHeaderImageUrl(arrows[csf.Name], urlArrowGray);

            nsf.Sorting = SortDirection.Ascending;
            grid.ChangeHeaderImageUrl(arrows[nsf.Name], urlArrowUp);
        }

        pageNumberer.CurrentPageIndex = 1;
        _paging.CurrentPageIndex = 1;
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (grid.UpdatedRow != null)
        {
            try
            {
                using (var db = new SQLDataAccess())
                {
                    db.cmd.CommandText = "UPDATE [Customers].[Customer] SET Subscribed4News = @Subscribed4News WHERE (CustomerID = @CustomerID)";
                    db.cmd.CommandType = CommandType.Text;
                    db.cmd.Parameters.Clear();
                    db.cmd.Parameters.AddWithValue("@CustomerID", grid.UpdatedRow["ID"]);
                    db.cmd.Parameters.AddWithValue("@Subscribed4News", grid.UpdatedRow["Subscribed4News"]);

                    db.cnOpen();
                    db.cmd.ExecuteNonQuery();
                    db.cnClose();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }

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
                int intIndex = i;
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

        grid.DataSource = data;
        grid.DataBind();


        pageNumberer.PageCount = _paging.PageCount;
        lblFound.Text = _paging.TotalRowsCount.ToString(CultureInfo.InvariantCulture);
    }

    protected void btnAdmin_SubscriptionUnreg_Click(object sender, EventArgs e)
    {
        Response.Redirect("Subscription_Unreg.aspx");
    }

    protected void btnAdmin_Subscription_DeactivateReason_Click(object sender, EventArgs e)
    {
        Response.Redirect("Subscription_DeactivateReason.aspx");
    }
}