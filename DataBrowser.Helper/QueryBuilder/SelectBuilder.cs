using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBrowser.Helper.QueryBuilder
{
    public static class SelectBuilder
    {
        public static QueryBuilderServiceModel Select(this QueryBuilderServiceModel queryBuilderModel, IEnumerable<string> columnNames)
        {
            queryBuilderModel.SelectedColumns = columnNames?.Select(s => new SelectBuilderModel(s, queryBuilderModel.FromTableName)).ToList();
            return queryBuilderModel;
        }

        public static QueryBuilderServiceModel Select(this QueryBuilderServiceModel queryBuilderModel, IEnumerable<ISelectColumnModel> columnNamesWithAlias)
        {
            queryBuilderModel.SelectedColumns = columnNamesWithAlias?.Select(s => new SelectBuilderModel(s, queryBuilderModel.FromTableName)).ToList();
            return queryBuilderModel;
        }

        public static QueryBuilderServiceModel Select(this QueryBuilderServiceModel queryBuilderModel, IEnumerable<ISelectColumnTableModel> columnNamesWithAliasAndTable)
        {
            queryBuilderModel.SelectedColumns = columnNamesWithAliasAndTable?.Select(s => new SelectBuilderModel(s)).ToList();
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
