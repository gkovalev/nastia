using System;
using AdvantShop;
using Newtonsoft.Json;

public partial class UserControls_Catalog_CompareControl : System.Web.UI.UserControl
{
    public string ResultOptions = "";

    public enum eType
    {
        Checkbox = 0,
        Icon = 1
    }

    public int ProductId { get; set; }
    public bool IsSelected { get; set; }
    public string CompareCountId { get; set; }
    public string CompareBasketId { get; set; }
    public int AnimationSpeed { get; set; }
    public double AnimationOpacity { get; set; }
    public eType Type { get; set; }
    public string CssClassContainer { get; set; }

    public UserControls_Catalog_CompareControl()
    {
        IsSelected = false;
        CompareCountId = "#compareCount";
        CompareBasketId = "#compareBasket";
        AnimationSpeed = 1200;
        AnimationOpacity = 0.1;
        Type = eType.Checkbox;
        CssClassContainer = "";
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected string GetOptions()
    {
        var options = new
            {
                compareCount = CompareCountId,
                compareBasket = CompareBasketId,
                animationSpeed = AnimationSpeed,
                animationOpacity = AnimationOpacity,
                type = Type.ToString().ToLower(),
                classContainer = CssClassContainer
            };

        return JsonConvert.SerializeObject(options);

    }
}