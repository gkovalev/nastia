//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using AdvantShop.Diagnostics;
using AdvantShop.Repository;
using AdvantShop.Orders;
using AdvantShop.Catalog;
using System.Linq;

namespace AdvantShop.Shipping
{
    public class ShippingByShippingCost : IShippingMethod
    {
        public ShoppingCart ShoppingCart { get; set; }
        private readonly bool _byMaxShippingCost;
        private readonly bool _useAmount;

        public ShippingByShippingCost(Dictionary<string, string> parameters)
        {
            try
            {                
                _byMaxShippingCost = parameters.ElementOrDefault(ShippingByShippingCostTemplate.ByMaxShippingCost).TryParseBool();

                _useAmount = parameters.ElementOrDefault(ShippingByShippingCostTemplate.UseAmount).TryParseBool();
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }

        public decimal GetRate()
        {
            if (!_useAmount)
            {
                return _byMaxShippingCost ? ShoppingCart.Where(item => item.ItemType == EnumItemType.Product)
                                                        .Max(item => OfferService.GetOffersByProductId(item.EntityId).First().ShippingPrice)

                                          : ShoppingCart.Where(item => item.ItemType == EnumItemType.Product)
                                                        .Sum(item => OfferService.GetOffersByProductId(item.EntityId).First().ShippingPrice);
            }
            else
            {
                return _byMaxShippingCost ? ShoppingCart.Where(item => item.ItemType == EnumItemType.Product)
                                                        .Max(item => OfferService.GetOffersByProductId(item.EntityId).First().ShippingPrice * item.Amount)

                                          : ShoppingCart.Where(item => item.ItemType == EnumItemType.Product)
                                                        .Sum(item => OfferService.GetOffersByProductId(item.EntityId).First().ShippingPrice * item.Amount);
            }
        }

        public List<ShippingOption> GetShippingOptions()
        {
            throw new Exception("GetShippingOptions method isnot avalible for ShippingByShippingCost");
        }
    }
}