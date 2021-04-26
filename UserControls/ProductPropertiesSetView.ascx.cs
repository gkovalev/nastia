using System;
using System.Collections.Generic;
using AdvantShop.Catalog;
using System.Linq;
using AdvantShop.Core.UrlRewriter;


public partial class UserControls_ProductPropertiesSetView : System.Web.UI.UserControl
{
    public bool HasProperties { get; private set; }
    public int ProductId { get; set; }
    public string  ManufacteruArtNo { get; set; }
    public List<PropertyValue> ProductProperties { get; set; }

    ///
    protected List<ProductItem> ProductItems = new List<ProductItem>();
    protected List<string> PropertyNames = new List<string>();
    protected Dictionary<string, string> SetPropertyNames = new Dictionary<string,string>();
    protected List<string> SetPropertyValues = new List<string>();

    //

    protected void Page_Load(object sender, EventArgs e)
    {
        // IEnumerable<PropertyValue> productProperties = PropertyService.GetSpecificValuesByProductId(ProductId, "Комплект поставки");
        string[] tempContainedStringArray = new string[] { "Комплект поставки", "Заголовок", "Комплектация", "Содержание", "Эффективность рекламной акции" };
        List<PropertyValue> apr = ProductProperties.Where(p => tempContainedStringArray.Contains(p.Property.Name)
                                                               )
                                                   .ToList();

        //

        if (apr != null && apr.Count > 0)
        {
            int[] compareProducts;
            compareProducts = ProductService.GetProductIdWithSameManufactureId(ManufacteruArtNo).ToArray();

            var propertyNames = new List<string>();
            foreach (var item in compareProducts)
            {
                propertyNames.AddRange(PropertyService.GetPropertyValuesByProductId(item).Select(p => p.Property.Name).Intersect(tempContainedStringArray));

            }

            PropertyNames = new List<string>();
            PropertyNames.AddRange(propertyNames.Distinct());

            ProductItems = new List<ProductItem>();
            foreach (var item in compareProducts)
            {
                Product product = ProductService.GetProduct(item);
                if (product == null) continue;
                ProductItems.Add(new ProductItem(product, PropertyNames));
                if (lblHeader.Text == string.Empty)
                {
                    lblHeader.Text = Resources.Resource.Client_Details_Set_Properties + ": " +
                        product.Name;
                }

                // 
            }


            foreach (var item in ProductItems)
            {
                foreach (var val in item.Properties)
                {
                    if (!SetPropertyNames.Keys.Contains(val.Value))
                        SetPropertyNames.Add(val.Value, UpdateValue(val.Value));
                }

            }
            HasProperties = true;

        }
        else
        {
            HasProperties = false;
            this.Visible = false;
        }

    }


    private string  UpdateValue(string valueToHtml)
    {
        Product temp;
        string outputValue = valueToHtml;

        if (outputValue.Contains("(") && outputValue.Contains(")"))
         {
             outputValue = outputValue.Remove(0, outputValue.IndexOf("(") + 1);
             outputValue = outputValue.Remove(outputValue.IndexOf(")"));
             outputValue = outputValue.Replace(" ", "");
             //ArtNo Bosch
             if (outputValue.Length == 10)
             {
                 temp = ProductService.GetProduct(outputValue);
                 if (temp != null && temp.UrlPath != string.Empty)
                 {
                     outputValue = @"<a href=" + UrlService.GetLink(ParamType.Product, temp.UrlPath, temp.ID) + " class=''link-pv-name'' target='_blank'>" + temp.Name + " [" + temp.ArtNo + "] </a>";
                     valueToHtml = outputValue;
                 }
             }

         }
         return valueToHtml;        
    }
}
public class ProductItem
{
    public ProductItem(Product product, IEnumerable<string> propertyNames)
    {
        ProductId = product.ProductId;
        CategoryId = ProductService.GetFirstCategoryIdByProductId(ProductId);
        Name = product.Name;
        ArtNo = product.ArtNo;
        Photo = product.Photo;
        //changed by Evgeni
        // Price = OfferService.GetOfferPrice(product.ID, CatalogService.DefaultOfferListId);
        var firstOrDefault = product.Offers.FirstOrDefault();
        if (firstOrDefault != null)
        {
            Price = firstOrDefault.Price;
            Amount = firstOrDefault.Amount;
        }


        Discount = product.Discount;

        Properties = new List<ProductProperty>();

        var properties = PropertyService.GetPropertyValuesByProductId(product.ProductId);
        foreach (var propertyName in propertyNames)
        {
            if (properties.Count(p => p.Property.Name == propertyName) > 0)
            {
                foreach (var val in properties.Where(p => p.Property.Name == propertyName))
                {
                    Properties.Add(new ProductProperty
                    {
                        Name = propertyName,
                        Value = val.Value
     
                    });

                }

                //Properties.Add(new ProductProperty
                //{
                //    Name = propertyName,
                //    //Changed By Evgeni to insert different values for same propertie
                //    //  Value = properties.Find(p => p.Property.Name == propertyName).Value
                //    Value = string.Join("<br>", properties.Where(t => t.Property.Name.Equals(propertyName)).Select(p => p.Value).ToList())

                //});
            }
            else
            {
                Properties.Add(new ProductProperty
                {
                    Name = propertyName,
                    Value = " - "
                });
            }
        }
    }

    public int ProductId { get; set; }
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public string ArtNo { get; set; }
    public string Photo { get; set; }
    public decimal Price { get; set; }
    public decimal Discount { get; set; }
    public List<ProductProperty> Properties { get; set; }
    //Added by Evgeni to resolve defect with adding products with 0 price or without amoun to the basket
    public int Amount { get; set; }
}