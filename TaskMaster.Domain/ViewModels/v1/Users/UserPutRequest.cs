using System;
using System.Collections.Generic;

namespace TaskMaster.Domain.ViewModels.v1.Users
{
    public class UserPutRequest
    {
        public string Email { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string Name { get; set; }
    }
}
