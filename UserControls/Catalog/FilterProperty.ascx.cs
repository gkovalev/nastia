using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using AdvantShop.Catalog;

//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

public partial class UserControls_FilterProperty : UserControl
{
    public List<int> AvaliblePropertyIDs { set; get; }
    public List<int> SelectedPropertyIDs { set; get; }


    public int CategoryId { get; set; }

    private Category _category;
    private Category Category
    {
        get { return _category ?? (_category = CategoryService.GetCategory(CategoryId)); }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        var propertyValues = PropertyService.GetPropertyValuesByCategories(CategoryId, Category.DisplayChildProducts).ToList();
        var properties = propertyValues.Select(item => new UsedProperty
                                                           {
                                                               PropertyId = item.PropertyId,
                                                               PropertyName = item.Property.Name,
                                                               ValuesList = propertyValues.Where(value => value.PropertyId == item.PropertyId).ToList(),
                                                               PropertySort = item.Property.SortOrder,
                                                               Expanded = item.Property.Expanded
                                                           }).Distinct(new PropertyComparer());

        lvProperties.DataSource = properties;
        lvProperties.DataBind();
        Visible = lvProperties.Items.Any();
    }

    public class UsedProperty
    {
        public int PropertyId { set; get; }
        public string PropertyName { set; get; }
        public IEnumerable<PropertyValue> ValuesList { set; get; }
        public int PropertySort { get; set; }
        public bool Expanded { get; set; }
    }

    public class PropertyComparer : IEqualityComparer<UsedProperty>
    {
        public bool Equals(UsedProperty x, UsedProperty y)
        {
            return (x.PropertyId == y.PropertyId);
        }

        public int GetHashCode(UsedProperty obj)
        {
            return obj.PropertyId.GetHashCode();
        }
    }
}
