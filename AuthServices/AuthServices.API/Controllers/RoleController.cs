using AuthServices.Domain.RequestModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthServices.API.Controllers
{
    [Route("api/auth/role")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleRequest request)
        {
            throw new NotImplementedException();
        }

        [HttpPost("remove-role")]
        public async Task<IActionResult> RemoveRole([FromBody] RemoveRoleRequest request) 
        {
            throw new NotImplementedException();
        }

        [HttpGet("get-roles")]
        public async Task<IActionResult> GetUserRoles()
        {
            throw new NotImplementedException();
        }
    }
}
