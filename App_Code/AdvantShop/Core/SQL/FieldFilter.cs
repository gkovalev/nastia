//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace AdvantShop.Core.FieldFilters
{
    [Serializable]
    public abstract class FieldFilter
    {
        public string ParamName { get; set; }

        public virtual string GetSqlCondition(string fieldName)
        {
            return fieldName;
        }

        public virtual void SetSqlConditionParameter(List<SqlParameter> cmd)
        {
        }
    }

    [Serializable]
    public class FieldFilterList : FieldFilter
    {
        public List<string> ListFilter { get; set; }

        public override string GetSqlCondition(string fieldName)
        {
            string searchCondition = string.Empty;
            if (!string.IsNullOrEmpty(ParamName))
            {
                for (var i = 0; i <= ListFilter.Count - 1; i++)
                {

                    if (string.IsNullOrEmpty(searchCondition))
                    {
                        searchCondition += ParamName + i;
                    }
                    else
                    {
                        searchCondition += "," + ParamName + i;
                    }

                }
                searchCondition = fieldName + " in (" + searchCondition + ")";
            }
            return searchCondition;
        }

        public override void SetSqlConditionParameter(List<SqlParameter> cmd)
        {
            if (!string.IsNullOrEmpty(ParamName))
            {
                for (var i = 0; i <= ListFilter.Count - 1; i++)
                {
                    var temp = new SqlParameter { ParameterName = ParamName + i, Value = ListFilter[i] };
                    cmd.Add(temp);
                }
            }
        }

    }

    //[Serializable]
    //public class PropertyFileldFilter : FieldFilter
    //{
    //    public string CategoryProperty { get; set; }
    //    public string CategoryValue { get; set; }
    //}

    //[Serializable]
    //public class ListPropertyFileldFilter : FieldFilter
    //{
    //    public List<PropertyFileldFilter> ListCategoryProperty { get; set; }
    //    public override string GetSqlCondition(string fieldName)
    //    {
    //        if (!string.IsNullOrEmpty(ParamName))
    //        {
    //            string searchCondition = string.Empty;
    //            string temp = string.Empty;
    //            for (var i = 0; i <= ListCategoryProperty.Count - 1; i++)
    //            {

    //                if (temp != ListCategoryProperty[i].CategoryProperty)
    //                {
    //                    if (string.IsNullOrEmpty(searchCondition))
    //                    {
    //                        searchCondition += fieldName + " = " + ParamName + i;
    //                    }
    //                    else
    //                    {
    //                        searchCondition += " ! " + fieldName + " = " + ParamName + i;
    //                    }
    //                    temp = ListCategoryProperty[i].CategoryProperty;
    //                }
    //                else
    //                {
    //                    searchCondition += " and " + fieldName + " = " + ParamName + i;
    //                }
    //            }

    //            string[] templist = searchCondition.Split('!');
    //            if (templist.Length > 1)
    //            {
    //                searchCondition = string.Empty;
    //                for (var i = 0; i <= templist.Length - 1; i++)
    //                {
    //                    if (i == templist.Length - 1)
    //                    {
    //                        searchCondition += templist[i] + " ";
    //                    }
    //                    else if (i == 0 && templist.Length == 1)
    //                    {
    //                        searchCondition += templist[i];
    //                    }
    //                    else
    //                    {
    //                        searchCondition += templist[i] + " ) or ( ";
    //                    }

    //                }

    //                searchCondition = "(" + searchCondition + ")";
    //            }
    //            return " (" + searchCondition + ") ";
    //        }
    //        return string.Empty;
    //    }

    //    public override void SetSqlConditionParameter(List<SqlParameter> cmd)
    //    {
    //        if (!string.IsNullOrEmpty(ParamName))
    //        {
    //            for (var i = 0; i <= ListCategoryProperty.Count - 1; i++)
    //            {
    //                var temp = new SqlParameter
    //                               {
    //                                   ParameterName = ParamName + i,
    //                                   Value = ListCategoryProperty[i].CategoryValue
    //                               };
    //                cmd.Add(temp);
    //            }
    //        }
    //    }
    //}

    [Serializable]
    public class RangeFieldFilter : FieldFilter
    {
        public decimal? From { get; set; }
        public decimal? To { get; set; }

        public override string GetSqlCondition(string fieldName)
        {
            string searchCondition = string.Empty;
            if (!string.IsNullOrEmpty(ParamName))
            {
                if (From.HasValue)
                {
                    searchCondition += fieldName + " >= " + ParamName + "1"; // >=
                }
                if (To.HasValue)
                {
                    if (string.IsNullOrEmpty(searchCondition))
                    {
                        searchCondition += fieldName + " <= " + ParamName + "2";
                    }
                    else
                    {
                        searchCondition += " AND " + fieldName + " <= " + ParamName + "2";
                    }
                }
            }
            return searchCondition;
        }

        public override void SetSqlConditionParameter(List<SqlParameter> cmd)
        {
            if (!string.IsNullOrEmpty(ParamName))
            {
                SqlParameter temp;
                if (From.HasValue)
                {
                    temp = new SqlParameter { ParameterName = ParamName + "1", Value = From.Value };
                    cmd.Add(temp);
                }
                if (To.HasValue)
                {
                    temp = new SqlParameter { ParameterName = ParamName + "2", Value = To.Value };
                    cmd.Add(temp);
                }
            }
        }
    }

    [Serializable]
    public class RangeListFieldFilter : FieldFilter
    {
        public List<RangeFieldFilter> ListFilter { get; set; }
        public override string GetSqlCondition(string fieldName)
        {
            if (!string.IsNullOrEmpty(ParamName))
            {
                string searchCondition = string.Empty;
                for (var i = 0; i <= ListFilter.Count - 1; i++)
                {
                    searchCondition +=
                        ((searchCondition != "") ? " OR " : " ")
                        + " (" + fieldName + " >= " + ParamName + "1" + i + " and " + fieldName + " <= " + ParamName + "2" + i + ") ";
                }
                return " (" + searchCondition + ") ";
            }
            return string.Empty;
        }

        public override void SetSqlConditionParameter(List<SqlParameter> cmd)
        {
            if (!string.IsNullOrEmpty(ParamName))
            {
                for (var i = 0; i <= ListFilter.Count - 1; i++)
                {
                    var temp = new SqlParameter { ParameterName = ParamName + "1" + i, Value = ListFilter[i].From };
                    cmd.Add(temp);

                    temp = new SqlParameter { ParameterName = ParamName + "2" + i, Value = ListFilter[i].To };
                    cmd.Add(temp);
                }
            }
        }
    }

    [Serializable]
    public class CompareFieldFilter : FieldFilter
    {
        public string Expression { get; set; }
        public override string GetSqlCondition(string fieldName)
        {
            string searchCondition = string.Empty;
            if (!string.IsNullOrEmpty(ParamName))
            {
                searchCondition = fieldName + " LIKE \'%\'+" + ParamName + "+\'%\'";
            }
            return searchCondition;
        }

        public override void SetSqlConditionParameter(List<SqlParameter> cmd)
        {
            if (!string.IsNullOrEmpty(ParamName))
            {
                var temp = new SqlParameter { ParameterName = ParamName, Value = Expression.Trim() };
                cmd.Add(temp);
            }
        }
    }

    [Serializable]
    public class CompareFieldListFilter : FieldFilter
    {
        public List<CompareFieldFilter> ListFilter { get; set; }

        public override string GetSqlCondition(string fieldName)
        {
            if (!string.IsNullOrEmpty(ParamName))
            {
                string searchCondition = string.Empty;
                for (var i = 0; i <= ListFilter.Count - 1; i++)
                {
                    if (searchCondition == "")
                    {
                        searchCondition = " (" + fieldName + " LIKE \'%\'+" + ParamName + i + "+\'%\'" + ") ";
                    }
                    else
                    {
                        searchCondition += " or (" + fieldName + " LIKE \'%\'+" + ParamName + i + "+\'%\'" + ") ";
                    }
                }
                return " (" + searchCondition + ") ";
            }
            return string.Empty;
        }

        public override void SetSqlConditionParameter(List<SqlParameter> cmd)
        {
            if (!string.IsNullOrEmpty(ParamName))
            {
                for (var i = 0; i <= ListFilter.Count - 1; i++)
                {
                    var temp = new SqlParameter { ParameterName = ParamName + i, Value = ListFilter[i].Expression.Trim() };
                    cmd.Add(temp);
                }
            }
        }
    }

    [Serializable]
    public class NullFieldFilter : FieldFilter
    {
        public bool Null { get; set; }

        public override string GetSqlCondition(string fieldName)
        {
            return (Null ? string.Empty : "NOT ") + fieldName + " IS NULL";
        }

    }

    [Serializable]
    public class EqualFieldFilter : FieldFilter
    {
        public string Value { get; set; }

        public override string GetSqlCondition(string fieldName)
        {
            if (!string.IsNullOrEmpty(ParamName))
            {
                return fieldName + " = " + ParamName;
            }
            return string.Empty;
        }

        public override void SetSqlConditionParameter(List<SqlParameter> cmd)
        {
            if (!string.IsNullOrEmpty(ParamName))
            {
                var temp = new SqlParameter { ParameterName = ParamName, Value = Value.Trim() };
                cmd.Add(temp);
            }
        }
    }

    [Serializable]
    public class PropertyFieldFilter : FieldFilter
    {
        public Dictionary<int, List<int>> ListFilter { get; set; }
        public int CategoryId { get; set; }
        public bool GetSubCategoryes { get; set; }

        public override string GetSqlCondition(string fieldName)
        {
            string sqlSubcomand = string.Empty;
            const string template = @" (SELECT [ProductPropertyValue].[ProductID] FROM [Catalog].[ProductPropertyValue] INNER JOIN [Catalog].[ProductCategories] ON [ProductCategories].[ProductID] = [ProductPropertyValue].[ProductID] ";

            string categoryCondishion = GetSubCategoryes ? " WHERE [CategoryID] IN (SELECT id FROM [Settings].[GetChildCategoryByParent](" + ParamName + "_CatId" + ")) " : " WHERE [CategoryID]=" + ParamName + "_CatId ";

            if (!string.IsNullOrEmpty(ParamName))
            {

                foreach (var i in ListFilter.Keys)
                {
                    string searchCondition = string.Empty;
                    for (var j = 0; j <= ListFilter[i].Count - 1; j++)
                    {
                        if (string.IsNullOrEmpty(searchCondition))
                            searchCondition += ParamName + i + j;
                        else
                            searchCondition += "," + ParamName + i + j;
                    }
                    if (string.IsNullOrEmpty(sqlSubcomand))
                        sqlSubcomand = sqlSubcomand + template + categoryCondishion + " AND [PropertyValueID] in (" + searchCondition + "))";
                    else
                        sqlSubcomand = sqlSubcomand + " INTERSECT " + template + categoryCondishion + " AND [PropertyValueID] in (" + searchCondition + "))";
                }
            }
            return fieldName + " in (" + sqlSubcomand + ")";
        }

        public override void SetSqlConditionParameter(List<SqlParameter> cmd)
        {
            if (!string.IsNullOrEmpty(ParamName))
            {
                cmd.Add(new SqlParameter { ParameterName = ParamName + "_CatId", Value = CategoryId });
                foreach (var i in ListFilter.Keys)
                {
                    for (var j = 0; j <= ListFilter[i].Count - 1; j++)
                    {
                        var temp = new SqlParameter { ParameterName = ParamName + i + j, Value = ListFilter[i][j] };
                        cmd.Add(temp);
                    }
                }
            }
        }
    }

    [Serializable]
    public class ProductIdInIds : FieldFilter
    {
        public List<int> ListFilter { get; set; }

        public override string GetSqlCondition(string fieldName)
        {
            var res = new StringBuilder();
            if (!string.IsNullOrEmpty(ParamName))
                for (int i = 0; i < ListFilter.Count; i++)
                {
                    if (res.Length == 0)
                        res.Append(ParamName + i);
                    else
                        res.Append("," + ParamName + i);
                }
            return fieldName + " in (" + res.ToString() + ")";
        }

        public override void SetSqlConditionParameter(List<SqlParameter> cmd)
        {
            if (!string.IsNullOrEmpty(ParamName))
                for (int i = 0; i < ListFilter.Count; i++)
                    cmd.Add(new SqlParameter { ParameterName = ParamName + i, Value = ListFilter[i] });
        }

    }

    //[Serializable]
    //public class ProductIdInIds2 : FieldFilter
    //{
    //    public string ListFilter { get; set; }

    //    public override string GetSqlCondition(string fieldName)
    //    {
    //        var res = new StringBuilder();
    //        if (!string.IsNullOrEmpty(ParamName))

    //        return fieldName + " in (" + res.ToString() + ")";
    //    }

    //    public override void SetSqlConditionParameter(List<SqlParameter> cmd)
    //    {
    //        if (!string.IsNullOrEmpty(ParamName))
    //            for (int i = 0; i < ListFilter.Count; i++)
    //                cmd.Add(new SqlParameter { ParameterName = ParamName + i, Value = ListFilter[i] });
    //    }

    //}


    [Serializable]
    public class InSetFieldFilter : FieldFilter
    {
        public string[] Values { get; set; }
        public bool IncludeValues { get; set; }

        public override string GetSqlCondition(string fieldName)
        {
            string searchCondition = "";
            if (!IncludeValues)
            {
                searchCondition += "NOT ";
            }
            searchCondition += fieldName + " IN (\'" + string.Join("\' ,\'", Values) + "\')";
            return searchCondition;
        }
    }

    [Serializable]
    public class DateTimeRangeFieldFilter : FieldFilter
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }

        public override string GetSqlCondition(string fieldName)
        {
            string searchCondition = string.Empty;
            if (From.HasValue)
            {
                searchCondition += fieldName + " >= " + ParamName + "_from"; //\'" + From.Value.ToString("yyyy-MM-dd HH:mm:ss") + "\'";
            }
            if (To.HasValue)
            {
                if (string.IsNullOrEmpty(searchCondition))
                {
                    searchCondition += fieldName + " <= " + ParamName + "_to"; //\'" + To.Value.ToString("yyyy-MM-dd HH:mm:ss") + "\'";
                }
                else
                {
                    searchCondition += " AND " + fieldName + " <= " + ParamName + "_to"; //\'" + To.Value.ToString("yyyy-MM-dd HH:mm:ss") + "\'";
                }
            }
            return searchCondition;
        }

        public override void SetSqlConditionParameter(List<SqlParameter> cmd)
        {
            if (!string.IsNullOrEmpty(ParamName))
            {
                var temp = new SqlParameter { ParameterName = ParamName + "_from", Value = From };
                cmd.Add(temp);
                temp = new SqlParameter { ParameterName = ParamName + "_to", Value = To };
                cmd.Add(temp);
            }
        }
    }

    [Serializable]
    public class InChildCategoriesFieldFilter : FieldFilter
    {

        public string CategoryId { get; set; }

        public override string GetSqlCondition(string fieldName)
        {
            if (!string.IsNullOrEmpty(ParamName))
            {
                return fieldName + " IN (SELECT id FROM [Settings].[GetChildCategoryByParent](" + ParamName + "))";
            }
            return string.Empty;
        }

        public override void SetSqlConditionParameter(List<SqlParameter> cmd)
        {
            if (!string.IsNullOrEmpty(ParamName))
            {
                var temp = new SqlParameter { ParameterName = ParamName, Value = CategoryId };
                cmd.Add(temp);
            }
        }
    }

    [Serializable]
    public class CountProductInCategory : FieldFilter
    {
        public override string GetSqlCondition(string fieldName)
        {
            return " ((select Count(CategoryID) from Catalog.ProductCategories where ProductID=[Product].[ProductID])<> 0) ";
        }

        public override void SetSqlConditionParameter(List<SqlParameter> cmd)
        {
        }
    }

    [Serializable]
    public class NotEqualFieldFilter : FieldFilter
    {
        public string Value { get; set; }

        public override string GetSqlCondition(string fieldName)
        {
            if (!string.IsNullOrEmpty(ParamName))
            {
                return fieldName + " <> " + ParamName;
            }
            return string.Empty;
        }

        public override void SetSqlConditionParameter(List<SqlParameter> cmd)
        {
            if (!string.IsNullOrEmpty(ParamName))
            {
                var temp = new SqlParameter { ParameterName = ParamName, Value = Value.Trim() };
                cmd.Add(temp);
            }
        }
    }

    [Serializable]
    public class LogicalFilter : FieldFilter
    {
        public LogicalFilter()
        {
            _filterList = new List<FieldFilter>();
            _logicalOperation = new List<string>();
        }

        private List<FieldFilter> _filterList;

        private List<string> _logicalOperation;

        public void AddLogicalOperation(string op)
        {
            _logicalOperation.Add(op);
        }

        public void AddFilter(FieldFilter filter)
        {
            _filterList.Add(filter);
        }
        public int FilterCount()
        {
            return _filterList.Count;
        }

        public override string GetSqlCondition(string fieldName)
        {
            var outstr = new StringBuilder();
            string[] fs = (from f in _filterList select f.GetSqlCondition(fieldName)).ToArray();
            for (int i = 0; i < fs.Length; i++)
            {
                if (i == 0)
                {
                    outstr.Append(" " + fs[i]);
                }
                else
                {
                    outstr.Append(" " + _logicalOperation[i - 1] + " " + fs[i]);
                }
            }
            return outstr.ToString();
        }

        public override void SetSqlConditionParameter(List<SqlParameter> cmd)
        {
            foreach (var f in _filterList)
            {
                f.SetSqlConditionParameter(cmd);
            }
        }
    }
}
