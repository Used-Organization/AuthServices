using AuthServices.Domain.Configurations;
using AuthServices.Domain.DTO;
using AuthServices.Infrastructure.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthServices.Application.Services
{
    public class TokenService: ITokenService
    {
        #region Member
        private readonly JwtSettings _jwt;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        #endregion

        #region Constructor
        public TokenService(IOptions<JwtSettings> jwt, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _jwt = jwt.Value;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }
        #endregion

        #region Methods
        public async Task<AuthDTO> CreateAuthenticationTokens(ApplicationUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = await GetUserClaims(user);
            var accessToken = CreateToken(authClaims);
            var refreshToken = GenerateRefreshToken();
            var newRefreshToken = new RefreshToken
            {
                Token = refreshToken,
                Expires = DateTime.UtcNow.AddDays(_jwt.RefreshTokenValidityInDays),
                CreatedByIp = GetIpAddress(),
                CreatedOn = DateTime.UtcNow
            };
            user.RefreshTokens.Add(newRefreshToken);
            user.RefreshTokens.RemoveAll(rt => rt.IsExpired || rt.Revoked != null);

            var updateResult = await _userManager.UpdateAsync(user);

            if (updateResult.Succeeded)
            {
                return new AuthDTO
                {
                    IsAuthenticated = true,
                    AccessToken = TokenToString(accessToken),
                    AccessTokenExpiration = accessToken.ValidTo,
                    RefreshToken = newRefreshToken.Token,
                    RefreshTokenExpiration = newRefreshToken.Expires,
                    UserId = user.Id,
                    Email = user.Email,
                    Username = user.UserName,
                    Roles = userRoles.ToList(),
                    Message = "Authorized"
                };
            }
            else
            {
                return new AuthDTO
                {
                    IsAuthenticated = false,
                    Email = user.Email,
                    UserId = user.Id,
                    Username = user.UserName,
                    Roles = userRoles.ToList(),
                    Message = updateResult.ToString()
                };
            }
        }
        public async Task<AuthDTO> CreateAccessToken(string refreshToken)
        {
            var user = await _userManager.Users
                .Include(u => u.RefreshTokens)
                .SingleOrDefaultAsync(u => u.RefreshTokens.Any(rt => rt.Token == refreshToken));

            if (user == null)
            {
                return new AuthDTO
                {
                    IsAuthenticated = false,
                    Message = "Invalid refresh token."
                };
            }

            var storedRefreshToken = user.RefreshTokens.Single(rt => rt.Token == refreshToken);

            if (!storedRefreshToken.IsActive)
            {
                return new AuthDTO
                {
                    IsAuthenticated = false,
                    Message = "Inactive or expired refresh token."
                };
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = await GetUserClaims(user);
            var accessToken = CreateToken(authClaims);

            return new AuthDTO
            {
                IsAuthenticated = true,
                AccessToken = TokenToString(accessToken),
                AccessTokenExpiration = accessToken.ValidTo,
                RefreshToken = storedRefreshToken.Token,
                RefreshTokenExpiration = storedRefreshToken.Expires,
                UserId = user.Id,
                Email = user.Email,
                Username = user.UserName,
                Roles = userRoles.ToList(),
                Message = "Authorized"
            };
        }
        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        private JwtSecurityToken CreateToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwt.TokenValidityInMinutes),
                signingCredentials: creds);
            return token;
        }
        private string TokenToString(JwtSecurityToken jwtSecurityToken) => new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        private async Task<List<Claim>> GetUserClaims(ApplicationUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("uid", user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            return claims;
        }
        private string GetIpAddress() => _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "Unknown";
        #endregion
    }
}
