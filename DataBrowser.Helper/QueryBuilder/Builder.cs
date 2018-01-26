using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBrowser.Helper.QueryBuilder
{
    public static class Builder
    {
        public static QueryBuilderServiceModel From(string tableName)
        {
            return new QueryBuilderServiceModel(tableName);
        }
    }
}
