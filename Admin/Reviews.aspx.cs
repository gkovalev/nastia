using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop.Configuration;
using AdvantShop.Core;
using AdvantShop.Core.FieldFilters;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using Resources;
using AdvantShop.CMS;
using System.Globalization;

partial class Admin_Reviews : Page
{
    private bool _inverseSelection;
    private SqlPaging _paging;
    private InSetFieldFilter _selectionFilter;

    public Admin_Reviews()
    {
        PreRender += Page_PreRender;
        Load += Page_Load;
    }

    protected override void InitializeCulture()
    {
        AdvantShop.Localization.Culture.InitializeCulture();
    }

    protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "DeleteReview")
        {
            ReviewService.DeleteReview(Convert.ToInt32(e.CommandArgument));
        }
    }

    protected void grid_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
    }

    protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            try
            {
                var commentId = Convert.ToInt32(((HiddenField)e.Row.FindControl("hfID")).Value);
                var reviewEntity = ReviewService.GetReviewEntity(commentId);
                var entity = ReviewService.GetReviewEntity(commentId);
                var url = ReviewService.GetEntityUrl(reviewEntity.ReviewEntityId, reviewEntity.Type);

                var ibPhoto = ((HyperLink)e.Row.FindControl("ibPhoto"));
                if (!string.IsNullOrEmpty(entity.Photo))
                {
                    ibPhoto.ImageUrl = FoldersHelper.GetImageProductPath(ProductImageType.XSmall, reviewEntity.Photo, false);
                    ibPhoto.Text = entity.PhotoDescription;
                    ibPhoto.ToolTip = entity.PhotoDescription;
                    ibPhoto.NavigateUrl = url;
                    ibPhoto.Attributes.Add("abbr", entity.Photo);
                }
                else
                {
                    ibPhoto.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                MsgErr("Unable to load product photo");
            }
        }
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
        Message.Text = @"<br/>" + messageText;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Message.Visible = false;
        Page.Title = string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_Reviews_Reviews);


        MsgErr(true);

        if (!IsPostBack)
        {
            _paging = new SqlPaging
                          {
                              TableName = "[CMS].[Review]",
                              ItemsPerPage = 10
                          };

            _paging.AddFieldsRange(
                new Field { Name = "ReviewId as ID", IsDistinct = true },
                new Field { Name = "Name" },
                new Field { Name = "Email" },
                new Field { Name = "AddDate", Sorting = SortDirection.Descending },
                new Field { Name = "Checked" },
                new Field { Name = "[Text]" });
            grid.ChangeHeaderImageUrl("arrowAddDate", "images/arrowdown.gif");

            _paging.ItemsPerPage = 10;

            pageNumberer.CurrentPageIndex = 1;
            _paging.CurrentPageIndex = 1;
            ViewState["Paging"] = _paging;
            if (Request["ReviewId"] != null)
            {
                _paging.Fields["ReviewId"].Filter = new CompareFieldFilter
                                                         {
                                                             ParamName = "@ReviewId",
                                                             Expression = Request["ReviewId"]
                                                         };
            }
        }
        else
        {
            _paging = (SqlPaging)ViewState["Paging"];
            _paging.ItemsPerPage = Convert.ToInt32(ddRowsPerPage.SelectedValue);

            if (_paging == null)
            {
                throw new Exception("Paging lost");
            }

            var strIds = Request.Form["SelectedIds"];

            if (!string.IsNullOrEmpty(strIds))
            {
                List<string> arrids = strIds.Trim().Split(' ').ToList();

                _selectionFilter = new InSetFieldFilter();
                if (arrids.Contains("-1"))
                {
                    _selectionFilter.IncludeValues = false;
                    _inverseSelection = true;
                    arrids.Remove("-1");
                }
                else
                {
                    _selectionFilter.IncludeValues = true;
                }
                _selectionFilter.Values = arrids.ToArray();
            }
        }
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        btnFilter_Click(sender, e);
        grid.ChangeHeaderImageUrl(null, null);
    }

    protected void grid_Sorting(object sender, GridViewSortEventArgs e)
    {
        var arrows = new Dictionary<string, string>
                         {
                             {"Checked", "arrowChecked"},
                             {"Name", "arrowName"},
                             {"Email", "arrowEmail"},
                             {"[Text]", "arrowText"},
                             {"AddDate", "arrowAddDate"}
                         };

        const string urlArrowUp = "images/arrowup.gif";
        const string urlArrowDown = "images/arrowdown.gif";
        const string urlArrowGray = "images/arrowdownh.gif";

        var csf = (from Field f in _paging.Fields.Values where f.Sorting.HasValue select f).First();
        var nsf = _paging.Fields[e.SortExpression];

        if (nsf.Name.Equals(csf.Name))
        {
            csf.Sorting = csf.Sorting == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
            grid.ChangeHeaderImageUrl(arrows[csf.Name],
                                      csf.Sorting == SortDirection.Ascending ? urlArrowUp : urlArrowDown);
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
            var commentId = Convert.ToInt32(grid.UpdatedRow["Id"]);
            var name = grid.UpdatedRow["Name"];
            var email = grid.UpdatedRow["Email"];
            var text = grid.UpdatedRow["[Text]"];


            var comment = ReviewService.GetReview(commentId);
            comment.Name = name;
            comment.Email = email;
            comment.Text = text;
            comment.Checked = Convert.ToBoolean(grid.UpdatedRow["Checked"]);


            ReviewService.UpdateReview(comment);
        }

        DataTable data = _paging.PageItems;
        while (data.Rows.Count < 1 & _paging.CurrentPageIndex > 1)
        {
            _paging.CurrentPageIndex -= 1;
            data = _paging.PageItems;
        }

        data.Columns.Add(new DataColumn("IsSelected", typeof(bool)) { DefaultValue = _inverseSelection });
        if (_selectionFilter != null && _selectionFilter.Values != null)
        {
            for (int i = 0; i <= data.Rows.Count - 1; i++)
            {
                Int32 intIndex = i;
                if (Array.Exists(_selectionFilter.Values, (string c) => c == data.Rows[intIndex]["ID"].ToString()))
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
            _paging.Fields["ID"].Filter = null;
        }

        //----Email filter 


        _paging.Fields["Email"].Filter = !string.IsNullOrEmpty(txtEmail.Text)
                                                ? new CompareFieldFilter { ParamName = "@Email", Expression = txtEmail.Text }
                                                : null;

        //-- productName
        _paging.Fields["Name"].Filter = !string.IsNullOrEmpty(txtName.Text)
                                            ? new CompareFieldFilter { ParamName = "@Name", Expression = txtName.Text }
                                            : null;
        //----Text filter 


        _paging.Fields["[Text]"].Filter = !string.IsNullOrEmpty(txtText.Text)
                                              ? new CompareFieldFilter { ParamName = "@Text", Expression = txtText.Text }
                                              : null;

        //----Checked filter

        _paging.Fields["Checked"].Filter = ddlChecked.SelectedValue != "-1"
                                               ? new EqualFieldFilter { ParamName = "@Checked", Value = ddlChecked.SelectedValue }
                                               : null;


        //---AddDate filter
        var dfilter = new DateTimeRangeFieldFilter { ParamName = "@dateRewiews" };
        if (!string.IsNullOrEmpty(txtDateFrom.Text))
        {
            DateTime? d = default(DateTime?);
            try
            {
                d = DateTime.Parse(txtDateFrom.Text);
            }
            catch (Exception ex)
            {
                //Debug.LogError(ex, sender, e);
                Debug.LogError(ex);
            }
            if (d.HasValue)
            {
                var dt = new DateTime(d.Value.Year, d.Value.Month, d.Value.Day, 0, 0, 0, 0);
                dfilter.From = dt;
            }
        }

        if (!string.IsNullOrEmpty(txtDateTo.Text))
        {
            DateTime? d = default(DateTime?);
            try
            {
                d = DateTime.Parse(txtDateTo.Text);
            }
            catch (Exception ex)
            {
                //Debug.LogError(ex, sender, e);
                Debug.LogError(ex);
            }
            if (d.HasValue)
            {
                var dt = new DateTime(d.Value.Year, d.Value.Month, d.Value.Day, 23, 59, 59, 99);
                dfilter.To = dt;
            }
        }

        if (dfilter.From.HasValue | dfilter.To.HasValue)
        {
            _paging.Fields["AddDate"].Filter = dfilter;
        }
        else
        {
            _paging.Fields["AddDate"].Filter = null;
        }
    }

    protected void pn_SelectedPageChanged(object sender, EventArgs e)
    {
        _paging.CurrentPageIndex = pageNumberer.CurrentPageIndex;
    }

    protected void lbDeleteSelected_Click(object sender, EventArgs e)
    {
        if ((_selectionFilter == null) || (_selectionFilter.Values == null))
        {
            return;
        }

        if (!_inverseSelection)
        {
            foreach (var id in _selectionFilter.Values)
            {
                ReviewService.DeleteReview(Convert.ToInt32(id));
            }
        }
        else
        {
            var itemsIds = _paging.ItemsIds<int>("ReviewId as ID");
            foreach (int id in itemsIds.Where(iId => !_selectionFilter.Values.Contains(iId.ToString())))
            {
                ReviewService.DeleteReview(id);
            }
        }
    }

    protected void lbSetChecked_Click(object sender, EventArgs e)
    {
        if ((_selectionFilter == null) || (_selectionFilter.Values == null))
        {
            return;
        }

        if (!_inverseSelection)
        {
            foreach (var id in _selectionFilter.Values)
            {
                ReviewService.CheckReview(Convert.ToInt32(id), true);
            }
        }
        else
        {
            var itemsIds = _paging.ItemsIds<int>("ReviewId as ID");
            foreach (int id in itemsIds.Where(iId => !_selectionFilter.Values.Contains(iId.ToString())))
            {
                ReviewService.CheckReview(id, true);
            }
        }
    }

    protected void lbSetNotChecked_Click(object sender, EventArgs e)
    {
        if ((_selectionFilter == null) || (_selectionFilter.Values == null))
        {
            return;
        }
        if (!_inverseSelection)
        {
            foreach (var id in _selectionFilter.Values)
            {
                ReviewService.CheckReview(Convert.ToInt32(id), false);
            }
        }
        else
        {
            var itemsIds = _paging.ItemsIds<int>("ReviewId as ID");
            foreach (int id in itemsIds.Where(iId => !_selectionFilter.Values.Contains(iId.ToString())))
            {
                ReviewService.CheckReview(id, false);
            }
        }
    }

    protected string GetRequestUrl(int commentId)
    {
        var review = ReviewService.GetReview(commentId);
        return review != null ? ReviewService.GetEntityUrl(review.EntityId, review.Type) : string.Empty;
    }

    protected string GetRequestName(int commentId)
    {
        var entity = ReviewService.GetReviewEntity(commentId);
        return entity != null ? entity.Name : string.Empty;
    }
}