using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBrowser.Helper.QueryBuilder
{
    public class JoinTableModel
    {
        public string TableName { get; set; }
        public List<FilterBuilderModel> JoinConditions { get; set; }
    }
}
