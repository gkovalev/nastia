//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using AdvantShop.Configuration;
using AdvantShop.FilePath;

public partial class UserControls_Favicon : System.Web.UI.UserControl
{
    #region  Properties
    public override sealed bool Visible { get; set; }
    public bool GetOnlyImage { get; set; }
    public string ImgSource { get; set; }
    public string CssClassImage { get; set; }
    public bool ForAdmin { get; set; }

    #endregion

    public UserControls_Favicon()
    {
        ImgSource = string.Empty;
        GetOnlyImage = false;
        Visible = true;
    }

    public string RenderHtml()
    {
        if (string.IsNullOrEmpty(ImgSource))
        {
            ImgSource = FoldersHelper.GetPath(FolderType.Pictures, SettingsMain.FaviconImageName, ForAdmin);
        }

        string resStrHtml = string.Empty;

        if (!string.IsNullOrEmpty(ImgSource))
        {
            const string imgTag = "<img id=\"favicon\" src=\"{0}\" {1} />";
            const string linkTag = "<link rel=\"{0}\" href=\"{1}\"{2} />";

            //Source
            string source = !string.IsNullOrEmpty(ImgSource) ? ImgSource : string.Empty;

            // styleClass
            string styleClass = !string.IsNullOrEmpty(CssClassImage) ? string.Format("class=\"{0}\"", CssClassImage) : string.Empty;

            //Source
            string rel = Request.Browser.Browser == "IE" ? "SHORTCUT ICON" : "shortcut icon";
            // string type = string.Format("type=\"image/{0}\"", GetType(source));

            resStrHtml = GetOnlyImage ? string.Format(imgTag, source, styleClass) : string.Format(linkTag, rel, source, string.Empty);// type);
        }

        return resStrHtml;
    }
}