using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBrowser.Service.Models
{
    public class DatabrowserDropdownFilterServiceModel
    {
        public int Id { get; set; }
        public int ConnectionId { get; set; }
        public string Name { get; set; }
        public string MasterTableName { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}
