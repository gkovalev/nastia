//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Web;
using AdvantShop.Modules.Interfaces;

namespace AdvantShop.Modules
{
    [Description("Callback")]
    public class Callback : IRenderIntoHtml
    {

        public static string ModuleStringId
        {
            get { return "Callback"; }
        }

        string IModule.ModuleStringId
        {
            get { return ModuleStringId; }
        }

        public List<IModuleControl> ModuleControls
        {
            get { return new List<IModuleControl> { new CallbackSetting(), new ViewCallbacks() }; }
        }

        public bool CheckAlive()
        {
            return ModulesRepository.IsInstallModule(ModuleStringId);
        }

        public bool InstallModule()
        {
            return CallbackRepository.InstallCallbackModule();
        }

        public bool UninstallModule()
        {
            return CallbackRepository.UninstallCallbackModule();
        }

        public string DoRenderIntoHead()
        {
            return string.Empty;
        }

        public string DoRenderAfterBodyStart()
        {
            return string.Empty;
        }

        public string DoRenderBeforeBodyEnd()
        {
            return
                String.Format("<link rel='stylesheet' href='{0}' /><script src='{1}' data-callback-options data-callback-title='{2}' data-callback-text='{3}'></script>" +
                              "<script src='{4}'></script>",
                    "Modules/Callback/callback.css",
                    "Modules/Callback/callback.js",
                    ModuleSettingsProvider.GetSettingValue<string>("windowTitle", ModuleStringId),
                    ModuleSettingsProvider.GetSettingValue<string>("windowText", ModuleStringId),
                    "Modules/Callback/localization/" + CultureInfo.CurrentCulture.ToString() +  "/lang.js"
                    );
        }


        public string ModuleName
        {
            get
            {
                switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                {
                    case "ru":
                        return "Обратный звонок";

                    case "en":
                        return "Callback";

                    default:
                        return "Callback";
                }
            }
        }

        private class CallbackSetting : IModuleControl
        {
            #region Implementation of IModuleControl

            public string NameTab
            {
                get
                {
                    switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                    {
                        case "ru":
                            return "Настройки";

                        case "en":
                            return "Settings";

                        default:
                            return "Settings";
                    }
                }
            }

            public string File
            {
                get { return "CallbackModule.ascx"; }
            }

            #endregion
        }

        private class ViewCallbacks : IModuleControl
        {
            #region Implementation of IModuleControl

            public string NameTab
            {
                get
                {
                    switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                    {
                        case "ru":
                            return "Список заявок";

                        case "en":
                            return "List of applications";

                        default:
                            return "List of applications";
                    }
                }
            }

            public string File
            {
                get { return "ViewCallbacks.ascx"; }
            }

            #endregion
        }

    }
}