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
                using (var dataBaseConnectionRepo = new RepositoryPattern<DataBaseConnection>())
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
                DataBaseConnection dataBaseDetails;
                List<string> tableNames = new List<string>();
                using (var dataBaseConnectionRepo = new RepositoryPattern<DataBaseConnection>())
                {
                    dataBaseDetails = dataBaseConnectionRepo.SelectByID(dataToFilter.ConnectionId);
                }
                if (dataBaseDetails == null)
                    return null;

                var connectionString = "server= " + dataBaseDetails.ServerInstanceName + ";Initial Catalog=" + dataBaseDetails.DataBaseName + " ;uid=" + dataBaseDetails.UserName + ";pwd=" + dataBaseDetails.Password + ";";
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
                DataBaseConnection dataBaseDetails;
                List<TableDetailsServiceModel> tableDetails = new List<TableDetailsServiceModel>();
                using (var dataBaseConnectionRepo = new RepositoryPattern<DataBaseConnection>())
                {
                    dataBaseDetails = dataBaseConnectionRepo.SelectByID(tableFilters.Id);
                }
                if (dataBaseDetails == null)
                {
                    //return null;
                }
                var connectionString = "server= " + dataBaseDetails.ServerInstanceName + ";Initial Catalog=" + dataBaseDetails.DataBaseName + " ;uid=" + dataBaseDetails.UserName + ";pwd=" + dataBaseDetails.Password + ";";
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
                                WHERE fk.TABLE_NAME = '" + tableFilters.Name + @"'
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
                                WHERE t.TABLE_NAME = '" + tableFilters.Name + @"'
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
                                WHERE c1.TABLE_NAME = '" + tableFilters.Name + @"'
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

                }
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
                DataBaseConnection dataBaseDetails;
                List<string> columnName = new List<string>();
                List<TableDetailsServiceModel> tableDetails = new List<TableDetailsServiceModel>();
                using (var dataBaseConnectionRepo = new RepositoryPattern<DataBaseConnection>())
                {
                    dataBaseDetails = dataBaseConnectionRepo.SelectByID(columnFilter.Id);
                }
                if (dataBaseDetails == null)
                {
                    //return null;
                }
                var connectionString = "server= " + dataBaseDetails.ServerInstanceName + ";Initial Catalog=" + dataBaseDetails.DataBaseName + " ;uid=" + dataBaseDetails.UserName + ";pwd=" + dataBaseDetails.Password + ";";
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
                tableAndFieldConfiguration.fieldConfiguration.ForEach(v => { v.TableConfigId = tableConfigData.Id; });
                List<FieldConfiguration> fieldDetails = Mapper.Map<List<FieldConfiguration>>(tableAndFieldConfiguration.fieldConfiguration);
                using (var fieldConfigRepo = new RepositoryPattern<FieldConfiguration>())
                {
                    fieldConfigRepo.BulkInsert(fieldDetails);
                }

                List<FieldMappingConfiguration> fieldMappingConfigurations = new List<FieldMappingConfiguration>();
                tableAndFieldConfiguration.fieldConfiguration.ForEach(field =>
                               {
                                   var dataWithId = fieldDetails.FirstOrDefault(a => a.SourceColumnName == field.SourceColumnName && a.SourceTableName == field.SourceTableName && a.ReferenceTableName == field.ReferenceTableName && a.ReferenceColumnName == field.ReferenceColumnName).Id;
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
                                    MasterTableName = field.SourceTableName
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
                     return new FieldConfigurationServiceModel
                     {
                         Id = field.Id,
                         ReferenceColumnName = field.ReferenceColumnName,
                         ReferenceTableName = field.ReferenceTableName,
                         SourceColumnName = field.SourceColumnName,
                         SourceTableName = field.SourceTableName,
                         TableConfigId = field.TableConfigId,
                         MappedCoumns = field.FieldMappingConfigurations.
                         Where(b => b.FieldConfigurationId == field.Id).Select(n => n.MapColumnName).ToList()
                     };
                 }).ToList();

                var tableDetails = GetTableDetails(tableConfigDetails.ConnectionId ?? 0);

                var deletedColumns = tableAndFieldConfigurationDetails.fieldConfiguration
                    .Where(d => !tableDetails.Any(c => c.ColumnName.ToLower() == d.SourceColumnName.ToLower())).ToList();

                var insertedColumnsOrNewColumns = tableDetails
                    .Where(f => 
                    !tableAndFieldConfigurationDetails.fieldConfiguration
                    .Any(c => c.SourceColumnName.ToLower() == f.ColumnName.ToLower())).ToList();

                //Delete deleted coluns and add aolumns to show at from end for configure
                
                return tableAndFieldConfigurationDetails;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private List<TableDetailsServiceModel> GetTableDetails(int connectionId)
        {
            return null;
        }

    }


}
