//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop.CMS;
using AdvantShop.Customers;

public partial class UserControls_StaticBlock : System.Web.UI.UserControl
{
    public string SourceKey { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(SourceKey))
        {
            //const string staticBlock = "<div class=\"staticBlock\">{0}</div>";
            //var temp = StaticBlockService.GetPagePartByKeyWithCache(SourceKey);
            //if (temp != null && temp.Enabled)
            //{
            //    ltOutputString.Text = CustomerSession.CurrentCustomer.IsAdmin ? string.Format(staticBlock, temp.Content) : temp.Content;
            //}
            //else if (CustomerSession.CurrentCustomer.IsAdmin)
            //{
            //    ltOutputString.Text = string.Format(staticBlock, "<div style=\"padding:10px;text-align:center;\">Пустой статический блок</div>");
            //}

            var temp = StaticBlockService.GetPagePartByKeyWithCache(SourceKey);
            if (temp != null && temp.Enabled)
                ltOutputString.Text = temp.Content;
        }
    }
}