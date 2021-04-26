//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using AdvantShop.Customers;
using AdvantShop.Orders;
using AdvantShop.Security;
using AdvantShop.Statistic;
using Quartz;

namespace AdvantShop.Core.Scheduler
{
    public class ClearExpiredJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            OrderConfirmationService.DeleteExpired();
            RecentlyViewService.DeleteExpired();
            InternalServices.DeleteExpiredAppRestartLogData();
            Secure.DeleteExpiredAuthorizeLog();
            //FileHelpers.DeleteCombineCssJs(true);
            //FileHelpers.DeleteCombineCssJs(false);
        }
    }
}