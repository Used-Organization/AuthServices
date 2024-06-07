using AuthServices.Domain.DTO;
using AuthServices.Domain.Models;

namespace AuthServices.Application.Services
{
    public interface IAccountService
    {
        Task<Response> RegisterAsync(RegisterDTO registerDTO);
        Task<Response> LoginAsync(LoginDTO loginDTO);
        Task<Response> LogoutAsync();
        Task<Response> ConfirmRegisterEmailAsync(string email, string code);
        Task<Response> RevokeTokenAsync(string refreshToken);
        Task<Response> RevokeUserTokenByIdAsync(string userId);
        Task<Response> RevokeUserTokenByEmailAsync(string userString);
        Task<Response> RefreshTokenAsync(string token);
        Task<Response> ResetPassword(UpdatePasswordDTO updatePasswordDTO);
        Task<Response> ForgetPasswordEmail(string Email);
        Task<Response> ForgetPasswordUpdate(ForgetPasswordDTO forgetPasswordDTO);
    }
}
