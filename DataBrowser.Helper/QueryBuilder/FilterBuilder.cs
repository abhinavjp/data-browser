using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBrowser.Helper.QueryBuilder
{
    public class FilterBuilderModel
    {
        private string _sourceTableAlias;
        private string _destinationTableAlias;
        public string SourceTableName { get; set; }
        internal string SourceTableAlias
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_sourceTableAlias))
                    return SourceTableName;
                return _sourceTableAlias;
            }
            set
            {
                _sourceTableAlias = value;
            }
        }
        public string SourceColumnName { get; set; }
        internal string SourceCondition => $"[{SourceTableAlias}].[{SourceColumnName}]";
        public string DestinationTableName { get; set; }
        internal string DestinationTableAlias
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_destinationTableAlias))
                    return DestinationTableName;
                return _destinationTableAlias;
            }
            set
            {
                _destinationTableAlias = value;
            }
        }
        public string DestinationColumnName { get; set; }
        public object DestinationColumnValue { get; set; }
        internal string DestinationCondition => IsComparedToDestinationValue ? DestinationColumnValue.ToString() : $"[{DestinationTableAlias}].[{DestinationColumnName}]";
        public List<FilterBuilderModel> InBlockFilters { get; set; }
        public ComparerOperator ComparerOperator { get; set; }
        public bool IsNotComparision { get; set; }
        public ChainLogic InwardConditionLogic { get; set; }
        public ChainLogic OutwardConditionLogic { get; set; }
        public bool IsComparedToDestinationValue { get; set; }
    }
    internal static class FilterBuilder
    {
        public static string BuildFilters(IEnumerable<FilterBuilderModel> filterBuilderModels)
        {
            var finalConditionBuilder = new StringBuilder();
            foreach (var filterBuilderModelItem in filterBuilderModels)
            {
                if (finalConditionBuilder.Length != 0)
                    finalConditionBuilder.Append($" {filterBuilderModelItem.OutwardConditionLogic} ");
                finalConditionBuilder.Append(BuildCondition(filterBuilderModelItem));
            }
            return finalConditionBuilder.ToString();
        }

        public static string BuildCondition(FilterBuilderModel filterBuilderModel)
        {
            if (filterBuilderModel == null)
                return string.Empty;
            var comparisionString = GetComparisionString(filterBuilderModel.ComparerOperator, filterBuilderModel.IsNotComparision, filterBuilderModel.IsComparedToDestinationValue);
            var conditionBuilder = new StringBuilder(string.Format(comparisionString, filterBuilderModel.SourceCondition, filterBuilderModel.DestinationCondition));
            if (filterBuilderModel.InBlockFilters != null)
                conditionBuilder.Append($" {filterBuilderModel.InwardConditionLogic} ({BuildFilters(filterBuilderModel.InBlockFilters)})");

            return conditionBuilder.ToString();
        }

        private static string GetComparisionString(ComparerOperator comparerOperator, bool isNotCondition, bool isValue)
        {
            var conditionOperator = GetComparisionOperator(comparerOperator, isNotCondition);
            var secondArgumentAsString = isValue ? "{1}" : "' + {1} + '";
            switch (comparerOperator)
            {
                case ComparerOperator.Contains:
                    return "{0} " + conditionOperator + " '%" + secondArgumentAsString + "%'";
                case ComparerOperator.StartsWith:
                    return "{0} " + conditionOperator + " '" + secondArgumentAsString + "%'";
                case ComparerOperator.EndsWith:
                    return "{0} " + conditionOperator + " '%" + secondArgumentAsString + "'";
                case ComparerOperator.LessThan:
                    return "{0} " + conditionOperator + " {1}";
                case ComparerOperator.LessThanOrEqualTo:
                    return "{0} " + conditionOperator + " {1}";
                case ComparerOperator.GreaterThan:
                    return "{0} " + conditionOperator + " {1}";
                case ComparerOperator.GreaterThanOrEqualTo:
                    return "{0} " + conditionOperator + " {1}";
                case ComparerOperator.In:
                    return "{0} " + conditionOperator + " ({1})";
                default:
                    return "{0} " + conditionOperator + (isValue ? " '{1}'" : " {1}");
            }
        }

        private static string GetComparisionOperator(ComparerOperator comparerOperator, bool isNotCondition)
        {
            switch (comparerOperator)
            {
                case ComparerOperator.Contains:
                case ComparerOperator.StartsWith:
                case ComparerOperator.EndsWith:
                    return isNotCondition ? "NOT LIKE" : "LIKE";
                case ComparerOperator.LessThan:
                    return isNotCondition ? ">=" : "<";
                case ComparerOperator.LessThanOrEqualTo:
                    return isNotCondition ? ">" : "<=";
                case ComparerOperator.GreaterThan:
                    return isNotCondition ? "<=" : ">";
                case ComparerOperator.GreaterThanOrEqualTo:
                    return isNotCondition ? "<" : ">=";
                case ComparerOperator.In:
                    return isNotCondition ? "NOT IN" : "IN";
                default:
                    return isNotCondition ? "<>" : "=";
            }
        }
    }

    public enum ComparerOperator
    {
        EqualTo,
        Contains,
        StartsWith,
        EndsWith,
        LessThan,
        LessThanOrEqualTo,
        GreaterThan,
        GreaterThanOrEqualTo,
        In
    }

    public enum ChainLogic
    {
        AND,
        OR
    }
}
