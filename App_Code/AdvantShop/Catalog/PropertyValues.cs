//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

namespace AdvantShop.Catalog
{
    /// <summary>
    /// Represents some Value of existed Property
    /// </summary>
    public class PropertyValue
    {
        public int PropertyValueId { get; set; }
        public int PropertyId { get; set; }
        public Property Property { get; set; }
        public string Value { get; set; }
        public int SortOrder { get; set; }
        //private Property _property = null;
        //public Property Property { get { return _property ?? (_property = PropertyService.GetPropertyById(PropertyId)); } }
        //private string _name;
        //public string Name
        //{
        //    get
        //    {
        //        if (_property != null) return _property.Name;
        //        return _name ?? Property.Name;
        //    }
        //    set { _name = string.IsNullOrEmpty(value) ? null : value; }
        //}

        //public int PropertySortOrder { get; set; }
    }
}