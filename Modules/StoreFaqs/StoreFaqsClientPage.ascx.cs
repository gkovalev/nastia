using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Text;
using System.Web;
using AdvantShop.Modules;
using AdvantShop.Customers;

namespace Advantshop.UserControls.Modules.StoreFaqs
{
    public partial class StoreFaqsClientPage : System.Web.UI.UserControl
    {
        protected const string _moduleStringId = "StoreFaqs";

        protected bool ShowRatio;

        private string childTemplate = "<div class=\"shop-Faqs-child-wrap\" data-sr-item=\"{0}\" data-sr-parentid=\"{1}\">" +
            "<div class=\"shop-Faqs-row\"><span class=\"shop-Faqs-name\">{2}</span> <span class=\"shop-Faqs-date\">{3}</span></div>" +
            "<div class=\"shop-Faqs-row shop-Faqs-text\">{4}</div>" +
            "<div data-sr-form-btn=\"true\">{5} {6}</div>" +
            "{7}" +
            "</div>";

        private int _currentPage = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!ModuleSettingsProvider.GetSettingValue<bool>("EnableStoreFaqs", _moduleStringId))
            {
                Response.Redirect("~/");
            }

            if (CustomerSession.CurrentCustomer.IsAdmin)
            {
                adminPAnel.Visible = true;
            }

            ShowRatio = ModuleSettingsProvider.GetSettingValue<bool>("ShowRatio", _moduleStringId);

            dp.PageSize = ModuleSettingsProvider.GetSettingValue<int>("PageSize", _moduleStringId);

            Bind();

            ltHeader.Text = String.Format("{0} ({1})", ModuleSettingsProvider.GetSettingValue<string>("PageTitle", _moduleStringId),
                          dp.TotalRowCount);
        }

        protected void btnClick(object sender, EventArgs e)
        {
            const string tpl = "<div class=\"error-item\">{0}</div>";
            var errList = new StringBuilder();

            int scope;
            var resultParse = int.TryParse(hfScope.Value, out scope);

            if (!resultParse)
            {
                errList.AppendFormat(tpl, GetLocalResourceObject("StoreFaqs_InvalidScope"));
            }

            if (string.IsNullOrEmpty(txtEmail.Text.Trim()))
            {
                errList.AppendFormat(tpl, GetLocalResourceObject("StoreFaqs_InvalidEmail"));
            }

            if (string.IsNullOrEmpty(txtFaqerName.Text.Trim()))
            {
                errList.AppendFormat(tpl, GetLocalResourceObject("StoreFaqs_InvalidName"));
            }

            if (string.IsNullOrEmpty(txtFaq.Text.Trim()))
            {
                errList.AppendFormat(tpl, GetLocalResourceObject("StoreFaqs_InvalidFaq"));
            }

            if (errList.Length > 0)
            {
                liError.InnerHtml = errList.ToString();
            }
            else
            {

                StoreFaqRepository.AddStoreFaq(new StoreFaq
                    {
                        Moderated = false,
                        Rate = ShowRatio ? scope : 0,
                        ParentId = 0,
                        FaqerEmail = HttpUtility.HtmlEncode(txtEmail.Text),
                        FaqerName = HttpUtility.HtmlEncode(txtFaqerName.Text),
                        Faq = HttpUtility.HtmlEncode(txtFaq.Text)
                    });

                txtFaqerName.Text = "";
                txtEmail.Text = "";
                txtFaq.Text = "";
            }

            Response.Redirect(Request.RawUrl);
        }

        protected string RenderChild(List<StoreFaq> childs)
        {
            var html = new StringBuilder();

            for (var i = 0; i < childs.Count; i++)
            {
                html.AppendFormat(childTemplate,
                                  childs[i].Id,
                                  childs[i].ParentId,
                                  childs[i].FaqerName,
                                  childs[i].DateAdded.ToString("dd MMMM yyyy, HH:mm"),
                                  childs[i].Faq,
                                  "<a href=\"javascript:void(0);\" data-sr-reply=\"true\">" +
                                  GetLocalResourceObject("StoreFaqs_Answer") + "</a>",
                                  "",
                                  childs[i].HasChild ? RenderChild(childs[i].ChildrenFaqs) : "");
            }

            return html.ToString();
        }

        protected void lvStoreFaqs_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            dp.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);

            _currentPage = (e.StartRowIndex / e.MaximumRows) + 1;

            Bind();
        }


        protected void lvStoreFaqs_PreRender(object sender, EventArgs e)
        {
            lvStoreFaqs.DataBind();
            DataPagerControlsVisible();
        }

        private void Bind()
        {

            lvStoreFaqs.DataSource = StoreFaqRepository.GetStoreFaqsByParentId(0,
                                                         ModuleSettingsProvider
                                                             .GetSettingValue<bool>(
                                                                 "ActiveModerateStoreFaqs",
                                                                 _moduleStringId));
            lvStoreFaqs.DataBind();
        }

        private void DataPagerControlsVisible()
        {
            dp.Visible = dp.TotalRowCount > dp.PageSize;

            var keyPrevious = dp.Controls[0].Controls[0];
            var previousLink = (LinkButton)dp.Controls[1].Controls[0];
            var nextLink = (LinkButton)dp.Controls[3].Controls[0];
            var keyNext = dp.Controls[4].Controls[0];

            previousLink.Attributes["data-sr-paging-prev"] = "true";
            nextLink.Attributes["data-sr-paging-next"] = "true";

            previousLink.Visible = keyPrevious.Visible = _currentPage > 1;
            nextLink.Visible = keyNext.Visible = (_currentPage * dp.PageSize) < dp.TotalRowCount;

        }

    }
}