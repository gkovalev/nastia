using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdvantShop.Customers
{
    public enum Role
    {
        Administrator = 100,
        Moderator = 50,
        User = 0
    }

    public enum RoleActionKey
    {
        DisplayCatalog,
        DisplayProperties,
        DisplayImportExport,
        DisplayMainPageBestsellers,
        DisplayMainPageNew,
        DisplayMainPageDiscount,
        DisplayComments,
        DisplayPriceRegulation,
        DisplayBrands,

        DisplayOrders,
        DisplayOrderStatuses,
        DisplayCustomers,
        DisplayCertificates,
        DisplaySubscription,

        DisplayStaticPages,
        DisplayStaticBlocks,
        DisplayMenus,
        DisplayNews,
        DisplayCarousel,

        AllowEditDiscounts,
        AllowEditCoupones,
        DislayVotes,
        DisplaySiteMap,
        DisplayExportFeed,
        DisplayRitmZ,
        DisplayElbuz,
        DisplaySendMessages,
        DisplayMailChimp,
        DisplayUsersOnline,

        DisplayCommonSettings,
        DisplayCountries,
        DisplayCurrencies,
        DisplayPayments,
        DisplayShippings,
        DisplayTaxes,
        DisplayMailFormats,
        DisplayLog,
        DisplayRedirect
    }

    public class RoleAction
    {
        public RoleAction()
        {
            CustomerID = Guid.Empty;
            RoleActionID = 0;
            Name = "";
            Enabled = false;
            Category = "";
            SortOrder = 0;
        }

        public Guid CustomerID { get; set; }
        public int RoleActionID { get; set; }
        public string Name { get; set; }
        public RoleActionKey Key { get; set; }
        public bool Enabled { get; set; }
        public string Category { get; set; }
        public int SortOrder { get; set; }
    }
}