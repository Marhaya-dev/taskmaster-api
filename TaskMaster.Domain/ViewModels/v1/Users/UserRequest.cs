using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMaster.Domain.ViewModels.v1.Users
{
    public class UserRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
