using AutoMapper;
using TaskMaster.Domain.Models;
using TaskMaster.Domain.ViewModels.v1.Tasks;
using TaskMaster.Domain.ViewModels.v1.Users;
using Task = TaskMaster.Domain.Models.Task;

namespace TaskMaster.Domain.Mappings
{
    public class TaskMappingProfile : Profile
    {
        public TaskMappingProfile()
        {
            CreateMap<Task, TaskResponse>();

            CreateMap<TaskRequest, Task>();

            CreateMap<TaskPutRequest, Task>();
        }
    }
}
