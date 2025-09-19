using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static BACKEND_STORE.Config.EnvironmentVariableConfig;

namespace BACKEND_STORE.Shared
{
    public static class JWT
    {
        public static string GenerateJwtToken(JWT_TokenRequest data)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, data.UserId.ToString()),
                    new Claim(ClaimTypes.Name, data.UserName),
                    new Claim(ClaimTypes.Role, data.Role.ToString()),
                    new Claim(ClaimTypes.Uri, data.URI)
                };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Variables.STORE_JWT_SECRET_KEY));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: Variables.STORE_JWT_ISSUER,
                audience: Variables.STORE_JWT_AUDIENCE,
                claims: claims,
                expires: DateTime.Now.AddDays(Variables.STORE_JWT_EXPIRATION_MINUTES), // Token expiration
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class JWT_TokenRequest
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int Role { get; set; }
        public string URI { get; set; }
    }

}
