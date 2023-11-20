using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Services;

namespace Project.SWP.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JWTSection _jwtSection;

        public JwtMiddleware(RequestDelegate next, JWTSection jwtSection)
        {
            _next = next;
            _jwtSection = jwtSection;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/User/Login") || context.Request.Path.StartsWithSegments("/User/Register"))
            {
                await _next(context);
                return;
            }

            if (!context.Request.Headers.ContainsKey("Authorization"))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Please include Authorization key in header & value is JWT key");
                return;
            }

            try
            {
                var authHeader = context.Request.Headers["Authorization"].ToString().Split(" ");
                var claimsPrincipal = GetClaimPrincipal(authHeader[1]);
                context.User = claimsPrincipal;
            }
            catch (Exception)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Token invalid");
                return;
            }
            await _next(context);
        }

        private ClaimsPrincipal GetClaimPrincipal(string jwtToken)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSection.SecretKey));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false, 
                ValidateAudience = false, 
                ValidateLifetime = true, 
                IssuerSigningKey = securityKey
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.ValidateToken(jwtToken, tokenValidationParameters, out _);
        }
    }

}
