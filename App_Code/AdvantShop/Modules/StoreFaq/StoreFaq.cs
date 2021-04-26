//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;

namespace AdvantShop.Modules
{
    public class StoreFaq
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string FaqerEmail { get; set; }
        public string FaqerName { get; set; }
        public string Faq { get; set; }
        public int Rate { get; set; }
        public bool Moderated { get; set; }
        public bool HasChild { get; set; }
        public DateTime DateAdded { get; set; }
        public List<StoreFaq> ChildrenFaqs { get; set; }
    }
}