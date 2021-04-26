using System;
using System.Web.UI;

//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

    public partial class Admin_RitmzSettings : Page
    {
        protected override void InitializeCulture()
        {
            AdvantShop.Localization.Culture.InitializeCulture();
        }

        protected void btnSave_Click(object sevder, EventArgs e)
        {
            RitmzSettings.SaveData();
        }
    }
