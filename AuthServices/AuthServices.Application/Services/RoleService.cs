using AuthServices.Domain.Models;
using AuthServices.Infrastructure.Model;
using Microsoft.AspNetCore.Identity;

namespace AuthServices.Application.Services
{
    public class RoleService : IRoleService
    {
        #region Member
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        #endregion

        #region constructor
        public RoleService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        #endregion

        #region Methods
        public async Task<Response> AddAdminRoleAsync(string userId) => await AddRoleAsync( userId,UserRoles.Admin);
        public async Task<Response> AddSystemAdminRoleAsync(string userId) => await AddRoleAsync(userId, UserRoles.SystemAdmin);
        public async Task<Response> AddUserRoleAsync(string userId) => await AddRoleAsync(userId, UserRoles.User);
        public async Task<Response> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new RolesResponse { IsSuccess = false, Message = "User not found." };
            }
            var roles = await _userManager.GetRolesAsync(user);
            return new RolesResponse { IsSuccess = true, Message = "User roles retrieved successfully.", Result = roles.ToList() };
        }
        public async Task<Response> RemoveRoleFromUserAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new Response { IsSuccess = false, Message = "User not found." };
            }

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return new Response { IsSuccess = true, Message = "Role removed from user successfully." };
            }

            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new Response { IsSuccess = false, Message = $"Error removing role from user: {errors}" };
        }
        private async Task<Response> AddRoleAsync(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null || !await _roleManager.RoleExistsAsync(role))
                return new Response { IsSuccess = false, Message = "Invalid user ID or Role" };

            if (await _userManager.IsInRoleAsync(user, role))
                return new Response { IsSuccess = false, Message = "User already assigned to this role" };

            var result = await _userManager.AddToRoleAsync(user, role);

            return result.Succeeded ? new Response { IsSuccess = true } : new Response { IsSuccess = false, Message = result.ToString() };
        }
        #endregion

    }
}
