//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using AdvantShop.Configuration;
using AdvantShop.ExportImport;
using Quartz;

namespace AdvantShop.Core.Scheduler
{
    public class GenerateXmlMapJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            if (Trial.IsTrialEnabled)
            {
                return;
            }

            string strFileName = "sitemap.xml".ToLower();
            string strPhysicalTargetFolder = SettingsGeneral.AbsolutePath;
            string strPhysicalFilePath = strPhysicalTargetFolder + strFileName;
            var temp = new ExportXmlMap(strPhysicalFilePath, strPhysicalTargetFolder);
            temp.Create();
        }
    }
}