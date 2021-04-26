//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Threading;
using AdvantShop.Configuration;
using AdvantShop.Permission;
using Quartz;

namespace AdvantShop.Core.Scheduler
{
    public class LicJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            var rand = new Random().Next(20 * 60);
            Thread.Sleep(rand * 1000);
            SettingsLic.ActiveLic = PermissionAccsess.ActiveDailyLic(SettingsLic.LicKey, SettingsMain.SiteUrl, SettingsMain.ShopName, SettingsGeneral.SiteVersion);
        }
    }
}