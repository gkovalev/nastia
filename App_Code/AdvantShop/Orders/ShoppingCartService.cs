//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Data;
using System.Data.SqlClient;
using AdvantShop.Catalog;
using AdvantShop.Core;
using AdvantShop.Customers;
using AdvantShop.Helpers;

namespace AdvantShop.Orders
{
    public enum ShoppingCartType
    {
        /// <summary>
        /// Shopping cart
        /// </summary>
        ShoppingCart = 1,
        /// <summary>
        /// Wishlist
        /// </summary>
        Wishlist = 2,
        /// <summary>
        /// compare product
        /// </summary>
        Compare = 3
    }

    public static class ShoppingCartService
    {
        public static ShoppingCart CurrentShoppingCart
        {
            get { return GetShoppingCart(ShoppingCartType.ShoppingCart); }
        }

        public static ShoppingCart CurrentCompare
        {
            get { return GetShoppingCart(ShoppingCartType.Compare); }
        }

        public static ShoppingCart CurrentWishlist
        {
            get { return GetShoppingCart(ShoppingCartType.Wishlist); }
        }

        
        public static ShoppingCart GetShoppingCart(ShoppingCartType shoppingCartType)
        {
            return GetShoppingCart(shoppingCartType, CustomerSession.CustomerId);
        }

        public static ShoppingCart GetShoppingCart(ShoppingCartType shoppingCartType, Guid customerId)
        {
            var templist = SQLDataAccess.ExecuteReadList<ShoppingCartItem>
                ("SELECT * FROM Catalog.ShoppingCart WHERE ShoppingCartTypeId = @ShoppingCartTypeId and CustomerId = @CustomerId",
                    CommandType.Text, GetFromReader,
                    new SqlParameter { ParameterName = "@ShoppingCartTypeId", Value = (int)shoppingCartType },
                    new SqlParameter { ParameterName = "@CustomerId", Value = customerId }
                );

            var shoppingCart = new ShoppingCart();
            shoppingCart.AddRange(templist);
            return shoppingCart;
        }

        public static ShoppingCart GetAllShoppingCarts(Guid customerId)
        {
            var shoppingCart = new ShoppingCart();

            foreach (ShoppingCartType shoppingCartType in Enum.GetValues(typeof(ShoppingCartType)))
            {
                shoppingCart.AddRange(GetShoppingCart(shoppingCartType, customerId));
            }

            return shoppingCart;
        }

        /// <summary>
        /// Gets a shopping cart item
        /// </summary>
        /// <param name = "itemId"></param>
        /// <returns></returns>
        public static ShoppingCartItem GetShoppingCartItem(int itemId)
        {
            if (itemId < 0)
            {
                return null;
            }

            return SQLDataAccess.ExecuteReadOne<ShoppingCartItem>
                ("SELECT * FROM Catalog.ShoppingCart WHERE ItemId = @ItemId", CommandType.Text, GetFromReader,
                    new SqlParameter { ParameterName = "@ItemId", Value = itemId }
                );
        }

        private static ShoppingCartItem GetShoppingCartItem(Guid customerId, ShoppingCartItem item)
        {
            return SQLDataAccess.ExecuteReadOne<ShoppingCartItem>
                 (" SELECT * FROM [Catalog].[ShoppingCart] " +
                  " WHERE [CustomerId] = @CustomerId AND " +
                        " [EntityId] = @EntityId AND [ItemType] = @ItemType AND " +
                        " [ShoppingCartTypeId] = @ShoppingCartTypeId AND " +
                        " [AttributesXml] = @AttributesXml",
                     CommandType.Text, GetFromReader,
                     new SqlParameter { ParameterName = "@CustomerId", Value = customerId },
                     new SqlParameter { ParameterName = "@ItemType", Value = item.ItemType },
                     new SqlParameter { ParameterName = "@EntityId", Value = item.EntityId },
                     new SqlParameter { ParameterName = "@AttributesXml", Value = item.AttributesXml ?? string.Empty },
                     new SqlParameter { ParameterName = "@ShoppingCartTypeId", Value = item.ShoppingCartTypeId }
                 );
        }

        public static void AddShoppingCartItem(ShoppingCartItem item)
        {
            var customerId = CustomerSession.CustomerId;
            AddShoppingCartItem(item, customerId);
        }

        public static void AddShoppingCartItem(ShoppingCartItem item, Guid customerId)
        {
            item.CustomerId = customerId;

            if (IsExistProduct(customerId, item))
            {
                int currentAmount = item.Amount;
                item = GetShoppingCartItem(customerId, item);
                item.Amount += currentAmount;
                UpdateShoppingCartItem(item);
            }
            else
            {
                InsertShoppingCartItem(item);
            }
        }

        public static bool IsExistProduct(Guid customerId, ShoppingCartItem item)
        {
            int productsCount = SQLDataAccess.ExecuteScalar<int>
                (" SELECT COUNT([ItemId]) FROM [Catalog].[ShoppingCart] " +
                 " WHERE [CustomerId] = @CustomerId AND [ItemType] = @ItemType AND " +
                       " [EntityId] = @EntityId AND " +
                       " [ShoppingCartTypeId] = @ShoppingCartTypeId AND " +
                       " [AttributesXml] = @AttributesXml",

                    CommandType.Text,
                    new SqlParameter { ParameterName = "@CustomerId", Value = customerId },
                    new SqlParameter { ParameterName = "@ItemType", Value = item.ItemType },
                    new SqlParameter { ParameterName = "@EntityId", Value = item.EntityId },
                    new SqlParameter { ParameterName = "@AttributesXml", Value = item.AttributesXml ?? string.Empty },
                    new SqlParameter { ParameterName = "@ShoppingCartTypeId", Value = item.ShoppingCartTypeId }
                );

            return productsCount != 0;
        }

        /// <summary>
        /// insert new shoppingCartItem, CreatedOn and UpdatedOn must get on sql GetDate()
        /// </summary>
        /// <param name = "shoppingCartItem"></param>
        public static void InsertShoppingCartItem(ShoppingCartItem shoppingCartItem)
        {
            shoppingCartItem.ItemId = SQLDataAccess.ExecuteScalar<int>
                (@"INSERT INTO Catalog.ShoppingCart (ShoppingCartTypeId, CustomerId, EntityId, AttributesXml, Amount, CreatedOn, UpdatedOn, ItemType) 
                   VALUES (@ShoppingCartTypeId, @CustomerId, @EntityId, @AttributesXml, @Amount, GetDate(), GetDate(), @ItemType); Select SCOPE_IDENTITY();",
                    CommandType.Text,
                    new SqlParameter { ParameterName = "@ShoppingCartTypeId", Value = shoppingCartItem.ShoppingCartTypeId },
                    new SqlParameter { ParameterName = "@CustomerId", Value = shoppingCartItem.CustomerId },
                    new SqlParameter { ParameterName = "@EntityId", Value = shoppingCartItem.EntityId },
                    new SqlParameter { ParameterName = "@AttributesXml", Value = shoppingCartItem.AttributesXml ?? string.Empty },
                    new SqlParameter { ParameterName = "@Amount", Value = shoppingCartItem.Amount },
                    new SqlParameter { ParameterName = "@ItemType", Value = shoppingCartItem.ItemType }
                );
        }

        /// <summary>
        /// Updates the shopping cart item
        /// </summary>
        /// <param name = "shoppingCartItem">The shopping cart item</param>
        public static void UpdateShoppingCartItem(ShoppingCartItem shoppingCartItem)
        {
            if (shoppingCartItem == null)
            {
                throw new ArgumentNullException("shoppingCartItem");
            }

            SQLDataAccess.ExecuteNonQuery
                (@"UPDATE [Catalog].[ShoppingCart] SET [ShoppingCartTypeId] = @ShoppingCartTypeId, [CustomerId] = @CustomerId, [EntityId] = @EntityId, [AttributesXml] = @AttributesXml, [UpdatedOn] = GetDate(), [Amount] = @Amount, [ItemType] = @ItemType WHERE [ItemId] = @ItemId",
                    CommandType.Text,
                    new SqlParameter { ParameterName = "@ItemId", Value = shoppingCartItem.ItemId },
                    new SqlParameter { ParameterName = "@ShoppingCartTypeId", Value = shoppingCartItem.ShoppingCartTypeId },
                    new SqlParameter { ParameterName = "@CustomerId", Value = shoppingCartItem.CustomerId },
                    new SqlParameter { ParameterName = "@EntityId", Value = shoppingCartItem.EntityId },
                    new SqlParameter { ParameterName = "@AttributesXml", Value = shoppingCartItem.AttributesXml ?? string.Empty },
                    new SqlParameter { ParameterName = "@Amount", Value = shoppingCartItem.Amount },
                    new SqlParameter { ParameterName = "@ItemType", Value = shoppingCartItem.ItemType }
                );
        }

        public static void ClearShoppingCart(ShoppingCartType shoppingCartType)
        {
            ClearShoppingCart(shoppingCartType, CustomerSession.CustomerId);
        }

        public static void ClearShoppingCart(ShoppingCartType shoppingCartType, Guid customerId)
        {
            SQLDataAccess.ExecuteNonQuery
               ("DELETE FROM Catalog.ShoppingCart WHERE ShoppingCartTypeId = @ShoppingCartTypeId and CustomerId = @CustomerId",
                   CommandType.Text,
                   new SqlParameter { ParameterName = "@ShoppingCartTypeId", Value = shoppingCartType },
                   new SqlParameter { ParameterName = "@CustomerId", Value = customerId }
               );
        }
        
        public static void DeleteExpiredShoppingCartItems(DateTime olderThan)
        {
            SQLDataAccess.ExecuteNonQuery
                ("DELETE FROM Catalog.ShoppingCart WHERE CreatedOn<@olderThan", CommandType.Text,
                    new SqlParameter { ParameterName = "@olderThan", Value = olderThan }
                );
        }

        public static void DeleteShoppingCartItem(int itemId)
        {
            SQLDataAccess.ExecuteNonQuery
                ("DELETE FROM Catalog.ShoppingCart WHERE ItemId = @ItemId", CommandType.Text,
                    new SqlParameter { ParameterName = "@ItemId", Value = itemId }
                );
        }

        private static ShoppingCartItem GetFromReader(SqlDataReader reader)
        {
            return new ShoppingCartItem
            {
                ItemId = SQLDataHelper.GetInt(reader, "ItemId"),
                ShoppingCartTypeId = SQLDataHelper.GetInt(reader, "ShoppingCartTypeId"),
                CustomerId = SQLDataHelper.GetGuid(reader, "CustomerId"),
                EntityId = SQLDataHelper.GetInt(reader, "EntityId"),
                AttributesXml = SQLDataHelper.GetString(reader, "AttributesXml"),
                Amount = SQLDataHelper.GetInt(reader, "Amount"),
                CreatedOn = SQLDataHelper.GetDateTime(reader, "CreatedOn"),
                UpdatedOn = SQLDataHelper.GetDateTime(reader, "UpdatedOn"),
                ItemType = (EnumItemType)SQLDataHelper.GetInt(reader, "ItemType"),
            };
        }
    }
}