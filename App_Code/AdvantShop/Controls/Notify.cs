//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdvantShop.Controls
{
    public class Notify
    {
        public enum NotifyType
        {
            Error = 0,
            Notice = 1
        }

        public static string ShowMessage(NotifyType notifyType, string message)
        {
            return String.Format("<div class=\"notify-item type-{0}\">{1}<div class=\"close\"></div></div>", notifyType.ToString().ToLower(), message);
        }
    }
}