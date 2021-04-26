//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Diagnostics;
using AdvantShop.Orders;

namespace AdvantShop.Shipping
{
    /// <summary>
    /// Summary description for ThreadShippingRate
    /// </summary>
    public class ThreadShippingRate
    {
        private readonly ShippingMethod _method;
        private int _index;
        private readonly string _countryName;
        private readonly string _countryIso2;
        private readonly string _currencyIso3;
        private readonly string _zipTo;
        private readonly string _cityTo;
        private readonly string _regionTo;
        private readonly ShoppingCart _shoppingCart;
        private readonly decimal _totalPrice;
        private readonly decimal _totalDiscount;

        private readonly List<ShippingListItem> _shippingRates;
        public List<ShippingListItem> ShippingRates
        {
            get { return _shippingRates; }
        }

        public ThreadShippingRate(ShippingMethod method, int index, string countryName, string countryIso2, string zipTo, string cityTo, string regionTo, string currencyIso3, ShoppingCart shoppingCart, decimal totalPrice, decimal totalDiscount)
        {
            _method = method;
            _index = index;
            _countryName = countryName;
            _countryIso2 = countryIso2;
            _currencyIso3 = currencyIso3;
            _zipTo = zipTo;
            _cityTo = cityTo;
            _regionTo = regionTo;
            _shoppingCart = shoppingCart;
            _totalPrice = totalPrice;
            _totalDiscount = totalDiscount;
            _shippingRates = new List<ShippingListItem>();
        }

        public void GetRate()
        {
            var i = _shoppingCart.TotalPrice;
            try
            {
                switch (_method.Type)
                {
                    case ShippingType.ShippingByWeight:
                        _shippingRates.AddRange(GetShippingByWeightRates());
                        break;
                    case ShippingType.ShippingByOrderPrice:
                        _shippingRates.AddRange(GetShippingByOrderPrice());
                        break;

                    case ShippingType.FedEx:
                        _shippingRates.AddRange(GetFedExRates());
                        break;

                    case ShippingType.Ups:
                        _shippingRates.AddRange(GetUpsRates());
                        break;

                    case ShippingType.Usps:
                        _shippingRates.AddRange(GetUspsRates());
                        break;

                    case ShippingType.FixedRate:
                        _shippingRates.AddRange(GetFixedRates());
                        break;

                    case ShippingType.FreeShipping:
                        _shippingRates.AddRange(GetFreeShippingRates());
                        break;
                    case ShippingType.eDost:
                        _shippingRates.AddRange(GetEdostShippingRates());
                        break;

                    case ShippingType.ShippingByShippingCost:
                        _shippingRates.AddRange(GetShippingByShippingCost());
                        break;
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private IEnumerable<ShippingListItem> GetShippingByWeightRates()
        {
            return new List<ShippingListItem>
                       {
                           new ShippingListItem
                               {
                                   Id = ++_index,
                                   Type = ShippingType.ShippingByWeight,
                                   MethodId = _method.ShippingMethodId,
                                   MethodName = _method.Name,
                                   MethodNameRate = _method.Name,
                                   MethodDescription = _method.Description,
                                   Rate = new ShippingByWeight(_method.Params).GetRate(_shoppingCart.TotalShippingWeight),
                                   DeliveryTime = _method.Params.ElementOrDefault(ShippingByWeightTemplate.DeliveryTime),
                                   IconName = _method.IconFileName.PhotoName   
                               }
                       };
        }

        private IEnumerable<ShippingListItem> GetShippingByShippingCost()
        {
            return new List<ShippingListItem>
                       {
                           new ShippingListItem
                               {
                                   Id = ++_index,
                                   Type = ShippingType.ShippingByShippingCost,
                                   MethodId = _method.ShippingMethodId,
                                   MethodName = _method.Name,
                                   MethodNameRate = _method.Name,
                                   MethodDescription = _method.Description,
                                   IconName = _method.IconFileName.PhotoName,
                                   Rate = new ShippingByShippingCost(_method.Params)
                                                {
                                                    ShoppingCart = _shoppingCart
                                                }
                                                .GetRate()
                               }
                       };
        }

        private IEnumerable<ShippingListItem> GetShippingByOrderPrice()
        {
            var shippingitem = new ShippingListItem
                {
                    Id = ++_index,
                    Type = ShippingType.ShippingByOrderPrice,
                    MethodId = _method.ShippingMethodId,
                    MethodName = _method.Name,
                    MethodNameRate = _method.Name,
                    MethodDescription = _method.Description,
                    DeliveryTime = _method.Params.ElementOrDefault(ShippingByOrderPriceTemplate.DeliveryTime),
                    IconName = _method.IconFileName.PhotoName,
                    Rate = new ShippingByOrderPrice(_method.Params).GetRate(_totalPrice, _totalPrice - _totalDiscount)
                };

            var list = new List<ShippingListItem>();

            if (shippingitem.Rate != -1)
                list.Add(shippingitem);

            return list;
        }


        private IEnumerable<ShippingListItem> GetFedExRates()
        {
            return new FedEx(_method.Params)
                       {
                           CountryCodeTo = _countryIso2,
                           PostalCodeTo = _zipTo,
                           ShoppingCart = _shoppingCart,
                           CurrencyIso3 = _currencyIso3,
                           TotalPrice = _totalPrice - _totalDiscount
                       }
                .GetShippingOptions()
                .Select(
                    item => new ShippingListItem
                                {
                                    Id = ++_index,
                                    Type = ShippingType.FedEx,
                                    MethodId = _method.ShippingMethodId,
                                    MethodName = _method.Name,
                                    MethodNameRate = item.Name,
                                    DeliveryTime = item.DeliveryTime,
                                    IconName = _method.IconFileName.PhotoName,
                                    MethodDescription = _method.Description,
                                    Rate = item.Rate
                                });
        }

        private IEnumerable<ShippingListItem> GetUpsRates()
        {
            return new Ups(_method.Params)
                       {
                           CountryCodeTo = _countryIso2,
                           PostalCodeTo = _zipTo,
                           ShoppingCart = _shoppingCart
                       }
                .GetShippingOptions()
                .Select(
                    item => new ShippingListItem
                                {
                                    Id = ++_index,
                                    Type = ShippingType.Ups,
                                    MethodId = _method.ShippingMethodId,
                                    MethodName = _method.Name,
                                    MethodNameRate = item.Name,
                                    IconName = _method.IconFileName.PhotoName,
                                    MethodDescription = _method.Description,
                                    Rate = item.Rate
                                });

        }

        private IEnumerable<ShippingListItem> GetUspsRates()
        {
            return new Usps(_method.Params)
                       {
                           CountryTo = _countryName,
                           CountryToIso2 = _countryIso2,
                           PostalCodeTo = _zipTo,
                           Size = Usps.PackageSize.Regular,
                           ShoppingCart = _shoppingCart,
                           TotalPrice = _totalPrice - _totalDiscount
                       }
                .GetShippingOptions()
                .Select(
                    item =>
                    new ShippingListItem
                        {
                            Id = ++_index,
                            Type = ShippingType.Usps,
                            MethodId = _method.ShippingMethodId,
                            MethodName = _method.Name,
                            MethodNameRate = item.Name,
                            IconName = _method.IconFileName.PhotoName,
                            MethodDescription = _method.Description,
                            Rate = item.Rate
                        }
                );
        }


        private IEnumerable<ShippingListItem> GetFixedRates()
        {
            return new List<ShippingListItem>
                       {
                           new ShippingListItem
                               {
                                   Id = ++_index,
                                   Type = ShippingType.FixedRate,
                                   MethodId = _method.ShippingMethodId,
                                   MethodName = _method.Name,
                                   MethodNameRate = _method.Name,
                                   MethodDescription = _method.Description,
                                   Rate = new FixeRateShipping(_method .Params).GetRate(),
                                   IconName = _method.IconFileName.PhotoName,
                                   DeliveryTime = _method .Params.ElementOrDefault(FixeRateShippingTemplate.DeliveryTime) 
                               }
                       };
        }

        private IEnumerable<ShippingListItem> GetFreeShippingRates()
        {
            return new List<ShippingListItem>
                       {
                           new ShippingListItem
                               {
                                   Id = ++_index,
                                   Type = ShippingType.FreeShipping,
                                   MethodId = _method.ShippingMethodId,
                                   MethodName = _method.Name,
                                   MethodNameRate = _method.Name,
                                   MethodDescription = _method.Description,
                                   Rate = new FreeShipping().GetRate(),
                                   IconName = _method.IconFileName.PhotoName,
                                   DeliveryTime = _method .Params.ElementOrDefault(FreeShippingTemplate.DeliveryTime)
                               }
                       };
        }

        private IEnumerable<ShippingListItem> GetEdostShippingRates()
        {
            var edost = new Edost(_method.Params)
                {
                    CityTo = _cityTo,
                    Zip = _zipTo,
                    ShoppingCart = _shoppingCart,
                    TotalPrice = _totalPrice - _totalDiscount,
                };

            var edostShippings = edost.GetShippingOptions();
            if (edostShippings == null || !edostShippings.Any())
            {
                edost.CityTo = _regionTo;
                edostShippings = edost.GetShippingOptions();
            }
            if (edostShippings == null || !edostShippings.Any())
            {
                edost.CityTo = _countryName;
                edostShippings = edost.GetShippingOptions();
            }

            return edostShippings.Select(
                item =>
                new ShippingListItem
                {
                    Id = ++_index,
                    Type = ShippingType.eDost,
                    MethodId = _method.ShippingMethodId,
                    MethodName = _method.Name,
                    MethodNameRate = item.Name,
                    Rate = item.Rate,
                    DeliveryTime = item.DeliveryTime,
                    IconName = _method.IconFileName.PhotoName,
                    MethodDescription = _method.Description,
                    Ext = GetId(item.Extend)
                }
            );
        }

        private ShippingOptionEx GetId(ShippingOptionEx item)
        {
            if (item != null)
            {
                item.ShippingId = _method.ShippingMethodId;
            }
            return item;
        }
    }
}
