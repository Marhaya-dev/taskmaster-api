using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMaster.Domain.ViewModels.v1.TaskStatus
{
    public class TaskStatusPutRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
