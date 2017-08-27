using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBrowser.Service.Models
{
    public class TableConfigurationViewServiceModel
    {
        public int Id { get; set; }
        public int ConnectionId { get; set; }
        public string DatabaseName { get; set; }
        public string TableName { get; set; }
        public string PrimaryKey { get; set; }
        public bool IsView { get; set; }
        public ConnectionConfigurationViewServiceModel ConnectionConfiguration { get; set; }
    }
}
