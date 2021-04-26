//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Text;
using AdvantShop.Configuration;
using AdvantShop.ExportImport;
using Quartz;

namespace AdvantShop.Core.Scheduler
{
    public class GenerateHtmlMapJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            if (Trial.IsTrialEnabled)
            {
                return;
            }

            string strFileName = "sitemap.html".ToLower();
            string strPhysicalTargetFolder = SettingsGeneral.AbsolutePath;
            string strPhysicalFilePath = strPhysicalTargetFolder + strFileName;
            var temp = new ExportHtmlMap(strPhysicalFilePath, SettingsMain.SiteUrl + "/", Encoding.UTF8);
            temp.Create();
        }
    }
}