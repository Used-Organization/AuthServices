using AuthServices.Domain.DTO;
using AuthServices.Domain.Models;

namespace AuthServices.Application.Services
{
    public interface IUserService
    {
        Task<UserResponse> GetUserByIdAsync(string userId);
        Task<UserResponse> GetUserByEmailAsync(string email);
        Task<UserResponse> UpdateUserProfileAsync(string userId, UpdateUserDTO updateUserDTO);
        Task<Response> DeleteUserAsync(string userId);
    }
}
