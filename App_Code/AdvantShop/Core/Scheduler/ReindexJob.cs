using System.Data;
using Quartz;

namespace AdvantShop.Core.Scheduler
{
    public class ReindexJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            SQLDataAccess.ExecuteNonQuery("[Settings].[sp_Reindex]", CommandType.StoredProcedure);
        }
    }
}