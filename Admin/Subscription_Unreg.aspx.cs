using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop.Configuration;
using AdvantShop.Core;
using AdvantShop.Core.FieldFilters;
using AdvantShop.Diagnostics;
using Resources;

//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

public partial class Admin_SubscriptionUnreg : Page
{
    private bool _inverseSelection;
    private SqlPaging _paging;
    private InSetFieldFilter _selectionFilter;

    public Admin_SubscriptionUnreg()
    {
        _inverseSelection = false;
    }

    protected override void InitializeCulture()
    {
        AdvantShop.Localization.Culture.InitializeCulture();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_Subscription_Header);

        if (!IsPostBack)
        {
            _paging = new SqlPaging { TableName = "[dbo].[Subscribe]", ItemsPerPage = 10 };

            var f = new Field { Name = "ID", IsDistinct = true, Filter = _selectionFilter };
            _paging.AddField(f);


            f = new Field { Name = "Email" };
            _paging.AddField(f);

            f = new Field { Name = "Enable" };
            _paging.AddField(f);

            f = new Field { Name = "ActivateCode" };
            _paging.AddField(f);

            f = new Field { Name = "DectivateCode" };
            _paging.AddField(f);

            f = new Field { Name = "ActivateDate" };
            _paging.AddField(f);

            f = new Field { Name = "AddDate", Sorting = SortDirection.Descending };
            _paging.AddField(f);

            grid.ChangeHeaderImageUrl("arrowAddDate", "images/arrowup.gif");

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
                string[] arrids = strIds.Split(' ');

                var ids = new string[arrids.Length ];
                _selectionFilter = new InSetFieldFilter { IncludeValues = true };
                for (int idx = 0; idx <= ids.Length - 1; idx++)
                {
                    int t = int.Parse(arrids[idx]);
                    if (t != -1)
                    {
                        ids[idx] = t.ToString(CultureInfo.InvariantCulture);
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


        //----Email filter
        if (!string.IsNullOrEmpty(txtEmail.Text))
        {
            var nfilter = new CompareFieldFilter { Expression = txtEmail.Text, ParamName = "@Email" };
            _paging.Fields["Email"].Filter = nfilter;
        }
        else
        {
            _paging.Fields["Email"].Filter = null;
        }

        //---AddDate filter
        var dfilter1 = new DateTimeRangeFieldFilter { ParamName = "@AddDate" };

        if (!string.IsNullOrEmpty(txtAddDateFrom.Text))
        {
            DateTime? d = null;
            try
            {
                d = DateTime.Parse(txtAddDateFrom.Text);
            }
            catch (Exception)
            {
            }
            if (d.HasValue)
            {
                var dt = new DateTime(d.Value.Year, d.Value.Month, d.Value.Day, 0, 0, 0, 0);
                dfilter1.From = dt;
            }
        }

        if (!string.IsNullOrEmpty(txtAddDateTo.Text))
        {
            DateTime? d = null;
            try
            {
                d = DateTime.Parse(txtAddDateTo.Text);
            }
            catch (Exception)
            {
            }
            if (d.HasValue)
            {
                var dt = new DateTime(d.Value.Year, d.Value.Month, d.Value.Day, 23, 59, 59, 99);
                dfilter1.To = dt;
            }
        }

        if (dfilter1.From.HasValue || dfilter1.To.HasValue)
        {
            _paging.Fields["AddDate"].Filter = dfilter1;
        }
        else
        {
            _paging.Fields["AddDate"].Filter = null;
        }

        //---AddDate filter
        var dfilter2 = new DateTimeRangeFieldFilter { ParamName = "@ActivateDate" };

        if (!string.IsNullOrEmpty(txtActivatedDateFrom.Text))
        {
            DateTime? d = null;
            try
            {
                d = DateTime.Parse(txtActivatedDateFrom.Text);
            }
            catch (Exception)
            {
            }
            if (d.HasValue)
            {
                var dt = new DateTime(d.Value.Year, d.Value.Month, d.Value.Day, 0, 0, 0, 0);
                dfilter2.From = dt;
            }
        }

        if (!string.IsNullOrEmpty(txtActivatedDateTo.Text))
        {
            DateTime? d = null;
            try
            {
                d = DateTime.Parse(txtActivatedDateTo.Text);
            }
            catch (Exception)
            {
            }
            if (d.HasValue)
            {
                var dt = new DateTime(d.Value.Year, d.Value.Month, d.Value.Day, 23, 59, 59, 99);
                dfilter2.To = dt;
            }
        }

        if (dfilter2.From.HasValue || dfilter2.To.HasValue)
        {
            _paging.Fields["ActivateDate"].Filter = dfilter2;
        }
        else
        {
            _paging.Fields["ActivateDate"].Filter = null;
        }


        //----Enable filter
        if (ddEnable.SelectedIndex != 0)
        {
            var enableFilter = new EqualFieldFilter { ParamName = "@Enable" };
            if (ddEnable.SelectedIndex == 1)
            {
                enableFilter.Value = "1";
            }
            if (ddEnable.SelectedIndex == 2)
            {
                enableFilter.Value = "0";
            }
            _paging.Fields["Enable"].Filter = enableFilter;
        }
        else
        {
            _paging.Fields["Enable"].Filter = null;
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

    protected void lbDeleteSelected_Click(object sender, EventArgs e)
    {
        if ((_selectionFilter != null) && (_selectionFilter.Values != null))
        {
            if (!_inverseSelection)
            {
                foreach (var id in _selectionFilter.Values)
                {
                    DeleteUnRegUsers(Convert.ToInt32(id));
                }
            }
            else
            {
                var itemsIds = _paging.ItemsIds<int>("ID");
                foreach (int id in itemsIds.Where(iId => !_selectionFilter.Values.Contains(iId.ToString(CultureInfo.InvariantCulture))))
                {
                    DeleteUnRegUsers(id);
                }
            }
        }
    }

    protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "DeleteUser")
        {
            DeleteUnRegUsers((int.Parse(e.CommandArgument.ToString())));
        }
    }

    protected void grid_Sorting(object sender, GridViewSortEventArgs e)
    {
        var arrows = new Dictionary<string, string>
                         {
                             {"ID", "arrowID"},
                             {"Email", "arrowEmail"},
                             {"Enable", "arrowEnable"},
                             {"ActivateDate", "arrowActivateDate"},
                             {"AddDate", "arrowAddDate"}
                         };
        const string urlArrowUp = "images/arrowup.gif";
        const string urlArrowDown = "images/arrowdown.gif";
        const string urlArrowGray = "images/arrowdownh.gif";


        Field csf = (from Field f in _paging.Fields.Values where f.Sorting.HasValue select f).First();
        Field nsf = _paging.Fields[e.SortExpression];

        if (nsf.Name.Equals(csf.Name))
        {
            csf.Sorting = csf.Sorting == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
            grid.ChangeHeaderImageUrl(arrows[csf.Name],
                                      (csf.Sorting == SortDirection.Ascending ? urlArrowUp : urlArrowDown));
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
                    db.cmd.CommandText =
                        "UPDATE [dbo].[Subscribe] SET Enable = @Enable WHERE (ID = @ID)";
                    db.cmd.CommandType = CommandType.Text;
                    db.cmd.Parameters.Clear();
                    db.cmd.Parameters.AddWithValue("@id", grid.UpdatedRow["ID"]);
                    db.cmd.Parameters.AddWithValue("@Enable", grid.UpdatedRow["Enable"]);

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

    private static void DeleteUnRegUsers(int id)
    {
        try
        {
            SQLDataAccess.ExecuteNonQuery("DELETE FROM [dbo].[Subscribe] where ID = @ID", CommandType.Text, new SqlParameter("@ID", id));
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
    }

    protected void btnAdmin_SubscriptionReg_Click(object sender, EventArgs e)
    {
        Response.Redirect("Subscription.aspx");
    }

    protected void btnAdmin_Subscription_DeactivateReason_Click(object sender, EventArgs e)
    {
        Response.Redirect("Subscription_DeactivateReason.aspx");
    }
}