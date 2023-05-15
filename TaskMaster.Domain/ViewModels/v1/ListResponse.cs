using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TaskMaster.Domain.ViewModels.v1
{
    public class ListResponse<T> where T : class
    {
        public ListMeta Metadata { get; set; }
        public IEnumerable<T> Result { get; set; }

        public ListResponse()
        {
            Result = new List<T>();
        }
    }
}
