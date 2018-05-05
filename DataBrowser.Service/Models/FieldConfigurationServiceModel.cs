using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBrowser.Service.Models
{
    public class FieldConfigurationServiceModel
    {
        public int Id { get; set; } // Id (Primary key)
        public string SourceTableName { get; set; } // SourceTableName (length: 50)
        public string SourceColumnName { get; set; } // SourceColumnName (length: 50)
        public string ReferenceTableName { get; set; } // ReferenceTableName (length: 50)
        public string ReferenceColumnName { get; set; } // ReferenceColumnName (length: 50)
        public List<string> MappedCoumns { get; set; }
        public int? TableConfigId { get; set; } // TableConfigId
    }
}
