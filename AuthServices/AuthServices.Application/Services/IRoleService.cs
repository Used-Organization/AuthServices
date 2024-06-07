using AuthServices.Domain.Models;

namespace AuthServices.Application.Services
{
    public interface IRoleService
    {
        Task<Response> AddSystemAdminRoleAsync(string userId);
        Task<Response> AddAdminRoleAsync(string userId);
        Task<Response> AddUserRoleAsync(string userId);
        Task<Response> RemoveRoleFromUserAsync(string userId, string roleName);
    }
}
