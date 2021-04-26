using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using AdvantShop.Core;
using AdvantShop.Core.FieldFilters;
using AdvantShop.Repository;

public partial class Admin_Cities : System.Web.UI.Page
{
    SqlPaging _paging;
    InSetFieldFilter _selectionFilter;
    bool _inverseSelection;

    protected override void InitializeCulture()
    {
        AdvantShop.Localization.Culture.InitializeCulture();
    }

    public Admin_Cities()
    {
        _inverseSelection = false;
    }

    protected int RegionID
    {
        get
        {
            if (ViewState["RegionID"] != null)
            {
                return (int)ViewState["RegionID"];
            }
            return !string.IsNullOrEmpty(Request["regionid"]) ? Convert.ToInt32(Request["regionid"]) : -1;
        }
        set { ViewState["RegionID"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = string.Format("{0} - {1}", AdvantShop.Configuration.SettingsMain.ShopName, Resources.Resource.Admin_MasterPageAdminCatalog_Catalog);
        if (string.IsNullOrEmpty(Request["regionid"]))
        {
            Response.Redirect("Country.aspx");
        }

        Region region = RegionService.GetRegion(Convert.ToInt32(RegionID));
        if (region == null)
            Response.Redirect("Country.aspx");
        else
        {
            lblHead.Text = region.Name;
            hlBack.NavigateUrl = "Regions.aspx?CountryId=" + region.CountryID;
            hlBack.Text = Resources.Resource.Admin_Cities_BackToRegions;
        }



        if (!IsPostBack)
        {
            _paging = new SqlPaging { TableName = "[Customers].[City]", ItemsPerPage = 20 };

            var f = new Field { Name = "CityID as ID", IsDistinct = true };
            _paging.AddField(f);

            f = new Field { Name = "CityName", Sorting = SortDirection.Ascending };
            _paging.AddField(f);

            f = new Field { Name = "CitySort" };
            _paging.AddField(f);

            f = new Field { Name = "RegionID", Filter = new EqualFieldFilter() { ParamName = "@RegionID", Value = RegionID.ToString() } };
            _paging.AddField(f);

            grid.ChangeHeaderImageUrl("arrowCityName", "images/arrowup.gif");

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
                        ids[idx] = t.ToString();
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
        if (string.Compare(ddSelect.SelectedIndex.ToString(), "0") != 0)
        {

            if (string.Compare(ddSelect.SelectedIndex.ToString(), "2") == 0)
            {
                if (_selectionFilter != null)
                {
                    _selectionFilter.IncludeValues = !_selectionFilter.IncludeValues;
                }
                else
                {
                    _selectionFilter = null; //New InSetFieldFilter()
                    //_SelectionFilter.IncludeValues = True
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
            _paging.Fields["CityName"].Filter = nfilter;
        }
        else
        {
            _paging.Fields["CityName"].Filter = null;
        }


        //---RegionSort filter
        if (!string.IsNullOrEmpty(txtRegSort.Text))
        {
            var nfilter = new CompareFieldFilter
            {
                Expression = txtRegSort.Text,
                ParamName = "@CitySort"
            };
            _paging.Fields["CitySort"].Filter = nfilter;
        }
        else
        {
            _paging.Fields["CitySort"].Filter = null;
        }


        pageNumberer.CurrentPageIndex = 1;
        _paging.CurrentPageIndex = 1;

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
                    CityService.DeleteCity(Convert.ToInt32(id));
                }
            }
            else
            {
                var itemsIds = _paging.ItemsIds<int>("CityID as ID");
                //List<int> ids = CityService.GetAllCityIDByRegion(RegionID);
                foreach (var id in itemsIds.Where(id => !_selectionFilter.Values.Contains(id.ToString(CultureInfo.InvariantCulture))))
                {
                    CityService.DeleteCity(id);
                }
            }
        }
    }

    protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Delete")
        {
            CityService.DeleteCity(Convert.ToInt32(e.CommandArgument));
        }
        if (e.CommandName == "AddCity")
        {
            GridViewRow footer = grid.FooterRow;

            if (string.IsNullOrEmpty(((TextBox)footer.FindControl("txtNewName")).Text))
            {
                grid.FooterStyle.BackColor = System.Drawing.Color.FromName("#ffcccc");
                return;
            }

            int temp;
            int.TryParse(((TextBox)footer.FindControl("txtNewSort")).Text, out temp);

            var city = new City
                             {
                                 Name = ((TextBox)footer.FindControl("txtNewName")).Text,
                                 CitySort = temp,
                                 RegionID = RegionID
                             };
            CityService.InsertCity(city);
            grid.ShowFooter = false;
        }
        if (e.CommandName == "CancelAdd")
        {
            grid.FooterStyle.BackColor = System.Drawing.Color.FromName("#ccffcc");
            grid.ShowFooter = false;
        }
    }

    protected void grid_Sorting(object sender, GridViewSortEventArgs e)
    {
        var arrows = new Dictionary<string, string>
                                                {
                                                    {"CityName", "arrowCityName"},
                                                    {"CityCode", "arrowCityCode"},
                                                    {"CitySort", "arrowCitySort"}
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
            var city = new City
                             {
                                 CityID = Convert.ToInt32(grid.UpdatedRow["ID"]),
                                 Name = grid.UpdatedRow["CityName"],
                                 RegionID = RegionID,
                                 CitySort = Convert.ToInt32(grid.UpdatedRow["CitySort"])
                             };
            CityService.UpdateCity(city);
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
                if (Array.Exists(_selectionFilter.Values, c => c == data.Rows[intIndex]["RegionID"].ToString()))
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
        lblFound.Text = _paging.TotalRowsCount.ToString();
    }

    protected void grid_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
    }

    protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
        }
    }
    protected void btnAddCity_Click(object sender, EventArgs e)
    {
        grid.ShowFooter = true;
        grid.FooterStyle.BackColor = System.Drawing.Color.FromName("#ccffcc");
        grid.DataBound += grid_DataBound;
    }

    void grid_DataBound(object sender, EventArgs e)
    {
        if (grid.ShowFooter)
        {
            grid.FooterRow.FindControl("txtNewName").Focus();
        }
    }
}