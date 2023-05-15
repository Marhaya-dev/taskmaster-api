using System;
using System.Text;
using System.Collections.Generic;

namespace TaskMaster.Domain.Filters
{
    public class UserFilter : ListFilter
    {
        public string Search { get; set; }
        public DateTime? CreatedAfter { get; set; }
        public DateTime? UpdatedAfter { get; set; }
    }
}
