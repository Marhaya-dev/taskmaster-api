using AutoMapper;
using TaskMaster.Domain.ViewModels.v1.Tasks;
using TaskMaster.Domain.ViewModels.v1.TaskStatus;
using TaskStatus = TaskMaster.Domain.Models.TaskStatus;

namespace TaskMaster.Domain.Mappings
{
    public class TaskStatusMappingProfile : Profile
    {
        public TaskStatusMappingProfile()
        {
            CreateMap<TaskStatus, TaskStatusResponse>();

            CreateMap<TaskStatusRequest, TaskStatus>();

            CreateMap<TaskStatusPutRequest, TaskStatus>();
        }
    }
}
