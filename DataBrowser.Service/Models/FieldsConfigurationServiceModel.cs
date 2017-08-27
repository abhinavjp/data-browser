using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBrowser.Service.Models
{
    public class FieldsConfigurationServiceModel
    {
        private string _columnAlias;
        public int Id { get; set; }
        public int TableConfigurationId { get; set; }
        public string SourceColumnName { get; set; }
        public string SourceColumnType { get; set; }
        public string SourceColumnSize { get; set; }
        public string TargetColumnName { get; set; }
        public string TargetColumnType { get; set; }
        public string TargetColumnSize { get; set; }
        public string ReferencedColumnName { get; set; }
        public string ReferencedColumnType { get; set; }
        public string ReferencedColumnSize { get; set; }
        public string ColumnAlias
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_columnAlias))
                    return SourceColumnName;
                return _columnAlias;
            }
            set
            {
                _columnAlias = value;
            }
        }
        public bool ToBeShown { get; set; }
        public string ReferencedTableName { get; set; }
        public string TargetTableName { get; set; }
        public string TargetTableAlias
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(TargetTableName))
                {
                    return $"{TargetTableName}{ColumnAlias}{TargetColumnName}";
                }
                if (!string.IsNullOrWhiteSpace(ReferencedTableName))
                {
                    return $"{ReferencedTableName}{ColumnAlias}{ReferencedColumnName}";
                }
                return string.Empty;
            }
        }
    }
}
