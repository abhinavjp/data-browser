using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBrowser.Helper.QueryBuilder
{
    public class QueryBuilderServiceModel
    {
        private string FromTableAlias => $"FromTable{DateTime.UtcNow.ToFileTimeUtc()}";
        private string Query => $"SELECT {BuildSelectClause()} FROM [{FromTableName}] AS [{FromTableAlias}] {WhereQuery}";
        public string WhereQuery => WhereFilters != null && WhereFilters.Any() ? $" WHERE {FilterBuilder.BuildFilters(BuildProperFilters())}" : "";
        internal QueryBuilderServiceModel(string tableName)
        {
            FromTableName = tableName;
        }
        internal QueryBuilderServiceModel()
        {
            SelectedColumns = new List<SelectBuilderModel>();
            WhereFilters = new List<FilterBuilderModel>();
            JoiningTables = new List<JoinTableModel>();
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
        private List<FilterBuilderModel> BuildProperFilters()
        {
            if (WhereFilters == null || !WhereFilters.Any())
                return new List<FilterBuilderModel>();

            var tableModel = new List<TableBuilderModel>();
            tableModel.AddItems(WhereFilters.Select(s => s.SourceTableName).Distinct().Select(s => new TableBuilderModel
            {
                TableName = s
            }));

            tableModel.AddItems(WhereFilters.Where(w => !string.IsNullOrWhiteSpace(w.DestinationTableName))
                            .Select(s => s.DestinationTableName).Distinct().Select(s => new TableBuilderModel
                            {
                                TableName = s
                            }));

            tableModel.AddItem(new TableBuilderModel
            {
                TableName = FromTableName,
                TableAlias = FromTableAlias
            }, true);

            BuildTableAliases(tableModel);

            WhereFilters.ForEach(f =>
            {
                var sourceTableItem = tableModel.FirstOrDefault(a => a.TableName == f.SourceTableName);
                if (sourceTableItem == null)
                {
                    tableModel.AddItem(new TableBuilderModel
                    {
                        TableName = f.SourceTableName,
                        TableAlias = $"{f.SourceTableName}_{DateTime.UtcNow.ToFileTimeUtc()}"
                    });
                    sourceTableItem = tableModel.FirstOrDefault(a => a.TableName == f.SourceTableName);
                }
                f.SourceTableAlias = sourceTableItem.TableAlias;


                var destinationTableItem = tableModel.FirstOrDefault(a => a.TableName == f.DestinationTableName);
                if (destinationTableItem == null)
                {
                    tableModel.AddItem(new TableBuilderModel
                    {
                        TableName = f.DestinationTableName,
                        TableAlias = $"{f.DestinationTableName}_{DateTime.UtcNow.ToFileTimeUtc()}"
                    });
                    destinationTableItem = tableModel.FirstOrDefault(a => a.TableName == f.DestinationTableName);
                }
                f.DestinationTableAlias = destinationTableItem.TableAlias;
            });
            return WhereFilters;
        }

        private static void BuildTableAliases(List<TableBuilderModel> tableBuilderModels)
        {
            tableBuilderModels = tableBuilderModels.Where(w => !string.IsNullOrWhiteSpace(w.TableName)).GroupBy(g => g.TableName).Select(s => s.FirstOrDefault()).ToList();
            tableBuilderModels.ForEach(f =>
            {
                Debug.WriteLine($"Before: Table Name: {f.TableName}, Alias: {f.TableAlias}");
                if (string.IsNullOrWhiteSpace(f.TableAlias))
                    f.TableAlias = $"{f.TableName}_{DateTime.UtcNow.ToFileTimeUtc()}";
                Debug.WriteLine($"After: Table Name: {f.TableName}, Alias: {f.TableAlias}");
            });
        }
    }
}
