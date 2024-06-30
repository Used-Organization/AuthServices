using AuthServices.Application.Services;
using AuthServices.Domain.DTO;
using AuthServices.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthServices.API.Controllers
{
    [Route("api/auth/user")]
    [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.SystemAdmin}")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userServices;

        public UserController(IUserService userServices)
        {
            _userServices = userServices;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            var result = await _userServices.GetUserByIdAsync(userId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var result = await _userServices.GetUserByEmailAsync(email);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUserProfile(string userId, [FromBody] UpdateUserDTO updateUserDTO)
        {
            var result = await _userServices.UpdateUserProfileAsync(userId, updateUserDTO);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var result = await _userServices.DeleteUserAsync(userId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
