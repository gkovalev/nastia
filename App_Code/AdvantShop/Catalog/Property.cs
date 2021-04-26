//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;

namespace AdvantShop.Catalog
{
    /// <summary>
    /// Class represents an object property entity that is using in product
    /// </summary>
    public class Property
    {
        public int PropertyId { get; set; }
        public string Name { get; set; }
        public bool UseInFilter { get; set; }
        public int SortOrder { get; set; }
        public bool Expanded { get; set; }
        private List<PropertyValue> _valuesList;

        public List<PropertyValue> ValuesList
        {
            get { return _valuesList ?? (_valuesList = PropertyService.GetValuesByPropertyId(PropertyId)); }
        }
    }
}
