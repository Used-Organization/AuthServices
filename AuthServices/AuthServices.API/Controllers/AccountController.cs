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
        // Register endpoint
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            throw new NotImplementedException();
        }

        // Login endpoint
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            throw new NotImplementedException();
        }

        // Logout endpoint
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            throw new NotImplementedException();
        }

        // Refresh token endpoint
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            throw new NotImplementedException();
        }

        // Revoke token endpoint
        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
