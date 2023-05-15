using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace TaskMaster.Application.Helpers
{
    public static class JwtHelper
    {
        public static string Secret = "cba616b1018cf2e08aa872eca2de8dd28fdbc155eeaaa6ca7bd5ebfb3e0e7105";

        public static SymmetricSecurityKey GetSecurityKey()
        {
            var key = Encoding.ASCII.GetBytes(Secret);

            var result = new SymmetricSecurityKey(key);

            return result;
        }

        public static string GenerateToken(IEnumerable<Claim> claims, int durationInHours)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddHours(durationInHours),
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = new SigningCredentials(
                    GetSecurityKey(),
                    SecurityAlgorithms.HmacSha256Signature
                ),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public static ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = GetSecurityKey(),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken securityToken;

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null
            || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            return principal;
        }
    }
}
