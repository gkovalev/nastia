using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using AdvantShop.Core;
using AdvantShop.Core.FieldFilters;
using AdvantShop.Diagnostics;
using AdvantShop.Repository;
using AdvantShop.Taxes;

public partial class Admin_Taxs : System.Web.UI.Page
{
    SqlPaging _paging;
    InSetFieldFilter _selectionFilter;
    bool _inverseSelection;

    protected override void InitializeCulture()
    {
        AdvantShop.Localization.Culture.InitializeCulture();
    }

    public Admin_Taxs()
    {
        _inverseSelection = false;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = string.Format("{0} - {1}", AdvantShop.Configuration.SettingsMain.ShopName, Resources.Resource.Admin_Taxes_Header);

        if (!IsPostBack)
        {
            _paging = new SqlPaging { TableName = "[Catalog].[Tax] JOIN [Customers].[Country] ON [Tax].[CountryID] = [Country].[CountryID]", ItemsPerPage = 10 };

            var f = new Field { Name = "TaxID as ID", IsDistinct = true };
            _paging.AddField(f);


            f = new Field { Name = "Name", Sorting = SortDirection.Ascending };
            _paging.AddField(f);

            f = new Field { Name = "RegNumber" };
            _paging.AddField(f);

            f = new Field { Name = "Enabled" };
            _paging.AddField(f);

            f = new Field { Name = "[Tax].CountryID" };
            _paging.AddField(f);

            f = new Field { Name = "CountryName" };
            _paging.AddField(f);

            grid.ChangeHeaderImageUrl("arrowName", "images/arrowup.gif");

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
                string[] arrids = strIds.Split(' ');

                var ids = new string[arrids.Length];
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
        if (String.CompareOrdinal(ddSelect.SelectedIndex.ToString(CultureInfo.InvariantCulture), "0") != 0)
        {

            if (String.CompareOrdinal(ddSelect.SelectedIndex.ToString(CultureInfo.InvariantCulture), "2") == 0)
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

        //----Name filter
        if (!string.IsNullOrEmpty(txtName.Text))
        {
            var nfilter = new CompareFieldFilter { Expression = txtName.Text, ParamName = "@Name" };
            _paging.Fields["Name"].Filter = nfilter;
        }
        else
        {
            _paging.Fields["Name"].Filter = null;
        }
        //----RegNumber filter
        if (!string.IsNullOrEmpty(txtRegNumber.Text))
        {
            var nfilter = new CompareFieldFilter
                                             {
                                                 Expression = txtRegNumber.Text,
                                                 ParamName = "@RegNumber"
                                             };
            _paging.Fields["RegNumber"].Filter = nfilter;
        }
        else
        {
            _paging.Fields["RegNumber"].Filter = null;
        }

        //----Country filter
        if (ddlCountryFilter.SelectedIndex != 0)
        {
            var nfilter = new CompareFieldFilter
            {
                Expression = ddlCountryFilter.SelectedItem.Text,
                ParamName = "@Country"
            };
            _paging.Fields["CountryName"].Filter = nfilter;
        }
        else
        {
            _paging.Fields["CountryName"].Filter = null;
        }

        //----Enabled filter
        if (ddlEnabled.SelectedIndex != 0)
        {

            var efilter = new EqualFieldFilter { ParamName = "@Enabled" };
            if (ddlEnabled.SelectedIndex == 1)
            {
                efilter.Value = "1";
            }
            if (ddlEnabled.SelectedIndex == 2)
            {
                efilter.Value = "0";
            }
            _paging.Fields["Enabled"].Filter = efilter;
        }
        else
        {
            _paging.Fields["Enabled"].Filter = null;
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


    //Delete!!
    protected void lbDeleteSelected_Click(object sender, EventArgs e)
    {
        if ((_selectionFilter != null) && (_selectionFilter.Values != null))
        {
            if (!_inverseSelection)
            {
                foreach (var id in _selectionFilter.Values)
                {
                    TaxServices.DeleteTax(Convert.ToInt32(id));
                }
            }
            else
            {
                var itemsIds = _paging.ItemsIds<int>("TaxID as ID");
                foreach (int tax in itemsIds.Where(tax => !_selectionFilter.Values.Contains(tax.ToString(CultureInfo.InvariantCulture))))
                {
                    TaxServices.DeleteTax(tax);
                }
            }
        }
    }

    protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Delete")
        {
            TaxServices.DeleteTax(int.Parse((string)e.CommandArgument));
        }
        if (e.CommandName == "AddTax")
        {
            GridViewRow footer = grid.FooterRow;
            string name = ((TextBox)footer.FindControl("txtNewName")).Text;
            string regNumber = ((TextBox)footer.FindControl("txtNewRegNumber")).Text;
            bool enabled = Convert.ToBoolean(((DropDownList)footer.FindControl("ddlNewEnabled")).SelectedValue);
            int countryId = Convert.ToInt32(((DropDownList)footer.FindControl("ddlCountry")).SelectedValue);
            if ((name.Trim().Length != 0) && (regNumber.Trim().Length != 0))
            {
                var tax = new TaxElement { CountryID = countryId, DependsOnAddress = TypeRateDepends.Default, Enabled = enabled, FederalRate = 0.0M, Name = name, Priority = 1, RegNumber = regNumber, ShowInPrice = false, Type = (RateType)1 };
                TaxServices.CreateTax(tax);
                //if (tax.TaxId != 0)
                //    Response.Redirect("Tax.aspx?TaxID=" + tax.TaxId);
            }
            grid.ShowFooter = false;
        }
    }

    protected void grid_Sorting(object sender, GridViewSortEventArgs e)
    {
        var arrows = new Dictionary<string, string>
                                                {
                                                    {"Name", "arrowName"},
                                                    {"RegNumber", "arrowRegNumber"},
                                                    {"DependsOnAddress", "arrowDependsON"},
                                                    {"ShowInPrice", "arrowShowInPrice"},
                                                    {"Enabled", "arrowEnabled"},
                                                    {"Priority", "arrowPriority"},
                                                    {"[Tax].CountryID", "arrowCountryID"},
                                                    {"RateType", "arrowRateType"},
                                                    {"FederalRate", "arrowFederalRate"}
                                                };
        const string urlArrowUp = "images/arrowup.gif";
        const string urlArrowDown = "images/arrowdown.gif";
        const string urlArrowGray = "images/arrowdownh.gif";


        Field csf = (from Field f in _paging.Fields.Values where f.Sorting.HasValue select f).First();
        Field nsf = _paging.Fields[e.SortExpression];

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
                    db.cmd.CommandText = "Update Catalog.Tax set name=@name, Enabled=@Enabled, RegNumber = @RegNumber, CountryId = @countryId where TaxID = @id";
                    db.cmd.CommandType = CommandType.Text;
                    db.cmd.Parameters.Clear();
                    db.cmd.Parameters.AddWithValue("@name", grid.UpdatedRow["Name"]);
                    db.cmd.Parameters.AddWithValue("@Enabled", grid.UpdatedRow["Enable"]);
                    db.cmd.Parameters.AddWithValue("@RegNumber", grid.UpdatedRow["RegNumber"]);
                    db.cmd.Parameters.AddWithValue("@id", grid.UpdatedRow["ID"]);
                    db.cmd.Parameters.AddWithValue("@countryId", grid.UpdatedRow["CountryID"]);
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

        var countries = CountryService.GetAllCountries();
        ddlCountryFilter.DataSource = countries;
        ddlCountryFilter.DataBind();
        ddlCountryFilter.Items.Insert(0, new ListItem(Resources.Resource.Admin_Taxes_AnyCountry, "0"));

        if (grid.FooterRow != null)
        {
            var ddlCountry = ((DropDownList)grid.FooterRow.FindControl("ddlCountry"));
            ddlCountry.DataSource = countries;
            ddlCountry.DataBind();
        }

        ddlCountryFilter.SelectedIndex = 0;
        pageNumberer.PageCount = _paging.PageCount;
        lblFound.Text = _paging.TotalRowsCount.ToString();
    }

    protected void grid_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
    }

    protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            var ddlCountry = ((DropDownList)e.Row.FindControl("ddlCountry"));
            var countries = CountryService.GetAllCountries();

            ddlCountry.DataSource = countries;
            ddlCountry.DataBind();
            ddlCountry.SelectedValue = ((DataRowView)e.Row.DataItem)["CountryID"].ToString();
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {

        }
    }


    protected void btnAddTax_Click(object sender, EventArgs e)
    {
        grid.ShowFooter = true;
    }

}
