//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop.Core;

namespace AdvantShop.Controls
{

    [ParseChildren(true)]
    public class SqlPagingDataSource : Control
    {
        private readonly SqlPaging _paging = new SqlPaging();

        public string TableName
        {
            get { return _paging.TableName; }
            set { _paging.TableName = value; }
        }

        public int ItemsPerPage
        {
            get { return _paging.ItemsPerPage; }
            set { _paging.ItemsPerPage = value; }
        }

        public int PageCount
        {
            get { return _paging.PageCount; }
        }

        public int TotalCount
        {
            get { return _paging.TotalRowsCount; }
        }

        [TemplateContainer(typeof(Field)), PersistenceMode(PersistenceMode.InnerProperty), Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Field Field
        {
            get
            {
                _paging.Fields.Values.GetEnumerator().MoveNext();
                return _paging.Fields.Values.GetEnumerator().Current;
            }
            set { _paging.AddField(value); }
        }

        public IDictionary<string, Field> Fields
        {
            get { return _paging.Fields; }
        }

        public DataTable Items
        {
            get { return _paging.PageItems; }
        }

        public List<int> PropertyValuesList
        {
            get { return _paging.PropertyValuesList; }
        }

        public List<int> BrandList
        {
            get { return _paging.BrandsList; }
        }

        public KeyValuePair<decimal, decimal> AvaliblePrices
        {
            get { return _paging.AvaliblePrices; }
        }


        public string ExtendedSorting
        {
            get { return _paging.ExtendedSorting; }
            set { _paging.ExtendedSorting = value; }
        }
        public SortDirection ExtendedSortingDirection
        {
            get { return _paging.ExtendedSortingDirection; }
            set { _paging.ExtendedSortingDirection = value; }
        }

        public int CurrentPageIndex
        {
            get { return _paging.CurrentPageIndex; }
            set { _paging.CurrentPageIndex = value; }
        }

        public void AddParamSql(SqlParam param)
        {
            _paging.AddParam(param);
        }

        public void AddCondition(string param)
        {
            _paging.AddCondition(param);
        }
    }
}