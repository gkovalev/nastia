//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using AdvantShop.Catalog;
using AdvantShop.Shipping;

namespace AdvantShop.Payment
{
    /// <summary>
    /// cash on delivery
    /// </summary>
    public class CashOnDelivery : PaymentMethod
    {
        public int ShippingMethodId { get; set; }

        public override PaymentType Type
        {
            get { return PaymentType.CashOnDelivery; }
        }
        public override ProcessType ProcessType
        {
            get { return ProcessType.None; }
        }

        public const string ShippingMethodTemplate = "ShippingMethod";

        public override Dictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>
                             {
                                 {ShippingMethodTemplate, ShippingMethodId.ToString()}
                             };
            }
            set
            {
                int decVal;
                ShippingMethodId = value.ContainsKey(ShippingMethodTemplate) && int.TryParse(value[ShippingMethodTemplate], out decVal) ? decVal : 0;
            }
        }

        public static string GetDecription(ShippingOptionEx item)
        {
            if (item != null)
            {
                string messageTemplate = Resources.Resource.CashOnDeliveryMessage;
                return string.Format(messageTemplate, CatalogService.GetStringPrice(item.PriceCash - item.BasePrice), CatalogService.GetStringPrice(item.Transfer), CatalogService.GetStringPrice(item.PriceCash - item.BasePrice + item.Transfer));
            }
            return string.Empty;
        }
    }
}