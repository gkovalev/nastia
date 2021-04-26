//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;

namespace AdvantShop.SEO
{
    [Serializable] // for deep copy
    public class MetaInfo
    {
        public int MetaId { get; set; }
        public int ObjId { get; set; }
        public MetaType Type { get; set; }
        public string Title { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }

        public MetaInfo()
        {
        }

        public MetaInfo(string str)
        {
            Title = str;
        }
        public MetaInfo(int metaId, int objId, MetaType type, string title, string metaKeywords, string metaDescription)
        {
            MetaId = metaId;
            ObjId = objId;
            Type = type;
            Title = title;
            MetaKeywords = metaKeywords;
            MetaDescription = metaDescription;
        }
    }
}
