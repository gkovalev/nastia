//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using AdvantShop.Catalog;

namespace AdvantShop.Shipping
{
    //*********************************************
    //in database this value using, new method add to end enum
    public enum ShippingType
    {
        None = 0,
        FreeShipping = 1,
        FixedRate = 2,
        ShippingByWeight = 3,
        FedEx = 4,
        Ups = 5,
        Usps = 6,
        eDost = 7,
        ShippingByShippingCost = 8,
        ShippingByOrderPrice = 9,
    }

    public enum ExtendedType
    {
        None = 0,
        CashOnDelivery = 1,
        Pickpoint = 2
    }

    public class ShippingOption
    {
        public int ShippingMethodId { get; set; }
        public decimal Rate { get; set; }
        public string Name { get; set; }
        public string DeliveryTime { get; set; }

        public ShippingOptionEx Extend { get; set; }
    }

    public class ShippingOptionEx
    {
        public ExtendedType Type { get; set; }
        public decimal BasePrice { get; set; }
        public decimal PriceCash { get; set; }
        public decimal Transfer { get; set; }
        public string Pickpointmap { get; set; }
        public string PickpointId { get; set; }
        public string PickpointAddress { get; set; }
        public int ShippingId { get; set; }
    }

    //*********************************************
    [Serializable]
    public class ShippingMethod
    {
        public int ShippingMethodId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public int SortOrder { get; set; }

        private Photo _picture;
        public Photo IconFileName
        {
            get
            {
                return _picture ?? (_picture = PhotoService.GetPhotoByObjId(ShippingMethodId, PhotoType.Shipping));
            }
            set
            {
                _picture = value;
            }
        }

        public virtual ShippingType Type { get; set; }

        public bool IsCalcualtable
        {
            get
            {
                return Type == ShippingType.FedEx || Type == ShippingType.Ups || Type == ShippingType.Usps || Type == ShippingType.eDost;
            }
        }

        //load if we want
        private Dictionary<string, string> _params;
        public Dictionary<string, string> Params
        {
            get
            {
                return _params ?? (_params = ShippingMethodService.GetShippingParams(ShippingMethodId));
            }
            set
            {
                _params = value;
            }
        }
        public static bool GetBooleanParam(Dictionary<string, string> items, string paramName)
        {
            return items.ElementOrDefault(paramName).TryParseBool();
        }

    }
}