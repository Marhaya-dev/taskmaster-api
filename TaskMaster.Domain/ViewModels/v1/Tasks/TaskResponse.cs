using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMaster.Domain.ViewModels.v1.Tasks
{
    public class TaskResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? UserId { get; set; }
        public int? StatusId { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
