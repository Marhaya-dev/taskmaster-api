using System.Collections.Generic;
using System.Threading.Tasks;
using TaskMaster.Domain.Filters;
using TaskStatus = TaskMaster.Domain.Models.TaskStatus;

namespace TaskMaster.Infra.Interfaces.Repositories
{
    public interface ITaskStatusRepository
    {
        Task<int> CreateTaskStatus(TaskStatus data);
        Task<TaskStatus> GetTaskStatusById(int id);
        Task<IEnumerable<TaskStatus>> ListTaskStatus(TaskStatusFilter filter);
        Task<int> GetTotalItems(TaskStatusFilter filter);
        Task<bool> UpdateTaskStatus(int id, TaskStatus data);
        Task<bool> DeleteTaskStatus(int id);
    }
}
