using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBrowser.Helper.QueryBuilder
{
    public static class SelectBuilder
    {
        public static QueryBuilderServiceModel Select(this QueryBuilderServiceModel queryBuilderModel, string columnName)
        {
            if (queryBuilderModel.SelectedColumns == null)
                queryBuilderModel.SelectedColumns = new List<SelectBuilderModel>();
            queryBuilderModel.SelectedColumns.Add(new SelectBuilderModel(columnName, queryBuilderModel.FromTableName));
            return queryBuilderModel;
        }
        public static QueryBuilderServiceModel Select(this QueryBuilderServiceModel queryBuilderModel, IEnumerable<string> columnNames)
        {
            if (queryBuilderModel.SelectedColumns == null)
                queryBuilderModel.SelectedColumns = new List<SelectBuilderModel>();
            queryBuilderModel.SelectedColumns.AddRange(columnNames?.Select(s => new SelectBuilderModel(s, queryBuilderModel.FromTableName)));
            return queryBuilderModel;
        }

        public static QueryBuilderServiceModel Select(this QueryBuilderServiceModel queryBuilderModel, IEnumerable<ISelectColumnModel> columnNamesWithAlias)
        {
            if (queryBuilderModel.SelectedColumns == null)
                queryBuilderModel.SelectedColumns = new List<SelectBuilderModel>();
            queryBuilderModel.SelectedColumns.AddRange(columnNamesWithAlias?.Select(s => new SelectBuilderModel(s, queryBuilderModel.FromTableName)));
            return queryBuilderModel;
        }

        public static QueryBuilderServiceModel Select(this QueryBuilderServiceModel queryBuilderModel, IEnumerable<ISelectColumnTableModel> columnNamesWithAliasAndTable)
        {
            if (queryBuilderModel.SelectedColumns == null)
                queryBuilderModel.SelectedColumns = new List<SelectBuilderModel>();
            queryBuilderModel.SelectedColumns.AddRange(columnNamesWithAliasAndTable?.Select(s => new SelectBuilderModel(s)));
            return queryBuilderModel;
        }
        //private List<SelectBuilderModel> _selectColumnList = new List<SelectBuilderModel>();
        //private bool _isSelectAllColumns = true;

        //internal SelectBuilder()
        //{

        //}

        //internal SelectBuilder(List<SelectBuilderModel> selectColumnList)
        //{
        //    if (_selectColumnList != null && _selectColumnList.Any())
        //    {
        //        _selectColumnList = selectColumnList;
        //        _isSelectAllColumns = false;
        //    }
        //}
    }
}
