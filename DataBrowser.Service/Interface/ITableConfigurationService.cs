using DataBrowser.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBrowser.Service.Interface
{
    public interface ITableConfigurationService
    {
        TableConfigListsServiceModel GetDatabaseConnectionName();
        List<string> GetTablesFromDatabase(TableConfigurationDatabaseFilterServiceModel dataToFilter);
        List<TableDetailsServiceModel> GetTablesDetails(IdNameServiceModel tableFilters);
        List<string> GetPrimaryKeyTableColumns(IdNameServiceModel columnFilter);
        string SaveTableConfiguraionDetails(TableAndFieldConfigurationServiceModel tableAndFieldConfiguration);
    }
}
