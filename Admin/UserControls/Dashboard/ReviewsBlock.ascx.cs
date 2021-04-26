using System;
using System.Data;
using System.Web.UI.WebControls;
using AdvantShop.CMS;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;

public partial class UserControls_ReviewsBlock : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void gvReviews_Command(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "DeleteReview")
        {
            ReviewService.DeleteReview(Convert.ToInt32(e.CommandArgument));
            upReviews.Update();
        }
    }

    protected void sdsReviews_Init(object sender, EventArgs e)
    {
        sdsReviews.ConnectionString = AdvantShop.Connection.GetConnectionString();
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

    protected void rptReviews_Command(object source, RepeaterCommandEventArgs e)
    {
        var commentId = Convert.ToInt32(e.CommandArgument);

        switch (e.CommandName)
        {
            case "Delete":
                ReviewService.DeleteReview(commentId);
                rptReviews.DataBind();
                break;

            case "Accept":
                ReviewService.CheckReview(commentId, true);
                rptReviews.DataBind();
                break;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        txtName.Text = string.Empty;
        txtText.Text = string.Empty;
        txtEmail.Text = string.Empty;
        hfID.Value = string.Empty;
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(hfID.Value))
        {
            return;
        }

        var commentId = int.Parse(hfID.Value);
        var comment = ReviewService.GetReview(commentId);
        comment.Name = txtName.Text;
        comment.Email = txtEmail.Text;
        comment.Text = txtText.Text;
        ReviewService.UpdateReview(comment);

        ModalPopupExtender1.Hide();
        rptReviews.DataBind();
        upReviews.Update();
    }

    protected void rptReviews_ItemDataBind(object sender, RepeaterItemEventArgs e)
    {
        if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
        {
            try
            {
                var commentId = Convert.ToInt32(((DataRowView)e.Item.DataItem)["ReviewId"]);
                var reviewEntity = ReviewService.GetReviewEntity(commentId);
                var url = ReviewService.GetEntityUrl(reviewEntity.ReviewEntityId, reviewEntity.Type);

                var ibPhoto = ((HyperLink)e.Item.FindControl("ibPhoto"));
                var requestHyperLink = ((HyperLink)e.Item.FindControl("requestHyperLink"));

                if (!string.IsNullOrEmpty(reviewEntity.Photo))
                {
                    ibPhoto.ImageUrl = FoldersHelper .GetImageProductPath(ProductImageType.Small, reviewEntity.Photo, false);
                    ibPhoto.Text = reviewEntity.PhotoDescription;
                    ibPhoto.NavigateUrl = url;
                    ibPhoto.Attributes.Add("abbr", FoldersHelper.GetImageProductPath(ProductImageType.Middle, reviewEntity.Photo, false));
                }
                else
                {
                    ibPhoto.Visible = false;
                }

                requestHyperLink.NavigateUrl = url;
                requestHyperLink.Text = reviewEntity.Name;
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                MsgErr("Unable to load product photo");
            }
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (AdvantShop.Configuration.SettingsCatalog.ModerateReviews)
        {
            rptReviews.DataBind();
            this.Visible = rptReviews.Items.Count > 0;
        }
        else
        {
            upReviews.Visible = false;

        }
    }
}