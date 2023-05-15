using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMaster.Domain.ViewModels.v1
{
    public class ValidationResultException : Exception
    {
        public ValidationResultException(string message) : base(message) { }
    }
}
