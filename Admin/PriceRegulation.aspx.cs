//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Web.UI;
using AdvantShop.Configuration;
using Resources;
using AdvantShop.SaasData;

public partial class Admin_PriceRegulation : Page
{
    protected override void InitializeCulture()
    {
        AdvantShop.Localization.Culture.InitializeCulture();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if ((SaasDataService.IsSaasEnabled) && (!SaasDataService.CurrentSaasData.HavePriceRegulating))
        {
            mainDiv.Visible = false;
            notInTariff.Visible = true;
        }

        Page.Title = string.Format("{0} - {1}", SettingsMain.ShopName,
                                Resource.Admin_PriceRegulation_Header);
    }
}