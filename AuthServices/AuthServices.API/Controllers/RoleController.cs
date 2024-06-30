using AuthServices.Application.Services;
using AuthServices.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthServices.API.Controllers
{
    [Route("api/auth/role")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [Authorize(Roles = UserRoles.SystemAdmin)]
        [HttpPost("assign-systemAdmin-role")]
        public async Task<IActionResult> AssignSystemAdminRole([FromBody] string userId)
        {
            var result = await _roleService.AddSystemAdminRoleAsync(userId);
            if(result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = UserRoles.SystemAdmin)]
        [HttpPost("assign-admin-role")]
        public async Task<IActionResult> AssignAdminRole([FromBody] string userId)
        {
            var result = await _roleService.AddAdminRoleAsync(userId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = UserRoles.SystemAdmin)]
        [HttpPost("remove-role")]
        public async Task<IActionResult> RemoveRole([FromBody] string userId, string role) 
        {
            var result = await _roleService.RemoveRoleFromUserAsync(userId,role);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("get-roles")]
        public async Task<IActionResult> GetUserRoles(string userId)
        {
            var result = await _roleService.GetUserRolesAsync(userId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
