//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Globalization;
using AdvantShop.Modules.Interfaces;

namespace AdvantShop.Modules
{
    public class StoreFaqs : IClientPageModule
    {
        public string ModuleStringId
        {
            get { return "StoreFaqs"; }
        }

        public string UrlPath
        {
            get { return "StoreFaqs"; }
        }

        public string PageTitle {
            get { return ModuleSettingsProvider.GetSettingValue<string>("PageTitle", ModuleStringId); }
        }

        public string MetaKeyWords
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("MetaKeyWords", ModuleStringId); }
        }

        public string MetaDescription
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("MetaDescription", ModuleStringId); }
        }

        public string ModuleName
        {
            get
            {
                switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                {
                    case "ru":
                        return "Часто задаваемые вопросы";

                    case "en":
                        return "Shop Faqs";

                    default:
                        return "Shop Faqs";
                }
            }
        }

        public List<IModuleControl> ModuleControls
        {
            get
            {
                return new List<IModuleControl>
                    {
                        new StoreFaqsSettings(),
                        new StoreFaqsManager()
                    };
            }
        }

        public bool CheckAlive()
        {
            return StoreFaqRepository.IsAliveStoreFaqsModule() && ModulesRepository.IsInstallModule(ModuleStringId);
        }

        public bool InstallModule()
        {
            return StoreFaqRepository.InstallStoreFaqsModule();
        }

        public bool UninstallModule()
        {
            return StoreFaqRepository.UninstallStoreFaqsModule();
        }

        private class StoreFaqsSettings : IModuleControl
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
                get { return "StoreFaqsSettings.ascx"; }
            }

            #endregion
        }

        private class StoreFaqsManager : IModuleControl
        {
            #region Implementation of IModuleControl

            public string NameTab
            {
                get
                {
                    switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                    {
                        case "ru":
                            return "Управление отзывами";

                        case "en":
                            return "Faqs manager";

                        default:
                            return "Faqs manager";
                    }
                }
            }

            public string File
            {
                get { return "StoreFaqsManager.ascx"; }
            }

            #endregion
        }

        public string ClientPageControlFileName
        {
            get { return "StoreFaqsClientPage.ascx"; }
        }


    }
}