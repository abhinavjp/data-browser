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
    public class DataBrowserController : ApiController
    {

        IDataBrowserService _dataBrowserService = null;
        public DataBrowserController()
        {
            _dataBrowserService = StructureMapperConfigurator.GetInstance<IDataBrowserService>();
        }
        [HttpGet]
        public HttpResponseMessage GetTableConfigurations()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _dataBrowserService.GetTableConfigurationDetails());
        }

        [HttpPost]
        public HttpResponseMessage GetFieldsDetails(DatabrowserDropdownFilterServiceModel fieldDetailsFilterModel)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _dataBrowserService.GetDataBrowserDetails(fieldDetailsFilterModel));
        }
    }
}
