using System;
using System.Text;
using System.Collections.Generic;

namespace TaskMaster.Domain.ViewModels.v1
{
    public class ListMeta
    {
        public int Page { get; set; }
        public int PageLimit { get; set; }
        public int Count { get; set; }
        public int TotalItems { get; set; }
    }
}
