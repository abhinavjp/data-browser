using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBrowser.Service.Models
{
    public class TableConfiguratonServiceModel
    {
        public int Id { get; set; } // Id (Primary key)
        public string Name { get; set; } // Name (length: 50)
        public string DataKey { get; set; } // DataKey (length: 50)
        public bool? IsTable { get; set; } // IsTable
        public bool? IsView { get; set; } // IsView
        public string MasterTableName { get; set; } // MasterTableName (length: 50)
        public int? ConnectionId { get; set; } // ConnectionId
    }
}
