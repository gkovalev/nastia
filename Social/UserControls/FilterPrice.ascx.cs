using System;
using System.Web.UI;
using AdvantShop.Catalog;
using AdvantShop.Repository.Currencies;

//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

public partial class UserControls_FilterPrice : UserControl
{
    public int CategoryId { set; get; }
    public bool InDepth { set; get; }
    public decimal CurValMin;
    public decimal CurValMax;

    protected decimal Min;
    protected decimal Max;
    
    
    static int Get10Pow(decimal src)
    {
        int pow = 1;
        while (src / (10 * pow) >= 1)
        {
            pow *= 10;
        }
        return pow;
    }

    static void MegaRound(ref decimal src1, ref decimal src2)
    {
        int pow = Get10Pow(Math.Max(src1, src2));
        int pow2 = Get10Pow(src1);
        src1 = Math.Floor((src1 / pow2)) * pow2;
        src2 = Math.Ceiling((src2 / pow)) * pow;
    }
    
    public void Page_Load(object sender, EventArgs e)
    {
        var prices = CategoryService.GetPriceRange(CategoryId, InDepth);
        Min = Math.Floor(prices.Key / CurrencyService.CurrentCurrency.Value);
        Max = Math.Ceiling(prices.Value / CurrencyService.CurrentCurrency.Value);
        MegaRound(ref Min, ref Max);
        Visible = Min != Max;

        if (CurValMin < Min)
            CurValMin = Min;

        if (CurValMax > Max)
            CurValMax = Max;
    }
}