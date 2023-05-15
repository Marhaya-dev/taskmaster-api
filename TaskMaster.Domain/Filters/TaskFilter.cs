using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMaster.Domain.Filters
{
    public class TaskFilter : ListFilter
    {
        public int? UserId { get; set; }
        public int? StatusId { get; set; }
        public string Search { get; set; }
        public DateTime? CreatedAfter { get; set; }
        public DateTime? UpdatedAfter { get; set; }
    }
}
