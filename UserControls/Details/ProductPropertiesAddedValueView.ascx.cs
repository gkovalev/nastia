using System;
using System.Collections.Generic;
using AdvantShop.Catalog;
using System.Linq;


public partial class UserControls_ProductPropertiesAddedValueView : System.Web.UI.UserControl
{
    public bool HasProperties { get; private set; }
    public int ProductId { get; set; }
    public List<PropertyValue> ProductProperties { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        string[] tempContainedStringArray = new string[] { "Позиционирование", "Дополнительные преимущества", "Потребительские преимущества", "Основные преимущества", "Преимущества", "Маркер пункта", "Информация об изделии" };
       // string[] tempNotContainedStringArray = new string[] { "Позиционирование3", "Позиционирование4", "Позиционирование5" };
        List<PropertyValue> apr = ProductProperties.Where(p => tempContainedStringArray.Contains(p.Property.Name) 
                                                                //&& !tempNotContainedStringArray.Contains(p.Property.Name)
                                                               )
                                                   .ToList();

        if (apr != null && apr.Count > 0)
        {
            
            lblHeader.Text = apr.Where(p => p.Property
                                             .Name
                                             .Contains("Позиционирование"))
                                .Select(p => p.Value)
                                .FirstOrDefault();

            lvProperties.DataSource = apr.Where(p=>!p.Property.Name.Contains("Позиционирование"));
            lvProperties.DataBind();
            HasProperties = true;
        }
        else
        {
            HasProperties = false;
            this.Visible = false;
        }
    }

}