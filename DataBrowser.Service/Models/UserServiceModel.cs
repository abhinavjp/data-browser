using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBrowser.Service.Models
{
    public class UserServiceModel
    {
        public int UserId { get; set; } // UserId (Primary key)
        public string Name { get; set; } // Name (length: 50)
        public string UserName { get; set; } // UserName (length: 50)
        public string Email { get; set; } // Email (length: 50)
        public string Password { get; set; } // Password (length: 50)
        public System.DateTime? CreatedOn { get; set; } // CreatedOn

    }
}
