using AuthServices.Domain.RequestModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthServices.API.Controllers
{
    [Route("api/auth/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        [HttpGet("get-users")]
        public async Task<IActionResult> GetAllUsers() 
        {
            throw new NotImplementedException();
        }

        [HttpGet("get-user/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("delete-user/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            throw new NotImplementedException();
        }

        [HttpGet("get-roles")]
        public async Task<IActionResult> GetAllRoles()
        {
            throw new NotImplementedException();
        }

        [HttpPost("create-role")]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("delete-role/{roleName}")]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}
