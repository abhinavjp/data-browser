using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBrowser.Service.Models;

namespace DataBrowser.Service.Services
{
    public class FieldConfigurationService
    {
        internal List<FieldsConfigurationServiceModel> GetAll(int tableConfigurationId)
        {
            var fieldConfigurations = new List<FieldsConfigurationServiceModel>();
            return fieldConfigurations.Where(w=>w.TableConfigurationId==tableConfigurationId).ToList();
        }
    }
}
