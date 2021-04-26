//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using AdvantShop.Configuration;
using AdvantShop.ExportImport;
using Quartz;

namespace AdvantShop.Core.Scheduler
{
    public class GenerateYandexMarketJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            if (Trial.IsTrialEnabled)
            {
                return;
            }
            string strFileName = ExportFeed.GetModuleSetting("YandexMarket", "FileName");
            string strPhysicalTargetFolder = SettingsGeneral.AbsolutePath;
            string strPhysicalFilePath = strPhysicalTargetFolder + strFileName;
            var exportFeedModule = new ExportFeedModuleYandex();
            exportFeedModule.GetExportFeedString(strPhysicalFilePath);
        }
    }
}