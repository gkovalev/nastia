using System;
using System.Web.UI.WebControls;
using AdvantShop.Modules;

namespace Advantshop.UserControls.Modules.StoreFaqs
{
    public partial class Admin_StoreFaqsManager : System.Web.UI.UserControl
    {
        private const string ModuleName = "StoreFaqs";

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            lvFaqs.DataSource = StoreFaqRepository.GetStoreFaqs();
            lvFaqs.DataBind();
        }

        protected void lvFaqsItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "deleteFaq":
                    int FaqId;
                    if (Int32.TryParse(e.CommandArgument.ToString(), out FaqId))
                    {
                        StoreFaqRepository.DeleteStoreFaqsById(FaqId);
                    }
                    break;
            }
        }
    }
}