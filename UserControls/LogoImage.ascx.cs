using AdvantShop.Core.UrlRewriter;

//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------
public partial class UserControls_LogoImage : System.Web.UI.UserControl
{
    #region  Properties
    public override sealed bool Visible { get; set; }
    public bool EnableHref { get; set; }
    public string ImgAlt { get; set; }
    public string ImgSource { get; set; }
    public string ImgHref { get; set; }
    public string CssClassHref { get; set; }
    public string CssClassImage { get; set; }
    public string Title { get; set; }
    #endregion
    
    public UserControls_LogoImage()
    {
        ImgHref = string.Empty;
        EnableHref = true;
        Visible = true;
        ImgAlt = AdvantShop.Configuration.SettingsMain.LogoImageAlt;
    }

    public string RenderHtml()
    {
        string resStrHtml = string.Empty;

        if (!string.IsNullOrEmpty(ImgSource))
        {
            const string aTag = "<a href=\"{0}\" {1} {2}>{3}</a>";
            const string imgTag = "<img id=\"logo\" src=\"{0}\" {1} {2}/>";
            //Source
            string source = !string.IsNullOrEmpty(ImgSource) ? ImgSource : string.Empty;
            //Alt
            string alt = !string.IsNullOrEmpty(ImgAlt) ? string.Format("alt=\"{0}\"", ImgAlt) : string.Empty;
            //CssClass
            string cssClass = !string.IsNullOrEmpty(CssClassImage)
                                  ? string.Format("class=\"{0}\"", CssClassImage)
                                  : string.Empty;
            //Result
            resStrHtml = string.Format(imgTag, source, alt, cssClass);
            //---------------------------------------------------------------------------

            //Creating href tag----------------------------------------------------------
            if (EnableHref && !string.IsNullOrEmpty(ImgHref))
            {
                //Href
                string href = UrlService.GetAbsoluteLink(ImgHref);
                //Title
                string title = !string.IsNullOrEmpty(Title) ? string.Format("title=\"{0}\"", Title) : string.Empty;
                //CssClass
                cssClass = !string.IsNullOrEmpty(CssClassHref) ? string.Format("class=\"{0}\"", CssClassHref) : string.Empty;
                //Href
                string image = resStrHtml;
                //Result
                resStrHtml = string.Format(aTag, href, title, cssClass, image);
            }
            //----------------------------------------------------------------------------
        }
        return resStrHtml;
    }
}