using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBrowser.Helper.QueryBuilder
{
    public class SelectBuilderModel : ISelectColumnTableModel
    {
        private string _aliasName;
        public SelectBuilderModel(ISelectColumnModel selectColumn, string tableName)
            : this(selectColumn.ColumnName, selectColumn.AliasName, tableName)
        {
        }
        public SelectBuilderModel(ISelectColumnTableModel selectColumn)
            : this(selectColumn.ColumnName, selectColumn.AliasName, selectColumn.TableName)
        {
        }
        public SelectBuilderModel(string columnName, string tableName) : this(columnName, columnName, tableName)
        {
        }
        public SelectBuilderModel(string columnName, string aliasName, string tableName)
        {
            TableName = tableName;
            ColumnName = columnName;
            AliasName = string.IsNullOrWhiteSpace(aliasName) ? columnName : aliasName;
        }
        public string ColumnName { get; set; }
        public string AliasName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_aliasName))
                    return ColumnName;
                return _aliasName;
            }
            set
            {
                _aliasName = value;
            }
        }
        public string TableName { get; set; }
    }

    public interface ISelectColumnModel
    {
        string ColumnName { get; set; }
        string AliasName { get; set; }
    }

    public interface ISelectColumnTableModel : ISelectColumnModel
    {
        string TableName { get; set; }
    }

    public interface IFinalSelectModel : ISelectColumnTableModel
    {
        string TableAlias { get; set; }
    }

    internal class TableBuilderModel
    {
        public string TableName { get; set; }
        public string TableAlias { get; set; }
    }

    internal static class TableBuilderHelper
    {
        public static void AddItem(this List<TableBuilderModel> tableBuilderList, TableBuilderModel tableBuilderItem)
        {
            AddItem(tableBuilderList, tableBuilderItem, false);
        }

        public static void AddItem(this List<TableBuilderModel> tableBuilderList, TableBuilderModel tableBuilderItem, bool overwriteIfExists)
        {
            if (tableBuilderItem == null)
                return;
            if (tableBuilderList == null)
                tableBuilderList = new List<TableBuilderModel>();
            if(overwriteIfExists)
            {
                tableBuilderList.RemoveAll(a => a.TableName.Equals(tableBuilderItem.TableName));
            }
            if (!tableBuilderList.Any(a => a.TableName.Equals(tableBuilderItem.TableName)))
                tableBuilderList.Add(tableBuilderItem);
        }

        public static void AddItems(this List<TableBuilderModel> tableBuilderList, IEnumerable<TableBuilderModel> tableBuilderItems)
        {
            AddItems(tableBuilderList, tableBuilderItems, false);
        }

            public static void AddItems(this List<TableBuilderModel> tableBuilderList, IEnumerable<TableBuilderModel> tableBuilderItems, bool overwriteIfExists)
        {
            if (tableBuilderItems == null)
                return;
            if (tableBuilderList == null)
                tableBuilderList = new List<TableBuilderModel>();

            if (overwriteIfExists)
            {
                tableBuilderList.RemoveAll(a => tableBuilderItems.Any(w => a.TableName.Equals(w.TableName)));
                tableBuilderList.AddRange(tableBuilderItems);
                return;
            }

            tableBuilderList.AddRange(tableBuilderItems.Where(w => !tableBuilderList.Any(a => a.TableName.Equals(w.TableName))));
        }
    }
}
