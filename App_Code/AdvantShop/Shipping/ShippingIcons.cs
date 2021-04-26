//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.IO;
using AdvantShop.FilePath;

namespace AdvantShop.Shipping
{
    public class ShippingIcons
    {
        public static string GetShippingIcon(ShippingType type, string iconName, string namefragment)
        {
            string folderPath = FoldersHelper.GetPath(FolderType.ShippingLogo, string.Empty, false);
            
            if (type == ShippingType.eDost)
            {
                namefragment = namefragment.ToLower();
                if (namefragment.Contains("почта россии"))
                    return folderPath + "7_pochtarussia.gif";
                if (namefragment.Contains("ems"))
                    return folderPath + "7_ems.gif";
                if (namefragment.Contains("спср экспресс"))
                    return folderPath + "7_spsrExpress.gif";
                if (namefragment.Contains("сдэк"))
                    return folderPath + "7_cdek.gif";
                if (namefragment.Contains("dhl"))
                    return folderPath + "7_dhl.gif";
                if (namefragment.Contains("ups"))
                    return folderPath + "7_ups.gif";
                if (namefragment.Contains("желдорэкспедиция"))
                    return folderPath + "7_trainroadExpedition.gif";
                if (namefragment.Contains("авторейдинг"))
                    return folderPath + "7_autotraiding.gif";
                if (namefragment.Contains("пэк"))
                    return folderPath + "7_pek.gif";
                if (namefragment.Contains("деловые линии"))
                    return folderPath + "7_delovielinies.gif";
                if (namefragment.Contains("мегаполис"))
                    return folderPath + "7_megapolis.gif";
                if (namefragment.Contains("гарантпост"))
                    return folderPath + "7_garantpost.gif";
                if (namefragment.Contains("pony"))
                    return folderPath + "7_ponyexpress.gif";
                if (namefragment.Contains("pickpoint"))
                    return folderPath + "7_pickpoint.gif";
                if (namefragment.Contains("boxberry"))
                    return folderPath + "7_boxberry.gif";

                if (File.Exists(FoldersHelper.GetPathAbsolut(FolderType.ShippingLogo, iconName)))
                {
                    return folderPath + iconName;
                }
                return folderPath + "7_default.gif";
            }
            if (File.Exists(FoldersHelper.GetPathAbsolut(FolderType.ShippingLogo, iconName)))
                return folderPath + iconName;

            return string.Format("{0}{1}.gif", folderPath, (int)type);
        }
    }
}