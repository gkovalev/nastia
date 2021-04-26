using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop.Modules;
using System.Text;

public partial class Modules_StoreFaqs_EditFaq : System.Web.UI.Page
{
    private int FaqId = 0;

    private StoreFaq Faq;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Request["id"]))
        {
            return;
        }
        if (!Int32.TryParse(Request["id"], out FaqId))
        {
            return;
        }

        if ((Faq = StoreFaqRepository.GetStoreFaq(FaqId)) == null)
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
        if (Faq == null)
        {
            return;
        }

        txtDateAdded.Text = Faq.DateAdded.ToString("yyyy.MM.dd HH:mm");
        txtEmail.Text = Faq.FaqerEmail;
        rblRating.SelectedValue = Faq.Rate.ToString();
        txtFaq.Text = Faq.Faq;
        ckbModerated.Checked = Faq.Moderated;
    }
    protected void btnSaveClick(object sender, EventArgs e)
    {
        if (Faq == null)
        {
            return;
        }

        DateTime date = Faq.DateAdded;
        try
        {
            date = DateTime.ParseExact(txtDateAdded.Text, "yyyy.MM.dd HH:mm", CultureInfo.InvariantCulture);
        }
        catch (Exception ex)
        {
            lError.Visible = true;
            return;
        }


        Faq.DateAdded = date;
        Faq.Moderated = ckbModerated.Checked;
        Faq.Rate = string.IsNullOrEmpty(rblRating.SelectedValue) ? 0 : Convert.ToInt32(rblRating.SelectedValue);
        Faq.Faq = txtFaq.Text;
        Faq.FaqerEmail = txtEmail.Text;

        StoreFaqRepository.UpdateStoreFaq(Faq);

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