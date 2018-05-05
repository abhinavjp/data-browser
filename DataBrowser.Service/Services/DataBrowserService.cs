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
                var connectionDetails = new DataBaseConnection();
                var paginations = fieldDetailsFilterModel.PageSize * (fieldDetailsFilterModel.PageNumber - 1);

                using (var repo = new RepositoryPattern<DataBaseConnection>())
                {
                    connectionDetails = repo.SelectByID(connectionId);
                }
                List<FieldConfiguration> fieldConfigurationDetails = new List<FieldConfiguration>();
                using (var repo = new RepositoryPattern<FieldConfiguration>())
                {
                    fieldConfigurationDetails = repo.SelectAll().Where(a => a.SourceTableName == fieldDetailsFilterModel.MasterTableName).ToList();
                }
                var fieldMappingDetails = fieldConfigurationDetails.
                                   Where(c => c.FieldMappingConfigurations.Any()).
                                   SelectMany(a => a.FieldMappingConfigurations.
                                   Where(f => f.FieldConfigurationId == a.Id).ToList());
                List<int> configurationIds = fieldMappingDetails.Select(a => a.FieldConfigurationId ?? 0).ToList();
                var fieldDetailsWithGroupBy = new List<Temporurly>();
                if (configurationIds.Any())
                {
                    fieldDetailsWithGroupBy = fieldConfigurationDetails.
                        Where(a => configurationIds.Contains(a.Id))
                        .GroupBy(g => g.ReferenceTableName)
                        .Select(c => new Temporurly { TableName = c.Key, Values = c.ToList() }).ToList();
                }
                string selectQuery = GetSelectQueryDetails(fieldDetailsFilterModel.MasterTableName, fieldConfigurationDetails, fieldMappingDetails.ToList());
                string leftJoinQuery = GetLeftJoinQueryDetails(fieldDetailsWithGroupBy);
                string totalCount = "SELECT COUNT(*) AS [TotalCount] FROM " + fieldDetailsFilterModel.MasterTableName;

                string query = selectQuery + " " + leftJoinQuery + " ORDER BY [" + fieldDetailsFilterModel.MasterTableName + "].Id OFFSET " + paginations + " ROWS FETCH NEXT " + fieldDetailsFilterModel.PageSize + " ROWS ONLY  " + totalCount;

                string connectionString = "server= " + connectionDetails.ServerInstanceName + ";Initial Catalog=" + connectionDetails.DataBaseName + " ;uid=" + connectionDetails.UserName + ";pwd=" + connectionDetails.Password + ";";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        DataSet ds = new DataSet();
                        SqlDataAdapter da = new SqlDataAdapter();
                        da = new SqlDataAdapter(cmd);
                        da.Fill(ds);
                        return ds;
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }

        }
        private string GetLeftJoinQueryDetails(List<Temporurly> fieldsWIthGroupBy)
        {
            string leftJoinQuery = string.Empty;
            if (fieldsWIthGroupBy.Any())
            {
                fieldsWIthGroupBy.ForEach(a =>
                               {
                                   var qu = string.Empty;
                                   if (a.Values.Count == 1)
                                   {
                                       //Make query without 'AND'
                                       a.Values.ForEach(q =>
                                      {
                                          qu = @"[" + q.SourceTableName + @"]." + q.SourceColumnName + @" = [" + q.ReferenceTableName + @"]." + q.ReferenceColumnName;
                                      });
                                   }
                                   else
                                   {
                                       var lastItem = a.Values.Last();
                                       a.Values.ForEach(q =>
                                       {
                                           var d = string.Empty;
                                           if (q.Equals(lastItem))
                                           {
                                               d = @"[" + q.SourceTableName + @"]." + q.SourceColumnName + @" = [" + q.ReferenceTableName + @"]." + q.ReferenceColumnName;
                                           }
                                           else
                                           {
                                               d = @"[" + q.SourceTableName + @"]." + q.SourceColumnName + @" = [" + q.ReferenceTableName + @"]." + q.ReferenceColumnName + " AND ";
                                           }
                                           qu += d;
                                       });
                                   }
                                   var datas = @" LEFT JOIN [" + a.TableName + @"] ON " + qu;
                                   leftJoinQuery += datas;
                               });
            }
            return leftJoinQuery;
        }
        private string GetSelectQueryDetails(string masterTable, List<FieldConfiguration> fieldConfigurationDetails, List<FieldMappingConfiguration> fieldMappingConfig)
        {
            string finalQuery = string.Empty;
            string masterTableDisplayColumns = GetSourcecolumnDetails(masterTable, fieldConfigurationDetails);
            string refTableDisplayColumns = GetReferenceColumnDetails(masterTable, fieldMappingConfig);
            finalQuery = @"SELECT" + masterTableDisplayColumns + "," + refTableDisplayColumns + " FROM " + masterTable;
            return finalQuery;
        }

        private string GetSourcecolumnDetails(string masterTable, List<FieldConfiguration> fieldsConfigDetails)
        {
            List<string> displayColumns = new List<string>();
            displayColumns = fieldsConfigDetails.Select(a => "[" + a.SourceTableName + "]." + a.SourceColumnName).ToList();
            var fieldWithCommaSaperators = String.Join(",", displayColumns);
            return fieldWithCommaSaperators;
        }
        private string GetReferenceColumnDetails(string masterTable, List<FieldMappingConfiguration> fieldMappingConfig)
        {
            List<string> displayColumns = new List<string>();
            displayColumns = fieldMappingConfig.Select(a => "[" + a.MapTableName + "]." + a.MapColumnName).Distinct().ToList();
            var fieldWithCommaSaperators = String.Join(",", displayColumns);
            return fieldWithCommaSaperators;
        }
    }
    public class Temporurly
    {
        public string TableName { get; set; }
        public List<FieldConfiguration> Values { get; set; }
    }

    public class ReturnTable
    {
        public DataSet MainTable { get; set; }
        public int CountTable { get; set; }
    }
}
