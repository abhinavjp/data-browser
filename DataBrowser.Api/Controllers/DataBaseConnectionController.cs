using DataBrowser.Service.Configurator;
using DataBrowser.Service.Interface;
using DataBrowser.Service.Models;
using DataBrowser.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DataBrowser.Api.Controllers
{
    public class DataBaseConnectionController : ApiController
    {
        IDataBaseConnectionService _dataBaseConnectionService = null;
        public DataBaseConnectionController()
        {
            _dataBaseConnectionService = StructureMapperConfigurator.GetInstance<IDataBaseConnectionService>();
        }
        [Authorize]
        [HttpGet]
        public HttpResponseMessage GetDataBaseConnection()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _dataBaseConnectionService.GetAll());
        }
        [HttpPost]
        public HttpResponseMessage GetDatabaseName(DataBaseNameFilterServiceModel databaseFilterServiceModel)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _dataBaseConnectionService.GetDataDataBaseLists(databaseFilterServiceModel));
        }

        [HttpPost]
        public HttpResponseMessage CreateDataBaseConnection(DataBaseConnectionServiceModel dataBaseConnection)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _dataBaseConnectionService.CreateDataBaseConnection(dataBaseConnection));
        }

        [HttpDelete]
        public HttpResponseMessage DeleteDatabaseConnection(int id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _dataBaseConnectionService.DeleteDatabaseConnection(id));
        }
    }
}
