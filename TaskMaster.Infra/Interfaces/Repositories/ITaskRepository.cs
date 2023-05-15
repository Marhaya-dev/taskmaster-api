using System.Collections.Generic;
using System.Threading.Tasks;
using TaskMaster.Domain.Filters;
using Task = TaskMaster.Domain.Models.Task;

namespace TaskMaster.Infra.Interfaces.Repositories
{
    public interface ITaskRepository
    {
        Task<int> CreateTask(Task data);
        Task<Task> GetTaskById(int id);
        Task<IEnumerable<Task>> ListTask(TaskFilter filter);
        Task<int> GetTotalItems(TaskFilter filter);
        Task<bool> UpdateTask(int id, Task data);
        Task<bool> DeleteTask(int id);
    }
}
