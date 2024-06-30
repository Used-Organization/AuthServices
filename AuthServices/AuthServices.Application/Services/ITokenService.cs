using AuthServices.Domain.DTO;
using AuthServices.Infrastructure.Model;

namespace AuthServices.Application.Services
{
    public interface ITokenService
    {
        public Task<AuthDTO> CreateAccessToken(string refreshToken);
        public Task<AuthDTO> CreateAuthenticationTokens(ApplicationUser user);
    }
}
