using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskMaster.Domain.ViewModels.v1.Auth;

namespace TaskMaster.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> Authenticate(LoginRequest credentials);
        Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest data);
        Task<bool> RevokeToken(int id);
    }
}
