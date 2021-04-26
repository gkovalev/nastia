//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading;
using System.Web;
using AdvantShop.Orders;
using AdvantShop.Repository;
using AdvantShop.Repository.Currencies;

namespace AdvantShop.Shipping
{
    public enum ShowMethods
    {
        All = 0,
        Static = 1,
        Dinamic = 2
    }

    [Serializable]
    public class ShippingListItem
    {
        public int Id { get; set; }
        public ShippingType Type { get; set; }
        public int MethodId { get; set; }
        public string MethodName { get; set; }
        public string MethodNameRate { get; set; }
        public decimal Rate { get; set; }
        public string DeliveryTime { get; set; }
        public string MethodDescription { get; set; }
        public ShippingOptionEx Ext { get; set; }
        public string IconName { get; set; }
    }

    [Serializable]
    public class ShippingManager
    {
        private readonly List<ShippingMethod> _listMethod;
        private static string _countryName = string.Empty;
        private static string _countryIso2 = string.Empty;
        private static string _zipTo = string.Empty;
        private static string _cityTo = string.Empty;
        private static string _regionTo = string.Empty;
        private ShoppingCart _shoppingCart;

        public int SelectIndex { get; set; }

        public ShippingManager()
        {
            _listMethod = ShippingMethodService.GetAllShippingMethods(true);
        }

        public ShippingManager(ShippingMethod item)
        {
            _listMethod = new List<ShippingMethod> { item };
        }

        private Currency _currency = CurrencyService.CurrentCurrency;
        public Currency Currency
        {
            get { return _currency; }
            set { if (value != null) _currency = value; }
        }

        public List<ShippingListItem> GetShippingRates()
        {
            return GetShippingRates(ShoppingCartService.CurrentShoppingCart);
        }

        public List<ShippingListItem> GetShippingRates(ShoppingCart shoppingCart)
        {
            _countryName = string.Empty;
            _countryIso2 = string.Empty;
            _zipTo = string.Empty;
            _cityTo = string.Empty;
            _shoppingCart = shoppingCart;
            return GetShippingRatesList();
        }

        public List<ShippingListItem> GetShippingRates(int countryId, string zip, string city, string region, ShoppingCart shoppingCart)
        {
            _countryName = CountryService.GetCountryNameById(countryId);
            _countryIso2 = CountryService.GetCountryISO2ById(countryId);
            _zipTo = zip;
            _cityTo = city;
            _shoppingCart = shoppingCart;
            _regionTo = region;
            return GetShippingRatesList();
        }



        private List<ShippingListItem> GetShippingRatesList()
        {
            if(_shoppingCart == null)
            {
                throw new Exception("_shoppingCart == null");
            }
            var lists = new List<ShippingListItem>();

            var currencyIso3 = Currency.Iso3;

            var threads = new List<Thread>();
            var shippingRates = new List<ThreadShippingRate>();
            var tempList = UseGeoMapping(_listMethod);
            var totalPrice = _shoppingCart.TotalPrice;
            var totalDiscount = _shoppingCart.TotalDiscount;

            int i = 1;
            foreach (var item in tempList)
            {
                int index = 1000 * (int)item.Type + 100 * i++;

                var shippingRate = new ThreadShippingRate(item, index, _countryName, _countryIso2, _zipTo, _cityTo,_regionTo, currencyIso3, _shoppingCart, totalPrice, totalDiscount);
                shippingRates.Add(shippingRate);

                var thread = new Thread(shippingRate.GetRate);
                thread.Start();
                threads.Add(thread);
            }

            foreach (var t in threads)
                t.Join();

            foreach (var shippingRate in shippingRates)
            {
                lists.AddRange(shippingRate.ShippingRates);
            }

            return lists;
        }

        private List<ShippingMethod> UseGeoMapping(List<ShippingMethod> listMethods)
        {
            var items = new List<ShippingMethod>();
            for (int i = 0; i < listMethods.Count; i++)
            {
                if (ShippingPaymentGeoMaping.IsExistGeoShipping(listMethods[i].ShippingMethodId))
                {
                    if (ShippingPaymentGeoMaping.CheckShippingEnabledGeo(listMethods[i].ShippingMethodId, _countryName, _cityTo))
                        items.Add(listMethods[i]);
                }
                else
                    items.Add(listMethods[i]);
            }
            return items;
        }

        public static List<ShippingListItem> CurrentShippingRates
        {
            get
            {
                return HttpContext.Current.Session["ShippingRates"] != null
                           ? (List<ShippingListItem>)HttpContext.Current.Session["ShippingRates"]
                           : new List<ShippingListItem>();
            }
            set
            {
                HttpContext.Current.Session["ShippingRates"] = value;
            }
        }
    }
}