using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskMaster.Application.Interfaces.Services;
using TaskMaster.Domain.Filters;
using TaskMaster.Domain.ViewModels.v1;
using TaskMaster.Domain.ViewModels.v1.Tasks;
using TaskMaster.Infra.Interfaces.Repositories;
using Task = TaskMaster.Domain.Models.Task;

namespace TaskMaster.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly IMapper _mapper;
        private readonly ITaskRepository _taskRepositoy;

        public TaskService(IMapper mapper, ITaskRepository taskRepositoy)
        {
            _mapper = mapper;
            _taskRepositoy = taskRepositoy;
        }

        public async Task<TaskResponse> CreateTask(TaskRequest data)
        {
            data.StatusId ??= 1;

            var task = _mapper.Map<Task>(data);

            var id = await _taskRepositoy.CreateTask(task);

            task = await _taskRepositoy.GetTaskById(id);

            return _mapper.Map<TaskResponse>(task);
        }

        public async Task<TaskListResponse> ListTasks(TaskFilter filter)
        {
            var result = await _taskRepositoy.ListTask(filter);

            return new TaskListResponse()
            {
                Metadata = new ListMeta()
                {
                    Count = result.Count(),
                    TotalItems = await _taskRepositoy.GetTotalItems(filter),
                    Page = filter.GetPage(),
                    PageLimit = filter.GetPageLimit(),
                },
                Result = _mapper.Map<IEnumerable<TaskResponse>>(result),
            };
        }

        public async Task<TaskResponse> GetTaskById(int id)
        {
            var result = await _taskRepositoy.GetTaskById(id);

            return _mapper.Map<TaskResponse>(result);
        }

        public async Task<TaskResponse> UpdateTask(int id, TaskPutRequest data)
        {
            var result = _mapper.Map<Task>(data);

            await _taskRepositoy.UpdateTask(id, result);

            result = await _taskRepositoy.GetTaskById(id);

            return _mapper.Map<TaskResponse>(result);
        }

        public async Task<bool> DeleteTask(int id)
        {
            return await _taskRepositoy.DeleteTask(id);
        }
    }
}
