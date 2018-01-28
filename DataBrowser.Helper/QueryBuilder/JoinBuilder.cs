using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBrowser.Helper.QueryBuilder
{
    public class JoinTableModel
    {
        public JoinType JoinType { get; set; }
        public string TableName { get; set; }
        public List<FilterBuilderModel> JoinConditions { get; set; }
    }

    public enum JoinType
    {
        Inner,
        Left,
        Right,
        LeftOuter,
        RightOuter,
        Cross,
        Full
    }
}
