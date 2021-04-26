//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using AdvantShop.Core;

namespace AdvantShop
{
    public class Trial
    {
        public static bool IsTrialEnabled
        {
            get { return ModeConfigService.IsModeEnabled(ModeConfigService.Modes.TrialMode); }
        }
    }
}