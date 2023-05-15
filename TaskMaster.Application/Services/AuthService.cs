using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TaskMaster.Application.Helpers;
using TaskMaster.Application.Interfaces.Services;
using TaskMaster.Domain.Helpers;
using TaskMaster.Domain.Settings;
using TaskMaster.Domain.ViewModels.v1.Auth;
using TaskMaster.Infra.Interfaces.Repositories;

namespace TaskMaster.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtSettings _jwtSettings;

        private const string DATE_FORMAT = "dd-MM-yyyy HH:mm:ss";

        public AuthService(IUserRepository userRepository,
            IOptions<JwtSettings> jwtSettings)
        {
            _userRepository = userRepository;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<LoginResponse> Authenticate(LoginRequest credentials)
        {
            var user = await _userRepository.GetUserByEmail(credentials.Email);

            if (user == null || !PasswordHelper.VerifyHashedPassword(user.Password, credentials.Password))
                return null;

            if (_jwtSettings.ExpireHours < 1) _jwtSettings.ExpireHours = 2;

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Name),
                new Claim("user_id", user.Id.ToString())
            };

            var token = JwtHelper.GenerateToken(claims, durationInHours: _jwtSettings.ExpireHours);
            var refreshToken = JwtHelper.GenerateRefreshToken();
            DateTime createDate = DateTime.Now;
            DateTime expirationDate = createDate.AddHours(_jwtSettings.ExpireHours * 2);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiresAt = expirationDate;

            await _userRepository.UpdateRefreshToken(user);

            return new LoginResponse()
            {
                Authenticated = true,
                Created = createDate.ToString(DATE_FORMAT),
                Expiration = createDate.AddHours(_jwtSettings.ExpireHours).ToString(DATE_FORMAT),
                Token = token,
                RefreshToken = refreshToken,
                Usuario = new UserLoginResponse()
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email
                }
            };
        }

        public async Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest data)
        {
            if (_jwtSettings.ExpireHours < 1) _jwtSettings.ExpireHours = 2;

            var accessToken = data.Token;
            var refreshToken = data.RefreshToken;
            var principal = JwtHelper.GetPrincipalFromExpiredToken(accessToken);

            if (principal is null) return null;

            var userId = int.Parse(principal.FindFirstValue("user_id"));
            var user = await _userRepository.GetUserById(userId);

            if (user == null ||
                user.RefreshToken != refreshToken ||
                user.RefreshTokenExpiresAt <= DateTime.Now)
                return null;

            accessToken = JwtHelper.GenerateToken(principal.Claims, durationInHours: _jwtSettings.ExpireHours);
            refreshToken = JwtHelper.GenerateRefreshToken();

            DateTime createDate = DateTime.Now;
            DateTime expirationDate = createDate.AddHours(_jwtSettings.ExpireHours * 2);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiresAt = expirationDate;

            await _userRepository.UpdateRefreshToken(user);

            return new RefreshTokenResponse
            {
                Authenticated = true,
                Created = createDate.ToString(DATE_FORMAT),
                Expiration = createDate.AddHours(_jwtSettings.ExpireHours).ToString(DATE_FORMAT),
                Token = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<bool> RevokeToken(int id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null) return false;

            user.RefreshToken = null;

            return await _userRepository.UpdateRefreshToken(user);
        }
    }
}
