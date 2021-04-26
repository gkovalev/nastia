//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

namespace AdvantShop.News
{
    public class NewsCategory
    {
        public int NewsCategoryID { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public int CountNews { get; set; }
        private string _urlPath;
        public string UrlPath
        {
            get { return _urlPath; }
            set { _urlPath = value.ToLower(); }
        }
    }
}