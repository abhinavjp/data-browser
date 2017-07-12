using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBrowser.Helper.AdoHelper
{
    public class DataAccessAdapter
    {
        #region Initialization
        private readonly string _dataSource;
        private readonly string _password;
        private DataAccessAdapter(string dataSource, string password)
        {
            _dataSource = dataSource;
            _password = password;
        }

        public static DataAccessAdapter Initialize(string dataSource, string password)
        {
            return new DataAccessAdapter(dataSource, password);
        }
        #endregion

        public bool DoEntity<T>(T entity, IDbCommand dbCommand)
        {
            throw new NotImplementedException();
        }

        public static string GenerateCommand<T>(IDbCommand dbCommand, T entity, string whereClause = "")
        {
            throw new NotImplementedException();
        }
    }
}
