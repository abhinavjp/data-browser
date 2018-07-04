using DataBrowser.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBrowser.Service.Interface
{
    public interface IUserService
    {
        string CreateUserRegistration(UserServiceModel userDetail);
    }
}
