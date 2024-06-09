using AuthServices.Infrastructure.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using AuthServices.Domain.Models;
using AuthServices.Domain.DTO;
using Microsoft.EntityFrameworkCore;
using System.Web;

namespace AuthServices.Application.Services
{
    public class AccountService:IAccountService
    {
        #region Member
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMailService _mailingService;
        #endregion

        #region Construtor
        public AccountService(
            IHttpContextAccessor httpContextAccessor,
            ITokenService tokenService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, 
            RoleManager<IdentityRole> roleManager,
            IMailService mailService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _tokenService = tokenService;
            _mailingService = mailService;
        }
        #endregion

        #region Methods
        public async Task<Response> ConfirmRegisterEmailAsync(string email, string code)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return new Response { Message = "Email not  registered!", IsSuccess = false };
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
                return new Response { Message = result.ToString(), IsSuccess = false };
            var authDto = await _tokenService.CreateAuthenticationTokens(user);
            return new Response
            {
                Message = authDto.Message,
                IsSuccess=authDto.IsAuthenticated,
                Result=authDto
            };
        }
        public async Task<Response> LoginAsync(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "Invalid email or password."
                };
            }

            if (!user.EmailConfirmed)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "Email is not confirmed. Please confirm your email first."
                };
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);
            if (!result.Succeeded)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "Invalid email or password."
                };
            }

            var authDTO = await _tokenService.CreateAuthenticationTokens(user);

            return new Response
            {
                IsSuccess = authDTO.IsAuthenticated,
                Message = authDTO.Message,
                Result = authDTO
            };
        }
        public async Task<Response> LogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return new Response { IsSuccess = false, Message = "Logout Success !" };
        }
        public async Task<Response> RefreshTokenAsync(string token)
        {
            var authDto= await _tokenService.CreateAccessToken(token);
            return new Response
            {
                IsSuccess = authDto.IsAuthenticated,
                Result = authDto,
                Message = authDto.Message
            };
        }
        public async Task<Response> RegisterAsync(RegisterDTO registerDTO)
        {
            if (await _userManager.FindByEmailAsync(registerDTO.Email) is not null)
                return new Response { IsSuccess = false, Message = "Email is already registered!" };
            var user = new ApplicationUser
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerDTO.FirstName + registerDTO.LastName,
                Email = registerDTO.Email,
            };
            var result = await _userManager.CreateAsync(user, registerDTO.Password);
            if (result.Succeeded && await _roleManager.RoleExistsAsync(UserRoles.User))     
                await _userManager.AddToRoleAsync(user, UserRoles.User);
            if (!result.Succeeded)
                return new Response { IsSuccess = false, Message = result.ToString() };
            await SendRegisterMail(registerDTO.Email); 
            return new Response { IsSuccess = true };
        }
        public async Task<Response> RevokeTokenAsync(string refreshToken)
        {
            var user = await _userManager.Users
                .Include(u => u.RefreshTokens)
                .SingleOrDefaultAsync(u => u.RefreshTokens.Any(rt => rt.Token == refreshToken));

            return await RevokeTokensForUser(user);
        }
        public async Task<Response> RevokeUserTokenByIdAsync(string userId)
        {
            var user = await _userManager.Users
                .Include(u => u.RefreshTokens)
                .SingleOrDefaultAsync(u => u.Id == userId);

            return await RevokeTokensForUser(user);
        }
        public async Task<Response> RevokeUserTokenByEmailAsync(string email)
        {
            var user = await _userManager.Users
                .Include(u => u.RefreshTokens)
                .SingleOrDefaultAsync(u => u.Email == email);

            return await RevokeTokensForUser(user);
        }
        public async Task<Response> ForgetPasswordEmail(string Email)
        {
            var result = await _userManager.FindByEmailAsync(Email);
            if (result == null)
                return new Response { IsSuccess = false, Message = "Email is not found" };
            var token = await _userManager.GeneratePasswordResetTokenAsync(result);
            string url = $"http://localhost:3000/resetpassword?t={token}&u={Email}";
            string PasswordMail = $@"
                                    <html>
                                        <body>
                                            <h1>Attention!</h1>
                                            <p>Please keep it secret and don't share it with anyone.</p>
                                            <p>
                                                <a href=""{url}"">
                                                    <button style=""background-color: #4CAF50; color: white; padding: 10px 20px; border: none; cursor: pointer;"">
                                                        Click Here
                                                    </button>
                                                </a>
                                            </p>            
                                        </body>
                                    </html>";
            await _mailingService.SendEmailAsync(result.Email, "Reset Password Mail!", PasswordMail);
            return new Response { IsSuccess = true, Message = "Reset Password Mail Send to your Email." }; ;
        }
        public async Task<Response> ForgetPasswordUpdate(ForgetPasswordDTO forgetPasswordDTO)
        {
            var applicationUser = await _userManager.FindByEmailAsync(forgetPasswordDTO.Email);
            if (applicationUser == null)
                return new Response { IsSuccess = false, Message = "Email is not found" };
            var result = await _userManager.ResetPasswordAsync(applicationUser, forgetPasswordDTO.VerificationCode, forgetPasswordDTO.Password);
            return result.Succeeded ? new Response { IsSuccess = true } : new Response { IsSuccess = false, Message = result.ToString() };
        }
        public async Task<Response> ResetPassword(UpdatePasswordDTO updatePasswordDTO)
        {
            var applicationUser = await _userManager.FindByEmailAsync(updatePasswordDTO.Email);
            if (applicationUser == null)
                return new Response { IsSuccess = false, Message = "Email is not found" };
            var result = await _userManager.ChangePasswordAsync(applicationUser, updatePasswordDTO.OldPassword, updatePasswordDTO.NewPassword);
            return result.Succeeded ? new Response { IsSuccess = true } : new Response { IsSuccess = false, Message = result.ToString() };
        }
        #endregion

        #region HelperMethods
        private async Task<bool> SendRegisterMail(string email)
        {
            var appUser = await _userManager.FindByEmailAsync(email);
            if (appUser is null)
                return false;
            var verificationCode = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
            string baseUrl = GetBaseUrl();
            string routePrefix = "api/Auth";
            string actionRoute = "confirmEmail";

            string url = $"{baseUrl}/{routePrefix}/{actionRoute}?Email={HttpUtility.UrlEncode(appUser.Email)}&verificationCode={HttpUtility.UrlEncode(verificationCode)}";
            string WelcomeMessage = $@"
                                    <html>
                                        <body>
                                            <h1>Welcome to Virtual Queue!</h1>
                                            <p>
                                                <a href=""{url}"">
                                                    <button style=""background-color: #4CAF50; color: white; padding: 10px 20px; border: none; cursor: pointer;"">
                                                        Click Here
                                                    </button>
                                                </a>
                                            </p>
                                        </body>
                                    </html>";
            await _mailingService.SendEmailAsync(appUser.Email, "Welcome To Queue System !", WelcomeMessage);
            return true;
        }
        //TODO:base url  must be your gateway url
        public string GetBaseUrl()
        {
            var request = _httpContextAccessor.HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}{request.PathBase}";
            return baseUrl;
        }
        private async Task<Response> RevokeTokensForUser(ApplicationUser user)
        {
            if (user == null)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "User not found."
                };
            }

            var activeTokens = user.RefreshTokens.Where(rt => rt.IsActive).ToList();

            if (!activeTokens.Any())
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "No active tokens found for the user."
                };
            }

            foreach (var token in activeTokens)
            {
                token.Revoked = DateTime.UtcNow;
                token.RevokedByIp = GetIpAddress();
            }

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "Failed to revoke tokens."
                };
            }

            return new Response
            {
                IsSuccess = true,
                Message = "Tokens revoked successfully."
            };
        }
        private string GetIpAddress() => _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "Unknown";
        #endregion

    }
}
