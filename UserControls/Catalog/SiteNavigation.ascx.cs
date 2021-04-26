//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop.Catalog;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Diagnostics;
using Resources;

public partial class UserControls_SiteNavigation : UserControl
{
    // render navigation on client
    public void BuildNavigationClient(int id)
    {
        pnlNavi.Controls.Clear();

        var hlMainPage = new HyperLink
            {
                //todo заменить явное указание каталога на автоматическое
                NavigateUrl = UrlService.GetLink(ParamType.Category, "catalog", 0),
                Text = Resource.Client_MasterPage_Catalog,
                CssClass = "client_SiteNavigation_text"
            };

        pnlNavi.Controls.Add(hlMainPage);
        try
        {
            var categoryList = CategoryService.GetParentCategories(id);

            for (var i = categoryList.Count - 1; i >= 0; i--)
            {
                var lblSeparator = new Label { Text = @" > ", CssClass = "client_SiteNavigation_Separator" };

                pnlNavi.Controls.Add(lblSeparator);

                var hl = new HyperLink
                             {
                                 NavigateUrl = UrlService.GetLink(ParamType.Category, categoryList[i].UrlPath, categoryList[i].CategoryId),
                                 Text = Convert.ToString(categoryList[i].Name),
                                 CssClass = "client_SiteNavigation_text"
                             };
                pnlNavi.Controls.Add(hl);
            }
        }
        catch (Exception ex)
        {
            //Debug.LogError(ex, id);
            Debug.LogError(ex);
        }
    }

    // render navigation on admin side
    public void BuildNavigationAdmin(int categoryID) // Modify for Admin
    {
        pnlNavi.Controls.Clear();
        var hlM = new HyperLink
                     {
                         NavigateUrl = UrlService.GetAdminAbsoluteLink("catalog.aspx?CategoryID=0"),
                         CssClass = "Link",
                         Text = Resource.Client_MasterPage_Catalog
                     };
        pnlNavi.Controls.Add(hlM);
        var categoryList = CategoryService.GetParentCategories(categoryID);
        for (var i = categoryList.Count - 1; i >= 0; i--)
        {
            var lblSeparator = new Label { CssClass = "Link", Text = @" > " };

            pnlNavi.Controls.Add(lblSeparator);

            var hl = new HyperLink
            {
                NavigateUrl = UrlService.GetAdminAbsoluteLink("catalog.aspx?CategoryID=" + categoryList[i].CategoryId),
                CssClass = "Link",
                Text = Convert.ToString(categoryList[i].Name)
            };
            pnlNavi.Controls.Add(hl);
        }
    }
}