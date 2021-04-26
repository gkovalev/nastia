//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using AdvantShop.Customers;

namespace AdvantShop.Security
{
    public class RoleAccess
    {
        private static readonly Dictionary<string, RoleActionKey> dictionary = new Dictionary<string, RoleActionKey>
        {
            {"catalog.aspx",         RoleActionKey.DisplayCatalog},
            {"product.aspx",         RoleActionKey.DisplayCatalog},
            {"m_category.aspx",      RoleActionKey.DisplayCatalog},
            {"m_categorysortorder.aspx",RoleActionKey.DisplayCatalog},
            {"m_productvideos.aspx", RoleActionKey.DisplayCatalog},
            {"exportcsv.aspx",       RoleActionKey.DisplayImportExport},
            {"importcsv.aspx",       RoleActionKey.DisplayImportExport},

            {"properties.aspx",     RoleActionKey.DisplayProperties},
            {"propertyvalues.aspx", RoleActionKey.DisplayProperties},            

            {"productsonmain.aspx?type=bestseller", RoleActionKey.DisplayMainPageBestsellers},
            {"productsonmain.aspx?type=new",        RoleActionKey.DisplayMainPageNew},
            {"productsonmain.aspx?type=discount",   RoleActionKey.DisplayMainPageDiscount},

            {"reviews.aspx", RoleActionKey.DisplayComments},

            {"priceregulation.aspx", RoleActionKey.DisplayPriceRegulation},

            {"brands.aspx",  RoleActionKey.DisplayBrands},
            {"m_brand.aspx", RoleActionKey.DisplayBrands},

            {"ordersearch.aspx",   RoleActionKey.DisplayOrders },
            {"editorder.aspx",     RoleActionKey.DisplayOrders},
            {"orderbyrequest.aspx", RoleActionKey.DisplayOrders},
            {"orderstatuses.aspx", RoleActionKey.DisplayOrderStatuses},
            {"export1c.aspx", RoleActionKey.DisplayOrders},

            {"customersearch.aspx", RoleActionKey.DisplayCustomers},
            {"createcustomer.aspx", RoleActionKey.DisplayCustomers},
            {"viewcustomer.aspx",   RoleActionKey.DisplayCustomers},

            {"certificates.aspx",  RoleActionKey.DisplayCertificates},
            {"m_certificate.aspx", RoleActionKey.DisplayCertificates},

            {"subscription.aspx",                  RoleActionKey.DisplaySubscription},
            {"subscription_unreg.aspx",            RoleActionKey.DisplaySubscription}, 
            {"subscription_deactivateReason.aspx", RoleActionKey.DisplaySubscription},

            {"staticpages.aspx", RoleActionKey.DisplayStaticPages},
            {"staticpage.aspx",  RoleActionKey.DisplayStaticPages},

            {"staticblocks.aspx", RoleActionKey.DisplayStaticBlocks},
            {"m_staticblock.aspx",  RoleActionKey.DisplayStaticBlocks},

            {"menu.aspx",   RoleActionKey.DisplayMenus},
            {"m_menu.aspx", RoleActionKey.DisplayMenus},

            {"newsadmin.aspx",    RoleActionKey.DisplayNews},
            {"newscategory.aspx", RoleActionKey.DisplayNews},
            {"m_news.aspx",       RoleActionKey.DisplayNews},

            {"carousel.aspx", RoleActionKey.DisplayCarousel},

            {"discount_pricerange.aspx", RoleActionKey.AllowEditDiscounts},
            {"coupons.aspx",             RoleActionKey.AllowEditCoupones },
            {"m_coupon.aspx",            RoleActionKey.AllowEditCoupones},

            {"voting.aspx",        RoleActionKey.DislayVotes},
            {"votinghistory.aspx", RoleActionKey.DislayVotes},

            {"sitemapgenerate.aspx",    RoleActionKey.DisplaySiteMap},
            {"sitemapgeneratexml.aspx", RoleActionKey.DisplaySiteMap},

            {"exportfeed.aspx",               RoleActionKey.DisplayExportFeed},
            {"ritmzsettings.aspx",            RoleActionKey.DisplayRitmZ},
            {"importelbuzcsv.aspx",           RoleActionKey.DisplayElbuz},
            {"importpropertieselbuzcsv.aspx", RoleActionKey.DisplayElbuz},

            {"sendmessage.aspx", RoleActionKey.DisplaySendMessages},
            {"mailchimpsettings.aspx", RoleActionKey.DisplayMailChimp},

            {"usersonline.aspx", RoleActionKey.DisplayUsersOnline},

            {"commonsettings.aspx", RoleActionKey.DisplayCommonSettings},

            {"country.aspx", RoleActionKey.DisplayCountries},
            {"regions.aspx", RoleActionKey.DisplayCountries},
            {"cities.aspx",  RoleActionKey.DisplayCountries},

            {"currencies.aspx", RoleActionKey.DisplayCurrencies},

            {"paymentmethod.aspx", RoleActionKey.DisplayPayments},

            {"shippingmethod.aspx", RoleActionKey.DisplayShippings},

            {"taxes.aspx", RoleActionKey.DisplayTaxes},
            {"tax.aspx",   RoleActionKey.DisplayTaxes},

            {"mailformat.aspx",       RoleActionKey.DisplayMailFormats},
            {"mailformatdetail.aspx", RoleActionKey.DisplayMailFormats},
            {"logviewer.aspx", RoleActionKey.DisplayLog},
            {"301redirects.aspx", RoleActionKey.DisplayRedirect},
        };

        public static bool Check(Customer customer, string currentPage)
        {
            if (customer.CustomerRole != Role.Moderator || currentPage.Contains("default.aspx") || currentPage.Contains("imagebrowser.aspx") || currentPage.Contains("linkbrowser.aspx"))
                return true;            
            
            currentPage = currentPage.Split(new[] { '/' }).Last();

            if (!currentPage.Contains("productsonmain.aspx"))
            {
                currentPage = currentPage.Split(new[] { '?' }).First();
            }

            if (dictionary.ContainsKey(currentPage))
            {
                RoleActionKey key = dictionary[currentPage];
                return RoleActionService.GetCustomerRoleActionsByCustomerId(customer.Id).Any(a => a.Key == key && a.Enabled);
            }

            return false;
        }
    }
}