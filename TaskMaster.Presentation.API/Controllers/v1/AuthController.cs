using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskMaster.Application.Interfaces.Services;
using TaskMaster.Domain.ViewModels.v1.Auth;

namespace TaskMaster.Presentation.API.Controllers.v1
{
    [ApiController]
    [ApiVersion("1")]
    [Route("[controller]")]

    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> GenerateToken([FromBody] LoginRequest data,
            [FromServices] IAuthService service)
        {
            var result = await service.Authenticate(data);

            if (result is null) return Unauthorized("Usuário ou senha inválidos.");

            return Ok(result);
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<RefreshTokenResponse>> RefreshToken([FromBody] RefreshTokenRequest data,
            [FromServices] IAuthService service)
        {
            var result = await service.RefreshToken(data);

            if (result is null) return BadRequest("Token expirado");

            return Ok(result);
        }

        [HttpPost("revoke/{id}")]
        [Authorize("Bearer")]
        public async Task<ActionResult> Revoke(int id, [FromServices] IAuthService service)
        {
            var result = await service.RevokeToken(id);

            if (!result) return BadRequest("Requisição de usuário inválida");

            return NoContent();
        }
    }
}
