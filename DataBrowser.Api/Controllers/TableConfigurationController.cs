using DataBrowser.Service.Configurator;
using DataBrowser.Service.Interface;
using DataBrowser.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DataBrowser.Api.Controllers
{
    public class TableConfigurationController : ApiController
    {
        ITableConfigurationService _tableConfigurationService = null;
        public TableConfigurationController()
        {
            _tableConfigurationService = StructureMapperConfigurator.GetInstance<ITableConfigurationService>();
        }

        [Authorize]
        [HttpGet]
        public HttpResponseMessage GetDatabaseConnectionName()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _tableConfigurationService.GetDatabaseConnectionName());
        }

        [HttpPost]
        public HttpResponseMessage GetTablesFromDatabase(TableConfigurationDatabaseFilterServiceModel dataToFilter)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _tableConfigurationService.GetTablesFromDatabase(dataToFilter));
        }
        [HttpPost]
        public HttpResponseMessage GetTablesDetails(IdNameServiceModel tableFilter)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _tableConfigurationService.GetTablesDetails(tableFilter));
        }

        [HttpPost]
        public HttpResponseMessage GetPrimaryKeyTableColumnsName(IdNameServiceModel columnFilter)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _tableConfigurationService.GetPrimaryKeyTableColumns(columnFilter));
        }
        [HttpPost]
        public HttpResponseMessage SaveFieldConfiguration(TableAndFieldConfigurationServiceModel tableAndFieldConfiguration)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _tableConfigurationService.SaveTableConfiguraionDetails(tableAndFieldConfiguration));
        }

        [HttpPost]
        public HttpResponseMessage GetDetailstableAndFieldsById(IdNameServiceModel ids)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _tableConfigurationService.GetDetailstableAndFieldsById(ids.Id));
        }
        [HttpPost]
        public HttpResponseMessage UpdateTableAndFieldMappingConfiguration(TableAndFieldConfigurationServiceModel tableAndFieldConfiguration)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _tableConfigurationService.UpdateTableAndfieldconfiguration(tableAndFieldConfiguration));
        }

    }

}
