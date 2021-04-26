//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using AdvantShop.Core;
using AdvantShop.Helpers;

namespace AdvantShop.Catalog
{
    public class PropertyService
    {
        /// <summary>
        /// returns all values that includes in property
        /// </summary>
        /// <param name="propertyId"></param>
        /// <returns></returns>
        public static List<PropertyValue> GetValuesByPropertyId(int propertyId)
        {
            List<PropertyValue> list = SQLDataAccess.ExecuteReadList<PropertyValue>("[Catalog].[sp_GetPropertyValuesByPropertyID]", CommandType.StoredProcedure,
                                                                     GetPropertyValueFromReader, new SqlParameter("@PropertyID", propertyId));
            return list;
        }

        /// <summary>
        /// returns all values of propepties belonging to product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static List<PropertyValue> GetPropertyValuesByProductId(int productId)
        {
            List<PropertyValue> list = SQLDataAccess.ExecuteReadList<PropertyValue>("[Catalog].[sp_GetPropertyValuesByProductID]", CommandType.StoredProcedure,
                                                                     GetPropertyValueFromReader, new SqlParameter("@ProductID", productId));
            return list;
        }

        public static PropertyValue GetPropertyValueById(int propertyValueId)
        {
            var res = SQLDataAccess.ExecuteReadOne<PropertyValue>("[Catalog].[sp_GetPropertyValueByID]", CommandType.StoredProcedure, GetPropertyValueFromReader,
                                                                            new SqlParameter("@PropertyValueId", propertyValueId));
            return res;
        }

        private static PropertyValue GetPropertyValueFromReader(SqlDataReader reader)
        {
            return new PropertyValue
            {
                PropertyValueId = SQLDataHelper.GetInt(reader, "PropertyValueID"),
                PropertyId = SQLDataHelper.GetInt(reader, "PropertyID"),
                Value = SQLDataHelper.GetString(reader, "Value"),
                SortOrder = SQLDataHelper.GetInt(reader, "SortOrder"),
                Property = new Property
                               {
                                   PropertyId = SQLDataHelper.GetInt(reader, "PropertyID"),
                                   Name = SQLDataHelper.GetString(reader, "PropertyName"),
                                   SortOrder = SQLDataHelper.GetInt(reader, "PropertySortOrder")
                               }
            };
        }

        /// <summary>
        /// returns property that include curent value
        /// </summary>
        /// <param name="valueId"></param>
        /// <returns></returns>
        public static Property GetPropertyByValueId(int valueId)
        {
            var prop = SQLDataAccess.ExecuteReadOne<Property>("[Catalog].[sp_GetPropertyByValueID]", CommandType.StoredProcedure,
                                                                   reader => new Property
                                                                                 {
                                                                                     PropertyId = SQLDataHelper.GetInt(reader, "PropertyID"),
                                                                                     Name = SQLDataHelper.GetString(reader, "Name"),
                                                                                     UseInFilter = SQLDataHelper.GetBoolean(reader, "UseInFilter"),
                                                                                     SortOrder = SQLDataHelper.GetInt(reader, "SortOrder")
                                                                                 },
                                                                   new SqlParameter("@PropertyValueId", valueId));
            return prop;
        }

        /// <summary>
        /// returns property of product by it's ID
        /// </summary>
        /// <param name="propertyId"></param>
        /// <returns></returns>
        public static Property GetPropertyById(int propertyId)
        {
            var prop = SQLDataAccess.ExecuteReadOne<Property>("[Catalog].[sp_GetPropertyByID]", CommandType.StoredProcedure,
                                                                   reader => new Property
                                                                                 {
                                                                                     PropertyId = propertyId,
                                                                                     Name = SQLDataHelper.GetString(reader, "Name"),
                                                                                     UseInFilter = SQLDataHelper.GetBoolean(reader, "UseInFilter"),
                                                                                     SortOrder = SQLDataHelper.GetInt(reader, "SortOrder")
                                                                                 },
                                                                   new SqlParameter("@PropertyID", propertyId)
                );

            return prop;
        }

        /// <summary>
        /// add's new property into DB
        /// </summary>
        /// <param name="property"></param>
        public static int AddProperty(Property property)
        {
            if (property == null)
                throw new ArgumentNullException("property");

            var res = SQLDataAccess.ExecuteScalar<int>("[Catalog].[sp_AddProperty]", CommandType.StoredProcedure,
                                                        new SqlParameter("@Name", property.Name),
                                                        new SqlParameter("@UseInFilter", property.UseInFilter),
                                                        new SqlParameter("@SortOrder", property.SortOrder)
                                                        );
            return res;

        }

        /// <summary>
        /// Deletes property from DB
        /// </summary>
        /// <param name="propertyId"></param>
        public static void DeleteProperty(int propertyId)
        {
            SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_DeleteProperty]", CommandType.StoredProcedure, new SqlParameter() { ParameterName = "@PropertyID", Value=propertyId });
        }

        /// <summary>
        /// updates property in DB
        /// </summary>
        /// <param name="property"></param>
        public static void UpdateProperty(Property property)
        {
            SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_UpdateProperty]", CommandType.StoredProcedure,
                                                 new SqlParameter("@PropertyID", property.PropertyId),
                                                 new SqlParameter("@Name", property.Name),
                                                 new SqlParameter("@UseInFilter", property.UseInFilter),
                                                 new SqlParameter("@SortOrder", property.SortOrder)
                                                 );
            SQLDataAccess.ExecuteNonQuery("update [Catalog].[PropertyValue] set [UseInFilter]= (select [UseInFilter] from [Catalog].[Property] where [Property].[PropertyID]=@PropertyId) where PropertyId=@PropertyId",
                                            CommandType.Text, new SqlParameter("@PropertyId", property.PropertyId));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="sortOrder"></param>
        public static void UpdateOrInsertProductProperty(int productId, string name, string value, int sortOrder)
        {
            SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_UpdateOrInsertProductProperty]", CommandType.StoredProcedure,
                                                 new SqlParameter("@ProductID", productId),
                                                 new SqlParameter("@Name", name),
                                                 new SqlParameter("@Value", value),
                                                 new SqlParameter("@SortOrder", sortOrder)
                                                 );
        }

        /// <summary>
        /// adds new value for some property
        /// </summary>
        /// <param name="propVal"></param>
        public static int AddPropertyValue(PropertyValue propVal)
        {
            if (propVal == null)
                throw new ArgumentNullException("propVal");
            if (propVal.PropertyId == 0)
                throw new ArgumentException(@"PropertyId cannot be zero", "propVal");

            var propValId = SQLDataAccess.ExecuteScalar<int>("[Catalog].[sp_AddPropertyValue]", CommandType.StoredProcedure,
                                                            new SqlParameter("@Value", propVal.Value),
                                                             new SqlParameter("@PropertyID", propVal.PropertyId),
                                                              new SqlParameter("@SortOrder", propVal.SortOrder)
                                                              );
            return propValId;
        }

        /// <summary>
        /// Deletes value from DB
        /// </summary>
        /// <param name="propertyValueId"></param>
        public static void DeletePropertyValueById(int propertyValueId)
        {
            SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_DeletePropertyValue]", CommandType.StoredProcedure, new SqlParameter("@PropertyValueID", propertyValueId));
        }

        /// <summary>
        /// updates value in DB
        /// </summary>
        /// <param name="value"></param>
        public static void UpdatePropertyValue(PropertyValue value)
        {
            SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_UpdatePropertyValue]", CommandType.StoredProcedure,
                                                 new SqlParameter("@Value", value.Value),
                                                 new SqlParameter("@SortOrder", value.SortOrder),
                                                 new SqlParameter("@PropertyValueId", value.PropertyValueId)
                                                 );
        }

        /// <summary>
        /// returns all products that includes this value
        /// </summary>
        /// <param name="propVal"></param>
        /// <returns></returns>
        public static List<int> GetProductsIDsByPropertyValue(PropertyValue propVal)
        {
            List<int> productIDs = SQLDataAccess.ExecuteReadList<int>("[Catalog].[sp_GetProductsIDsByPropertyValue]",
                                                                 CommandType.StoredProcedure,
                                                                 reader => SQLDataHelper.GetInt(reader, "ProductID"),
                                                                 new SqlParameter("@ValueID", propVal.PropertyValueId));
            return productIDs;
        }

        public static int GetProductsCountByProperty(int propId)
        {
            return SQLDataAccess.ExecuteScalar<int>("[Catalog].[sp_GetCOUNTProductsByProperty]", CommandType.StoredProcedure, new SqlParameter("@PropertyID", propId));
        }

        public static IList<int> GetAllPropertiesId()
        {
            List<int> res = SQLDataAccess.ExecuteReadList("SELECT [PropertyID] FROM [Catalog].[Property]",
                                                     CommandType.Text,
                                                     reader => SQLDataHelper.GetInt(reader, "PropertyID"));
            return res;
        }

        public static IList<Property> GetAllProperties()
        {
            List<Property> res = SQLDataAccess.ExecuteReadList<Property>("[Catalog].[sp_GetAllProperties]", CommandType.StoredProcedure, GetPropertyFromReader);
            return res;
        }

        private static Property GetPropertyFromReader(SqlDataReader reader)
        {
            return new Property
                       {
                           PropertyId = SQLDataHelper.GetInt(reader, "PropertyId"),
                           Name = SQLDataHelper.GetString(reader, "Name"),
                           SortOrder = SQLDataHelper.GetInt(reader, "SortOrder"),
                           UseInFilter = SQLDataHelper.GetBoolean(reader, "UseInFilter")
                       };
        }

        public static void DeleteProductPropertyValue(int productId, int propertyValueId)
        {
            SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_DeleteProductPropertyValue]", CommandType.StoredProcedure,
                                            new SqlParameter("@ProductID", productId),
                                            new SqlParameter("@PropertyValueID", propertyValueId)
                                            );
        }

        public static void DeleteProductProperties(int productId)
        {
            SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_DeleteProductProperties]", CommandType.StoredProcedure,
                                            new SqlParameter("@ProductID", productId)
                                            );
        }

        public static void AddProductProperyValue(int propValId, int productId, int sort)
        {
            if (propValId == 0)
                throw new ArgumentException(@"Value cannot be zero", "propValId");

            SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_AddProductPropertyValue]", CommandType.StoredProcedure,
                                            new SqlParameter("@ProductID", productId),
                                            new SqlParameter("@PropertyValueID", propValId),
                                            new SqlParameter("@SortOrder", sort)
                                            );
        }

        public static int GetNewPropertyValueSortOrder(int productId)
        {
            var intResult = SQLDataHelper.GetInt(SQLDataAccess.ExecuteScalar("SELECT MAX(SortOrder) + 10 FROM [Catalog].[ProductPropertyValue] where ProductID=@ProductID",
                                                                CommandType.Text, new SqlParameter("@ProductID", productId)), 10);
            return intResult;
        }

        public static void UpdateProductPropertyValue(int productId, int oldPropertyValueId, int newPropertyValueId)
        {
            if (oldPropertyValueId == 0)
                throw new ArgumentException(@"Value cannot be zero", "oldPropertyValueId");
            if (newPropertyValueId == 0)
                throw new ArgumentException(@"Value cannot be zero", "newPropertyValueId");
            SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_UpdateProductProperty]", CommandType.StoredProcedure,
                                            new SqlParameter("@ProductID", productId),
                                            new SqlParameter("@OldPropertyValueID", oldPropertyValueId),
                                            new SqlParameter("@NewPropertyValueID", newPropertyValueId)
                                            );
        }

        public static void UpdateProductPropertyValue(int productId, int propertyValueId, string value)
        {
            if (propertyValueId == 0)
                throw new ArgumentException(@"Value cannot be zero", "propertyValueId");
            //I was drunk
            int propertyId = GetPropertyByValueId(propertyValueId).PropertyId;
            DeleteProductPropertyValue(productId, propertyValueId);
            AddProductProperyValue(AddPropertyValue(new PropertyValue { PropertyId = propertyId, Value = value, SortOrder = 0 }), productId, 0);
        }

        public static void AddProductProperty(int productId, string name, string value, int propSortOrder, int valSortOrder, bool useInFilter)
        {
            //todo low perfomance in loop
            var prop = GetAllProperties().FirstOrDefault(p => p.Name == name)
                        ?? GetPropertyById(AddProperty(new Property { Name = name, SortOrder = propSortOrder, UseInFilter = useInFilter }));
            var propval = prop.ValuesList.FirstOrDefault(pv => pv.Value == value)
                        ?? GetPropertyValueById(AddPropertyValue(new PropertyValue { Value = value, PropertyId = prop.PropertyId, SortOrder = valSortOrder }));
            AddProductProperyValue(propval.PropertyValueId, productId, 0);
        }

        public static IEnumerable<PropertyValue> GetPropertyValuesByCategories(int categoryId, bool useDepth)
        {
            return SQLDataAccess.ExecuteReadIEnumerable<PropertyValue>(
                @"[Catalog].[sp_GetPropertyInFilter]",
                CommandType.StoredProcedure,
                reader => new PropertyValue
                    {
                        PropertyValueId = SQLDataHelper.GetInt(reader, "PropertyValueID"),
                        PropertyId = SQLDataHelper.GetInt(reader, "PropertyID"),
                        Value = SQLDataHelper.GetString(reader, "Value"),
                        Property = new Property
                                       {
                                           PropertyId=SQLDataHelper.GetInt(reader, "PropertyID"),
                                           Name = SQLDataHelper.GetString(reader, "PropertyName"),
                                           SortOrder = SQLDataHelper.GetInt(reader, "PropertySortOrder")
                                       }
                    },
                    new SqlParameter("@categoryId", categoryId),
                    new SqlParameter("@useDepth", useDepth)
                );
        }

        public static void AddAndUpdateProductProperty(int productId, string propName, string propValue, int propSortOrder, int valSortOrder, bool useInFilter)
        {

            if (CheckIsProductPropertyExistByPropNameAndPropVal(productId, propName, propValue))
            {
                return; // Есть. Нечего тут делать.
            }
            
            //
            // Загружаем свойства -----------------------------------------------------------------------
            //

            // Получаем свойство
            // Если нет его, то создаем
            var prop = GetPropertyByName(propName.ToLower())
                        ?? GetPropertyById(AddPropertyEntityIntoDbAndReturnPropId(new Property { Name = propName, SortOrder = propSortOrder, UseInFilter = useInFilter }));

            // Получаем значение
            // Если нет его, то создаем
            var propval = prop.ValuesList.FirstOrDefault(pv => pv.Value == propValue)
                        ?? GetPropertyValueById(AddPropertyValueIntoDbAndReturnPropId(new PropertyValue { Value = propValue, PropertyId = prop.PropertyId, SortOrder = valSortOrder }));

            //
            // Обновляем в БД ------------------------------------------------------------------------
            //

            // Проверям если у продукта данное свойство (только по названию).
            if (CheckIsProductPropertyExistByPropName(productId, propName))
            {
                // Есть. Нужно обновить на новое значение
                UpdateProductPropertyValue(productId, GetPropValueIdByProductIdAndPropName(productId, propName), propval.PropertyValueId);
            }
            else
            {
                // Нет. Нужо добавить свойство (и значение) к товару.
                AddProductProperyValue(propval.PropertyValueId, productId, propSortOrder);
            }
        }

        public static Boolean CheckIsProductPropertyExistByPropNameAndPropVal(int productId, string strPropName, string strPropValue)
        {
            return SQLDataAccess.ExecuteScalar<int>("SELECT COUNT(Catalog.ProductPropertyValue.ProductID) FROM Catalog.ProductPropertyValue INNER JOIN Catalog.PropertyValue ON Catalog.ProductPropertyValue.PropertyValueID = Catalog.PropertyValue.PropertyValueID INNER JOIN Catalog.Property ON Catalog.PropertyValue.PropertyID = Catalog.Property.PropertyID "
                                             +
                                             "WHERE (Catalog.PropertyValue.Value = @Value) AND (Catalog.Property.Name = @Name) AND (Catalog.ProductPropertyValue.ProductID = @PID)",
                                             CommandType.Text,
                                             new SqlParameter("@Value", strPropValue),
                                             new SqlParameter("@Name", strPropName),
                                             new SqlParameter("@PID", productId)) > 0;
        }

        /// <summary>
        /// returns property of product by it's name
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static Property GetPropertyByName(string propertyName)
        {
            var prop = SQLDataAccess.ExecuteReadOne<Property>("select top(1) * from [Catalog].[Property] where lower(Name)=@Name", CommandType.Text,
                                                                   reader => new Property
                                                                                 {
                                                                                     PropertyId = SQLDataHelper.GetInt(reader, "PropertyID"),
                                                                                     Name = SQLDataHelper.GetString(reader, "Name"),
                                                                                     UseInFilter = SQLDataHelper.GetBoolean(reader, "UseInFilter"),
                                                                                     SortOrder = SQLDataHelper.GetInt(reader, "SortOrder")
                                                                                 },
                                                                   new SqlParameter("@Name", propertyName)
                );

            return prop;
        }

        /// <summary>
        /// add's new property into DB
        /// </summary>
        /// <param name="property"></param>
        public static int AddPropertyEntityIntoDbAndReturnPropId(Property property)
        {
            if (property == null)
                throw new ArgumentNullException("property");

            var res = SQLDataAccess.ExecuteScalar<int>("[Catalog].[sp_AddProperty]",
                                                        CommandType.StoredProcedure,
                                                        new SqlParameter("@Name", property.Name),
                                                        new SqlParameter("@UseInFilter", property.UseInFilter),
                                                        new SqlParameter("@SortOrder", property.SortOrder)
                                                        );
            return res;
        }

        /// <summary>
        /// adds new value for some property
        /// </summary>
        /// <param name="propVal"></param>
        public static int AddPropertyValueIntoDbAndReturnPropId(PropertyValue propVal)
        {
            if (propVal == null)
                throw new ArgumentNullException("propVal");
            if (propVal.PropertyId == 0)
                throw new ArgumentException(@"PropertyId cannot be zero", "propVal");

            var propValId = SQLDataAccess.ExecuteScalar<int>("[Catalog].[sp_AddPropertyValue]", CommandType.StoredProcedure,
                                                                new SqlParameter("@Value", propVal.Value),
                                                                new SqlParameter("@PropertyID", propVal.PropertyId),
                                                                new SqlParameter("@SortOrder", propVal.SortOrder)
                                                                );
            return propValId;
        }

        public static Boolean CheckIsProductPropertyExistByPropName(int productId, string strPropName)
        {

            return
            SQLDataAccess.ExecuteScalar<int>("SELECT COUNT(Catalog.ProductPropertyValue.ProductID) FROM Catalog.ProductPropertyValue INNER JOIN Catalog.PropertyValue ON Catalog.ProductPropertyValue.PropertyValueID = Catalog.PropertyValue.PropertyValueID INNER JOIN Catalog.Property ON Catalog.PropertyValue.PropertyID = Catalog.Property.PropertyID WHERE (Catalog.Property.Name = @Name) AND (Catalog.ProductPropertyValue.ProductID = @PID)",
                                               CommandType.Text,
                                               new SqlParameter("@Name", strPropName),
                                               new SqlParameter("@PID", productId)
                                               ) > 0;
        }

        public static Int32 GetPropValueIdByProductIdAndPropName(int productId, string propName)
        {
            int intPropValueIdResult = SQLDataAccess.ExecuteReadOne("SELECT TOP(1) Catalog.ProductPropertyValue.PropertyValueID as intPropValueIDResult FROM Catalog.ProductPropertyValue INNER JOIN Catalog.PropertyValue ON Catalog.ProductPropertyValue.PropertyValueID = Catalog.PropertyValue.PropertyValueID INNER JOIN Catalog.Property ON Catalog.PropertyValue.PropertyID = Catalog.Property.PropertyID " +
                                                                    "WHERE (Catalog.ProductPropertyValue.ProductID = @ProductID) AND (Catalog.Property.Name = @Name)",
                                                                    CommandType.Text, reader => SQLDataHelper.GetInt(reader, "intPropValueIDResult", 0),
                                                                    new SqlParameter("@ProductID", productId), new SqlParameter("@Name", propName));
            return intPropValueIdResult;
        }

        public static void ShowInFilter(int propertyId, bool showInFilter)
        {
            SQLDataAccess.ExecuteNonQuery(
                "UPDATE [Catalog].[Property] SET UseInFilter = @UseInFilter WHERE [PropertyID] = @PropertyID",
                CommandType.Text, new SqlParameter("@PropertyID", propertyId),
                new SqlParameter("@UseInFilter", showInFilter));
        }

        public static void UpdateProductPropertyValueWithSort(int productId, int propertyValueId, string value, int sort)
        {
            if (propertyValueId == 0)
                throw new ArgumentException(@"Value cannot be zero", "propertyValueId");
            //I was drunk
            int propertyId = GetPropertyByValueId(propertyValueId).PropertyId;
            DeleteProductPropertyValue(productId, propertyValueId);
            AddProductProperyValue(AddPropertyValue(new PropertyValue { PropertyId = propertyId, Value = value, SortOrder = 0 }), productId, sort);
        }

        public static void UpdateProductPropertyValueWithSort(int productId, int oldPropertyValueId, int newPropertyValueId, int sortOrder)
        {
            if (oldPropertyValueId == 0)
                throw new ArgumentException(@"Value cannot be zero", "oldPropertyValueId");
            if (newPropertyValueId == 0)
                throw new ArgumentException(@"Value cannot be zero", "newPropertyValueId");

            SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_UpdateProductPropertyWithSort]", CommandType.StoredProcedure,
                                            new SqlParameter("@ProductID", productId),
                                            new SqlParameter("@OldPropertyValueID", oldPropertyValueId),
                                            new SqlParameter("@NewPropertyValueID", newPropertyValueId),
                                            new SqlParameter("@SortOrder", sortOrder)
                                            );
        }
    }
}