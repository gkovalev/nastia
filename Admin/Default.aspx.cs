//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using AdvantShop.Configuration;
using AdvantShop.Orders;
using AdvantShop.SaasData;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Customers;

public partial class _Default : Page
{
    protected override void InitializeCulture()
    {
        AdvantShop.Localization.Culture.InitializeCulture();
    }

    private void MsgErr(string messageText)
    {
        Message.Visible = true;
        Message.Text += "<br/>" + messageText + "<br/>";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SaasTab.Visible = SaasDataService.IsSaasEnabled;
        Page.Title = SettingsMain.ShopName;

        Customer _customer = CustomerSession.CurrentCustomer;

        if (_customer.CustomerRole == Role.Moderator)
        {
            var actions = RoleActionService.GetCustomerRoleActionsByCustomerId(_customer.Id);

            if (actions.Any(a => a.Key == RoleActionKey.DisplayOrders && a.Enabled))
            {
                OrderTab.Visible = true;
                GoogleCharts2.Visible = true;
                FinanceStatistics.Visible = true;
            }
            else
            {
                OrderTab.Visible = false;
                GoogleCharts2.Visible = false;
                FinanceStatistics.Visible = false;
            }

            ReviewsBlock1.Visible = actions.Any(a => a.Key == RoleActionKey.DisplayComments && a.Enabled);
            tblDashBoard.Visible = actions.Any(a => (a.Key == RoleActionKey.DisplayOrders || a.Key == RoleActionKey.DisplayCatalog || a.Key== RoleActionKey.DisplayComments) && a.Enabled);
            NotepadTab.Visible = false;
        }

        if (OrderTab.Visible)
        {
            TabContainer.OnClientActiveTabChanged = "chartOrder";
        }
    }

    protected string RenderBigOrdersChartData()
    {
        DateTime minDate = DateTime.Now.AddMonths(-1).Date;
        DateTime maxDate = DateTime.Now.AddDays(2).Date;
        var result = new StringBuilder();
        Dictionary<DateTime, decimal> sums = OrderStatisticsService.GetOrdersSumByDays(minDate, maxDate);
        Dictionary<DateTime, decimal> profits = OrderStatisticsService.GetOrdersProfitByDays(minDate, maxDate);
        if (sums == null)
        {
            return "";
        }
        string[] monthsArr = Resources.Resource.Admin_Charts_Months.Split(',');
        for (DateTime i = minDate.Date; i < maxDate; )
        {
            decimal sum = 0;
            decimal profit = 0;
            if (sums.ContainsKey(i))
            {
                sum = sums[i];
            }
            if (profits.ContainsKey(i))
            {
                profit = profits[i];
            }
            result.AppendFormat("['{0}', {1}, {2}],", i.Date.ToString("dd.MM"), sum.ToString(System.Globalization.CultureInfo.InvariantCulture), profit.ToString(System.Globalization.CultureInfo.InvariantCulture));
            i = i.AddDays(1);
        }
        result.Remove(result.ToString().LastIndexOf(','), 1);
        return result.ToString();
    }
}