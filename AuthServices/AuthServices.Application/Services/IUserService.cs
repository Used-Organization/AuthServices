using AuthServices.Domain.DTO;
using AuthServices.Domain.Models;

namespace AuthServices.Application.Services
{
    public interface IUserService
    {
        Task<Response> GetUserByIdAsync(string userId);
        Task<Response> GetUserByEmailAsync(string email);
        Task<Response> UpdateUserProfileAsync(string userId, UpdateUserDTO updateUserDTO);
        Task<Response> DeleteUserAsync(string userId);
    }
}
