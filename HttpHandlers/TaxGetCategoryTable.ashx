<%@ WebHandler Language="C#" Class="TaxGetCategoryTable" %>

using System;
using System.IO;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop.Catalog;
using AdvantShop.Taxes;

public class TaxGetCategoryTable : IHttpHandler, IReadOnlySessionState
{
    public void ProcessRequest(HttpContext context)
    {
        int catId = Convert.ToInt32(context.Request["catId"]);
        int depth = int.Parse(context.Request["depth"]);
        int parentCategoryId = string.IsNullOrEmpty(context.Request["parentCategoryId"]) ? 0 : Convert.ToInt32(context.Request["parentCategoryId"]);
        var cat = CategoryService.GetCategory(catId);

        var table = new Table { ID = "CatalogDataTree" };
        TaxLasyLoad.ProcessCategoryRow(cat, depth, parentCategoryId, table, true, Convert.ToInt32(context.Request["TaxID"]));

        var sw = new StringWriter();
        var hdw = new HtmlTextWriter(sw);
        table.Controls.RemoveAt(0);

        table.RenderControl(hdw);

        string result = sw.ToString();
        result = result.Substring(39, result.Length - 39);
        result = result.Substring(0, result.Length - 8);

        context.Response.Write(result);
        context.Response.End();
    }

    public bool IsReusable
    {
        get { return false; }
    }
}