//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core;
using AdvantShop.Core.FieldFilters;
using Resources;

public partial class Admin_UserControls_OfferLists : UserControl
{
    private bool _inverseSelection;
    private SqlPaging _paging;

    private InSetFieldFilter _selectionFilter;

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_OfferLists_Header);

        if (!IsPostBack)
        {
            _paging = new SqlPaging { TableName = "[Catalog].[OffersList]", ItemsPerPage = 10 };
            _paging.AddFieldsRange(new[]
                                       {
                                           new Field
                                               {Name = "OfferListID as ID", IsDistinct = true, Filter = _selectionFilter},
                                           new Field {Name = "Name", Sorting = SortDirection.Ascending}
                                       });

            grid.ChangeHeaderImageUrl("arrowName", "~/admin/images/arrowup.gif");

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
                List<string> arrids = strIds.Trim().Split(' ').ToList();
                if (arrids.Contains("-1"))
                {
                    _inverseSelection = true;
                    arrids.Remove("-1");
                }
                int t;
                _selectionFilter = new InSetFieldFilter
                {
                    IncludeValues = !_inverseSelection,
                    Values = arrids.Where(id => int.TryParse(id, out t)).ToArray()
                };
            }
        }
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        //-----Selection filter
        if (ddSelect.SelectedIndex != 0 && ddSelect.SelectedIndex == 2 && _selectionFilter != null)
        {
            _selectionFilter.IncludeValues = !_selectionFilter.IncludeValues;
        }
        _paging.Fields["ID"].Filter = _selectionFilter;


        //----Name filter
        _paging.Fields["Name"].Filter = !string.IsNullOrEmpty(txtName.Text)
                                            ? new CompareFieldFilter { Expression = txtName.Text, ParamName = "@Name" }
                                            : null;

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
                    OfferService.DeleteOfferList(Convert.ToInt32(id));
                }
            }
            else
            {
                var itemsIds = _paging.ItemsIds<int>("OfferListID as ID");
                foreach (int id in itemsIds.Where(id => !_selectionFilter.Values.Contains(id.ToString())))
                {
                    OfferService.DeleteOfferList(id);
                }
            }
        }
    }

    protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Delete")
        {
            OfferService.DeleteOfferList(Convert.ToInt32(e.CommandArgument));
        }
        if (e.CommandName == "AddOfferList")
        {
            OfferService.AddOfferList(((TextBox)grid.FooterRow.FindControl("txtNewName")).Text);
            grid.ShowFooter = false;
        }
        if (e.CommandName == "CancelAdd")
        {
            grid.ShowFooter = false;
        }
    }

    protected void grid_Sorting(object sender, GridViewSortEventArgs e)
    {
        var arrows = new Dictionary<string, string>
                         {
                             {"Name", "arrowName"},
                         };
        const string urlArrowUp = "~/admin/images/arrowup.gif";
        const string urlArrowDown = "~/admin/images/arrowdown.gif";
        const string urlArrowGray = "~/admin/images/arrowdownh.gif";


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
            OfferService.UpdateOfferList(Convert.ToInt32(grid.UpdatedRow["ID"]), grid.UpdatedRow["Name"]);
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

    protected void btnAddOfferList_Click(object sender, EventArgs e)
    {
        grid.ShowFooter = true;
    }
}