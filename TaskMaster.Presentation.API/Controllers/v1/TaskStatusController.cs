using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskMaster.Application.Interfaces.Services;
using TaskMaster.Domain.Filters;
using TaskMaster.Domain.ViewModels.v1.TaskStatus;

namespace TaskMaster.Presentation.API.Controllers.v1
{
    [ApiController]
    [ApiVersion("1")]
    [Route("[controller]")]
    [Authorize("Bearer")]

    public class TaskStatusController : ControllerBase
    {
        [HttpPost("")]
        public async Task<ActionResult<TaskStatusResponse>> CreateTaskStatus(
            [FromBody] TaskStatusRequest data,
            [FromServices] ITaskStatusService service
        )
        {
            return Ok(await service.CreateTaskStatus(data));
        }

        [HttpGet("")]
        public async Task<ActionResult<TaskStatusListResponse>> ListTaskStatuss(
            [FromQuery] TaskStatusFilter filter,
            [FromServices] ITaskStatusService service
        )
        {
            return Ok(await service.ListTaskStatus(filter));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskStatusResponse>> GetTaskStatusById(
            int id,
            [FromServices] ITaskStatusService service
        )
        {
            var result = await service.GetTaskStatusById(id);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TaskStatusResponse>> UpdateTaskStatus(
            int id,
            [FromBody] TaskStatusPutRequest data,
            [FromServices] ITaskStatusService service
        )
        {
            var result = await service.UpdateTaskStatus(id, data);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTaskStatus(
            int id,
            [FromServices] ITaskStatusService service
        )
        {
            var result = await service.DeleteTaskStatus(id);

            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
