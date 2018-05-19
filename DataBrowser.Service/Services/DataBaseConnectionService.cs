using AutoMapper;
using DataBrowser.Data;
using DataBrowser.Data.Repository;
using DataBrowser.Service.Interface;
using DataBrowser.Service.Models;
using HelperFoundation.ErrorHandler;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using static HelperFoundation.ErrorHandler.ProcessResultHelper;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBrowser.Service.Services
{
    public class DataBaseConnectionService : IDataBaseConnectionService
    {
        public ProcessResult<List<DataBaseConnectionServiceModel>> GetAll()
        {
            try
            {
                List<DataBaseConnectionServiceModel> dataBaseConnection = null;
                using (var repo = new RepositoryPattern<DatabaseConnection>())
                {
                    var result = repo.SelectAll().ToList();
                    dataBaseConnection = Mapper.Map<List<DataBaseConnectionServiceModel>>(result);
                }
                return dataBaseConnection.GetResult();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public List<string> GetDataDataBaseLists(DataBaseNameFilterServiceModel databaseFilterServiceModel)
        {
            try
            {
                if (databaseFilterServiceModel == null)
                {
                    
                }

                string connectionString = string.Empty;
                List<string> dataBaseName = new List<string>();
                if (!string.IsNullOrEmpty(databaseFilterServiceModel.UserName) || !string.IsNullOrEmpty(databaseFilterServiceModel.Password))
                    connectionString = "server= " + databaseFilterServiceModel.ServerInstanceName + " ;uid=" + databaseFilterServiceModel.UserName + ";pwd=" + databaseFilterServiceModel.Password + ";";
                else
                    connectionString = "Data Source=" + databaseFilterServiceModel.ServerInstanceName + "; Integrated Security=True;";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT name from sys.databases", conn))
                    {
                        using (IDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                                dataBaseName.Add(dr[0].ToString());
                        }
                    }
                }
                return dataBaseName;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public string CreateDataBaseConnection(DataBaseConnectionServiceModel dataBaseConnection)
        {
            try
            {
                if (dataBaseConnection == null)
                {

                }
                using (var dataBaseRepo = new RepositoryPattern<DatabaseConnection>())
                {
                    var data = Mapper.Map<DatabaseConnection>(dataBaseConnection);
                    dataBaseRepo.Insert(data);
                    dataBaseRepo.Save();
                }
                return "";
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public string DeleteDatabaseConnection(int id)
        {
            try
            {
                if (id == default(int))
                {
                    // retirn exception
                }
                using (var dataBaseRepo = new RepositoryPattern<DatabaseConnection>())
                {
                    dataBaseRepo.Delete(id);
                    dataBaseRepo.Save();
                }
                return "Database Connection Deleted successfully";
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}