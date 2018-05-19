using DataBrowser.Data;
using DataBrowser.Data.Repository;
using DataBrowser.Service.Interface;
using DataBrowser.Service.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBrowser.Service.Services
{
    public class DataBrowserService : IDataBrowserService
    {

        public List<DatabrowserDropdownFilterServiceModel> GetTableConfigurationDetails()
        {
            try
            {
                var details = new List<DatabrowserDropdownFilterServiceModel>();
                using (var repo = new RepositoryPattern<TableConfiguration>())
                {
                    details = repo.SelectAll().Select(a => new DatabrowserDropdownFilterServiceModel
                    { ConnectionId = a.ConnectionId ?? 0, Id = a.Id, MasterTableName = a.MasterTableName, Name = a.Name }).ToList();
                }
                return details;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public DataSet GetDataBrowserDetails(DatabrowserDropdownFilterServiceModel fieldDetailsFilterModel)
        {
            try
            {
                if (fieldDetailsFilterModel == null)
                {
                    // return exceptions
                }
                //get connection related Details
                int connectionId = fieldDetailsFilterModel.ConnectionId;
                var connectionDetails = new DatabaseConnection();
                var paginations = fieldDetailsFilterModel.PageSize * (fieldDetailsFilterModel.PageNumber - 1);

                using (var repo = new RepositoryPattern<DatabaseConnection>())
                {
                    connectionDetails = repo.SelectByID(connectionId);
                }
                List<FieldConfiguration> fieldConfigurationDetails = new List<FieldConfiguration>();
                using (var repo = new RepositoryPattern<FieldConfiguration>())
                {
                    fieldConfigurationDetails = repo.SelectAll().Where(a => a.TableConfigId == fieldDetailsFilterModel.Id && a.IsDisplay.HasValue && a.IsDisplay.Value).ToList();
                }
                string refTableSelectQuery = string.Empty;
                string masterTableAlias = $"{fieldDetailsFilterModel.MasterTableName}_{DateTime.UtcNow.ToFileTimeUtc()}";

                ///////
                var leftJoinDetails = GetLeftJoinTablesDetailsToDisplay(fieldConfigurationDetails, fieldDetailsFilterModel, masterTableAlias, out refTableSelectQuery);
                //////

                string selectQuery = GetSelectQueryDetails(fieldDetailsFilterModel.MasterTableName, fieldConfigurationDetails, refTableSelectQuery, masterTableAlias);
                string totalCount = "SELECT COUNT(*) AS [TotalCount] FROM " + fieldDetailsFilterModel.MasterTableName;

                string query = selectQuery + " " + leftJoinDetails + " ORDER BY " + masterTableAlias + ".Id OFFSET " + paginations + " ROWS FETCH NEXT " + fieldDetailsFilterModel.PageSize + " ROWS ONLY  " + totalCount;

                string connectionString = "server= " + connectionDetails.ServerInstanceName + ";Initial Catalog=" + connectionDetails.DatabaseName + " ;uid=" + connectionDetails.UserName + ";pwd=" + connectionDetails.Password + ";";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        DataSet ds = new DataSet();
                        SqlDataAdapter da = new SqlDataAdapter();
                        da = new SqlDataAdapter(cmd);
                        da.Fill(ds);
                        conn.Close();
                        return ds;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private string GetLeftJoinTablesDetailsToDisplay(List<FieldConfiguration> fieldConfigurationDetails, DatabrowserDropdownFilterServiceModel fieldDetailsFilterModel, string masterTableAlias, out string refTableSelectQuery)
        {
            var newFieldConfigurationDetails = fieldConfigurationDetails.Where(c => c.FieldMappingConfigurations.Any()).ToList();
            var leftJoinDetails = string.Empty;
            var refTableSelectQueriesWithAlias = new List<string>();
            newFieldConfigurationDetails.ForEach(v =>
            {
                var queries = string.Empty;
                var onQuery = string.Empty;
                List<string> displayColumns = new List<string>();
                var tableAlias = $"{v.ReferenceTableName}_{DateTime.UtcNow.ToFileTimeUtc()}";
                if (!string.IsNullOrEmpty(v.ReferenceTableName) && !string.IsNullOrWhiteSpace(v.ReferenceTableName))
                {
                    onQuery = tableAlias + @"." + v.ReferenceColumnName + @" = " + masterTableAlias + @"." + v.SourceColumnName;
                }
                queries = @" LEFT JOIN [" + v.ReferenceTableName + "] " + tableAlias + @" ON " + onQuery;
                leftJoinDetails += queries;
                if (v.FieldMappingConfigurations.Any())
                {
                    displayColumns = v.FieldMappingConfigurations.Select(mapping => tableAlias + @"." + mapping.MapColumnName).ToList();
                }
                refTableSelectQueriesWithAlias.AddRange(displayColumns);
            });
            refTableSelectQuery = String.Join(",", refTableSelectQueriesWithAlias);
            return leftJoinDetails;
        }

        private string GetSelectQueryDetails(string masterTable, List<FieldConfiguration> fieldConfigurationDetails, string refTableSelectQuery, string masterTableAlias)
        {
            string finalQuery = string.Empty;
            string masterTableDisplayColumns = GetSourcecolumnDetails(masterTable, fieldConfigurationDetails, masterTableAlias);

            finalQuery = (string.IsNullOrEmpty(refTableSelectQuery) || string.IsNullOrWhiteSpace(refTableSelectQuery))
                ? @"SELECT " + masterTableDisplayColumns + " FROM " + masterTable + @" " + masterTableAlias
                : @"SELECT " + masterTableDisplayColumns + "," + refTableSelectQuery + " FROM " + masterTable + @" " + masterTableAlias;
            return finalQuery;
        }
        private string GetSourcecolumnDetails(string masterTable, List<FieldConfiguration> fieldsConfigDetails, string masterTableAlias)
        {
            List<string> displayColumns = new List<string>();
            displayColumns = fieldsConfigDetails.Select(a => "" + masterTableAlias + "." + a.SourceColumnName).ToList();
            var fieldWithCommaSaperators = String.Join(",", displayColumns);
            return fieldWithCommaSaperators;
        }
    }

}
