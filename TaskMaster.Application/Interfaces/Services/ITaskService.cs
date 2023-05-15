using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskMaster.Domain.Filters;
using TaskMaster.Domain.ViewModels.v1.Tasks;

namespace TaskMaster.Application.Interfaces.Services
{
    public interface ITaskService
    {
        Task<TaskResponse> CreateTask(TaskRequest data);
        Task<TaskListResponse> ListTasks(TaskFilter filter);
        Task<TaskResponse> GetTaskById(int id);
        Task<TaskResponse> UpdateTask(int id, TaskPutRequest data);
        Task<bool> DeleteTask(int id);
    }
}
