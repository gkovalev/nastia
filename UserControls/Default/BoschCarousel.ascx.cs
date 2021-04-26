//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Web.UI;
using AdvantShop.Configuration;
using AdvantShop.Core;
using AdvantShop.CMS;

public partial class UserControls_Default_Carousel : UserControl
{
    public SettingsDesign.eMainPageMode Mode { set; get; }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if(CarouselService.GetCarouselsCount() == 0)
        {
            this.Visible = false;
        }
    }
}