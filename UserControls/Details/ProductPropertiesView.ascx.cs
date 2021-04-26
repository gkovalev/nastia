using System;
using System.Collections.Generic;
using AdvantShop.Catalog;
using System.Linq;


public partial class UserControls_ProductPropertiesView : System.Web.UI.UserControl
{
    public bool HasProperties { get; private set; }
    public int ProductId { get; set; }
    public List<PropertyValue> ProductProperties { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {

        string[] tempNotContainedStringArray = new string[] { "Дополнительные преимущества", "Содержание", "Преимущества", "Потребительские преимущества", "Комплект поставки", "Позиционирование", "Тип продукта", "Комплектация", "Маркер пункта", "Информация об изделии", "Новинка!", "Наличие продукта", "Акционная модель", "Эффективность рекламной акции", "Основные преимущества" };
        List<PropertyValue> apr = ProductProperties.Where(p => !tempNotContainedStringArray.Contains(p.Property.Name)
                                                               )
                                                   .ToList();
        if (apr != null && apr.Count > 0)
        {

            lblHeader.Text = apr.Where(p => p.Property
                                             .Name
                                             .Contains("Заголовок"))
                                .Select(p => p.Value)
                                .FirstOrDefault();
            foreach (var prodValue in apr)
            {
                if (prodValue.Value.Trim() == "1")
                {
                    prodValue.Value =  @"<img src=" + @"images/icons/ico_yes.gif" +  " />";
                }
                else if (prodValue.Value.Trim() == "0")
                {
                    prodValue.Value = @"<img src=" + @"images/icons/ico_no.gif" + " />";
                }
            }

            lvProperties.DataSource = apr.Where(p => !p.Property.Name.Contains("Заголовок"));
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