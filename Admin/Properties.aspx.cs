using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core;
using AdvantShop.Core.FieldFilters;
using AdvantShop.Diagnostics;
using Resources;

public partial class Admin_Properties : Page
{
    private bool _inverseSelection;
    private SqlPaging _paging;
    private InSetFieldFilter _selectionFilter;

    protected override void InitializeCulture()
    {
        AdvantShop.Localization.Culture.InitializeCulture();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_Properties_ListPropreties);

        if (!IsPostBack)
        {
            _paging = new SqlPaging
                          {
                              TableName = "[Catalog].[Property]",
                              ItemsPerPage = 20,
                              CurrentPageIndex = 1
                          };

            _paging.AddFieldsRange(new[]
                                 {
                                     new Field {Name = "PropertyID as ID", IsDistinct = true},
                                     new Field {Name = "Name"},
                                     new Field {Name = "UseInFilter"},
                                     new Field {Name = "SortOrder", Sorting = SortDirection.Ascending},
                                     new Field {Name = "(Select Count(ProductID) from Catalog.ProductPropertyValue Where [PropertyValueID] in ( SELECT [PropertyValueID]  FROM [Catalog].[PropertyValue] WHERE [PropertyID] = [Property].PropertyID)) as ProductsCount"},
                                 });
            grid.ChangeHeaderImageUrl("arrowSortOrder", "images/arrowup.gif");
            pageNumberer.CurrentPageIndex = 1;
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
            if (String.CompareOrdinal(ddSelect.SelectedIndex.ToString(CultureInfo.InvariantCulture), "2") == 0 && _selectionFilter != null)
            {
                _selectionFilter.IncludeValues = !_selectionFilter.IncludeValues;
            }
            _paging.Fields["ID"].Filter = _selectionFilter;
        }
        else
        {
            _paging.Fields["ID"].Filter = null;
        }

        //----Name filter
        _paging.Fields["Name"].Filter = string.IsNullOrEmpty(txtName.Text)
                                            ? null
                                            : new CompareFieldFilter
                                                {
                                                    Expression = txtName.Text,
                                                    ParamName = "@Name"
                                                };

        //----Enabled filter
        _paging.Fields["UseInFilter"].Filter = ddlUseInFilter.SelectedValue != "Any"
                                                   ? new EqualFieldFilter
                                                         {
                                                             ParamName = "@UseInFilter",
                                                             Value = ddlUseInFilter.SelectedValue
                                                         }
                                                   : null;

        //----SortOrder filter
        _paging.Fields["SortOrder"].Filter = string.IsNullOrEmpty(txtSortOrder.Text)
                                                 ? null
                                                 : new CompareFieldFilter
                                                       {
                                                           ParamName = "@SortOrder",
                                                           Expression = txtSortOrder.Text
                                                       };

        _paging.Fields["ProductsCount"].Filter = string.IsNullOrEmpty(txtProductsCount.Text)
                                                     ? null
                                                     : new EqualFieldFilter
                                                         {
                                                             ParamName = "@ProductsCount",
                                                             Value = txtProductsCount.Text
                                                         };

        
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
                    PropertyService.DeleteProperty(Convert.ToInt32(id));
                }
            }
            else
            {
                var itemsIds = _paging.ItemsIds<int>("PropertyID as ID");
                foreach (var id in itemsIds.Where(id => !_selectionFilter.Values.Contains(id.ToString())))
                {
                    PropertyService.DeleteProperty(id);
                }
            }
        }
    }

    protected void lbSetInFilter_Click(object sender, EventArgs e)
    {
        if ((_selectionFilter != null) && (_selectionFilter.Values != null))
        {
           if (!_inverseSelection)
            {
                foreach (var id in _selectionFilter.Values)
                {
                    PropertyService.ShowInFilter(Convert.ToInt32(id), true);
                }
            }
            else
            {
                var itemsIds = _paging.ItemsIds<int>("PropertyID as ID");
                foreach (var id in itemsIds.Where(id => !_selectionFilter.Values.Contains(id.ToString())))
                {
                    PropertyService.ShowInFilter(Convert.ToInt32(id), true);
                }
            }
        }
    }

    protected void lbSetNotInFilter_Click(object sender, EventArgs e)
    {
        if ((_selectionFilter != null) && (_selectionFilter.Values != null))
        {
            if (!_inverseSelection)
            {
                foreach (var id in _selectionFilter.Values)
                {
                    PropertyService.ShowInFilter(Convert.ToInt32(id), false);
                }
            }
            else
            {
                var itemsIds = _paging.ItemsIds<int>("PropertyID as ID");
                foreach (var id in itemsIds.Where(id => !_selectionFilter.Values.Contains(id.ToString())))
                {
                    PropertyService.ShowInFilter(Convert.ToInt32(id), false);
                }
            }
        }
    }


    protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "DeleteProperty")
        {
            PropertyService.DeleteProperty(Convert.ToInt32(e.CommandArgument));
        }

        if (e.CommandName == "AddProperty")
        {
            try
            {
                GridViewRow footer = grid.FooterRow;
                int temp;
                int.TryParse(((TextBox)footer.FindControl("txtNewSortOrder")).Text, out temp);
                if (string.IsNullOrEmpty(((TextBox)footer.FindControl("txtNewName")).Text))
                {
                    grid.FooterStyle.BackColor = System.Drawing.Color.FromName("#ffcccc");
                    return;
                }

                if (PropertyService.AddProperty(new Property
                                                {
                                                    Name = ((TextBox)footer.FindControl("txtNewName")).Text.Trim(),
                                                    UseInFilter = ((CheckBox)footer.FindControl("chkNewUse")).Checked,
                                                    SortOrder = temp
                                                })
                    != 0)
                    grid.ShowFooter = false;
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
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
                             {"Name", "arrowName"},
                             {"UseInFilter", "arrowUseInFilter"},
                             {"SortOrder", "arrowSortOrder"},
                             {"ProductsCount", "arrowProductsCount"},

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
            PropertyService.UpdateProperty(new Property
                                                    {
                                                        Name = grid.UpdatedRow["Name"],
                                                        UseInFilter = Convert.ToBoolean(grid.UpdatedRow["UseInFilter"]),
                                                        PropertyId = Convert.ToInt32(grid.UpdatedRow["ID"]),
                                                        SortOrder = grid.UpdatedRow["SortOrder"].TryParseInt()
                                                    }
                                               );
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
                if (Array.Exists(_selectionFilter.Values, c => c == (data.Rows[intIndex]["ID"]).ToString()))
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

    protected void grid_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
    }

    protected string RenderProductsCount(int propID)
    {
        return PropertyService.GetProductsCountByProperty(propID).ToString(CultureInfo.InvariantCulture);
    }

    private void grid_DataBound(object sender, EventArgs e)
    {
        if (grid.ShowFooter)
        {
            grid.FooterRow.FindControl("txtNewName").Focus();
        }
    }

    protected void btnAddProperty_Click(object sender, EventArgs e)
    {
        grid.ShowFooter = true;
        grid.FooterStyle.BackColor = System.Drawing.Color.FromName("#ccffcc");
        grid.DataBound += grid_DataBound;
    }
}