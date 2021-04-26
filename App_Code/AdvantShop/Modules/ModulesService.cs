//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Linq;
using System.IO;
using System.Web;
using System.Net;
using AdvantShop.Configuration;
using AdvantShop.SaasData;
using Newtonsoft.Json;

using AdvantShop.Diagnostics;
using AdvantShop.Modules.Interfaces;
using AdvantShop.Helpers;

namespace AdvantShop.Modules
{
    public class ModulesService
    {
        private const string RequestUrlGetModules = "http://modules.advantshop.net/modules/GetModules/{0}";
        private const string RequestUrlGetModuleArchive = "http://modules.advantshop.net/modules/GetModule?lickey={0}&moduleId={1}";

        #region Process modules from remote server

        public static ModuleBox GetModules()
        {
            var modules = GetModulesFromRemoteServer() ?? new ModuleBox();

            //if (modules.Message.IsNotEmpty())
            //{
            //    return modules;
            //}

            var isExistsOnlineModules = modules.Items.Count > 0;

            foreach (var type in AttachedModules.GetModules(AttachedModules.EModuleType.All))
            {
                var moduleInst = (IModule)Activator.CreateInstance(type, null);
                Module curModule;
                if (isExistsOnlineModules && (curModule = modules.Items.FirstOrDefault(item => item.StringId.ToLower() == moduleInst.ModuleStringId.ToLower())) != null)
                {
                    curModule.IsInstall = moduleInst.CheckAlive() &&
                                        ModulesRepository.IsInstallModule(moduleInst.ModuleStringId);
                    curModule.IsLocalVersion = true;
                }
                else
                {
                    modules.Items.Add(new Module
                    {
                        Name = moduleInst.ModuleName,
                        StringId = moduleInst.ModuleStringId,
                        Version = Resources.Resource.ModulesService_ModuleInDebug,
                        IsInstall =
                            moduleInst.CheckAlive() && ModulesRepository.IsInstallModule(moduleInst.ModuleStringId),
                        Price = 0,
                        IsLocalVersion = true
                    });
                }
            }

            var existModules = ModulesRepository.GetModulesFromDb();
            foreach (var module in modules.Items)
            {
                Module currentModule;
                if (existModules.Count > 0 &&
                    (currentModule = existModules.FirstOrDefault(item => item.StringId == module.StringId)) != null)
                {
                    module.CurrentVersion = currentModule.Version;

                }
            }
            return modules;
        }

        private static ModuleBox GetModulesFromRemoteServer()
        {
            var modules = new ModuleBox();

            try
            {
                var request = WebRequest.Create(string.Format(RequestUrlGetModules, SaasDataService.IsSaasEnabled ? SettingsGeneral.CurrentSaasId : SettingsLic.LicKey));
                request.Method = "GET";

                using (var dataStream = request.GetResponse().GetResponseStream())
                {
                    using (var reader = new StreamReader(dataStream))
                    {
                        var responseFromServer = reader.ReadToEnd();
                        if (!string.IsNullOrEmpty(responseFromServer))
                        {
                            modules = JsonConvert.DeserializeObject<ModuleBox>(responseFromServer);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }

            return modules;
        }

        public static string GetModuleArchiveFromRemoteServer(string moduleId)
        {
            var zipFileName = HttpContext.Current.Server.MapPath("~/App_Data/" + Guid.NewGuid() + ".Zip");
            try
            {
                new WebClient().DownloadFile(
                    string.Format(RequestUrlGetModuleArchive, SettingsLic.LicKey, moduleId),
                    zipFileName
                   );

                if (!FileHelpers.UnZipFile(zipFileName, HttpContext.Current.Server.MapPath("~/")))
                {
                    return "error on UnZipFile";
                }
                FileHelpers.DeleteFile(zipFileName);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                return "error on UnZipFile";
            }

            return string.Empty;
        }

        #endregion

        #region Public methods to process modules

        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleStringId"></param>
        /// <returns></returns>
        public static bool InstallModule(string moduleStringId)
        {
            return InstallModule(moduleStringId, "0");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleStringId"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public static bool InstallModule(string moduleStringId, string version)
        {
            var moduleInst = AttachedModules.GetModules(AttachedModules.EModuleType.All).FirstOrDefault(
            item =>
            ((IModule)Activator.CreateInstance(item, null)).ModuleStringId.ToLower() == moduleStringId.ToLower());

            if (moduleInst != null)
            {
                var module = ((IModule)Activator.CreateInstance(moduleInst, null));
                if (module.InstallModule())
                {
                    ModulesRepository.InstallModuleToDb(
                        new Module
                        {
                            StringId = module.ModuleStringId,
                            Name = module.ModuleName,
                            DateModified = DateTime.Now,
                            DateAdded = DateTime.Now,
                            Version = version
                        });
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleStringId"></param>
        /// <returns></returns>
        public static bool UninstallModule(string moduleStringId)
        {
            var moduleInst = AttachedModules.GetModules(AttachedModules.EModuleType.All).FirstOrDefault(
                    item =>
                    ((IModule)Activator.CreateInstance(item, null)).ModuleStringId.ToLower() == Convert.ToString(moduleStringId).ToLower());

            if (moduleInst != null)
            {
                var module = ((IModule)Activator.CreateInstance(moduleInst, null));
                if (module.UninstallModule())
                {
                    ModulesRepository.UninstallModuleFromDb(module.ModuleStringId);
                }
                else
                {
                    return false;
                }
            }

            if (Directory.Exists(HttpContext.Current.Server.MapPath("~/App_Code/Advantshop/Modules/" + moduleStringId)))
            {
                Directory.Delete(HttpContext.Current.Server.MapPath("~/App_Code/Advantshop/Modules/" + moduleStringId), true);
            }
            if (Directory.Exists(HttpContext.Current.Server.MapPath("~/Modules/" + moduleStringId)))
            {
                Directory.Delete(HttpContext.Current.Server.MapPath("~/Modules/" + moduleStringId), true);
            }
            return true;
        }


        public static string GetModuleStringIdByUrlPath(string urlPath)
        {
            if (urlPath.IsNullOrEmpty())
            {
                return string.Empty;
            }

            var moduleInst = AttachedModules.GetModules(AttachedModules.EModuleType.ClientPage).FirstOrDefault(
                        item =>
                        ((IClientPageModule)Activator.CreateInstance(item, null)).UrlPath.ToLower() == Convert.ToString(urlPath).ToLower());

            if (moduleInst != null)
            {
                var module = ((IModule)Activator.CreateInstance(moduleInst, null));
                if (module.CheckAlive())
                {
                    return module.ModuleStringId;
                }
            }

            return string.Empty;
        }
        #endregion

    }
}