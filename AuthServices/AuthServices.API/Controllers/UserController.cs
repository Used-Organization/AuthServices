using AuthServices.Domain.RequestModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace AuthServices.API.Controllers
{
    [Route("api/auth/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet("user-info")]
        public async Task<IActionResult> GetUserInfo()
        {
            throw new NotImplementedException();
        }

        [HttpPut("update-user")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request)
        {
            throw new NotImplementedException();
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            throw new NotImplementedException();
        }

        [HttpPost("reset-password-request")]
        public async Task<IActionResult> ResetPasswordRequest([FromBody] ResetPasswordRequest request)
        {
            throw new NotImplementedException();
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            throw new NotImplementedException();
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequest request)
        {
            throw new NotImplementedException();
        }

        [HttpPost("resend-verification-email")]
        public async Task<IActionResult> ResendVerificationEmail([FromBody] ResendVerificationEmailRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
