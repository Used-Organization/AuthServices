using AuthServices.Domain.DTO;
using AuthServices.Domain.Models;
using AuthServices.Infrastructure.Model;
using Microsoft.AspNetCore.Identity;

namespace AuthServices.Application.Services
{
    public class UserService : IUserService
    {
        #region Member
        private readonly UserManager<ApplicationUser> _userManager;
        #endregion
        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        #region Constructor
        public async Task<UserResponse> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new UserResponse { IsSuccess = false, Message = "User not found." };
            }

            return new UserResponse { IsSuccess = true, Message = "User found.",
                Result = new UserDTO
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Id = user.Id,
                    UserEmail = user.Email,
                    UserName = user.UserName
                }
            };
        }

        public async Task<UserResponse> GetUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new UserResponse { IsSuccess = false, Message = "User not found." };
            }

            return new UserResponse { IsSuccess = true, Message = "User found.",
                Result = new UserDTO
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Id = user.Id,
                    UserEmail = user.Email,
                    UserName = user.UserName
                }
            };
        }

        public async Task<UserResponse> UpdateUserProfileAsync(string userId, UpdateUserDTO updateUserDTO)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new UserResponse { IsSuccess = false, Message = "User not found." };
            }

            user.FirstName = updateUserDTO.firstName;
            user.LastName = updateUserDTO.lastName;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return new UserResponse
                {
                    IsSuccess = true,
                    Message = "User profile updated successfully.",
                    Result = new UserDTO
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Id = user.Id,
                        UserEmail = user.Email,
                        UserName = user.UserName
                    }
                };
            }

            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new UserResponse { IsSuccess = false, Message = $"Error updating user profile: {errors}" };
        }

        public async Task<Response> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new Response { IsSuccess = false, Message = "User not found." };
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return new Response { IsSuccess = true, Message = "User deleted successfully." };
            }

            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new Response { IsSuccess = false, Message = $"Error deleting user: {errors}" };
        }
        #endregion
    }
}
