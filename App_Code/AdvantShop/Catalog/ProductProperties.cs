//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;

namespace AdvantShop.Catalog
{
    [Serializable]
    public class ProductProperty
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public ProductProperty()
        {
            Name = string.Empty;
            Value = string.Empty;
        }
    }
}