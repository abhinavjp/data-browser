using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBrowser.Helper.AdoHelper
{
    public class AppConfigConnectionFactory : IConnectionFactory
    {
        private readonly DbProviderFactory _provider;
        private readonly string _connectionString;
        private readonly string _name;

        public AppConfigConnectionFactory(string connectionStringOrConnectionName, bool isConnectionString)
        {
            if (isConnectionString)
            {
                _connectionString = connectionStringOrConnectionName;
            }
            else
            {
                if (connectionStringOrConnectionName == null) throw new ArgumentNullException(nameof(connectionStringOrConnectionName), "Connection String or Name cannot be null");
                var conStr = ConfigurationManager.ConnectionStrings[connectionStringOrConnectionName];
                if (conStr == null)
                    throw new ConfigurationErrorsException($"Failed to find connection string named '{connectionStringOrConnectionName}' in app/web.config.");
                _name = conStr.ProviderName;
                _provider = DbProviderFactories.GetFactory(conStr.ProviderName);
                _connectionString = conStr.ConnectionString;
            }
        }
        public AppConfigConnectionFactory(string connectionString, DbProviderFactory provider) : this(connectionString, true)
        {
            _provider = provider;
        }

        public AppConfigConnectionFactory(string connectionName) : this(connectionName, false)
        {

        }
        public IDbConnection Create()
        {
            if (_provider == null)
            {
                throw new ConfigurationErrorsException("No provider configured");
            }
            DbConnection connection;
            connection = _provider.CreateConnection();
            if (connection == null)
                if (string.IsNullOrWhiteSpace(_name))
                    throw new ConfigurationErrorsException($"Failed to create a connection using the provided connection string");
                else
                    throw new ConfigurationErrorsException($"Failed to create a connection using the connection string named '{_name}' in app/web.config.");
            connection.ConnectionString = _connectionString;
            connection.Open();
            return connection;
        }
    }
}
