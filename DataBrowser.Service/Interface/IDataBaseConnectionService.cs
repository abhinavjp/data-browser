using DataBrowser.Service.Models;
using HelperFoundation.ErrorHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBrowser.Service.Interface
{
    public interface IDataBaseConnectionService
    {
        ProcessResult<List<DataBaseConnectionServiceModel>> GetAll();
        List<string> GetDataDataBaseLists(DataBaseNameFilterServiceModel databaseFilterServiceModel);
        string CreateDataBaseConnection(DataBaseConnectionServiceModel dataBaseConnection);
        string DeleteDatabaseConnection(int id);
    }
}
