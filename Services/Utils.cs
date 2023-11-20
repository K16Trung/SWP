using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services
{
    public static class Utils
    {
        public static string Hash(this string input, string salt)
        {
            return BCrypt.Net.BCrypt.HashPassword(input, salt);
        }
        public static bool Verify(this string input, string stringVerify)
        {
            return BCrypt.Net.BCrypt.Verify(input, stringVerify);
        }
        public static string Satl()
        {
            return BCrypt.Net.BCrypt.GenerateSalt();
        }
        public static string GenerateJwtToken(this string id, string email, string role, JWTSection jwt)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.SecretKey));
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
                (
                    claims: new[] { new Claim("Id", id),
                                    new Claim("email", email),
                                    new Claim("role", role)
                                  },
                    expires: DateTime.Now.AddDays(Convert.ToDouble(jwt.ExpiresInDays)),
                    signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
