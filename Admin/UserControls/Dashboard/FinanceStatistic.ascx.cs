using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AdvantShop;
using AdvantShop.Repository.Currencies;

public partial class Admin_UserControls_FinanceStatistic : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected string GetPrice(object dbObject)
    {
        return dbObject == DBNull.Value ? AdvantShop.Catalog.CatalogService.GetStringPrice(0, CurrencyService.CurrentCurrency.Value, CurrencyService.CurrentCurrency.Iso3) : AdvantShop.Catalog.CatalogService.GetStringPrice(Convert.ToDecimal(dbObject), CurrencyService.CurrentCurrency.Value, CurrencyService.CurrentCurrency.Iso3);
    }
    protected string GetPersent(object dbObject)
    {
        return dbObject == DBNull.Value ? "0%" : Convert.ToDecimal(dbObject).ToString("F2") + "%";
    }
    protected void sds_Init(object sender, EventArgs e)
    {
        ((SqlDataSource)sender).ConnectionString = Connection.GetConnectionString();
    }

    private readonly string[] _hlinkLabels = {
                                       Resources.Resource.Admin_FinanceStatistics_Today,
                                       Resources.Resource.Admin_FinanceStatistics_Yesterday,
                                       Resources.Resource.Admin_FinanceStatistics_Month,
                                       Resources.Resource.Admin_FinanceStatistics_AllTime
                                   };
    private readonly string[] _hlinkUrls = {
                                       "~/admin/OrderSearch.aspx?Filter=today",
                                       "~/admin/OrderSearch.aspx?Filter=yesterday",
                                       "~/admin/OrderSearch.aspx?Filter=lastmonth",
                                       "~/admin/OrderSearch.aspx"
                                   };
    protected void rpt_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            ((HyperLink)e.Item.FindControl("hlLink")).Text = _hlinkLabels[e.Item.ItemIndex] + @" (" + ((DataRowView)e.Item.DataItem)["Count"].ToString() + @")";
            ((HyperLink)e.Item.FindControl("hlLink")).NavigateUrl = _hlinkUrls[e.Item.ItemIndex];
            if (e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ((HtmlTableRow)e.Item.FindControl("trItem")).Attributes["class"] = "row2";
            }
            //lbl.Text = ((DataRowView) e.Item.DataItem)["Count"].ToString();

            var lbl = (Label)e.Item.FindControl("lblSum");
            lbl.Text = GetPrice(((DataRowView)e.Item.DataItem)["Sum"]);

            lbl = (Label)e.Item.FindControl("lblSumDiscount");
            lbl.Text = GetPrice(((DataRowView)e.Item.DataItem)["SumWDiscount"]);

            lbl = (Label)e.Item.FindControl("lblCost");
            lbl.Text = GetPrice(((DataRowView)e.Item.DataItem)["Cost"]);

            lbl = (Label)e.Item.FindControl("lblTax");
            lbl.Text = GetPrice(((DataRowView)e.Item.DataItem)["Tax"]);

            lbl = (Label)e.Item.FindControl("lblShipping");
            lbl.Text = GetPrice(((DataRowView)e.Item.DataItem)["Shipping"]);

            lbl = (Label)e.Item.FindControl("lblProfit");
            lbl.Text = GetPrice(((DataRowView)e.Item.DataItem)["Profit"]);

            lbl = (Label)e.Item.FindControl("lblProfitability");
            lbl.Text = GetPersent(((DataRowView)e.Item.DataItem)["Profitability"]);

        }
    }

}