using DataBrowser.Service.Models;
using RepositoryFoundation.Helper.Mapper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBrowser.Service.Services
{
    public class ConfigurationService
    {
        private const string SqlDataProvider = "System.Data.SqlClient";

        private List<ConnectionConfigurationViewServiceModel> GetAllConnectionConfiguration()
        {
            return GetConnectionConfiguration<ConnectionConfigurationViewServiceModel>(null);
        }

        private ConnectionConfigurationServiceModel GetConnectionConfigurationById(int id)
        {
            return GetConnectionConfiguration(c => c.Id == id).FirstOrDefault();
        }

        private static bool TestConnectionString(string connectionString, string dataProvider)
        {
            try
            {
                var factory = DbProviderFactories.GetFactory(dataProvider);
                using (DbConnection conn = factory.CreateConnection())
                {
                    conn.ConnectionString = connectionString;
                    conn.Open();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static List<TMapper> GetConnectionConfiguration<TMapper>()
        {
            return GetConnectionConfiguration<TMapper>(null);
        }

        private static List<TMapper> GetConnectionConfiguration<TMapper>(Func<ConnectionConfigurationServiceModel, bool> configurationSelector)
        {
            return GetConnectionConfiguration(configurationSelector).MapTo<TMapper>();
        }

        private static List<ConnectionConfigurationServiceModel> GetConnectionConfiguration()
        {
            return GetConnectionConfiguration(null);
        }

        private static List<ConnectionConfigurationServiceModel> GetConnectionConfiguration(Func<ConnectionConfigurationServiceModel, bool> configurationSelector)
        {
            var connectionConfigurations = new List<ConnectionConfigurationServiceModel>();
            if (configurationSelector == null)
            {
                return connectionConfigurations;
            }
            return connectionConfigurations.Where(configurationSelector).ToList();
        }
    }
}
