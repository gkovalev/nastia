//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Web;
using AdvantShop.Configuration;
using AdvantShop.Core.Caching;
using AdvantShop.Core.Scheduler;
using AdvantShop.Diagnostics;
using AdvantShop.Modules;
using AdvantShop.Repository.Currencies;
using AdvantShop.Statistic;

namespace AdvantShop.Core
{

    public static class AppServiceStartAction
    {
        public static DataBaseService.PingDbState state;
        public static string errMessage;
        public static bool isAppNeedToReRun;
        public static bool isAppFistRun;
    }

    public class ApplicationService
    {
        public static void LoadModules()
        {
            AttachedModules.LoadModules();
        }

        public static void StartApplication(HttpContext current)
        {
            ApplicationUptime.SetApplicationStartTime();

            SettingsGeneral.SetAbsolutePath(current.Server.MapPath("~/"));

            // loger must init ONLY after SetAbsolutePath 
            Debug.InitLogger();

            // SetIsDbFirstCheckAtApplicationStart(true);
            AppServiceStartAction.isAppFistRun = true;

            TryToStartDbDependServices();

            LoadModules();
        }

        public static void TryToStartDbDependServices()
        {
            var appStartDbRes = AdvantShop.Core.DataBaseService.CheckDbStates();

            AppServiceStartAction.state = appStartDbRes;

            if (AppServiceStartAction.state == DataBaseService.PingDbState.NoError)
            {
                // Other db depend codes
                RunDBDependAppStartServices();
            }

        }

        public static void RunDBDependAppStartServices()
        {

            // TaskManager
            TaskManager.TaskManagerInstance().Init();
            TaskManager.TaskManagerInstance().Start();
            TaskManager.TaskManagerInstance().ManagedTask(TaskSqlSettings.TaskSettings);

            // LogSessionRestart
            InternalServices.LogApplicationRestart(false, false);

            // RefreshCurrency
            CurrencyService.RefreshCurrency();

            // ...
        }

    }
}