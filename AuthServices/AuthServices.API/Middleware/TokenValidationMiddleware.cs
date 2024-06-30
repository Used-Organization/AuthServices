using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using System.Text;
using AuthServices.Domain.Configurations;
using Microsoft.Extensions.Options;
using AuthServices.Domain.Models;
using AuthServices.Domain.DTO;

namespace AuthServices.API.Middleware
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JwtSettings _jwt;

        public TokenValidationMiddleware(RequestDelegate next, IOptions<JwtSettings> jwtSettings)
        {
            _next = next;
            _jwt = jwtSettings.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string accessToken = context.Request.Cookies[CookiesTokens.AccessTokenName];
            if (!string.IsNullOrEmpty(accessToken))
            {
                try
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var validationParameters = GetTokenValidationParameters();
                    ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(accessToken, validationParameters, out SecurityToken validatedToken);
                    context.User = claimsPrincipal;
                    //var roles = claimsPrincipal.FindAll("roles").Select(c => c.Value).ToList();
                    //context.Items["Roles"] = roles;
                }
                catch (SecurityTokenException)
                {
                    var responseMessage = new AuthenticateTokenDTO
                    {
                        Message = "Access Token is invalid or Expired",
                        IsAuthenticated = false,
                        IsInvaliedAccessToken = true,
                        IsInvalidRefreshToken = false
                    };
                    var responseJson = JsonSerializer.Serialize(responseMessage);
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(responseJson, Encoding.UTF8);
                    return;
                }
            }
            else
            {
                var responseMessage = new { Message = "Access Token is Missing!" };
                var responseJson = JsonSerializer.Serialize(responseMessage);
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(responseJson, Encoding.UTF8);
                return;
            }
            await _next(context);
        }

        private TokenValidationParameters GetTokenValidationParameters()
        {
            return new TokenValidationParameters
            {
                ValidIssuer = _jwt.Issuer,
                ValidAudience = _jwt.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };
        }
        
    }
}
