using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBrowser.Service.Models
{
    public class TableConfigurationDatabaseFilterServiceModel
    {
        public int ConnectionId { get; set; }
        public bool IsTable { get; set; }
        public bool IsView { get; set; }
    }
}
