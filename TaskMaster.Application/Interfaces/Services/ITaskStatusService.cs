using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskMaster.Domain.Filters;
using TaskMaster.Domain.ViewModels.v1.TaskStatus;

namespace TaskMaster.Application.Interfaces.Services
{
    public interface ITaskStatusService
    {
        Task<TaskStatusResponse> CreateTaskStatus(TaskStatusRequest data);
        Task<TaskStatusListResponse> ListTaskStatus(TaskStatusFilter filter);
        Task<TaskStatusResponse> GetTaskStatusById(int id);
        Task<TaskStatusResponse> UpdateTaskStatus(int id, TaskStatusPutRequest data);
        Task<bool> DeleteTaskStatus(int id);
    }
}
