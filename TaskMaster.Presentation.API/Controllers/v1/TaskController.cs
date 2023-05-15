using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskMaster.Application.Interfaces.Services;
using TaskMaster.Domain.Filters;
using TaskMaster.Domain.ViewModels.v1.Tasks;

namespace TaskMaster.Presentation.API.Controllers.v1
{
    [ApiController]
    [ApiVersion("1")]
    [Route("[controller]")]
    [Authorize("Bearer")]

    public class TaskController : ControllerBase
    {
        [HttpPost("")]
        public async Task<ActionResult<TaskResponse>> CreateTask(
            [FromBody] TaskRequest data,
            [FromServices] ITaskService service
        )
        {
            return Ok(await service.CreateTask(data));
        }

        [HttpGet("")]
        public async Task<ActionResult<TaskListResponse>> ListTasks(
            [FromQuery] TaskFilter filter,
            [FromServices] ITaskService service
        )
        {
            return Ok(await service.ListTasks(filter));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskResponse>> GetTaskById(
            int id,
            [FromServices] ITaskService service
        )
        {
            var result = await service.GetTaskById(id);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TaskResponse>> UpdateTask(
            int id,
            [FromBody] TaskPutRequest data,
            [FromServices] ITaskService service
        )
        {
            var result = await service.UpdateTask(id, data);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTask(
            int id,
            [FromServices] ITaskService service
        )
        {
            var result = await service.DeleteTask(id);

            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
