using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskMaster.Application.Interfaces.Services;
using TaskMaster.Domain.Filters;
using TaskMaster.Domain.ViewModels.v1.Users;

namespace TaskMaster.Presentation.API.Controllers.v1
{
    [ApiController]
    [ApiVersion("1")]
    [Route("[controller]")]
    [Authorize("Bearer")]

    public class UsersController : ControllerBase
    {
        [HttpPost("")]
        public async Task<ActionResult<UserResponse>> CreateUser(
            [FromBody] UserRequest data,
            [FromServices] IUserService service
        )
        {     
            return Ok(await service.CreateUser(data));
        }

        [HttpGet("")]
        public async Task<ActionResult<UserListResponse>> ListUsers(
            [FromQuery] UserFilter filter,
            [FromServices] IUserService service
        )
        {
            return Ok(await service.ListUsers(filter));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponse>> GetUserById(
            int id,
            [FromServices] IUserService service
        )
        {
            var result = await service.GetUserById(id);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserResponse>> UpdateUser(
            int id,
            [FromBody] UserPutRequest data,
            [FromServices] IUserService service
        )
        {
            var result = await service.UpdateUser(id, data);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(
            int id,
            [FromServices] IUserService service
        )
        {
            var result = await service.DeleteUser(id);

            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
