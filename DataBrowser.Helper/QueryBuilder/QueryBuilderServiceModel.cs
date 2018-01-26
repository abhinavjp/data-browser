using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBrowser.Helper.QueryBuilder
{
    public class QueryBuilderServiceModel
    {
        private string FromTableAlias => $"FromTable{DateTime.UtcNow.ToFileTimeUtc()}";
        private string Query => $"SELECT {BuildSelectClause()} FROM {FromTableName} AS {FromTableAlias}";
        internal QueryBuilderServiceModel(string tableName)
        {
            FromTableName = tableName;
        }
        internal QueryBuilderServiceModel()
        {

        }
        public string FromTableName { get; set; }
        public List<SelectBuilderModel> SelectedColumns { get; set; }
        public List<FilterBuilderModel> WhereFilters { get; set; }
        public List<JoinTableModel> JoiningTables { get; set; }
        public override string ToString()
        {
            return Query;
        }
        private string BuildSelectClause()
        {

            return string.Join(",", SelectedColumns.Select(s =>
             {
                 var tableName = (string.IsNullOrWhiteSpace(s.TableName) || s.TableName == FromTableName) ? FromTableAlias : s.TableName;
                 return $"[{tableName}].[{s.ColumnName}] AS [{s.AliasName}]";
             }));
        }
    }
}
