using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMaster.Domain.Models
{
    public class TaskStatus : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
