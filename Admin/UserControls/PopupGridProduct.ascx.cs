//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop.Core;
using AdvantShop.Core.FieldFilters;

public partial class Admin_UserControls_PopupGridProduct : UserControl
{
    private SqlPaging _paging;
    public int SelectProductId = 0;

    protected void Page_PreRender(object sender, EventArgs e)
    {
        agvProducts.DataSource = _paging.PageItems;
        agvProducts.DataBind();
        pageNumberer.PageCount = _paging.PageCount;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // load modalpopup for contact
            _paging = new SqlPaging { TableName = "[Catalog].[Product]", ItemsPerPage = 10 };

            var f = new Field { Name = "ProductID", IsDistinct = true };

            _paging.AddField(f);

            f = new Field { Name = "ArtNo" , Sorting=SortDirection.Ascending };

            _paging.AddField(f);

            f = new Field { Name = "Name" };
            _paging.AddField(f);

            agvProducts.ChangeHeaderImageUrl("arrowArtNo", "../images/arrowup.gif");

            _paging.ItemsPerPage = 10;

            pageNumberer.CurrentPageIndex = 1;
            _paging.CurrentPageIndex = 1;
            ViewState["Paging"] = _paging;
        }
        else
        {
            _paging = (SqlPaging)(ViewState["Paging"]);

            if (_paging == null)
            {
                throw (new Exception("Paging lost"));
            }
        }
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        //----Login filter
        if (!string.IsNullOrEmpty(txtSearchArtNo.Text))
        {
            var sfilter = new CompareFieldFilter { Expression = txtSearchArtNo.Text, ParamName = "@artNo" };
            _paging.Fields["ArtNo"].Filter = sfilter;
        }
        else
        {
            _paging.Fields["ArtNo"].Filter = null;
        }

        //----Firstname filter
        if (!string.IsNullOrEmpty(txtSearchName.Text))
        {
            var nfilter = new CompareFieldFilter { Expression = txtSearchName.Text, ParamName = "@Name" };
            _paging.Fields["Name"].Filter = nfilter;
        }
        else
        {
            _paging.Fields["Name"].Filter = null;
        }

        pageNumberer.CurrentPageIndex = 1;
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtSearchArtNo.Text = string.Empty;
        txtSearchName.Text = string.Empty;
        btnFilter_Click(sender, e);
        agvProducts.DataBind();
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
        if (pagen < 1 || pagen > _paging.PageCount) return;
        pageNumberer.CurrentPageIndex = pagen;
        _paging.CurrentPageIndex = pagen;
    }

    protected void pn_SelectedPageChanged(object sender, EventArgs e)
    {
        _paging.CurrentPageIndex = pageNumberer.CurrentPageIndex;
    }

    protected void agv_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Select")
        {
            SelectProductId = Convert.ToInt32(e.CommandArgument);
        }
    }

    protected void agvProducts_Sorting(object sender, GridViewSortEventArgs e)
    {
        var arrows = new Dictionary<string, string>
                         {
                             {"ArtNo", "arrowArtNo"},
                             {"Name", "arrowName"},                             
                         };

        const string urlArrowUp = "../images/arrowup.gif";
        const string urlArrowDown = "../images/arrowdown.gif";
        const string urlArrowGray = "../images/arrowdownh.gif";

        Field csf = (from Field f in _paging.Fields.Values where f.Sorting.HasValue select f).First();
        Field nsf = _paging.Fields[e.SortExpression];

        if (nsf.Name.Equals(csf.Name))
        {
            csf.Sorting = csf.Sorting == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
            agvProducts.ChangeHeaderImageUrl(arrows[csf.Name],
                                              (csf.Sorting == SortDirection.Ascending ? urlArrowUp : urlArrowDown));
        }
        else
        {
            csf.Sorting = null;
            //If Not csf.Name.Contains("SortOrder") Then
            agvProducts.ChangeHeaderImageUrl(arrows[csf.Name], urlArrowGray);
            //End If

            nsf.Sorting = SortDirection.Ascending;
            agvProducts.ChangeHeaderImageUrl(arrows[nsf.Name], urlArrowUp);
        }

        pageNumberer.CurrentPageIndex = 1;
        _paging.CurrentPageIndex = 1;
        //UpdatePanel2.Update();
    }


}
