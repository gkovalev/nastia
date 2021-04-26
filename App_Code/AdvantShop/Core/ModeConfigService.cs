//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Linq;
using System.Web;
using System.Xml;

namespace AdvantShop.Core
{
    public class ModeConfigService
    {
        public enum Modes
        {
            DemoMode,
            TrialMode,
            SaasMode
        }

        public static bool IsModeEnabled(Modes mode)
        {
            var myXmlDocument = new XmlDocument();
            myXmlDocument.Load(Configuration.SettingsGeneral.AbsolutePath + "Web.ModeSettings.config");
            var root = myXmlDocument.ChildNodes.OfType<XmlNode>().First(p => p.Name.Equals("modesettings"));
            if (root != null)
            {
                foreach (XmlNode node in root.ChildNodes)
                {
                    if (node.Name ==  mode.ToString())
                    {
                        return Boolean.Parse(node.Attributes["value"].Value);
                    }
                }
            }
            throw new NotImplementedException("this mode is not configured in ~/Web.ModeSettings.config");
        }

    }
}