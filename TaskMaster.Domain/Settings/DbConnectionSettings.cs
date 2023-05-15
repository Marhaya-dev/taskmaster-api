using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMaster.Domain.Settings
{
    public class DbConnectionSettings
    {
        public string Server { get; set; }
        public string Database { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }
}
