//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop.Configuration;
using AdvantShop.Controls;
using AdvantShop.Mails;
using AdvantShop.SEO;

public partial class SubscribeDeactivate : AdvantShopPage
{
    protected void btnDeactivate_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtEmailAdress.Text))
        {
            ShowMessage(Notify.NotifyType.Error, Resources.Resource.Client_SubscribeDeactivate_NoEmail);
            return;
        }

        try
        {
            if (!SubscribeService.IsExistInSubscribeEmails(txtEmailAdress.Text))
            {
                ShowMessage(Notify.NotifyType.Error, Resources.Resource.Client_SubscribeDeactivate_EmailNotFound);
                return;
            }

            if (!string.IsNullOrEmpty(txtDeactivateReason.Text))
                SubscribeService.SubscribeInsertDeactivateReason(txtDeactivateReason.Text);

            var strDeactivateCode = SubscribeService.SubscribeGetDectivateCodeByEmail(txtEmailAdress.Text);

            string strLink = "<a href=\'" + SettingsMain.SiteUrl + "/subscribedeactivate.aspx?id=" + strDeactivateCode + "\'>" + SettingsMain.SiteUrl + "/subscribedeactivate.aspx?id=" + strDeactivateCode + "</a>";

            var clsParam = new ClsMailParamOnSubscribeDeactivate { Link = strLink };

            string message = SendMail.BuildMail(clsParam);

            SendMail.SendMailNow(txtEmailAdress.Text, Resources.Resource.Client_SubscribeDeactivate_DeactivateNews, message, true);

            MultiView1.SetActiveView(viewMessage);
            lblInfo.Text = Resources.Resource.Client_SubscribeDeactivate_EmailSent;
            lblInfo.Visible = true;
        }
        catch (Exception ex)
        {
            AdvantShop.Diagnostics.Debug.LogError(ex);
            ShowMessage(Notify.NotifyType.Error, ex.Message + @" at Subscribe Diactivate");
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SetMeta(new MetaInfo(string.Format("{0} - {1}", SettingsMain.ShopName, Resources.Resource.Client_SubscribeDeactivate_DeleteSubscribe)), null);

        if (!IsPostBack)
        {
            if (Page.Request["ID"] != null)
            {
                try
                {
                    var temp = SubscribeService.SubscribeGetEmailCountByDeactivateCode(Page.Request["ID"]);
                    if (temp != 1)
                    {
                        ShowMessage(Notify.NotifyType.Error, Resources.Resource.Client_SubscribeDeactivate_DamageLink);
                        return;
                    }

                    var email = SubscribeService.SubscribeDeleteEmail(Page.Request["ID"]);
                    if (SettingsMailChimp.MailChimpActive)
                    {
                        MailChimp.UnsubscribeListMember(SettingsMailChimp.MailChimpId,
                                                        SettingsMailChimp.MailChimpRegUsersList, email);
                    }
                    MultiView1.SetActiveView(viewMessage);
                    lblInfo.Text = Resources.Resource.Client_SubscribeDeactivate_Deactivated;
                    lblInfo.Visible = true;
                }
                catch (Exception ex)
                {
                    AdvantShop.Diagnostics.Debug.LogError(ex);
                    ShowMessage(Notify.NotifyType.Error, ex.Message + " at Subscribe <br/>");
                }
            }
        }
    }
}
