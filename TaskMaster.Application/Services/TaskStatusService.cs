using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskMaster.Application.Interfaces.Services;
using TaskMaster.Domain.Filters;
using TaskMaster.Domain.ViewModels.v1;
using TaskMaster.Domain.ViewModels.v1.TaskStatus;
using TaskMaster.Infra.Interfaces.Repositories;
using TaskStatus = TaskMaster.Domain.Models.TaskStatus;

namespace TaskMaster.Application.Services
{
    public class TaskStatusService : ITaskStatusService
    {
        private readonly IMapper _mapper;
        private readonly ITaskStatusRepository _taskStatusRepositoy;

        public TaskStatusService(IMapper mapper, ITaskStatusRepository taskStatusRepositoy)
        {
            _mapper = mapper;
            _taskStatusRepositoy = taskStatusRepositoy;
        }

        public async Task<TaskStatusResponse> CreateTaskStatus(TaskStatusRequest data)
        {
            var taskStatus = _mapper.Map<TaskStatus>(data);

            var id = await _taskStatusRepositoy.CreateTaskStatus(taskStatus);

            taskStatus = await _taskStatusRepositoy.GetTaskStatusById(id);

            return _mapper.Map<TaskStatusResponse>(taskStatus);
        }

        public async Task<TaskStatusListResponse> ListTaskStatus(TaskStatusFilter filter)
        {
            var result = await _taskStatusRepositoy.ListTaskStatus(filter);

            return new TaskStatusListResponse()
            {
                Metadata = new ListMeta()
                {
                    Count = result.Count(),
                    TotalItems = await _taskStatusRepositoy.GetTotalItems(filter),
                    Page = filter.GetPage(),
                    PageLimit = filter.GetPageLimit(),
                },
                Result = _mapper.Map<IEnumerable<TaskStatusResponse>>(result),
            };
        }

        public async Task<TaskStatusResponse> GetTaskStatusById(int id)
        {
            var result = await _taskStatusRepositoy.GetTaskStatusById(id);

            return _mapper.Map<TaskStatusResponse>(result);
        }

        public async Task<TaskStatusResponse> UpdateTaskStatus(int id, TaskStatusPutRequest data)
        {
            var result = _mapper.Map<TaskStatus>(data);

            await _taskStatusRepositoy.UpdateTaskStatus(id, result);

            result = await _taskStatusRepositoy.GetTaskStatusById(id);

            return _mapper.Map<TaskStatusResponse>(result);
        }

        public async Task<bool> DeleteTaskStatus(int id)
        {
            return await _taskStatusRepositoy.DeleteTaskStatus(id);
        }
    }
}
