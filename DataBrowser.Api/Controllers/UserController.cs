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
    public class UserController : ApiController
    {
        IUserService _userService = null;
        public UserController()
        {
            _userService = StructureMapperConfigurator.GetInstance<IUserService>();
        }

        [HttpPost]
        public HttpResponseMessage CreateUserRegistration(UserServiceModel userDetails)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _userService.CreateUserRegistration(userDetails));
        }
    }
}
