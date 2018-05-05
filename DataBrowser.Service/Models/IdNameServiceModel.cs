using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBrowser.Service.Models
{
    public class IdNameServiceModel
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }

    public class TableConfigListsServiceModel
    {
        public List<IdNameServiceModel> IdAndName { get; set; }
        public List<TableConfiguratonServiceModel> TableConfigList { get; set; }
    }

}
