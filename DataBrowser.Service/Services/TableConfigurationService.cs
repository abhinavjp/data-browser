using AutoMapper;
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
    public class TableConfigurationService : ITableConfigurationService
    {
        public TableConfigListsServiceModel GetDatabaseConnectionName()
        {
            try
            {
                List<IdNameServiceModel> dataBaseLists = new List<IdNameServiceModel>();
                using (var dataBaseConnectionRepo = new RepositoryPattern<DatabaseConnection>())
                {
                    dataBaseLists = dataBaseConnectionRepo.SelectAll().Select(a => new IdNameServiceModel { Name = a.Name, Id = a.Id }).ToList();
                }
                List<TableConfiguration> tableConfigLists = new List<TableConfiguration>();
                using (var repo = new RepositoryPattern<TableConfiguration>())
                {
                    tableConfigLists = repo.SelectAll().ToList();
                }
                return new TableConfigListsServiceModel()
                {
                    IdAndName = dataBaseLists,
                    TableConfigList = Mapper.Map<List<TableConfiguratonServiceModel>>(tableConfigLists)
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<string> GetTablesFromDatabase(TableConfigurationDatabaseFilterServiceModel dataToFilter)
        {
            try
            {
                if (dataToFilter.ConnectionId == default(int))
                {
                    return null;
                }
                //get Database Name from database connection
                DatabaseConnection dataBaseDetails;
                List<string> tableNames = new List<string>();
                using (var dataBaseConnectionRepo = new RepositoryPattern<DatabaseConnection>())
                {
                    dataBaseDetails = dataBaseConnectionRepo.SelectByID(dataToFilter.ConnectionId);
                }
                if (dataBaseDetails == null)
                    return null;

                var connectionString = "server= " + dataBaseDetails.ServerInstanceName + ";Initial Catalog=" + dataBaseDetails.DatabaseName + " ;uid=" + dataBaseDetails.UserName + ";pwd=" + dataBaseDetails.Password + ";";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = string.Empty;
                    if (dataToFilter.IsTable)
                        query = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE';";
                    else
                        query = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.VIEWS";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (IDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                                tableNames.Add(dr[0].ToString());
                        }
                    }
                }


                return tableNames;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<TableDetailsServiceModel> GetTablesDetails(IdNameServiceModel tableFilters)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tableFilters.Name))
                {
                    //return error;
                }
                var tableDetails = GetTablesFieldsDetails(tableFilters.Id, tableFilters.Name);
                tableDetails.ForEach(v =>
                {
                    var refTableColumn = new List<string>();
                    if (!string.IsNullOrWhiteSpace(v.RelationShipTableName) && !string.IsNullOrEmpty(v.RelationShipTableName))
                    {
                        refTableColumn = GetPrimaryKeyTableColumns(new IdNameServiceModel() { Id = tableFilters.Id, Name = v.RelationShipTableName });
                        v.ReferenceTableColumns = refTableColumn;
                    }
                });
                return tableDetails;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<string> GetPrimaryKeyTableColumns(IdNameServiceModel columnFilter)
        {
            try
            {
                if (columnFilter == null)
                {
                    //return exception
                }
                if (columnFilter.Id == default(int))
                {
                    //return exception
                }
                DatabaseConnection dataBaseDetails;
                List<string> columnName = new List<string>();
                List<TableDetailsServiceModel> tableDetails = new List<TableDetailsServiceModel>();
                using (var dataBaseConnectionRepo = new RepositoryPattern<DatabaseConnection>())
                {
                    dataBaseDetails = dataBaseConnectionRepo.SelectByID(columnFilter.Id);
                }
                if (dataBaseDetails == null)
                {
                    //return null;
                }
                var connectionString = "server= " + dataBaseDetails.ServerInstanceName + ";Initial Catalog=" + dataBaseDetails.DatabaseName + " ;uid=" + dataBaseDetails.UserName + ";pwd=" + dataBaseDetails.Password + ";";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = string.Empty;
                    query = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + columnFilter.Name + "' ORDER BY ORDINAL_POSITION ";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (IDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                                columnName.Add(dr[0].ToString());
                        }
                    }
                    conn.Close();
                }

                return columnName;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string SaveTableConfiguraionDetails(TableAndFieldConfigurationServiceModel tableAndFieldConfiguration)
        {
            try
            {
                if (tableAndFieldConfiguration == null)
                {
                    //return exce
                }
                if (!tableAndFieldConfiguration.fieldConfiguration.Any())
                {
                    //return
                }
                var tableConfigData = Mapper.Map<TableConfiguration>(tableAndFieldConfiguration.tableConfiguration);
                using (var tableConfigRepo = new RepositoryPattern<TableConfiguration>())
                {
                    tableConfigRepo.Insert(tableConfigData);
                    tableConfigRepo.Save();
                }

                //for entry in table
                tableAndFieldConfiguration.fieldConfiguration.ForEach(v => { v.TableConfigId = tableConfigData.Id; v.IsDisplay = true; });
                List<FieldConfiguration> fieldDetails = Mapper.Map<List<FieldConfiguration>>(tableAndFieldConfiguration.fieldConfiguration);
                using (var fieldConfigRepo = new RepositoryPattern<FieldConfiguration>())
                {
                    fieldConfigRepo.BulkInsert(fieldDetails);
                }
                List<FieldMappingConfiguration> fieldMappingConfigurations = new List<FieldMappingConfiguration>();
                tableAndFieldConfiguration.fieldConfiguration.ForEach(field =>
                               {
                                   var dataWithId = fieldDetails.FirstOrDefault(a => a.SourceColumnName == field.SourceColumnName
                                   && a.TableConfigId == field.TableConfigId && a.ReferenceTableName == field.ReferenceTableName
                                   && a.ReferenceColumnName == field.ReferenceColumnName).Id;
                                   field.Id = dataWithId;
                               });
                tableAndFieldConfiguration.fieldConfiguration.ForEach(field =>
                               {
                                   if (!string.IsNullOrWhiteSpace(field.ReferenceTableName) && !string.IsNullOrWhiteSpace(field.ReferenceColumnName))
                                   {
                                       if (field.MappedCoumns.Any())
                                       {
                                           field.MappedCoumns.ForEach(m =>
                                           {
                                               var mappingDetails = new FieldMappingConfiguration()
                                               {
                                                   FieldConfigurationId = field.Id,
                                                   MapColumnName = m,
                                                   MapTableName = field.ReferenceTableName,
                                               };
                                               fieldMappingConfigurations.Add(mappingDetails);
                                           });
                                       }
                                   }
                               });

                if (fieldMappingConfigurations.Any())
                {
                    using (var fieldMappingConfigRepo = new RepositoryPattern<FieldMappingConfiguration>())
                    {
                        fieldMappingConfigRepo.BulkInsert(fieldMappingConfigurations);
                    }
                }
                return "Table Configuration Saved Succefully";
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public TableAndFieldConfigurationServiceModel GetDetailstableAndFieldsById(int id)
        {
            try
            {
                if (id == default(int))
                {
                    // return excepions
                }
                var tableAndFieldConfigurationDetails = new TableAndFieldConfigurationServiceModel();
                var tableConfigDetails = new TableConfiguration();
                using (var repo = new RepositoryPattern<TableConfiguration>())
                {
                    tableConfigDetails = repo.SelectByID(id);
                }
                tableAndFieldConfigurationDetails.tableConfiguration = Mapper.Map<TableConfiguratonServiceModel>(tableConfigDetails);

                //Getting Table Fields and Mapping Configuration Details
                var fieldsConfigurationDetails = new List<FieldConfiguration>();
                using (var repo = new RepositoryPattern<FieldConfiguration>())
                {
                    fieldsConfigurationDetails = repo.SelectAll().Where(f => f.TableConfigId == tableConfigDetails.Id).ToList();
                }
                tableAndFieldConfigurationDetails.fieldConfiguration = fieldsConfigurationDetails.Select(field =>
                 {
                     var refTableColumn = new List<string>();
                     if (!string.IsNullOrWhiteSpace(field.ReferenceTableName) && !string.IsNullOrEmpty(field.ReferenceTableName))
                     {
                         refTableColumn = GetPrimaryKeyTableColumns(new IdNameServiceModel() { Id = tableConfigDetails.ConnectionId ?? 0, Name = field.ReferenceTableName });
                     }
                     return new FieldConfigurationServiceModel
                     {
                         Id = field.Id,
                         IsDisplay = (field.IsDisplay.HasValue) ? field.IsDisplay.Value : false,
                         SourceColumnName = field.SourceColumnName,
                         TableConfigId = field.TableConfigId,
                         ReferenceColumnName = (field.IsDisplay.HasValue && field.IsDisplay.Value) ? field.ReferenceColumnName : string.Empty,
                         ReferenceTableName = (field.IsDisplay.HasValue && field.IsDisplay.Value) ? field.ReferenceTableName : string.Empty,
                         ReferenceTableColumns = (field.IsDisplay.HasValue && field.IsDisplay.Value) ? refTableColumn : new List<string>(),
                         MappedCoumns = (field.IsDisplay.HasValue && field.IsDisplay.Value) ? field.FieldMappingConfigurations.
                                             Where(b => b.FieldConfigurationId == field.Id).Select(n => n.MapColumnName).ToList() : new List<string>()
                     };
                 }).ToList();

                var tableDetails = GetTablesFieldsDetails(tableConfigDetails.ConnectionId ?? 0, tableConfigDetails.MasterTableName);
                var deletedColumns = tableAndFieldConfigurationDetails.fieldConfiguration
                                   .Where(d => !tableDetails.Any(c => c.ColumnName.ToLower() == d.SourceColumnName.ToLower())).ToList();
                if (deletedColumns.Any())
                {
                    DeleteColumnsIfNeeded(deletedColumns);
                }

                if (tableDetails.Any())
                {
                    UpdateConstraintTypeAndInsertColumn(tableDetails, tableAndFieldConfigurationDetails.fieldConfiguration, tableAndFieldConfigurationDetails.tableConfiguration.ConnectionId ?? 0);
                }

                return tableAndFieldConfigurationDetails;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string UpdateTableAndfieldconfiguration(TableAndFieldConfigurationServiceModel tableAndFieldConfiguration)
        {
            try
            {
                if (tableAndFieldConfiguration == null)
                {
                    //return Exception;
                }
                var dataToInsert = new List<FieldConfiguration>();
                var dataToUpdate = new List<FieldConfiguration>();
                var idsToDeleteFieldMapping = new List<FieldMappingConfiguration>();

                using (var repo = new RepositoryPattern<TableConfiguration>())
                {
                    var detailsToUpdate = Mapper.Map<TableConfiguration>(tableAndFieldConfiguration.tableConfiguration);
                    repo.Update(detailsToUpdate);
                    repo.Save();
                }
                tableAndFieldConfiguration.fieldConfiguration.ForEach(f =>
                { f.TableConfigId = tableAndFieldConfiguration.tableConfiguration.Id; f.IsDisplay = true; });

                dataToInsert = Mapper.Map<List<FieldConfiguration>>(tableAndFieldConfiguration.fieldConfiguration.Where(f => f.Id == default(int)).ToList());
                dataToUpdate = Mapper.Map<List<FieldConfiguration>>(tableAndFieldConfiguration.fieldConfiguration.Where(u => u.Id != default(int)).ToList());

                var configurationIds = dataToUpdate.Select(i => i.Id).ToList();

                using (var repo = new RepositoryPattern<FieldConfiguration>())
                {
                    //Update existing records which will not display in future
                    var records = repo.SelectAll().Where(f => f.TableConfigId == tableAndFieldConfiguration.tableConfiguration.Id
                    && !configurationIds.Any(c => c == f.Id)).Select(c => { c.IsDisplay = false; return c; }).ToList();

                    if (records.Any())
                        dataToUpdate.AddRange(records);

                    if (dataToUpdate.Any())
                    {
                        repo.BulkUpdate(dataToUpdate);
                    }
                    if (dataToInsert.Any())
                    {
                        repo.BulkInsert(dataToInsert);
                    }

                    idsToDeleteFieldMapping.AddRange(repo.SelectAll().Where(h => h.TableConfigId == tableAndFieldConfiguration.tableConfiguration.Id && h.FieldMappingConfigurations.Any())
                        .SelectMany(g => g.FieldMappingConfigurations).ToList());
                }
                tableAndFieldConfiguration.fieldConfiguration.ForEach(field =>
                               {
                                   if (field.Id == default(int))
                                   {
                                       var dataWithIds = dataToInsert.FirstOrDefault(a => a.SourceColumnName == field.SourceColumnName
                                                  && a.TableConfigId == field.TableConfigId && a.ReferenceTableName == field.ReferenceTableName
                                                  && a.ReferenceColumnName == field.ReferenceColumnName && field.IsDisplay).Id;
                                       field.Id = dataWithIds;
                                   }
                               });

                List<FieldMappingConfiguration> fieldMappingConfigurations = new List<FieldMappingConfiguration>();
                tableAndFieldConfiguration.fieldConfiguration.ForEach(fields =>
                              {
                                  if (fields.MappedCoumns.Any())
                                  {
                                      fields.MappedCoumns.ForEach(m =>
                                      {
                                          var mappingDetails = new FieldMappingConfiguration()
                                          {
                                              FieldConfigurationId = fields.Id,
                                              MapColumnName = m,
                                              MapTableName = fields.ReferenceTableName,
                                            };
                                          fieldMappingConfigurations.Add(mappingDetails);
                                      });
                                  }
                              });

                using (var repo = new RepositoryPattern<FieldMappingConfiguration>())
                {
                    if (idsToDeleteFieldMapping.Any())
                    {
                        repo.BulkDelete(idsToDeleteFieldMapping);
                    }
                    if (fieldMappingConfigurations.Any())
                    {
                        repo.BulkInsert(fieldMappingConfigurations);
                    }
                }

                return "Table and field mapping Save succesfully";

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private void UpdateConstraintTypeAndInsertColumn(List<TableDetailsServiceModel> tableDetails, List<FieldConfigurationServiceModel> fieldConfigurationDetailsToUpdate, int connectionId)
        {

            tableDetails.ForEach(s =>
            {
                var updateDetails = fieldConfigurationDetailsToUpdate.FirstOrDefault(d => d.SourceColumnName.ToLower() == s.ColumnName.ToLower());
                if (updateDetails != null) { updateDetails.ConstraintsType = s.ConstraintsType; }
                var refTableColumn = new List<string>();
                if (!string.IsNullOrWhiteSpace(s.RelationShipTableName) && !string.IsNullOrEmpty(s.RelationShipTableName))
                {
                    refTableColumn = GetPrimaryKeyTableColumns(new IdNameServiceModel() { Id = connectionId, Name = s.RelationShipTableName });
                }
                if (updateDetails == null || (!updateDetails.IsDisplay && updateDetails.Id == default(int)))
                {
                    var insertColumns = new FieldConfigurationServiceModel()
                    {
                        SourceColumnName = s.ColumnName,
                        ReferenceColumnName = s.PrimaryTableColumnName,
                        ReferenceTableName = s.RelationShipTableName,
                        ConstraintsType = s.ConstraintsType,
                        ReferenceTableColumns = refTableColumn,
                        MappedCoumns = new List<string> { s.PrimaryTableColumnName }
                    };
                    fieldConfigurationDetailsToUpdate.Add(insertColumns);
                }
                else
                {
                    if (!updateDetails.IsDisplay)
                    {
                        updateDetails.SourceColumnName = s.ColumnName;
                        updateDetails.ReferenceColumnName = s.PrimaryTableColumnName;
                        updateDetails.ReferenceTableName = s.RelationShipTableName;
                        updateDetails.ConstraintsType = s.ConstraintsType;
                        updateDetails.ReferenceTableColumns = refTableColumn;
                        updateDetails.MappedCoumns = new List<string> { s.PrimaryTableColumnName };
                    }
                }
            });
        }
        private void DeleteColumnsIfNeeded(List<FieldConfigurationServiceModel> deleteData)
        {
            var idsToDelete = deleteData.Select(v => v.Id).ToList();
            using (var repo = new RepositoryPattern<FieldConfiguration>())
            {
                var dataToDelete = repo.SelectAll().Where(v => idsToDelete.Contains(v.Id)).ToList();
                repo.BulkDelete(dataToDelete);
            }
            using (var repo = new RepositoryPattern<FieldMappingConfiguration>())
            {
                var dataToDelete = repo.SelectAll().Where(v => idsToDelete.Contains(v.FieldConfigurationId ?? 0)).ToList();
                repo.BulkDelete(dataToDelete);
            }
        }
        private List<TableDetailsServiceModel> GetTablesFieldsDetails(int connectionId, string masterTableName)
        {
            if (connectionId == default(int))
                return null;

            var dataBaseDetails = new DatabaseConnection();
            List<TableDetailsServiceModel> tableDetails = new List<TableDetailsServiceModel>();
            using (var dataBaseConnectionRepo = new RepositoryPattern<DatabaseConnection>())
            {
                dataBaseDetails = dataBaseConnectionRepo.SelectByID(connectionId);
            }
            if (dataBaseDetails == null)
            {
                //return null;
            }
            var connectionString = "server= " + dataBaseDetails.ServerInstanceName + ";Initial Catalog=" + dataBaseDetails.DatabaseName + " ;uid=" + dataBaseDetails.UserName + ";pwd=" + dataBaseDetails.Password + ";";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = string.Empty;
                query = @";WITH RelationshipTableDetails AS (
                            SELECT
                                pk.TABLE_NAME AS RelationShipTableName,
                                ku.COLUMN_NAME AS ForeignKeyColumnName,
                                PT.COLUMN_NAME AS PrimaryTableColumnName
                                FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS rc
                                INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS pk
                                ON rc.UNIQUE_CONSTRAINT_NAME = pk.CONSTRAINT_NAME
                                INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS fk
                                ON rc.CONSTRAINT_NAME = fk.CONSTRAINT_NAME
                                INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE ku
                                ON rc.CONSTRAINT_NAME = ku.CONSTRAINT_NAME
                                INNER JOIN (
                                SELECT i1.TABLE_NAME, i2.COLUMN_NAME
                                FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS i1
                                INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE i2
                                ON i1.CONSTRAINT_NAME = i2.CONSTRAINT_NAME
                                WHERE i1.CONSTRAINT_TYPE = 'PRIMARY KEY')
                                PT ON PT.TABLE_NAME = PK.TABLE_NAME
                                WHERE fk.TABLE_NAME = '" + masterTableName + @"'
                                 ),
                            TableConstraintsDetails AS (
                                SELECT
                                t.TABLE_NAME AS TableName,
                                c.COLUMN_NAME AS CoumnName,
                                t.CONSTRAINT_TYPE AS ConstraintsType
                                from INFORMATION_SCHEMA.TABLE_CONSTRAINTS t
                                LEFT JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE c
                                ON t.CONSTRAINT_CATALOG = c.CONSTRAINT_CATALOG
                                AND t.CONSTRAINT_SCHEMA = c.CONSTRAINT_SCHEMA
                                AND t.CONSTRAINT_NAME = c.CONSTRAINT_NAME
                                WHERE t.TABLE_NAME = '" + masterTableName + @"'
                                )
                            SELECT
                                c1.COLUMN_NAME AS ColumnName,
                                tcDetails.ConstraintsType AS ConstraintsType,
                                rlDetails.RelationShipTableName AS RelationShipTableName,
                                rlDetails.PrimaryTableColumnName AS PrimaryTableColumnName
                                FROM INFORMATION_SCHEMA.COLUMNS c1
                                LEFT JOIN TableConstraintsDetails tcDetails
                                ON tcDetails.CoumnName = c1.COLUMN_NAME
                                LEFT JOIN RelationshipTableDetails rlDetails
                                ON rlDetails.ForeignKeyColumnName = c1.COLUMN_NAME
                                WHERE c1.TABLE_NAME = '" + masterTableName + @"'
                            ";


                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var tables = new TableDetailsServiceModel();
                            tables.ColumnName = (dr["ColumnName"] == DBNull.Value || dr["ColumnName"] == null) ? "" : dr["ColumnName"].ToString();
                            tables.ConstraintsType = (dr["ConstraintsType"] == DBNull.Value || dr["ConstraintsType"] == null) ? "" : dr["ConstraintsType"].ToString();
                            tables.RelationShipTableName = (dr["RelationShipTableName"] == DBNull.Value || dr["RelationShipTableName"] == null) ? "" : dr["RelationShipTableName"].ToString();
                            tables.PrimaryTableColumnName = (dr["PrimaryTableColumnName"] == DBNull.Value || dr["PrimaryTableColumnName"] == null) ? "" : dr["PrimaryTableColumnName"].ToString();
                            tableDetails.Add(tables);
                        }
                    }
                }
                conn.Close();

            }
            return tableDetails;
        }
    }


}
