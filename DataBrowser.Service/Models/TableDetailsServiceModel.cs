using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBrowser.Service.Models
{
    public class TableDetailsServiceModel
    {
        public string ColumnName { get; set; }
        public string ConstraintsType { get; set; }
        public string RelationShipTableName { get; set; }
        public string PrimaryTableColumnName { get; set; }
        public List<string> ReferenceTableColumns { get; set; }

    }
}
