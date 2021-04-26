//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Globalization;
using AdvantShop.Modules.Interfaces;

namespace AdvantShop.Modules
{
    public class StoreReviews : IClientPageModule
    {
        public string ModuleStringId
        {
            get { return "StoreReviews"; }
        }

        public string UrlPath
        {
            get { return "StoreReviews"; }
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
                        return "Отзывы о магазине";

                    case "en":
                        return "Shop reviews";

                    default:
                        return "Shop reviews";
                }
            }
        }

        public List<IModuleControl> ModuleControls
        {
            get
            {
                return new List<IModuleControl>
                    {
                        new StoreReviewsSettings(),
                        new StoreReviewsManager()
                    };
            }
        }

        public bool CheckAlive()
        {
            return StoreReviewRepository.IsAliveStoreReviewsModule() && ModulesRepository.IsInstallModule(ModuleStringId);
        }

        public bool InstallModule()
        {
            return StoreReviewRepository.InstallStoreReviewsModule();
        }

        public bool UninstallModule()
        {
            return StoreReviewRepository.UninstallStoreReviewsModule();
        }

        private class StoreReviewsSettings : IModuleControl
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
                get { return "StoreReviewsSettings.ascx"; }
            }

            #endregion
        }

        private class StoreReviewsManager : IModuleControl
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
                            return "Reviews manager";

                        default:
                            return "Reviews manager";
                    }
                }
            }

            public string File
            {
                get { return "StoreReviewsManager.ascx"; }
            }

            #endregion
        }

        public string ClientPageControlFileName
        {
            get { return "StoreReviewsClientPage.ascx"; }
        }


    }
}