//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using AdvantShop.Diagnostics;

namespace AdvantShop.Shipping
{
    public class FixeRateShipping : IShippingMethod
    {
        private readonly decimal _shippingPrice;
        private readonly decimal _extracharge;
        public FixeRateShipping(Dictionary<string, string> parameters)
        {
            try
            {
                _shippingPrice = parameters.ElementOrDefault(FixeRateShippingTemplate.ShippingPrice).TryParseDecimal();
                _extracharge = parameters.ElementOrDefault(FixeRateShippingTemplate.Extracharge).TryParseDecimal();
            }
            catch (Exception e)
            {

                Debug.LogError(e);
            }
        }

        public decimal GetRate()
        {
            return _shippingPrice + _extracharge;
        }

        public List<ShippingOption> GetShippingOptions()
        {
            throw new Exception("GetShippingOptions method isnot avalible for FixeRateShipping");
        }
    }
}