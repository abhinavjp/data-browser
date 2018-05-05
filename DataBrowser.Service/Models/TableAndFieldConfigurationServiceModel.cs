using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBrowser.Service.Models
{
    public class TableAndFieldConfigurationServiceModel
    {
        public TableConfiguratonServiceModel tableConfiguration { get; set; }
        public List<FieldConfigurationServiceModel> fieldConfiguration { get; set; }
    }
}
