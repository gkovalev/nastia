//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop.Controls;
using AdvantShop.Helpers;
using AdvantShop.Mails;
using AdvantShop.SEO;
using Resources;

public partial class Subscribe : AdvantShopPage
{

    protected void btnSubscribe_Click(object sender, EventArgs e)
    {
        try
        {
            if (SubscribeService.IsExistInSubscribeEmails(txtEmail.Text))
            {
                //MultiView1.SetActiveView(ViewEmailSend);
                //lblError.Visible = true;
                ShowMessage(Notify.NotifyType.Error, Resources.Resource.Client_Subscribe_EmailAlreadyReg);
                return;
            }

            if (SubscribeService.IsExistInCustomerEmails(txtEmail.Text))
            {
                //MultiView1.SetActiveView(ViewEmailSend);
                //lblError.Visible = true;
                ShowMessage(Notify.NotifyType.Error, Resources.Resource.Client_Subscribe_EmailAlreadyReg);
                return;
            }

            string strActivateCode = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + DateTime.Now.Second.ToString();
            string strDeactivateCode = DateTime.Now.Millisecond.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Second.ToString();

            SubscribeService.SubscribeInsertEmail(txtEmail.Text, strActivateCode, strDeactivateCode);
            string strLink = "<a href=\'" + AdvantShop.Configuration.SettingsMain.SiteUrl + "/subscribe.aspx?ID=" + strActivateCode + "\'>" + AdvantShop.Configuration.SettingsMain.SiteUrl + "/subscribe.aspx?ID=" + strActivateCode + "</a>";
            var clsParam = new ClsMailParamOnSubscribeActivate { Link = strLink };
            string message = SendMail.BuildMail(clsParam);


            SendMail.SendMailNow(txtEmail.Text, Resources.Resource.Client_Subscribe_NewSubscribe, message, true);

            MultiView1.SetActiveView(ViewEmailSend);
            //--------------------------

            txtEmail.Text = string.Empty;
            lblInfo.Visible = true;
            lblInfo.Text = Resources.Resource.Client_Subscribe_RegSuccess + @" <br /><br />" +
                           Resources.Resource.Client_Subscribe_Instruction;
            //ShowMessage(Notify.NotifyType.Notice, Resources.Resource.Client_Subscribe_RegSuccess + @" <br /><br />" + Resources.Resource.Client_Subscribe_Instruction);
            //lblInfo.ForeColor = System.Drawing.Color.Black;
        }
        catch (Exception ex)
        {
            AdvantShop.Diagnostics.Debug.LogError(ex);
            //lblError.Visible = true;
            ShowMessage(Notify.NotifyType.Error, ex.Message + @" at Subscribe");
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SetMeta(new MetaInfo(string.Format("{0} - {1}", AdvantShop.Configuration.SettingsMain.ShopName, Resources.Resource.Client_Subscribe_NewSubscribe)), null);

        if (!IsPostBack)
        {
            if (Request["emailtosubscribe"] != null)
            {
                txtEmail.Text = Request["emailtosubscribe"];
                if (ValidationHelper.IsValidEmail(txtEmail.Text))
                {
                    btnSubscribe_Click(sender, e);
                }
            }

            if (Page.Request["ID"] != null)
            {
                //lblActivated.Visible = true;
                //MultiView1.SetActiveView(ViewEmailSend);
                try
                {
                    var temp = SubscribeService.SubscribeGetEmailCountByActivateCode(Page.Request["ID"]);
                    if (temp != 1)
                    {
                        ShowMessage(Notify.NotifyType.Error, Resources.Resource.Client_Subscribe_DamageLink);
                        //lblActivated.Text = Resources.Resource.Client_Subscribe_DamageLink;
                        //lblActivated.ForeColor = System.Drawing.Color.Red;
                        return;
                    }
                    SubscribeService.SubscribeUpdateEnableByActivateCode(Page.Request["ID"]);
                    MultiView1.SetActiveView(ViewEmailSend);
                    lblInfo.Text = Resource.Client_Subscribe_Activated;
                    lblInfo.Visible = true;
                    //lblActivated.Text = Resources.Resource.Client_Subscribe_Activated;
                    //lblActivated.ForeColor = System.Drawing.Color.Black;
                }
                catch (Exception ex)
                {
                    AdvantShop.Diagnostics.Debug.LogError(ex);
                    //lblError.Visible = true;
                    ShowMessage(Notify.NotifyType.Error, ex.Message + @" at Subscribe");
                }
            }
        }
    }
}