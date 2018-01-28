using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBrowser.Helper.QueryBuilder
{
    public static class WhereBuilder
    {
        public static QueryBuilderServiceModel Where(this QueryBuilderServiceModel queryBuilder, string sourceTableName, string sourceColumnName, ComparerOperator comparerOperator, string destinationTableName, string destinationColumnName)
        {
            return Where(queryBuilder, sourceTableName, sourceColumnName, comparerOperator, destinationTableName, destinationColumnName, false);
        }

        public static QueryBuilderServiceModel Where(this QueryBuilderServiceModel queryBuilder, string sourceTableName, string sourceColumnName, ComparerOperator comparerOperator, string destinationTableName, string destinationColumnName, bool isNotComparision)
        {
            return Where(queryBuilder, sourceTableName, sourceColumnName, comparerOperator, destinationTableName, destinationColumnName, isNotComparision, ChainLogic.AND);
        }

        public static QueryBuilderServiceModel Where(this QueryBuilderServiceModel queryBuilder, string sourceTableName, string sourceColumnName, ComparerOperator comparerOperator, string destinationTableName, string destinationColumnName, bool isNotComparision, ChainLogic outwardConditionLogic)
        {
            if (queryBuilder.WhereFilters == null)
                queryBuilder.WhereFilters = new List<FilterBuilderModel>();

            queryBuilder.WhereFilters.Add(new FilterBuilderModel
            {
                SourceTableName = sourceTableName,
                SourceColumnName = sourceColumnName,
                ComparerOperator = comparerOperator,
                DestinationTableName = destinationTableName,
                DestinationColumnName = destinationColumnName,
                OutwardConditionLogic = outwardConditionLogic,
                IsNotComparision = isNotComparision
            });
            return queryBuilder;
        }

        public static QueryBuilderServiceModel Where(this QueryBuilderServiceModel queryBuilder, string sourceTableName, string sourceColumnName, ComparerOperator comparerOperator, object destinationValue)
        {
            return Where(queryBuilder, sourceTableName, sourceColumnName, comparerOperator, destinationValue, false);
        }

        public static QueryBuilderServiceModel Where(this QueryBuilderServiceModel queryBuilder, string sourceTableName, string sourceColumnName, ComparerOperator comparerOperator, object destinationValue, bool isNotComparision)
        {
            return Where(queryBuilder, sourceTableName, sourceColumnName, comparerOperator, destinationValue, isNotComparision, ChainLogic.AND);
        }

        public static QueryBuilderServiceModel Where(this QueryBuilderServiceModel queryBuilder, string sourceTableName, string sourceColumnName, ComparerOperator comparerOperator, object destinationValue, bool isNotComparision, ChainLogic outwardConditionLogic)
        {
            if (queryBuilder.WhereFilters == null)
                queryBuilder.WhereFilters = new List<FilterBuilderModel>();

            queryBuilder.WhereFilters.Add(new FilterBuilderModel
            {
                SourceTableName = sourceTableName,
                SourceColumnName = sourceColumnName,
                ComparerOperator = comparerOperator,
                DestinationColumnValue = destinationValue,
                OutwardConditionLogic = outwardConditionLogic,
                IsNotComparision = isNotComparision,
                IsComparedToDestinationValue = true
            });
            return queryBuilder;
        }
    }
}
