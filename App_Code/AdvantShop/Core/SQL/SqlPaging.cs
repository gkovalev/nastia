//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;
using AdvantShop.Helpers;

namespace AdvantShop.Core
{
    [Serializable]
    public struct SqlParam
    {
        public string ParamName;
        public string ParamValue;
    }

    [Serializable]
    public class SqlPaging
    {
        public SqlPaging()
        {
            CurrentPageIndex = 1;
            ItemsPerPage = 10;
        }

        public SqlPaging(string tableName)
            : this()
        {
            TableName = tableName;
        }

        public SqlPaging(string tableName, string tableNameForUnion)
            : this(tableName)
        {
            TableNameForUnion = tableNameForUnion;
        }

        public SqlPaging(string tableName, params Field[] fields)
            : this(tableName)
        {
            AddFieldsRange(fields);
        }
        public SqlPaging(string tableName, string tableNameForUnion, Field[] fields, Field[] unionFields)
            : this(tableName, tableNameForUnion)
        {
            AddFieldsRange(fields);
            AddFieldsRangeUnionTable(unionFields);
        }

        private IDictionary<string, Field> _fields;
        private IDictionary<string, Field> _fieldsUnionTable;

        public string TableName { get; set; }

        public string TableNameForUnion { get; set; }

        public string ExtensionWhere { get; set; }

        public IDictionary<string, Field> Fields
        {
            get { return _fields; }
        }

        public IDictionary<string, Field> FieldsUnionTable
        {
            get { return _fieldsUnionTable; }
        }

        public int ItemsPerPage { get; set; }

        public int CurrentPageIndex { get; set; }

        public int PageCount
        {
            get { return (int)(Math.Ceiling((double)TotalRowsCount / ItemsPerPage)); }
        }

        public int TotalRowsCount
        {
            get
            {
                var cmd = new List<SqlParameter>();
                var searchCondition = string.Empty;
                bool first = true;
                foreach (var f in Fields.Values)
                {
                    if (f.Filter != null)
                    {
                        if (first)
                        {
                            searchCondition = " WHERE ";
                            first = false;
                        }
                        else
                        {
                            searchCondition += " AND ";
                        }

                        searchCondition += f.Filter.GetSqlCondition(f.FilterExpression);
                        f.Filter.SetSqlConditionParameter(cmd);
                    }
                }

                if (_listCondition != null)
                {
                    for (int i = 0; i < _listCondition.Count; i++)
                    {
                        if (string.IsNullOrEmpty(searchCondition))
                        {
                            searchCondition = "WHERE ";
                        }
                        else
                        {
                            searchCondition += " AND ";
                        }
                        searchCondition += _listCondition[i];
                    }
                }

                if (!string.IsNullOrWhiteSpace(ExtensionWhere))
                    searchCondition += " " + ExtensionWhere;

                string seachConditionUnionTable = string.Empty;
                if (!string.IsNullOrEmpty(TableNameForUnion))
                {
                    first = true;
                    foreach (Field f in FieldsUnionTable.Values)
                    {
                        if (f.Filter != null)
                        {
                            if (first)
                            {
                                seachConditionUnionTable = "WHERE ";
                                first = false;
                            }
                            else
                            {
                                seachConditionUnionTable += " AND ";
                            }

                            seachConditionUnionTable += f.Filter.GetSqlCondition(f.FilterExpression);
                            f.Filter.SetSqlConditionParameter(cmd);
                        }
                    }
                }

                var distQuery = from f in Fields.Values
                                where !f.NotInQuery && f.IsDistinct
                                select f.FilterExpression;

                string distinctField;


                if (distQuery.Any())
                {
                    distinctField = "distinct " + distQuery.First();
                }
                else
                {
                    distinctField = "*";
                }

                string query;
                if (!string.IsNullOrEmpty(TableNameForUnion))
                {
                    var distUnionQuery = from f in FieldsUnionTable.Values
                                         where !f.NotInQuery && f.IsDistinct
                                         select f.FilterExpression;
                    string distinctUnionField;
                    if (distUnionQuery.Any())
                    {
                        distinctUnionField = "distinct " + distUnionQuery.First();
                    }
                    else
                    {
                        distinctUnionField = "*";
                    }

                    query = "SELECT (SELECT COUNT(" + distinctField + ") FROM " + TableName + searchCondition +
                            ") + ( SELECT COUNT(" + distinctUnionField + ") FROM " + TableNameForUnion +
                            seachConditionUnionTable + ")";
                }
                else
                {
                    query = "SELECT COUNT(" + distinctField + ") FROM " + TableName + searchCondition;
                }

                if (_listParam != null)
                    cmd.AddRange(_listParam.Select(param => new SqlParameter { ParameterName = param.ParamName, Value = param.ParamValue }));

                var ttlRowsCount = SQLDataAccess.ExecuteScalar<int>(query, CommandType.Text, cmd.ToArray());

                return ttlRowsCount;
            }
        }

        public List<T> ItemsIds<T>(string idName) where T : IConvertible
        {
            //get
            //{
            var cmd = new List<SqlParameter>();
            var searchCondition = string.Empty;
            bool first = true;
            foreach (var f in Fields.Values)
            {
                if (f.Filter != null)
                {
                    if (first)
                    {
                        searchCondition = " WHERE ";
                        first = false;
                    }
                    else
                    {
                        searchCondition += " AND ";
                    }

                    searchCondition += f.Filter.GetSqlCondition(f.FilterExpression);
                    f.Filter.SetSqlConditionParameter(cmd);
                }
            }

            if (!string.IsNullOrWhiteSpace(ExtensionWhere))
                searchCondition += " " + ExtensionWhere;

            string query = "SELECT " + idName + " FROM " + TableName + searchCondition;

            if (_listParam != null)
                cmd.AddRange(_listParam.Select(param => new SqlParameter { ParameterName = param.ParamName, Value = param.ParamValue }));

            List<T> itemsIds = SQLDataAccess.ExecuteReadList(query, CommandType.Text, reader => SQLDataHelper.GetValue<T>(reader, "ID"), cmd.ToArray());

            return itemsIds;
            //}
        }

        public List<T> ItemsUnionIds<T>(string idName) where T : IConvertible
        {
            //get
            //{
            var cmd = new List<SqlParameter>();

            string seachConditionUnionTable = string.Empty;
            if (!string.IsNullOrEmpty(TableNameForUnion))
            {
                bool first = true;
                foreach (Field f in FieldsUnionTable.Values)
                {
                    if (f.Filter != null)
                    {
                        if (first)
                        {
                            seachConditionUnionTable = "WHERE ";
                            first = false;
                        }
                        else
                        {
                            seachConditionUnionTable += " AND ";
                        }

                        seachConditionUnionTable += f.Filter.GetSqlCondition(f.FilterExpression);
                        f.Filter.SetSqlConditionParameter(cmd);
                    }
                }
            }

            string query = string.Empty;
            if (!string.IsNullOrEmpty(TableNameForUnion))
            {
                query = "SELECT " + idName + " FROM " + TableNameForUnion + seachConditionUnionTable;
            }

            if (_listParam != null)
                cmd.AddRange(_listParam.Select(param => new SqlParameter { ParameterName = param.ParamName, Value = param.ParamValue }));

            var itemsIds = new List<T>();
            if (string.IsNullOrEmpty(query))
                return itemsIds;

            itemsIds = SQLDataAccess.ExecuteReadList(query, CommandType.Text, reader => SQLDataHelper.GetValue<T>(reader, "ID"), cmd.ToArray());

            return itemsIds;
            //}
        }


        public List<Guid> ItemsIds(string idName)
        {
            //get
            //{
            var cmd = new List<SqlParameter>();
            var searchCondition = string.Empty;
            bool first = true;
            foreach (var f in Fields.Values)
            {
                if (f.Filter != null)
                {
                    if (first)
                    {
                        searchCondition = " WHERE ";
                        first = false;
                    }
                    else
                    {
                        searchCondition += " AND ";
                    }

                    searchCondition += f.Filter.GetSqlCondition(f.FilterExpression);
                    f.Filter.SetSqlConditionParameter(cmd);
                }
            }

            if (!string.IsNullOrWhiteSpace(ExtensionWhere))
                searchCondition += " " + ExtensionWhere;

            string query = "SELECT " + idName + " FROM " + TableName + searchCondition;

            if (_listParam != null)
                cmd.AddRange(_listParam.Select(param => new SqlParameter { ParameterName = param.ParamName, Value = param.ParamValue }));

            List<Guid> itemsIds = SQLDataAccess.ExecuteReadList(query, CommandType.Text, reader => SQLDataHelper.GetGuid(reader, "ID"), cmd.ToArray());

            return itemsIds;
            //}
        }

        public List<Guid> ItemsUnionIds(string idName)
        {
            //get
            //{
            var cmd = new List<SqlParameter>();

            string seachConditionUnionTable = string.Empty;
            if (!string.IsNullOrEmpty(TableNameForUnion))
            {
                bool first = true;
                foreach (Field f in FieldsUnionTable.Values)
                {
                    if (f.Filter != null)
                    {
                        if (first)
                        {
                            seachConditionUnionTable = "WHERE ";
                            first = false;
                        }
                        else
                        {
                            seachConditionUnionTable += " AND ";
                        }

                        seachConditionUnionTable += f.Filter.GetSqlCondition(f.FilterExpression);
                        f.Filter.SetSqlConditionParameter(cmd);
                    }
                }
            }

            string query = string.Empty;
            if (!string.IsNullOrEmpty(TableNameForUnion))
            {
                query = "SELECT " + idName + " FROM " + TableNameForUnion + seachConditionUnionTable;
            }

            if (_listParam != null)
                cmd.AddRange(_listParam.Select(param => new SqlParameter { ParameterName = param.ParamName, Value = param.ParamValue }));

            var itemsIds = new List<Guid>();
            if (string.IsNullOrEmpty(query))
                return itemsIds;

            itemsIds = SQLDataAccess.ExecuteReadList(query, CommandType.Text, reader => SQLDataHelper.GetGuid(reader, "ID"), cmd.ToArray());

            return itemsIds;
            //}
        }


        public string ExtendedSorting { get; set; }
        public SortDirection ExtendedSortingDirection { get; set; }

        public DataTable PageItems
        {
            get
            {
                string query;

                string columns = string.Join(", ",
                                             (from f in Fields.Values where !f.NotInQuery select f.SelectExpression).ToArray());
                string columnsUnionTable = string.Empty;
                if (!string.IsNullOrEmpty(TableNameForUnion))
                {
                    columnsUnionTable = string.Join(", ",
                                                    (from f in FieldsUnionTable.Values
                                                     where !f.NotInQuery
                                                     select f.SelectExpression).ToArray());
                }

                if ((from f in Fields.Values where f.IsDistinct select f).Any())
                {
                    columns = "distinct " + columns;
                }

                string order = Fields.Values.Where(f => f.Sorting.HasValue).Aggregate(string.Empty, (current, f) => current + ((f.Name.Contains(".") ? "Temp." + f.Name.Split(new[] { '.' })[1] : f.Name) + (f.Sorting.Value == SortDirection.Ascending ? " ASC" : " DESC") + ", "), current => current.TrimEnd(' ', ','));

                if (!string.IsNullOrEmpty(ExtendedSorting))
                    order = (!string.IsNullOrEmpty(order) ? order + " , " : "") + ExtendedSorting +
                        (ExtendedSortingDirection == SortDirection.Ascending ? " ASC " : " DESC ");

                if (string.IsNullOrEmpty(order))
                {
                    order = Fields.FirstOrDefault().Value.Name;
                    order = order.Contains(".") ? "Temp." + order.Split(new[] { '.' })[1] : order;
                }

                string searchCondition = string.Empty;
                var cmd = new List<SqlParameter>();
                var first = true;
                foreach (Field f in Fields.Values)
                {
                    if (f.Filter != null)
                    {
                        if (first)
                        {
                            searchCondition = "WHERE ";
                            first = false;
                        }
                        else
                        {
                            searchCondition += " AND ";
                        }

                        searchCondition += f.Filter.GetSqlCondition(f.FilterExpression);
                        f.Filter.SetSqlConditionParameter(cmd);
                    }
                }

                if (_listCondition != null)
                {
                    for (int i = 0; i < _listCondition.Count; i++)
                    {
                        if (string.IsNullOrEmpty(searchCondition))
                        {
                            searchCondition = "WHERE ";
                        }
                        else
                        {
                            searchCondition += " AND ";
                        }
                        searchCondition += _listCondition[i];
                    }
                }

                if (!string.IsNullOrWhiteSpace(ExtensionWhere))
                    searchCondition += " " + ExtensionWhere;

                string seachConditionUnionTable = string.Empty;
                if (!string.IsNullOrEmpty(TableNameForUnion))
                {
                    first = true;
                    foreach (Field f in FieldsUnionTable.Values)
                    {
                        if (f.Filter != null)
                        {
                            if (first)
                            {
                                seachConditionUnionTable = "WHERE ";
                                first = false;
                            }
                            else
                            {
                                seachConditionUnionTable += " AND ";
                            }

                            seachConditionUnionTable += f.Filter.GetSqlCondition(f.FilterExpression);
                            f.Filter.SetSqlConditionParameter(cmd);
                        }
                    }
                }



                int needRow = CurrentPageIndex * ItemsPerPage;

                int keyid = (CurrentPageIndex - 1) * ItemsPerPage;


                //query = String.Format("WITH Temp AS(SELECT TOP ({0})  Row_Number() OVER (ORDER BY {1} )AS RowNum, {2} FROM {3} {4})SELECT * FROM Temp WHERE RowNum > {5}", needRow, order, columns, TableName, searchCondition, keyid)
                if (string.IsNullOrEmpty(TableNameForUnion))
                {
                    query =
                        string.Format(
                            "WITH Temp AS(SELECT {2} FROM {3} {4})SELECT * FROM (SELECT TOP ({0})  Row_Number() OVER (ORDER BY {1} )AS RowNum,* from Temp) as t WHERE RowNum > {5}",
                            needRow, order, columns, TableName, searchCondition, keyid);
                }
                else
                {
                    query =
                        string.Format(
                            "WITH Temp AS( SELECT {6} from {7} {8} union all SELECT {2} FROM {3} {4})SELECT * FROM (SELECT TOP ({0})  Row_Number() OVER (ORDER BY {1} )AS RowNum,* from Temp) as t WHERE RowNum > {5}",
                            needRow, order, columns, TableName, searchCondition, keyid, columnsUnionTable,
                            TableNameForUnion, seachConditionUnionTable);
                }

                if (_listParam != null)
                    cmd.AddRange(_listParam.Select(param => new SqlParameter { ParameterName = param.ParamName, Value = param.ParamValue }));

                DataTable tbl = SQLDataAccess.ExecuteTable(query, CommandType.Text, cmd.ToArray());
                return tbl;
            }
        }

        public List<int> PropertyValuesList
        {
            get
            {
                var cmd = new List<SqlParameter>();
                var searchCondition = string.Empty;
                bool first = true;
                foreach (var f in Fields.Values)
                {
                    if (f.Filter != null)
                    {
                        if (first)
                        {
                            searchCondition = " WHERE ";
                            first = false;
                        }
                        else
                        {
                            searchCondition += " AND ";
                        }

                        searchCondition += f.Filter.GetSqlCondition(f.FilterExpression);
                        f.Filter.SetSqlConditionParameter(cmd);
                    }
                }

                if (_listParam != null)
                    cmd.AddRange(_listParam.Select(param => new SqlParameter { ParameterName = param.ParamName, Value = param.ParamValue }));

                string query = "SELECT distinct PropertyValueID FROM " + TableName + searchCondition + " AND PropertyValueID is not null";

                List<int> list = SQLDataAccess.ExecuteReadList<int>(query, CommandType.Text, reader => SQLDataHelper.GetInt(reader, "PropertyValueID"), cmd.ToArray());

                return list;
            }
        }

        public List<int> BrandsList
        {
            get
            {
                var cmd = new List<SqlParameter>();
                var searchCondition = string.Empty;
                bool first = true;
                foreach (var f in Fields.Values)
                {
                    if (f.Filter != null)
                    {
                        if (first)
                        {
                            searchCondition = " WHERE ";
                            first = false;
                        }
                        else
                        {
                            searchCondition += " AND ";
                        }

                        searchCondition += f.Filter.GetSqlCondition(f.FilterExpression);
                        f.Filter.SetSqlConditionParameter(cmd);
                    }
                }

                if (_listParam != null)
                    cmd.AddRange(_listParam.Select(param => new SqlParameter { ParameterName = param.ParamName, Value = param.ParamValue }));

                string query = "SELECT distinct brandID FROM " + TableName + searchCondition + " AND brandID is not NULL";

                List<int> list = SQLDataAccess.ExecuteReadList<int>(query, CommandType.Text, reader => SQLDataHelper.GetInt(reader, "brandID"), cmd.ToArray());

                return list;
            }
        }

        public KeyValuePair<decimal, decimal> AvaliblePrices
        {
            get
            {
                var cmd = new List<SqlParameter>();
                var searchCondition = string.Empty;
                bool first = true;
                foreach (var f in Fields.Values)
                {
                    if (f.Filter != null)
                    {
                        if (first)
                        {
                            searchCondition = " WHERE ";
                            first = false;
                        }
                        else
                        {
                            searchCondition += " AND ";
                        }

                        searchCondition += f.Filter.GetSqlCondition(f.FilterExpression);
                        f.Filter.SetSqlConditionParameter(cmd);
                    }
                }

                if (_listParam != null)
                    cmd.AddRange(_listParam.Select(param => new SqlParameter { ParameterName = param.ParamName, Value = param.ParamValue }));

                string query = "SELECT min(Price - Price * discount/100) as PriceFrom, max(Price - Price * discount/100) as PriceTo FROM " + TableName + searchCondition;

                return SQLDataAccess.ExecuteReadOne(query, CommandType.Text, reader => new KeyValuePair<decimal, decimal>(SQLDataHelper.GetDecimal(reader, "PriceFrom"), SQLDataHelper.GetDecimal(reader, "PriceTo")), cmd.ToArray());
            }
        }



        private IList<string> _listCondition;
        public void AddCondition(string f)
        {
            if (_listCondition == null)
            {
                _listCondition = new List<string>();
            }
            _listCondition.Add(f.Trim());
        }

        private IList<SqlParam> _listParam;
        public void AddParam(SqlParam f)
        {
            if (_listParam == null)
            {
                _listParam = new List<SqlParam>();
            }
            _listParam.Add(f);
        }

        public void AddFieldsRange(IEnumerable<Field> fields)
        {
            if (_fields == null)
            {
                _fields = new Dictionary<string, Field>();
            }
            _fields.AddRange(fields.ToDictionary(f => f.Name));
        }

        public void AddFieldsRange(params Field[] fields)
        {
            if (_fields == null)
            {
                _fields = new Dictionary<string, Field>();
            }
            _fields.AddRange(fields.ToDictionary(f => f.Name));
        }

        public void AddField(Field f)
        {
            if (_fields == null)
            {
                _fields = new Dictionary<string, Field>();
            }
            _fields.Add(f.Name, f);
        }

        public void AddFieldsRangeUnionTable(params Field[] fields)
        {
            if (_fieldsUnionTable == null)
            {
                _fieldsUnionTable = new Dictionary<string, Field>();
            }
            _fieldsUnionTable.AddRange(fields.ToDictionary(f => f.Name));
        }

        public void AddFieldUnionTable(Field f)
        {
            if (_fieldsUnionTable == null)
            {
                _fieldsUnionTable = new Dictionary<string, Field>();
            }
            _fieldsUnionTable.Add(f.Name, f);
        }
    }
}