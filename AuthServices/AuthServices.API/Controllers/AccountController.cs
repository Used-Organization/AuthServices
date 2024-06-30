using AuthServices.API.Helpers;
using AuthServices.Application.Services;
using AuthServices.Domain.DTO;
using Microsoft.AspNetCore.Mvc;

namespace AuthServices.API.Controllers
{
    [Route("api/auth/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly CookiesHelper _cookiesHelper;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
            _cookiesHelper = new CookiesHelper();
        }

        // Register endpoint
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO request)
        {
            var result = await _accountService.RegisterAsync(request);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        // Login endpoint
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO request)
        {
            var result = await _accountService.LoginAsync(request);
            if (result.IsSuccess)
            {
                var authDTO = (AuthDTO)result.Result;
                if (authDTO != null && authDTO is AuthDTO)
                {
                    _cookiesHelper.SetAccessToken(Response, authDTO.AccessToken, authDTO.AccessTokenExpiration);
                    _cookiesHelper.SetRefreshTokenInCookie(Response, authDTO.RefreshToken, authDTO.RefreshTokenExpiration);
                }
                return Ok(result);
            }
            return BadRequest(result);
        }

        // Confirm email endpoint
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string Email, string verificationCode)
        {
            var result = await _accountService.ConfirmRegisterEmailAsync(Email, verificationCode);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        // Refresh token endpoint
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(string token)
        {
            var result = await _accountService.RefreshTokenAsync(token);
            if (result.IsSuccess)
            {
                var authDTO = (AuthDTO)result.Result;
                if (authDTO != null && authDTO is AuthDTO)
                {
                    _cookiesHelper.SetAccessToken(Response, authDTO.AccessToken, authDTO.AccessTokenExpiration);
                    _cookiesHelper.SetRefreshTokenInCookie(Response, authDTO.RefreshToken, authDTO.RefreshTokenExpiration);
                }
                return Ok(result);
            }
            return BadRequest(result);
        }

        // Revoke token endpoint
        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken(string token)
        {
            var result = await _accountService.RevokeTokenAsync(token);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        // Revoke token by user ID endpoint
        [HttpPost("revoke-token-by-id")]
        public async Task<IActionResult> RevokeTokenById(string userId)
        {
            var result = await _accountService.RevokeUserTokenByIdAsync(userId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        // Revoke token by user email endpoint
        [HttpPost("revoke-token-by-email")]
        public async Task<IActionResult> RevokeTokenByEmail(string email)
        {
            var result = await _accountService.RevokeUserTokenByEmailAsync(email);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        // Reset password endpoint
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] UpdatePasswordDTO request)
        {
            var result = await _accountService.ResetPassword(request);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        // Forget password email endpoint
        [HttpPost("forget-password-email")]
        public async Task<IActionResult> ForgetPasswordEmail(string email)
        {
            var result = await _accountService.ForgetPasswordEmail(email);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        // Forget password update endpoint
        [HttpPost("forget-password-update")]
        public async Task<IActionResult> ForgetPasswordUpdate(string Email, string Password, string VerificationCode)
        {
            ForgetPasswordDTO request= new ForgetPasswordDTO(Email, Password, VerificationCode);
            var result = await _accountService.ForgetPasswordUpdate(request);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
