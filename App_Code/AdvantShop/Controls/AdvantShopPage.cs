//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop.Configuration;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Customers;
using AdvantShop.SEO;
using AdvantShop.SaasData;
using AdvantShop.Core;

namespace AdvantShop.Controls
{
    public class AdvantShopPage : Page
    {

        public void ShowMessage(Notify.NotifyType notifyType, string message)
        {
            var masterPage = (IMasterPage)(Master);
            if (masterPage != null) masterPage.ShowMessage(notifyType, message);
        }

        protected override void InitializeCulture()
        {
            Localization.Culture.InitializeCulture();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //// Проверяем можно ли обработать повторное посещение
            //if (!IsPostBack)
            //{
            //    // Блокируем запросы без кукисов
            //    if (IsFirstVisit())
            //    {
            //        if (!ActionValidator.IsValid(ActionValidator.ActionTypeEnum.FirstVisit))
            //            Response.End();
            //    }
            //    else
            //    {
            //        if (!ActionValidator.IsValid(ActionValidator.ActionTypeEnum.ReVisit))
            //            Response.End();
            //    }
            //}
            //else
            //{
            //    // Ограничение количества постбэков
            //    if (!ActionValidator.IsValid(ActionValidator.ActionTypeEnum.Postback))
            //        Response.End();
            //}

            if (AppServiceStartAction.state != DataBaseService.PingDbState.NoError)
            {
                AdvantShop.Core.SessionServices.StartSession(HttpContext.Current);
                return;
            }

            if (!System.IO.File.Exists(SettingsGeneral.InstallFilePath))
            {
                Response.Redirect(UrlService.GetAbsoluteLink("install/default.aspx"));
            }

            if ((CustomerSession.CurrentCustomer.IsVirtual) || Demo.IsDemoEnabled || Trial.IsTrialEnabled || SaasDataService.IsSaasEnabled)
            {
                return;
            }

            if (!SettingsLic.ActiveLic && !Request.CurrentExecutionFilePath.Contains("err404.aspx") && !Request.CurrentExecutionFilePath.Contains("adv-admin.aspx"))
            {
                Response.Redirect(UrlService.GetAbsoluteLink("liccheck.aspx"));
            }
        }

        //public bool IsFirstVisit()
        //{
        //    if (Session["IsFirstVisit"] == null)
        //    {

        //        Session["IsFirstVisit"] = false;

        //        return true;

        //    }
        //    return false;
        //}


        protected void Error404()
        {
            Server.Transfer(GetAbsoluteLink("/err404.aspx"));
        }

        protected string GetAbsoluteLink(string link)
        {
            return UrlService.GetAbsoluteLink(link);
        }

        protected void RedirectToMainPage()
        {
            Page.Response.Redirect(GetAbsoluteLink("/"));
        }

        protected void Redirect(string link)
        {
            Page.Response.Redirect(GetAbsoluteLink(link));
        }

        protected void Redirect(string link, bool endResponse)
        {
            Page.Response.Redirect(GetAbsoluteLink(link), endResponse);
        }

        #region SetMeta

        protected void SetMeta(MetaInfo meta, string name)
        {
            MetaInfo newMeta = meta != null ? meta.DeepClone() : MetaInfoService.GetDefaultMetaInfo(); // Creating new object to modify - keeping original Meta for cache
            SetMetaTags(MetaInfoService.GetFormatedMetaInfo(newMeta, name));
        }

        private void SetMetaTags(MetaInfo meta)
        {
            var contr = (Literal)Page.Controls[0].FindControl("headMeta");
            if (contr == null) return;
            var strmeta = new StringBuilder();
            strmeta.Append("\n");
            strmeta.AppendFormat("<title>{0}</title>", meta.Title);
            strmeta.Append("\n");
            strmeta.AppendFormat("<meta name='Description' content='{0}'/>", meta.MetaDescription);
            strmeta.Append("\n");
            strmeta.AppendFormat("<meta name='Keywords' content='{0}'/>", meta.MetaKeywords);
            contr.Text = strmeta.ToString();
        }
        #endregion
    }
}