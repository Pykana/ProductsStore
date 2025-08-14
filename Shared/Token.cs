using BACKEND_STORE.Config;
using BACKEND_STORE.Models.DB;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime;
using System.Security.Claims;
using System.Text;

namespace BACKEND_STORE.Shared
{
    public class Token
    {
        private readonly string _JWT_SECRET_KEY;
        private readonly string _JWT_ISSUER;
        private readonly string _JWT_AUDIENCE;
        private readonly string _JWT_EXPIRATION_MINUTES;

        public Token(IConfiguration configuration)
        {
            _JWT_SECRET_KEY = configuration["STORE_JWT_SECRET_KEY"] ?? "8n@m4PPJ*r1$e^nULZtofokQpxjWlLB0";
            _JWT_ISSUER = configuration["STORE_JWT_ISSUER"] ?? "BACKEND_STORE";
            _JWT_AUDIENCE = configuration["STORE_JWT_AUDIENCE"] ?? "BACKEND_CLIENT";
            _JWT_EXPIRATION_MINUTES = configuration["STORE_JWT_EXPIRATION_MINUTES"] ?? "60";
        }
        public string GenerateToken(string userId, string username, string roleId)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.UniqueName, username),
                new Claim(ClaimTypes.Role, roleId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_JWT_SECRET_KEY));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            double expirationMinutes = double.TryParse(_JWT_EXPIRATION_MINUTES, out var exp) ? exp : 60;

            var token = new JwtSecurityToken(
                issuer: _JWT_ISSUER,
                audience: _JWT_AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
