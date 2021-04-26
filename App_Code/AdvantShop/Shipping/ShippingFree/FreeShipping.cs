//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;

namespace AdvantShop.Shipping
{
    public struct FreeShippingTemplate
    {
        public const string DeliveryTime = "DeliveryTime";
    }

    public class FreeShipping : IShippingMethod
    {
       public decimal GetRate()
        {
            return decimal.Zero;
        }

        public List<ShippingOption> GetShippingOptions()
        {
            throw new Exception("GetShippingOptions method isnot avalible for FreeShipping");
        }
    }
}