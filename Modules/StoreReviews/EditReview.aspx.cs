using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop.Modules;
using System.Text;

public partial class Modules_StoreReviews_EditReview : System.Web.UI.Page
{
    private int reviewId = 0;

    private StoreReview review;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Request["id"]))
        {
            return;
        }
        if (!Int32.TryParse(Request["id"], out reviewId))
        {
            return;
        }

        if ((review = StoreReviewRepository.GetStoreReview(reviewId)) == null)
        {
            return;
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        LoadData();
    }

    protected void LoadData()
    {
        if (review == null)
        {
            return;
        }

        txtDateAdded.Text = review.DateAdded.ToString("yyyy.MM.dd HH:mm");
        txtEmail.Text = review.ReviewerEmail;
        rblRating.SelectedValue = review.Rate.ToString();
        txtReview.Text = review.Review;
        ckbModerated.Checked = review.Moderated;
    }
    protected void btnSaveClick(object sender, EventArgs e)
    {
        if (review == null)
        {
            return;
        }

        DateTime date = review.DateAdded;
        try
        {
            date = DateTime.ParseExact(txtDateAdded.Text, "yyyy.MM.dd HH:mm", CultureInfo.InvariantCulture);
        }
        catch (Exception ex)
        {
            lError.Visible = true;
            return;
        }


        review.DateAdded = date;
        review.Moderated = ckbModerated.Checked;
        review.Rate = string.IsNullOrEmpty(rblRating.SelectedValue) ? 0 : Convert.ToInt32(rblRating.SelectedValue);
        review.Review = txtReview.Text;
        review.ReviewerEmail = txtEmail.Text;

        StoreReviewRepository.UpdateStoreReview(review);

        var jScript = new StringBuilder();
        jScript.Append("<script type=\'text/javascript\' language=\'javascript\'> ");
        if (string.IsNullOrEmpty(string.Empty))
            jScript.Append("window.opener.location.reload();");
        else
            jScript.Append("window.opener.location =" + string.Empty);
        jScript.Append("self.close();");
        jScript.Append("</script>");
        Type csType = this.GetType();
        ClientScriptManager clScriptMng = this.ClientScript;
        clScriptMng.RegisterClientScriptBlock(csType, "Close_window", jScript.ToString());
    }
}