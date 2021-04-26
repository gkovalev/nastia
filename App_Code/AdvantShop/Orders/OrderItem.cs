//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Catalog;

namespace AdvantShop.Orders
{
    [Serializable]
    public class OrderItem
    {

        public string ArtNo { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public int EntityId { get; set; }
        
        public decimal Price { get; set; }

        public int Amount { get; set; }

        public bool IsCouponApplied { get; set; }

        public decimal SupplyPrice { get; set; }

        public decimal Weight { get; set; }

        public EnumItemType ItemType { get; set; }

        public IList<EvaluatedCustomOptions> SelectedOptions { get; set; }

        public static explicit operator OrderItem(ShoppingCartItem item)
        {
            switch (item.ItemType)
            {
                case EnumItemType.Product:
                    return new OrderItem
                               {
                                   EntityId = item.EntityId,
                                   Name = item.Product.Name,
                                   ArtNo = item.Product.ArtNo,
                                   Price = item.Price,
                                   Amount = item.Amount,
                                   SupplyPrice = item.Product.Offers[0].SupplyPrice,
                                   SelectedOptions = CustomOptionsService.DeserializeFromXml(item.AttributesXml),
                                   Weight = item.Product.Weight,
                                   ItemType = (EnumItemType)item.ItemType,
                                   IsCouponApplied = item.IsCouponApplied,
                               };
                case EnumItemType.Certificate:
                    return new OrderItem
                               {
                                   EntityId = item.EntityId,
                                   Name = "Certificate",
                                   Amount = 1,
                                   ArtNo = string.Empty,
                                   SupplyPrice = 0,
                                   SelectedOptions = CustomOptionsService.DeserializeFromXml(item.AttributesXml),
                                   Weight = 0,
                                   Price = item.Price,
                                   ItemType = (EnumItemType)item.ItemType,
                                   IsCouponApplied = item.IsCouponApplied,
                               };
                default:
                    throw new NotImplementedException();
            }
        }

        public static bool operator ==(OrderItem first, OrderItem second)
        {
            if (ReferenceEquals(first, second))
            {
                return true;
            }

            if (((object)first == null) || ((object)second == null))
            {
                return false;
            }

            return first.EntityId == second.EntityId && first.SelectedOptions.SequenceEqual(second.SelectedOptions);
        }

        public static bool operator !=(OrderItem first, OrderItem second)
        {
            return !(first == second);
        }

        public bool Equals(OrderItem other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (Equals(other.ArtNo, ArtNo) &&
                Equals(other.Name, Name) &&
                Equals(other.EntityId, EntityId) &&
                other.Amount == Amount &&
                other.Price == Price &&
                other.SupplyPrice == SupplyPrice &&
                Equals(other.SelectedOptions, SelectedOptions) &&
                other.Id == Id)
            {
                return true;
            }

            //WARNING !!!!!! Equals() is same shit as == operator !!!!!!!!!!!
            return other == this;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            return obj.GetType() == typeof(OrderItem) && Equals((OrderItem)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = EntityId;
                result = (result * 397) ^ Amount;
                result = (result * 397) ^ (SelectedOptions != null ? SelectedOptions.AggregateHash() : 0);
                return result;
            }
        }
    }
}