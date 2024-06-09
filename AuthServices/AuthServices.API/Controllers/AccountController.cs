using AuthServices.Application.Services;
using AuthServices.Domain.DTO;
using AuthServices.Domain.RequestModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace AuthServices.API.Controllers
{
    [Route("api/auth/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
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
                return Ok(result);
            return BadRequest(result);
        }

        // Logout endpoint
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var result = await _accountService.LogoutAsync();
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        // Confirm email endpoint
        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailDTO request)
        {
            var result = await _accountService.ConfirmRegisterEmailAsync(request.Email, request.Code);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        // Refresh token endpoint
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] string token)
        {
            var result = await _accountService.RefreshTokenAsync(token);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        // Revoke token endpoint
        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken([FromBody] string token)
        {
            var result = await _accountService.RevokeTokenAsync(token);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        // Revoke token by user ID endpoint
        [HttpPost("revoke-token-by-id")]
        public async Task<IActionResult> RevokeTokenById([FromBody] string userId)
        {
            var result = await _accountService.RevokeUserTokenByIdAsync(userId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        // Revoke token by user email endpoint
        [HttpPost("revoke-token-by-email")]
        public async Task<IActionResult> RevokeTokenByEmail([FromBody] string email)
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
        public async Task<IActionResult> ForgetPasswordEmail([FromBody] string email)
        {
            var result = await _accountService.ForgetPasswordEmail(email);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        // Forget password update endpoint
        [HttpPost("forget-password-update")]
        public async Task<IActionResult> ForgetPasswordUpdate([FromBody] ForgetPasswordDTO request)
        {
            var result = await _accountService.ForgetPasswordUpdate(request);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
