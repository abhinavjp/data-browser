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
    public class AppConfigConnectionFactory: IConnectionFactory
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
                if (connectionStringOrConnectionName == null) throw new ArgumentNullException("connectionName");
                var conStr = ConfigurationManager.ConnectionStrings[connectionStringOrConnectionName];
                if (conStr == null)
                    throw new ConfigurationErrorsException(string.Format("Failed to find connection string named '{0}' in app/web.config.", connectionStringOrConnectionName));
                _name = conStr.ProviderName;
                _provider = DbProviderFactories.GetFactory(conStr.ProviderName);
                _connectionString = conStr.ConnectionString;
            }
        }
        public AppConfigConnectionFactory(string connectionName): this(connectionName, false)
        {
            
        }
        public IDbConnection Create()
        {
            DbConnection connection;
            if(_provider != null)
            {
                connection = _provider.CreateConnection();
            }
            else
            {
                connection = new SqlConnection();
            }
            if (_provider != null && connection == null)
                throw new ConfigurationErrorsException($"Failed to create a connection using the connection string named '{_name}' in app/web.config.");
            connection.ConnectionString = _connectionString;
            connection.Open();
            return connection;
        }
    }
}
