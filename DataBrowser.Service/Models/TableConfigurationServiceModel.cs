using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBrowser.Service.Models
{
    public class TableConfigurationServiceModel
    {
        public int Id { get; set; }
        public int ConnectionId { get; set; }
        public string DatabaseName { get; set; }
        public string TableName { get; set; }
        public string PrimaryKey { get; set; }
        public bool IsView { get; set; }
        public ConnectionConfigurationServiceModel ConnectionConfiguration { get; set; }
        public string ConnectionString => $"data source={ConnectionConfiguration.ServerInstance};initial catalog={DatabaseName};user id={ConnectionConfiguration.Username};password={ConnectionConfiguration.Password};MultipleActiveResultSets=True;App=EntityFramework";
    }
}
