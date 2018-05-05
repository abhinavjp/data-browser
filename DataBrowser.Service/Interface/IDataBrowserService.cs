using DataBrowser.Data;
using DataBrowser.Service.Models;
using DataBrowser.Service.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBrowser.Service.Interface
{
    public interface IDataBrowserService
    {
        List<DatabrowserDropdownFilterServiceModel> GetTableConfigurationDetails();
        DataSet GetDataBrowserDetails(DatabrowserDropdownFilterServiceModel fieldDetailsFilterModel);
    }
}
